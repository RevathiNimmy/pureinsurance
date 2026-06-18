// Author: mrojas
// Project: Artinsoft.Windows.Forms
// Path: Artinsoft.Windows.Forms\ExtendedDataGridView\Behaviours
// Creation date: 8/6/2009 2:29 PM
// Last modified: 9/18/2009 10:28 AM

#region Using statements
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
#endregion

namespace Artinsoft.Windows.Forms
{
	/// <summary>
	/// Interface for Grid Behaviour classes for use with the ExtendedDataGridView.
	/// </summary>
    public interface IGridBehaviour : ISupportInitialize
    {
        /// <summary>
        /// Sets all properties to their default values
        /// </summary>
        void Reset(IGridBehaviour previous);

		/// <summary>
		/// Gets/sets if the grid is being initialized.
		/// </summary>
        bool IsInitializing { get; set; }

        /// <summary>
        /// Returns the currently active Cell
        /// </summary>
        DataGridViewCell CurrentCell
        {
            get;
            set;
        }
        /// <summary>
        /// Sets or get the CurrentColumn. Setting this property will change the 
        /// CurrentCell
        /// </summary>
        int CurrentColumnIndex { get;set;}
		/// <summary>
		/// Gets/sets the CurrentRow.
		/// </summary>
        int CurrentRowIndex { get; set; }
		/// <summary>
		/// Gets/sets the grid lines' width.
		/// </summary>
        int GridLineWidth { get; set; }
		/// <summary>
		/// Gets/sets the SelectionMode the grid will be using.
		/// </summary>
        DataGridViewSelectionMode SelectionMode { get; set; }

		/// <summary>
		/// Gets/sets the cells of the grid.
		/// </summary>
		/// <param name="columnindex">The index of the column of the desired cell.</param>
		/// <param name="rowindex">The index of the row of the desired cell.</param>
		/// <returns>The Cell at the specified position.</returns>
        DataGridViewCell this[int columnindex, int rowindex]
        { get; set; }

		/// <summary>
		/// Gets/sets the DataSource the grid will be using.
		/// </summary>
        object DataSource { get; set; }



        /// <summary>
        /// Returns the row the mouse pointer currently is.
        /// </summary>
        int MouseRow { get; }

        /// <summary>
        /// Returns the column the mouse pointer currently is.
        /// </summary>
        int MouseCol { get; }

		/// <summary>
		/// Gets/sets the amount of rows the grid will have.
		/// </summary>
        int RowsCount { get; set; }
		/// <summary>
		/// Gets/set the amount of columns the grid will have.
		/// </summary>
        int ColumnsCount { get; set; }

		/// <summary>
		/// Gets/sets the minimum row height allowed for the grid.
		/// </summary>
		int RowHeightMin { get; set; }

		/// <summary>
		/// Gets/sets the index of the first displayed row, not including fixed rows.
		/// </summary>
		int FirstDisplayedScrollingRowIndex { get; set; }

		/// <summary>
		/// Gets/sets the index of the first displayed column, not including fixed columns.
		/// </summary>
		int FirstDisplayedScrollingColumnIndex { get; set; }

		/// <summary>
		/// Clears the grid's selection.
		/// </summary>
        void ClearSelection();

		/// <summary>
		/// Called when the selection changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		void OnSelectionChanged(EventArgs e);

		/// <summary>
		/// Called when the DataMember value changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		void OnDataMemberChanged(EventArgs e);
		/// <summary>
		/// Called when the DataSource changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		void OnDataSourceChanged(EventArgs e);

		/// <summary>
		/// Called when a data error occurs.
		/// </summary>
		/// <param name="displayErrorDialogIfNoHandler">Indicates if an error should be displayed if there is no handler.</param>
		/// <param name="e">The event arguments.</param>
		void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e);


        /// <summary>
        /// Cell left
        /// </summary>
        int CellLeft {get;} 


        /// <summary>
        /// Returns or sets the top position of the current cell
        /// </summary>
        int CellTop { get; }


          /// <summary>
        /// Returns the height of the current cell.
        /// </summary>
        int CellHeight { get; }


        /// <summary>
        /// Returns the width of the current cell
        /// </summary>
        int CellWidth { get; }


            /// <summary>
        /// Returns/sets an image to be displayed in the currently selected cells
        /// </summary>
        Image CellPicture { get; set; }

	}

   
    


}