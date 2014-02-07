using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MyParser.Library;
using MyParser.Library.Forms;
using MyParser.Managed;
using MyWebSimulator;

namespace MyEmailExtractor
{
    public partial class ChildForm : XtraForm, IChildForm
    {
        public static readonly ReturnFieldInfos ReturnFieldInfos = new ReturnFieldInfos
        {
            Defaults.ReturnFieldInfos.Url,
            Defaults.ReturnFieldInfos.Email,
        };

        private int _navigatingCountdown = 3;

        public ChildForm()
        {
            InitializeComponent();
            WebTaskManager = new WebTaskManager {ListView = listViewQueue};
            WebSimulator = new WebSimulator {WebBrowser = new ManagedWebBrowser(webBrowser1)};
        }

        public List<string> Lines { get; set; }

        public IWebTaskManager WebTaskManager { get; set; }
        public IWebSimulator WebSimulator { get; set; }

        public object LastError { get; set; }

        public bool IsRunning
        {
            get { return timerLauncher.Enabled; }
            set
            {
                if (value)
                {
                    WebTaskManager.ResumeAllTasks();
                    WebTaskManager.StartAllTasks();
                    timerLauncher.Enabled = true;
                }
                else
                {
                    timerLauncher.Enabled = false;
                }
            }
        }

        public int MaxLevel
        {
            get { return WebTaskManager.MaxLevel; }
            set { WebTaskManager.MaxLevel = value; }
        }

        public int MaxThreads
        {
            get { return WebTaskManager.MaxThreads; }
            set { WebTaskManager.MaxThreads = value; }
        }

        public void GenerateTasks()
        {
            Debug.Assert(Lines != null, "Lines != null");
            foreach (Uri uri in Lines.Select(url => new Uri(url)))
            {
                try
                {
                    Debug.Assert(WebTaskManager != null, "WebTaskManager != null");
                    IWebTask newTask = WebTaskManager.AddTask(new WebTask
                    {
                        Url = uri.ToString(),
                        OnStartCallback = OnWebTaskDefault,
                        OnAbortCallback = OnWebTaskDefault,
                        OnResumeCallback = OnWebTaskDefault,
                        OnErrorCallback = OnWebTaskDefault,
                        OnCompliteCallback = OnWebTaskComplite,
                        ReturnFieldInfos = ReturnFieldInfos
                    });
                    Debug.WriteLine(newTask.ToString());
                }
                catch (Exception exception)
                {
                    LastError = exception;
                    Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
                }
            }
        }

        public void StartWorker()
        {
            IsRunning = true;
        }

        public void StopWorker()
        {
            IsRunning = false;
        }

        public void ShowAdvert()
        {
            WebSimulator.Click(@"//*");
        }

        public void AbortWorker()
        {
            WebTaskManager.AbortAllTasks();
        }

        public void OnWebTaskDefault(IWebTask webTask)
        {
            OnWebTaskDelegate d = WebTaskManager.OnWebTaskDefault;
            object[] arr = {webTask};
            try
            {
                Invoke(d, arr);
            }
            catch (Exception exception)
            {
                LastError = exception;
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
            }
        }

        public void OnWebTaskComplite(IWebTask webTask)
        {
            OnWebTaskDefault(webTask);

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (listViewEmail.InvokeRequired || listViewQueue.InvokeRequired)
            {
                OnWebTaskDelegate d = OnWebTaskComplite;
                object[] arr = {webTask};
                try
                {
                    Invoke(d, arr);
                }
                catch (Exception exception)
                {
                    LastError = exception;
                    Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
                }
            }
            else
            {
                foreach (
                    string email in
                        webTask.ReturnFields.Email.Where(email => listViewEmail.Items[email.ToLower()] == null)
                    )
                {
                    listViewEmail.Items.Add(new ListViewItem(email) {ImageIndex = 0, Name = email.ToLower()});
                }
                var baseUri = new Uri(webTask.Url);
                foreach (Uri uri in webTask.ReturnFields.Url.Select(url => new Uri(baseUri, url)))
                {
                    try
                    {
                        Debug.Assert(WebTaskManager != null, "WebTaskManager != null");
                        IWebTask newTask = WebTaskManager.AddTask(new WebTask
                        {
                            Url = uri.ToString(),
                            Level = webTask.Level + 1,
                            OnStartCallback = OnWebTaskDefault,
                            OnAbortCallback = OnWebTaskDefault,
                            OnResumeCallback = OnWebTaskDefault,
                            OnErrorCallback = OnWebTaskDefault,
                            OnCompliteCallback = OnWebTaskComplite,
                            ReturnFieldInfos = ReturnFieldInfos
                        });
                        Debug.WriteLine(newTask.ToString());
                    }
                    catch (Exception exception)
                    {
                        LastError = exception;
                        Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
                    }
                }
            }
        }

        public void NavigateToAdvert(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (_navigatingCountdown == 0)
            {
                e.Cancel = true;
                String url = e.Url.ToString();
                Process.Start(url);
            }
        }

        public void SaveAs(String fileName)
        {
            var outfile = new StreamWriter(fileName);
            foreach (ListViewItem lvi in listViewEmail.Items)
            {
                outfile.WriteLine(lvi.Text);
            }
            outfile.Close();
        }

        public void AdvertRefresh(object sender, EventArgs e)
        {
            _navigatingCountdown = 3;
            webBrowser1.Navigate(webBrowser1.Url);
        }

        private void childForm_Load(object sender, EventArgs e)
        {
            listViewEmail.SuspendLayout();
            listViewQueue.SuspendLayout();
        }

        private void ResumeSuspendLayout(object sender, EventArgs e)
        {
            listViewEmail.ResumeLayout();
            listViewQueue.ResumeLayout();
            Thread.Sleep(0);
            listViewEmail.SuspendLayout();
            listViewQueue.SuspendLayout();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //WebSimulator.Window = WebSimulator.TopmostWindow;
            _navigatingCountdown--;
        }

        private void timerLauncher_Tick(object sender, EventArgs e)
        {
            WebTaskManager.StartAllTasks();
        }


        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewQueue.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listViewQueue.SelectedItems;
                ListViewItem item = items[0];
                int index = listViewQueue.Items.IndexOf(item);
                IWebTask task = WebTaskManager.Tasks[index];
                string url = task.Url;
                Process.Start(url);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listViewEmail.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listViewEmail.SelectedItems;
                ListViewItem item = items[0];
                Process.Start(@"mailto:" + item.Name);
            }
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WebTaskManager.AbortAllTasks();
            Thread.Sleep(0);
        }
    }
}