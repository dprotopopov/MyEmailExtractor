using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using HtmlAgilityPack;

namespace MyEmEx
{
    public partial class ChildForm : Form
    {
        private class ThreadProcArg
        {
            public Semaphore Semaphore0;
            public Semaphore Semaphore1;
            public Semaphore Semaphore2;
            public ChildForm This;
            public int Index;
            public int MaxLevel;
        }

        private Semaphore _semaphore0;
        private Semaphore _semaphore1 = new Semaphore(1, 1);
        private Semaphore _semaphore2 = new Semaphore(1, 1);
        private int _index;
        private bool _isRunning;

        private int _maxLevel;
        private int _navigatingCountdown = 3;

        delegate void GetUrlCallback(int index, ref String url, ref int level);
        delegate void AddUrlCallback(String url, int level);
        delegate void AddEmailCallback(String email);

        public void GetUrl(int index, ref String url, ref int level)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (listView2.InvokeRequired)
            {
                GetUrlCallback d = new GetUrlCallback(GetUrl);
                var arr = new object[] { index, url, level };
                this.Invoke(d, arr);
                url = (String)arr[1];
                level = (int)arr[2];
            }
            else
            {
                ListViewItem lvi = listView2.Items[index];
                url = lvi.Text;
                level = Convert.ToInt32(lvi.SubItems[1].Text);
                lvi.ImageIndex = 1;
            }

        }
        public void AddUrl(String url, int level)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (listView2.InvokeRequired)
            {
                AddUrlCallback d = new AddUrlCallback(AddUrl);
                Invoke(d, new object[] { url, level });
            }
            else
            {
                if (listView2.Items[url.ToLower()] == null)
                {
                    ListViewItem lvi = new ListViewItem(url);
                    lvi.ImageIndex = 0;
                    lvi.Name = url.ToLower();
                    lvi.SubItems.Add(Convert.ToString(level));
                    listView2.Items.Add(lvi);
                }
            }
        }
        public void AddEmail(String email)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (listView1.InvokeRequired)
            {
                AddEmailCallback d = new AddEmailCallback(AddEmail);
                this.Invoke(d, new object[] { email });
            }
            else
            {
                if (listView1.Items[email.ToLower()] == null)
                {
                    ListViewItem lvi = new ListViewItem(email) {ImageIndex = 0, Name = email.ToLower()};
                    this.listView1.Items.Add(lvi);
                }
            }
        }

        public static void ThreadProc(object obj)
        {
            Debug.Print("Start ThreadProc");
            ThreadProcArg arg = obj as ThreadProcArg;


            String url = @"";
            int level = 0;
            Debug.Assert(arg != null, "arg != null");
            if (arg != null)
            {
                arg.Semaphore2.WaitOne();
                arg.This.GetUrl(arg.Index, ref url, ref level);
                arg.Semaphore2.Release();
          
                try
                {
                    HtmlWeb hw = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = hw.Load(url);

                    if (level != arg.MaxLevel)
                    {
                        Uri baseUri = new Uri(url);

                        foreach (HtmlAgilityPack.HtmlNode link in doc.DocumentNode.SelectNodes(@".//a[@href]"))
                        {
                            HtmlAgilityPack.HtmlAttribute att = link.Attributes["href"];
                            Uri uri = new Uri(baseUri, att.Value);
                            String url2 = uri.AbsoluteUri;
                            arg.Semaphore2.WaitOne();
                            arg.This.AddUrl(url2, level + 1);
                            arg.Semaphore2.Release();
                        }
                    }

                    String html = doc.DocumentNode.InnerHtml;
                    const string patternEmail = @"\b[a-zAZ0-9]+([\.-][a-zAZ0-9]+)*@([a-zAZ0-9-]+\.)+[a-zA-Z]{2,6}\b";
                    Regex rgxEmail = new Regex(patternEmail, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    foreach (Match matchEmail in rgxEmail.Matches(html))
                    {
                        arg.Semaphore1.WaitOne();
                        arg.This.AddEmail(matchEmail.Value);
                        arg.Semaphore1.Release();
                    }
                }
                catch
                {
                }

                arg.Semaphore0.Release();
            }
            Debug.Print("End ThreadProc");
        }

        public ChildForm(int maxLevel, int maxThreads)
        {
            _isRunning = false;
            _index = 0;
            InitializeComponent();
            _maxLevel = maxLevel;
            _semaphore0 = new Semaphore(maxThreads, maxThreads);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_navigatingCountdown == 0)
            {
                e.Cancel = true;
                String url = e.Url.ToString();
                Process.Start(url);
            }
        }

        private void childForm_Load(object sender, EventArgs e)
        {
            listView1.SuspendLayout();
            listView2.SuspendLayout();
        }

        public void StartWorker()
        {
            if (!_isRunning)
            {
                backgroundWorker1.RunWorkerAsync();
                _isRunning = true;
            }
        }

        public void StopWorker()
        {
            if (_isRunning && backgroundWorker1.WorkerSupportsCancellation)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                _isRunning = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            BackgroundWorker worker = sender as BackgroundWorker;

            Debug.Assert(worker != null, "worker != null");
            while (worker != null && worker.CancellationPending != true)
            {
                if (_index < listView2.Items.Count)
                {
                    _semaphore0.WaitOne();

                    ThreadProcArg arg = new ThreadProcArg();
                    arg.Semaphore0 = _semaphore0;
                    arg.Semaphore1 = _semaphore1;
                    arg.Semaphore2 = _semaphore2;
                    arg.This = this;
                    arg.Index = _index;
                    arg.MaxLevel = _maxLevel;

                    Thread t = new Thread(new ParameterizedThreadStart(ThreadProc));
                    t.Start(arg);
                    _index++;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isRunning = false;
        }

        public void SaveAs(String fileName)
        {
            StreamWriter outfile = new StreamWriter(fileName);
            _semaphore1.WaitOne();
            foreach (ListViewItem lvi in listView1.Items)
            {
                outfile.WriteLine(lvi.Text);
            }
            _semaphore1.Release();
            outfile.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            listView1.ResumeLayout();
            listView2.ResumeLayout();
            Thread.Sleep(0);
            listView1.SuspendLayout();
            listView2.SuspendLayout();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _navigatingCountdown--;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            _navigatingCountdown = 3;
            this.webBrowser1.Refresh();
        }
    }
}
