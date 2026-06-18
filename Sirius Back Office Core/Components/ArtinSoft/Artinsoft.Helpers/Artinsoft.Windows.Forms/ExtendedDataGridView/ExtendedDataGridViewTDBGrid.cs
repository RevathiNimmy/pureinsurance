using System;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Drawing.Design;
using System.Collections.Generic;
using System.Collections;
using Artinsoft.VB6.DB.ADO;
using Artinsoft.VB6.DB.RDO;
using System.IO;
using Artinsoft.Windows.Forms.Properties;
using System.Diagnostics;

internal class ResFinder { }

namespace Artinsoft.Windows.Forms
{
    /// <summary>
    /// List of constants used to support column's NumberFormat property
    /// </summary>
    public static class NumberFormatConstants
    {
        /// <summary>
        /// Display number as is, with no thousand separators.
        /// </summary>
        public const string GeneralNumber = "G";
        /// <summary>
        /// Display number with thousand separator, if appropriate; display two digits to the right of the 
        /// decimal separator. Note that output is based on system locale settings.
        /// </summary>
        public const string Currency = "C";
        /// <summary>
        /// Display number with thousands separator, at least one digit to the left and two digits to the 
        /// right of the decimal separator.
        /// </summary>
        public const string Standard = "N2";
        /// <summary>
        /// Display number multiplied by 100 with a percent sign (%) appended to the right; always display 
        /// two digits to the right of the decimal separator.
        /// </summary>
        public const string Percent = "#.00%";
    }

    /// <summary>
    /// This is class implements a component that extends the
    /// System.Windows.Forms.DataGridView control.  It adds new properties and also
    /// provides &quot;Compatibility&quot; support for some Grid controls commonly used
    /// in  VB6: MSFlexGrid and APEX TrueDBGrid
    /// </summary>
    [ToolboxBitmap(typeof(ResFinder), "Artinsoft.Windows.Forms.Resources.ToolGrid.bmp")]
    public partial class ExtendedDataGridView : DataGridView, ISupportInitialize
    {
        #region Events from TrueDBGrid

        internal void RaiseCellUpdatedEvent(object sender, DataGridViewCellValueEventArgs e)
        {
            if (CellUpdated != null)
            {
                CellUpdated(sender, e);
            }
        }

        internal void RaiseCellUpdatingEvent(object sender, DataGridViewCellValueCancelEventArgs e)
        {
            if (CellUpdating != null)
            {
                CellUpdating(sender, e);
            }
        }

        internal void RaiseRowDeletedEvent(object sender, DataRowChangeEventArgs e)
        {
            if (RowDeleted != null)
            {
                RowDeleted(sender, e);
            }
        }

        internal void RaiseRowDeletingEvent(object sender, DataRowChangeEventArgs e)
        {
            if (RowDeleting != null)
            {
                RowDeleting(sender, e);
            }
        }

        internal void RaiseRowAddedEvent(object sender, DataTableNewRowEventArgs e)
        {
            if (RowAdded != null)
            {
                RowAdded(sender, e);
            }
        }

        /// <summary>
        /// Occurs after the data has updated its data source
        /// </summary>
        public event DataGridViewCellValueEventHandler CellUpdated;
        /// <summary>
        /// Occurs when the data is going to update its data source
        /// </summary>
        public event DataGridViewCellValueCancelEventHandler CellUpdating;
        /// <summary>
        /// Occurs after the record is deleted
        /// </summary>
        public event DataRowChangeEventHandler RowDeleted;
        /// <summary>
        /// Occurs before the record is deleting
        /// </summary>
        public event DataRowChangeEventHandler RowDeleting;
        /// <summary>
        /// Occurs when a new row is added
        /// </summary>
        public event DataTableNewRowEventHandler RowAdded;
        /// <summary>
        /// Occurs when the cell enters to edit mode
        /// </summary>
        public event DataGridViewCellEventHandler CellEdit;
        /// <summary>
        /// Occurs when the button of the column is clicked
        /// </summary>
        public event DataGridViewCellEventHandler ButtonClick;
        /// <summary>
        /// Occurs when the combo of the column is selected
        /// </summary>
        public event DataGridViewCellEventHandler ComboSelect;

        /// <summary>
        /// Fires up a new event called CellEdit just after the BeginEdit event is fired
        /// </summary>
        /// <param name="e">Event args of the cell</param>
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            base.OnCellBeginEdit(e);
            if (!e.Cancel && CellEdit != null)
            {
                CellEdit(this, new DataGridViewCellEventArgs(e.ColumnIndex, e.RowIndex));
            }
        }

        internal void RaiseButtonClickEvent(object sender, int colIndex, int rowIndex)
        {
            if (ButtonClick != null)
            {
                DataGridViewCellEventArgs args = new DataGridViewCellEventArgs(colIndex, rowIndex);
                ButtonClick(sender, args);
            }
        }

        internal void RaiseComboSelectEvent(object sender, int colIndex, int rowIndex)
        {
            if (ComboSelect != null)
            {
                DataGridViewCellEventArgs args = new DataGridViewCellEventArgs(colIndex, rowIndex);
                ComboSelect(sender, args);
            }
        }
        #endregion

        #region Overriden Events

        /// <summary>
        /// Overrides base OnDataSourceChanged and delegates the handling of the event
        /// down onto the specific grid behaviour implementation.
        /// </summary>
        /// <param name="e">EventArgs instance</param>
        protected override void OnDataSourceChanged(EventArgs e)
        {
            currentBehaviour.OnDataSourceChanged(e);
        }

        /// <summary>
        /// Overrides base OnDataMemberChanged and delegates the handling of the event
        /// down onto the specific grid behaviour implementation.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDataMemberChanged(EventArgs e)
        {
            currentBehaviour.OnDataMemberChanged(e);
        }

        /// <summary>
        /// Raises the DataError event. 
        /// </summary>
        /// <param name="displayErrorDialogIfNoHandler">true to display an error dialog box if there is no handler for the DataError event.</param>
        /// <param name="e">A DataGridViewDataErrorEventArgs that contains the event data.</param>
		protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
		{
			currentBehaviour.OnDataError(displayErrorDialogIfNoHandler, e);
		}


        #endregion

        #region Properties from TrueDBGrid

        /// <summary>
        /// Sets or returns the back color for the current selection.
        /// If multiple cells are selected it will return the first cell back color
        /// </summary>
        public Color SelectedBackColor
        {
            get
            {
                if (SelectedCells.Count > 1)
                {
                    return SelectedCells[0].Style.BackColor;
                }
                return SystemColors.AppWorkspace;
            }
            set
            {
                if (SelectedCells.Count > 1)
                {
                    foreach (DataGridViewCell cell in SelectedCells)
                    {
                        cell.Style.BackColor = value;
                    }
                }
            }
        }

        /// <summary>
        /// Sets or returns the fore color for the current selection.
        /// If multiple cells are selected it will return the first cell fore color
        /// </summary>
        public Color SelectedForeColor
        {
            get
            {
                if (SelectedCells.Count > 1)
                {
                    return SelectedCells[0].Style.ForeColor;
                }
                return SystemColors.AppWorkspace;
            }
            set
            {
                if (SelectedCells.Count > 1)
                {
                    foreach (DataGridViewCell cell in SelectedCells)
                    {
                        cell.Style.ForeColor = value;
                    }
                }
            }
        }

        /// <summary>
        /// Sets or returns the style for the current selection.
        /// If multiple cells are selected it will return the first cell style
        /// </summary>
        public DataGridViewCellStyle SelectedStyle
        {
            get
            {
                if (SelectedCells.Count > 1)
                {
                    return SelectedCells[0].Style;
                }
                return null;
            }
            set
            {
                if (SelectedCells.Count > 1)
                {
                    foreach (DataGridViewCell cell in SelectedCells)
                    {
                        cell.Style = value;
                    }
                }
            }
        }

        private DataGridViewCell _PreviousCell;

        /// <summary>
        /// Returns the previous cell before the user changed the current one
        /// </summary>
        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewCell PreviousCell
        {
            get
            {
                return _PreviousCell;
            }
            set
            {
                _PreviousCell = value;
            }
        }

        /// <summary>
        /// Sets the previous cell when the user leaves the current one
        /// </summary>
        /// <param name="e">Argument with the cell to leave</param>
        protected override void OnCellLeave(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                PreviousCell = this[e.RowIndex, e.ColumnIndex];
            }
            base.OnCellLeave(e);
        }

        #endregion

        #region Selection Text Properties

        internal bool HasSelectionTextProperties
        {
            get
            {
                return (CurrentCell != null || CurrentCell is DataGridExtendedCell ||
                    CurrentCell is DataGridViewTextBoxCell || CurrentCell is DataGridViewComboBoxCell);
            }
        }

        /// <summary>
        /// Gets or sets a character index for the beginning of the current selection.
        /// </summary>
        public int SelStart
        {
            get 
            {
                int result = -1;
                if (HasSelectionTextProperties)
                {
                    BeginEdit(false);
                    if (CurrentCell  is DataGridExtendedCell)
                    {
                        DataGridExtendedEditingControl editControl = (DataGridExtendedEditingControl)EditingControl;
                        result = editControl._TextBox.SelectionStart;
                    }
                    else if (CurrentCell is DataGridViewTextBoxCell)
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)EditingControl;
                        result = editControl.SelectionStart;
                    }
                    else if (CurrentCell is DataGridViewComboBoxCell)
                    {
                        DataGridViewComboBoxEditingControl editControl = (DataGridViewComboBoxEditingControl)EditingControl;
                        result = editControl.SelectionStart;
                    }
                }
                return result;
            }
            set 
            {
                if (HasSelectionTextProperties)
                {
                    BeginEdit(false);
                    if (CurrentCell is DataGridExtendedCell)
                    {
                        DataGridExtendedEditingControl editControl = (DataGridExtendedEditingControl)EditingControl;
                        editControl._TextBox.SelectionStart = value;
                    }
                    else if (CurrentCell is DataGridViewTextBoxCell)
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)EditingControl;
                        editControl.SelectionStart = value;
                    }
                    else if (CurrentCell is DataGridViewComboBoxCell)
                    {
                        DataGridViewComboBoxEditingControl editControl = (DataGridViewComboBoxEditingControl)EditingControl;
                        editControl.SelectionStart = value;
                    }
                } 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the number of characters in the current selection in the text box.
        /// </summary>
        public int SelLength
        {
            get
            {
                int result = -1;
                if (HasSelectionTextProperties)
                {
                    BeginEdit(false);
                    if (CurrentCell is DataGridExtendedCell)
                    {
                        DataGridExtendedEditingControl editControl = (DataGridExtendedEditingControl)EditingControl;
                        result = editControl._TextBox.SelectionLength;
                    }
                    else if (CurrentCell is DataGridViewTextBoxCell)
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)EditingControl;
                        result = editControl.SelectionLength;
                    }
                    else if (CurrentCell is DataGridViewComboBoxCell)
                    {
                        DataGridViewComboBoxEditingControl editControl = (DataGridViewComboBoxEditingControl)EditingControl;
                        result = editControl.SelectionLength;
                    }
                }
                return result;
            }
            set
            {
                if (HasSelectionTextProperties)
                {
                    BeginEdit(false);
                    if (CurrentCell is DataGridExtendedCell)
                    {
                        DataGridExtendedEditingControl editControl = (DataGridExtendedEditingControl)EditingControl;
                        editControl._TextBox.SelectionLength = value;
                    }
                    else if (CurrentCell is DataGridViewTextBoxCell)
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)EditingControl;
                        editControl.SelectionLength = value;
                    }
                    else if (CurrentCell is DataGridViewComboBoxCell)
                    {
                        DataGridViewComboBoxEditingControl editControl = (DataGridViewComboBoxEditingControl)EditingControl;
                        editControl.SelectionLength = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the content of the current selection in the text box.
        /// </summary>
        public string SelText
        {
            get
            {
                string result = string.Empty;
                if (HasSelectionTextProperties)
                {
                    BeginEdit(false);
                    if (CurrentCell is DataGridExtendedCell)
                    {
                        DataGridExtendedEditingControl editControl = (DataGridExtendedEditingControl)EditingControl;
                        result = editControl._TextBox.SelectedText;
                    }
                    else if (CurrentCell is DataGridViewTextBoxCell)
                    {
                        DataGridViewTextBoxEditingControl editControl = (DataGridViewTextBoxEditingControl)EditingControl;
                        result = editControl.SelectedText;
                    }
                    else if (CurrentCell is DataGridViewComboBoxCell)
                    {
                        DataGridViewComboBoxEditingControl editControl = (DataGridViewComboBoxEditingControl)EditingControl;
                        result = editControl.SelectedText;
                    }
                }
                return result;
            }
        }
        #endregion

        #region Methods from TrueDBGrid

        WeakReference weakRefToBindingSource;
        /// <summary>
        /// Disconnects the grid from the data source
        /// </summary>
        public void Close()
        {
            Close(true);
        }

        /// <summary>
        /// Disconnects the grid from the data source and optionally repaint the grid
        /// </summary>
        /// <param name="repaint">Determines if the gird should repaint</param>
        public void Close(bool repaint)
        {

            weakRefToBindingSource = new WeakReference(this.DataSource);

            if (repaint)
            {
                this.DataSource = null;
            }
            else
            {
                BindingSource bnd = null;
                if (this.DataSource is ADODataControlHelper)
                {
                    ADODataControlHelper dhelper = (ADODataControlHelper)this.DataSource;
                    bnd = new BindingSource(dhelper.Recordset, dhelper.Recordset.Tables[0].TableName);
                    bnd.Position = ((ADODataControlHelper)this.DataSource).Recordset.AbsolutePosition - 1;
                }
                else if (this.DataSource is RDODataControlHelper)
                {
                    RDODataControlHelper dhelper = (RDODataControlHelper)this.DataSource;
                    bnd = new BindingSource(dhelper.Recordset, dhelper.Recordset.Tables[0].TableName);
                    bnd.Position = ((RDODataControlHelper)this.DataSource).Recordset.AbsolutePosition - 1;
                }
                else if (this.DataSource is BindingSource)
                {
                    int position = ((BindingSource)this.DataSource).Position;
                    bnd = new BindingSource();
                    bnd.DataSource = this.DataSource;
                    bnd.Position = position;
                }
                this.DataSource = bnd;
                this.Enabled = false;
                //The following statement prevents the DataSource from reflecting any changes made 
                //to the underlying RecordSet or List either through the BindingSource or any other means
                ((BindingSource)this.DataSource).RaiseListChangedEvents = false;
            }
        }

        /// <summary>
        /// Determines the rows to be exported
        /// </summary>
        public enum RowSelectorConstants
        {
            /// <summary>
            /// Apply Selector to all rows
            /// </summary>
            AllRows,
            /// <summary>
            /// Apply Selector to selected rows only
            /// </summary>
            SelectedRows,
            /// <summary>
            /// Apply Selector to current row only
            /// </summary>
            CurrentRow
        }

        /// <summary>
        /// Exports all rows from the grid to the specified file in html format
        /// </summary>
        /// <param name="pathname">Specifies the file where the rows are exported</param>
        /// <param name="append">The file will be appended or created</param>
        public void ExportToFile(string pathname, bool append)
        {
            ExportToFile(pathname, append, RowSelectorConstants.AllRows, string.Empty);
        }

        /// <summary>
        /// Exports the specified rows from the grid to the specified file in html format
        /// </summary>
        /// <param name="pathname">Specifies the file where the rows are exported</param>
        /// <param name="append">The file will be appended or created</param>
        /// <param name="selector">The rows to be exported</param>
        public void ExportToFile(string pathname, bool append, RowSelectorConstants selector)
        {
            ExportToFile(pathname, append, selector, string.Empty);
        }

        /// <summary>
        /// Exports the specified rows from the grid to the specified file in html format
        /// </summary>
        /// <param name="pathname">Specifies the file where the rows are exported</param>
        /// <param name="append">The file will be appended or created</param>
        /// <param name="selector">The rows to be exported</param>
        /// <param name="tableWidth">The width to the new html table</param>
        public void ExportToFile(string pathname, bool append, RowSelectorConstants selector, int tableWidth)
        {
            ExportToFile(pathname, append, selector, tableWidth.ToString());
        }

        /// <summary>
        /// Exports the specified rows from the grid to the specified file in html format
        /// </summary>
        /// <param name="pathname">Specifies the file where the rows are exported</param>
        /// <param name="append">The file will be appended or created</param>
        /// <param name="selector">The rows to be exported</param>
        /// <param name="tableWidth">The width to the new html table</param>
        public void ExportToFile(string pathname, bool append, RowSelectorConstants selector, string tableWidth)
        {

            StringBuilder htmlTable = new StringBuilder(Resources.HtmlTableTemplate);
            tableWidth = tableWidth == null ? string.Empty : tableWidth;
            if (tableWidth.Length > 0)
            {
                tableWidth = "width=\"" + tableWidth + "\"";
            }
            htmlTable.Replace("#table_width#", tableWidth);

            StringBuilder htmlTableHeaders = new StringBuilder();
            StringBuilder htmlTableRows = new StringBuilder();

            StreamWriter htmlFile = null;

            StringBuilder tmpLine = null;
            try
            {
                #region Headers Generation Code
                double columnsWidth = 0;
                double colWidth = 0;
                foreach (DataGridViewColumn column in Columns)
                {
                    columnsWidth += column.Width;
                }

                //Append the headers of the new Html Table
                if (ColumnHeadersVisible)
                {
                    foreach (DataGridViewColumn column in Columns)
                    {
                        tmpLine = new StringBuilder(Resources.HtmlTableHeaderTemplate);
                        tmpLine.Replace("#header_Text#", column.HeaderText);

                        colWidth = (column.Width * 100 / columnsWidth);
                        colWidth = Math.Round(colWidth, MidpointRounding.AwayFromZero);
                        tmpLine.Replace("#width#", colWidth.ToString());

                        htmlTableHeaders.AppendLine(tmpLine.ToString());
                    }
                }
                #endregion Headers Generation Code

                #region Rows Generation Code
                ICollection selectedRows = null;
                switch (selector)
                {
                    case RowSelectorConstants.AllRows:
                        selectedRows = Rows;
                        break;
                    case RowSelectorConstants.CurrentRow:
                        selectedRows = new List<DataGridViewRow>();
                        (selectedRows as IList).Add(CurrentRow);
                        break;
                    case RowSelectorConstants.SelectedRows:
                        selectedRows = SelectedRows;
                        break;
                }

                foreach (DataGridViewRow row in selectedRows)
                {
                    if (AllowUserToAddRows && row.Index == selectedRows.Count - 1)
                    {
                        break;
                    }

                    StringBuilder cellLines = new StringBuilder();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        tmpLine = new StringBuilder(Resources.HtmlTableRowTemplate_Cell);
                        if (cell == null || cell.Value == null)
                        {
                            tmpLine.Replace("#cell_Text#", string.Empty);
                            tmpLine.Replace("#width#", string.Empty);
                        }
                        else
                        {
                            tmpLine.Replace("#cell_Text#", cell.Value.ToString());

                            colWidth = (cell.Size.Width * 100 / columnsWidth);
                            colWidth = Math.Round(colWidth, MidpointRounding.AwayFromZero);
                            tmpLine.Replace("#width#", colWidth.ToString());
                        }
                        cellLines.AppendLine(tmpLine.ToString());
                    }

                    StringBuilder rowLine = new StringBuilder(Resources.HtmlTableRowTemplate);
                    rowLine.Replace("#table_row_cells#", cellLines.ToString());
                    htmlTableRows.AppendLine(rowLine.ToString());
                }
                #endregion Rows Generation Code

                #region Formatting Html Code
                //Replacing the htmlHolders of the htmlTableTemplate with the new generated code
                htmlTable.Replace("#table_headers#", htmlTableHeaders.ToString());
                htmlTable.Replace("#table_rows#", htmlTableRows.ToString());
                #endregion Formatting Html Code

                #region Saving Html Code to the file
                //Open/Append/Create the file to write the html generated
                htmlFile = new StreamWriter(pathname, append);
                if (append)
                {
                    htmlFile.WriteLine();
                }
                htmlFile.Write(htmlTable.ToString());
                #endregion Saving Html Code to the file
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (htmlFile != null)
                {
                    htmlFile.Flush();
                    htmlFile.Close();
                    htmlFile = null;
                }
                htmlTable = null;
            }
        }

        /// <summary>
        /// Deletes the current row
        /// </summary>
        public void DeleteCurrentRow()
        {
            if (CurrentRow!=null && !CurrentRow.IsNewRow)
            {
                try
                {
                    Rows.Remove(CurrentRow);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Equals("RowDeleting-Cancel", StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw ex;
                    } 
                } 
            } 
        }

        /// <summary>
        /// Moves the current record to the first data record
        /// </summary>
        public void MoveFirst()
        {
            if (this.DataSource is BindingSource)
            {
                ((BindingSource)this.DataSource).MoveFirst();
            }
            else if (this.DataSource is ADODataControlHelper)
            {
                BindingSource source = ((ADODataControlHelper)this.DataSource).Source;
                source.MoveFirst();
            }
            else if (this.DataSource is RDODataControlHelper)
            {
                BindingSource source = ((RDODataControlHelper)this.DataSource).Source;
                source.MoveFirst();
            }
        }


        /// <summary>
        /// Moves to the next record 
        /// </summary>
        public void MoveNext()
        {
            if (this.DataSource is BindingSource)
            {
                ((BindingSource)this.DataSource).MoveNext();
            }
            else if (this.DataSource is ADODataControlHelper)
            {
                BindingSource source = ((ADODataControlHelper)this.DataSource).Source;
                source.MoveNext();
            }
            else if (this.DataSource is RDODataControlHelper)
            {
                BindingSource source = ((RDODataControlHelper)this.DataSource).Source;
                source.MoveNext();
            }
        }


        /// <summary>
        /// Moves to the last record 
        /// </summary>
        public void MoveLast()
        {
            if (this.DataSource is BindingSource)
            {
                ((BindingSource)this.DataSource).MoveLast();
            }
            else if (this.DataSource is ADODataControlHelper)
            {
                BindingSource source = ((ADODataControlHelper)this.DataSource).Source;
                source.MoveLast();
            }
            else if (this.DataSource is RDODataControlHelper)
            {
                BindingSource source = ((RDODataControlHelper)this.DataSource).Source;
                source.MoveLast();
            }
        }

        /// <summary>
        /// Repaints the current row
        /// </summary>
        public void RefreshCurrentRow()
        {
            if (!CurrentRow.IsNewRow)
                InvalidateRow(CurrentRow.Index);
        }

        /// <summary>
        /// Reconnects the grid to the data source
        /// </summary>
        public void ReOpen()
        {
            if (weakRefToBindingSource != null && weakRefToBindingSource.IsAlive)
            {
                this.DataSource = weakRefToBindingSource.Target;
                this.Enabled = true;
            }
            if (this.DataSource is BindingSource)
            {
                if (((BindingSource)this.DataSource).IsBindingSuspended)
                {
                    ((BindingSource)this.DataSource).ResumeBinding();
                }
                ((BindingSource)this.DataSource).ResetBindings(true);
            }
        }

        ///// <summary>
        ///// Gets/Sets the DataGridViewColumnCollection that stores the columns in the control
        ///// </summary>
        //public new DataGridViewColumnCollection Columns
        //{
        //    get
        //    {
        //        if (this.DataSource != null && base.DesignMode == true)
        //        {
        //            BindingSource source = ((RDODataControlHelper)this.DataSource).Source;
        //            source.SuspendBinding();
        //        }

        //        return base.Columns;
        //    }
        //    set
        //    {
        //        if (value != null && value.Count > 0)
        //        {
        //            base.Columns.Clear();

        //            DataGridViewColumn[] _columns = new DataGridViewColumn[value.Count];
        //            while (value.Count > 0)
        //            {
        //                _columns[value.Count - 1] = value[value.Count - 1];
        //                value.RemoveAt(value.Count - 1);
        //            }

        //            base.Columns.AddRange(_columns);
        //        }
        //    }
        //}

        /// <summary>
        /// Reconnects the grid to the bound data source. Resets the columns, headings and other properties
        /// based on the current data source
        /// </summary>
        public void ReBind()
        {
            if (weakRefToBindingSource != null && weakRefToBindingSource.IsAlive)
            {
                this.DataSource = weakRefToBindingSource.Target;
                this.Enabled = true;
            }
            else
                if (this.DataSource is BindingSource)
                {
                    if (((BindingSource)this.DataSource).IsBindingSuspended)
                    {
                        ((BindingSource)this.DataSource).ResumeBinding();
                    }
                    ((BindingSource)this.DataSource).ResetBindings(false);
                }
                else if (this.DataSource is ADODataControlHelper)
                {
                    BindingSource source = ((ADODataControlHelper)this.DataSource).Source;
                    if (source.IsBindingSuspended)
                    {
                        source.ResumeBinding();
                    }
                    source.ResetBindings(false);
                }
                else if (this.DataSource is RDODataControlHelper)
                {
                    BindingSource source = ((RDODataControlHelper)this.DataSource).Source;
                    if (source.IsBindingSuspended)
                    {
                        source.ResumeBinding();
                    }
                    source.ResetBindings(false);
                }
        }

        /// <summary>
        /// Repopulates the current row from the data source control and/or unbound events
        /// </summary>
        public void RefetchCurrentRow()
        {
            if (this.DataSource is BindingSource)
            {
                ((BindingSource)this.DataSource).ResetItem(((BindingSource)this.DataSource).Position);
            }
            else if (this.DataSource is ADODataControlHelper)
            {
                BindingSource source = ((ADODataControlHelper)this.DataSource).Source;
                source.ResetItem(((BindingSource)this.DataSource).Position);
            }
            else if (this.DataSource is RDODataControlHelper)
            {
                BindingSource source = ((RDODataControlHelper)this.DataSource).Source;
                source.ResetItem(((BindingSource)this.DataSource).Position);
            }
        }

        /// <summary>
        /// Returns the index of the row of the specified coordinate value
        /// </summary>
        /// <param name="coordinate">defines the coordinate value</param>
        /// <returns>The index of the row</returns>
        public int RowContaining(int coordinate)
        {
            int rowIndex = -1;
            int rowTop = ColumnHeadersVisible == false ? Top : Top + ColumnHeadersHeight;

            DataGridViewElementStates displayOptions = DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible;
            for (rowIndex = Rows.GetFirstRow(displayOptions); rowIndex != -1; rowIndex = Rows.GetNextRow(rowIndex, displayOptions, DataGridViewElementStates.None))
            {
                int rowBottom = rowTop + Rows[rowIndex].Height;
                if ((coordinate >= rowTop) && (coordinate <= rowBottom))
                {
                    break;
                }
                else
                {
                    rowTop = rowBottom;
                }
            }
            return rowIndex;
        }

        /// <summary>
        /// Forces the data of the current row to be updated to the database
        /// </summary>
        public void UpdateCurrentRow()
        {
            if (this.DataSource is ADODataControlHelper)
            {
                ((ADODataControlHelper)this.DataSource).Recordset.Update();
            }
            else if (this.DataSource is RDODataControlHelper)
            {
                ((RDODataControlHelper)this.DataSource).Recordset.Update();
            }
        }

        #endregion

        #region Component Designer generated code

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

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.CellMouseEnter -= new DataGridViewCellEventHandler(ExtendedDataGridView_CellMouseEnter);
            this.CellMouseEnter += new DataGridViewCellEventHandler(ExtendedDataGridView_CellMouseEnter);
            this.RowHeaderMouseClick -= new DataGridViewCellMouseEventHandler(ExtendedDataGridView_RowHeaderMouseClick);
            this.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(ExtendedDataGridView_RowHeaderMouseClick);
        }


        internal int mouse_cell_row = -1;
        internal int mouse_cell_column = -1;


        void ExtendedDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            mouse_cell_row = e.RowIndex + (ColumnHeadersVisible ? 1 : 0);
            mouse_cell_column = e.ColumnIndex + (RowHeadersVisible ? 1 : 0);

        }
  

        #endregion
    }
}
