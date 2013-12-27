using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyEmEx
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        SetupForm setupForm = new SetupForm();
        public MainForm()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, EventArgs e)
        {

            if (this.setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(Convert.ToInt32(this.setupForm.Deep.Text), Convert.ToInt32(this.setupForm.Threads.Text));
                childForm.MdiParent = this;
                childForm.Show();
                foreach(String line in this.setupForm.Url.Lines)
                    childForm.AddUrl(line, 0);
                childForm.StartWorker();
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null)
                childForm.StartWorker();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null)
                childForm.StopWorker();
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null && this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(this.saveFileDialog1.FileName);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.setupForm.ShowDialog() == DialogResult.OK)
            {
                ChildForm childForm = new ChildForm(Convert.ToInt32(this.setupForm.Deep.Text), Convert.ToInt32(this.setupForm.Threads.Text));
                childForm.MdiParent = this;
                childForm.Show();
                foreach (String line in this.setupForm.Url.Lines)
                    childForm.AddUrl(line, 0);
                childForm.StartWorker();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null)
                childForm.StartWorker();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null)
                childForm.StopWorker();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChildForm childForm = this.ActiveMdiChild as ChildForm;
            if (childForm != null && this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                childForm.SaveAs(this.saveFileDialog1.FileName);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in this.MdiChildren)
            {
                ChildForm childForm = form as ChildForm;
                childForm.StopWorker();
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
