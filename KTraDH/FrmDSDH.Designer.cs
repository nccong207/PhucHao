namespace KTraDH
{
    partial class FrmDSDH
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
            this.gcDSDH = new DevExpress.XtraGrid.GridControl();
            this.gvDSDH = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.gcDSDH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDSDH)).BeginInit();
            this.SuspendLayout();
            // 
            // gcDSDH
            // 
            this.gcDSDH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDSDH.EmbeddedNavigator.Name = "";
            this.gcDSDH.Location = new System.Drawing.Point(0, 0);
            this.gcDSDH.MainView = this.gvDSDH;
            this.gcDSDH.Name = "gcDSDH";
            this.gcDSDH.Size = new System.Drawing.Size(630, 320);
            this.gcDSDH.TabIndex = 0;
            this.gcDSDH.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDSDH});
            // 
            // gvDSDH
            // 
            this.gvDSDH.GridControl = this.gcDSDH;
            this.gvDSDH.Name = "gvDSDH";
            this.gvDSDH.OptionsBehavior.Editable = false;
            this.gvDSDH.OptionsView.ColumnAutoWidth = false;
            // 
            // FrmDSDH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 320);
            this.Controls.Add(this.gcDSDH);
            this.Name = "FrmDSDH";
            this.Text = "Đơn hàng chưa lập lệnh sản xuất";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmDSDH_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcDSDH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDSDH)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcDSDH;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDSDH;
    }
}