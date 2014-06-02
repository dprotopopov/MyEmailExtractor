using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MyLibrary.Trace;
using MyParser;
using MyParser.Forms;
using MyParser.ItemView;
using MyParser.WebSessions;
using MyParser.WebTasks;
using MyProtector;

namespace MyEmailExtractor
{
    public partial class ChildForm : XtraForm, IChildForm<WebTaskView, WebSessionView, EmailView>, ISettings
    {
        public static readonly ReturnFieldInfos ReturnFieldInfos = new ReturnFieldInfos
        {
            Defaults.ReturnFieldInfos.Url,
            Defaults.ReturnFieldInfos.Email,
        };

        private readonly Object _progress = new Object();

        public ChildForm()
        {
            InitializeComponent();
            Current = 0;
            Total = 0;
            DataGridViewTaskManager = new DataGridViewTaskManager<WebTaskView>
            {
                DataGridView = dataGridViewQueue,
                ChildForm = this,
            };
            DataGridViewResultManager = new DataGridViewResultManager<EmailView>
            {
                DataGridView = dataGridViewEmail,
                ChildForm = this,
            };
            dataGridViewQueue.DataSource = DataGridViewTaskManager.Items;
            dataGridViewEmail.DataSource = DataGridViewResultManager.Items;
            Application.Idle += (sender, e) => IdleLauncher(sender, e);
            Application.Idle += (sender, e) => IdleUpdate(sender, e);
            Application.Idle += (sender, e) => Thread.Yield();
        }

        public ISettings Settings { get; set; }

        private long Current { get; set; }
        private long Total { get; set; }
        public List<string> Lines { get; set; }
        public DataGridViewResultManager<EmailView> DataGridViewResultManager { get; set; }
        public DataGridViewSessionManager<WebSessionView> DataGridViewSessionManager { get; set; }
        public DataGridViewTaskManager<WebTaskView> DataGridViewTaskManager { get; set; }
        public object LastError { get; set; }
        public TimeSpan Timeout { get; set; }

        public bool IsRunning { get; set; }

        public int MaxLevel
        {
            get { return DataGridViewTaskManager.MaxLevel; }
            set { DataGridViewTaskManager.MaxLevel = value; }
        }

        public int MaxThreads
        {
            get { return DataGridViewTaskManager.MaxThreads; }
            set { DataGridViewTaskManager.MaxThreads = value; }
        }

        public int MaxSessions
        {
            get { return DataGridViewSessionManager.MaxSessions; }
            set { DataGridViewSessionManager.MaxSessions = value; }
        }

        public bool UseRandomProxy { get; set; }

        public void GenerateTasks()
        {
            foreach (Uri uri in Lines.Select(url => new Uri(url)))
            {
                try
                {
                    var webSession = new WebSession();
                    AddTask(new WebTask
                    {
                        Url = uri.ToString(),
                        Level = 0,
                        OnStartCallback = OnWebTaskDefault,
                        OnAbortCallback = OnWebTaskDefault,
                        OnResumeCallback = OnWebTaskDefault,
                        OnErrorCallback = OnWebTaskDefault,
                        OnSuspendCallback = OnWebTaskDefault,
                        OnCompliteCallback = OnWebTaskComplite,
                        ReturnFieldInfos = ReturnFieldInfos,
                        WebSession = webSession,
                    });
                    Application.DoEvents();
                    Thread.Yield();
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
                finally
                {
                }
            }
        }

        public void LoadFrom(string fileName)
        {
            throw new NotImplementedException();
        }

        public void OnWebSessionDefault(IWebSession webSession)
        {
            throw new NotImplementedException();
        }

        public void OnWebTaskDefault(IWebTask webTask)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (DataGridViewTaskManager.DataGridView.InvokeRequired)
            {
                OnWebTaskCallbackDelegate d = OnWebTaskDefault;
                object[] arr = {webTask};
                Invoke(d, arr);
            }
            else
                try
                {
                    DataGridViewTaskManager.Wait(DataGridViewTaskManager.Items);
                    DataGridViewTaskManager.OnWebTaskDefault(webTask);
                    Application.DoEvents();
                    Thread.Yield();
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewTaskManager.Release(DataGridViewTaskManager.Items);
                }
        }


        public void OnWebTaskCompliteOrError(IWebTask webTask)
        {
            Application.DoEvents();
        }

        /// <summary>
        ///     Добавление новой задачи в очередь
        ///     InvokeRequired required compares the thread ID of the
        ///     calling thread to the thread ID of the creating thread.
        ///     If these threads are different, it returns true.
        /// </summary>
        /// <returns></returns>
        public IWebTask AddTask(IWebTask webTask)
        {
            if (DataGridViewTaskManager.DataGridView.InvokeRequired)
            {
                AddTaskDelegate d = AddTask;
                object[] arr = {webTask};
                webTask = (IWebTask) Invoke(d, arr);
            }
            else
                try
                {
                    DataGridViewTaskManager.Wait(DataGridViewTaskManager.Items);
                    lock (_progress) ProgressCallback(Current, Total++);
                    webTask = DataGridViewTaskManager.AddTask(webTask);
                    Application.DoEvents();
                    Thread.Yield();
                }
                catch (Exception exception)
                {
                    lock (_progress) ProgressCallback(Current, --Total);
                    LastError = exception;
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewTaskManager.Release(DataGridViewTaskManager.Items);
                }
            return webTask;
        }

        public void RemoveTask(IWebTask newTask)
        {
            if (DataGridViewTaskManager.DataGridView.InvokeRequired)
            {
                RemoveTaskDelegate d = RemoveTask;
                object[] arr = {newTask};
                Invoke(d, arr);
            }
            else
                try
                {
                    DataGridViewTaskManager.Wait(DataGridViewTaskManager.Items);
                    DataGridViewTaskManager.RemoveTask(newTask);
                    Application.DoEvents();
                    Thread.Yield();
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewTaskManager.Release(DataGridViewTaskManager.Items);
                }
        }

        public void ClearTaskManager()
        {
            Application.DoEvents();
            Thread.Yield();
        }

        public void ClearSessionManager()
        {
            Application.DoEvents();
            Thread.Yield();
        }

        public IWebSession AddSession(IWebSession webSession)
        {
            if (DataGridViewSessionManager.DataGridView.InvokeRequired)
            {
                AddSessionDelegate d = AddSession;
                object[] arr = {webSession};
                webSession = (IWebSession) Invoke(d, arr);
            }
            else
                try
                {
                    DataGridViewSessionManager.Wait(DataGridViewSessionManager.Items);
                    webSession = DataGridViewSessionManager.AddSession(webSession);
                    Application.DoEvents();
                    Thread.Yield();
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewSessionManager.Release(DataGridViewSessionManager.Items);
                }
            return webSession;
        }

        public void RemoveSession(IWebSession webSession)
        {
            if (DataGridViewSessionManager.DataGridView.InvokeRequired)
            {
                RemoveSessionDelegate d = RemoveSession;
                object[] arr = {webSession};
                Invoke(d, arr);
            }
            else
                try
                {
                    DataGridViewSessionManager.Wait(DataGridViewSessionManager.Items);
                    DataGridViewSessionManager.RemoveSession(webSession);
                    Application.DoEvents();
                    Thread.Yield();
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewSessionManager.Release(DataGridViewSessionManager.Items);
                }
        }


        public void SaveAs(String fileName)
        {
            if (DataGridViewTaskManager.DataGridView.InvokeRequired)
            {
                SaveAsDelegate d = SaveAs;
                object[] arr = {};
                Invoke(d, arr);
            }
            else
                try
                {
                    DataGridViewTaskManager.Wait(DataGridViewTaskManager.Items);
                    using (var outfile = new StreamWriter(fileName))
                    {
                        foreach (EmailView emailView in DataGridViewResultManager.Items)
                        {
                            outfile.WriteLine(emailView.Address);
                            Application.DoEvents();
                            Thread.Yield();
                        }
                        outfile.Close();
                    }
                    Application.DoEvents();
                    Thread.Yield();
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewTaskManager.Release(DataGridViewTaskManager.Items);
                }
        }

        public void IdleLauncher(params object[] parameters)
        {
            if (IsRunning) DataGridViewTaskManager.ResumeAllTasks();
            if (IsRunning) DataGridViewTaskManager.StartAllTasks();
            Application.DoEvents();
            Thread.Yield();
        }

        public void IdleUpdate(params object[] parameters)
        {
            Application.DoEvents();
            Thread.Yield();
        }

        public string SecretPhase
        {
            get { return Settings.SecretPhase; }
            set { Settings.SecretPhase = value; }
        }

        public string ActivationEmailTemplate
        {
            get { return Settings.ActivationEmailTemplate; }
            set { Settings.ActivationEmailTemplate = value; }
        }

        public string AsymmetricAlgorithmPublicKey
        {
            get { return Settings.AsymmetricAlgorithmPublicKey; }
            set { Settings.AsymmetricAlgorithmPublicKey = value; }
        }

        public string[] FeatureNames
        {
            get { return Settings.FeatureNames; }
            set { Settings.FeatureNames = value; }
        }

        public string ApplicationName
        {
            get { return Settings.ApplicationName; }
            set { Settings.ApplicationName = value; }
        }

        public string ActivationEmailAddress
        {
            get { return Settings.ActivationEmailAddress; }
            set { Settings.ActivationEmailAddress = value; }
        }

        public string ActivationEmailSubject
        {
            get { return Settings.ActivationEmailSubject; }
            set { Settings.ActivationEmailSubject = value; }
        }

        #region DataGridViewTaskManager

        public void StartWorker(params object[] parameters)
        {
            IsRunning = true;
            DataGridViewTaskManager.ResumeAllTasks();
            DataGridViewTaskManager.StartAllTasks();
            Application.DoEvents();
            Thread.Yield();
        }

        public void StopWorker(params object[] parameters)
        {
            IsRunning = false;
            DataGridViewTaskManager.SuspendAllTasks();
            Application.DoEvents();
            Thread.Yield();
        }

        public void AbortWorker(params object[] parameters)
        {
            IsRunning = false;
            DataGridViewTaskManager.AbortAllTasks();
            Application.DoEvents();
            Thread.Yield();
        }

        #endregion

        private void ProgressCallback(long current, long total)
        {
            Debug.Assert(current <= total);
            if (progressBar1.InvokeRequired)
            {
                ProgressCallback d = ProgressCallback;
                object[] objects = {current, total};
                Invoke(d, objects);
            }
            else
            {
                progressBar1.Maximum = (int) Math.Min(total, 10000);
                progressBar1.Value = (int) (current*progressBar1.Maximum/(1 + total));
                Application.DoEvents();
                Thread.Yield();
            }
        }

        public void OnWebTaskComplite(IWebTask webTask)
        {
            OnWebTaskDefault(webTask);

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (dataGridViewEmail.InvokeRequired || dataGridViewQueue.InvokeRequired)
            {
                OnWebTaskCallbackDelegate d = OnWebTaskComplite;
                object[] arr = {webTask};
                try
                {
                    Invoke(d, arr);
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
            }
            else
            {
                try
                {
                    DataGridViewResultManager.Wait(DataGridViewResultManager.DataGridView);
                    foreach (
                        EmailView result in
                            webTask.ReturnFields.Email.Where(
                                s =>
                                    DataGridViewResultManager.Items.All(
                                        i => string.Compare(i.Address, s, StringComparison.OrdinalIgnoreCase) != 0))
                                .Select(s => new EmailView {Address = s}))
                        try
                        {
                            DataGridViewResultManager.Items.Add(result);
                            Application.DoEvents();
                            Thread.Yield();
                        }
                        catch (Exception exception)
                        {
                            LastError = exception;
                        }
                        finally
                        {
                        }
                    Application.DoEvents();
                    Thread.Yield();
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewResultManager.Release(DataGridViewResultManager.DataGridView);
                }
                var baseUri = new Uri(webTask.Url);
                try
                {
                    DataGridViewTaskManager.Wait(DataGridViewTaskManager.DataGridView);
                    foreach (Uri uri in webTask.ReturnFields.Url.Select(s => new Uri(baseUri, s)))
                        try
                        {
                            var webSession = new WebSession();
                            AddTask(new WebTask
                            {
                                Url = uri.ToString(),
                                Level = webTask.Level + 1,
                                OnStartCallback = OnWebTaskDefault,
                                OnAbortCallback = OnWebTaskDefault,
                                OnResumeCallback = OnWebTaskDefault,
                                OnErrorCallback = OnWebTaskDefault,
                                OnSuspendCallback = OnWebTaskDefault,
                                OnCompliteCallback = OnWebTaskComplite,
                                ReturnFieldInfos = ReturnFieldInfos,
                                WebSession = webSession,
                            });
                            Application.DoEvents();
                            Thread.Yield();
                        }
                        catch (Exception exception)
                        {
                            LastError = exception;
                        }
                        finally
                        {
                        }
                    Application.DoEvents();
                    Thread.Yield();
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
                finally
                {
                    // Whether or not the exception was thrown, the current 
                    // thread owns the mutex, and must release it. 
                    DataGridViewTaskManager.Release(DataGridViewTaskManager.DataGridView);
                }
            }
        }

        private void childForm_Load(object sender, EventArgs e)
        {
            //Application.Idle += IdleLauncher;
        }

        private void ChildForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataGridViewTaskManager.AbortAllTasks();
            Application.DoEvents();
            Thread.Yield();
        }

        private void ChildForm_Activated(object sender, EventArgs e)
        {
            var mainForm = MdiParent as IMainForm;
            if (mainForm != null && mainForm.MaxLevel != MaxLevel) mainForm.MaxLevel = MaxLevel;
            if (mainForm != null && mainForm.MaxThreads != MaxThreads) mainForm.MaxThreads = MaxThreads;
            Application.DoEvents();
            Thread.Yield();
        }

        private void dataGridViewQueue_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridViewTaskManager.Wait(DataGridViewTaskManager.DataGridView);
                Process.Start(gridView2.GetRowCellValue(gridView2.FocusedRowHandle,"Url").ToString());
            }
            catch (Exception exception)
            {
            }
            finally
            {
                DataGridViewTaskManager.Release(DataGridViewTaskManager.DataGridView);
            }
        }

        private void dataGridViewEmail_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridViewResultManager.Wait(DataGridViewResultManager.DataGridView);
                Process.Start(string.Format("mailto:{0}", gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Address")));
            }
            catch (Exception exception)
            {
            }
            finally
            {
                DataGridViewResultManager.Release(DataGridViewResultManager.DataGridView);
            }
        }
    }
}