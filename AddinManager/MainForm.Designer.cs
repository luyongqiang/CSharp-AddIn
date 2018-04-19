namespace AddinManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.windowsUIView1 = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView(this.components);
            this.pageGroup = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.PageGroup(this.components);
            this.closeFlyout = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Flyout(this.components);
            this.imageList1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeFlyout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageList1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.View = this.windowsUIView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.windowsUIView1});
            // 
            // windowsUIView1
            // 
            this.windowsUIView1.AddTileWhenCreatingDocument = DevExpress.Utils.DefaultBoolean.False;
            this.windowsUIView1.AllowCaptionDragMove = DevExpress.Utils.DefaultBoolean.True;
            this.windowsUIView1.Caption = "Setup Wizard Caption";
            this.windowsUIView1.ContentContainers.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.IContentContainer[] {
            this.pageGroup,
            this.closeFlyout});
            this.windowsUIView1.UseSplashScreen = DevExpress.Utils.DefaultBoolean.False;
            this.windowsUIView1.NavigationBarsShowing += new DevExpress.XtraBars.Docking2010.Views.WindowsUI.NavigationBarsCancelEventHandler(this.windowsUIView1_NavigationBarsShowing);
            this.windowsUIView1.QueryDocumentActions += new DevExpress.XtraBars.Docking2010.Views.WindowsUI.QueryDocumentActionsEventHandler(this.windowsUIView1_QueryDocumentActions);
            this.windowsUIView1.NavigatedTo += new DevExpress.XtraBars.Docking2010.Views.WindowsUI.NavigationEventHandler(this.windowsUIView1_NavigatedTo);
            // 
            // pageGroup
            // 
            this.pageGroup.ButtonInterval = 30;
            this.pageGroup.Name = "pageGroup";
            this.pageGroup.Properties.ShowPageHeaders = DevExpress.Utils.DefaultBoolean.False;
            // 
            // closeFlyout
            // 
            this.closeFlyout.Document = null;
            this.closeFlyout.FlyoutButtons = System.Windows.Forms.MessageBoxButtons.YesNo;
            this.closeFlyout.Name = "closeFlyout";
            // 
            // imageList1
            // 
            this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.InsertGalleryImage("backward_32x32.png", "grayscaleimages/navigation/backward_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("grayscaleimages/navigation/backward_32x32.png"), 0);
            this.imageList1.Images.SetKeyName(0, "backward_32x32.png");
            this.imageList1.InsertGalleryImage("forward_32x32.png", "grayscaleimages/navigation/forward_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("grayscaleimages/navigation/forward_32x32.png"), 1);
            this.imageList1.Images.SetKeyName(1, "forward_32x32.png");
            this.imageList1.InsertGalleryImage("cancel_32x32.png", "grayscaleimages/actions/cancel_32x32.png", DevExpress.Images.ImageResourceCache.Default.GetImage("grayscaleimages/actions/cancel_32x32.png"), 2);
            this.imageList1.Images.SetKeyName(2, "cancel_32x32.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 495);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Wizard";
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeFlyout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageList1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView windowsUIView1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.PageGroup pageGroup;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Flyout closeFlyout;
        private DevExpress.Utils.ImageCollection imageList1;
    }
}

