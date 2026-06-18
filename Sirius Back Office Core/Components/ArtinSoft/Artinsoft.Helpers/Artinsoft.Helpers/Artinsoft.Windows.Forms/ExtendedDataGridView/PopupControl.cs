using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Artinsoft.Windows.Forms
{
    /// <summary>
    /// Enum that lists different Resizing Modes used by PopUp controls
    /// </summary>
    public enum PopupResizeMode
    {
        /// <summary>
        /// No resizing allowed
        /// </summary>
        None = 0,

        // Individual styles.
        /// <summary>
        /// Resizing on left side
        /// </summary>
        Left = 1,
        /// <summary>
        /// Resizing on top side
        /// </summary>
        Top = 2,
        /// <summary>
        /// Resizing on right side
        /// </summary>
        Right = 4,
        /// <summary>
        /// Resizing on bottom side
        /// </summary>
        Bottom = 8,

        // Combined styles.
        /// <summary>
        /// Resizing on all sides
        /// </summary>
        All = (Top | Left | Bottom | Right),
        /// <summary>
        /// Resizing on top and left side
        /// </summary>
        TopLeft = (Top | Left),
        /// <summary>
        /// Resizing on top and right side
        /// </summary>
        TopRight = (Top | Right),
        /// <summary>
        /// Resizing on bottom and left side
        /// </summary>
        BottomLeft = (Bottom | Left),
        /// <summary>
        /// Resizing on top and right side
        /// </summary>
        BottomRight = (Bottom | Right),
    }

    /// <summary>
    /// Enumeration used to specify and control Grip Alignment modes used by popup controls
    /// </summary>
    public enum GripAlignMode
    {
        /// <summary>
        /// Top and Left Grip alignment
        /// </summary>
        TopLeft,
        /// <summary>
        /// Top and Right Grip alignment
        /// </summary>
        TopRight,
        /// <summary>
        /// Bottom and Left Grip alignment
        /// </summary>
        BottomLeft,
        /// <summary>
        /// Bottom and Right Grip alignment
        /// </summary>
        BottomRight,
    }

    internal class PopupDropDown : ToolStripDropDown
    {
        #region Construction and destruction

        public PopupDropDown(bool autoSize)
        {
            AutoSize = autoSize;
            Padding = Margin = Padding.Empty;
        }

        #endregion

        #region ToolStripDropDown overrides

        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
                hostedControl.SizeChanged -= hostedControl_SizeChanged;
            base.OnClosing(e);
        }

        Brush BackgroundBrush
        {
            get
            {
                return SystemBrushes.ControlLightLight;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            GripBounds = Rectangle.Empty;

            if (CompareResizeMode(PopupResizeMode.BottomLeft))
            {
                // Draw grip area at bottom-left of popup.
                e.Graphics.FillRectangle(BackgroundBrush, 1, Height - 16, Width - 2, 14);
                GripBounds = new Rectangle(1, Height - 16, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.BottomLeft);
            }
            else if (CompareResizeMode(PopupResizeMode.BottomRight))
            {
                // Draw grip area at bottom-right of popup.
                e.Graphics.FillRectangle(BackgroundBrush, 1, Height - 16, Width - 2, 14);
                GripBounds = new Rectangle(Width - 17, Height - 16, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.BottomRight);
            }
            else if (CompareResizeMode(PopupResizeMode.TopLeft))
            {
                // Draw grip area at top-left of popup.
                e.Graphics.FillRectangle(BackgroundBrush, 1, 1, Width - 2, 14);
                GripBounds = new Rectangle(1, 0, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.TopLeft);
            }
            else if (CompareResizeMode(PopupResizeMode.TopRight))
            {
                // Draw grip area at top-right of popup.
                e.Graphics.FillRectangle(BackgroundBrush, 1, 1, Width - 2, 14);
                GripBounds = new Rectangle(Width - 17, 0, 16, 16);
                GripRenderer.Render(e.Graphics, GripBounds.Location, GripAlignMode.TopRight);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            // When drop-down window is being resized by the user (i.e. not locked),
            // update size of hosted control.
            if (!_lockedThisSize)
                RecalculateHostedControlLayout();
        }

        protected void hostedControl_SizeChanged(object sender, EventArgs e)
        {
            // Only update size of this container when it is not locked.
            if (!_lockedHostedControlSize)
                ResizeFromContent(-1);
        }

        #endregion

        #region Methods

        public new void Show(int x, int y)
        {
            Show(x, y, -1, -1);
        }

        public void Show(int x, int y, int width, int height)
        {
            // If no hosted control is associated, this procedure is pointless!
            Control hostedControl = GetHostedControl();
            if (hostedControl == null)
                return;

            // Initially hosted control should be displayed within a drop down of 1x1, however
            // its size should exceed the dimensions of the drop-down.
            {
                _lockedHostedControlSize = true;
                _lockedThisSize = true;

                // Display actual popup and occupy just 1x1 pixel to avoid automatic reposition.
                Size = new Size(1, 1);
                base.Show(x, y);

                _lockedHostedControlSize = false;
                _lockedThisSize = false;
            }

            // Resize drop-down to fit its contents.
            ResizeFromContent(width);

            // If client area was enlarged using the minimum width paramater, then the hosted
            // control must also be enlarged.
            if (_refreshSize)
                RecalculateHostedControlLayout();

            // If popup is overlapping the initial position then move above!
            if (y > Top && y <= Bottom)
            {
                Top = y - Height - (height != -1 ? height : 0);

                PopupResizeMode previous = ResizeMode;
                if (ResizeMode == PopupResizeMode.BottomLeft)
                    ResizeMode = PopupResizeMode.TopLeft;
                else if (ResizeMode == PopupResizeMode.BottomRight)
                    ResizeMode = PopupResizeMode.TopRight;

                if (ResizeMode != previous)
                    RecalculateHostedControlLayout();
            }

            // Assign event handler to control.
            hostedControl.SizeChanged -= hostedControl_SizeChanged;
            hostedControl.SizeChanged += hostedControl_SizeChanged;
        }

        protected void ResizeFromContent(int width)
        {
            if (_lockedThisSize)
                return;

            // Prevent resizing hosted control to 1x1 pixel!
            _lockedHostedControlSize = true;

            // Resize from content again because certain information was not available before.
            Rectangle bounds = Bounds;
            bounds.Size = SizeFromContent(width);

            if (!CompareResizeMode(PopupResizeMode.None))
            {
                if (width > 0 && bounds.Width - 2 > width)
                    if (!CompareResizeMode(PopupResizeMode.Right))
                        bounds.X -= bounds.Width - 2 - width;
            }

            Bounds = bounds;

            _lockedHostedControlSize = false;
        }

        protected void RecalculateHostedControlLayout()
        {
            if (_lockedHostedControlSize)
                return;

            _lockedThisSize = true;

            // Update size of hosted control.
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                // Fetch control bounds and adjust as necessary.
                Rectangle bounds = hostedControl.Bounds;
                if (CompareResizeMode(PopupResizeMode.TopLeft) || CompareResizeMode(PopupResizeMode.TopRight))
                    bounds.Location = new Point(1, 16);
                else
                    bounds.Location = new Point(1, 1);

                bounds.Width = ClientRectangle.Width - 2;
                bounds.Height = ClientRectangle.Height - 2;
                if (IsGripShown)
                    bounds.Height -= 16;

                if (bounds.Size != hostedControl.Size)
                    hostedControl.Size = bounds.Size;
                if (bounds.Location != hostedControl.Location)
                    hostedControl.Location = bounds.Location;
            }
            _lockedThisSize = false;
        }

        public Control GetHostedControl()
        {
            if (Items.Count > 0)
            {
                ToolStripControlHost host = Items[0] as ToolStripControlHost;
                if (host != null)
                    return host.Control;
            }
            return null;
        }

        public bool CompareResizeMode(PopupResizeMode resizeMode)
        {
            return (ResizeMode & resizeMode) == resizeMode;
        }

        protected Size SizeFromContent(int width)
        {
            Size contentSize = Size.Empty;

            _refreshSize = false;

            // Fetch hosted control.
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                if (CompareResizeMode(PopupResizeMode.TopLeft) || CompareResizeMode(PopupResizeMode.TopRight))
                    hostedControl.Location = new Point(1, 16);
                else
                    hostedControl.Location = new Point(1, 1);
                contentSize = SizeFromClientSize(hostedControl.Size);

                // Use minimum width (if specified).
                if (width > 0 && contentSize.Width < width)
                {
                    contentSize.Width = width;
                    _refreshSize = true;
                }
            }

            // If a grip box is shown then add it into the drop down height.
            if (IsGripShown)
                contentSize.Height += 16;

            // Add some additional space to allow for borders.
            contentSize.Width += 2;
            contentSize.Height += 2;

            return contentSize;
        }

        #endregion

        #region Win32 message processing

        #region Win32 stuff

        protected const int WM_GETMINMAXINFO = 0x0024;
        protected const int WM_NCHITTEST = 0x0084;

        protected const int HTTRANSPARENT = -1;
        protected const int HTLEFT = 10;
        protected const int HTRIGHT = 11;
        protected const int HTTOP = 12;
        protected const int HTTOPLEFT = 13;
        protected const int HTTOPRIGHT = 14;
        protected const int HTBOTTOM = 15;
        protected const int HTBOTTOMLEFT = 16;
        protected const int HTBOTTOMRIGHT = 17;

        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }

        protected static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }
        protected static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }
        protected static int LOWORD(int n)
        {
            return n & 0xffff;
        }
        protected static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        #endregion

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (!ProcessGrip(ref m, false))
                base.WndProc(ref m);
        }

        /// <summary>
        /// Processes the resizing messages.
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns>true, if the WndProc method from the base class shouldn't be invoked.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessGrip(ref Message m)
        {
            return ProcessGrip(ref m, true);
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool ProcessGrip(ref Message m, bool contentControl)
        {
            if (ResizeMode != PopupResizeMode.None)
            {
                switch (m.Msg)
                {
                    case WM_NCHITTEST:
                        return OnNcHitTest(ref m, contentControl);

                    case WM_GETMINMAXINFO:
                        return OnGetMinMaxInfo(ref m);
                }
            }
            return false;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            Control hostedControl = GetHostedControl();
            if (hostedControl != null)
            {
                MINMAXINFO minmax = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));

                // Maximum size.
                if (hostedControl.MaximumSize.Width != 0)
                    minmax.maxTrackSize.Width = hostedControl.MaximumSize.Width;
                if (hostedControl.MaximumSize.Height != 0)
                    minmax.maxTrackSize.Height = hostedControl.MaximumSize.Height;

                // Minimum size.
                minmax.minTrackSize = new Size(32, 32);
                if (hostedControl.MinimumSize.Width > minmax.minTrackSize.Width)
                    minmax.minTrackSize.Width = hostedControl.MinimumSize.Width;
                if (hostedControl.MinimumSize.Height > minmax.minTrackSize.Height)
                    minmax.minTrackSize.Height = hostedControl.MinimumSize.Height;

                Marshal.StructureToPtr(minmax, m.LParam, false);
            }
            return true;
        }

        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            Point location = PointToClient(new Point(LOWORD(m.LParam), HIWORD(m.LParam)));
            IntPtr transparent = new IntPtr(HTTRANSPARENT);

            // Check for simple gripper dragging.
            if (GripBounds.Contains(location))
            {
                if (CompareResizeMode(PopupResizeMode.BottomLeft))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOMLEFT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.BottomRight))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOMRIGHT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.TopLeft))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOPLEFT;
                    return true;
                }
                else if (CompareResizeMode(PopupResizeMode.TopRight))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOPRIGHT;
                    return true;
                }
            }
            else   // Check for edge based dragging.
            {
                Rectangle rectClient = ClientRectangle;
                if (location.X > rectClient.Right - 3 && location.X <= rectClient.Right && CompareResizeMode(PopupResizeMode.Right))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTRIGHT;
                    return true;
                }
                else if (location.Y > rectClient.Bottom - 3 && location.Y <= rectClient.Bottom && CompareResizeMode(PopupResizeMode.Bottom))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTBOTTOM;
                    return true;
                }
                else if (location.X > -1 && location.X < 3 && CompareResizeMode(PopupResizeMode.Left))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTLEFT;
                    return true;
                }
                else if (location.Y > -1 && location.Y < 3 && CompareResizeMode(PopupResizeMode.Top))
                {
                    m.Result = contentControl ? transparent : (IntPtr)HTTOP;
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Type of resize mode, grips are automatically drawn at bottom-left and bottom-right corners.
        /// </summary>
        public PopupResizeMode ResizeMode
        {
            get { return _resizeMode; }
            set
            {
                if (value != _resizeMode)
                {
                    _resizeMode = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Bounds of active grip box position.
        /// </summary>
        protected Rectangle GripBounds
        {
            get { return this._gripBounds; }
            set { this._gripBounds = value; }
        }

        /// <summary>
        /// Indicates when a grip box is shown.
        /// </summary>
        protected bool IsGripShown
        {
            get
            {
                return (ResizeMode == PopupResizeMode.TopLeft || ResizeMode == PopupResizeMode.TopRight ||
                        ResizeMode == PopupResizeMode.BottomLeft || ResizeMode == PopupResizeMode.BottomRight);
            }
        }

        #endregion

        #region Attributes

        private PopupResizeMode _resizeMode = PopupResizeMode.None;
        private Rectangle _gripBounds = Rectangle.Empty;

        private bool _lockedHostedControlSize;
        private bool _lockedThisSize;
        private bool _refreshSize;

        #endregion
    }

    /// <summary>
    /// Public class used to render grips
    /// </summary>
    public sealed class GripRenderer
    {
        #region Construction and destruction

        private GripRenderer()
        {
        }

        #endregion

        #region Methods

        private static void InitializeGripBitmap(Graphics g, Size size, bool forceRefresh)
        {
            if (_sGripBitmap == null || forceRefresh || size != _sGripBitmap.Size)
            {
                // Draw size grip into a bitmap image.
                _sGripBitmap = new Bitmap(size.Width, size.Height, g);
                using (Graphics gripG = Graphics.FromImage(_sGripBitmap))
                    ControlPaint.DrawSizeGrip(gripG, SystemColors.ButtonFace, 0, 0, size.Width, size.Height);
            }
        }

        /// <summary>
        /// This method refreshes system colors using specified Graphics and Size.
        /// It internally invokes InitializeGripBitmap passing the specified Graphics and Size as well as a boolean of true.
        /// </summary>
        /// <param name="g">Graphics instance used to handle grips</param>
        /// <param name="size">Grip Bitmap size</param>
        public static void RefreshSystemColors(Graphics g, Size size)
        {
            InitializeGripBitmap(g, size, true);
        }

        /// <summary>
        /// Renders Grip according to the specified Graphics, Point, Size and Grip AlignMode.
        /// </summary>
        /// <param name="g">Graphics instance used to render the grip</param>
        /// <param name="location">Point specifing the location for the grip</param>
        /// <param name="size">Size to be used when rendering the grip</param>
        /// <param name="mode">GripAlignMode specifying the alignment of the grip</param>
        public static void Render(Graphics g, Point location, Size size, GripAlignMode mode)
        {
            InitializeGripBitmap(g, size, false);

            // Calculate display size and position of grip.
            switch (mode)
            {
                case GripAlignMode.TopLeft:
                    size.Height = -size.Height;
                    size.Width = -size.Width;
                    break;
                case GripAlignMode.TopRight:
                    size.Height = -size.Height;
                    break;
                case GripAlignMode.BottomLeft:
                    size.Width = -size.Height;
                    break;
            }
            // Reverse size grip for left-aligned.
            if (size.Width < 0)
                location.X -= size.Width;
            if (size.Height < 0)
                location.Y -= size.Height;
            g.DrawImage(GripBitmap, location.X, location.Y, size.Width, size.Height);
        }

        /// <summary>
        /// Renders Grip according to the specified Graphics, Point and Grip AlignMode.
        /// </summary>
        /// <param name="g">Graphics instance used to render the gri</param>
        /// <param name="location">Point specifing the location for the gri</param>
        /// <param name="mode">GripAlignMode specifying the alignment of the grip</param>
        public static void Render(Graphics g, Point location, GripAlignMode mode)
        {
            Render(g, location, new Size(16, 16), mode);
        }

        #endregion

        #region Properties

        private static Bitmap GripBitmap
        {
            get { return _sGripBitmap; }
        }

        #endregion

        #region Attributes

        private static Bitmap _sGripBitmap;

        #endregion
    }

    /// <summary>
    /// Interface used to publich all common methods relative to PopupControls of different sorts
    /// </summary>
    public interface IPopupControlHost
    {
        #region Methods

        /// <summary>
        /// Displays drop-down area of combo box, if not already shown.
        /// </summary>
        void ShowDropDown();

        /// <summary>
        /// Hides drop-down area of combo box, if shown.
        /// </summary>
        void HideDropDown();

        #endregion
    }

    internal class PopupControl
    {
        #region Construction and destruction

        public PopupControl()
        {
            InitializeDropDown();
        }

        #endregion

        #region Event handlers

        private void _dropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (AutoResetWhenClosed)
                DisposeHost();

            // Hide drop down within popup control.
            if (PopupControlHost != null)
                PopupControlHost.HideDropDown();
        }

        #endregion

        #region Events

        public event ToolStripDropDownClosingEventHandler Closing
        {
            add
            {
                _dropDown.Closing -= value;
                _dropDown.Closing += value;
            }
            remove
            {
                _dropDown.Closing -= value;
            }
        }

        #endregion

        #region Methods

        public void Show(Control control, int x, int y)
        {
            Show(control, x, y, PopupResizeMode.None);
        }

        public void Show(Control control, int x, int y, PopupResizeMode resizeMode)
        {
            Show(control, x, y, -1, -1, resizeMode);
        }

        public void Show(Control control, int x, int y, int width, int height, PopupResizeMode resizeMode)
        {
            ListView lv = control as ListView;
            ListViewItem prevFocused = null;
            if (lv != null)
                prevFocused = lv.FocusedItem;
            InitializeHost(control);
            _dropDown.ResizeMode = resizeMode;
            _dropDown.Show(x, y, width, height);
            control.Focus();
            if (lv != null && prevFocused != null)
            {
                lv.FocusedItem = prevFocused;
                lv.FocusedItem.Selected = true;
            }
        }

        public void Hide()
        {
            if (_dropDown != null && _dropDown.Visible)
            {
                _dropDown.Hide();
                DisposeHost();
            }
        }

        public void Reset()
        {
            DisposeHost();
        }

        #endregion

        #region Internal methods

        protected void DisposeHost()
        {
            if (_host != null)
            {
                // Make sure host is removed from drop down.
                if (_dropDown != null)
                    _dropDown.Items.Clear();

                // Dispose of host.
                _host = null;
            }

            PopupControlHost = null;
        }

        protected void InitializeHost(Control control)
        {
            InitializeDropDown();

            // If control is not yet being hosted then initialize host.
            if (control != Control)
                DisposeHost();

            // Create a new host?
            if (_host == null)
            {
                _host = new ToolStripControlHost(control);
                _host.AutoSize = false;
                _host.Padding = Padding;
                _host.Margin = Margin;
            }

            // Add control to drop-down.
            _dropDown.Items.Clear();
            _dropDown.Padding = _dropDown.Margin = Padding.Empty;
            _dropDown.Items.Add(_host);
        }

        protected void InitializeDropDown()
        {
            // Does a drop down exist?
            if (_dropDown == null)
            {
                _dropDown = new PopupDropDown(false);
                _dropDown.Closed +=new ToolStripDropDownClosedEventHandler(_dropDown_Closed);
            }
        }

        #endregion

        #region Properties

        public bool Visible
        {
            get { return (this._dropDown != null && this._dropDown.Visible) ? true : false; }
        }

        public Control Control
        {
            get { return (this._host != null) ? this._host.Control : null; }
        }

        public Padding Padding
        {
            get { return this._padding; }
            set { this._padding = value; }
        }

        public Padding Margin
        {
            get { return this._margin; }
            set { this._margin = value; }
        }

        public bool AutoResetWhenClosed
        {
            get { return this._autoReset; }
            set { this._autoReset = value; }
        }

        /// <summary>
        /// Gets or sets the popup control host, this is used to hide/show popup.
        /// </summary>
        IPopupControlHost _popControlHost;
        public IPopupControlHost PopupControlHost 
        { 
            get { return _popControlHost; } 
            set { _popControlHost = value; } 
        }

        #endregion

        #region Attributes

        private ToolStripControlHost _host;
        private PopupDropDown _dropDown;

        private Padding _padding = Padding.Empty;
        private Padding _margin = new Padding(1, 1, 1, 1);

        private bool _autoReset;

        #endregion
    }
}
