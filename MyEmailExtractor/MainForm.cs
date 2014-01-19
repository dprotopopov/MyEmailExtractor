using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MyEmEx
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        readonly SetupForm _setupForm = new SetupForm();
        public MainForm()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, EventArgs e)
        {

            if (_setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(Convert.ToInt32(_setupForm.Deep.Text), Convert.ToInt32(_setupForm.Threads.Text))
                {
                    MdiParent = this
                };
                childForm.Show();
                childForm.AddUrls(_setupForm.Url.Lines.ToList());
                childForm.StartWorker();
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null)
                childForm.StartWorker();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null)
                childForm.StopWorker();
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(saveFileDialog1.FileName);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(Convert.ToInt32(_setupForm.Deep.Text), Convert.ToInt32(_setupForm.Threads.Text))
                {
                    MdiParent = this
                };
                childForm.Show();
                childForm.AddUrls(_setupForm.Url.Lines.ToList());
                childForm.StartWorker();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null) childForm.StartWorker();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null) childForm.StopWorker();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = ActiveMdiChild as ChildForm;
            if (childForm != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(saveFileDialog1.FileName);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in MdiChildren)
            {
                ChildForm childForm = form as ChildForm;
                Debug.Assert(childForm != null, "childForm != null");
                if (childForm != null) childForm.AbortWorker();
            }
            Close();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }
    }
}
