using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace MyEmEx
{
    public partial class ChildForm : Form
    {
        private static readonly ReturnFieldInfos ReturnFieldInfos = new ReturnFieldInfos
        {
            new ReturnFieldInfo
            {
                ReturnFieldId = "Url",
                ReturnFieldXpathTemplate = @"//a[@href]",
                ReturnFieldResultTemplate = @"{{HrefValue}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexSelect = @".+"
            },
            new ReturnFieldInfo
            {
                ReturnFieldId = "Email",
                ReturnFieldXpathTemplate = @"//.",
                ReturnFieldResultTemplate = @"{{InnerHtml}}",
                ReturnFieldRegexPattern = @".*",
                ReturnFieldRegexReplacement = @"$&",
                ReturnFieldRegexSelect = @"\b[a-zAZ0-9]+([\.-][a-zAZ0-9]+)*@([a-zAZ0-9-]+\.)+[a-zA-Z]{2,6}\b"
            }
        };

        readonly List<WebTask> _webTasks = new List<WebTask>();
        readonly Dictionary<string, int> _webTasksIndex = new Dictionary<string, int>();

        private int TotalRunning
        {
            get
            {
                return _webTasks.Count(task => task.Status == WebTask.WebTaskStatus.Running);
            }
        }

        private void StartAllTasks()
        {
            int toStart = MaxThreads - TotalRunning;
            foreach (var task in _webTasks.Where(task => task.Status == WebTask.WebTaskStatus.Ready).Where(task => toStart-- > 0))
                task.Start();
        }
        private void ResumeAllTasks()
        {
            int toStart = MaxThreads - TotalRunning;
            foreach (var task in _webTasks.Where(task => task.Status == WebTask.WebTaskStatus.Paused).Where(task => toStart-- > 0))
                task.Resume();
        }

        delegate void OnWebTask(WebTask task);

        public void OnWebTaskDefault(WebTask task)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (listView2.InvokeRequired)
            {
                OnWebTask d = new OnWebTask(OnWebTaskDefault);
                var arr = new object[] { task };
                try
                {
                    Invoke(d, arr);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                Debug.Assert(listView2.Items.Count > task.Id);
                ListView.ListViewItemCollection items = listView2.Items;
                var lvi = items[task.Id];
                if (lvi != null) lvi.ImageIndex = (int)task.Status;
            }
        }

        private readonly Object _thisLock = new Object();
        public void AddTask(Uri uri, int level)
        {
            lock (_thisLock)
            {
                Debug.Assert(listView2.Items.Count == _webTasks.Count);
                Debug.Assert(_webTasksIndex.Count == _webTasks.Count);
                var newTask = new WebTask(_webTasks.Count, level)
                {
                    Url = uri.AbsoluteUri,
                    Method = "GET",
                    OnStartCallback = OnWebTaskDefault,
                    OnAbortCallback = OnWebTaskDefault,
                    OnResumeCallback = OnWebTaskDefault,
                    OnErrorCallback = OnWebTaskDefault,
                    OnCompliteCallback = OnWebTaskComplite,
                    ReturnFieldInfos = ReturnFieldInfos
                };
                if (newTask.Level < MaxLevel && !_webTasksIndex.ContainsKey(newTask.ToString().ToLower()))
                {
                    _webTasks.Add(newTask);
                    _webTasksIndex.Add(newTask.ToString().ToLower(), newTask.Id);
                    ListViewItem viewItem = new ListViewItem(newTask.ToString())
                    {
                        ImageIndex = 0,
                        Name = newTask.ToString().ToLower()
                    };
                    viewItem.SubItems.Add(Convert.ToString(newTask.Level));
                    listView2.Items.Add(viewItem);
                }
            }
        }
        public void OnWebTaskComplite(WebTask task)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (listView1.InvokeRequired || listView2.InvokeRequired)
            {
                OnWebTask d = new OnWebTask(OnWebTaskComplite);
                var arr = new object[] { task };
                try
                {
                    Invoke(d, arr);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                OnWebTaskDefault(task);

                foreach (var email in task.ReturnFields.Email.Where(email => listView1.Items[email.ToLower()] == null))
                {
                    listView1.Items.Add(new ListViewItem(email) { ImageIndex = 0, Name = email.ToLower() });
                }
                Uri baseUri = new Uri(task.Url);
                foreach (Uri uri in task.ReturnFields.Url.Select(url => new Uri(baseUri, url)))
                {
                    AddTask(uri, task.Level + 1);
                }
            }
        }

        public void AddUrls(List<string> urlList)
        {
            foreach (Uri uri in urlList.Select(url => new Uri(url)))
            {
                AddTask(uri, 0);
            }
        }

        private bool IsRunning
        {
            get
            {
                return timerLauncher.Enabled;
            }
            set
            {
                if (value)
                {
                    ResumeAllTasks();
                    StartAllTasks();
                    timerLauncher.Enabled = true;
                }
                else
                {
                    timerLauncher.Enabled = false;
                }
            }
        }

        private int MaxLevel { get; set; }
        private int MaxThreads { get; set; }

        private int _navigatingCountdown = 3;

        public ChildForm(int maxLevel, int maxThreads)
        {
            InitializeComponent();
            MaxLevel = maxLevel;
            MaxThreads = maxThreads;
        }

        private void NavigateToAdvert(object sender, WebBrowserNavigatingEventArgs e)
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
            IsRunning = true;
        }

        public void StopWorker()
        {
            IsRunning = false;
        }

        public void SaveAs(String fileName)
        {
            StreamWriter outfile = new StreamWriter(fileName);
            foreach (ListViewItem lvi in listView1.Items)
            {
                outfile.WriteLine(lvi.Text);
            }
            outfile.Close();
        }

        private void ResumeSuspendLayout(object sender, EventArgs e)
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

        private void AdvertRefresh(object sender, EventArgs e)
        {
            _navigatingCountdown = 3;
            webBrowser1.Refresh();
        }

        private void timerLauncher_Tick(object sender, EventArgs e)
        {
            StartAllTasks();
        }

        public void AbortWorker()
        {
            AbortAllTasks();
        }

        private void AbortAllTasks()
        {
            foreach (var task in _webTasks.Where(task => task.Status == WebTask.WebTaskStatus.Running || task.Status == WebTask.WebTaskStatus.Paused))
                task.Abort();
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView2.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView2.SelectedItems;
                ListViewItem item = items[0];
                int index = listView2.Items.IndexOf(item);
                WebTask task = _webTasks[index];
                string url = task.Url;
                Process.Start(url);
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
                ListViewItem item = items[0];
                Process.Start(@"mailto:" + item.Name);
            }
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AbortAllTasks();
        }
    }
}
