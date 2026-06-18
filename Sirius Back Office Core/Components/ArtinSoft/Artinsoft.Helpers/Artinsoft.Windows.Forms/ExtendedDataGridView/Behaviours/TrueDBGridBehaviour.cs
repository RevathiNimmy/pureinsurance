using System.Windows.Forms;
using System;
using System.Data;
using System.ComponentModel;
using Artinsoft.VB6.DB.ADO;
using Artinsoft.VB6.DB.RDO;

namespace Artinsoft.Windows.Forms
{
	/// <summary>
	/// Interface for the TrueDBGridBehaviour.
	/// </summary>
    public interface ITrueDBGridBehaviour
    {



    }

	/// <summary>
	/// Event args that allow the user to cancel cell value updating.
	/// </summary>
	public class DataGridViewCellValueCancelEventArgs : DataGridViewCellValueEventArgs
	{
		bool _Cancel;

		/// <summary>
		/// Gets/sets if the the cell value change should be cancelled.
		/// </summary>
		public bool Cancel
		{
			get { return _Cancel; }
			set { _Cancel = value; }
		}
		/// <summary>
		/// Creates a DataGridViewCellValueCancelEventArgs class with a specified column and row.
		/// </summary>
		/// <param name="col">The row which is being updated.</param>
		/// <param name="row">The column which is being updated.</param>
		public DataGridViewCellValueCancelEventArgs(int col, int row) : base(col, row) { }
		/// <summary>
		/// Creates a DataGridViewCellValueCancelEventArgs class with a specified column and row, and the value that is being set.
		/// </summary>
		/// <param name="col">The row which is being updated.</param>
		/// <param name="row">The column which is being updated.</param>
		/// <param name="value">The proposed value for the cell.</param>
		public DataGridViewCellValueCancelEventArgs(int col, int row, object value) : base(col, row) { this.Value = value; }
	}

	/// <summary>
	/// Delegate to handle ExtendedDataGridView cell updating events.
	/// </summary>
	/// <param name="sender">The sender of the event.</param>
	/// <param name="e">The event arguments.</param>
	public delegate void DataGridViewCellValueCancelEventHandler(object sender, DataGridViewCellValueCancelEventArgs e);

	/// <summary>
	/// This class represents the behaviour of a TrueDBGrid to be used with an ExtendedDataGridView.
	/// </summary>
	public class TrueDBGridBehaviour : StandardDataGridViewBehaviour, ITrueDBGridBehaviour
    {

		/// <summary>
		/// Creates a TrueDBGridBehaviour for a specified grid, and copies the behaviour from another behaviour.
		/// </summary>
		/// <param name="grid">The grid for which to create the behaviour.</param>
		/// <param name="currentBehaviour">The behaviour from which to copy properties.</param>
        public TrueDBGridBehaviour(ExtendedDataGridView grid, IGridBehaviour currentBehaviour)
            : base(grid,currentBehaviour)
        {
            InitializeTDBGEventHandlers();
        }

        private DataColumnChangeEventHandler _DataColumnChangedHandler;
        private DataColumnChangeEventHandler _DataColumnChangingHandler;
        private DataRowChangeEventHandler _RowDeletedHandler;
        private DataRowChangeEventHandler _RowDeletingHandler;

        //private DataTableNewRowEventHandler _RowAddedHandler;

        private DataTable _CurrentDataTable;

        internal void InitializeTDBGEventHandlers()
        {
            _DataColumnChangedHandler = new DataColumnChangeEventHandler(_CurrentDataTable_ColumnChanged);
            _DataColumnChangingHandler = new DataColumnChangeEventHandler(_CurrentDataTable_ColumnChanging);

            _RowDeletedHandler = new DataRowChangeEventHandler(_CurrentDataTable_RowDeleted);
            _RowDeletingHandler = new DataRowChangeEventHandler(_CurrentDataTable_RowDeleting);

            //_RowAddedHandler = new DataTableNewRowEventHandler(_CurrentDataTable_RowAdded);
        }

        void _CurrentDataTable_RowAdded(object sender, DataTableNewRowEventArgs e)
        {
            grid.RaiseRowAddedEvent(sender, e);
        }

        void _CurrentDataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            grid.RaiseRowDeletedEvent(sender, e);
        }

        void _CurrentDataTable_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            grid.RaiseRowDeletingEvent(sender, e);
        }

        void _CurrentDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            int col = _CurrentDataTable.Columns.IndexOf(e.Column);
            int row = _CurrentDataTable.Rows.IndexOf(e.Row);
            if (row != -1)
            {
                DataGridViewCellValueEventArgs args = new DataGridViewCellValueEventArgs(col, row);
                args.Value = e.ProposedValue;
                grid.RaiseCellUpdatedEvent(sender, args);
            }
        }

        void _CurrentDataTable_ColumnChanging(object sender, DataColumnChangeEventArgs e)
        {
            int col = _CurrentDataTable.Columns.IndexOf(e.Column);
            int row = _CurrentDataTable.Rows.IndexOf(e.Row);
            if (row != -1)
            {
				DataGridViewCellValueCancelEventArgs args = new DataGridViewCellValueCancelEventArgs(col, row, e.ProposedValue);
				grid.RaiseCellUpdatingEvent(sender, args);
				if (args.Cancel)
				{
					e.ProposedValue = _CurrentDataTable.Rows[row].ItemArray[col];
				}
			}
        }

        private void DetachEventsFromDataTable(DataTable table)
        {
            table.ColumnChanged -= _DataColumnChangedHandler;
            table.ColumnChanging -= _DataColumnChangingHandler;

            table.RowDeleted -= _RowDeletedHandler;
            table.RowDeleting -= _RowDeletingHandler;

            table.TableNewRow -= _CurrentDataTable_RowAdded;

            attachedEventsToDataTable = false;
        }

        private bool attachedEventsToDataTable;

        private void AttachEventsToDataTable(DataTable table)
        {
            if (_CurrentDataTable != null)
            {
                DetachEventsFromDataTable(_CurrentDataTable);
            }
            _CurrentDataTable = table;

            attachedEventsToDataTable = true;

            table.ColumnChanged -= _DataColumnChangedHandler;
                table.ColumnChanged += _DataColumnChangedHandler;

            table.ColumnChanging -= _DataColumnChangingHandler;
            table.ColumnChanging += _DataColumnChangingHandler;
            
            table.RowDeleted -= _RowDeletedHandler;
            table.RowDeleted += _RowDeletedHandler;

            table.RowDeleting -= _RowDeletingHandler;
            table.RowDeleting += _RowDeletingHandler;

            table.TableNewRow -= _CurrentDataTable_RowAdded;
            table.TableNewRow += _CurrentDataTable_RowAdded;

            attachedEventsToDataTable = true;
        }


        /// <summary>
        /// It resets the grid to the default values
        /// </summary>
        /// <param name="previous">The previous behaviour used</param>
        public override void Reset(IGridBehaviour previous)
        {
            grid.EditMode = DataGridViewEditMode.EditOnEnter;        
        }

        private void ReloadColumns()
        {
            AttachEventsToInternalDataTable();
        }

        private void AttachEventsToInternalDataTable()
        {
            DataTable table = GetInternalDataTable();
            if (table != null)
            {
                AttachEventsToDataTable(table);
            }
        }

        private DataTable GetInternalDataTable()
        {
            BindingSource binding = grid.DataSource as BindingSource;
            DataTable table = null;
            if (binding != null)
            {
                DataSet dataSet = binding.DataSource as DataSet;
                if (dataSet != null && !string.IsNullOrEmpty(binding.DataMember))
                {
                    table = dataSet.Tables[binding.DataMember];
                }
                if (dataSet == null)
                {
                    // Case for DBArray
                    DataTable internalTable = binding.DataSource as DataTable;
                    if (internalTable != null)
                    {
                        table = internalTable;
                    }
                }
            }
            if (table == null)
            {
                ADODataControlHelper adoHelper = grid.DataSource as ADODataControlHelper;                
                if (adoHelper != null && adoHelper.Recordset != null && adoHelper.Recordset.Tables.Count > 0)
                {
                    table = adoHelper.Recordset.Tables[0];
                }
                if (adoHelper == null)
                {
                    RDODataControlHelper rdoHelper = grid.DataSource as RDODataControlHelper;
                    if (rdoHelper != null && rdoHelper.Recordset != null && rdoHelper.Recordset.Tables.Count > 0)
                    {
                        table = rdoHelper.Recordset.Tables[0];
                    }
                }
            }
            return table;
        }

        /// <summary>
        /// When the component has finished initializing
        /// it attaches the events to the data table internal
        /// </summary>
        public override void EndInit()
        {
            //We must set the datasource at the end
            if (!attachedEventsToDataTable)
            {
                AttachEventsToInternalDataTable();
            }
        }

		/// <summary>
		/// Called when the DataMember value changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		public override void OnDataMemberChanged(EventArgs e)
        {
            if (!IsInitializing)
            {
                ReloadColumns();
            }
        }

		/// <summary>
		/// Called when the DataSource changes.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		public override void OnDataSourceChanged(EventArgs e)
        {
           if (!IsInitializing)
            {
                ReloadColumns();
            }
        }
        /// <summary>
        /// Provides a DataSource property customized for a TDBGrid behaviour
        /// </summary>
        public override object DataSource
        {
            get
            {
                return grid.BaseDataSource;
            }
            set
            {
                BindingSource bd = value as BindingSource;
                if (bd != null)
                {
                    string xarrayType = bd.DataSource.GetType().ToString();
                    //Avoiding to add a reference to the dll containing the type
                    if (xarrayType == "Artinsoft.VB6.Utils.XArrayHelper")
                    {
                        DataTable xarray = bd.DataSource as DataTable;
                        if (xarray != null && grid.Columns.Count > 0)
                        {
                            //Bound Columns
                            int index = 0;
                            foreach (DataColumn c in xarray.Columns)
                            {
                                if (index < grid.Columns.Count)
                                {
                                    grid.Columns[index].DataPropertyName = c.ColumnName;
                                }
                                else
                                    break;
                                index++;
                            }
                        }
                    }
                }
                grid.BaseDataSource = value;
            }
        }

    }
}