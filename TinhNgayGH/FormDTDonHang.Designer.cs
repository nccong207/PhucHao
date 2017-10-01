namespace TinhNgayGH
{
    partial class FormDTDonHang
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridDTDonHang = new DevExpress.XtraGrid.GridControl();
            this.gvChonPhoiSong = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcLoai = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLop = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcTenHang = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcDai = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRong = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcCao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcSoluong = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcDao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcChonPS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rilookUpEditPS = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDTDonHang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvChonPhoiSong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilookUpEditPS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.gridDTDonHang);
            this.panelControl1.Location = new System.Drawing.Point(2, 1);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(868, 302);
            this.panelControl1.TabIndex = 0;
            // 
            // gridDTDonHang
            // 
            this.gridDTDonHang.EmbeddedNavigator.Name = "";
            this.gridDTDonHang.Location = new System.Drawing.Point(0, 0);
            this.gridDTDonHang.MainView = this.gvChonPhoiSong;
            this.gridDTDonHang.Name = "gridDTDonHang";
            this.gridDTDonHang.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rilookUpEditPS});
            this.gridDTDonHang.Size = new System.Drawing.Size(864, 264);
            this.gridDTDonHang.TabIndex = 0;
            this.gridDTDonHang.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvChonPhoiSong});
            // 
            // gvChonPhoiSong
            // 
            this.gvChonPhoiSong.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcLoai,
            this.gcLop,
            this.gcTenHang,
            this.gcDai,
            this.gcRong,
            this.gcCao,
            this.gcSoluong,
            this.gcDao,
            this.gcChonPS});
            this.gvChonPhoiSong.GridControl = this.gridDTDonHang;
            this.gvChonPhoiSong.Name = "gvChonPhoiSong";
            // 
            // gcLoai
            // 
            this.gcLoai.Caption = "Loại";
            this.gcLoai.FieldName = "Loai";
            this.gcLoai.Name = "gcLoai";
            this.gcLoai.Visible = true;
            this.gcLoai.VisibleIndex = 0;
            this.gcLoai.Width = 65;
            // 
            // gcLop
            // 
            this.gcLop.Caption = "Lớp";
            this.gcLop.DisplayFormat.FormatString = "###,###";
            this.gcLop.FieldName = "Lop";
            this.gcLop.Name = "gcLop";
            this.gcLop.Visible = true;
            this.gcLop.VisibleIndex = 1;
            this.gcLop.Width = 40;
            // 
            // gcTenHang
            // 
            this.gcTenHang.Caption = "Tên hàng";
            this.gcTenHang.FieldName = "TenHang";
            this.gcTenHang.Name = "gcTenHang";
            this.gcTenHang.Visible = true;
            this.gcTenHang.VisibleIndex = 2;
            this.gcTenHang.Width = 203;
            // 
            // gcDai
            // 
            this.gcDai.Caption = "Dài";
            this.gcDai.DisplayFormat.FormatString = "###,###.###";
            this.gcDai.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gcDai.FieldName = "Dai";
            this.gcDai.Name = "gcDai";
            this.gcDai.Visible = true;
            this.gcDai.VisibleIndex = 3;
            this.gcDai.Width = 65;
            // 
            // gcRong
            // 
            this.gcRong.Caption = "Rộng";
            this.gcRong.DisplayFormat.FormatString = "###,###.###";
            this.gcRong.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gcRong.FieldName = "Rong";
            this.gcRong.Name = "gcRong";
            this.gcRong.Visible = true;
            this.gcRong.VisibleIndex = 4;
            this.gcRong.Width = 71;
            // 
            // gcCao
            // 
            this.gcCao.Caption = "Cao";
            this.gcCao.DisplayFormat.FormatString = "###,###.###";
            this.gcCao.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gcCao.FieldName = "Cao";
            this.gcCao.Name = "gcCao";
            this.gcCao.Visible = true;
            this.gcCao.VisibleIndex = 5;
            this.gcCao.Width = 66;
            // 
            // gcSoluong
            // 
            this.gcSoluong.Caption = "Số lượng";
            this.gcSoluong.DisplayFormat.FormatString = "###,###,###";
            this.gcSoluong.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gcSoluong.FieldName = "SoLuong";
            this.gcSoluong.Name = "gcSoluong";
            this.gcSoluong.Visible = true;
            this.gcSoluong.VisibleIndex = 6;
            this.gcSoluong.Width = 85;
            // 
            // gcDao
            // 
            this.gcDao.Caption = "Dao";
            this.gcDao.DisplayFormat.FormatString = "###";
            this.gcDao.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gcDao.FieldName = "Dao";
            this.gcDao.Name = "gcDao";
            this.gcDao.Visible = true;
            this.gcDao.VisibleIndex = 7;
            // 
            // gcChonPS
            // 
            this.gcChonPS.Caption = "Chọn phôi sóng";
            this.gcChonPS.ColumnEdit = this.rilookUpEditPS;
            this.gcChonPS.FieldName = "DTDHPSID";
            this.gcChonPS.Name = "gcChonPS";
            this.gcChonPS.Visible = true;
            this.gcChonPS.VisibleIndex = 8;
            this.gcChonPS.Width = 173;
            // 
            // rilookUpEditPS
            // 
            this.rilookUpEditPS.AutoHeight = false;
            this.rilookUpEditPS.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rilookUpEditPS.Name = "rilookUpEditPS";
            this.rilookUpEditPS.NullText = "";
            this.rilookUpEditPS.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(769, 270);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(87, 24);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Đồng ý";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FormDTDonHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 301);
            this.Controls.Add(this.panelControl1);
            this.MaximizeBox = false;
            this.Name = "FormDTDonHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chi tiết đơn hàng";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDTDonHang_FormClosed);
            this.Load += new System.EventHandler(this.FormDTDonHang_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDTDonHang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvChonPhoiSong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rilookUpEditPS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridDTDonHang;
        private DevExpress.XtraGrid.Views.Grid.GridView gvChonPhoiSong;
        private DevExpress.XtraGrid.Columns.GridColumn gcLoai;
        private DevExpress.XtraGrid.Columns.GridColumn gcLop;
        private DevExpress.XtraGrid.Columns.GridColumn gcTenHang;
        private DevExpress.XtraGrid.Columns.GridColumn gcDai;
        private DevExpress.XtraGrid.Columns.GridColumn gcRong;
        private DevExpress.XtraGrid.Columns.GridColumn gcCao;
        private DevExpress.XtraGrid.Columns.GridColumn gcSoluong;
        private DevExpress.XtraGrid.Columns.GridColumn gcDao;
        private DevExpress.XtraGrid.Columns.GridColumn gcChonPS;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit rilookUpEditPS;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}