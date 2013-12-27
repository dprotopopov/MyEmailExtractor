using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Resources;
using HtmlAgilityPack;

namespace MyEmEx
{
    public partial class ChildForm : Form
    {
        private class ThreadProcArg
        {
            public Semaphore semaphore0;
            public Semaphore semaphore1;
            public Semaphore semaphore2;
            public ChildForm This;
            public int index;
            public int maxLevel;
        }

        private Semaphore _semaphore0;
        private Semaphore _semaphore1 = new Semaphore(1, 1);
        private Semaphore _semaphore2 = new Semaphore(1, 1);
        private int _index = 0;
        private bool _isRunning = false;
        private ImageList _imageList1 = new ImageList();
        private ImageList _imageList2 = new ImageList();

        private int _maxLevel;
        private int _navigatingCountdown = 3;

        delegate void GetUrlCallback(int index, ref String Url, ref int level);
        delegate void AddUrlCallback(String Url, int level);
        delegate void AddEmailCallback(String email);

        public void GetUrl(int index, ref String url, ref int level)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listView2.InvokeRequired)
            {
                GetUrlCallback d = new GetUrlCallback(GetUrl);
                var arr = new object[] { index, url, level };
                this.Invoke(d, arr);
                url = (String)arr[1];
                level = (int)arr[2];
            }
            else
            {
                ListViewItem lvi = this.listView2.Items[index];
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
            if (this.listView2.InvokeRequired)
            {
                AddUrlCallback d = new AddUrlCallback(AddUrl);
                this.Invoke(d, new object[] { url, level });
            }
            else
            {
                if (this.listView2.Items[url.ToLower()] == null)
                {
                    ListViewItem lvi = new ListViewItem(url);
                    lvi.ImageIndex = 0;
                    lvi.Name = url.ToLower();
                    lvi.SubItems.Add(Convert.ToString(level));
                    this.listView2.Items.Add(lvi);
                }
            }
        }
        public void AddEmail(String email)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listView1.InvokeRequired)
            {
                AddEmailCallback d = new AddEmailCallback(AddEmail);
                this.Invoke(d, new object[] { email });
            }
            else
            {
                if (this.listView1.Items[email.ToLower()] == null)
                {
                    ListViewItem lvi = new ListViewItem(email);
                    lvi.ImageIndex = 0;
                    lvi.Name = email.ToLower();
                    this.listView1.Items.Add(lvi);
                }
            }
        }

        public static void ThreadProc(object obj)
        {
            Debug.Print("Start ThreadProc");
            ThreadProcArg arg = obj as ThreadProcArg;


            String URL = @"";
            int level = 0;
            arg.semaphore2.WaitOne();
            arg.This.GetUrl(arg.index, ref URL, ref level);
            arg.semaphore2.Release();
          
            try
            {
                HtmlWeb hw = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = hw.Load(URL);

                if (level != arg.maxLevel)
                {
                    Uri baseUri = new Uri(URL);

                    foreach (HtmlAgilityPack.HtmlNode link in doc.DocumentNode.SelectNodes(@".//a[@href]"))
                    {
                        HtmlAgilityPack.HtmlAttribute att = link.Attributes["href"];
                        Uri uri = new Uri(baseUri, att.Value);
                        String URL2 = uri.AbsoluteUri;
                        arg.semaphore2.WaitOne();
                        arg.This.AddUrl(URL2, level + 1);
                        arg.semaphore2.Release();
                    }
                }

                String html = doc.DocumentNode.InnerHtml;
                String patternEmail = @"\b[a-zAZ0-9]+([\.-][a-zAZ0-9]+)*@([a-zAZ0-9-]+\.)+[a-zA-Z]{2,6}\b";
                Regex rgxEmail = new Regex(patternEmail, RegexOptions.IgnoreCase | RegexOptions.Multiline);

                foreach (Match matchEmail in rgxEmail.Matches(html))
                {
                    arg.semaphore1.WaitOne();
                    arg.This.AddEmail(matchEmail.Value);
                    arg.semaphore1.Release();
                }
            }
            catch
            {
            }

            arg.semaphore0.Release();
            Debug.Print("End ThreadProc");
        }

        public ChildForm(int maxLevel, int maxThreads)
        {
            InitializeComponent();
            _maxLevel = maxLevel;
            this._semaphore0 = new Semaphore(maxThreads, maxThreads);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_navigatingCountdown == 0)
            {
                e.Cancel = true;
                SHDocVw.InternetExplorer IE = new SHDocVw.InternetExplorer();
                object Empty = null;
                String URL = e.Url.ToString();
                IE.Visible = true;
                IE.Navigate(URL, ref Empty, ref Empty, ref Empty, ref Empty);
            }
        }

        private void childForm_Load(object sender, EventArgs e)
        {
            this.listView1.SuspendLayout();
            this.listView2.SuspendLayout();
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
            if (_isRunning && backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                _isRunning = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            BackgroundWorker worker = sender as BackgroundWorker;


            while (worker.CancellationPending != true)
            {
                if (_index < this.listView2.Items.Count)
                {
                    this._semaphore0.WaitOne();

                    ThreadProcArg arg = new ThreadProcArg();
                    arg.semaphore0 = this._semaphore0;
                    arg.semaphore1 = this._semaphore1;
                    arg.semaphore2 = this._semaphore2;
                    arg.This = this;
                    arg.index = this._index;
                    arg.maxLevel = this._maxLevel;

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
            this._semaphore1.WaitOne();
            foreach (ListViewItem lvi in this.listView1.Items)
            {
                outfile.WriteLine(lvi.Text);
            }
            this._semaphore1.Release();
            outfile.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.listView1.ResumeLayout();
            this.listView2.ResumeLayout();
            Thread.Sleep(0);
            this.listView1.SuspendLayout();
            this.listView2.SuspendLayout();
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
