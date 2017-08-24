namespace DieuChinhNhapKho
{
    partial class HistoryForm
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.clID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clIdnksx = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clOldValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clNewValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.Name = "";
            gridLevelNode1.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(687, 336);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.clID,
            this.clIdnksx,
            this.clUser,
            this.clDate,
            this.clOldValue,
            this.clNewValue});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // clID
            // 
            this.clID.Caption = "ID";
            this.clID.FieldName = "ID";
            this.clID.Name = "clID";
            // 
            // clIdnksx
            // 
            this.clIdnksx.Caption = "ID Nhật ký sản xuất";
            this.clIdnksx.FieldName = "IDNKSX";
            this.clIdnksx.Name = "clIdnksx";
            this.clIdnksx.Visible = true;
            this.clIdnksx.VisibleIndex = 0;
            // 
            // clUser
            // 
            this.clUser.Caption = "Người cập nhật";
            this.clUser.FieldName = "UserName";
            this.clUser.Name = "clUser";
            this.clUser.Visible = true;
            this.clUser.VisibleIndex = 1;
            // 
            // clDate
            // 
            this.clDate.Caption = "Ngày cập nhật";
            this.clDate.FieldName = "Date";
            this.clDate.Name = "clDate";
            this.clDate.Visible = true;
            this.clDate.VisibleIndex = 2;
            // 
            // clOldValue
            // 
            this.clOldValue.Caption = "Giá trị trước cập nhật";
            this.clOldValue.FieldName = "OLDVALUE";
            this.clOldValue.Name = "clOldValue";
            this.clOldValue.Visible = true;
            this.clOldValue.VisibleIndex = 3;
            // 
            // clNewValue
            // 
            this.clNewValue.Caption = "Giá trị đã cập nhật";
            this.clNewValue.FieldName = "NEWVALUE";
            this.clNewValue.Name = "clNewValue";
            this.clNewValue.Visible = true;
            this.clNewValue.VisibleIndex = 4;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControl1);
            this.panelControl1.Location = new System.Drawing.Point(1, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(687, 336);
            this.panelControl1.TabIndex = 1;
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 337);
            this.Controls.Add(this.panelControl1);
            this.Name = "HistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lịch sử cập nhật";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn clID;
        private DevExpress.XtraGrid.Columns.GridColumn clIdnksx;
        private DevExpress.XtraGrid.Columns.GridColumn clUser;
        private DevExpress.XtraGrid.Columns.GridColumn clDate;
        private DevExpress.XtraGrid.Columns.GridColumn clOldValue;
        private DevExpress.XtraGrid.Columns.GridColumn clNewValue;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}