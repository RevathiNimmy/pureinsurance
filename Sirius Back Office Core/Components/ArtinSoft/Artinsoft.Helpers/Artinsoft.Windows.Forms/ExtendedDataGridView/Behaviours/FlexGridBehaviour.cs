// Author: mrojas
// Project: Artinsoft.Windows.Forms
// Path: Artinsoft.Windows.Forms\ExtendedDataGridView\Behaviours
// Creation date: 8/6/2009 2:29 PM
// Last modified: 9/25/2009 10:19 AM

using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using Artinsoft.Windows.Forms.Properties;
using System.Collections;
using System.Data;

namespace Artinsoft.Windows.Forms
{
	/// <summary>
	/// Interface for FlexGrid behaviours.
	/// </summary>
    public interface IFlexGridBehaviour
    {
		/// <summary>
		/// Gets/sets the currently selected row.
		/// </summary>
        int RowSel { get; set; }
		/// <summary>
		/// Gets/sets the currently selected column.
		/// </summary>
        int ColSel { get; set; }

		/// <summary>
		/// Returns/sets flag to activate BigSelection
		/// </summary>
        bool AllowBigSelection { get; set; }

        /// <summary>
        /// Returns/sets the total number of fixed (non-scrollable) rows 
        /// </summary>
        int FixedRows { get; set; }

        /// <summary>
        /// Returns/sets the total number of fixed (non-scrollable) columns 
        /// </summary>
        int FixedColumns { get; set; }

		/// <summary>
		/// Gets/sets the background color for fixed cells.
		/// </summary>
        Color BackColorFixed { get; set; }

		/// <summary>
		/// Gets/sets the focus rectangle settings for the grid.
		/// </summary>
        Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings FocusRect { get; set; }

		/// <summary>
		/// Gets/sets the highlight settings for the grid.
		/// </summary>
        Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings HighLight { get; set; }

		/// <summary>
		/// Called when a mouse button is pressed down on a cell.
		/// </summary>
		/// <param name="e"></param>
        void OnCellMouseDown(DataGridViewCellMouseEventArgs e);

    }

	/// <summary>
	/// This class represents the behaviour of a FlexGrid to be used with an ExtendedDataGridView.
	/// </summary>
    public class FlexGridBehaviour : IGridBehaviour, IFlexGridBehaviour
    {
        internal const int LOWEST_GRIDLINEWIDTH_VALUE = 0;
        private const int DEFAULT_ROWHEIGHTMIN = 0;
        private const int DEFAULT_NEW_CUSTOM_COLUMN_WIDTH = 66;
        private const int DEFAULT_FIXED_ROWS = 1;
        private const int DEFAULT_FIXED_COLUMNS = 1;
        private const int DEFAULT_ROWSCOUNT = 2;
        private const int DEFAULT_COLUMNSCOUNT = 2;
        private const int DEFAULT_GRIDLINEWIDTH = 1;
        private const DataGridViewSelectionMode DEFAULT_SELECTIONMODE = DataGridViewSelectionMode.CellSelect;
        private const int DEFAULT_CELL_HEIGHT = 16;

        #region FlexGrid properties and methods to support the FlexGridBehaviour

		/// <summary>
		/// Creates an instance of the FlexGridBehaviour for a specified grid, and copies
		/// behaviour properties from a specified behaviour.
		/// </summary>
		/// <param name="grid">The grid for which this behaviour will be used.</param>
		/// <param name="previousBehaviour">The behaviour from which to copy properties.</param>
        public FlexGridBehaviour(ExtendedDataGridView grid, IGridBehaviour previousBehaviour)
        {
            this.grid = grid;
            SetValuesFromInitializeComponents(previousBehaviour);
        }

        private void SetValuesFromInitializeComponents(IGridBehaviour previousBehaviour)
        {
            Dictionary<String, object> oldValues = null;
            if (previousBehaviour is ValueHolderBehaviour)
            {
                IFlexGridBehaviour previousBehaviourFlex = previousBehaviour as IFlexGridBehaviour;
                oldValues = new Dictionary<string, object>();
                oldValues.Add("DataSource", previousBehaviour.DataSource);
                oldValues.Add("GridLineWidth", previousBehaviour.GridLineWidth);
                oldValues.Add("SelectionMode", previousBehaviour.SelectionMode);
                oldValues.Add("RowsCount", previousBehaviour.RowsCount);
                oldValues.Add("ColumnsCount", previousBehaviour.ColumnsCount);
                oldValues.Add("RowHeightMin", previousBehaviour.RowHeightMin);
                oldValues.Add("AllowBigSelection", previousBehaviourFlex.AllowBigSelection);
                oldValues.Add("FixedRows", previousBehaviourFlex.FixedRows);
                oldValues.Add("FixedColumns", previousBehaviourFlex.FixedColumns);
                oldValues.Add("BackColorFixed", previousBehaviourFlex.BackColorFixed);
                oldValues.Add("FocusRect", previousBehaviourFlex.FocusRect);
                oldValues.Add("HighLight", previousBehaviourFlex.HighLight);
            } // if
            Reset(previousBehaviour); //To set to defaults
            if (previousBehaviour is ValueHolderBehaviour)
            {
                if (Convert.ToInt32(oldValues["GridLineWidth"]) > LOWEST_GRIDLINEWIDTH_VALUE)
                {
                    GridLineWidth = Convert.ToInt32(oldValues["GridLineWidth"]);
                } // if
                if (((DataGridViewSelectionMode)oldValues["SelectionMode"]) != DataGridViewSelectionMode.CellSelect)
                {
                    SelectionMode = ((DataGridViewSelectionMode)oldValues["SelectionMode"]);
                } // if
                int _RowsCount = Convert.ToInt32(oldValues["RowsCount"]);
                _RowsCount = (_RowsCount == ValueHolderBehaviour.UNSETVALUE) ? DEFAULT_ROWSCOUNT : _RowsCount;
                int _FixedRows = Convert.ToInt32(oldValues["FixedRows"]);
                _FixedRows = (_FixedRows == ValueHolderBehaviour.UNSETVALUE) ? DEFAULT_FIXED_ROWS : _FixedRows;
                int _ColumnsCount = Convert.ToInt32(oldValues["ColumnsCount"]);
                _ColumnsCount = (_ColumnsCount == ValueHolderBehaviour.UNSETVALUE) ? DEFAULT_COLUMNSCOUNT : _ColumnsCount;
                int _FixedColumns = Convert.ToInt32(oldValues["FixedColumns"]);
                _FixedColumns = (_FixedColumns == ValueHolderBehaviour.UNSETVALUE) ? DEFAULT_FIXED_COLUMNS : _FixedColumns;
                if (_FixedRows == -1 || _FixedColumns == -1 || _RowsCount < 0 ||
                    _FixedRows > _RowsCount ||
                    _FixedColumns > _ColumnsCount)
                {
                    //If there is any invalid value, then reset to defaults
                    FixedRows = DEFAULT_FIXED_ROWS;
                    FixedColumns = DEFAULT_FIXED_COLUMNS;
                    RowsCount = DEFAULT_ROWSCOUNT;
                    ColumnsCount = DEFAULT_COLUMNSCOUNT;
                } // if
                else
                {
                    if (_FixedRows >= RowsCount)
                    {
                        RowsCount = _RowsCount;
                        FixedRows = _FixedRows;
                    }
                    else
                    {
                        FixedRows = _FixedRows;
                        RowsCount = _RowsCount;
                    } // else
                    if (_FixedColumns >= ColumnsCount)
                    {
                        ColumnsCount = _ColumnsCount;
                        FixedColumns = _FixedColumns;
                    }
                    else
                    {
                        FixedColumns = _FixedColumns;
                        ColumnsCount = _ColumnsCount;
                    } // else
                } // else
                if (Convert.ToInt32(oldValues["RowHeightMin"]) > 0)
                {
                    RowHeightMin = Convert.ToInt32(oldValues["RowHeightMin"]);
                } // if
                AllowBigSelection = Convert.ToBoolean(oldValues["AllowBigSelection"]);
                if (((Color)oldValues["BackColorFixed"]) != Color.Empty)
                {
                    BackColorFixed = (Color)oldValues["BackColorFixed"];
                } // if
                if (((ExtendedDataGridView.FocusRectSettings)oldValues["FocusRect"]) != ExtendedDataGridView.FocusRectSettings.FocusNone)
                {
                    FocusRect = ((ExtendedDataGridView.FocusRectSettings)oldValues["FocusRect"]);
                } // if

                if (((ExtendedDataGridView.HighLightSettings)oldValues["HighLight"]) != ExtendedDataGridView.HighLightSettings.HighlightNever)
                {
                    HighLight = ((ExtendedDataGridView.HighLightSettings)oldValues["HighLight"]);
                } // if
                DataSource = oldValues["DataSource"];
            } // if

        }

        #region ISupportInitialize Members

        private int _FixedColumns = DEFAULT_FIXED_COLUMNS;
        private int _FixedRows = DEFAULT_FIXED_ROWS;

        private bool isInitializing;
		/// <summary>
		/// Indicates if the behaviour is initializing or not.
		/// </summary>
        public bool IsInitializing
        {

            get { return isInitializing; }
            set { isInitializing = true; }
        }


		/// <summary>
		/// Begin initializing the behaviour.
		/// </summary>
        public void BeginInit()
        {

        }

		/// <summary>
		/// Finish initializing the behaviour.
		/// </summary>
        public void EndInit()
        {
    
            //It is done before turning off isInitializing to avoid events.
            GeneralCurrentCell = grid.BaseCurrentCell;

            isInitializing = false;

            SelectionStarted = true;
            //TODO SelectionMode = _selectionMode;
        }


        #endregion


        internal ExtendedDataGridView grid;
        internal DataGridViewCell _generalCurrentCell;
        /// <summary>
        /// MSFlexGrid has a different selection behaviour than DataGridView.
        /// In a MSFlexGrid the cell that currently has the focus does not have
        /// the same background color or "FocusRect" than the other selected cells.
        /// To provide an MSFlexGrid compatible behaviour we must provide a way 
        /// to track that cell
        /// </summary>
        internal DataGridViewCell GeneralCurrentCell
        {
            get
            {
                if (_generalCurrentCell == null || !grid.SelectedCells.Contains(_generalCurrentCell))
                    _generalCurrentCell = grid.CurrentCell;
                return _generalCurrentCell;

            }
            set
            {
                if (_generalCurrentCell == value)
                    return;
                _generalCurrentCell = value;
            }
        }


        /// <summary>
        /// TODO: REVIEW Holds the value of the currently selected cell column
        /// </summary>
        private int _colSel;

        /// TODO: REVIEW Holds the value of the currently selected cell row
        private int _rowSel;

        /// <summary>
        /// Flag for indicate when the component starts to select
        /// </summary>
        private bool SelectionStarted;

        #endregion

		/// <summary>
		/// Resets the behaviour to default property values.
		/// </summary>
		/// <param name="previous">Not used.</param>
        public void Reset(IGridBehaviour previous)
        {
            grid.AllowUserToAddRows = false; //Default Behaivor
            grid.AllowUserToDeleteRows = false;
            grid.RowHeadersWidth = DEFAULT_NEW_CUSTOM_COLUMN_WIDTH;
            grid.RowTemplate.Height = DEFAULT_CELL_HEIGHT;
            grid.ColumnHeadersHeight = DEFAULT_CELL_HEIGHT;
            grid.AllowUserToResizeColumns = false;
            grid.AllowUserToResizeRows = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = grid.ColumnHeadersDefaultCellStyle.ForeColor;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
            grid.RowHeadersDefaultCellStyle.SelectionForeColor = grid.RowHeadersDefaultCellStyle.ForeColor;
            grid.RowHeadersDefaultCellStyle.SelectionBackColor = grid.RowHeadersDefaultCellStyle.BackColor;
            grid.BorderStyle = BorderStyle.Fixed3D;
            grid.ReadOnly = true;
            RowsCount = 2;
            ColumnsCount = 2;
            if (currentCellChanged == null)
            {
                currentCellChanged = new EventHandler(grid_CurrentCellChanged);
                grid.CurrentCellChanged -= currentCellChanged;
                grid.CurrentCellChanged += currentCellChanged;
            } // if
            if (rowpostPaint == null)
            {

                rowpostPaint = new DataGridViewRowPostPaintEventHandler(grid_RowPostPaint);
                grid.RowPostPaint -= rowpostPaint;
                grid.RowPostPaint += rowpostPaint;
            } // if
            grid.EnableHeadersVisualStyles = false;
            AllowBigSelection = true;

        }
        DataGridViewRowPostPaintEventHandler rowpostPaint;
        void grid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (!isInitializing && grid.FocusRect!= ExtendedDataGridView.FocusRectSettings.FocusNone)
            {
                DataGridViewCell generalCurrentCell = GeneralCurrentCell;
                if (generalCurrentCell != null && generalCurrentCell.RowIndex == e.RowIndex)
                {

                    generalCurrentCell.Style.SelectionForeColor = grid.DefaultCellStyle.ForeColor;
                    Rectangle rect = grid.GetCellDisplayRectangle(generalCurrentCell.ColumnIndex, generalCurrentCell.RowIndex, false);
                    DataGridViewPaintParts parts = DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.ContentForeground | DataGridViewPaintParts.Focus;
                    e.PaintCells(rect, parts);
                    e.DrawFocus(rect, false);
                }
            } 
        }


        EventHandler currentCellChanged;

        void grid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (!grid.SelectedCells.Contains(_generalCurrentCell))
            _generalCurrentCell = grid.CurrentCell;
        }

		/// <summary>
		/// Obtains the row over which the mouse is currently positioned.
		/// </summary>
        public int MouseRow
        {
            get
            {
                return grid.mouse_cell_row;
            }
        }

		/// <summary>
		/// Obtains the column over which the mouse is currently positioned.
		/// </summary>
        public int MouseCol
        {
            get { return grid.mouse_cell_column; }
        }

        /// <summary>
        /// Returns the currently selected column. This is not a design time property
        /// </summary>
        public int CurrentColumnIndex
        {
            get
            {
                if (GeneralCurrentCell != null)
                    return grid.ColumnHeadersVisible ? GeneralCurrentCell.ColumnIndex + 1 : GeneralCurrentCell.ColumnIndex;
                else if (grid.BaseCurrentCell != null)
                {
                    return grid.ColumnHeadersVisible ? grid.BaseCurrentCell.ColumnIndex + 1 : grid.BaseCurrentCell.ColumnIndex;
                }
                else
                    return 0;
            }
            set
            {
                if ((value < 0) || (value > grid.Columns.Count))
                    throw new IndexOutOfRangeException("Index must be between 0 and the Count of Columns");
                DataGridViewCell cell = GetCell(CurrentRowIndex, value);
                _colSel = value;
                if (cell is DataGridViewHeaderCell || !cell.Visible)
                {
                    GeneralCurrentCell = cell;
                }
                else
                {
                    GeneralCurrentCell = cell;
                }
            }
        }

        /// <summary>
        /// Returns\sets the current row 
        /// </summary>
        public int CurrentRowIndex
        {
            get
            {
                if (GeneralCurrentCell != null)
                    return grid.RowHeadersVisible ? GeneralCurrentCell.RowIndex + 1 : GeneralCurrentCell.RowIndex;
                else if (grid.BaseCurrentCell != null)
                {
                    return grid.RowHeadersVisible ? grid.BaseCurrentCell.RowIndex + 1 : grid.BaseCurrentCell.RowIndex;
                }
                else
                    return 0;
            }
            set
            {
                if ((value < 0) || (value > grid.Rows.Count))
                    throw new IndexOutOfRangeException(Resources.InvalidCurrentRowIndex);

                DataGridViewCell cell = GetCell(value, CurrentColumnIndex);
                _rowSel = value;
                if (cell is DataGridViewHeaderCell || !cell.Visible)
                {
                    GeneralCurrentCell = cell;
                }
                else
                {
                    GeneralCurrentCell = cell;
                }
            }
        }

        #region UtilityMethods
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
                if (grid.RowHeadersVisible && grid.ColumnHeadersVisible)
                    return grid.TopLeftHeaderCell;
                else if (grid.RowHeadersVisible)
                    return grid.Rows[0].HeaderCell;
                else if (grid.ColumnHeadersVisible)
                    return grid.Columns[0].HeaderCell;
            }
            else if (colIndex == 0)
            {
                if (grid.RowHeadersVisible && grid.ColumnHeadersVisible)
                    return grid.Rows[rowindex - 1].HeaderCell;
                else if (grid.RowHeadersVisible)
                {
                    return grid.Rows[rowindex].HeaderCell;
                }
                else if (grid.ColumnHeadersVisible)
                    return grid.BaseGetCell(0, rowindex - 1);
            }
            else if (rowindex == 0)
            {
                if (grid.RowHeadersVisible && grid.ColumnHeadersVisible)
                    return grid.Columns[colIndex - 1].HeaderCell;
                else if (grid.RowHeadersVisible)
                    return grid.BaseGetCell(colIndex - 1, 0);
                else if (grid.ColumnHeadersVisible)
                    return grid.Columns[colIndex].HeaderCell;
            }

            if (grid.ColumnHeadersVisible && grid.RowHeadersVisible)
            {
                int realCol = colIndex - 1;
                int realRow = rowindex - 1;
                return grid.BaseGetCell(realCol, realRow);
            }
            else if (grid.RowHeadersVisible)
            {
                int realCol = colIndex - 1;
                int realRow = rowindex;
                return grid.BaseGetCell(realCol, realRow);
            }
            else if (grid.ColumnHeadersVisible)
            {
                int realCol = colIndex;
                int realRow = rowindex - 1;
                return grid.BaseGetCell(realCol, realRow);
            }
            else
            {
                int realRow = rowindex;
                int realCol = colIndex;
                return grid.BaseGetCell(realCol, realRow);
            }
        }

        private void SetSelectedCells(int prow, int pcol)
        {
            int c1 = CurrentColumnIndex, c2 = pcol;
            int r1 = CurrentRowIndex, r2 = prow;
            int temp;
            if (c1 > c2)
            {
                temp = c2;
                c2 = c1;
                c1 = temp;
            }
            if (r1 > r2)
            {
                temp = r2;
                r2 = r1;
                r1 = temp;
            }
            DataGridViewCell Firstcell = null;
            for (int i = 0; i < ColumnsCount; i++)
            {
                for (int j = 0; j < RowsCount; j++)
                {
                    DataGridViewCell cell = GetCell(j, i);
                    DataGridViewHeaderCell headerCell = cell as DataGridViewHeaderCell;
                    if (c1 <= i && i <= c2)
                    {
                        if (r1 <= j && j <= r2)
                        {
                            if (i == c1 && j == r1)
                                Firstcell = cell;
                            if (headerCell == null)
                            {
                                if (!cell.Selected)
                                {
                                    cell.Selected = true;
                                }
                            }
                            continue;
                        }
                    }
                    //Deselect any other Cell
                    if (headerCell == null)
                    {
                        if (cell.Selected && cell.Visible)
                        {
                            cell.Selected = false;
                        }
                    }
                }
            }
            _generalCurrentCell = Firstcell;
            grid.Refresh();//TODO CHECK it was base.Refresh
        }







        /// <summary>
        /// Returns/sets whether a grid should allow regular cell selection, selection by rows, or selection by columns.
        /// </summary>
        public DataGridViewSelectionMode SelectionMode
        {
            get
            {
                    return grid.BaseSelectionMode;
            }
            set
            {
					grid.BaseSelectionMode = value;
					if (SelectionStarted)
                    {
                        if (value == DataGridViewSelectionMode.FullColumnSelect)
                        {
                            foreach (DataGridViewColumn column in grid.Columns)
                            {
                                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                            }
                        }
                        //When the SelectionMode changes, The Cell is not selected. 
                        //In flex grid this doesn't happen, so to correct this
                        //Cell Is selected manually.
                        if (CurrentCell != null)
                            CurrentCell.Selected = true;
                    }
              }
                //TODO _selectionMode = value;
        }

		/// <summary>
		/// Obtains a cell from the grid.
		/// </summary>
		/// <param name="columnindex">Index of the desired column.</param>
		/// <param name="rowindex">Index of the desired row.</param>
		/// <returns></returns>
        public DataGridViewCell this[int columnindex, int rowindex]
        {
            get
            {
                return GetCell(rowindex, columnindex);
            }
            set
            {
                grid.BaseSetCell(columnindex, rowindex, value);
            }


        }


        /// <summary>
        /// Clears the Selection of the Grid and avoid FocusRect to Show
        /// </summary>
        public void ClearSelection()
        {
            grid.BaseClearSelection();
        }

        /// <summary>
        /// Returns/Sets the total number of rows
        /// </summary>
        public int RowsCount
        {
            get
            {
                 return grid.Rows.Count + (grid.ColumnHeadersVisible ? 1 : 0);
            }
            set
            {

                #region Validating
                if (value < 0)
                    throw new ArgumentException(Resources.ValueZeroOrGreater);
                int _fixedRows = FixedRows;
                if (_fixedRows > value)
                {
                    if (_fixedRows != 0 && grid.DesignMode)
                        throw new ArgumentException(Resources.FixedRowsLessThanRowsCount);
                }
                #endregion
                    //Remove all Items, Value is 0
                    if (value == 0)
                    {
                        grid.Rows.Clear();
                        grid.ColumnHeadersVisible = false;
                        return;
                    }

                    int realvalue = grid.ColumnHeadersVisible ? value - 1 : value;
                    if (grid.Rows.Count < realvalue)
                    {
                        //Add A Column (because an Exception is thrown when no columns are available)
                        if (grid.Columns.Count == 0)
                        {
                            Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn customColumn = new Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn();
                            customColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                            customColumn.HeaderText = "";
                            customColumn.Name = "Column" + grid.Columns.Count;
                            customColumn.DividerWidth = _gridLineWidth;
                            customColumn.Width = DEFAULT_NEW_CUSTOM_COLUMN_WIDTH;
                            grid.Columns.Add(customColumn);
                        }

                        grid.Rows.Add(realvalue - grid.Rows.Count);
                    }
                    else if (grid.Rows.Count > realvalue)
                    {
                        for (int i = grid.Rows.Count - 1; i >= realvalue; i--)
                        {
                            grid.Rows.RemoveAt(i);
                        }
                    }
                    //TODO Review UpdateResizeMode(); Why??
                    grid.Refresh();
 
            }
        }

		/// <summary>
		/// Gets/sets the minimum row height allowed for the grid.
		/// </summary>
        public int RowHeightMin
        {
            get
            {

                    return grid.Rows.Count > 0 ? grid.Rows[0].MinimumHeight : 0;
            }
            set
            {
                    if (grid.DesignMode && value < 0)
                    {
                        throw new ArgumentException(Resources.ValueZeroOrGreater);
                    }
                    if (value > 0)
                    {
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            row.MinimumHeight = value;
                        }
                    }
            }
        }

		/// <summary>
		/// Gets/sets the index of the first displayed row, not including fixed rows.
		/// </summary>
        public int FirstDisplayedScrollingRowIndex
        {
            get { return grid.ColumnHeadersVisible ? grid.BaseFirstDisplayedScrollingRowIndex + 1 : grid.BaseFirstDisplayedScrollingRowIndex; }
            set
            {
                if (grid.DesignMode && value < 0)
                {
                    throw new ArgumentException(Resources.ValueZeroOrGreater);
                }
                if (value > 0)
                    grid.BaseFirstDisplayedScrollingRowIndex = grid.ColumnHeadersVisible ? value - 1 : value;
            }
        }

		/// <summary>
		/// Gets/sets the index of the first displayed column, not including fixed columns.
		/// </summary>
		public int FirstDisplayedScrollingColumnIndex
        {
            get { return grid.RowHeadersVisible ? grid.BaseFirstDisplayedScrollingColumnIndex + 1 : grid.BaseFirstDisplayedScrollingColumnIndex; }
            set
            {
                if (grid.DesignMode && value < 0)
                {
                    throw new ArgumentException(Resources.ValueZeroOrGreater);
                }
                if (value > 0)
                    grid.BaseFirstDisplayedScrollingColumnIndex = grid.RowHeadersVisible ? value - 1 : value;
            }
        }

		/// <summary>
		/// Gets the default amount of columns the grid will have.
		/// </summary>
        public static int DefaultColumnsCountValue
        {
            get
            {
                return 2;
            }
        }
        /// <summary>
        /// Determines the total number of columns 
        /// </summary>
        public int ColumnsCount
        {
            get
            {
                    return grid.Columns.Count + (grid.RowHeadersVisible ? 1 : 0);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(Resources.ValueZeroOrGreater);
                }

                //Validating
                int _fixedColumns = FixedColumns;
                if (_fixedColumns >= value)
                {
                    if (_fixedColumns != 0)
                        if (grid.DesignMode)
                            throw new ArgumentException(Resources.FixedColumnLessThanColumnsCount);
                }

                //Remove all Items, Value is 0
                if (value == 0)
                {
                    if (grid.Rows.Count == 0)
                    {
                        grid.Columns.Clear();
                        grid.RowHeadersVisible = false;
                        return;
                    }
                    else
                    {
                        throw new InvalidOperationException(Resources.NoRemoveColumnsWithRows);
                    }
                }

                int realvalue = grid.RowHeadersVisible ? value - 1 : value;
                if (grid.Columns.Count < realvalue)
                {
                    int oldStart = grid.Columns.Count;
                    for (int i = grid.Columns.Count; i < realvalue; i++)
                    {
                        Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn newCustomColumn = new Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn();
                        newCustomColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        newCustomColumn.HeaderText = "";
                        newCustomColumn.Name = "Column" + grid.Columns.Count;
                        newCustomColumn.DividerWidth = _gridLineWidth;
                        newCustomColumn.Width = DEFAULT_NEW_CUSTOM_COLUMN_WIDTH;
                        grid.Columns.Add(newCustomColumn);
                    }
                        
                    DataGridViewCellStyle cellstyleFixed = GetCellStyleFixed();
                    DataGridViewCellStyle cellstyleNormal = GetCellStyleNonFixed();
                    //Now we have to check the fixed rows
                    int fixedRows = FixedRows;
                    foreach (DataGridViewRow row in grid.Rows)
                    {
                        if (row.Index < fixedRows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                                cell.Style = cellstyleFixed;
                            
                        }
                        else break;
                    } // for
                }
                else if (grid.Columns.Count > realvalue)
                {
                    for (int i = grid.Columns.Count - 1; i >= realvalue; i--)
                    {
                        grid.Columns.RemoveAt(i);
                    }
                }
                //TODO UpdateResizeMode(); WHY???
            }
        }





        private int _gridLineWidth = DEFAULT_GRIDLINEWIDTH;

        /// <summary>
        /// Returns/sets the width in Pixels of the gridlines for the control.
        /// </summary>
        public int GridLineWidth
        {
            get
            {
  
                    return _gridLineWidth + 1;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentException(Resources.ValueGreaterThanZero);
                    if (grid.BorderStyle == BorderStyle.FixedSingle)
                    {
                        _gridLineWidth = value - 1;
                        grid.RowTemplate.DividerHeight = _gridLineWidth;
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            row.DividerHeight = _gridLineWidth;
                        }
                        foreach (DataGridViewColumn col in grid.Columns)
                        {
                            col.DividerWidth = _gridLineWidth;
                        }
                    }
                    else
                    {
                        //TODO: value must be ignored?
                    }
            }
        }

		/// <summary>
		/// Gets the default amount of fixed columns the grid will have.
		/// </summary>
        public static int DefaultFixedColumnsValue
        {
            get
            {
                return 1;
            }
        }






        #region GridBehaviour Members

		/// <summary>
		/// Gets/sets the current cell.
		/// </summary>
        public DataGridViewCell CurrentCell
        {
            get
            {
                return GeneralCurrentCell;
            }
            set
            {
                GeneralCurrentCell = value;
            }
        }


        #endregion

        #region IFlexGridBehaviour Members

        /// <summary>
        /// Returns or sets the start or end column for a range of cells
        /// You can use these properties (ColSel/RowSel) to select a specific region of the grid programmatically, 
        /// or to read the dimensions of an area that the user selects into code.
        /// The grid cursor is in the cell at Row, Col. 
        /// The grid selection is the region between rows Row and RowSel and columns Col and ColSel. 
        /// Note that RowSel may be above or below Row, and ColSel may be to the left or to the right of Col.
        /// </summary>
        public int ColSel
        {
            get
            {
                int maxColSel = CurrentColumnIndex;
                foreach (DataGridViewCell cell in grid.SelectedCells)
                {
                    if (cell.Selected)
                    {
                        int realCol = grid.RowHeadersVisible ? cell.ColumnIndex + 1 : cell.ColumnIndex;
                        if (maxColSel < realCol)
                            maxColSel = realCol;
                    }
                }
                return maxColSel;
            }
            set
            {
                _colSel = value;
                SetSelectedCells(RowSel, _colSel);
            }
        }

        /// <summary>
        /// Returns or sets the start or end column for a range of cells
        /// You can use these properties (ColSel/RowSel) to select a specific region of the grid programmatically, 
        /// or to read the dimensions of an area that the user selects into code.
        /// The grid cursor is in the cell at Row, Col. 
        /// The grid selection is the region between rows Row and RowSel and columns Col and ColSel. 
        /// Note that RowSel may be above or below Row, and ColSel may be to the left or to the right of Col.
        /// </summary>
        public int RowSel
        {
            get
            {
                int maxRowSel = CurrentRowIndex;
                foreach (DataGridViewCell cell in grid.SelectedCells)
                {
                    if (cell.Selected)
                    {
                        int realRow = grid.ColumnHeadersVisible ? cell.RowIndex + 1 : cell.RowIndex;
                        if (maxRowSel < realRow)
                            maxRowSel = realRow;
                    }
                }
                return maxRowSel;
            }
            set
            {
                _rowSel = value;
                SetSelectedCells(_rowSel, ColSel);
            }
        }


        /// <summary>
        /// Returns/sets the total number of fixed (non-scrollable) columns or rows for a FlexGrid.
        /// </summary>
        public int FixedRows
        {
            get
            {

                int result = grid.ColumnHeadersVisible ? 1 : 0;
                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.Frozen)
                    {
                        result++;
                    }
                    else
                    {
                        break;
                    }
                }
                return result;
            }
            set
            {
                    int _rows = RowsCount;
                    //Validating
                    if (value < 0)
                    {
                        throw new ArgumentException(Resources.ValueZeroOrGreater);
                    }

                    if (value >= _rows)
                    {
                        throw new ArgumentException(Resources.FixedRowsLessThanRowsCount);
                    }

                    //The value is the same
                    if (FixedRows == value)
                    {
                        return;
                    }

                    if (value == 0)
                    {
                        grid.ColumnHeadersVisible = false;

                        grid.Rows.Insert(0, 1);

                        for (int i = 0; i < ColumnsCount; i++)
                        {
                            DataGridViewCell cell = GetCell(0, i);
                            int realCol = grid.RowHeadersVisible ? i - 1 : i;
                            if (realCol < 0)
                            {
                                cell.Value = grid.TopLeftHeaderCell.Value;
                            }
                            else
                            {
                                cell.Value = grid.Columns[realCol].HeaderCell.Value;
                            }
                        }

                        for (int i = 0; i < grid.Rows.Count; i++)
                        {
                            grid.Rows[i].Frozen = false;
                        }
                    }
                    else
                    {
                        int realvalue = value - 1;
                        if (!grid.ColumnHeadersVisible)
                        {
                            if (grid.Rows.Count > 0)
                            {
                                for (int i = 0; i < ColumnsCount; i++)
                                {
                                    DataGridViewCell cell = GetCell(0, i);
                                    int realCol = grid.RowHeadersVisible ? i - 1 : i;
                                    if (realCol < 0)
                                    {
                                        grid.TopLeftHeaderCell.Value = cell.Value;
                                    }
                                    else
                                    {
                                        grid.Columns[realCol].HeaderCell.Value = cell.Value;
                                    }
                                }
                                grid.Rows.RemoveAt(0);
                            }
                        }
                        grid.ColumnHeadersVisible = true;
                        DataGridViewCellStyle cellStyleFixed = GetCellStyleFixed();
                        DataGridViewCellStyle cellStyleNormal = GetCellStyleNonFixed();
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            Boolean Frozen = row.Index < realvalue;
                            row.Frozen = Frozen;
                                foreach(DataGridViewCell cell in row.Cells)
                                {
                                    cell.Style = Frozen ? cellStyleFixed : cellStyleNormal;
                                } // foreach
                        }
                    }
                    grid.Refresh();
            }
        }

        private DataGridViewCellStyle GetCellStyleNonFixed()
        {
            DataGridViewCellStyle cellStyleNormal = new DataGridViewCellStyle();
            cellStyleNormal.BackColor = grid.DefaultCellStyle.BackColor;
            cellStyleNormal.ForeColor = grid.DefaultCellStyle.ForeColor;
            cellStyleNormal.SelectionBackColor = grid.DefaultCellStyle.SelectionBackColor;
            cellStyleNormal.SelectionForeColor = grid.DefaultCellStyle.SelectionForeColor;
            return cellStyleNormal;
        }

        private DataGridViewCellStyle GetCellStyleFixed()
        {
            DataGridViewCellStyle cellStyleFixed = new DataGridViewCellStyle();
            cellStyleFixed.BackColor = grid.BackColorFixed;
            cellStyleFixed.ForeColor = grid.ForeColorFixed;
            cellStyleFixed.SelectionBackColor = grid.BackColorFixed;
            cellStyleFixed.SelectionForeColor = grid.ForeColor;
            return cellStyleFixed;
        }

        /// <summary>
        /// Returns/sets the total number of fixed (non-scrollable) columns 
        /// </summary>
        public int FixedColumns
        {
            get
            {

                int result = grid.RowHeadersVisible ? 1 : 0;
                foreach (DataGridViewColumn dataGridViewColumn in grid.Columns)
                {
                    if (dataGridViewColumn.Frozen)
                        result++;
                    else
                        break;
                } // foreach
                return result;
            } // get
            set
            {
                    int _cols = ColumnsCount;
                    //Validating
                    #region Validating
                    if (value < 0)
                        throw new ArgumentException("Value is not valid");
                    if (value >= _cols && grid.DesignMode)
                        throw new ArgumentException(Resources.FixedColumnLessThanColumnsCount);
                    if (_FixedColumns == value) //The value is the same just exit
                        return;
                    #endregion
                    _FixedColumns = value;
                    if (SelectionStarted)
                    {
                        grid.BaseSelectionMode = DataGridViewSelectionMode.CellSelect;
                        SelectionStarted = false;
                    }
                    if (value == 0)
                    {
                        grid.RowHeadersVisible = false;
                        Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn newCustomColumn = new Artinsoft.Windows.Forms.ExtendedDataGridView.CustomColumn();
                        newCustomColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                        newCustomColumn.HeaderText = "";
                        newCustomColumn.Name = "Column" + grid.Columns.Count;
                        newCustomColumn.DividerWidth = _gridLineWidth;
                        newCustomColumn.Width = DEFAULT_NEW_CUSTOM_COLUMN_WIDTH;
                        grid.Columns.Insert(0, newCustomColumn);
                        for (int i = 0; i < RowsCount; i++)
                        {
                            DataGridViewCell cell = GetCell(i, 0);
                            int realRow = grid.ColumnHeadersVisible ? i - 1 : i;
                            if (realRow < 0)
                            {
                                cell.Value = grid.TopLeftHeaderCell.Value;
                            }
                            else
                            {
                                cell.Value = grid.Rows[realRow].HeaderCell.Value;
                            }
                        }
                        for (int i = 0; i < grid.Columns.Count; i++)
                        {
                            grid.Columns[i].Frozen = false;
                        }
                    }
                    else
                    {
                        int realvalue = value - 1;
                        DataGridViewCellStyle cellStyleFixed = GetCellStyleFixed();
                        DataGridViewCellStyle cellStyleNormal = GetCellStyleNonFixed();
                        if (!grid.RowHeadersVisible)
                        {
                            if (grid.Columns.Count > 0)
                            {
                                for (int i = 0; i < RowsCount; i++)
                                {
                                    DataGridViewCell cell = GetCell(i, 0);
                                    int realRow = grid.ColumnHeadersVisible ? i - 1 : i;
                                    if (realRow < 0)
                                    {
                                        grid.TopLeftHeaderCell.Value = cell.Value;
                                    }
                                    else
                                    {
                                        grid.Rows[realRow].HeaderCell.Value = cell.Value;
                                    }
                                }
                                grid.Columns.RemoveAt(0);
                            }
                        }
                        grid.RowHeadersVisible = true;
                        for (int i = 0; i < grid.Columns.Count; i++)
                        {
                            bool Frozen = i < realvalue;
                            grid.Columns[i].Frozen = Frozen;
                            
                        }
                        foreach (DataGridViewRow row in grid.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (row.Frozen) break; //It has already been set.
                                bool Frozen = cell.ColumnIndex < realvalue;
                                cell.Style = Frozen ? cellStyleFixed : cellStyleNormal;
                            } // foreach
                        } // foreach
                    }
                    //In case that then number of Rows non fixed is 0
                    if (_FixedRows != RowsCount)
                    {
                        _generalCurrentCell = GetCell(FixedRows, FixedColumns);
                        CurrentCell = _generalCurrentCell;
                    }
                    grid.Refresh();
            }
        }

        bool _allowBigSelection = true;
        /// <summary>
        /// Returns/sets flag to activate BigSelection
        /// </summary>
        public bool AllowBigSelection
        {
            get
            {
                return _allowBigSelection;
            }
            set
            {
                _allowBigSelection = value;
            }
        }

		/// <summary>
		/// Gets/sets the background color for fixed cells.
		/// </summary>
        public Color BackColorFixed
        {
            get { return grid.ColumnHeadersDefaultCellStyle.BackColor; }
            set
            {
                grid.ColumnHeadersDefaultCellStyle.BackColor = value;
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = value;
                grid.RowHeadersDefaultCellStyle.BackColor = value;
                grid.RowHeadersDefaultCellStyle.SelectionBackColor = value;
                grid.TopLeftHeaderCell.Style.BackColor = value;
                grid.TopLeftHeaderCell.Style.SelectionBackColor = value;
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    column.HeaderCell.Style.BackColor = value;
                }
                DataGridViewCellStyle cellStyleFixed = GetCellStyleFixed();
                cellStyleFixed.BackColor = value;
                foreach (DataGridViewRow row in grid.Rows)
                {
                    row.HeaderCell.Style.BackColor = value;
                    if (row.Frozen)
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.HasStyle)
                        {
                            cell.Style.BackColor = value;
                        }
                        else
                        {
                            cell.Style = cellStyleFixed;
                        } 
                    } 
                }
                grid.TopLeftHeaderCell.Style.BackColor = value;
            }
        }

        private Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings _focusRect = Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings.FocusLight;
		/// <summary>
		/// Gets/sets the focus rectangle settings.
		/// </summary>
        public Artinsoft.Windows.Forms.ExtendedDataGridView.FocusRectSettings FocusRect
        {
            get { return _focusRect; }
            set
            {
                _focusRect = value;
            }
        }

        private bool selectAllFlag;


        /// <summary>
        /// Gets the Position of the given Row Col Cell in the SelectedCells array
        /// </summary>
        /// <param name="row">RowIndex</param>
        /// <param name="col">ColIndex</param>
        /// <returns>The IndexOf the Given Row Col in the SelectedCells Array</returns>
        private int GetPosition(int row, int col)
        {
            int i = 0;
            foreach (DataGridViewCell iCell in grid.SelectedCells)
            {
                if (iCell.ColumnIndex == col && iCell.RowIndex == row)
                {
                    if (iCell.Frozen || grid.Rows[iCell.RowIndex].Frozen || grid.Columns[iCell.ColumnIndex].Frozen)
                    {
                        return -1;
                    }

                    break;
                }
                i++;
            }
            return i;
        }

        /// Flags

        private bool rowSelection;
        private bool colSelection;

        /// <summary>
        /// Gets the Corner Cell of the Selected Cells
        /// </summary>
        /// <param name="cells">SelectedCellCollection Array</param>
        /// <returns>The Indexof the Corner Cell in the Array</returns>
        private int GetCornerCell(DataGridViewSelectedCellCollection cells)
        {
            int result = -1;
            DataGridViewCell cell;
            if (selectAllFlag)
            {
                selectAllFlag = false;
                DataGridViewCell cellTemp = GetCell(FixedRows, FixedColumns);
                return GetPosition(cellTemp.RowIndex, cellTemp.ColumnIndex);
            }
            else if (colSelection)
            {
                cell = grid.SelectedCells[grid.SelectedCells.Count - 1];
                DataGridViewCell cellTemp = GetCell(FixedRows, grid.RowHeadersVisible ? cell.ColumnIndex + 1 : cell.ColumnIndex);
                return GetPosition(cellTemp.RowIndex, cellTemp.ColumnIndex);
            }
            else if (rowSelection)
            {
                cell = grid.SelectedCells[grid.SelectedCells.Count - 1];
                DataGridViewCell cellTemp = GetCell(grid.ColumnHeadersVisible ? cell.RowIndex + 1 : cell.RowIndex, FixedColumns);
                return GetPosition(cellTemp.RowIndex, cellTemp.ColumnIndex);
            }

            if (grid.SelectedCells.Count == 1)
            {
                cell = grid.SelectedCells[0];
                return cell.Frozen || grid.Rows[cell.RowIndex].Frozen || grid.Columns[cell.ColumnIndex].Frozen ? -1 : 0;
            }

            DataGridViewCell cellTemp2;
            switch (SelectionMode)
            {
                case DataGridViewSelectionMode.CellSelect:
                    result = grid.SelectedCells.Count - 1;
                    break;
                case DataGridViewSelectionMode.FullRowSelect:
                    cell = grid.SelectedCells[grid.SelectedCells.Count - 1];
                    cellTemp2 = GetCell(grid.ColumnHeadersVisible ? cell.RowIndex + 1 : cell.RowIndex, FixedColumns);
                    result = GetPosition(cellTemp2.RowIndex, cellTemp2.ColumnIndex);
                    break;
                case DataGridViewSelectionMode.FullColumnSelect:
                    cell = grid.SelectedCells[grid.SelectedCells.Count - 1];
                    cellTemp2 = GetCell(FixedRows, grid.RowHeadersVisible ? cell.ColumnIndex + 1 : cell.ColumnIndex);
                    result = GetPosition(cellTemp2.RowIndex, cellTemp2.ColumnIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }






        private Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings _highLight = Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings.HighlightAlways;

        /// <summary>
        /// Returns/sets whether selected cells appear highlighted.
        /// </summary>
        public Artinsoft.Windows.Forms.ExtendedDataGridView.HighLightSettings HighLight
        {
            get { return _highLight; }
            set
            {
                if (grid.DesignMode)
                {
                    return;
                }

                _highLight = value;
                grid.Refresh();
            }
        }


        /// <summary>
        /// Overrides the method of the base parent
        /// </summary>
        /// <param name="e">Event Arguments</param>
        public void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex == -1 && AllowBigSelection)
            {
                selectAllFlag = true;
            }
            else if (e.ColumnIndex == -1)
            {
                if (SelectionMode == DataGridViewSelectionMode.CellSelect && AllowBigSelection)
                {
                    grid.BaseSelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

                    rowSelection = true;
                    colSelection = false;
                }
            }
            else if (e.RowIndex == -1)
            {
                if (SelectionMode == DataGridViewSelectionMode.CellSelect && AllowBigSelection)
                {
                    grid.BaseSelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
                    rowSelection = false;
                    colSelection = true;
                }
            }
            else
            {
                if (SelectionMode == DataGridViewSelectionMode.CellSelect)
                {
                    grid.BaseSelectionMode = DataGridViewSelectionMode.CellSelect;
                    rowSelection = false;
                    colSelection = false;
                }
            }

            grid.BaseOnCellMouseDown(e);
        }





        #endregion




        #region IGridBehaviour Members


		/// <summary>
		/// Called when the selection changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
        public void OnSelectionChanged(EventArgs e)
        {
            grid.BaseOnSelectionChanged(e);
        }

		/// <summary>
		/// Called when the DataMember value changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
        public void OnDataMemberChanged(EventArgs e)
        {
        }

		/// <summary>
		/// Called when the DataSource changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
        public void OnDataSourceChanged(EventArgs e)
        {
            CopyDataFromDataSource();
        }

		/// <summary>
		/// Called when a data error occurs.
		/// </summary>
		/// <param name="b">Indicates if an error should be displayed if there is no handler.</param>
		/// <param name="e">The event arguments.</param>
		public void OnDataError(bool b, DataGridViewDataErrorEventArgs e)
		{
		}

        private void CopyDataFromDataSource()
        {
            int numColumnsInDataSource = 0;
            if (DataSource is System.ComponentModel.ITypedList)
            {

                System.ComponentModel.PropertyDescriptorCollection columnInfo = ((System.ComponentModel.ITypedList)DataSource).GetItemProperties(null);
                numColumnsInDataSource = columnInfo.Count;
                ColumnsCount = FixedColumns + columnInfo.Count;
                if (grid.RowHeadersVisible)
                {
                    int colIndex = 0;
                    foreach (System.ComponentModel.PropertyDescriptor columnData in columnInfo)
                    {
                        grid.Columns[colIndex++].HeaderText = columnData.DisplayName;
                    } // foreach
                } // if

            } // if
            if (DataSource is IList)
            {
                IList data = DataSource as IList;
                int _FixedRows = FixedRows;
                RowsCount = _FixedRows + data.Count;
                //First check if there is data
                if (data.Count > 0 && numColumnsInDataSource > 0)
                {
                    int _FixedColumns = FixedColumns;
                    int rowindex = _FixedRows;
                    foreach (object rowObj in data)
                    {
                        DataRowView rowView = rowObj as DataRowView;
                        if (rowObj != null)
                        {
                            for (int i = 0; i < numColumnsInDataSource; i++)
                                this[_FixedColumns + i, rowindex].Value = rowView[i];
                        } // if
                        rowindex++;
                    } // foreach

                } // if

            } // if
        }

        #endregion

        #region IGridBehaviour Members

        object _dataSource;
		/// <summary>
		/// Gets/sets the Data Source that the grid will use.
		/// </summary>
        public object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                OnDataSourceChanged(EventArgs.Empty);
            }
        }

        #endregion





        /// <summary>
        /// Returns the left position of the current cell
        /// </summary>
        public int CellLeft
        {
            get 
            {
                if (GeneralCurrentCell != null)
                {
                    if (GeneralCurrentCell.ColumnIndex != -1 && GeneralCurrentCell.RowIndex != -1)
                        return grid.GetCellDisplayRectangle(GeneralCurrentCell.ColumnIndex, GeneralCurrentCell.RowIndex, false).Left;
                    return 0;
                }
                else
                    return 0;

            
            }
        }


        /// <summary>
        /// Returns or sets the top position of the current cell
        /// </summary>
        public int CellTop
        {
            get
            {
                if (GeneralCurrentCell != null)
                {
                    if (GeneralCurrentCell.ColumnIndex != -1 && GeneralCurrentCell.RowIndex != -1)
                        return grid.GetCellDisplayRectangle(GeneralCurrentCell.ColumnIndex, GeneralCurrentCell.RowIndex, false).Top;
                    return 0;
                }
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the height of the current cell.
        /// </summary>
        public int CellHeight
        {
            get
            {
                if (GeneralCurrentCell != null)
                {
                    if (GeneralCurrentCell.RowIndex == -1)
                        return grid.ColumnHeadersHeight;
                    else
                        return grid.Rows[GeneralCurrentCell.RowIndex].Height;
                }
                return 0;
            }
        }

        /// <summary>
        /// Returns the width of the current cell
        /// </summary>
        public int CellWidth
        {
            get
            {
                if (GeneralCurrentCell != null)
                {
                    if (GeneralCurrentCell.ColumnIndex == -1)
                        return grid.RowHeadersWidth;
                    else
                        return GeneralCurrentCell != null ? grid.Columns[GeneralCurrentCell.ColumnIndex].Width : 0;
                }
                return 0;
            }
        }


        /// <summary>
        /// Returns/sets an image to be displayed in the currently selected cells
        /// </summary>
        public Image CellPicture
        {
            get
            {
                DataGridViewImageCell imageCell = GeneralCurrentCell as DataGridViewImageCell;
                if (imageCell != null)
                {
                    return (Image)imageCell.Value;
                }
                return null;
            }
            set
            {
                if (GeneralCurrentCell != null)
                {
                    if (grid._fillStyle == Artinsoft.Windows.Forms.ExtendedDataGridView.FillStyleSettings.FillSingle)
                    {
                        if (value != null)
                        {
                            if (!(GeneralCurrentCell is DataGridViewImageCell))
                            {
                                Artinsoft.Windows.Forms.ExtendedDataGridView.CustomCell cell = GeneralCurrentCell as Artinsoft.Windows.Forms.ExtendedDataGridView.CustomCell;
                                if (cell != null)
                                    cell.CellPicture = value;
                            }
                        }
                        else
                        {
                            if (GeneralCurrentCell is DataGridViewImageCell)
                            {
                                int row, col;
                                row = GeneralCurrentCell.RowIndex;
                                col = GeneralCurrentCell.ColumnIndex;
                                if (row > 0 && col > 0)
                                    this[col, row] = new Artinsoft.Windows.Forms.ExtendedDataGridView.CustomCell();
                            }
                        }
                    }
                    else
                    {
                        foreach (DataGridViewCell dataGridViewCell in grid.SelectedCells)
                        {
                            DataGridViewImageCell imageCell = dataGridViewCell as DataGridViewImageCell;
                            if (value != null)
                            {
                                if (imageCell == null)
                                {
                                    int row, col;
                                    row = dataGridViewCell.RowIndex;
                                    col = dataGridViewCell.ColumnIndex;
                                    if (row > 0 && col > 0)
                                    {
                                        DataGridViewImageCell cell = new DataGridViewImageCell();
                                        cell.Value = value;
                                        this[col, row] = cell;
                                    }
                                }
                            }
                            else if (imageCell != null)
                            {
                                int row, col;
                                row = dataGridViewCell.RowIndex;
                                col = dataGridViewCell.ColumnIndex;
                                if (row > 0 && col > 0)
                                    this[col, row] = new Artinsoft.Windows.Forms.ExtendedDataGridView.CustomCell();
                            }
                        }
                    }
                }
            }
        }

        #endregion
       
    }

	/// <summary>
	/// Comparer class that can be used with the Flex Grid behaviour of the ExtendedDataGridView.
	/// </summary>
    public class FlexComparer : System.Collections.IComparer
    {
        private ExtendedDataGridView.SortSettings sortSetting = ExtendedDataGridView.SortSettings.SortGenericAscending;
        private int sortOrder = 1;
		/// <summary>
		/// Creates a FlexComparer with the specified sort order.
		/// </summary>
		/// <param name="sortOrder">The sort order to use.</param>
        public FlexComparer(ExtendedDataGridView.SortSettings sortOrder)
        {
            sortSetting = sortOrder;
            this.sortOrder = (sortOrder == ExtendedDataGridView.SortSettings.SortGenericAscending ||
                        sortOrder == ExtendedDataGridView.SortSettings.SortNumericAscending ||
                        sortOrder == ExtendedDataGridView.SortSettings.SortStringAscending ||
                        sortOrder == ExtendedDataGridView.SortSettings.SortStringNoCaseAscending) ? 1 : -1;
        }

		/// <summary>
		/// Compares two objects using the internal sort order.
		/// </summary>
		/// <param name="x">The first object</param>
		/// <param name="y">The second object</param>
		/// <returns>The difference in values between the items.  If they are equal, returns 0.</returns>
        public int Compare(object x, object y)
        {
            DataGridViewRow DataGridViewRow1 = (DataGridViewRow)x;
            DataGridViewRow DataGridViewRow2 = (DataGridViewRow)y;
            int CompareResult = 0;
            int currentColumnIndex = 0;


            if (DataGridViewRow1.DataGridView.SelectedCells.Count > 0)
            {
                //set the minimum valid value to the currentColumnIndex
				currentColumnIndex = Math.Max(0, DataGridViewRow1.DataGridView.SelectedCells[0].ColumnIndex);
                //check the maximum valid value to the currentColumnIndex
                currentColumnIndex = Math.Min(currentColumnIndex, DataGridViewRow1.DataGridView.Columns.Count);
            }

            String value1 = String.Empty;
            if (DataGridViewRow1.Cells[currentColumnIndex].Value != null)
            {
                value1 = DataGridViewRow1.Cells[currentColumnIndex].Value.ToString();
            }
            else
                return -1 * sortOrder;

            String value2 = String.Empty;
            if (DataGridViewRow2.Cells[currentColumnIndex].Value != null)
            {
                value2 = DataGridViewRow2.Cells[currentColumnIndex].Value.ToString();
            }
            else return 1*sortOrder;

            Double tempGeneric1 = 0.0;
            Double tempGeneric2 = 0.0;
            bool value1IsNumeric = false;
            bool value2IsNumeric = false;

            switch (sortSetting)
            {
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortNone:
                    CompareResult = 0;
                    break;
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortGenericAscending:
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortGenericDescending:
                    value1IsNumeric = Double.TryParse(value1, out tempGeneric1);
                    value2IsNumeric = Double.TryParse(value2, out tempGeneric2);
                    if (value1IsNumeric && value2IsNumeric)
                    {
                        CompareResult = tempGeneric1 > tempGeneric2 ? 1 : tempGeneric1 < tempGeneric2 ? -1 : 0;
                    }
                    else if (value1IsNumeric && !value2IsNumeric)
                    {
                        CompareResult = -1;
                    }
                    else if (!value1IsNumeric && value2IsNumeric)
                    {
                        CompareResult = 1;
                    }
                    else
                    {
                        CompareResult = System.String.Compare(value1, value2);
                    }
                    break;
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortStringAscending:
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortStringDescending:
                    CompareResult = System.String.Compare(value1, value2);
                    break;
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortStringNoCaseAscending:
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortStringNoCaseDescending:
                    CompareResult = System.String.Compare(value1, value2, true);
                    break;
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortNumericAscending:
                case Artinsoft.Windows.Forms.ExtendedDataGridView.SortSettings.SortNumericDescending:
                    value1IsNumeric = Double.TryParse(value1, out tempGeneric1);
                    value2IsNumeric = Double.TryParse(value2, out tempGeneric2);
                    if (value1IsNumeric && value2IsNumeric)
                    {
                        CompareResult = tempGeneric1 > tempGeneric2 ? 1 : tempGeneric1 < tempGeneric2 ? -1 : 0;
                    }
                    else if (value1IsNumeric && !value2IsNumeric)
                    {
                        CompareResult = 1;
                    }
                    else if (!value1IsNumeric && value2IsNumeric)
                    {
                        CompareResult = -1;
                    }
                    else
                    {
                        CompareResult = 0;
                    }
                    break;
            }
            return CompareResult * sortOrder;
        }
    }
}
