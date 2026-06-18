// Author: mrojas
// Project: Artinsoft.Windows.Forms
// Path: Artinsoft.Windows.Forms\ExtendedDataGridView
// Creation date: 7/16/2009 2:51 PM
// Last modified: 9/17/2009 11:11 AM

#region Using directives
using Artinsoft.Windows.Forms.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Artinsoft.Windows.Forms
{


    /// <summary>
    /// This partial class add functionality for compatibility with MSFlexGrid
    /// </summary>
    partial class ExtendedDataGridView
    {

        #region Enums

        /// <summary>
        /// Sort Constants
        /// </summary>
        public enum SortSettings
        {
            /// <summary>
            /// Generic ascending sort
            /// </summary>
            SortGenericAscending = 1,
            /// <summary>
            /// Generic descending sort
            /// </summary>
            SortGenericDescending = 2,
            /// <summary>
            /// No Sort
            /// </summary>
            SortNone = 0,
            /// <summary>
            /// Numeric ascending sort
            /// </summary>
            SortNumericAscending = 3,
            /// <summary>
            /// Numeric descending sort
            /// </summary>
            SortNumericDescending = 4,
            /// <summary>
            ///  String ascending sort, case-sensitive
            /// </summary>
            SortStringAscending = 7,
            /// <summary>
            /// String descending sort, case-sensitive
            /// </summary>
            SortStringDescending = 8,
            /// <summary>
            /// String ascending sort, case-insensitive
            /// </summary>
            SortStringNoCaseAscending = 5,
            /// <summary>
            /// String descending sort, case-insensitive
            /// </summary>
            SortStringNoCaseDescending = 6
        }

        /// <summary>
        /// FillStyle Constants
        /// </summary>
        public enum FillStyleSettings
        {
            /// <summary>
            /// The Style Changes applies only to the Current Cell
            /// </summary>
            FillSingle = 0,
            /// <summary>
            /// The Style Changes applies to the Selected Cells
            /// </summary>
            FillRepeat = 1
        }

        /// <summary>
        /// AllowUserResizing Constants
        /// </summary>
        public enum AllowUserResizingSettings
        {
            /// <summary>
            /// None of the Columns or Rows could be Resized.
            /// </summary>
            ResizeNone = 0,
            /// <summary>
            /// Just Columns could be resized
            /// </summary>
            ResizeColumns = 1,
            /// <summary>
            /// Just Rows could be resized
            /// </summary>
            ResizeRows = 2,
            /// <summary>
            /// Both Rows and Columns be resized
            /// </summary>
            ResizeBoth = 3
        }



        /// <summary>
        /// Indicates the type of the Focus drawn in the control
        /// </summary>
        public enum FocusRectSettings
        {
            /// <summary>
            /// No Focus
            /// </summary>
            FocusNone = 0,
            /// <summary>
            /// Focus rect drawn lightly
            /// </summary>
            FocusLight = 1,
            /// <summary>
            /// Focus rect drawn more heavy
            /// </summary>
            FocusHeavy = 2
        }

        /// <summary>
        /// Indicates the type of Line used in the control
        /// </summary>
        public enum GridLineSettings
        {
            /// <summary>
            /// No Grid Line
            /// </summary>
            GridNone = 0,
            /// <summary>
            /// Plain type
            /// </summary>
            GridFlat = 1,
            /// <summary>
            /// Intern lines
            /// </summary>
            GridInset = 2,
            /// <summary>
            /// Extern Lines
            /// </summary>
            GridRaised = 3
        }

        /// <summary>
        /// Indicates highLight Type
        /// </summary>
        public enum HighLightSettings
        {
            /// <summary>
            /// Never Highlights
            /// </summary>
            HighlightNever = 0,
            /// <summary>
            /// Highlights always
            /// </summary>
            HighlightAlways = 1,
            /// <summary>
            /// Highlight With Focus
            /// </summary>
            HighlightWithFocus = 2
        }


        /// <summary>
        /// ScrollBar Constants
        /// </summary>
        public enum ScrollBarStyle
        {
            /// <summary>
            /// Neither Horizontal or Vertical ScrollBar
            /// </summary>
            ScrollBarNone = 0,
            /// <summary>
            /// Only Horizontal ScrollBar
            /// </summary>
            ScrollBarHorizontal = 1,
            /// <summary>
            /// Only Vertical ScrollBar
            /// </summary>
            ScrollBarVertical = 2,
            /// <summary>
            /// Both, Horizontal and Vertical ScrollBar
            /// </summary>
            ScrollBarBoth = 3
        }

        /// <summary>
        /// Controls the Style of the text
        /// </summary>
        public enum TextStyleSettings
        {
            /// <summary>
            /// Flat Text
            /// </summary>
            TextFlat = 0,
            /// <summary>
            /// Raised Text
            /// </summary>
            TextRaised = 1,
            /// <summary>
            /// Inset Text
            /// </summary>
            TextInset = 2,
            /// <summary>
            /// Raised Light
            /// </summary>
            TextRaisedLight = 3,
            /// <summary>
            /// Inset Light
            /// </summary>
            TextInsetLight = 4
        }

        #endregion


        #region Properties

        internal new bool DesignMode
        {
            get
            {
                return base.DesignMode;
            }
        }

        internal DataGridViewSelectionMode BaseSelectionMode
        {
            get { return base.SelectionMode; }
            set { base.SelectionMode = value; }
        }

        private DataGridViewCell _GeneralCurrentCell;

        private DataGridViewCell GeneralCurrentCell
        {
            get
            {
                if (_GeneralCurrentCell == null)
                {
                    _GeneralCurrentCell = this.CurrentCell;
                } // if
                return _GeneralCurrentCell;
            } // get
        } 

        /// <summary>
        /// Avoids Focus Rectangle to be displayed. FocusRectangle is Managed in the Control
        /// </summary>
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }







        /// <summary>
        /// Returns/sets the total number of fixed (non-scrollable) columns or rows
        /// </summary>
        [Description("Returns/sets the total number of fixed (non-scrollable) columns or rows for a FlexGrid."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),DefaultValue(1)]
        public int FixedRows
        {
            get { return ((IFlexGridBehaviour)currentBehaviour).FixedRows; }
            set { ((IFlexGridBehaviour)currentBehaviour).FixedRows = value; }
        }

        /// <summary>
        /// Returns/sets the total number of fixed (non-scrollable) columns or rows for a FlexGrid.
        /// </summary>
        [Description("Returns/sets the total number of fixed (non-scrollable) columns or rows for a FlexGrid."), Browsable(true),  DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),DefaultValue(1)]
        public int FixedColumns
        {
            get
            {
                return ((IFlexGridBehaviour)currentBehaviour).FixedColumns;
            }
            set
            {
                ((IFlexGridBehaviour)currentBehaviour).FixedColumns = value;
            }
        }


        internal FillStyleSettings _fillStyle;

        /// <summary>
        /// Determines whether setting the Text property or one of the Cell formatting properties of a FlexGrid applies the change to all selected cells
        /// </summary>
        [Description("Determines whether setting the Text property or one of the Cell formatting properties of a FlexGrid applies the change to all selected cells"), Browsable(true), DefaultValue(ScrollBarStyle.ScrollBarBoth), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public FillStyleSettings FillStyle
        {
            get { return _fillStyle; }
            set { _fillStyle = value; }
        }

        /// <summary>
        /// Returns/sets the text contents of a cell or range of cells.
        /// </summary>
        [Description("Returns/sets the text contents of a cell or range of cells."), Browsable(true), DefaultValue(""), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Value + "" : base.Text; }
            set
            {
                if (GeneralCurrentCell != null)
                    GeneralCurrentCell.Value = value;
                else
                    base.Text = value;
            }
        }

        /// <summary>
        /// Returns/sets the text contents of a cell or range of cells.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CellText
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Value : null; }
            set
            {
                if (GeneralCurrentCell != null)
                    GeneralCurrentCell.Value = value;
            }
        }



        /// <summary>
        /// Returns/sets the background and foreground colors of individual cells or ranges of cells.
        /// Provides compatibility with MSFlexGrid CellBackColor
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CellBackColor
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.BackColor : BackColor; }
            set
            {
                if (_fillStyle == FillStyleSettings.FillSingle)
                {
                    if (GeneralCurrentCell != null)
                    {
                        if (!GeneralCurrentCell.HasStyle)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle(GeneralCurrentCell.Style);
                            style.BackColor = value;
                            GeneralCurrentCell.Style = style;
                        }
                        else
                        {
                            GeneralCurrentCell.Style.BackColor = value;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewCell dataGridViewCell in SelectedCells)
                    {
                        if (!dataGridViewCell.HasStyle)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle(dataGridViewCell.Style);
                            style.BackColor = value;
                            dataGridViewCell.Style = style;
                        }
                        else
                        {
                            dataGridViewCell.Style.BackColor = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns/sets the background and foreground colors of individual cells or ranges of cells.
        /// Provides compatibility with the MSFlexGrid CellForeColor
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color CellForeColor
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.ForeColor : ForeColor; }
            set
            {

                if (_fillStyle == FillStyleSettings.FillSingle)
                {
                    if (GeneralCurrentCell != null)
                    {
                        if (!GeneralCurrentCell.HasStyle)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle(GeneralCurrentCell.Style);
                            style.ForeColor = value;
                            GeneralCurrentCell.Style = style;
                        }
                        else
                        {
                            GeneralCurrentCell.Style.ForeColor = value;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewCell dataGridViewCell in SelectedCells)
                    {
                        if (!dataGridViewCell.HasStyle)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle(dataGridViewCell.Style);
                            style.ForeColor = value;
                            dataGridViewCell.Style = style;
                        }
                        else
                        {
                            dataGridViewCell.Style.ForeColor = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns/sets the alignment of data in a cell or range of selected cells. Not available at design time
        /// Provides compatibility with the MSFlexGrid CellAligment behaviour. DataGridViewContentAligment contants
        /// are used, but the property behaviour resembles the equivalent in MSFlexGrid
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewContentAlignment CellAlignment
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.Alignment : DataGridViewContentAlignment.NotSet; }
            set
            {
                if (_fillStyle == FillStyleSettings.FillSingle)
                {
                    if (GeneralCurrentCell != null)
                    {
                        if (!GeneralCurrentCell.HasStyle)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle(GeneralCurrentCell.Style);
                            style.Alignment = value;
                            GeneralCurrentCell.Style = style;
                        }
                        else
                        {
                            GeneralCurrentCell.Style.Alignment = value;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewCell dataGridViewCell in SelectedCells)
                    {
                        if (!dataGridViewCell.HasStyle)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle(dataGridViewCell.Style);
                            style.Alignment = value;
                            dataGridViewCell.Style = style;
                        }
                        else
                        {
                            dataGridViewCell.Style.Alignment = value;
                        }
                    }
                }
            }
        }


        /// <summary>
        ///  Thsi property is used to change the alignment of picture in a cell
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewContentAlignment CellPictureAlignment
        {
            get
            {
                if (GeneralCurrentCell != null)
                {
                    CustomCell cell = GeneralCurrentCell as CustomCell;
                    if (cell != null)
                    {
                        return cell.CellPictureAlignment;
                    }
                }
                return DataGridViewContentAlignment.NotSet;
            }
            set
            {
                if (GeneralCurrentCell != null)
                {
                    CustomCell cell = GeneralCurrentCell as CustomCell;
                    if (cell != null)
                        cell.CellPictureAlignment = value;
                }
            }
        }

        
        string _toolTipText = String.Empty;

        /// <summary>
        /// Gets/Sets the tool tip for the complete grid control
        /// </summary>
        public string ToolTipText
        {
            get
            {
                return _toolTipText;
            }
            set
            {
                _toolTipText = value;
            }
        }


        #region Font Settings

        private void SetCellFont(DataGridViewCell cell, Font font)
        {
            DataGridViewCellStyle style;
            if (cell.Style == DefaultCellStyle)
            {
                //Creates a new style for the Cell (Modifiying the DefaultCellStyle would change all the grid's Appereance)
                style = new DataGridViewCellStyle(DefaultCellStyle);
                cell.Style = style;
            }
            else
            {
                style = cell.Style;
            }
            style.Font = font;
        }

        private void SetCellFont(DataGridViewCell cell, FontStyle fontStyle, bool property)
        {
            DataGridViewCellStyle style = cell.Style;
            Font newFont;
            if (style.Font != null)
            {
                FontStyle newFontStyle = property ? style.Font.Style | fontStyle : style.Font.Style & ~fontStyle;
                newFont = new Font(style.Font, newFontStyle);
            }
            else
            {
                newFont = new Font(this.Font, property ? fontStyle : FontStyle.Regular);
            }

            SetCellFont(cell, newFont);
        }

        private void SetFontStyle(FontStyle style, bool property)
        {
            if (GeneralCurrentCell != null)
            {
                if (_fillStyle == FillStyleSettings.FillSingle)
                {
                    SetCellFont(GeneralCurrentCell, style, property);
                }
                else
                {
                    foreach (DataGridViewCell dataGridViewCell in SelectedCells)
                    {
                        SetCellFont(dataGridViewCell, style, property);
                    }
                }
            }
        }

        /// <summary>
        /// Returns or sets the bold style for the current cell text.
        /// Provides compatibility for MSFlexGrid
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CellFontBold
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.Font.Bold : false; }
            set { SetFontStyle(FontStyle.Bold, value); }
        }

        /// <summary>
        /// Returns or sets the bold style for the current cell text.
        /// Provides compatibility for MSFlexGrid
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CellFontStrikeOut
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.Font.Strikeout: false; }
            set { SetFontStyle(FontStyle.Strikeout, value); }
        }

        /// <summary>
        /// Returns or sets the italic style for the current cell text.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CellFontItalic
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.Font.Italic : false; }
            set { SetFontStyle(FontStyle.Italic, value); }
        }

        /// <summary>
        /// Returns or sets the underline style for the current cell text.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CellFontUnderline
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.Font.Underline : false; }
            set { SetFontStyle(FontStyle.Underline, value); }
        }

        /// <summary>
        /// Returns/sets the font to be used for individual cells or ranges of cells.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CellFontName
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.Font.Name : Font.Name; }
            set
            {
                string realName;
                switch (value)
                {
                    case "MS Sans Serif":
                        realName = "Microsoft Sans Serif";
                        break;
                    default:
                        realName = value;
                        break;
                }

                if (GeneralCurrentCell != null)
                {
                    if (_fillStyle == FillStyleSettings.FillSingle)
                    {
                        Font f = GeneralCurrentCell.Style.Font;
                        f = new Font(realName, f.Size, f.Style, f.Unit);
                        SetCellFont(GeneralCurrentCell, f);
                    }
                    else
                    {
                        foreach (DataGridViewCell dataGridViewCell in SelectedCells)
                        {
                            Font f = dataGridViewCell.Style.Font;
                            f = new Font(realName, f.Size, f.Style, f.Unit);
                            SetCellFont(dataGridViewCell, f);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns or sets the size, in points, for the current cell text.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float CellFontSize
        {
            get { return GeneralCurrentCell != null ? GeneralCurrentCell.Style.Font.Size : Font.Size; }
            set
            {
                if (GeneralCurrentCell != null)
                {
                    if (_fillStyle == FillStyleSettings.FillSingle)
                    {
                        Font f = GeneralCurrentCell.Style.Font;
                        f = new Font(f.Name, value, f.Style, f.Unit);
                        SetCellFont(GeneralCurrentCell, f);
                    }
                    else
                    {
                        foreach (DataGridViewCell dataGridViewCell in SelectedCells)
                        {
                            Font f = dataGridViewCell.Style.Font;
                            f = new Font(f.Name, value, f.Style, f.Unit);
                            SetCellFont(dataGridViewCell, f);
                        }
                    }
                }
            }
        }

        #endregion

  

        /// <summary>
        /// Determines the starting or ending row or column for a range of cells. Not available at design time.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ColSel
        {
            get
            {
                return ((IFlexGridBehaviour)currentBehaviour).ColSel;
            }
            set
            {
                ((IFlexGridBehaviour)currentBehaviour).ColSel = value;
            }
        }

        /// <summary>
        /// Determines the starting or ending row or column for a range of cells. Not available at design time.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RowSel
        {
            get
            {
                return ((IFlexGridBehaviour)currentBehaviour).RowSel;
            }
            set
            {
                ((IFlexGridBehaviour)currentBehaviour).RowSel = value;
            }
        }










        /// <summary>
        /// Returns/sets the contents of the cells in a FlexGrid's selected region. Not available at design time.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Clip
        {
            get
            {
                ClipClass clipclass = new ClipClass();
                return clipclass.GetClip(GridCellCollectionToArray(SelectedCells));
            }
            set
            {
                
                ClipClass clipclass = new ClipClass();
                clipclass.SetClip(GridCellCollectionToArray(SelectedCells), value);
            }
        }

		private static DataGridViewCell[] GridCellCollectionToArray(DataGridViewSelectedCellCollection collection)
		{
			DataGridViewCell[] cells = new DataGridViewCell[collection.Count];
			int x = 0;
			foreach (DataGridViewCell cell in collection)
			{
				cells[x] = cell;
				x++;
			}
			return cells;
		}

        private Color? _foreColorFixed;
        /// <summary>
        ///  Determines the color used to draw text on each part of the FlexGrid.
        /// </summary>
        [Description(" Determines the color used to draw text on each part of the FlexGrid."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ForeColorFixed
        {
            get
            {
                if (_foreColorFixed == null)
                {
                    _foreColorFixed = ColumnHeadersDefaultCellStyle.ForeColor;
                }
                return _foreColorFixed.Value;
            }
            set
            {
                ColumnHeadersDefaultCellStyle.ForeColor = value;
                ColumnHeadersDefaultCellStyle.SelectionForeColor = value;
                RowHeadersDefaultCellStyle.ForeColor = value;
                RowHeadersDefaultCellStyle.SelectionForeColor = value;
                TopLeftHeaderCell.Style.ForeColor = value;
                TopLeftHeaderCell.Style.SelectionForeColor = value;

                foreach (DataGridViewColumn column in Columns)
                {
                    column.HeaderCell.Style.ForeColor = value;
                }

                foreach (DataGridViewRow row in base.Rows)
                {
                    row.HeaderCell.Style.ForeColor = value;
                }

                TopLeftHeaderCell.Style.ForeColor = value;
                _foreColorFixed = value;
            }
        }


        /// <summary>
        /// Returns/sets the color as the background color for all fixed cells.
        /// </summary>
        [Description("Returns/sets the color as the background color for all fixed cells. Provided for compatibility with MSFlexGrid BackColorFixed property"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackColorFixed
        {
            get { return ((IFlexGridBehaviour)currentBehaviour).BackColorFixed; }
            set
            {
                ((IFlexGridBehaviour)currentBehaviour).BackColorFixed = value;
            }
        }

        /// <summary>
        /// Returns/sets the color used to draw the lines between FlexGrid cells.
        /// </summary>
        [Description("Returns/sets the color used to draw the lines between FlexGrid cells."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Obsolete("Use Grid Color Instead")]
        public Color GridColorFixed
        {
            get { return GridColor; }
            set { GridColor = value; }
        }






        /// <summary>
        /// Returns/sets whether selected cells appear highlighted.
        /// </summary>
        [Description("Returns/sets whether selected cells appear highlighted."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public HighLightSettings HighLight
        {
            get { return ((IFlexGridBehaviour)currentBehaviour).HighLight; }
            set
            {
                ((IFlexGridBehaviour)currentBehaviour).HighLight = value;
            }
        }

        internal DataGridViewCell BaseCurrentCell
        {
            get
            {
                return base.CurrentCell;
            }
            set
            {
                base.CurrentCell = value;
            }
        }


        internal int BaseFirstDisplayedScrollingRowIndex
        {
            get
            {
                return base.FirstDisplayedScrollingRowIndex;
            }
            set
            {
                base.FirstDisplayedScrollingRowIndex = value;
            }
        }

        internal int BaseFirstDisplayedScrollingColumnIndex
        {
            get
            {
                return base.FirstDisplayedScrollingColumnIndex;
            }
            set
            {
                base.FirstDisplayedScrollingColumnIndex = value;
            }
        }



        internal DataGridViewTriState BaseWordWrap
        {
            get
            {
                return base.DefaultCellStyle.WrapMode;
            }
            set
            {
                base.DefaultCellStyle.WrapMode = value;
            }
        }


      

        

        /// <summary>
        /// Determines whether the FlexGrid control should draw a focus rectangle around the current cell.
        /// </summary>
        [Description("Determines whether the FlexGrid control should draw a focus rectangle around the current cell."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(FocusRectSettings.FocusLight)]
        public FocusRectSettings FocusRect
        {
            get { return ((IFlexGridBehaviour)currentBehaviour).FocusRect; }
            set
            {
                ((IFlexGridBehaviour)currentBehaviour).FocusRect = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes a row from a FlexGrid control at run time
        /// </summary>
        /// <param name="index">The index of the Row</param>
        public void RemoveItem(int index)
        {
            if (index == 0)
            {
                if (ColumnHeadersVisible)
                    throw new InvalidOperationException("It's not possible to remove a Fixed Row");
            }
            index = ColumnHeadersVisible ? index - 1 : index;
            base.Rows.RemoveAt(index);
        }

        internal DataGridViewCell BaseGetCell(int columnindex, int rowindex)
        {
            return base[columnindex, rowindex];
        }

        internal void BaseSetCell(int columnindex, int rowindex,DataGridViewCell cell)
        {
            base[columnindex, rowindex] = cell;
        }

        private object ColumnHeadersTag;
        private object RowHeadersTag;

        /// <summary>
        /// Gets Array of long integer values with one item for each row (RowData) of the FlexGrid. Not available at design time.
        /// </summary>
        /// <param name="index">The index of the Row</param>
        /// <returns>The Data Stored on the Row</returns>
        public int get_RowData(int index)
        {
            if (index == 0)
            {
                if (ColumnHeadersVisible)
                    return ColumnHeadersTag != null ? (int)ColumnHeadersTag : 0;
            }

            int realindex = ColumnHeadersVisible ? index - 1 : index;
            return base.Rows[realindex].Tag != null ? (int)base.Rows[realindex].Tag : 0;
        }

        /// <summary>
        /// Sets Array of long integer values with one item for each row (RowData) of the FlexGrid. Not available at design time.
        /// </summary>
        /// <param name="index">The index of the Row</param>
        /// <param name="value">The Data to be Stored on the Row</param>
        public void set_RowData(int index, int value)
        {
            if (index == 0)
            {
                if (ColumnHeadersVisible)
                {
                    ColumnHeadersTag = value;
                    return;
                }
            }
            int realindex = ColumnHeadersVisible ? index - 1 : index;
            base.Rows[realindex].Tag = value;
        }

		/// <summary>
		/// This class manages the Column Data of a specified grid.
		/// </summary>
        public class ColDataProperty
        {
            internal ExtendedDataGridView parent;
			/// <summary>
			/// Creates a ColDataProperty class for a specified grid.
			/// </summary>
			/// <param name="parent">The grid for which this class should be created.</param>
            public ColDataProperty(ExtendedDataGridView parent) { this.parent = parent; }
			/// <summary>
			/// Gets/sets the Column Data property for the specified column.
			/// </summary>
			/// <param name="index">The index of the column.</param>
			/// <returns>The column data of the selected column.</returns>
            public int this[int index]
            {
                get
                {
                    if (index == 0)
                    {
                        if (parent.RowHeadersVisible)
                            return parent.RowHeadersTag != null ? (int)parent.RowHeadersTag : 0;
                    }
                    int realindex = parent.RowHeadersVisible ? index - 1 : index;
                    return parent.Columns[realindex].Tag != null ? (int)parent.Columns[realindex].Tag : 0;
                }
                set
                {
                    if (index == 0)
                    {
                        if (parent.RowHeadersVisible)
                        {
                            parent.RowHeadersTag = value;
                            return;
                        }
                    }
                    int realindex = parent.RowHeadersVisible ? index - 1 : index;
                    parent.Columns[realindex].Tag = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets Array of long integer values with one item for each column (ColData) of the FlexGrid.
        /// Not available at design time.
        /// </summary>
        [Browsable(false)]
        public ColDataProperty ColData
        {
            get
            {
                return new ColDataProperty(this);
            }
        }


        /// <summary>
        /// Returns True if the specified column is visible.
        /// </summary>
        /// <param name="index">The index of the Column</param>
        /// <returns></returns>
        public bool get_ColIsVisible(int index)
        {
            if (index == 0)
            {
                return RowHeadersVisible;
            }
            int realindex = index - 1;
            return base.Columns[realindex].Visible;
        }

        /// <summary>
        /// Returns True if the specified row is visible.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RowIsVisible(int index)
        {
            if (index == 0)
            {
                return ColumnHeadersVisible;
            }
            int realindex = index - 1;
            return base.Rows[realindex].Visible;
        }




        /// <summary>
        /// Sets/Gets the alignment of data in the fixed cells of a column.
        /// </summary>
        [Browsable(false)]
        public FixedAlignmentProperty FixedAlignment
        {
            get
            {
                return new FixedAlignmentProperty(this);
            }

        }

        /// <summary>
        /// Gets/Sets the alignment of data in a column. 
        /// Not available at design time (except indirectly through the FormatString property).
        /// </summary>
        [Browsable(false)]
        public ColAlignmentProperty ColAlignment
        {
            get
            {
                return new ColAlignmentProperty(this);
            }
        }


        /// <summary>
        /// Gets the distance in Pixels between the upper-left corner of the control and the upper-left corner of a specified column.
        /// </summary>
        [Browsable(false)]
        public ColPosProperty ColPos
        {
            get
            {
                return new ColPosProperty(this);
            }
        }

        /// <summary>
        /// Gets the distance in Pixels between the upper-left corner of the control and the upper-left corner of a specified row.
        /// </summary>
        [Browsable(false)]
		public RowPosProperty RowPos
		{
			get
			{
				return new RowPosProperty(this);
			}
		}



        /// <summary>
        /// Clears the contents of the FlexGrid. This includes all text, pictures, and cell formatting.
        /// </summary>
        public void Clear()
        {
            foreach (DataGridViewColumn col in base.Columns)
            {
                ClearCell(col.HeaderCell);
            } // foreach
            foreach (DataGridViewRow row in base.Rows)
            {
                ClearCell(row.HeaderCell);
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell!=null)
                    {
                        ClearCell(cell);
                    } // if (cell!=null
                }
            }           
        }

        private static void ClearCell(DataGridViewCell cell)
        {
            cell.ErrorText = String.Empty;
            cell.Value = null;
            cell.ToolTipText = String.Empty;
            cell.Style = new DataGridViewCellStyle();
        }

        /// <summary>
        /// Adds a new row to a FlexGrid control at run time.
        /// </summary>
        /// <param name="vsItem">String for elements to add</param>
        public void AddItem(string vsItem)
        {
            string[] items = vsItem.Split(new char[] { '\t' });
            DataGridViewRow row = new DataGridViewRow();
            row.Height = RowTemplate.Height;
            row.MinimumHeight = RowTemplate.MinimumHeight;
            Rows.Add(row);
            for (int i = 0; i < ColumnsCount && i < items.Length; i++)
            {
                GetCell(RowsCount - 1, i).Value = items[i];
            }

        }

        /// <summary>
        /// Adds a new row to a FlexGrid control at run time.
        /// </summary>
        /// <param name="vsItem">String for elements to add</param>
        /// <param name="viIndex">New Row Position</param>
        public void AddItem(string vsItem, int viIndex)
        {
            string[] items = vsItem.Split(new char[] { '\t' });
            DataGridViewRow row = new DataGridViewRow();
            row.Height = RowTemplate.Height;
            row.MinimumHeight = RowTemplate.MinimumHeight;

            if (viIndex < FixedRows)
                throw new InvalidOperationException("Cannot use AddItem on a fixed row");
            int realIndex = ColumnHeadersVisible ? viIndex - 1 : viIndex;
            Rows.Insert(realIndex, row);
            for (int i = 0; i < ColumnsCount && i < items.Length; i++)
            {
                GetCell(viIndex, i).Value = items[i];
            }
        }

        /// <summary>
        /// Changes the value, if the setted value contains \0 chars, then those are removed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments</param>
        void ExtendedDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCell cell;

            if (e.ColumnIndex < 0 && e.RowIndex < 0)
            {

                cell = TopLeftHeaderCell;

            }

            else if (e.ColumnIndex < 0)
            {

                cell = Rows[e.RowIndex].HeaderCell;

            }

            else if (e.RowIndex < 0)
            {

                cell = Columns[e.ColumnIndex].HeaderCell;

            }

            else
            {

                cell = this[e.ColumnIndex, e.RowIndex];

            }





            string value = Convert.ToString(cell.Value);

            if (value.IndexOf('\0') != -1)
            {

                cell.Value = value.Substring(0, value.IndexOf('\0'));

            }

        }





        #endregion
        
        #region Support Classes

		/// <summary>
		/// Class used to manage the Alignment properties of the columns of a grid.
		/// </summary>
        public class ColAlignmentProperty
        {
            internal ExtendedDataGridView parent;
			/// <summary>
			/// Creates a ColumnAlignmentProperty class for a specified grid.
			/// </summary>
			/// <param name="parent">The grid for which to create this class.</param>
            public ColAlignmentProperty(ExtendedDataGridView parent)
            { this.parent = parent; }
			/// <summary>
			/// Gets/sets the Alignment property for a specified column.
			/// </summary>
			/// <param name="index">The index of the column.</param>
			/// <returns>The Alignment property of the selected column.</returns>
            public DataGridViewContentAlignment this[int index]
            {
                get
                {
                    if (index == 0)
                    {
                        if (parent.RowHeadersVisible)
                        {
                            return parent.RowHeadersDefaultCellStyle.Alignment;
                        }
                    }
                    int realindex = parent.RowHeadersVisible ? index - 1 : index;
                    DataGridViewColumn column = parent.Columns[realindex];
                    return column.DefaultCellStyle.Alignment;

                }
                set
                {
                    if (index == 0)
                    {
                        if (parent.RowHeadersVisible)
                        {
                            parent.RowHeadersDefaultCellStyle.Alignment = value;
                            return;
                        }
                    }
                    int realindex = parent.RowHeadersVisible ? index - 1 : index;
                    DataGridViewColumn column = parent.Columns[realindex];
                    if (column.CellTemplate.Style == parent.DefaultCellStyle)
                    {
                        DataGridViewCellStyle style = new DataGridViewCellStyle(column.CellTemplate.Style);
                        style.Alignment = value;
                        column.CellTemplate.Style = style;
                        column.DefaultCellStyle = style;
                        if (column.HeaderCell.HasStyle)
                        {
                            if (column.HeaderCell.Style.Alignment == DataGridViewContentAlignment.NotSet)
                            {
                                column.HeaderCell.Style.Alignment = value;
                            }
                        }
                        else
                        {
                            column.HeaderCell.Style.Alignment = value;
                        }

                        foreach (DataGridViewRow row in parent.Rows)
                        {
                            if (row.Cells[realindex].Style == parent.DefaultCellStyle)
                            {
                                row.Cells[realindex].Style = style;
                            }
                            else
                            {
                                row.Cells[realindex].Style.Alignment = value;
                            }
                        }
                    }
                    else
                    {
                        column.CellTemplate.Style.Alignment = value;
                        column.DefaultCellStyle.Alignment = value;
                        if (column.HeaderCell.HasStyle)
                        {
                            if (column.HeaderCell.Style.Alignment == DataGridViewContentAlignment.NotSet)
                            {
                                column.HeaderCell.Style.Alignment = value;
                            }
                        }
                        else
                        {
                            column.HeaderCell.Style.Alignment = value;
                        }

                    }
                }
            }
        }

		/// <summary>
		/// Class used to access the indexed ColPos property.
		/// </summary>
		public class ColPosProperty
        {
            internal ExtendedDataGridView parent;
			/// <summary>
			/// Creates a ColPosProperty class for the specified grid.
			/// </summary>
			/// <param name="parent">The grid to use to create the ColPosProperty class.</param>
            public ColPosProperty(ExtendedDataGridView parent)
            {
                this.parent = parent;
            }
			/// <summary>
			/// Enumerate the columns of the grid.
			/// </summary>
			/// <param name="index">The index of the column</param>
			/// <returns>The ColPos value for the specified column.</returns>
            public int this[int index]
            {
                get
                {
                    if (index == 0)
                        return 0;
                    else
                    {
						int result = parent.RowHeadersVisible ? parent.RowHeadersWidth : 0;
						if (parent.FirstDisplayedCell.ColumnIndex <= index)
						{
							for (int i = parent.FirstDisplayedCell.ColumnIndex; i < index - 1; i++)
							{
								result += parent.Columns[i].Width;
							}
						}
						else
						{
							for (int i = parent.FirstDisplayedCell.ColumnIndex; i > index - 1; i--)
							{
								result -= parent.Columns[i - 1].Width;
							}
						}
						return result;
                    }
                }

            }

        }

		/// <summary>
		/// Enumeration class used to access the indexed ColPos property.
		/// </summary>
		public class RowPosProperty
		{
			internal ExtendedDataGridView parent;
			/// <summary>
			/// Creates a RowPosProperty class for the specified grid.
			/// </summary>
			/// <param name="parent">The grid to use to create the RowPosProperty class.</param>
			public RowPosProperty(ExtendedDataGridView parent)
			{
				this.parent = parent;
			}
			/// <summary>
			/// Enumerate the rows of the grid.
			/// </summary>
			/// <param name="index">The index of the rows</param>
			/// <returns>The RowPos value for the specified row.</returns>
			public int this[int index]
			{
				get
				{
					if (index == 0)
						return 0;
					else
					{
						int result = parent.ColumnHeadersVisible ? parent.ColumnHeadersHeight : 0;
						if (parent.FirstDisplayedCell.RowIndex <= index)
						{
							for (int i = parent.FirstDisplayedCell.RowIndex; i < index - 1; i++)
							{
								result += parent.Rows[i].Height;
							}
						}
						else
						{
							for (int i = parent.FirstDisplayedCell.RowIndex; i > index - 1; i--)
							{
								result -= parent.Rows[i - 1].Height;
							}
						}
						return result;
					}
				}

			}

		}

		/// <summary>
		/// Enumeration class used to access the indexed FixedAlignment property.
		/// </summary>
        public class FixedAlignmentProperty
        {
            internal ExtendedDataGridView parent;
			/// <summary>
			/// Creates a FixedAlignmentProperty class for a specified grid.
			/// </summary>
			/// <param name="parent">The grid for which to create the class.</param>
            public FixedAlignmentProperty(ExtendedDataGridView parent) { this.parent = parent; }
			/// <summary>
			/// Obtains the FixedAlignment property for the specified column.
			/// </summary>
			/// <param name="index">The index of the column.</param>
			/// <returns>The Fixed Alignment of the column.</returns>
            public DataGridViewContentAlignment this[int index]
            {
                get
                {
                    if (index == 0)
                    {
                        if (parent.RowHeadersVisible)
                        {
                            return parent.RowHeadersDefaultCellStyle.Alignment;
                        }
                    }
                    int realindex = parent.RowHeadersVisible ? index - 1 : index;
                    DataGridViewColumn column = parent.Columns[realindex];
                    if (column.Frozen)
                        return column.DefaultCellStyle.Alignment;
                    else
                        return DataGridViewContentAlignment.NotSet;
                }
                set
                {
                    if (index == 0)
                    {
                        if (parent.RowHeadersVisible)
                        {
                            parent.RowHeadersDefaultCellStyle.Alignment = value;
                            return;
                        }
                    }
                    int realindex = parent.RowHeadersVisible ? index - 1 : index;
                    DataGridViewContentAlignment align = value;
                    align = align == DataGridViewContentAlignment.NotSet ? DataGridViewContentAlignment.MiddleLeft : align;
                    DataGridViewColumn column = parent.Columns[realindex];
                    if (column.Frozen)
                        column.DefaultCellStyle.Alignment = align;
                    else
                        column.HeaderCell.Style.Alignment = align;
                }
            }
        }


        private class ClipClass
        {
            private int minrow, mincol, maxrow, maxcol, rowcount, colcount;

            private void InitValues(DataGridViewCell[] cells)
            {
                minrow = int.MaxValue;
                mincol = int.MaxValue;
                maxrow = int.MinValue;
                maxcol = int.MinValue;
                rowcount = 0;
                colcount = 0;
                List<int> rows = new List<int>();
                List<int> cols = new List<int>();

                foreach (DataGridViewCell cell in cells)
                {
                    if (!rows.Contains(cell.RowIndex))
                    {
                        rows.Add(cell.RowIndex);
                        if (cell.RowIndex < minrow)
                            minrow = cell.RowIndex;

                        if (cell.RowIndex > maxrow)
                            maxrow = cell.RowIndex;
                    }
                    if (!cols.Contains(cell.ColumnIndex))
                    {
                        cols.Add(cell.ColumnIndex);
                        if (cell.ColumnIndex < mincol)
                            mincol = cell.ColumnIndex;

                        if (cell.ColumnIndex > maxcol)
                            maxcol = cell.ColumnIndex;
                    }
                }
                rowcount = rows.Count;
                colcount = cols.Count;
            }

            public string GetClip(DataGridViewCell[] cells)
            {
                if (cells.Length == 0)
                    return "";

                string[][] Content = GetContent(cells);
                return FormatContent(Content);
            }

            public string[][] GetContent(DataGridViewCell[] cells)
            {
                //Calculates the min, max and the count of rows and cols
                InitValues(cells);

                string[][] Content = new string[rowcount][];
                for (int i = 0; i < rowcount; i++)
                {
                    Content[i] = new string[colcount];
                }

                foreach (DataGridViewCell cell in cells)
                {
                    int rowpos = cell.RowIndex - minrow;
                    int colpos = cell.ColumnIndex - mincol;

                    Content[rowpos][colpos] = cell.Value + "";
                }
                return Content;
            }

            public string FormatContent(string[][] Content)
            {
                //Pass the content to a String
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < rowcount; i++)
                {
                    for (int j = 0; j < colcount; j++)
                    {
                        stringBuilder.Append(Content[i][j]);
                        if (j + 1 != colcount)
                            stringBuilder.Append("\t");
                    }
                    if (i + 1 != rowcount)
                        stringBuilder.AppendLine();
                }
                return stringBuilder.ToString();
            }

            public void SetClip(DataGridViewCell[] cells, string value)
            {
                if (cells.Length == 0)
                    return;

                //Calculates the min, max and the count of rows and cols
                InitValues(cells);
                string[][] Content = new string[rowcount][];
                for (int i = 0; i < rowcount; i++)
                {
                    Content[i] = new string[colcount];
                }

                string[] rowValues = value.Split('\n', '\r');
                for (int i = 0; i < rowValues.Length && i + minrow <= maxrow; i++)
                {
                    string[] colValues = rowValues[i].Split('\t');
                    for (int j = 0; j < colValues.Length && j + mincol <= maxcol; j++)
                    {
                        Content[i][j] = colValues[j].Trim();
                    }
                }
                SetContent(Content, cells);
            }

            public void SetContent(string[][] Content, DataGridViewCell[] cells)
            {
                //Pass the value to the selected cells
                foreach (DataGridViewCell cell in cells)
                {
                    int rowpos = cell.RowIndex - minrow;
                    int colpos = cell.ColumnIndex - mincol;
                    if (Content[rowpos][colpos] != null)
                        cell.Value = Content[rowpos][colpos];
                }
            }
        }

		/// <summary>
		/// This class represents a custom column to be used with the grid.
		/// </summary>
        public class CustomColumn : DataGridViewColumn
        {
			/// <summary>
			/// Creates a new CustomColumn with CustomCell template.
			/// </summary>
            public CustomColumn() : base(new CustomCell())
            {

            }


			/// <summary>
			/// Gets a copy of the CustomColumn.
			/// </summary>
			/// <returns>A copy of the column.</returns>
            public override object Clone()
            {
                CustomColumn col = base.Clone() as CustomColumn;
                col.CellTemplate = this.CellTemplate;
                return col;
            }

        }

        internal class CustomCell : DataGridViewTextBoxCell
        {
            #region "Properties"
            private Image _cellPicture;
            /// <summary>
            /// Gets/Sets the Image of the Cell
            /// </summary>
            [DefaultValue(null)]
            public Image CellPicture
            {
                get { return _cellPicture; }
                set { _cellPicture = value; }
            }

            private DataGridViewContentAlignment _cellPictureAlignment = DataGridViewContentAlignment.NotSet;
            /// <summary>
            /// Gets/Sets the Alignement of the CellPicture
            /// </summary>
            [DefaultValue(DataGridViewContentAlignment.NotSet)]
            public DataGridViewContentAlignment CellPictureAlignment
            {
                get { return _cellPictureAlignment; }
                set { _cellPictureAlignment = value; }
            }

            private DataGridViewImageCellLayout _imageLayout = DataGridViewImageCellLayout.NotSet;
            /// <summary>
            /// Gets/Sets the ImageLayout
            /// </summary>
            [DefaultValue(0)]
            public DataGridViewImageCellLayout ImageLayout
            {
                get { return _imageLayout; }
                set { _imageLayout = value; }
            }

            /// <summary>
            /// Indicates if the Cell has the focus
            /// </summary>
            public bool Focused
            {
                get
                {
                    ExtendedDataGridView grid = ParentGrid;
                    return grid != null && grid.GeneralCurrentCell == this && grid.FocusRect != FocusRectSettings.FocusNone;
                }
            }

            /// <summary>
            /// Indicates if the Cell must be HighLighted or not
            /// </summary>
            public bool HighLighted
            {
                get
                {
                    ExtendedDataGridView grid = ParentGrid;
                    return grid.HighLight != HighLightSettings.HighlightNever;
                }
            }

            /// <summary>
            /// Returns the XDataGridView that contains this cell
            /// </summary>
            public ExtendedDataGridView ParentGrid
            {
                get { return this.DataGridView as ExtendedDataGridView; }
            }

            /// <summary>
            /// Indicates if the Parent has the focus
            /// </summary>
            public bool ParentIsFocused
            {
                get
                {
                    ExtendedDataGridView grid = ParentGrid;
                    return grid != null && grid.Focused;
                }
            }

            /// <summary>
            /// Returns the type of the Focus
            /// </summary>
            public FocusRectSettings FocusRect
            {
                get
                {
                    ExtendedDataGridView grid = ParentGrid;
                    return grid != null ? grid.FocusRect : FocusRectSettings.FocusNone;
                }
            }
            #endregion

            #region "Methods"


            private static Rectangle ImgBounds(Rectangle bounds, int imgWidth, int imgHeight, DataGridViewImageCellLayout imageLayout, DataGridViewContentAlignment Alignment)
            {
                Rectangle empty = Rectangle.Empty;
                switch (imageLayout)
                {
                    case DataGridViewImageCellLayout.NotSet:
                    case DataGridViewImageCellLayout.Normal:
                        empty = new Rectangle(bounds.X, bounds.Y, imgWidth, imgHeight);
                        break;

                    case DataGridViewImageCellLayout.Zoom:
                        if ((imgWidth * bounds.Height) >= (imgHeight * bounds.Width))
                        {
                            empty = new Rectangle(bounds.X, bounds.Y, bounds.Width, decimal.ToInt32((imgHeight * bounds.Width) / imgWidth));
                            break;
                        }
                        empty = new Rectangle(bounds.X, bounds.Y, decimal.ToInt32((imgWidth * bounds.Height) / imgHeight), bounds.Height);
                        break;
                }
                switch (Alignment)
                {
                    case DataGridViewContentAlignment.MiddleRight:
                        empty.X = bounds.Right - empty.Width;
                        goto Label_025B;

                    case DataGridViewContentAlignment.BottomLeft:
                        empty.X = bounds.X;
                        goto Label_025B;

                    case DataGridViewContentAlignment.BottomRight:
                        empty.X = bounds.Right - empty.Width;
                        goto Label_025B;

                    case DataGridViewContentAlignment.TopLeft:
                        empty.X = bounds.X;
                        goto Label_025B;

                    case DataGridViewContentAlignment.TopRight:
                        empty.X = bounds.Right - empty.Width;
                        goto Label_025B;

                    case DataGridViewContentAlignment.MiddleLeft:
                        empty.X = bounds.X;
                        goto Label_025B;
                }
            Label_025B:
                switch (Alignment)
                {
                    case DataGridViewContentAlignment.TopCenter:
                    case DataGridViewContentAlignment.MiddleCenter:
                    case DataGridViewContentAlignment.BottomCenter:
                        empty.X = bounds.X + ((bounds.Width - empty.Width) / 2);
                        break;
                }
                DataGridViewContentAlignment alignment = Alignment;
                if (alignment <= DataGridViewContentAlignment.MiddleCenter)
                {
                    switch (alignment)
                    {
                        case DataGridViewContentAlignment.TopLeft:
                        case DataGridViewContentAlignment.TopCenter:
                        case DataGridViewContentAlignment.TopRight:
                            empty.Y = bounds.Y;
                            return empty;

                        case (DataGridViewContentAlignment.TopCenter | DataGridViewContentAlignment.TopLeft):
                            return empty;

                        case DataGridViewContentAlignment.MiddleLeft:
                        case DataGridViewContentAlignment.MiddleCenter:
                            goto Label_030C;
                    }
                    return empty;
                }
                if (alignment <= DataGridViewContentAlignment.BottomLeft)
                {
                    switch (alignment)
                    {
                        case DataGridViewContentAlignment.MiddleRight:
                            goto Label_030C;

                        case DataGridViewContentAlignment.BottomLeft:
                            goto Label_032E;
                    }
                    return empty;
                }
                switch (alignment)
                {
                    case DataGridViewContentAlignment.BottomCenter:
                    case DataGridViewContentAlignment.BottomRight:
                        goto Label_032E;

                    default:
                        return empty;
                }
            Label_030C:
                empty.Y = bounds.Y + ((bounds.Height - empty.Height) / 2);
                return empty;
            Label_032E:
                empty.Y = bounds.Bottom - empty.Height;
                return empty;
            }

            private void PaintImage(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates elementState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint, Color backcolor)
            {
                Rectangle cellValueBounds = cellBounds;
                Rectangle rectangle3 = this.BorderWidths(advancedBorderStyle);
                cellValueBounds.Offset(rectangle3.X, rectangle3.Y);
                cellValueBounds.Width -= rectangle3.Right;
                cellValueBounds.Height -= rectangle3.Bottom;

                Rectangle destRect = cellValueBounds;
                if (cellStyle.Padding != Padding.Empty)
                {
                    destRect.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
                    destRect.Width -= cellStyle.Padding.Horizontal;
                    destRect.Height -= cellStyle.Padding.Vertical;
                }

                if ((destRect.Width > 0) && (destRect.Height > 0))
                {
                    Image image = formattedValue as Image;
                    Icon icon = null;
                    if (image == null)
                    {
                        icon = formattedValue as Icon;
                    }
                    if ((icon != null) || (image != null))
                    {
                        if (paint)
                        {
                            switch (ImageLayout)
                            {
                                case DataGridViewImageCellLayout.NotSet:

                                    break;
                                case DataGridViewImageCellLayout.Normal:
                                    break;
                                case DataGridViewImageCellLayout.Stretch:
                                    if (paint)
                                    {
                                        if (image != null)
                                        {
                                            ImageAttributes imageAttr = new ImageAttributes();
                                            imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                                            g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                                            imageAttr.Dispose();
                                        }
                                        else
                                        {
                                            g.DrawIcon(icon, destRect);
                                        }
                                    }
                                    break;
                            }
                        }
                    }

                    if (image != null)
                    {
                        g.FillRectangle(new SolidBrush(backcolor), destRect);
                    }
                    Rectangle empty = ImgBounds(destRect, (image == null) ? icon.Width : image.Width, (image == null) ? icon.Height : image.Height, ImageLayout, CellPictureAlignment);
                    if (paint)
                    {
                        Region clip = g.Clip;
                        g.SetClip(Rectangle.Intersect(Rectangle.Intersect(empty, destRect), Rectangle.Truncate(g.VisibleClipBounds)));
                        if (image != null)
                        {
                            g.DrawImage(image, empty);
                        }
                        else
                        {
                            g.DrawIconUnstretched(icon, empty);
                        }
                        g.Clip = clip;
                    }
                }
            }
            #endregion
        }

        #endregion

        
        /// <summary>
        /// Returns the Cell in the given Row and Column
        /// </summary>
        /// <param name="rowindex">The Row</param>
        /// <param name="colIndex">The Column</param>
        /// <returns>The Cell specified</returns>
        public DataGridViewCell GetCell(int rowindex, int colIndex)
        {
            if (colIndex < 0 || rowindex < 0)
                return null;
            else if (colIndex == 0 && rowindex == 0)
            {
                if (RowHeadersVisible && ColumnHeadersVisible)
                    return TopLeftHeaderCell;
                else if (RowHeadersVisible)
                    return base.Rows[0].HeaderCell;
                else if (ColumnHeadersVisible)
                    return Columns[0].HeaderCell;
            }
            else if (colIndex == 0)
            {
                if (RowHeadersVisible && ColumnHeadersVisible)
                    return base.Rows[rowindex - 1].HeaderCell;
                else if (RowHeadersVisible)
                {
                    return base.Rows[rowindex].HeaderCell;
                }
                else if (ColumnHeadersVisible)
                    return this[0, rowindex - 1];
            }
            else if (rowindex == 0)
            {
                if (RowHeadersVisible && ColumnHeadersVisible)
                    return Columns[colIndex - 1].HeaderCell;
                else if (RowHeadersVisible)
                    return this[colIndex - 1, 0];
                else if (ColumnHeadersVisible)
                    return Columns[colIndex].HeaderCell;
            }

            if (ColumnHeadersVisible && RowHeadersVisible)
            {
                int realCol = ColumnHeadersVisible ? colIndex : colIndex - 1;
                int realRow = RowHeadersVisible ? rowindex : rowindex - 1;
                return this[realRow, realCol];
            }
            else if (RowHeadersVisible)
            {
                int realCol = colIndex - 1;
                int realRow = rowindex;
                return this[realCol, realRow];
            }
            else if (ColumnHeadersVisible)
            {
                int realCol = colIndex;
                int realRow = rowindex - 1;
                return this[realCol, realRow];
            }
            else
            {
                int realRow = rowindex;
                int realCol = colIndex;
                return this[realCol, realRow];
            }
        }

        #region FlexGrid


        void changeService_ComponentChanging(object sender, ComponentChangingEventArgs e)
        {
            Control c = e.Component as Control;
            ExtendedDataGridView grid = c as ExtendedDataGridView;
            if (c != null && grid != null && (grid.Compatibility == GridCompatibility.MSFlexGrid &&
                    (e.Member.DisplayName == "Columns")))
            {
                throw new InvalidOperationException(Resources.MSFlexGridColumnEditor);
            }
        }

        /// <summary>
        /// Returns or sets a value that determines whether clicking on a column or row header should cause the entire column or row to be selected.
        /// </summary>
        [Description("Returns or sets a value that determines whether clicking on a column or row header should cause the entire column or row to be selected."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true)]
        public bool AllowBigSelection
        {
            get { return ((IFlexGridBehaviour)currentBehaviour).AllowBigSelection; }
            set { ((IFlexGridBehaviour)currentBehaviour).AllowBigSelection = value; }
        }

        #endregion



        /// <summary>
        /// Overrides the method of the base parent
        /// </summary>
        /// <param name="e">Event Arguments</param>
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            ((IFlexGridBehaviour)currentBehaviour).OnCellMouseDown(e);
        }

		/// <summary>
		/// Called when a Column Header is clicked.
		/// </summary>
		/// <param name="e">The Event arguments.</param>
		protected override void OnColumnHeaderMouseClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnColumnHeaderMouseClick(e);
			foreach (DataGridViewColumn column in Columns)
			{
				column.Selected = (column.Index == e.ColumnIndex);
			}
		}



        /// <summary>
        /// Indicates if a value is numeric
        /// </summary>
        /// <param name="value">The object to be evaluated</param>
        /// <returns>True if it is numeric</returns>
        internal static bool IsNumeric(object value)
        {
            double d;
            if (value is int || value is Double)
                return true;
            else
            {
                string strValue = value as string;
                if (!string.IsNullOrEmpty(strValue) && Double.TryParse(strValue, out d))
                    return true;
            }

            return false;
        }


    }


	/// <summary>
	/// This structure stores settings for resizing grids.
	/// </summary>
    public struct GridResizeSettings
    {
		/// <summary>
		/// Indicates if rows should be allowed to be resized.
		/// </summary>
        public bool Rows;
		/// <summary>
		/// Indicates if columns should be allowed to be resized.
		/// </summary>
		public bool Columns;
		/// <summary>
		/// Does not allow either rows nor columns to be resized.
		/// </summary>
		/// <returns>The GridResizeSettings that do not allow the user to do any resizing.</returns>
        public static GridResizeSettings ResizeNone()
        {
            GridResizeSettings newVal = new GridResizeSettings();
            newVal.Rows = false;
            newVal.Columns = false;
            return newVal;
        }
		/// <summary>
		/// Allows both rows and columns to be resized.
		/// </summary>
		/// <returns>The GridResizeSettings that allow the user to resize both rows and columns.</returns>
        public static GridResizeSettings ResizeBoth()
        {
            GridResizeSettings newVal = new GridResizeSettings();
            newVal.Rows = true;
            newVal.Columns = false;
            return newVal;
        }
		/// <summary>
		/// Allows columns to be resized, but not rows.
		/// </summary>
		/// <returns>The GridResizeSettings that allow the user to resize columns, but not rows</returns>
        public static GridResizeSettings ResizeColumns()
        {
            GridResizeSettings newVal = new GridResizeSettings();
            newVal.Rows = false;
            newVal.Columns = true;
            return newVal;
        }
		/// <summary>
		/// Allows rows to be resized, but not columns.
		/// </summary>
		/// <returns>The GridResizeSettings that allow the user to resize rows, but not columns</returns>
		public static GridResizeSettings ResizeRows()
        {
            GridResizeSettings newVal = new GridResizeSettings();
            newVal.Rows = true;
            newVal.Columns = false;
            return newVal;
        }

		/// <summary>
		/// Apply the grid resize settings to the specified grid.
		/// </summary>
		/// <param name="grid">The grid to which the settings should be applied.</param>
        public void Apply(ExtendedDataGridView grid)
        {
            grid.AllowUserToResizeColumns = Columns;
            grid.AllowUserToResizeRows = Rows;
        }

		/// <summary>
		/// Obtains the GridResizeSettings of a specified grid.
		/// </summary>
		/// <param name="grid">The grid from which to obtain the GridResizeSettings.</param>
		/// <returns>The GridResizeSettings of the specified grid.</returns>
        public static GridResizeSettings GetCurrent(ExtendedDataGridView grid)
        {
            GridResizeSettings newVal = new GridResizeSettings();
            newVal.Rows = grid.AllowUserToResizeRows;
            newVal.Columns = grid.AllowUserToResizeColumns;
            return newVal;
        }

        /// <summary>
        /// Operator that compares if two values of type GridResizeSettings are equal
        /// </summary>
        /// <param name="g1">First value of type GridResizeSettings</param>
        /// <param name="g2">Second value of type GridResizeSettings</param>
        /// <returns>True if the values are equal</returns>
        public static bool operator ==(GridResizeSettings g1, GridResizeSettings g2)
        {
            return (g1.Columns == g2.Columns) && (g1.Rows == g2.Rows);
        }

        /// <summary>
        /// Operator that compares if two values of type GridResizeSettings are different
        /// </summary>
        /// <param name="g1">First value of type GridResizeSettings</param>
        /// <param name="g2">Second value of type GridResizeSettings</param>
        /// <returns>True if the values are different</returns>
        public static bool operator !=(GridResizeSettings g1, GridResizeSettings g2)
        {
            return !(g1 == g2);
        }

        /// <summary>
        /// Returns the hash code of this instance
        /// </summary>
        /// <returns>Hash code of this instance</returns>
        public override int GetHashCode()
        {
            return Rows.GetHashCode() ^ Columns.GetHashCode();
        }

        /// <summary>
        /// Indicates if this instance and specified object are equal 
        /// </summary>
        /// <param name="obj">Another object to compare to</param>
        /// <returns>True if the instances are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            GridResizeSettings g = (GridResizeSettings)obj;
            return Rows.Equals(g.Rows) && Columns.Equals(g.Columns);
        }
    }   
}