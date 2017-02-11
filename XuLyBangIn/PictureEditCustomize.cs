using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using System.Drawing;
using System.ComponentModel;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Drawing;
using System.Windows.Forms;

namespace XuLyBangIn
{
    public class ZoomPictureEdit : PictureEdit
    {
        private DevExpress.XtraEditors.VScrollBar vScroll = null;

        public DevExpress.XtraEditors.VScrollBar VScroll
        {
            get { return vScroll; }
            set { vScroll = value; }
        }

        private DevExpress.XtraEditors.HScrollBar hScroll = null;

        public DevExpress.XtraEditors.HScrollBar HScroll
        {
            get { return hScroll; }
            set { hScroll = value; }
        }

        static ZoomPictureEdit()
        {
            RepositoryItemZoomPictureEdit.Register();
        }

        public ZoomPictureEdit()
        {
            CreateVerticalScrollBar();
            CreateHorizontalScrollBar();
            hScroll.Show();
            vScroll.Show();
        }

        private void CreateVerticalScrollBar()
        {
            if (vScroll != null)
            {
                return;
            }
            vScroll = new DevExpress.XtraEditors.VScrollBar();
            vScroll.Parent = this;
            vScroll.ScrollBarAutoSize = true;
            vScroll.Scroll += OnVerticalScroll;
            vScroll.HandleCreated += OnScrollBarHandleCreated;
        }

        private void CreateHorizontalScrollBar()
        {
            if (hScroll != null)
            {
                return;
            }
            hScroll = new DevExpress.XtraEditors.HScrollBar();
            hScroll.Parent = this;
            hScroll.ScrollBarAutoSize = true;
            hScroll.Scroll += OnHorizontalScroll;
            hScroll.HandleCreated += OnScrollBarHandleCreated;
        }

        protected virtual void OnScrollBarHandleCreated(object sender, EventArgs e)
        {
            ((ScrollBarBase)sender).Value = 0;
        }

        protected virtual void OnVerticalScroll(Object sender, ScrollEventArgs e)
        {
            Properties.YIndent = vScroll.Value;
            Properties.MaximumYIndent = vScroll.Maximum;
            this.Refresh();
        }

        protected virtual void OnHorizontalScroll(Object sender, ScrollEventArgs e)
        {
            Properties.XIndent = hScroll.Value;
            Properties.MaximumXIndent = hScroll.Maximum;
            this.Refresh();
        }

        protected virtual int CalcVScrollBarMaximum()
        {
            if (ViewInfo.PictureRect.Width > this.Width)
            {
                return ViewInfo.PictureRect.Height - this.Height + vScroll.LargeChange - 1 + hScroll.Height;
            }
            return ViewInfo.PictureRect.Height - this.Height + vScroll.LargeChange - 1;
        }

        protected virtual int CalcHScrollBarMaximum()
        {
            if (ViewInfo.PictureRect.Height > this.Height)
            {
                return ViewInfo.PictureRect.Width - this.Width + hScroll.LargeChange - 1 + vScroll.Width;
            }
            return ViewInfo.PictureRect.Width - this.Width + hScroll.LargeChange - 1;
        }

        protected virtual void UpdateScrollBars()
        {
            if (this.Image != null && this.Properties.Scrollable)
            {
                if (ViewInfo.PictureRect.Height > this.Height)
                {
                    vScroll.Left = this.Width - vScroll.Width;
                    vScroll.Height = this.Image.Width > this.Width ? this.Height - hScroll.Height : this.Height;
                    vScroll.Maximum = CalcVScrollBarMaximum();
                    vScroll.Show();
                }
                else
                {
                    Properties.XIndent = 0;
                    vScroll.Hide();
                }
                if (ViewInfo.PictureRect.Width > this.Width)
                {
                    hScroll.Top = this.Height - hScroll.Height;
                    hScroll.Width = this.Image.Height > this.Height ? this.Width - vScroll.Width : this.Width;
                    hScroll.Maximum = (int)Math.Round((double)CalcHScrollBarMaximum() * Properties.fZoomFactor);
                    hScroll.Show();
                }
                else
                {
                    Properties.YIndent = 0;
                    hScroll.Hide();
                }
            }
            else
            {
                vScroll.Hide();
                hScroll.Hide();
            }
        }

        protected override void LayoutChanged()
        {
            base.LayoutChanged();
            UpdateScrollBars();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
                Properties.ZoomFactor += 10;
            if (e.Delta < 0 && Properties.ZoomFactor > 10)
                Properties.ZoomFactor -= 10;
            vScroll.Value = vScroll.Maximum / 2;
            hScroll.Value = hScroll.Maximum / 2;
            UpdateScrollBars();
            base.OnMouseWheel(e);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new RepositoryItemZoomPictureEdit Properties
        {
            get { return base.Properties as RepositoryItemZoomPictureEdit; }
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemZoomPictureEdit.EditorName; }
        }
    }

    public class RepositoryItemZoomPictureEdit : RepositoryItemPictureEdit
    {
        private int xIndent;

        private int yIndent;

        public int XIndent
        {
            get { return xIndent; }
            set { xIndent = value; }
        }

        public int YIndent
        {
            get { return yIndent; }
            set { yIndent = value; }
        }

        private int fMaximumXIndent;

        public int MaximumXIndent
        {
            get { return fMaximumXIndent; }
            set { fMaximumXIndent = value; }
        }

        private int fMaximumYIndent;

        public int MaximumYIndent
        {
            get { return fMaximumYIndent; }
            set { fMaximumYIndent = value; }
        }

        internal const string EditorName = "ZoomPictureEdit";
        private float zoomFactor = 1;
        private bool scrollable;

        static RepositoryItemZoomPictureEdit()
        {
            Register();
        }
        public RepositoryItemZoomPictureEdit()
        {
            SizeMode = PictureSizeMode.Zoom;
        }

        public static void Register()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(ZoomPictureEdit),
                typeof(RepositoryItemZoomPictureEdit), typeof(ZoomPictureEditViewInfo),
                    new PictureEditPainter(), true, null));
        }

        public override void Assign(RepositoryItem item)
        {
            BeginUpdate();
            try
            {
                base.Assign(item);
                this.ZoomFactor = ((RepositoryItemZoomPictureEdit)item).ZoomFactor;
                this.Scrollable = ((RepositoryItemZoomPictureEdit)item).Scrollable;
            }
            finally
            {
                EndUpdate();
            }
        }

        public bool Scrollable
        {
            get { return scrollable; }
            set
            {
                scrollable = value;
                this.OnPropertiesChanged();
            }
        }

        public new PictureSizeMode SizeMode
        {
            get { return base.SizeMode; }
            set
            {
                base.SizeMode = value;
                if ((SizeMode != PictureSizeMode.Clip) && (SizeMode != PictureSizeMode.Zoom))
                {
                    Scrollable = false;
                }
            }
        }

        public override string EditorTypeName
        {
            get { return EditorName; }
        }
        public int ZoomFactor
        {
            get { return (int)Math.Round(zoomFactor * 100); }
            set
            {
                zoomFactor = value / 100.0f;
                this.OnPropertiesChanged();
            }
        }
        protected internal float fZoomFactor
        {
            get { return zoomFactor; }
        }
    }

    public class ZoomPictureEditViewInfo : PictureEditViewInfo
    {
        public ZoomPictureEditViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem item)
            : base(item)
        {
        }

        protected override void CalcImageRect(Rectangle bounds)
        {
            //float zoomFactor = ((RepositoryItemZoomPictureEdit)Item).fZoomFactor;

            //Rectangle originRect = bounds;
            //bounds.Width = (int)Math.Round(bounds.Width * zoomFactor);
            //int horizIndent = (originRect.Width - bounds.Width) / 2;
            //bounds.X += horizIndent;

            //bounds.Height = (int)Math.Round(bounds.Height * zoomFactor);
            //int vertIndent = (originRect.Height - bounds.Height) / 2;
            //bounds.Y += vertIndent;

            //base.CalcImageRect(bounds);

            RepositoryItemZoomPictureEdit rep = (RepositoryItemZoomPictureEdit)Item;
            float zoomFactor = rep.fZoomFactor;
            int xIndent = rep.XIndent - rep.MaximumXIndent / 2;
            int yIndent = rep.YIndent - rep.MaximumYIndent / 2;
            Rectangle originRect = bounds;
            bounds.Width = (int)Math.Round(originRect.Width * zoomFactor);
            int horizIndent = (originRect.Width - bounds.Width) / 2;
            bounds.X += horizIndent - xIndent;
            bounds.Height = (int)Math.Round(originRect.Height * zoomFactor);
            int vertIndent = (originRect.Height - bounds.Height) / 2;
            bounds.Y += vertIndent - yIndent;
            base.CalcImageRect(bounds);
        }
    }
}
