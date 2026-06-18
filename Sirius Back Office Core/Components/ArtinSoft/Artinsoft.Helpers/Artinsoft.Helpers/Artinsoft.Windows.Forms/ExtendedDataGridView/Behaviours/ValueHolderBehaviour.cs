// Author: mrojas
// Project: Artinsoft.Windows.Forms
// Path: D:\VbcSPP\src\Helpers\Artinsoft.Windows.Forms\ExtendedDataGridView\Behaviours
// Creation date: 7/16/2009 2:51 PM
// Last modified: 9/17/2009 2:42 PM

#region Using directives
using Artinsoft.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Drawing;
#endregion
namespace Artinsoft.Windows.Forms
{
    /// <summary>
    /// Value holder behaviour
    /// </summary>
    internal class ValueHolderBehaviour : IGridBehaviour, IFlexGridBehaviour
    {
        internal const int UNSETVALUE = -5;
        /// <summary>
        /// My values
        /// </summary>
        Dictionary<string, object> myValues = new Dictionary<string, object>();
        #region IGridBehaviour Members

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset(IGridBehaviour previous)
        {
            myValues.Clear();
        } // Reset(previous)

        /// <summary>
        /// Is initializing
        /// </summary>
        public bool IsInitializing
        {
            get
            {
                return true;
            } // get
            set
            {
            } // set
        } // IsInitializing

        /// <summary>
        /// Current cell
        /// </summary>
        public DataGridViewCell CurrentCell
        {
            get
            {
                return null;
            } // get
            set
            {
            } // set
        } // CurrentCell

        /// <summary>
        /// Current column index
        /// </summary>
        public int CurrentColumnIndex
        {
            get
            {
                return UNSETVALUE;
            } // get
            set
            {
            } // set
        } // CurrentColumnIndex

        /// <summary>
        /// Current row index
        /// </summary>
        public int CurrentRowIndex
        {
            get
            {
                return UNSETVALUE;
            } // get
            set
            {

            } // set
        } // CurrentRowIndex

 
        /// <summary>
        /// Grid line width
        /// </summary>
        public int GridLineWidth
        {
            get
            {
                return GetValueIfFound("GridLineWidth", UNSETVALUE);
            } // get
            set
            {
                myValues["GridLineWidth"] = value;
            } // set
        } // GridLineWidth

        private T GetValueIfFound<T>(String keyname, T notfoundvalue)
        {
            if (!myValues.ContainsKey(keyname))
                return notfoundvalue;
            else
                return (T)myValues[keyname];
        } // GetValueIfFound(, keyname, notfoundvalue)

        /// <summary>
        /// Selection mode
        /// </summary>
        public DataGridViewSelectionMode SelectionMode
        {
            get
            {
                return GetValueIfFound("SelectionMode", DataGridViewSelectionMode.CellSelect);
            } // get
            set
            {
                myValues["SelectionMode"] = value;

            } // set
        } // SelectionMode

        /// <summary>
        /// The indexer for the Cell in the Grid
        /// </summary>
        public DataGridViewCell this[int columnindex, int rowindex]
        {
            get
            {
                return null;
            } // get
            set
            {

            } // set
        } // Item

        /// <summary>
        /// Mouse row
        /// </summary>
        public int MouseRow
        {
            get { return UNSETVALUE; } // get
        } // MouseRow

        /// <summary>
        /// Mouse color
        /// </summary>
        public int MouseCol
        {
            get { return UNSETVALUE; } // get
        } // MouseCol

        /// <summary>
        /// Rows count
        /// </summary>
        public int RowsCount
        {
            get
            {
                return GetValueIfFound("RowsCount", UNSETVALUE);
            } // get
            set
            {
                myValues["RowsCount"] = value;
            } // set
        } // RowsCount

        /// <summary>
        /// Columns count
        /// </summary>
        public int ColumnsCount
        {
            get
            {
                return GetValueIfFound("ColumnsCount", UNSETVALUE);
            } // get
            set
            {
                myValues["ColumnsCount"] = value;
            } // set
        } // ColumnsCount

        /// <summary>
        /// Row height minimum
        /// </summary>
        public int RowHeightMin
        {
            get
            {
                return GetValueIfFound("RowHeightMin", UNSETVALUE);
            } // get
            set
            {
                myValues["RowHeightMin"] = value;
            } // set
        } // RowHeightMin

        /// <summary>
        /// First displayed scrolling row index
        /// </summary>
        public int FirstDisplayedScrollingRowIndex
        {
            get
            {
                return UNSETVALUE;
            } // get
            set
            {

            } // set
        } // FirstDisplayedScrollingRowIndex

        /// <summary>
        /// First displayed scrolling column index
        /// </summary>
        public int FirstDisplayedScrollingColumnIndex
        {
            get
            {
                return UNSETVALUE;
            } // get
            set
            {
            } // set
        } // FirstDisplayedScrollingColumnIndex

        /// <summary>
        /// Clear selection
        /// </summary>
        public void ClearSelection()
        {
        } // ClearSelection()

        /// <summary>
        /// On selection changed
        /// </summary>
        public void OnSelectionChanged(EventArgs e)
        {
        } // OnSelectionChanged()

        #endregion

        #region ISupportInitialize Members

        /// <summary>
        /// Begin init
        /// </summary>
        public void BeginInit()
        {
        } // BeginInit()

        /// <summary>
        /// End init
        /// </summary>
        public void EndInit()
        {
        } // EndInit()

        #endregion

        #region IFlexGridBehaviour Members

        int IFlexGridBehaviour.RowSel
        {
            get
            {
                return GetValueIfFound("RowSel", UNSETVALUE);
            } // get
            set
            {
                myValues["RowSel"] = value;
            } // set

        } // IFlexGridBehaviour.RowSel

        int IFlexGridBehaviour.ColSel
        {
            get
            {
                return GetValueIfFound("ColSel", UNSETVALUE);
            } // get
            set
            {
                myValues["ColSel"] = value;
            } // set
        } // IFlexGridBehaviour.ColSel

        bool IFlexGridBehaviour.AllowBigSelection
        {
            get
            {
                return GetValueIfFound("AllowBigSelection", true);
            } // get
            set
            {
                myValues["AllowBigSelection"] = value;
            } // set
        } // IFlexGridBehaviour.AllowBigSelection

        int IFlexGridBehaviour.FixedRows
        {
            get
            {
                return GetValueIfFound("FixedRows", UNSETVALUE);
            } // get
            set
            {
                myValues["FixedRows"] = value;
            } // set
        } // IFlexGridBehaviour.FixedRows

        int IFlexGridBehaviour.FixedColumns
        {
            get
            {
                return GetValueIfFound("FixedColumns", UNSETVALUE);
            } // get
            set
            {
                myValues["FixedColumns"] = value;
            } // set
        } // IFlexGridBehaviour.FixedColumns

        Color IFlexGridBehaviour.BackColorFixed
        {
            get
            {
                return GetValueIfFound("BackColorFixed", Color.Empty);
            } // get
            set
            {
                myValues["BackColorFixed"] = value;
            } // set
        } // IFlexGridBehaviour.BackColorFixed

        ExtendedDataGridView.FocusRectSettings IFlexGridBehaviour.FocusRect
        {
            get
            {
                return GetValueIfFound("FocusRect", ExtendedDataGridView.FocusRectSettings.FocusNone);
            } // get
            set
            {
                myValues["FocusRect"] = value;
            } // set
        } // IFlexGridBehaviour.FocusRect

        ExtendedDataGridView.HighLightSettings IFlexGridBehaviour.HighLight
        {
            get
            {
                return GetValueIfFound("HighLight", ExtendedDataGridView.HighLightSettings.HighlightNever);
            } // get
            set
            {
                myValues["HighLight"] = value;
            } // set
        } // IFlexGridBehaviour.HighLight

        void IFlexGridBehaviour.OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {

        } // IFlexGridBehaviour.OnCellMouseDown()

        #endregion

        #region IGridBehaviour Members


        /// <summary>
        /// On data member changed
        /// </summary>
        public void OnDataMemberChanged(EventArgs e)
        {
        } // OnDataMemberChanged()

        /// <summary>
        /// On data source changed
        /// </summary>
        public void OnDataSourceChanged(EventArgs e)
        {
        } // OnDataSourceChanged()

		public void OnDataError(bool b, DataGridViewDataErrorEventArgs e)
		{
		}

        #endregion

        #region IGridBehaviour Members


        /// <summary>
        /// Data source
        /// </summary>
        public object DataSource
        {
            get
            {
                return GetValueIfFound<object>("DataSource",null);
            } // get
            set
            {
                myValues["DataSource"] = value;
            } // set
        } // DataSource


        public int CellLeft
        {
            get { return 0; }
        }



        public int CellTop
        {
            get { return 0; }
        }

        public int CellHeight
        {
            get { return 0; }
        }

        public int CellWidth
        {
            get { return 0; }
        }

        /// <summary>
        /// Cell picture
        /// </summary>
        public Image CellPicture { get { return null; } set { } } 


        #endregion

    } // class ValueHolderBehaviour
} // namespace Artinsoft.Windows.Forms
