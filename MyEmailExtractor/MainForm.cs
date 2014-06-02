using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Ribbon;
using MyParser.Forms;
using MyParser.ItemView;
using MyProtector;
using MyProtector.Forms;

namespace MyEmailExtractor
{
    public partial class MainForm : RibbonForm, IMainForm, ISettings
    {
        private readonly ReturnFieldInfosDialog _returnFieldInfosDialog = new ReturnFieldInfosDialog
        {
            ReturnFieldInfos = ChildForm.ReturnFieldInfos.ToList()
        };

        private readonly StartupForm _startupForm = new StartupForm();

        public MainForm()
        {
            InitializeComponent();
            SecretPhase = "My$ecretPa$$W0rd"; // the passsword
            ActivationEmailAddress = "activate@company.com";
            ApplicationName = "My Email Extractor";
            ActivationEmailSubject = "Activation request";
            ActivationEmailTemplate =
                @"Please, activate application @Model.ApplicationName for key @Model.Key and query @Model.Query. My payment details is ...";
            FeatureNames = new[]
            {
                "Save emails",
                "Not Implemented",
                "Not Implemented",
                "Not Implemented",
                "Not Implemented",
                "Not Implemented",
                "Not Implemented",
                "Not Implemented"
            };
            AsymmetricAlgorithmPublicKey = @"";
            Application.Idle += (sender, e) => IdleUpdate(sender, e);
            Application.Idle += (sender, e) => Thread.Yield();
        }

        public void SaveAs(object sender, ItemClickEventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm<WebTaskView, WebSessionView, EmailView>;
            if (childForm != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(saveFileDialog1.FileName);
            }
        }


        public void ShowAboutBox(object sender, ItemClickEventArgs e)
        {
            var about = new AboutBox {Assembly = Assembly.GetExecutingAssembly()};
            about.ShowDialog();
        }

        public void ShowAdvertisement(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ShowFieldInfosDialog(object sender, ItemClickEventArgs e)
        {
            _returnFieldInfosDialog.ShowDialog();
        }

        public object LastError { get; set; }

        public void IdleUpdate(params object[] parameters)
        {
            Application.DoEvents();
        }

        public string SecretPhase { get; set; }
        public string ApplicationName { get; set; }
        public string ActivationEmailAddress { get; set; }
        public string ActivationEmailSubject { get; set; }
        public string ActivationEmailTemplate { get; set; }
        public string AsymmetricAlgorithmPublicKey { get; set; }
        public string[] FeatureNames { get; set; }

        #region

        public void StartWorker(object sender, ItemClickEventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm<WebTaskView, WebSessionView, EmailView>;
            if (childForm != null) childForm.StartWorker(sender, e);
        }

        public void StopWorker(object sender, ItemClickEventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm<WebTaskView, WebSessionView, EmailView>;
            if (childForm != null) childForm.StopWorker(sender, e);
        }

        public void AbortWorker(object sender, ItemClickEventArgs e)
        {
            foreach (
                var childForm in
                    MdiChildren.Select(form => form as IChildForm<WebTaskView, WebSessionView, EmailView>)
                        .Where(childForm => childForm != null))
            {
                childForm.AbortWorker(sender, e);
            }
            Close();
        }

        public int MaxLevel
        {
            get { return Convert.ToInt32(barEditItemMaxLevel.EditValue); }
            set { barEditItemMaxLevel.EditValue = value; }
        }

        public int MaxThreads
        {
            get { return Convert.ToInt32(barEditItemMaxThreads.EditValue); }
            set { barEditItemMaxThreads.EditValue = value; }
        }

        public int MaxSessions { get; set; }
        public bool UseRandomProxy { get; set; }
        public TimeSpan Timeout { get; set; }

        #endregion

        private void ShowWizardDialog(object sender, ItemClickEventArgs e)
        {
            if (_startupForm.ShowDialog() == DialogResult.OK)
            {
                var childForm = new ChildForm
                {
                    MdiParent = this,
                    Lines = _startupForm.textBoxUrl.Lines.ToList(),
                    MaxLevel = Convert.ToInt32(_startupForm.textBoxMaxLevel.Value),
                    MaxThreads = Convert.ToInt32(_startupForm.textBoxMaxThreads.Value),
                    Settings = this
                };
                childForm.Show();
                childForm.GenerateTasks();
                childForm.StartWorker(sender, e);
            }
        }

        private void barEditItemMaxThreads_EditValueChanged(object sender, EventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm<WebTaskView, WebSessionView, EmailView>;
            if (childForm != null && childForm.MaxThreads != MaxThreads)
                childForm.MaxThreads = MaxThreads;
        }

        private void barEditItemMaxLevel_EditValueChanged(object sender, EventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm<WebTaskView, WebSessionView, EmailView>;
            if (childForm != null && childForm.MaxLevel != MaxLevel)
                childForm.MaxLevel = MaxLevel;
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new LicensesDialog
            {
                Settings = this
            };
            dialog.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SkinHelper.InitSkinGallery(ribbonGalleryBarItemSkin, true, true);
        }
    }
}