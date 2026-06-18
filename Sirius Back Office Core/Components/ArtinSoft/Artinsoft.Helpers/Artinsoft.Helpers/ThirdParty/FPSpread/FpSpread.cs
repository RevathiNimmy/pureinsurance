namespace Artinsoft.VBUpgrade.Spread
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Security.Cryptography;
    using System.Windows.Forms;
    using FarPoint.Win.Spread;
    using FarPoint.Win.Spread.CellType;
    using FarPoint.Win.Spread.Model;
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.Devices;
    using Appearance=FarPoint.Win.Spread.Appearance;
    using System.Reflection;

    /// <summary>
    /// Farpoint Helper Class for VB6 control equivalence
    /// </summary>
    public partial class FpSpread : FarPoint.Win.Spread.FpSpread
    {
        private int _Row;
        private int _Col;
        private int _Row2;
        private int _Col2;
        private IComparer _Comparer;
        private SortInfo[] _SortInfo;
        private string _ToolTipText;
        private Graphics graficsObject;

        private bool _RunLocalEvents;
        private int _ActiveColumnIndex = 0;
        private int _ActiveRowIndex = 0;
        private bool _EventClickCell = false;

        // remember the last 3 sort orders and active cell to be able to reinstate them 
        // last active cell is remembered on .Persist 
        // sort order and active cell restored on .Reinstate 

        private int _SortCol1;
        private int _SortCol2;
        private int _SortCol3;
        private SortKeyOrderConstants _SortKeyOrder1;
        private SortKeyOrderConstants _SortKeyOrder2;
        private SortKeyOrderConstants _SortKeyOrder3;
        private SortByConstants _SortBy;

        private ScrollBarPolicy _SavedHScrollBarPolicy;
        private ScrollBarPolicy _SavedVScrollBarPolicy;
        private ItemDataList<int> _RowItemData = new ItemDataList<int>(501);
        private ItemDataList<int> _ColItemData = new ItemDataList<int>(501);

        /// <summary>
        /// Change event handler
        /// </summary>
        public new event ChangeEventHandler Change;
        /// <summary>
        /// CellClick event handler
        /// </summary>
        public new event CellClickEventHandler CellClick;
        /// <summary>
        /// Cell Right click event handler
        /// </summary>
        public event CellClickEventHandler CellRightClick;
        /// <summary>
        /// Leave Cell event handler
        /// </summary>
        public new event LeaveCellEventHandler LeaveCell;
        /// <summary>
        /// Leave Row event handler
        /// </summary>
        public event LeaveRowEventHandler LeaveRow;
        /// <summary>
        /// TextTip Fetch Event handler
        /// </summary>
        public new event TextTipFetchEventHandler TextTipFetch;
        /// <summary>
        /// Top Left Change event handler
        /// </summary>
        public event TopLeftChangeEventHandler TopLeftChange;

        /// <summary>
        /// Constructor
        /// </summary>
        public FpSpread(): base()
        {
            InitializeSpread();
        }

        /// <summary>
        /// Get/Set Back color
        /// </summary>
        public new Color BackColor
        {
            get
            {
                StyleInfo si = GetCompositeInfo(this.Row, this.Col);
                if ((si != null))
                {
                    return si.BackColor;
                }
                else
                {
                    return Color.Empty;
                }
            }
            set
            {
                StyleInfo si = GetCompositeInfo(this.Row, this.Col);
                if ((si != null) && si.BackColor.ToArgb() != value.ToArgb())
                {
                    switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                    {
                        case "ao":
                            {
                                ((StyleInfo) GetPropertyAtRowCol(this.Row, this.Col)).BackColor = value;
                                break;
                            }
                        case "Row":
                            {
                                ((Row) GetPropertyAtRowCol(this.Row, this.Col)).BackColor = value;
                                break;
                            }
                        case "Column":
                            {
                                ((Column) GetPropertyAtRowCol(this.Row, this.Col)).BackColor = value;
                                break;
                            }
                        case "Cell":
                            {
                                ((Cell) GetPropertyAtRowCol(this.Row, this.Col)).BackColor = value;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set Active Row Index. This property is 1-based
        /// </summary>
        public int ActiveRowIndex
        {
            get
            {
                if(this.ActiveSheet == null)
                    return -1;
                if (_EventClickCell)
                    return _ActiveRowIndex + 1;
                else
                {
                    return this.ActiveSheet.ActiveRowIndex + 1;                    
                }
            }
            set
            {
                if (this.ActiveSheet != null)
                {
                    if(_EventClickCell)
                        _ActiveRowIndex = value - 1;
                    else
                        this.ActiveSheet.ActiveRowIndex = value - 1;
                    
                }
            }
        }

        /// <summary>
        /// Get/Set Active column index. This property is 1-based
        /// </summary>        
        public int ActiveColumnIndex
        {
            get
            {
                if(this.ActiveSheet == null)
                    return -1;
                if(_EventClickCell)
                    return _ActiveColumnIndex + 1;
                else
                    return this.ActiveSheet.ActiveColumnIndex + 1;
            }
            set
            {
                if (this.ActiveSheet != null)
                {
                    if (_EventClickCell)
                        _ActiveColumnIndex = value - 1;
                    else
                        this.ActiveSheet.ActiveColumnIndex = value - 1;                    
                }
            }
        }

        /// <summary>
        /// Initialize Sheet View
        /// </summary>
        /// <param name="sheet">SheetView to set</param>
        public void InitializeSheetView(SheetView sheet)
        {
            sheet.Rows.Default.Height = 15;
            sheet.ColumnHeader.Rows.Default.Height = 15;
            sheet.ColumnHeaderVisible = true;
            sheet.RowHeaderVisible = true;
            sheet.GrayAreaBackColor = SystemColors.Control;
            sheet.SelectionStyle = FarPoint.Win.Spread.SelectionStyles.None;

            Appearance app = new Appearance();
            sheet.GetStyleInfo(0, 0).GetAppearance(app);
            app.SelectionForeColor = SystemColors.HighlightText;
            app.SelectionBackColor = SystemColors.Highlight;

            sheet.ActiveSkin = FarPoint.Win.Spread.DefaultSkins.Classic;
            sheet.ColumnHeader.DefaultStyle.HorizontalAlignment = CellHorizontalAlignment.Left;
            sheet.ColumnHeader.DefaultStyle.VerticalAlignment = CellVerticalAlignment.Top;
        }

        /// <summary>
        /// Initialize default parameters for FP Spread
        /// </summary>
        public void InitializeSpread()
        {
            // Newer style grids run common events for resizing, sorting, exporting, 
            // etc. Older style grids set this to false and have their own custom events 
            _RunLocalEvents = true;

            //Initialise properties for remebering sort order, last active cell 
            _SortCol1 = 0;
            _SortCol2 = 0;
            _SortCol3 = 0;

            //Me.MaxRows = 0 
            Row = -1;
            Col = -1;
            ReDraw = true;
            FocusRenderer = new SolidFocusIndicatorRenderer();
            // These features on spread don't seem to work 
            // so use the Windows default for now 
            CausesValidation = true;
            //.TabStop = False 
            //.ProcessTab = True 
            TextTipDelay = 250;
            
            BorderStyle = BorderStyle.None;
            EditModeReplace = true;
            //.NoBeep = True 
            HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            VerticalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            ScrollBarTrackPolicy = ScrollBarTrackPolicy.Both;
            
            //.BackColorStyle = BackColorStyleConstants.BackColorStyleUnderGrid 
            //.SortBy = FPSpreadADO.SortByConstants.SortByRow 
            ClipboardOptions = ClipboardOptions.AllHeaders;
            // copy row and column headers, do not paste them 
            SetCursor(CursorType.LockedCell, Cursors.Default);
            SetCursor(CursorType.Normal, Cursors.Default);

            TipAppearance TipAppearance1 = new TipAppearance();
            TipAppearance1.BackColor = SystemColors.Info;
            //TipAppearance1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point,
            TipAppearance1.Font = new Font("MS Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point,
                                           (byte)0);
            TipAppearance1.ForeColor = SystemColors.InfoText;
            this.TextTipAppearance = TipAppearance1;

            
            _Comparer = new CellComparer();
            _SortInfo = new SortInfo[]
                            {
                                new SortInfo(-1, false, _Comparer), new SortInfo(-1, false, _Comparer),
                                new SortInfo(-1, false, _Comparer)
                            };
            
            Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;

            base.Change += new FarPoint.Win.Spread.ChangeEventHandler(FpSpread_Change);
            base.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(FpSpread_CellClick);
            base.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(FpSpread_LeaveCell);
            base.TextTipFetch += new FarPoint.Win.Spread.TextTipFetchEventHandler(FpSpread_TextTipFetch);
            base.LeftChange += new LeftChangeEventHandler(FpSpread_LeftChange);
            base.TopChange += new TopChangeEventHandler(FpSpread_TopChange);
        }

        //TODO: to be removed
        void Sheets_Changed(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action == CollectionChangeAction.Add)
            {
                this.InitializeSheetView((SheetView) e.Element);
            }
        }

        private object GetPropertyAtRowCol(int Row, int Col)
        {
            object Prop = null;
            try
            {
                if (ActiveSheet != null)
                {
                    if (Col == -1 & Row == -1)
                    {
                        Prop = this.ActiveSheet.DefaultStyle;
                    }
                    else if (Col == -1 & Row == 0)
                    {
                        Prop = this.ActiveSheet.ColumnHeader.Rows[Row];
                    }
                    else if (ValidCol(Col) & Row == 0)
                    {
                        Prop = this.ActiveSheet.ColumnHeader.Columns[Col - 1];
                    }
                    else if (Col == -1 & ValidRow(Row))
                    {
                        Prop = this.ActiveSheet.Rows[Row - 1];
                    }
                    else if (Col == 0 & Row == -1)
                    {
                        Prop = this.ActiveSheet.RowHeader.Columns[Col];
                    }
                    else if (Col == 0 & ValidRow(Row))
                    {
                        Prop = this.ActiveSheet.RowHeader.Rows[Row - 1];
                    }
                    else if (Row == -1 & ValidCol(Col))
                    {
                        Prop = this.ActiveSheet.Columns[Col - 1];
                    }
                    else if (ValidRowAndCol(Row, Col))
                    {
                        Prop = this.ActiveSheet.Cells[Row - 1, Col - 1];
                    }
                    else if (Col == 0 & Row == 0)
                    {
                        Prop = this.ActiveSheet.ColumnHeader.Cells[Row, Col];
                    }
                }
            }
            catch
            {
            }
            return Prop;
        }

        private StyleInfo GetCompositeInfo(int Row, int Col)
        {
            StyleInfo si = null;
             if (ActiveSheet != null)
                {
                    if (Row == 0 & Col == -1)
                    {
                        si = this.ActiveSheet.Models.ColumnHeaderStyle.GetCompositeInfo(Row, Col, 0, null);
                    }
                    else if (Row == 0 & ValidCol(Col))
                    {
                        si = this.ActiveSheet.Models.ColumnHeaderStyle.GetCompositeInfo(Row, Col - 1, 0, null);
                    }
                    else if (Row == -1 & Col == 0)
                    {
                        si = this.ActiveSheet.Models.RowHeaderStyle.GetCompositeInfo(Row, Col, 0, null);
                    }
                    else if (ValidRow(Row) & Col == 0)
                    {
                        si = this.ActiveSheet.Models.RowHeaderStyle.GetCompositeInfo(Row - 1, Col, 0, null);
                    }
                    else if (Row <= MaxRows & Col <= MaxCols)
                    {
                        if (Row > 0)
                        {
                            Row = Row - 1;
                        }
                        if (Col > 0)
                        {
                            Col = Col - 1;
                        }
                        si = this.ActiveSheet.Models.Style.GetCompositeInfo(Row, Col, 0, null);
                    }
                }
            
            return si;
        }
        
        /// <summary>
        /// Get/Set number of Rows
        /// </summary>
        public int Row
        {
            get { return this._Row; }
            set
            {
                //We should not set the Row to 0 since, because the user might messup the Row Header by accident.
                if (value > this.MaxRows && this.MaxRows > 0)
                {
                    this._Row = this.MaxRows;
                }
                else
                {
                    this._Row = value;
                }
            }
        }

        /// <summary>
        /// Get/Set number of Columns
        /// </summary>
        public int Col
        {
            get { return this._Col; }
            set
            {
                if (value > this.MaxCols && this.MaxCols > 0)
                {
                    this._Col = this.MaxCols;
                }
                else
                {
                    this._Col = value;
                }
            }
        }

        /// <summary>
        /// Get/Set number of Rows 2
        /// </summary>
        public int Row2
        {
            get { return this._Row2; }
            set
            {
                if (value > this.MaxRows && this.MaxRows > 0)
                {
                    this._Row2 = this.MaxRows;
                }
                else
                {
                    this._Row2 = value;
                }
            }
        }

        /// <summary>
        /// Get/Set number of Columns 2
        /// </summary>
        public int Col2
        {
            get { return this._Col2; }
            set
            {
                if (value > this.MaxCols && this.MaxCols > 0)
                {
                    this._Col2 = this.MaxCols;
                }
                else
                {
                    this._Col2 = value;
                }
            }
        }

        /// <summary>
        /// Validates if a Row,Col pair is within the bounds of the Grid 
        /// </summary>
        /// <param name="Row">Row to validate</param>
        /// <param name="Col">Column to validate</param>
        /// <returns></returns>
        private bool ValidRowAndCol(int Row, int Col)
        {
            return ValidRow(Row) && ValidCol(Col);
        }

        private bool ValidRow(int Row)
        {
            return Row > 0 && Row <= MaxRows;
        }

        /// <summary>
        /// Left Column is obsolete in FpSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int LeftCol
        {
            get { return 1; }

            set { }
        }

        private bool ValidCol(int Col)
        {
            return Col > 0 && Col <= MaxCols;
        }

        /// <summary>
        /// Get/Set Max of Columns
        /// </summary>
        public int MaxCols
        {
            get { return ((this.ActiveSheet != null) ? this.ActiveSheet.ColumnCount : 0); }
            set
            {
                if (this.ActiveSheet != null)
                {
                    this.ActiveSheet.ColumnCount = value;
                    AdjustScrollBars();
                }
            }
        }

        /// <summary>
        /// Get/Set Max of Rows
        /// </summary>
        public int MaxRows
        {
            get { return ((this.ActiveSheet != null) ? this.ActiveSheet.RowCount : 0); }
            set
            {
                if (this.ActiveSheet != null)
                {
                    CellRange[] cellRangeList = new CellRange[0];
                    if (value < MaxRows && value > 0)
                    {
                        cellRangeList  = ActiveSheet.GetSelections();
                    }
                    this.ActiveSheet.RowCount = value;
                    AdjustScrollBars();
                    //
                    if (cellRangeList.Length > 0)
                    {
                        SaveSelectionRows(cellRangeList, value);
                    }
                }
            }
        }

        private void SaveSelectionRows(CellRange[] cellRangeList, int rowDelete)
        {
            if (cellRangeList.Length > 0 && ActiveSheet.RowCount > 0)
            {
                switch (ActiveSheet.OperationMode)
                {
                    case OperationMode.RowMode:
                    case OperationMode.SingleSelect:
                        if (cellRangeList[0].Row >= ActiveSheet.RowCount)
                        {
                            ActiveSheet.AddSelection(ActiveSheet.RowCount - 1, cellRangeList[0].Column, 1, cellRangeList[0].ColumnCount);   
                        }
                        break;
                    case OperationMode.Normal:
                    case OperationMode.ReadOnly:
                    case OperationMode.ExtendedSelect:
                        foreach (CellRange cellRange in cellRangeList)
                        {
                            if ((cellRange.Row <= rowDelete - 1) && ((cellRange.Row + cellRange.RowCount) > rowDelete - 1) && cellRange.RowCount > 1)
                            {
                                ActiveSheet.AddSelection(cellRange.Row, cellRange.Column,Math.Min((cellRange.RowCount - 1), (ActiveSheet.RowCount - cellRange.Row)) , cellRange.ColumnCount);
                            }
                            //if ((cellRange.Row <= value - 1) && ((cellRange.Row + cellRange.RowCount) > value - 1) && cellRange.RowCount > 1)
                            //{
                            //    ActiveSheet.AddSelection(cellRange.Row, cellRange.Column, value - cellRange.Row, cellRange.ColumnCount);
                            //}
                        }
                        break;
                    default:
                        //case OperationMode.MultiSelect:
                        ;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets Row Height
        /// </summary>
        /// <param name="Row">row to look at</param>
        /// <returns></returns>
        public double GetRowHeight(int Row)
        {
            //if (ActiveSheet != null)
            {
            if (Row == 0)
            {
                return this.ActiveSheet.ColumnHeader.Rows[Row].Height;
            }
            else if ((Row == -1))
            {
                return this.ActiveSheet.Rows.Default.Height;
            }
            else if (Row > 0)
            {
                return this.ActiveSheet.Rows[Row - 1].Height;
            }
            }
            return 0;
        }

        /// <summary>
        /// Gets Cell Height
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <param name="Col">Column to look at</param>
        /// <returns></returns>
        public float GetCellHeight(int Row,  int Col)
        {
            string s = this.Value.ToString();
            
            Font defaultFont = null;
            defaultFont = this.ActiveSheet.Cells[Row, Col].Font;
            if (defaultFont == null)
                defaultFont = this.ActiveSheet.Rows[Row].Font;
            
            if (defaultFont == null)
                defaultFont = this.Font;
            if (graficsObject != null)
                graficsObject = CreateGraphics();

            SizeF sf = graficsObject.MeasureString(s, defaultFont);
            return sf.Height;
        }

        /// <summary>
        /// This method is the equivalent to. Backcolor of farpoint 3.0
        /// </summary>
        /// <param name="setColor"></param>
        public void SetBackColor(Color setColor)
        {
            int endCol = (this.Col2 == -1) ? this.MaxCols : this.Col2;
            int endRow = (this.Row2 == -1) ? this.MaxRows : this.Row2;
            int startRow = this.Row;
            int startCol = this.Col;

            for (int row = startRow; row <= endRow; row++)
            {
                for (int col = startCol; col <= endCol; col++)
                {
                    this.Col = col;
                    this.Row = row;
                    this.BackColor = setColor;
                }
            }
        }

        /// <summary>
        /// Returns the Preferred RowHeight
        /// This method might behave diferently than the VB6 version of it.
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <returns></returns>
        public float MaxTextRowHeight(int Row)
        {
            if (Row == 0 || Row == -1)
            {
                return this.ActiveSheet.GetPreferredRowHeight(Row);
            }
            else
            {
                return this.ActiveSheet.GetPreferredRowHeight(Row - 1, true);
            }
        }
        /// <summary>
        /// Sets the Height for a row using the value
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <param name="Value">Value to set</param>
        public void SetRowHeight(int Row, float Value)
        {

            if (Row == 0)
            {
                this.ActiveSheet.ColumnHeader.Rows[Row].Height = Value * 1.05F;
            }
            else if ((Row == -1))
            {
                this.ActiveSheet.Rows.Default.Height = Value * 1.05F;
            }
            else if (Row > 0)
            {
              this.ActiveSheet.Rows[Row - 1].Height = Value * 1.05F;
            }
        }

        /// <summary>
        /// Gets Column Width
        /// </summary>
        /// <param name="Col">Column to look at</param>
        /// <returns></returns>
        public float GetColWidth(int Col)
        {
            float Width = 0;
            if (Col == 0)
            {
                Width = this.ActiveSheet.RowHeader.Columns[Col].Width;
            }
            else if (ValidCol(Col))
            {
                Width = this.ActiveSheet.GetColumnWidth(Col - 1);
            }
            return ColWidthFromPixels(Width);
        }

        /// <summary>
        /// Sets Column Width
        /// </summary>
        /// <param name="Col">Column to look at</param>
        /// <param name="Value">Value to set</param>
        public void SetColWidth(int Col, float Value)
        {
            if (Col == 0)
            {
                this.ActiveSheet.RowHeader.Columns[Col].Width = ColWidthToPixels(Value);
            }
            else if (ValidCol(Col))
            {
                this.ActiveSheet.SetColumnWidth(Col - 1, Convert.ToInt32(ColWidthToPixels(Value)));
            }
        }

        /// <summary>
        /// Gets hidden condition for a specific row
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <returns></returns>
        public bool GetRowHidden(int Row)
        {

            if (Row == 0)
            {
                return !this.ActiveSheet.ColumnHeader.Visible;
            }
            else if (Row > 0)
            {
                return !this.ActiveSheet.Rows[Row - 1].Visible;
            }
            return false;
        }

        /// <summary>
        /// Sets hidden state for a specific row
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <param name="Value">Value to set</param>
        public void SetRowHidden(int Row, bool Value)
        {
            if (Row == 0)
            {
                this.ActiveSheet.ColumnHeader.Visible = !Value;
            }
            else if (Row > 0)
            {
                this.ActiveSheet.Rows[Row - 1].Visible = !Value;
            }

        }

        /// <summary>
        /// Get/Set Row Hidden state, using the actual Row value
        /// </summary>
        public bool RowHidden
        {
            get { return GetRowHidden(Row); }
            set { SetRowHidden(Row, value); }
        }

        /// <summary>
        /// Gets Column Hidden state for a specific Column
        /// </summary>
        /// <param name="Col">Column to look at</param>
        /// <returns>True if column specified has visible false state</returns>
        public bool GetColHidden(int Col)
        {
            if (Col == 0)
            {
                return !this.ActiveSheet.RowHeader.Visible;
            }
            else if (ValidCol(Col))
            {
                return !this.ActiveSheet.Columns[Col - 1].Visible;
            }
            return false;
        }

        /// <summary>
        /// Sets Column Hidden state for specified column
        /// </summary>
        /// <param name="Col">Column to look at</param>
        /// <param name="Value">Value to set</param>
        public void SetColHidden(int Col, bool Value)
        {
            if (Col == 0)
            {
                this.ActiveSheet.RowHeader.Visible = !Value;
            }
            else if (ValidCol(Col))
            {
                this.ActiveSheet.Columns[Col - 1].Visible = !Value;
            }
        }

        /// <summary>
        /// Get/Set Column Hidden state
        /// </summary>
        public bool ColHidden
        {
            get { return GetColHidden(Col); }
            set { SetColHidden(Col, value); }
        }

        /// <summary>
        /// full width of the grid object (in pixels) based on displayed column widths 
        /// </summary>
        public float GridWidth
        {
            get
            {
                float result = 0;

                int w;

                for (int Col = 0; Col <= this.ActiveSheet.ColumnCount; Col++)
                {
                    if (!GetColHidden(Col))
                    {
                        result += ColWidthToPixels(GetColWidth(Col)) + 1;
                    }
                }

                //The Spread COM GetClientArea method returned the area of the control excluding the scroll bars; 
                //the ClientRectangle method in .NET returns the area of the component including the scroll bars 
                w = this.ClientRectangle.Width;

                if (this.Width > w)
                {
                    result += this.Width - w;
                    // adjust for vertical scrollbar width 
                }
                return result;
            }
        }
        
        /// <summary>
        /// Gets Last non empty Row using the NonEmptyItemFlag
        /// </summary>
        /// <param name="dataFlag">NonEmptyItemFlag to use</param>
        /// <returns></returns>
        public int GetLastNonEmptyRow(NonEmptyItemFlag dataFlag)
        {
            return this.ActiveSheet.GetLastNonEmptyRow(dataFlag) + 1;
        }

        /// <summary>
        /// Gets Last non Empty Column using the NonEmptyItemFlag
        /// </summary>
        /// <param name="dataFlag">NonEmptyItemFlag to use</param>
        /// <returns></returns>
        public int GetLastNonEmptyColumn(NonEmptyItemFlag dataFlag)
        {
            return this.ActiveSheet.GetLastNonEmptyColumn(dataFlag) + 1;
        }

        /// <summary>
        /// Converts a pixel width to Column Width
        /// </summary>
        /// <param name="Width">value to convert</param>
        /// <returns></returns>
        public static float ColWidthFromPixels(float Width)
        {
            return Width / GetFixedFont().SizeInPoints;
        }

        /// <summary>
        /// Convert Width Column to Pixels
        /// </summary>
        /// <param name="Width">value to convert</param>
        /// <returns></returns>
        public static float ColWidthToPixels(float Width)
        {
            return GetFixedFont().SizeInPoints * Width;
        }

        private static Font fixedFont = null;

        private static Font GetFixedFont()
        {
            if (fixedFont == null)
            {
                fixedFont = new Font("Courier", float.Parse(8.25.ToString()));
            }
            return fixedFont;
        }

        private void AdjustScrollBars()
        {
            //This method is a workaround for a bug in the Spread component. When ScrollBarPolicy = AsNeeded and 
            //the spread's height is less than 4 rows the vertical and horizontal scrollbars dissapear. This was 
            //confirmed by FarPoint as a bug and in theory will be fixed in a future release (later than 3.0.2005.2005) 
            if (!(HorizontalScrollBarPolicy == ScrollBarPolicy.Never))
            {
                int Col = this.Col;
                int Row = this.Row;
                float height = this.ActiveSheet.ColumnHeader.Rows[0].Height + this.ActiveSheet.Rows.Default.Height * 4;
                if (HorizontalScrollBarPolicy == ScrollBarPolicy.AsNeeded & base.ClientRectangle.Width < GridWidth &
                    base.ClientRectangle.Height < height)
                {
                    HorizontalScrollBarPolicy = ScrollBarPolicy.Always;
                    _SavedHScrollBarPolicy = ScrollBarPolicy.AsNeeded;
                }
                else if (base.ClientRectangle.Width >= GridWidth & _SavedHScrollBarPolicy == ScrollBarPolicy.AsNeeded &
                         HorizontalScrollBarPolicy == ScrollBarPolicy.Always)
                {
                    HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
                }

                float height2 = this.ActiveSheet.ColumnHeader.Rows[0].Height +
                                this.ActiveSheet.Rows.Default.Height * MaxRows;
                if (VerticalScrollBarPolicy == ScrollBarPolicy.AsNeeded & base.ClientRectangle.Height < height &
                    base.ClientRectangle.Height < height2)
                {
                    VerticalScrollBarPolicy = ScrollBarPolicy.Always;
                    _SavedVScrollBarPolicy = ScrollBarPolicy.AsNeeded;
                }
                else if (base.ClientRectangle.Height >= height2 & _SavedVScrollBarPolicy == ScrollBarPolicy.AsNeeded &
                         VerticalScrollBarPolicy == ScrollBarPolicy.Always)
                {
                    VerticalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
                }
                this.Col = Col;
                this.Row = Row;
            }
        }

        /// <summary>
        /// Sets Active Cell
        /// </summary>
        /// <param name="Row">row to look at</param>
        /// <param name="Col">column to look at</param>
        /// <param name="VPosition">vertical position</param>
        /// <param name="HPosition">horizontal position</param>
        public void SetActiveCell(int Row, int Col, VerticalPosition VPosition, HorizontalPosition HPosition)
        {
            SetActiveCell(Row, Col);
            this.ShowActiveCell(VPosition, HPosition);
        }

        /// <summary>
        /// Sets Active Cell
        /// </summary>
        /// <param name="Row">row to look at</param>
        /// <param name="Col">column to look at</param>
        public void SetActiveCell(int Row, int Col)
        {
            if (Row == 0)
            {
                // Don't allow the active cell to be in the column headers 
                this.Row = 1;
                //this.ActiveRowIndex = 1;
                //activecolunmIndex = -1 ?? TODO
            }
            else
            {
                this.Row = Row;
            }
            this.Col = Col;
            this.ActiveSheet.SetActiveCell(this.Row - 1, this.Col - 1);
        }

        /// <summary>
        /// Clear Range
        /// </summary>
        /// <param name="Row">row to look at</param>
        /// <param name="Col">column to look at</param>
        public void ClearRange(int Row, int Col)
        {
            if ((Row > 0 & Col == -1))
            {
                this.ActiveSheet.ClearRange(Row - 1, 0, 1, this.MaxCols, true);
            }
            else if (Row == -1 & Col > 0)
            {
                this.ActiveSheet.ClearRange(0, Col - 1, this.MaxRows, 1, true);
            }
        }

        /// <summary>
        /// Clear Range
        /// </summary>
        /// <param name="Row1">Left Row to look at</param>
        /// <param name="Col1">Top column to look at</param>
        /// <param name="Row2">Right Row to look at</param>
        /// <param name="Col2">Bottom column to look at</param>
        /// <param name="DataOnly">if only data</param>
        public void ClearRange(int Row1, int Col1, int Row2, int Col2, bool DataOnly)
        {
            this.ActiveSheet.ClearRange(Row1 - 1, Col1 - 1, Row2 - Row1 + 1, Col2 - Col1 + 1, DataOnly);
        }

        /// <summary>
        /// Clears selection
        /// </summary>
        public void ClearSelection()
        {
            this.ActiveSheet.ClearSelection();
        }

        /// <summary>
        /// Sets Selection
        /// </summary>
        /// <param name="Col">Top Column</param>
        /// <param name="Row">Left Row</param>
        /// <param name="Col2">Bottom Column</param>
        /// <param name="Row2">Right Row</param>
        public void SetSelection(int Col, int Row, int Col2, int Row2)
        {
            if ((Row == -1 | Row2 >= Row) & (Col == -1 | Col2 >= Col))
            {
                ActiveSheet.AddSelection((Row == -1 ? Row : Row - 1), (Col == -1 ? Col : Col - 1), Row2 - Row + 1,
                                              Col2 - Col + 1);
            }
            else
            {
                ClearSelection();
            }
        }        

        /// <summary>
        /// Gets Selection
        /// </summary>
        /// <param name="BlockNo">Block number</param>
        /// <param name="Col">returned top column</param>
        /// <param name="Row">returned left row</param>
        /// <param name="Col2">returned bottom column</param>
        /// <param name="Row2">returned right row</param>
        public void GetSelection(int BlockNo, ref int Col, ref int Row, ref int Col2, ref int Row2)
        {
            CellRange selection = ActiveSheet.GetSelection(BlockNo);

            if ((selection != null))
            {
                Col = selection.Column + 1;
                Row = selection.Row + 1;
                Col2 = selection.Column + selection.ColumnCount;
                Row2 = selection.Row + selection.RowCount;
            }
            else
            {
                Col = -1;
                Row = -1;
                Col2 = -1;
                Row2 = -1;
            }
        }

        /// <summary>
        /// Gets Multi Selection items
        /// </summary>
        /// <param name="SelPrev">Selected Previous</param>
        /// <returns></returns>
        public int GetMultiSelItem(int SelPrev)
        {
            int ret = -1;
            List<CellRange> rangeOfSelections = new List<CellRange>();
            rangeOfSelections.AddRange(this.ActiveSheet.GetSelections());
            rangeOfSelections.Sort(delegate(CellRange cellRange1, CellRange cellRange2)
                {
                    return cellRange1.Row.CompareTo(cellRange2.Row);
                });

            for (int i = 0; i < rangeOfSelections.Count; ++i)
            {
                if (SelPrev == 0)
                {
                    ret = rangeOfSelections[i].Row + 1;
                    break;
                }

                if ((rangeOfSelections[i].Row + rangeOfSelections[i].RowCount + 1) > SelPrev)
                {
                    if ((rangeOfSelections[i].Row + rangeOfSelections[i].RowCount) == SelPrev)
                    {
                        if (rangeOfSelections.Count > i + 1)
                        {
                            ret = rangeOfSelections[i + 1].Row + 1;
                        }
                        break;
                    }
                    ret = SelPrev + 1;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Delete Row
        /// </summary>
        /// <param name="Row">row to delete</param>
        public void DeleteRow(int Row)
        {
            if (ValidRow(Row))
            {
                CellRange[] cellRangeList = this.ActiveSheet.GetSelections();
                ActiveSheet.Rows[Row - 1].Remove();
                this.Row = Math.Min(MaxRows, Row);
                //It maintains the selection of the rows and columns
                SaveSelectionRows(cellRangeList, Row);
            }
        }

        /// <summary>
        /// Insert a row after indicated Row
        /// </summary>
        /// <param name="Row">Row number</param>
        /// <returns></returns>
        public int InsertRow(int Row)
        {
            //Inserts after the indicated row 
            this.Row = Row + 1;
            if (Row == MaxRows)
            {
                MaxRows += 1;
            }
            else
            {
                ActiveSheet.Rows[Row].Add();
            }
            return this.Row;
        }

        /// <summary>
        /// Sets Action from Action Constants
        /// </summary>
        public ActionConstants Action
        {
            set
            {
                switch (value)
                {
                    case ActionConstants.ActionActiveCell:
                        SetActiveCell(Row, Col);
                        if (ActiveSheet.OperationMode == OperationMode.ExtendedSelect || 
                            ActiveSheet.OperationMode == OperationMode.MultiSelect ||
                            ActiveSheet.OperationMode == OperationMode.SingleSelect ||
                            ActiveSheet.OperationMode == OperationMode.RowMode)
                        {
                            ActiveSheet.AddSelection(Row-1, 0, 1, MaxCols);
                            ActiveColumnIndex = Col;
                            ActiveRowIndex = Row;
                        }
                        break;
                    case ActionConstants.ActionGotoCell:
                        SetActiveCell(Row, Col, VerticalPosition.Top, HorizontalPosition.Right);
                        break;
                    case ActionConstants.ActionSelectBlock:
                        SetSelection(Col, Row, Col2, Row2);
                        break;
                    case ActionConstants.ActionClear:
                        ClearRange(Row, Col, Row2, Col2, false);
                        break;
                    case ActionConstants.ActionClearText:
                        ClearRange(Row, Col);
                        break;
                    case ActionConstants.ActionClipboardCopy:
                        ActiveSheet.ClipboardCopy();
                        break;
                    case ActionConstants.ActionClipboardPaste:
                        ActiveSheet.ClipboardPaste();
                        break;
                    case ActionConstants.ActionDeselectBlock:
                        ActiveSheet.ClearSelection();
                        break;
                    case ActionConstants.ActionInsertCol:
                        ActiveSheet.Columns[Col - 1].Add();
                        //Need to decrement MaxCols because it gets incremented twice: first by the client before inserting and then here by Columns.Add() 
                        MaxCols -= 1;
                        break;
                    case ActionConstants.ActionInsertRow:
                        //Inserts before the indicated row 
                        ActiveSheet.Rows[Row - 1].Add();
                        //Need to decrement MaxRows because it gets incremented twice: first by the client before inserting and then here by Rows.Add() 
                        MaxRows -= 1;
                        break;
                    case ActionConstants.ActionDeleteRow:
                        DeleteRow(Row);
                        //Need to increment MaxRows because it gets decremented twice: first here by DeleteRow and then by the client after the call to Action 
                        MaxRows += 1;
                        break;
                    case ActionConstants.ActionSmartPrint:
                        PrintSheet(ActiveSheet);
                        break;
                    case ActionConstants.ActionSort:
                        bool sort = SortBy == SortByConstants.SortByRow ? true : false;
                        ActiveSheet.SortRange(Row - 1, Col - 1, MaxRows, MaxCols, sort, _SortInfo);
                        break;
                    default:
                        throw new ArgumentException(String.Format("Action {0} is not implemented", value.ToString()));
                }
            }
        }

        /// <summary>
        /// Gets Cell Type for specified row and column
        /// </summary>
        /// <param name="Row">row to look at</param>
        /// <param name="Col">column to look at</param>
        /// <returns>ICellType</returns>
        public ICellType GetCellType(int Row, int Col)
        {
            StyleInfo si = GetCompositeInfo(Row, Col);
            if (si != null)
            {
                return si.CellType;
            }
            return null;
        }

        /// <summary>
        /// Sets Cell Type for row and column
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <param name="Col">Column to look at</param>
        /// <param name="Value">value to set</param>
        public void SetCellType(int Row, int Col, ICellType Value)
        {
            StyleInfo si = GetCompositeInfo(Row, Col);
            if (si != null)
            {
                BaseCellType t1 = (BaseCellType) si.CellType;
                BaseCellType t2 = (BaseCellType) Value;
                if ((t1 != null) && (!ReferenceEquals(t1.GetType(), t2.GetType())))
                {
                    switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                    {
                        case "ao":
                            {
                                ((StyleInfo) GetPropertyAtRowCol(Row, Col)).CellType = Value;
                                break;
                            }
                        case "Row":
                            {
                                ((Row) GetPropertyAtRowCol(Row, Col)).CellType = Value;
                                break;
                            }
                        case "Column":
                            {
                                ((Column) GetPropertyAtRowCol(Row, Col)).CellType = Value;
                                break;
                            }
                        case "Cell":
                            {
                                ((Cell) GetPropertyAtRowCol(Row, Col)).CellType = Value;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set CellType for actual Row and Column
        /// </summary>
        public ICellType CellType
        {
            get { return GetCellType(Row, Col); }
            set { SetCellType(Row, Col, value); }
        }

        /// <summary>
        /// Get/Set FontName to use
        /// </summary>
        public string FontName
        {
            get
            {
                StyleInfo si = GetCompositeInfo(this.Row, this.Col);
                Font t1 = null;
                if (si != null)
                {
                    t1 = si.Font;
                }
                if (t1 == null)
                {
                    t1 = new Font(Font.Name, Font.Size);
                    object prop = GetPropertyAtRowCol(this.Row, this.Col);
                    if (prop != null)
                    {
                        switch (prop.GetType().Name)
                        {
                            case "ao":
                                {
                                    ((StyleInfo) prop).Font = t1;
                                    break;
                                }
                            case "Row":
                                {
                                    ((Row) prop).Font = t1;
                                    break;
                                }
                            case "Column":
                                {
                                    ((Column) prop).Font = t1;
                                    break;
                                }
                            case "Cell":
                                {
                                    ((Cell) prop).Font = t1;
                                    break;
                                }
                        }
                    }
                }
                return t1.Name;
            }
            set
            {
                StyleInfo si = GetCompositeInfo(this.Row, this.Col);
                if (si != null)
                {
                    Font t1 = si.Font;
                    if (t1 == null)
                    {
                        t1 = new Font(value, Font.Size);
                        switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                        {
                            case "ao":
                                {
                                    ((StyleInfo) GetPropertyAtRowCol(this.Row, this.Col)).Font = t1;
                                    break;
                                }
                            case "Row":
                                {
                                    ((Row) GetPropertyAtRowCol(this.Row, this.Col)).Font = t1;
                                    break;
                                }
                            case "Column":
                                {
                                    ((Column) GetPropertyAtRowCol(this.Row, this.Col)).Font = t1;
                                    break;
                                }
                            case "Cell":
                                {
                                    ((Cell) GetPropertyAtRowCol(this.Row, this.Col)).Font = t1;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        Font t2 = new Font(value, t1.Size);
                        if (! t1.Name.Equals(t2.Name))
                        {
                            switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                            {
                                case "ao":
                                    {
                                        ((StyleInfo) GetPropertyAtRowCol(this.Row, this.Col)).Font = t2;
                                        break;
                                    }
                                case "Row":
                                    {
                                        ((Row) GetPropertyAtRowCol(this.Row, this.Col)).Font = t2;
                                        break;
                                    }
                                case "Column":
                                    {
                                        ((Column) GetPropertyAtRowCol(this.Row, this.Col)).Font = t2;
                                        break;
                                    }
                                case "Cell":
                                    {
                                        ((Cell) GetPropertyAtRowCol(this.Row, this.Col)).Font = t2;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set Grid Color
        /// </summary>
        public Color GridColor
        {
            get { return ((this.ActiveSheet != null) ? this.ActiveSheet.HorizontalGridLine.Color : Color.LightGray); }
            set
            {
                if (this.ActiveSheet != null)
                {
                    GridLine line = new GridLine(GridLineType.Flat, value);
                    this.ActiveSheet.HorizontalGridLine = line;
                    this.ActiveSheet.VerticalGridLine = line;
                }
            }
        }

        /// <summary>
        /// Get/Set Shadow Dark
        /// </summary>
        public Color ShadowDark
        {
            get { return ((this.ActiveSheet != null) ? this.ActiveSheet.HorizontalGridLine.ShadowColor : Color.LightGray); }
            set
            {
                if (this.ActiveSheet != null)
                {
                    GridLine line = new GridLine(GridLineType.Flat, this.ActiveSheet.HorizontalGridLine.Color, this.ActiveSheet.HorizontalGridLine.HighlightColor, value, this.ActiveSheet.HorizontalGridLine.Width);
                    this.ActiveSheet.HorizontalGridLine = line;
                    this.ActiveSheet.VerticalGridLine = line;
                }
            }
        }

        private GridLine m_HGridLineWidth = new GridLine(GridLineType.Flat);

        /// <summary>
        /// Get/Set Grid Show Horizontal Grid Lines
        /// </summary>
        [DefaultValue(true)]
        public bool GridShowHoriz
        {
            get
            {
                return ((this.ActiveSheet != null)
                            ? (this.ActiveSheet.HorizontalGridLine.Width != 0 ? true : false)
                            : true);
            }
            set
            {
                if (value)
                {
                    this.ActiveSheet.HorizontalGridLine = m_HGridLineWidth;
                }
                else
                {
                    m_HGridLineWidth = ((this.ActiveSheet != null) ? this.ActiveSheet.HorizontalGridLine : null);
                    this.ActiveSheet.HorizontalGridLine = new GridLine(GridLineType.None);
                }
            }
        }

        private GridLine m_VGridLineWidth = new GridLine(GridLineType.Flat);

        /// <summary>
        /// Get/Set Grid Show Vertical Lines
        /// </summary>
        [DefaultValue(true)]
        public bool GridShowVert
        {
            get
            {
                return ((this.ActiveSheet != null)
                            ? (this.ActiveSheet.VerticalGridLine.Width != 0 ? true : false)
                            : true);
            }
            set
            {
                if (value)
                {
                    this.ActiveSheet.VerticalGridLine = m_VGridLineWidth;
                }
                else
                {
                    m_VGridLineWidth = ((this.ActiveSheet != null) ? this.ActiveSheet.VerticalGridLine : null);
                    this.ActiveSheet.VerticalGridLine = new GridLine(GridLineType.None);
                }
            }
        }

        /// <summary>
        /// Lock the Grid
        /// </summary>
        public bool Lock
        {
            get
            {
                bool result = false;
                if (Row != 0 & Col != 0)
                {
                    if ((GetPropertyAtRowCol(Row, Col) != null))
                    {
                        switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                        {
                            case "ao":
                                {
                                    result = ((StyleInfo) GetPropertyAtRowCol(this.Row, this.Col)).Locked;
                                    break;
                                }
                            case "Row":
                                {
                                    result = ((Row) GetPropertyAtRowCol(this.Row, this.Col)).Locked;
                                    break;
                                }
                            case "Column":
                                {
                                    result = ((Column) GetPropertyAtRowCol(this.Row, this.Col)).Locked;
                                    break;
                                }
                            case "Cell":
                                {
                                    result = ((Cell) GetPropertyAtRowCol(this.Row, this.Col)).Locked;
                                    break;
                                }
                        }
                    }
                }
                return result;
            }
            set
            {
                if (Row != 0 & Col != 0)
                {
                    if ((GetPropertyAtRowCol(Row, Col) != null))
                    {
                        switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                        {
                            case "ao":
                                {
                                    ((StyleInfo) GetPropertyAtRowCol(this.Row, this.Col)).Locked = value;
                                    break;
                                }
                            case "Row":
                                {
                                    ((Row) GetPropertyAtRowCol(this.Row, this.Col)).Locked = value;
                                    break;
                                }
                            case "Column":
                                {
                                    ((Column) GetPropertyAtRowCol(this.Row, this.Col)).Locked = value;
                                    break;
                                }
                            case "Cell":
                                {
                                    ((Cell) GetPropertyAtRowCol(this.Row, this.Col)).Locked = value;
                                    break;
                                }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets Selected Block Row
        /// </summary>
        public int SelBlockRow
        {
            get
            {
                CellRange Selection = this.ActiveSheet.GetSelection(0);
                int BlockRow = 0;
                if ((Selection != null))
                {
                    if (Selection.Row == -1)
                    {
                        BlockRow = -1;
                    }
                    else
                    {
                        //add 1 to change 0 based to 1 based rows-columns 
                        BlockRow = Selection.Row + 1;
                    }
                }
                return BlockRow;
            }
        }

        /// <summary>
        /// Gets Selected Block Row 2
        /// </summary>
        public int SelBlockRow2
        {
            get
            {
                CellRange Selection = this.ActiveSheet.GetSelection(0);
                int BlockRow = 0;
                if ((Selection != null))
                {
                    if (Selection.Row == -1)
                    {
                        BlockRow = Selection.Row;
                    }
                    else if (Selection.RowCount == 1)
                    {
                        BlockRow = Selection.Row + 1;
                    }
                    else
                    {
                        BlockRow = Selection.Row + Selection.RowCount;
                    }
                }
                return BlockRow;
            }
        }

        /// <summary>
        /// Gets Selected Mode Selection Count
        /// </summary>
        public long SelModeSelCount
        {
            get
            {
                long m_SelModeSelCount = 0;
                foreach (CellRange range in this.ActiveSheet.GetSelections())
                {
                    m_SelModeSelCount += range.RowCount;
                }
                return m_SelModeSelCount;
            }
        }

        /// <summary>
        /// Sets or returns the selection state of a row when the spreadsheet is in extended- or multiple-selection operation mode.
        /// </summary>
        public bool SelModeSelected
        {
            get
            {
                return this.ActiveSheet != null ? this.ActiveSheet.IsSelected(Row - 1, Col - 1) : false;
            }
            set
            {
                if (this.ActiveSheet != null)
                {
                    if (this.ActiveSheet.OperationMode == OperationMode.ExtendedSelect || this.ActiveSheet.OperationMode == OperationMode.MultiSelect)
                    {
                        int selRow = Row <= 0 ? -1 : Row - 1;
                        int selCol = Col <= 0 ? -1 : Col - 1;

                        if (value)
                        {
                            this.ActiveSheet.AddSelection(selRow, selCol, 1, this.MaxCols);
                        }
                        else
                        {
                            this.ActiveSheet.RemoveSelection(selRow, selCol, 1, this.MaxCols);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set SortBy constant
        /// </summary>
        public SortByConstants SortBy
        {
            get { return _SortBy; }

            set { _SortBy = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public int GetSortKey(int nIndex)
        {
            if (nIndex <= _SortInfo.Length)
            {
                return _SortInfo[nIndex - 1].Index + 1;
            }
            else
            {
                return 0;
            }
        }
        
        /// <summary>
        /// Sets Sort Key for index
        /// </summary>
        /// <param name="nIndex">index in sort list</param>
        /// <param name="value">value to set</param>
        /// <returns></returns>
        public int SetSortKey(int nIndex, int value)
        {
            if (nIndex <= _SortInfo.Length)
            {
                _SortInfo[nIndex - 1].Index = value - 1;
            }
            if (nIndex == 1)
            {
                _SortCol1 = value;
            }
            if (nIndex == 2)
            {
                _SortCol2 = value;
            }
            if (nIndex == 3)
            {
                _SortCol3 = value;
            }
            return 0;
        }

        /// <summary>
        /// Gets Sort Key Order constant
        /// </summary>
        /// <param name="nIndex">index in sort list</param>
        /// <returns>SortKeyOrderConstants constant</returns>
        public SortKeyOrderConstants GetSortKeyOrder(int nIndex)
        {
            if (nIndex >= 1 & nIndex <= 3)
            {
                return (_SortInfo[nIndex - 1].Ascending
                            ? SortKeyOrderConstants.SortKeyOrderAscending
                            : SortKeyOrderConstants.SortKeyOrderDescending);
            }
            else
            {
                return SortKeyOrderConstants.SortKeyOrderNone;
            }
        }

        /// <summary>
        /// Sets Sort Key Order with SortKeyOrderConstants constant
        /// </summary>
        /// <param name="nIndex">index in sort list</param>
        /// <param name="value">constant</param>
        public void SetSortKeyOrder(int nIndex, SortKeyOrderConstants value)
        {
            if (nIndex >= 1 & nIndex <= 3)
            {
                _SortInfo[nIndex - 1].Ascending = (value == SortKeyOrderConstants.SortKeyOrderAscending ? true : false);
            }
            if (nIndex == 1)
            {
                _SortKeyOrder1 = value;
            }
            if (nIndex == 2)
            {
                _SortKeyOrder2 = value;
            }
            if (nIndex == 3)
            {
                _SortKeyOrder3 = value;
            }
        }

        /// <summary>
        /// Get/Set Text in actual Row,Column
        /// </summary>
        public override string Text
        {
            get
            {
                string value = string.Empty;
                if (CellType is ComboBoxCellType)
                {
                    value = TypeComboBoxListControl.Text;
                }else if (Col == 0 & ValidRow(Row))
                {
                    value = this.ActiveSheet.RowHeader.Cells[Row - 1, Col].Text;
                }
                else if (ValidCol(Col) & Row == 0)
                {
                    value = this.ActiveSheet.ColumnHeader.Cells[Row, Col - 1].Text;
                }
                else if (ValidRowAndCol(Row, Col))
                {
                    value = this.ActiveSheet.Cells[Row - 1, Col - 1].Text;
                }
                if (value == null)
                {
                    value = string.Empty;
                }
                if (Row  == -1)
                {
                value = string.Empty;
                }
                return value;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }                
                if (Col == 0 & ValidRow(Row))
                {
                    this.ActiveSheet.RowHeader.Cells[Row - 1, Col].Text = value;
                }
                else if (ValidCol(Col) & Row == 0)
                {
                    this.ActiveSheet.ColumnHeader.Cells[Row, Col - 1].Text = value;
                }
                else if (ValidRowAndCol(Row, Col))
                {
                    this.ActiveSheet.Cells[Row - 1, Col - 1].Text = value;
                }
            }
        }

        /// <summary>
        /// Get/Set ToolTip text
        /// </summary>
        public string ToolTipText
        {
            get { return _ToolTipText; }
            set { _ToolTipText = value; }
        }

        /// <summary>
        /// Sets Type combobox List using a string separated by tabs
        /// </summary>
        public string TypeComboBoxList
        {
            set
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    ListBox lbox = new ListBox();
                    lbox.Items.AddRange(value.Split(Strings.Chr(9)));
                    ((ComboBoxCellType) GetCellType(Row, Col)).ListControl = lbox;
                    ((ComboBoxCellType) GetCellType(Row, Col)).Items = value.Split(Strings.Chr(9));
                }
            }
        }

        /// <summary>
        /// Get/Set Type Combobox List Control
        /// </summary>
        public ListBox TypeComboBoxListControl
        {
            get
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    ComboBoxCellType cellType = (ComboBoxCellType)GetCellType(Row, Col);
                    ListBox lbox = cellType.ListControl;

                    if (lbox == null && cellType.Items != null)
                    {
                        lbox = new ListBox();
                        lbox.Items.AddRange(cellType.Items);
                        cellType.ListControl = lbox;
                    }
                    return lbox;
                }
                return null;
            }
            set
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    ((ComboBoxCellType)GetCellType(Row, Col)).ListControl = value;
                }
            }
            

        } 

        /// <summary>
        /// Gets Type ComboBox Count as string
        /// </summary>
        public string TypeComboBoxCount
        {
            get
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    return ((ComboBoxCellType) GetCellType(Row, Col)).Items.Length.ToString();
                }
                else
                {
                    return Convert.ToString(-1);
                }
            }
        }

        /// <summary>
        /// Get/Set Type Combobox current selection
        /// </summary>
        public int TypeComboBoxCurSel
        {
            get
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    return TypeComboBoxListControl.SelectedIndex;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    TypeComboBoxListControl.SelectedItem = TypeComboBoxListControl.Items[value];
                    this.Text = TypeComboBoxListControl.SelectedItem.ToString();
                }
            }
        }

        /// <summary>
        /// Get/Set Type Combobox Editable property
        /// </summary>
        public bool TypeComboBoxEditable
        {
            get
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    return ((ComboBoxCellType) GetCellType(Row, Col)).Editable;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    ((ComboBoxCellType) GetCellType(Row, Col)).Editable = value;
                }
            }
        }

        /// <summary>
        /// Get/Set TypeComboBox String Array
        /// </summary>
        public string[] TypeComboBoxString
        {
            get
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    return ((ComboBoxCellType) GetCellType(Row, Col)).Items;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (GetCellType(Row, Col) is ComboBoxCellType)
                {
                    ((ComboBoxCellType)GetCellType(Row, Col)).Items = value;
                }
            }
        }

        /// <summary>
        /// Gets Set Type Edit Multiline state
        /// </summary>
        public bool TypeEditMultiLine
        {
            get
            {
                if (GetCellType(Row, Col) is TextCellType)
                {
                    return ((TextCellType) GetCellType(Row, Col)).Multiline;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (GetCellType(Row, Col) is TextCellType)
                {
                    ((TextCellType) GetCellType(Row, Col)).Multiline = value;
                }
            }
        }

        /// <summary>
        /// Get/Set Type Float Decimal Char Separator
        /// </summary>
        public string TypeFloatDecimalChar
        {
            get
            {
                if (GetCellType(Row, Col) is CurrencyCellType)
                {
                    return ((CurrencyCellType) GetCellType(Row, Col)).DecimalSeparator;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (GetCellType(Row, Col) is CurrencyCellType)
                {
                    ((CurrencyCellType) GetCellType(Row, Col)).DecimalSeparator = value;
                }
            }
        }

        /// <summary>
        /// Get/Set Type Float Separator Char for thousands
        /// </summary>
        public string TypeFloatSepChar
        {
            get
            {
                if (GetCellType(Row, Col) is CurrencyCellType)
                {
                    return ((CurrencyCellType) GetCellType(Row, Col)).Separator;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (GetCellType(Row, Col) is CurrencyCellType)
                {
                    ((CurrencyCellType) GetCellType(Row, Col)).Separator = value;
                }
            }
        }

        /// <summary>
        /// Sets Type Horizontal Alignment
        /// </summary>
        public CellHorizontalAlignment TypeHAlign
        {
            set
            {
                StyleInfo si = GetCompositeInfo(this.Row, this.Col);
                if ((si != null) && si.HorizontalAlignment != value)
                {
                    switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                    {
                        case "ao":
                            {
                                ((StyleInfo) GetPropertyAtRowCol(this.Row, this.Col)).HorizontalAlignment = value;
                                break;
                            }
                        case "Row":
                            {
                                ((Row) GetPropertyAtRowCol(this.Row, this.Col)).HorizontalAlignment = value;
                                break;
                            }
                        case "Column":
                            {
                                ((Column) GetPropertyAtRowCol(this.Row, this.Col)).HorizontalAlignment = value;
                                break;
                            }
                        case "Cell":
                            {
                                ((Cell) GetPropertyAtRowCol(this.Row, this.Col)).HorizontalAlignment = value;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Sets Type Vertical Align
        /// </summary>
        public CellVerticalAlignment TypeVAlign
        {
            set
            {
                StyleInfo si = GetCompositeInfo(this.Row, this.Col);
                if ((si != null) && si.VerticalAlignment != value)
                {
                    switch (GetPropertyAtRowCol(this.Row, this.Col).GetType().Name)
                    {
                        case "ao":
                            {
                                ((StyleInfo) GetPropertyAtRowCol(this.Row, this.Col)).VerticalAlignment = value;
                                break;
                            }
                        case "Row":
                            {
                                ((Row) GetPropertyAtRowCol(this.Row, this.Col)).VerticalAlignment = value;
                                break;
                            }
                        case "Column":
                            {
                                ((Column) GetPropertyAtRowCol(this.Row, this.Col)).VerticalAlignment = value;
                                break;
                            }
                        case "Cell":
                            {
                                ((Cell) GetPropertyAtRowCol(this.Row, this.Col)).VerticalAlignment = value;
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Gets object Value in Row, Column
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <param name="Col">Column to look at</param>
        /// <returns>Object value</returns>
        public object GetValue(int Row, int Col)
        {
            if (Col == 0 & ValidRow(Row))
            {
                return this.ActiveSheet.RowHeader.Cells[Row - 1, Col].Value;
            }
            else if (ValidCol(Col) & Row == 0)
            {
                return this.ActiveSheet.ColumnHeader.Cells[Row, Col - 1].Value;
            }
            else if (ValidRowAndCol(Row, Col))
            {
                return this.ActiveSheet.Cells[Row - 1, Col - 1].Value;
            }
            return null;
        }

        /// <summary>
        /// Sets Value at Row,Column
        /// </summary>
        /// <param name="Row">Row to set value at</param>
        /// <param name="Col">Column to set value at</param>
        /// <param name="value">Value to set</param>
        public void SetValue(int Row, int Col, object value)
        {
            if (Col == 0 & ValidRow(Row))
            {
                this.ActiveSheet.RowHeader.Cells[Row - 1, Col].Value = value;
            }
            else if (ValidCol(Col) & Row == 0)
            {
                this.ActiveSheet.ColumnHeader.Cells[Row, Col].Value = value;
            }
            else if (ValidRowAndCol(Row, Col))
            {
                this.ActiveSheet.Cells[Row - 1, Col - 1].Value = value;
            }
        }

        /// <summary>
        /// Get/Set Value for actual Row, Column
        /// </summary>
        public object Value
        {
            get { return GetValue(Row, Col); }
            set { SetValue(Row, Col, value); }
        }

        /// <summary>
        /// Get/Set Rows Frozen state
        /// </summary>
        public int RowsFrozen
        {
            get { return ((this.ActiveSheet != null) ? this.ActiveSheet.FrozenRowCount : 0); }
            set
            {
                if (this.ActiveSheet != null)
                {
                    this.ActiveSheet.FrozenRowCount = value;
                }
            }
        }

        /// <summary>
        /// Get/Set Columns Frozen state
        /// </summary>
        public int ColsFrozen
        {
            get { return ((this.ActiveSheet != null) ? this.ActiveSheet.FrozenColumnCount : 0); }
            set
            {
                if (this.ActiveSheet != null)
                {
                    this.ActiveSheet.FrozenColumnCount = value;
                }
            }
        }

        /// <summary>
        /// Gets Cell from screen coordinate
        /// </summary>
        /// <param name="Col">returned column</param>
        /// <param name="Row">returned row</param>
        /// <param name="X">X pixel position</param>
        /// <param name="Y">Y pixel position</param>
        /// <returns>Cell Range at X,Y position</returns>
        public CellRange GetCellFromScreenCoord(ref int Col, ref int Row, float X, float Y)
        {
            CellRange Range = GetCellFromPixel(X, Y);
            Col = Range.Column + 1;
            Row = Range.Row + 1;
            return Range;
        }

        /// <summary>
        /// Gets Cell from pixel
        /// </summary>
        /// <param name="X">X pixel position</param>
        /// <param name="Y">Y pixel position</param>
        /// <returns>Cell Range at X,Y position</returns>
        public CellRange GetCellFromPixel(float X, float Y)
        {
            CellRange Range;
            Range = this.GetRootWorkbook().GetCellFromPixel(0, 0, Convert.ToInt32(X), Convert.ToInt32(Y));
            if (Range.Column == -1 & Range.Row == -1 & RowsFrozen > 0)
            {
                Range = this.GetRootWorkbook().GetCellFromPixel(-1, 0, Convert.ToInt32(X), Convert.ToInt32(Y));
            }
            if (Range.Column == -1 & Range.Row == -1 & ColsFrozen > 0)
            {
                Range = this.GetRootWorkbook().GetCellFromPixel(0, -1, Convert.ToInt32(X), Convert.ToInt32(Y));
            }
            if (Range.Column == -1 & Range.Row == -1 & ColsFrozen > 0 & RowsFrozen > 0)
            {
                Range = this.GetRootWorkbook().GetCellFromPixel(-1, -1, Convert.ToInt32(X), Convert.ToInt32(Y));
            }
            return Range;
        }

        /// <summary>
        /// Gets Cell Position
        /// </summary>
        /// <param name="Col">Column</param>
        /// <param name="Row">Row</param>
        /// <param name="l">Left position</param>
        /// <param name="t">Right position</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public void GetCellPos(int Col, int Row, ref int l, ref int t, ref int w, ref int h)
        {
            Rectangle r = this.GetCellRectangle(0, 0, Row - 1, Col - 1);
            l = r.Left;
            t = r.Right;
            w = r.Width;
            h = r.Height;
        }

        /// <summary>
        /// Get/Set Redraw action
        /// </summary>
        public bool ReDraw
        {
            get { return !this.IsLayoutSuspended; }
            set
            {
                if (value)
                {
                    this.ResumeLayout();
                }
                else
                {
                    this.SuspendLayout();
                }
            }
        }

        /// <summary>
        /// Sets RunLocalEvents status
        /// </summary>
        public bool RunLocalEvents
        {
            set { _RunLocalEvents = value; }
        }

        /// <summary>
        /// ItemData is stored in a generic collection instead of the Tag property to avoid boxing/unboxing 
        /// which has a performance hit
        /// </summary>
        /// <param name="Col">Column to set at</param>
        /// <param name="value">value to set</param>
        public void SetColItemData(ref int Col, ref int value)
        {
            //this.ActiveSheet.Columns(Col - 1).Tag = value 
            _ColItemData[Col] = value;
        }

        /// <summary>
        /// Sets Row Item Data at Row
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <param name="value">value to set</param>
        public void SetRowItemData(ref int Row, ref int value)
        {
            //this.ActiveSheet.Rows(Row - 1).Tag = value 
            _RowItemData[Row] = value;
        }

        /// <summary>
        /// Gets Column Item Data
        /// </summary>
        /// <param name="Col">Column to look at</param>
        /// <returns>Data in column position</returns>
        public int GetColItemData(int Col)
        {
            //Return this.ActiveSheet.Columns(Col - 1).Tag 
            return _ColItemData[Col];
        }

        /// <summary>
        /// Gets Row Item data
        /// </summary>
        /// <param name="Row">Row to look at</param>
        /// <returns>Data in Row position</returns>
        public int GetRowItemData(int Row)
        {
            //Return this.ActiveSheet.Rows(Row - 1).Tag 
            return _RowItemData[Row];
        }

        private void FpSpread_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs eventArgs)
        {
            if (Change != null)
            {
                Change(this, new ChangeEventArgs(eventArgs.Column+ 1, eventArgs.Row + 1));
            }
        }

        private void FpSpread_CellClick(object sender, CellClickEventArgs eventArgs)
        {
            _EventClickCell = true;
            try
            {
                //Trigger LeaveRow
                if ((this.ActiveSheet.OperationMode == OperationMode.RowMode || this.ActiveSheet.OperationMode == OperationMode.SingleSelect || this.ActiveSheet.OperationMode == OperationMode.MultiSelect || this.ActiveSheet.OperationMode == OperationMode.ExtendedSelect) && this.ActiveRowIndex != eventArgs.Row + 1)
                {
                    if (LeaveRow != null)
                    {
                        LeaveRow(sender, new LeaveCellEventArgs(this.GetRootWorkbook().GetActiveWorkbook().GetSpreadView(this.ActiveSheet, -1, -1), this.ActiveRowIndex, this.ActiveColumnIndex, eventArgs.Row + 1, eventArgs.Column + 1));
                    }
                }

                //Updates ActiveIndexes
                this.ActiveColumnIndex = eventArgs.Column + 1;
                this.ActiveRowIndex = eventArgs.Row + 1;

                //Check which mouse button was clicked
                if (eventArgs.Button == MouseButtons.Right)
                {
                    if (CellRightClick != null)
                    {
                        CellRightClick(this, new CellClickEventArgs(eventArgs.View, eventArgs.Row + 1, eventArgs.Column + 1, eventArgs.X, eventArgs.Y, eventArgs.Button, eventArgs.ColumnHeader, eventArgs.RowHeader));
                    }
                }
                else if (CellClick != null)
                {
                    CellClick(this, new CellClickEventArgs(eventArgs.View, eventArgs.Row + 1, eventArgs.Column + 1, eventArgs.X, eventArgs.Y, eventArgs.Button, eventArgs.ColumnHeader, eventArgs.RowHeader));
                }
                _EventClickCell = false;
            }
            catch(Exception Ex)
            {
                _EventClickCell = false;
                throw Ex;
            }
        }

        /// <summary>
        /// LeaveCell event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="eventArgs">Event arguments</param>
        public void FpSpread_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs eventArgs)
        {
            if (LeaveCell != null)
            {
                LeaveCell(sender, new LeaveCellEventArgs(this.GetRootWorkbook().GetActiveWorkbook().GetSpreadView(this.ActiveSheet, -1, -1), eventArgs.Row + 1, eventArgs.Column + 1, eventArgs.NewRow + 1, eventArgs.NewColumn + 1));
            }
        }

        private void FpSpread_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs eventArgs)
        {
            eventArgs.ShowTip = true;
            eventArgs.TipText = ToolTipText;
            if (TextTipFetch != null)
            {
                TextTipFetch(sender, new TextTipFetchEventArgs(eventArgs));
            }
        }

        /// <summary>
        /// Left Change event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="eventArgs">Event arguments</param>
        public void FpSpread_LeftChange(object sender, LeftChangeEventArgs eventArgs)
        {
            if (TopLeftChange != null)
            {
                TopLeftChange(sender,
                              new TopLeftChangeEventArgs(
                                  this.GetRootWorkbook().GetActiveWorkbook().GetSpreadView(this.ActiveSheet, -1,
                                                                                           eventArgs.ColumnViewportIndex),
                                  eventArgs.OldLeft, eventArgs.NewLeft, this.Top, this.Top, -1,
                                  eventArgs.ColumnViewportIndex));
            }
        }

        /// <summary>
        /// Top Change event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="eventArgs">Event Arguments</param>
        public void FpSpread_TopChange(object sender, TopChangeEventArgs eventArgs)
        {
            if (TopLeftChange != null)
            {
                TopLeftChange(sender,
                              new TopLeftChangeEventArgs(
                                  this.GetRootWorkbook().GetActiveWorkbook().GetSpreadView(this.ActiveSheet,
                                                                                           eventArgs.RowViewportIndex,
                                                                                           -1), this.Left, this.Left,
                                  eventArgs.OldTop, eventArgs.NewTop, eventArgs.RowViewportIndex, -1));
            }
        }

        private bool Pasting = false;
        private int LinesPasted;
        /// <summary>
        /// For Pasting purposes with Clipboard
        /// </summary>
        public bool PastingHandled = false;
       
        private void Spread_ClipboardPasting(object sender, ClipboardPastingEventArgs e)
        {
            if (!PastingHandled)
            {
                Pasting = true;
                e.Handled = PastingHandled;
                PastingHandled = false;

                //Count how many lines are to be pasted to raise as many Change events as lines 
                string[] lines = new Computer().Clipboard.GetText().Split(new string[] {Constants.vbCrLf},
                                                                          StringSplitOptions.None);
                LinesPasted = 0;
                foreach (string line in lines)
                {
                    if (line.Trim().Length > 0)
                    {
                        LinesPasted += 1;
                    }
                }
            }
        }

        private bool ModelChanging = false;
        private void DataModel_Changed(object sender, SheetDataModelEventArgs e)
        {
            //This.Changed is not raised on a paste. DataModel.Changed is raised when data is changed by the 
            //user, by code or paste. We need to raise the Change event only with paste or user changes. 
            //User changes are caught on Me.Change, paste changes are caught here 
            if (Pasting & !PastingHandled & !ModelChanging)
            {
                ModelChanging = true;
                if (Change != null)
                {
                    Change(this, new ChangeEventArgs(e.Column + 1, e.Row + 1));
                }
                LinesPasted -= 1;
                ModelChanging = false;
                if (!(LinesPasted > 0))
                {
                    Pasting = false;
                }
            }

            if (e.Type == SheetDataModelEventType.RowsAdded)
            {
                _RowItemData.InsertItems(e.Row + 1, e.RowCount);
            }
            else if (e.Type == SheetDataModelEventType.RowsRemoved)
            {
                _RowItemData.RemoveRange(e.Row + 1, e.RowCount);
            }
            else if (e.Type == SheetDataModelEventType.ColumnsAdded)
            {
                _ColItemData.InsertItems(e.Column + 1, e.ColumnCount);
            }
            else if (e.Type == SheetDataModelEventType.ColumnsRemoved)
            {
                _ColItemData.RemoveRange(e.Column + 1, e.ColumnCount);
            }
        }

        private bool _FirstPaint = true;
        private void FpSpread_Paint(object sender, PaintEventArgs e)
        {
            if (_FirstPaint)
            {
                InitializeSpread();
                _FirstPaint = false;
            }
        }

        /// <summary>
        /// BackColorStyle has no equivalent in FPSpread.NET
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BackColorStyleConstants BackColorStyle
        {
            get { return BackColorStyleConstants.BackColorStyleOverGrid; }

            set { }
        }

        /// <summary>
        /// BlockMode has no equivalent in FPSpread.NET
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BlockMode
        {
            get { return false; }

            set { }
        }

        /// <summary>
        /// Get/Set Change Made has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ChangeMade
        {
            get { return false; }

            set { }
        }

        /// <summary>
        /// Get/Set Count has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Count
        {
            get { return 0; }
            // 
            set { }
        }

        /// <summary>
        /// Get/Set Item has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Item
        {
            get { return null; }

            set { }
        }

        /// <summary>
        /// Get/Set MultSelIndex has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MultiSelIndex
        {
            get { return 0; }

            set { }
        }

        /// <summary>
        /// Get/Set TypeComboBox has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TypeComboBoxIndex
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Get/Set TypeEditLen has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TypeEditLen
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Get/Set FormulaSync has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FormulaSync
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Get/Set NoBeep has no equivalent en FPSpread.Net
        /// </summary>
        [Obsolete("No equivalent in FpSpread.NET")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool NoBeep
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Creates a copy of the format FpSpread.
        /// </summary>
        /// <returns></returns>
        public FpSpread Clone()
        {
            FpSpread fpSpreadClone = new FpSpread();
            if (fpSpreadClone.Sheets.Count <= 0)
            {
                fpSpreadClone.Sheets.AddRange(new SheetView[] { new SheetView() });
            }

            if (Sheets.Count > 0)
            {
                fpSpreadClone.Sheets[0].RowCount = 0;// fpSpreadFather.Sheets[0].RowCount;
                fpSpreadClone.Sheets[0].ColumnCount = this.Sheets[0].ColumnCount;
                //Clone the property(Sheet)
                foreach (PropertyInfo propertyInfo in this.Sheets[0].GetType().GetProperties())
                {
                    if (propertyInfo.Name != "RowCount" && propertyInfo.Name != "Models")
                    {
                        if (propertyInfo.CanRead && propertyInfo.CanWrite)
                        {
                            try
                            {
                                object[] index = new object[0];
                                if (propertyInfo.GetValue(this.Sheets[0], index) == null)
                                {
                                    propertyInfo.SetValue(fpSpreadClone.Sheets[0], null, index);
                                }
                                else if (!propertyInfo.GetValue(this.Sheets[0], index).Equals(propertyInfo.GetValue(fpSpreadClone.Sheets[0], index)))
                                {
                                    propertyInfo.SetValue(fpSpreadClone.Sheets[0], propertyInfo.GetValue(this.Sheets[0], index), index);
                                }
                            }
                            catch { }
                        }
                    }
                }

                fpSpreadClone.Sheets[0].Rows.Default.Height = this.Sheets[0].Rows.Default.Height;
                fpSpreadClone.Sheets[0].Rows.Default.Font = this.Sheets[0].Rows.Default.Font;
                fpSpreadClone.Sheets[0].ColumnHeader.Rows.Default.Height = this.Sheets[0].ColumnHeader.Rows.Default.Height;
                fpSpreadClone.Sheets[0].RowHeader.Columns.Default.Resizable = this.Sheets[0].RowHeader.Columns.Default.Resizable;

                for (int col = 0; col < this.Sheets[0].ColumnCount; col++)
                {
                    fpSpreadClone.Sheets[0].Columns[col].Label = this.Sheets[0].Columns[col].Label;
                    fpSpreadClone.Sheets[0].Columns[col].StyleName = this.Sheets[0].Columns[col].StyleName;
                    fpSpreadClone.Sheets[0].Columns[col].Width = this.Sheets[0].Columns[col].Width;
                    fpSpreadClone.Sheets[0].Columns[col].Resizable = this.Sheets[0].Columns[col].Resizable;
                    fpSpreadClone.Sheets[0].Columns[col].Visible = this.Sheets[0].Columns[col].Visible;
                }

                for (int colRowHeader = 0; colRowHeader < this.Sheets[0].RowHeader.ColumnCount; colRowHeader++)
                {
                    fpSpreadClone.Sheets[0].RowHeader.Columns.Get(colRowHeader).Width = this.Sheets[0].RowHeader.Columns.Get(colRowHeader).Width;
                    fpSpreadClone.Sheets[0].RowHeader.Columns.Get(colRowHeader).Visible = this.Sheets[0].RowHeader.Columns.Get(colRowHeader).Visible;
                }

                fpSpreadClone.Row = 0;
                fpSpreadClone.Col = this.Col;
                
                foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType.Name != "SheetView" && propertyInfo.Name != "VerticalScrollBar" && propertyInfo.Name != "WindowTarget" && propertyInfo.Name != "BackColor" && propertyInfo.Name != "Row" && propertyInfo.Name != "Col" && propertyInfo.Name != "MaxRows")
                    {
                        if (propertyInfo.CanRead && propertyInfo.CanWrite)
                        {
                            try
                            {
                                object[] index = new object[0];
                                if (propertyInfo.GetValue(this, index) == null)
                                {
                                    propertyInfo.SetValue(fpSpreadClone, null, index);
                                }
                                else if (!propertyInfo.GetValue(this, index).Equals(propertyInfo.GetValue(fpSpreadClone, index)))
                                {
                                    propertyInfo.SetValue(fpSpreadClone, propertyInfo.GetValue(this, index), index);
                                }
                            }
                            catch { }
                        }
                    }
                }

                ////Bind the same events set in the base control to the new control
                //Delegate[] EventDelegates = null;
                //foreach (EventInfo eInfo in GetType().GetEvents())
                //{
                //    EventDelegates = GetEventSubscribers(this, eInfo.Name);

                //    //The event in the new control will be bound to the same delegates of the base control
                //    if (EventDelegates != null)
                //    {
                //        foreach (Delegate del in EventDelegates)
                //        {
                //            try
                //            {
                //                this.GetType().GetEvent("").EventHandlerType.
                //                eInfo.AddEventHandler(fpSpreadClone, del);
                //            }
                //            catch { }
                //        }
                //    }
                //}
            }
            return fpSpreadClone;
        }

        /// <summary>
        /// Gets the delegates bound to an event in an object
        /// </summary>
        /// <param name="Target">The object</param>
        /// <param name="EventName">The event name</param>
        /// <returns>Null if no delegates or event where found</returns>
        private static Delegate[] GetEventSubscribers(object Target, string EventName)
        {
            Delegate del = null;
            string[] WinFormsEventsName = new string[] { "Event" + EventName, "Event_" + EventName
                , "EVENT" + EventName.ToUpper(), "EVENT_" + EventName.ToUpper()};
            Type TargetType = Target.GetType();
            FieldInfo fInfo = null;

            while (TargetType != null)
            {
                //Look for a field in the Target with the name of the event
                fInfo = TargetType.GetField(EventName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                if (fInfo != null)
                {
                    //Gets the current value in the Target instance
                    del = (Delegate)fInfo.GetValue(Target);
                    if (del != null)
                    {
                        return del.GetInvocationList();
                    }
                }
                else
                {
                    foreach (string winEventName in WinFormsEventsName)
                    {
                        //Look for a field in the Target with the name of the event as defined in some cases
                        fInfo = TargetType.GetField(winEventName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                        if (fInfo != null)
                        {
                            EventHandlerList eHandlerList = (EventHandlerList)Target.GetType().GetProperty("Events", (BindingFlags.FlattenHierarchy | (BindingFlags.NonPublic | BindingFlags.Instance))).GetValue(Target, null);

                            del = eHandlerList[fInfo.GetValue(Target)];
                            if (del != null)
                            {
                                return del.GetInvocationList();
                            }
                        }
                    }
                }

                //Repeats the process in the base types if nothing has been found so far
                TargetType = TargetType.BaseType;
            }

            return null;
        }
    }

   #region ItemDataList

    internal class ItemDataList<T> : List<T>
    {
        public ItemDataList(int Capacity)
        {
            for (int i = 0; i <= Capacity - 1; i++)
            {
                this.Insert(i, default(T));
            }
        }

        public void InsertItems(int StartIndex, int Count)
        {
            int index = StartIndex + 1;
            if (index > this.Count)
            {
                index = StartIndex;
            }
            for (int i = 1; i <= Count; i++)
            {
                this.Insert(index, default(T));
                index += 1;
            }
        }
    }

    #endregion

    #region CellComparer

    /// <summary>
    /// Class for Cell comparison purposes
    /// </summary>
    public class CellComparer : IComparer
    {
        private Comparer DefaultComparer = Comparer.Default;

        int IComparer.Compare(object x, object y)
        {
            int CompareResult = 0;
            if (x is Bitmap & y is Bitmap)
            {
                CompareResult = CompareBitmaps((Bitmap) x, (Bitmap) y);
            }
            else
            {
                CompareResult = DefaultComparer.Compare(x, y);
            }
            return CompareResult;
        }

        private int CompareBitmaps(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null & bmp2 == null)
            {
                return 0;
            }
            else if (bmp1 == null & (bmp2 != null))
            {
                return 1;
            }
            else if ((bmp1 != null) & bmp2 == null)
            {
                return -1;
            }
            else if (bmp1.Equals(bmp2))
            {
                return 0;
            }
            else if (bmp1.Width * bmp1.Height > bmp2.Width * bmp2.Height)
            {
                return 1;
            }
            else if (bmp1.Width * bmp1.Height < bmp2.Width * bmp2.Height)
            {
                return -1;
            }
            else
            {
                //Convert each image to a byte array 
                ImageConverter ic = new ImageConverter();
                byte[] btImage1 = new byte[2];
                btImage1 = (byte[]) ic.ConvertTo(bmp1, btImage1.GetType());
                byte[] btImage2 = new byte[2];
                btImage2 = (byte[]) ic.ConvertTo(bmp2, btImage2.GetType());

                //Compute a hash for each image 
                SHA256Managed shaM = new SHA256Managed();
                byte[] hash1 = shaM.ComputeHash(btImage1);
                byte[] hash2 = shaM.ComputeHash(btImage2);

                //Compare the hash values 
                for (int i = 0; i <= hash1.Length - 1 & i < hash2.Length; i++)
                {
                    if (hash1[i] > hash2[i])
                    {
                        return -1;
                    }
                    else if (hash1[i] < hash2[i])
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
    }

    #endregion

    #region EventHandlers

    /// <summary>
    /// Change event handler
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">Change Event arguments</param>
    public delegate void ChangeEventHandler(object sender, ChangeEventArgs e);

    /// <summary>
    /// Cell Click event handler
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">Cell Click event arguments</param>
    public delegate void CellClickEventHandler(object sender, CellClickEventArgs e);

    /// <summary>
    /// Leave Cell event handler
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">Leave cell event arguments</param>
    public delegate void LeaveCellEventHandler(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e);

    /// <summary>
    /// Leave Row event handler
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">Leave cell event arguments</param>
    public delegate void LeaveRowEventHandler(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e);

    /// <summary>
    /// TextTip Fetch event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TextTipFetchEventHandler(object sender, TextTipFetchEventArgs e);

    /// <summary>
    /// Top Left Change event handler
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">Top Left change event arguments</param>
    public delegate void TopLeftChangeEventHandler(object sender, TopLeftChangeEventArgs e);

    #endregion

    #region ChangeEventArgs

    /// <summary>
    /// Change Event Arguments class
    /// </summary>
    public sealed class ChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Row
        /// </summary>
        public int Row;
        /// <summary>
        /// Column
        /// </summary>
        public int Col;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Col">Column, Row</param>
        /// <param name="Row"></param>
        public ChangeEventArgs(int Col, int Row): base()
        {
            this.Col = Col;
            this.Row = Row;
        }
    }

    #endregion

    #region LeaveCellEventArgs

    /// <summary>
    /// Leave Cell Event Arguments Class, implemented from Farpoint Leave Cell Event Args
    /// </summary>
    public sealed class LeaveCellEventArgs : FarPoint.Win.Spread.LeaveCellEventArgs
    {
        /// <summary>
        /// Constructor passing all parameter to Farpoint class
        /// </summary>
        /// <param name="view">view</param>
        /// <param name="row">row</param>
        /// <param name="column">column</param>
        /// <param name="newRow">new row</param>
        /// <param name="newColumn">new column</param>
        public LeaveCellEventArgs(SpreadView view, int row, int column, int newRow, int newColumn)
            : base(view, row, column, newRow, newColumn)
        {
        }
    }

    #endregion

    #region TextTipFetchEventArgs

    /// <summary>
    /// TextTip Fetch Event Arguments
    /// </summary>
    public sealed class TextTipFetchEventArgs : EventArgs
    {
        private FarPoint.Win.Spread.TextTipFetchEventArgs _eventArgs;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventArgs"></param>
        public TextTipFetchEventArgs(FarPoint.Win.Spread.TextTipFetchEventArgs eventArgs) : base()
        {
            _eventArgs = eventArgs;
        }

        /// <summary>
        /// Gets Column
        /// </summary>
        public int Col
        {
            get { return (_eventArgs.RowHeader ? _eventArgs.Column : _eventArgs.Column + 1); }
        }

        /// <summary>
        /// Gets Row
        /// </summary>
        public int Row
        {
            get { return (_eventArgs.ColumnHeader ? _eventArgs.Row : _eventArgs.Row + 1); }
        }

        /// <summary>
        /// Gets Sets Multiline state
        /// </summary>
        public int MultiLine
        {
            get { return Convert.ToInt32(_eventArgs.WrapText); }
            set { _eventArgs.WrapText = Convert.ToBoolean(value); }
        }

        /// <summary>
        /// Get/Set TipWidth value
        /// </summary>
        public int TipWidth
        {
            get { return _eventArgs.TipWidth; }
            set { _eventArgs.TipWidth = value; }
        }

        /// <summary>
        /// Get/Set Tip Text value
        /// </summary>
        public string TipText
        {
            get { return _eventArgs.TipText; }
            set { _eventArgs.TipText = value; }
        }
        
        /// <summary>
        /// Get/Set Show Tip
        /// </summary>
        public bool ShowTip
        {
            get { return _eventArgs.ShowTip; }
            set { _eventArgs.ShowTip = value; }
        }
    }

    #endregion

    #region TopLeftChangeEventArgs

    /// <summary>
    /// Top Left Change Event Arguments class
    /// </summary>
    public class TopLeftChangeEventArgs : EventArgs
    {
        // Fields
        private SpreadView _SpreadView;
        private int _OldLeft;
        private int _NewLeft;
        private int _OldTop;
        private int _NewTop;
        private int _RowViewportIndex;
        private int _ColumnViewportIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">SpredView</param>
        /// <param name="oldLeft">old left</param>
        /// <param name="newLeft">new left</param>
        /// <param name="oldTop">old top</param>
        /// <param name="newTop">new top</param>
        /// <param name="rowViewportIndex">row view port index</param>
        /// <param name="columnViewportIndex">column view port index</param>
        public TopLeftChangeEventArgs(SpreadView view, int oldLeft, int newLeft, int oldTop, int newTop,
                                      int rowViewportIndex, int columnViewportIndex)
        {
            this._SpreadView = view;
            this._OldTop = oldTop;
            this._NewTop = newTop;
            this._OldLeft = oldLeft;
            this._NewLeft = newLeft;
            this._RowViewportIndex = rowViewportIndex;
            this._ColumnViewportIndex = columnViewportIndex;
        }

        /// <summary>
        /// New Left
        /// </summary>
        public int NewLeft
        {
            get { return this._NewLeft; }
        }

        /// <summary>
        /// Old Left
        /// </summary>
        public int OldLeft
        {
            get { return this._OldLeft; }
        }

        /// <summary>
        /// New Top
        /// </summary>
        public int NewTop
        {
            get { return this._NewTop; }
        }

        /// <summary>
        /// Old Top
        /// </summary>
        public int OldTop
        {
            get { return this._OldTop; }
        }

        /// <summary>
        /// Row view port index
        /// </summary>
        public int RowViewportIndex
        {
            get { return this._RowViewportIndex; }
        }

        /// <summary>
        /// Column view port index
        /// </summary>
        public int ColumnViewportIndex
        {
            get { return this._ColumnViewportIndex; }
        }
    }

    #endregion

    #region Enums
    /// <summary>
    /// Back Color style constants
    /// </summary>
    public enum BackColorStyleConstants
    {
        /// <summary>
        /// Over Grid
        /// </summary>
        BackColorStyleOverGrid = 0,
        /// <summary>
        /// Under Grid
        /// </summary>
        BackColorStyleUnderGrid = 1,
        /// <summary>
        /// Over Horizantal Grid only
        /// </summary>
        BackColorStyleOverHorzGridOnly = 2,
        /// <summary>
        /// Over Vertical Grid only
        /// </summary>
        BackColorStyleOverVertGridOnly = 3 
    }

    /// <summary>
    /// Sort key order constants
    /// </summary>
    public enum SortKeyOrderConstants
    {
        /// <summary>
        /// No order
        /// </summary>
        SortKeyOrderNone = 0,
        /// <summary>
        /// Order Ascending
        /// </summary>
        SortKeyOrderAscending = 1,
        /// <summary>
        /// Order Descending
        /// </summary>
        SortKeyOrderDescending = 2
    }

    /// <summary>
    /// Action constants
    /// </summary>
    public enum ActionConstants
    {
        /// <summary>
        /// Active cell
        /// </summary>
        ActionActiveCell = 0,
        /// <summary>
        /// Goto cell
        /// </summary>
        ActionGotoCell = 1,
        /// <summary>
        /// Select block
        /// </summary>
        ActionSelectBlock = 2,
        /// <summary>
        /// Clear
        /// </summary>
        ActionClear = 3,
        /// <summary>
        /// Delete Row
        /// </summary>
        ActionDeleteRow = 5,
        /// <summary>
        /// Insert column
        /// </summary>
        ActionInsertCol = 6,
        /// <summary>
        /// Insert row
        /// </summary>
        ActionInsertRow = 7,
        /// <summary>
        /// Clear text
        /// </summary>
        ActionClearText = 12,
        /// <summary>
        /// Deselect Block
        /// </summary>
        ActionDeselectBlock = 14,
        /// <summary>
        /// Add multiselect block
        /// </summary>
        ActionAddMultiSelBlock = 17,
        /// <summary>
        /// Gets multi selection
        /// </summary>
        ActionGetMultiSelection = 18,
        /// <summary>
        /// Clipboard copy
        /// </summary>
        ActionClipboardCopy = 22,
        /// <summary>
        /// Clipboard paste
        /// </summary>
        ActionClipboardPaste = 24,
        /// <summary>
        /// Sort
        /// </summary>
        ActionSort = 25,
        /// <summary>
        /// Smart print
        /// </summary>
        ActionSmartPrint = 32
    }

    /// <summary>
    /// Click Type constants
    /// </summary>
    public enum ClickType
    {
        /// <summary>
        /// Down
        /// </summary>
        Down = 0,
        /// <summary>
        /// Up
        /// </summary>
        Up,
        /// <summary>
        /// Double click
        /// </summary>
        DoubleClick
    }

    /// <summary>
    /// Position Constants
    /// </summary>
    public enum PositionConstants
    {
        /// <summary>
        /// Upper left
        /// </summary>
        PositionUpperLeft,
        /// <summary>
        /// Upper center
        /// </summary>
        PositionUpperCenter,
        /// <summary>
        /// Upper right
        /// </summary>
        PositionUpperRight,
        /// <summary>
        /// Center left
        /// </summary>
        PositionCenterLeft,
        /// <summary>
        /// Center
        /// </summary>
        PositionCenter,
        /// <summary>
        /// Center right
        /// </summary>
        PositionCenterRight,
        /// <summary>
        /// Bottom left
        /// </summary>
        PositionBottomLeft,
        /// <summary>
        /// Bottom center
        /// </summary>
        PositionBottomCenter,
        /// <summary>
        /// Bottom right
        /// </summary>
        PositionBottomRight
    }

    /// <summary>
    /// Scroll bars constants
    /// </summary>
    public enum ScrollBarsConstants
    {
        /// <summary>
        /// None
        /// </summary>
        ScrollBarsNone,
        /// <summary>
        /// Horizontal
        /// </summary>
        ScrollBarsHorizontal,
        /// <summary>
        /// Vertical
        /// </summary>
        ScrollBarsVertical,
        /// <summary>
        /// Both
        /// </summary>
        ScrollBarsBoth
    }

    /// <summary>
    /// Standard Aggregate Column
    /// </summary>
    public enum enumStdAggregateColumn
    {
        /// <summary>
        /// First
        /// </summary>
        colFirst = 1,
        /// <summary>
        /// Name
        /// </summary>
        colName = colFirst,
        /// <summary>
        /// Index Percent
        /// </summary>
        colIndexPercent,
        /// <summary>
        /// Index value
        /// </summary>
        colIndexValue,
        /// <summary>
        /// Benchmark count
        /// </summary>
        colBenchmarkCount,
        /// <summary>
        /// Fund count
        /// </summary>
        colFundCount,
        /// <summary>
        /// Fund value pretrade
        /// </summary>
        colFundValuePreTrade,
        /// <summary>
        /// Fund percent pretrade
        /// </summary>
        colFundPercentPreTrade,
        /// <summary>
        /// Missing weight percent pretrade
        /// </summary>
        colMisweightPercentPreTrade,
        /// <summary>
        /// Missing weight value pretrade
        /// </summary>
        colMisweightValuePreTrade,
        /// <summary>
        /// Fund value post trade
        /// </summary>
        colFundValuePostTrade,
        /// <summary>
        /// Fund percent post trade
        /// </summary>
        colFundPercentPostTrade,
        /// <summary>
        /// Missing weight percent post trade
        /// </summary>
        colMisweightPercentPostTrade,
        /// <summary>
        /// Missing weight value post trade
        /// </summary>
        colMisweightValuePostTrade,
        /// <summary>
        /// Order value
        /// </summary>
        colOrderValue,
        /// <summary>
        /// Column Id
        /// </summary>
        ColID,
        /// <summary>
        /// Last column
        /// </summary>
        colLast = ColID
    }

    /// <summary>
    /// Cell Image constants
    /// </summary>
    public enum enumCellImage
    {
        /// <summary>
        /// Checked
        /// </summary>
        Checked = 1,
        /// <summary>
        /// Minus
        /// </summary>
        Minus = 2,
        /// <summary>
        /// Plus
        /// </summary>
        Plus = 3,
        /// <summary>
        /// Unchecked
        /// </summary>
        Unchecked = 4
    }

    /// <summary>
    /// Sort by constants
    /// </summary>
    public enum SortByConstants
    {
        /// <summary>
        /// By row
        /// </summary>
        SortByRow = 0,
        /// <summary>
        /// By column
        /// </summary>
        SortByCol = 1
    }

    #endregion
}
