namespace MyEmailExtractor
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::MyEmailExtractor.SplashScreen1), true, true);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DevExpress.XtraBars.Ribbon.GalleryItemGroup galleryItemGroup1 = new DevExpress.XtraBars.Ribbon.GalleryItemGroup();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.formAssistant1 = new DevExpress.XtraBars.FormAssistant();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.applicationMenu1 = new DevExpress.XtraBars.Ribbon.ApplicationMenu();
            this.barButtonItemSearch = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemStart = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemStop = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSaveAs = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemExit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemAbout = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSettings = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemMaxThreads = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.barEditItemMaxLevel = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSpinEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.barButtonItemLicenses = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            this.galleryDropDown1 = new DevExpress.XtraBars.Ribbon.GalleryDropDown();
            this.ribbonGalleryBarItemSkin = new DevExpress.XtraBars.RibbonGalleryBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.galleryDropDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.AddExtension = false;
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.Filter = "Text Files (*.txt)|*.txt";
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ApplicationButtonDropDownControl = this.applicationMenu1;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barButtonItemSearch,
            this.barButtonItemStart,
            this.barButtonItemStop,
            this.barButtonItemSaveAs,
            this.barButtonItemExit,
            this.barButtonItemAbout,
            this.barButtonItem7,
            this.barButtonItemSettings,
            this.barEditItemMaxThreads,
            this.barEditItemMaxLevel,
            this.barButtonItemLicenses,
            this.ribbonGalleryBarItemSkin});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 2;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1,
            this.repositoryItemSpinEdit2});
            this.ribbonControl1.Size = new System.Drawing.Size(1064, 158);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.barButtonItemSearch);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.barButtonItemSaveAs);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.barButtonItemAbout);
            // 
            // applicationMenu1
            // 
            this.applicationMenu1.ItemLinks.Add(this.barButtonItemSearch);
            this.applicationMenu1.ItemLinks.Add(this.barButtonItemStart);
            this.applicationMenu1.ItemLinks.Add(this.barButtonItemStop);
            this.applicationMenu1.ItemLinks.Add(this.barButtonItemSaveAs);
            this.applicationMenu1.ItemLinks.Add(this.barButtonItemExit);
            this.applicationMenu1.Name = "applicationMenu1";
            this.applicationMenu1.Ribbon = this.ribbonControl1;
            // 
            // barButtonItemSearch
            // 
            this.barButtonItemSearch.Caption = "Новый поиск";
            this.barButtonItemSearch.Glyph = global::MyEmailExtractor.Properties.Resources.wizard_32x32;
            this.barButtonItemSearch.Id = 1;
            this.barButtonItemSearch.LargeGlyph = global::MyEmailExtractor.Properties.Resources.wizard_32x32;
            this.barButtonItemSearch.Name = "barButtonItemSearch";
            this.barButtonItemSearch.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ShowWizardDialog);
            // 
            // barButtonItemStart
            // 
            this.barButtonItemStart.Caption = "Запустить загрузку";
            this.barButtonItemStart.Glyph = global::MyEmailExtractor.Properties.Resources.download_16x16;
            this.barButtonItemStart.Id = 2;
            this.barButtonItemStart.LargeGlyph = global::MyEmailExtractor.Properties.Resources.download_32x32;
            this.barButtonItemStart.Name = "barButtonItemStart";
            this.barButtonItemStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.StartWorker);
            // 
            // barButtonItemStop
            // 
            this.barButtonItemStop.Caption = "Остановить загрузку";
            this.barButtonItemStop.Glyph = global::MyEmailExtractor.Properties.Resources.cancel_16x16;
            this.barButtonItemStop.Id = 3;
            this.barButtonItemStop.LargeGlyph = global::MyEmailExtractor.Properties.Resources.cancel_32x32;
            this.barButtonItemStop.Name = "barButtonItemStop";
            this.barButtonItemStop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.StopWorker);
            // 
            // barButtonItemSaveAs
            // 
            this.barButtonItemSaveAs.Caption = "Сохранить результаты";
            this.barButtonItemSaveAs.Glyph = global::MyEmailExtractor.Properties.Resources.saveas_16x16;
            this.barButtonItemSaveAs.Id = 4;
            this.barButtonItemSaveAs.LargeGlyph = global::MyEmailExtractor.Properties.Resources.saveas_32x32;
            this.barButtonItemSaveAs.Name = "barButtonItemSaveAs";
            this.barButtonItemSaveAs.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SaveAs);
            // 
            // barButtonItemExit
            // 
            this.barButtonItemExit.Caption = "Выход";
            this.barButtonItemExit.Glyph = global::MyEmailExtractor.Properties.Resources.close_16x16;
            this.barButtonItemExit.Id = 5;
            this.barButtonItemExit.LargeGlyph = global::MyEmailExtractor.Properties.Resources.close_32x32;
            this.barButtonItemExit.Name = "barButtonItemExit";
            this.barButtonItemExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AbortWorker);
            // 
            // barButtonItemAbout
            // 
            this.barButtonItemAbout.Caption = "О программе";
            this.barButtonItemAbout.Glyph = global::MyEmailExtractor.Properties.Resources.info_16x16;
            this.barButtonItemAbout.Id = 6;
            this.barButtonItemAbout.LargeGlyph = global::MyEmailExtractor.Properties.Resources.info_32x32;
            this.barButtonItemAbout.Name = "barButtonItemAbout";
            this.barButtonItemAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ShowAboutBox);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "Посмотреть рекламу";
            this.barButtonItem7.Id = 7;
            this.barButtonItem7.LargeGlyph = global::MyEmailExtractor.Properties.Resources.operatingsystem_32x32;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ShowAdvertisement);
            // 
            // barButtonItemSettings
            // 
            this.barButtonItemSettings.Caption = "Конфигурация";
            this.barButtonItemSettings.Id = 8;
            this.barButtonItemSettings.LargeGlyph = global::MyEmailExtractor.Properties.Resources.operatingsystem_32x32;
            this.barButtonItemSettings.Name = "barButtonItemSettings";
            this.barButtonItemSettings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ShowFieldInfosDialog);
            // 
            // barEditItemMaxThreads
            // 
            this.barEditItemMaxThreads.Caption = "Количество потоков";
            this.barEditItemMaxThreads.Edit = this.repositoryItemSpinEdit1;
            this.barEditItemMaxThreads.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItemMaxThreads.Glyph")));
            this.barEditItemMaxThreads.Id = 9;
            this.barEditItemMaxThreads.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barEditItemMaxThreads.LargeGlyph")));
            this.barEditItemMaxThreads.Name = "barEditItemMaxThreads";
            this.barEditItemMaxThreads.Width = 80;
            this.barEditItemMaxThreads.EditValueChanged += new System.EventHandler(this.barEditItemMaxThreads_EditValueChanged);
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit1.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.IsFloatValue = false;
            this.repositoryItemSpinEdit1.Mask.EditMask = "N00";
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.MinValue = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            this.repositoryItemSpinEdit1.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            // 
            // barEditItemMaxLevel
            // 
            this.barEditItemMaxLevel.Caption = "Максимальный уровень";
            this.barEditItemMaxLevel.Edit = this.repositoryItemSpinEdit2;
            this.barEditItemMaxLevel.Glyph = ((System.Drawing.Image)(resources.GetObject("barEditItemMaxLevel.Glyph")));
            this.barEditItemMaxLevel.Id = 10;
            this.barEditItemMaxLevel.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barEditItemMaxLevel.LargeGlyph")));
            this.barEditItemMaxLevel.Name = "barEditItemMaxLevel";
            this.barEditItemMaxLevel.Width = 80;
            this.barEditItemMaxLevel.EditValueChanged += new System.EventHandler(this.barEditItemMaxLevel_EditValueChanged);
            // 
            // repositoryItemSpinEdit2
            // 
            this.repositoryItemSpinEdit2.AutoHeight = false;
            this.repositoryItemSpinEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit2.MaxValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.repositoryItemSpinEdit2.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
            this.repositoryItemSpinEdit2.SpinStyle = DevExpress.XtraEditors.Controls.SpinStyles.Horizontal;
            // 
            // barButtonItemLicenses
            // 
            this.barButtonItemLicenses.Caption = "Регистрация";
            this.barButtonItemLicenses.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLicenses.Glyph")));
            this.barButtonItemLicenses.Id = 11;
            this.barButtonItemLicenses.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItemLicenses.LargeGlyph")));
            this.barButtonItemLicenses.Name = "barButtonItemLicenses";
            this.barButtonItemLicenses.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup4,
            this.ribbonPageGroup3,
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Главная";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.Glyph = ((System.Drawing.Image)(resources.GetObject("ribbonPageGroup1.Glyph")));
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemSearch);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemStart);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemStop);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemSaveAs);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Поиск";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.Glyph = ((System.Drawing.Image)(resources.GetObject("ribbonPageGroup4.Glyph")));
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItemSettings);
            this.ribbonPageGroup4.ItemLinks.Add(this.barEditItemMaxThreads);
            this.ribbonPageGroup4.ItemLinks.Add(this.barEditItemMaxLevel);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "Настройки";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.Glyph = ((System.Drawing.Image)(resources.GetObject("ribbonPageGroup3.Glyph")));
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItem7);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemLicenses);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemAbout);
            this.ribbonPageGroup3.ItemLinks.Add(this.ribbonGalleryBarItemSkin);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "Помощь";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemExit);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "Выход";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 672);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1064, 27);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Visual Studio 2013 Light";
            // 
            // galleryDropDown1
            // 
            // 
            // 
            // 
            galleryItemGroup1.Caption = "Group1";
            this.galleryDropDown1.Gallery.Groups.AddRange(new DevExpress.XtraBars.Ribbon.GalleryItemGroup[] {
            galleryItemGroup1});
            this.galleryDropDown1.Name = "galleryDropDown1";
            this.galleryDropDown1.Ribbon = this.ribbonControl1;
            // 
            // ribbonGalleryBarItemSkin
            // 
            this.ribbonGalleryBarItemSkin.Caption = "ribbonGalleryBarItemSkin";
            this.ribbonGalleryBarItemSkin.Id = 1;
            this.ribbonGalleryBarItemSkin.Name = "ribbonGalleryBarItemSkin";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 699);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "My Email Extractor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applicationMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.galleryDropDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private DevExpress.XtraBars.FormAssistant formAssistant1;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu1;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSearch;
        private DevExpress.XtraBars.BarButtonItem barButtonItemStart;
        private DevExpress.XtraBars.BarButtonItem barButtonItemStop;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSaveAs;
        private DevExpress.XtraBars.BarButtonItem barButtonItemExit;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAbout;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSettings;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarEditItem barEditItemMaxThreads;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemMaxLevel;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemLicenses;
        private DevExpress.XtraBars.Ribbon.GalleryDropDown galleryDropDown1;
        private DevExpress.XtraBars.RibbonGalleryBarItem ribbonGalleryBarItemSkin;
     }
}

