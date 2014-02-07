using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using MyParser.Library.Forms;

namespace MyEmailExtractor
{
    public partial class MainForm : RibbonForm
    {
        private readonly ReturnFieldInfosDialog _returnFieldInfosDialog = new ReturnFieldInfosDialog
        {
            ReturnFieldInfos = ChildForm.ReturnFieldInfos
        };

        private readonly StartupForm _startupForm = new StartupForm();

        public MainForm()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_startupForm.ShowDialog() == DialogResult.OK)
            {
                var childForm = new ChildForm
                {
                    MaxLevel = Convert.ToInt32(_startupForm.textBoxMaxLevel.Value),
                    MaxThreads = Convert.ToInt32(_startupForm.textBoxMaxThreads.Value),
                    Lines = _startupForm.textBoxUrl.Lines.ToList(),
                    MdiParent = this
                };
                childForm.Show();
                childForm.GenerateTasks();
                childForm.StartWorker();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm;
            if (childForm != null) childForm.StartWorker();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm;
            if (childForm != null) childForm.StopWorker();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm;
            if (childForm != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(saveFileDialog1.FileName);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            foreach (Form form in MdiChildren)
            {
                var childForm = form as IChildForm;
                Debug.Assert(childForm != null, "childForm != null");
                if (childForm != null) childForm.AbortWorker();
            }
            Close();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            var about = new AboutBox { Assembly = Assembly.GetExecutingAssembly() };
            about.ShowDialog();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            var childForm = ActiveMdiChild as IChildForm;
            if (childForm != null) childForm.ShowAdvert();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            _returnFieldInfosDialog.ShowDialog();
        }
    }
}