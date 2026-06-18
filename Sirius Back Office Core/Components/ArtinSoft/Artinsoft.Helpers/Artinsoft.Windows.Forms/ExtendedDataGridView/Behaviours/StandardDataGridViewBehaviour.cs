using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections.Generic;
namespace Artinsoft.Windows.Forms
{
	/// <summary>
	/// This class represents the behaviour of a Standard Grid to be used with an ExtendedDataGridView.
	/// </summary>
    public class StandardDataGridViewBehaviour : IGridBehaviour, IFlexGridBehaviour
    {
        internal ExtendedDataGridView grid;

        Dictionary<String, object> tempValues;

        Dictionary<String, object> TempValue
        {
            get
            {
                if (tempValues == null)
                    tempValues = new Dictionary<string, object>();
                return tempValues;
            }
        }

        private void AddTempValue<T>(string key,T value)
        {
            if (tempValues.ContainsKey(key))
                tempValues[key] = value;
            else
                tempValues.Add(key, value);
        }

        private int GetIntTempValue(string key, int default_value)
        {
            if (tempValues.ContainsKey(key))
                return (int)tempValues[key];
            else
                return default_value;
        }

		/// <summary>
		/// Creates a StandardDataGridViewBehaviour for a specified grid, usgin a previous behaviour as base.
		/// </summary>
		/// <param name="grid">The grid for which to create this behaviour.</param>
		/// <param name="previousBehaviour">The behaviour from which to copy information.</param>

        public StandardDataGridViewBehaviour(Artinsoft.Windows.Forms.ExtendedDataGridView grid, IGridBehaviour previousBehaviour)
        {
            this.grid = grid;
            CopyBehaviourProperties(previousBehaviour);

        }

        private void CopyBehaviourProperties(IGridBehaviour previousBehaviour)
        {
            if (previousBehaviour != null)
            {
                IFlexGridBehaviour previousBehaviourFlex = previousBehaviour as IFlexGridBehaviour;
                Dictionary<String, object> oldValues = new Dictionary<string, object>();
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
                oldValues.Add("DataSource", previousBehaviour.DataSource);
                Reset(previousBehaviour); //To set to defaults
                BeginInit();
                if (previousBehaviour.GridLineWidth != -1)
                {
                    GridLineWidth = previousBehaviour.GridLineWidth;
                }
                this.SelectionMode = previousBehaviour.SelectionMode;
                
                if (Convert.ToInt32(oldValues["RowHeightMin"])>0)
                    RowHeightMin = Convert.ToInt32(oldValues["RowHeightMin"]);

                DataSource = oldValues["DataSource"];

            }

        }


        #region IGridBehaviour Members

		/// <summary>
		/// Gets/sets the index of the first displayed row that is not a fixed row.
		/// </summary>
		public int FirstDisplayedScrollingRowIndex
        {
            get { return grid.BaseFirstDisplayedScrollingRowIndex; }
            set
            {
                grid.BaseFirstDisplayedScrollingRowIndex = value;
            }
        }

		/// <summary>
		/// Gets/sets the index of the first displayed column that is not a fixed column.
		/// </summary>
        public int FirstDisplayedScrollingColumnIndex
        {
            get { return grid.BaseFirstDisplayedScrollingColumnIndex; }
            set
            {
                grid.FirstDisplayedScrollingColumnIndex = value;
            }
        }

		/// <summary>
		/// Gets/sets the minimum row height allowed for the grid.
		/// </summary>
        public int RowHeightMin
        {
            get 
            {
                if (IsInitializing)
                    return GetIntTempValue("RowHeightMin",-1);
                else 
                    return grid.Rows.Count > 0 ? grid.Rows[0].MinimumHeight : 0; 
            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("RowHeightMin", value);
                }
                else
                {
                    if (grid.DesignMode && value < 0)
                    {
                        throw new ArgumentException("RowHeightMin must be 0 or greater");
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
        }

        /// <summary>
        /// Resets the grid based on its previous behaviour, erasing all rows and columns, while holding the DataSource intact.
        /// </summary>
        /// <param name="previous">IGridBehaviour instance representing the previous behaviour held by the Grid</param>
        public virtual void Reset(IGridBehaviour previous)
        {
            if (!(previous is ValueHolderBehaviour))
            {
                object savedDataSource = null;
                if (grid.DataSource != null)
                    savedDataSource = grid.DataSource;
                grid.Columns.Clear();
                grid.Rows.Clear();
                if (savedDataSource != null)
                    grid.DataSource = savedDataSource;
            }
            grid.EditMode = DataGridViewEditMode.EditOnEnter;
        }

		/// <summary>
		/// Accessor for the cells of the grid.
		/// </summary>
		/// <param name="columnindex">The column of the desired cell.</param>
		/// <param name="rowindex">The row of the desired cell.</param>
		/// <returns>The DataGridViewCell at the specified location.</returns>
        public DataGridViewCell this[int columnindex, int rowindex]
        {
            get { return grid.BaseGetCell(columnindex, rowindex); }
            set { grid.BaseSetCell(columnindex,rowindex,value); }
        }

        bool _isInitializing;
		/// <summary>
		/// Indicates if the grid is being initialized or not.
		/// </summary>
        public bool IsInitializing
        {
            get { return _isInitializing; }
            set { _isInitializing = true; }
        }

		/// <summary>
		/// Gets/sets the current cell.
		/// </summary>
        public DataGridViewCell CurrentCell
        {
            get
            {
                return grid.BaseCurrentCell;
            }
            set
            {
                grid.BaseCurrentCell = value;
            }
        }
		/// <summary>
		/// Gets/sets the index of the currently selected column.
		/// </summary>
        public int CurrentColumnIndex
        {
            get
            {
                if (grid.BaseCurrentCell == null) return -1;
                else return grid.BaseCurrentCell.ColumnIndex;
            }
            set
            {
                if (grid.BaseCurrentCell != null)
                {
                    int newcolumnindex = value;
                    DataGridViewCell newCell = grid[newcolumnindex, grid.BaseCurrentCell.RowIndex];
                    grid.BaseCurrentCell = newCell;
                }
            }
        }

		/// <summary>
		/// Gets/sets the index of the currently selected row.
		/// </summary>
		public int CurrentRowIndex
        {
            get
            {
                if (grid.BaseCurrentCell == null) return -1;
                else return grid.BaseCurrentCell.RowIndex;
            }
            set
            {
                if (grid.Columns == null || grid.Columns.Count == 0)
                    return;
                int currentColumnIndex = 0;
                if (grid.BaseCurrentCell != null)
                {
                    currentColumnIndex = grid.BaseCurrentCell.ColumnIndex;
                }
                int newrowindex = value;
                DataGridViewCell newCell = grid[currentColumnIndex, newrowindex];
                grid.BaseCurrentCell = newCell;

            }
        }

		/// <summary>
		/// Gets/sets the currently selected row.
		/// </summary>
		public int RowSel
        {
            get
            {
                if (IsInitializing)
                    return GetIntTempValue("RowSel", -1);
                else
                    return -1;
            }
            set
            {
                if (IsInitializing)
                    AddTempValue("RowSel", value);
            }
        }

		/// <summary>
		/// Gets/sets the currently selected column.
		/// </summary>
		public int ColSel
        {
            get
            {
                if (IsInitializing)
                    return GetIntTempValue("ColSel", -1);
                else
                    return -1;
            }
            set
            {
                if (IsInitializing)
                    AddTempValue("ColSel", value);
            }
        }
		/// <summary>
		/// Gets/sets the word wrap behaviour of the grid.
		/// </summary>
        public DataGridViewTriState WordWrap
        {
            get
            {
                if (IsInitializing)
                {
                    return GetTempValue("WordWrap", DataGridViewTriState.NotSet);
                }
                else
                    return grid.BaseWordWrap;
            }
            set
            {
                grid.BaseWordWrap = value;
            }
        }

        private T GetTempValue<T>(string key, T default_value)
        {
            if (TempValue.ContainsKey(key))
                return (T)tempValues[key];
            else
                return default_value;
        }

		/// <summary>
		/// Gets/sets the line width for the grid's lines.
		/// </summary>
        public int GridLineWidth
        {
            get
            {
                if (IsInitializing)
                {
                    return GetIntTempValue("GridLineWidth", -1);
                }
                else
                {
                    if (grid.Columns.Count > 0)
                    {
                        return grid.Columns[0].DividerWidth;
                    }
                    else
                        return 0;
                }
            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("GridLineWidth", value);
                }
                else
                {
                    foreach (DataGridViewColumn col in grid.Columns)
                    {
                        col.DividerWidth = value;
                    }
                }

            }
        }

		/// <summary>
		/// Gets/sets the selection mode with which the grid works.
		/// </summary>
        public DataGridViewSelectionMode SelectionMode
        {
            get
            {
                if (IsInitializing)
                {
                    return GetTempValue("SelectionMode", DataGridViewSelectionMode.CellSelect);
                }
                else
                    return grid.BaseSelectionMode;
            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("SelectionMode", value);
                }
                else
                    grid.BaseSelectionMode = value;
            }
        }

		/// <summary>
		/// Gets the index of the row over which the mouse is hovering.
		/// </summary>
		public int MouseRow
        {
            get 
            {
                return grid.mouse_cell_row;
            }
        }
		/// <summary>
		/// Gets the index of the column over which the mouse is hovering.
		/// </summary>
        public int MouseCol
        {
            get { return grid.mouse_cell_column; }
        }
		/// <summary>
		/// Gets/sets the amount of rows in the grid.
		/// </summary>
        public int RowsCount
        {
            get
            {
                if (IsInitializing)
                {
                    return GetIntTempValue("RowsCount", -1);
                }
                else
                    return grid.Rows.Count;
            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("RowsCount", value);
                }
                else
                {
                    if (grid.Rows.Count < value)
                    {
                        int diff = value - grid.Rows.Count;
                        grid.Rows.Add(diff);
                    }
                    else if (grid.Rows.Count > value)
                    {
                        int index = value - 1;
                        int diff = grid.Rows.Count - value;
                        for (int i = grid.Rows.Count - 1; i < index; i--)
                        {
                            grid.Rows.RemoveAt(i);
                        }
                    }
                }
            }
        }
		/// <summary>
		/// Gets/sets the amount of columns in the grid.
		/// </summary>
        public int ColumnsCount
        {
            get
            {
                if (IsInitializing)
                {
                    return GetIntTempValue("ColumnsCount", -1);
                }
                return grid.Columns.Count;
            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("ColumnsCount", value);
                }
                else
                {
                    if (grid.Columns.Count < value)
                    {
                        int diff = value - grid.Columns.Count;
                        for (int i = 0; i < diff; i++)
                            grid.Columns.Add(new DataGridViewColumn());
                    }
                    else if (grid.Rows.Count > value)
                    {
                        int index = value - 1;
                        int diff = grid.Columns.Count - value;
                        for (int i = grid.Columns.Count - 1; i < index; i--)
                        {
                            grid.Columns.RemoveAt(i);
                        }
                    }
                }    
            }
        }
		/// <summary>
		/// Called when the selection on the grid changes.
		/// </summary>
		/// <param name="e"></param>
        public void OnSelectionChanged(EventArgs e)
        {
            //Nothing
        } 
		/// <summary>
		/// Clear the selection.
		/// </summary>
        public void ClearSelection()
        {
            grid.BaseClearSelection();
        }
        #endregion

        #region ISupportInitialize Members

		/// <summary>
		/// Begin initializing the behaviour.
		/// </summary>
        public virtual void BeginInit()
        {
            tempValues = new Dictionary<string, object>();
        }

		/// <summary>
		/// Finish initializing the behaviour.
		/// </summary>
        public virtual void EndInit()
        {
            
        }

        #endregion

        #region IFlexGridBehaviour Members

		/// <summary>
		/// Big Selection is not allowed.  Always returns false, and setting the value does nothing.
		/// </summary>
        public bool AllowBigSelection
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

		/// <summary>
		/// Gets/sets the number of fixed rows.
		/// </summary>
        public int FixedRows
        {
            get
            {
                if (IsInitializing)
                {
                    return GetIntTempValue("FixedRows", -1);
                }
                else
                    return -1;
            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("FixedRows", value);
                }
            }
        }

		/// <summary>
		/// Gets/sets the number of fixed columns.
		/// </summary>
        public int FixedColumns
        {
            get
            {
                if (IsInitializing)
                {
                    return GetIntTempValue("FixedColumns", -1);
                }
                else
                    return -1;

            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("FixedColumns", value);
                }                
            }
        }

		/// <summary>
		/// Gets/sets the background color of fixed cells.
		/// </summary>
        public Color BackColorFixed
        {
            get
            {
                if (IsInitializing)
                {
                    return GetTempValue("BackColorFixed", Color.Empty);
                }
                else
                    return Color.Empty;
            }
            set
            {

                if (IsInitializing)
                {
                    AddTempValue("BackColorFixed", value);
                }
            }
        }

		/// <summary>
		/// Gets/sets the Focus Rectangle settings.
		/// </summary>
        public ExtendedDataGridView.FocusRectSettings FocusRect
        {
            get
            {
                if (IsInitializing)
                {
                    return GetTempValue("FocusRect", ExtendedDataGridView.FocusRectSettings.FocusNone);
                }
                else
                    return ExtendedDataGridView.FocusRectSettings.FocusNone; 
            }
            set
            {
                if (IsInitializing)
                {
                    AddTempValue("FocusRect", value);
                }
            }
        }

		/// <summary>
		/// Gets/sets the Highlight settings.
		/// </summary>
        public ExtendedDataGridView.HighLightSettings HighLight
        {
            get
            {
                if (IsInitializing)
                {
                    return GetTempValue("HighLight", ExtendedDataGridView.HighLightSettings.HighlightNever);
                }
                else 
                    return ExtendedDataGridView.HighLightSettings.HighlightNever;
            }
            set
            {
                if (IsInitializing)
                    AddTempValue("HighLight", value);
            }
        }

		/// <summary>
		/// Called when the mouse button is pressed down on a cell.
		/// </summary>
		/// <param name="e">The event arguments.</param>
        public void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            grid.BaseOnCellMouseDown(e);
        }
        #endregion

        #region IGridBehaviour Members

		/// <summary>
		/// Called when the DataMember value changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
        public virtual void OnDataMemberChanged(EventArgs e)
        {
        }

		/// <summary>
		/// Called when the DataSource value changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
        public virtual void OnDataSourceChanged(EventArgs e)
        {
            grid.BaseOnDataSourceChanged(e);
        }

		/// <summary>
		/// Called when there is a data error.
		/// </summary>
		/// <param name="b">Indicates if a message should be displayed.</param>
		/// <param name="e">The event arguments.</param>
		public virtual void OnDataError(bool b, DataGridViewDataErrorEventArgs e)
		{
		}

        /// <summary>
        /// Gets/Sets the DataSource to which the grid is attached. 
        /// In this behaviour it is just like the DataGridView.DataSource property
        /// </summary>
        public virtual object DataSource
        {
            get
            {
                return grid.BaseDataSource;
            }
            set
            {
                grid.BaseDataSource = value;
            }
        }

        #endregion

        #region IGridBehaviour Members


        /// <summary>
        /// Returns the left position of the current cell
        /// </summary>
        public int CellLeft
        {
            get 
            {
                if (grid.BaseCurrentCell != null )
                {
                    DataGridViewCell cell = grid.BaseCurrentCell;
                    if (cell.RowIndex != -1 && cell.ColumnIndex != -1)
                    {
                        return grid.GetCellDisplayRectangle(cell.RowIndex, cell.ColumnIndex, false).Left;
                    }
                    return 0;
                } // get
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
                if (grid.BaseCurrentCell != null)
                {
                    DataGridViewCell cell = grid.BaseCurrentCell;
                    if (cell.RowIndex != -1 && cell.ColumnIndex != -1)
                    {
                        return grid.GetCellDisplayRectangle(cell.RowIndex, cell.ColumnIndex, false).Top;
                    }
                    return 0;
                } // get
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
                if (grid.BaseCurrentCell != null)
                {
                    int rowIndex = grid.BaseCurrentCell.RowIndex;
                    if (rowIndex == -1)
                        return grid.RowTemplate.Height;
                    else
                        return grid.Rows[rowIndex].Height;
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
                if (grid.BaseCurrentCell != null)
                {
                    int colIndex = grid.BaseCurrentCell.ColumnIndex;
                    if (colIndex == -1)
                        return grid.RowHeadersWidth;
                    else
                        return grid.Columns[colIndex].Width;
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
                DataGridViewImageCell imageCell = grid.BaseCurrentCell as DataGridViewImageCell;
                if (imageCell != null)
                {
                    return (Image)imageCell.Value;
                }
                return null;
            }
            set 
            {
                DataGridViewImageCell imageCell = grid.BaseCurrentCell as DataGridViewImageCell;
                if (imageCell != null)
                {
                    imageCell.Value = value;
                } // if
            }
        } 



        #endregion

    }
}