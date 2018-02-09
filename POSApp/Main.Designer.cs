namespace POSApp
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTra = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ms1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ms2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ms3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTra);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.gridControl1);
            this.panel1.Location = new System.Drawing.Point(12, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1094, 368);
            this.panel1.TabIndex = 4;
            // 
            // btnTra
            // 
            this.btnTra.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.btnTra.Appearance.Options.UseFont = true;
            this.btnTra.Location = new System.Drawing.Point(526, 296);
            this.btnTra.Name = "btnTra";
            this.btnTra.Size = new System.Drawing.Size(75, 65);
            this.btnTra.TabIndex = 6;
            this.btnTra.Text = "Trả";
            // 
            // btnAdd
            // 
            this.btnAdd.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.btnAdd.Appearance.Options.UseFont = true;
            this.btnAdd.Location = new System.Drawing.Point(431, 296);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 65);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.Name = "";
            this.gridControl1.Location = new System.Drawing.Point(3, 3);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1088, 287);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 40;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ms1,
            this.ms2,
            this.ms3});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.RowHeight = 30;
            // 
            // ms1
            // 
            this.ms1.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 20F);
            this.ms1.AppearanceCell.Options.UseFont = true;
            this.ms1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 20F);
            this.ms1.AppearanceHeader.Options.UseFont = true;
            this.ms1.Caption = "MS 1";
            this.ms1.FieldName = "MS1";
            this.ms1.Name = "ms1";
            this.ms1.Visible = true;
            this.ms1.VisibleIndex = 0;
            this.ms1.Width = 202;
            // 
            // ms2
            // 
            this.ms2.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 20F);
            this.ms2.AppearanceCell.Options.UseFont = true;
            this.ms2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 20F);
            this.ms2.AppearanceHeader.Options.UseFont = true;
            this.ms2.Caption = "MS 2";
            this.ms2.FieldName = "MS2";
            this.ms2.Name = "ms2";
            this.ms2.Visible = true;
            this.ms2.VisibleIndex = 1;
            this.ms2.Width = 202;
            // 
            // ms3
            // 
            this.ms3.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 20F);
            this.ms3.AppearanceCell.Options.UseFont = true;
            this.ms3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 20F);
            this.ms3.AppearanceHeader.Options.UseFont = true;
            this.ms3.Caption = "MS 3";
            this.ms3.FieldName = "MS3";
            this.ms3.Name = "ms3";
            this.ms3.Visible = true;
            this.ms3.VisibleIndex = 2;
            this.ms3.Width = 205;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(15, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(134, 65);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "Trang chủ";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(165, 12);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(128, 65);
            this.simpleButton2.TabIndex = 8;
            this.simpleButton2.Text = "Báo cáo";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.Location = new System.Drawing.Point(310, 12);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(122, 65);
            this.simpleButton3.TabIndex = 9;
            this.simpleButton3.Text = "Cài đặt";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 473);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phần mềm quản lý giấy cuộn tại xưởng";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn ms1;
        private DevExpress.XtraGrid.Columns.GridColumn ms2;
        private DevExpress.XtraGrid.Columns.GridColumn ms3;
        private DevExpress.XtraEditors.SimpleButton btnTra;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
    }
}