using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Artinsoft.Windows.Forms
{
    /// <summary>
    /// The DataGridViewDropDown class represents a DropDown control which can be used inside
    /// a Grid control to display a table filled with data.
    /// It is the equivalent of a TDBDropDown.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    [Designer(typeof(DataGridViewDropDownDesigner))]
    public class DataGridViewDropDown : DataGridView
    {
        /// <summary>
        /// Default constructor with no parameters.
        /// Initializes a DataGridViewDropDown with RowHeadersVisible, Visible, AllowUserToAddRows
        /// and AllowUserToDeleteRows set to False
        /// </summary>
        public DataGridViewDropDown()
            : base()
        {
            RowHeadersVisible = false;
            Visible = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
        }

        String _DataField;

        /// <summary>
        /// Sets/Gets the field that will be used to determine the column to used to return a value from the
        /// DropDownList
        /// </summary>
        public String DataField
        {
            get { return _DataField; }
            set { _DataField = value; }
        }

        /// <summary>
        /// Needed to allow the DataGridViewDropDown to handle Enter and Escape key strokes in a custom manner.     
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    {
                        return true;
                    }
                case Keys.Escape:
                    {
                        return true;
                    }
                default:
                    return base.IsInputKey(keyData);
            }
        }

        /// <summary>
        /// Returns the column index for the current DataField
        /// </summary>
        /// <returns></returns>
        public int GetDataFieldColumnIndex()
        {
            if (DataSource is ITypedList)
            {
                if (String.IsNullOrEmpty(DataField))
                {
                    return 0;
                }
                else
                {
                    PropertyDescriptorCollection columnInfo = ((ITypedList)DataSource).GetItemProperties(null);
                    if (columnInfo != null)
                    {
                        int columnIndex = 0;
                        foreach (PropertyDescriptor columnData in columnInfo)
                        {
                            if (String.Compare(DataField, columnData.Name, true) == 0)
                            {
                                return columnIndex;
                            }
                            columnIndex++;
                        } // foreach
                    }
                }
            } // if
            return 0;
        }


        /// <summary>
        /// Returns the currently selected value in this dropdown
        /// </summary>
        public object SelectedValue
        {
            set
            {
                int columnIndex = GetDataFieldColumnIndex();
                foreach (DataGridViewRow row in this.Rows)
                {
                    if (object.Equals(row.Cells[columnIndex].Value,value))
                    {
                        this.CurrentCell = this[columnIndex, row.Index];
                    }
                }
            }
            get 
            {
                if (CurrentRow == null)
                    return null;
                if (DataSource is System.ComponentModel.ITypedList)
                {
                    int columnIndex = GetDataFieldColumnIndex();
                    return this[columnIndex, CurrentRow.Index].Value;
                } // if
                return null;
            }
        }
    }

    internal class DataGridViewDropDownDesigner : ParentControlDesigner
    {
        protected override void PreFilterProperties(System.Collections.IDictionary properties)
        {
            base.PreFilterProperties(properties);
            properties.Remove("AllowUserToAddRows");
            properties.Remove("AllowUserToDeleteRows");
            properties.Remove("RowHeadersVisible");
            properties.Remove("RowTemplate");
            properties.Remove("SelectionMode");
            properties.Remove("VirtualMode");
            properties.Remove("Visible");
        }

        protected override void PreFilterEvents(System.Collections.IDictionary events)
        {
            base.PreFilterEvents(events);
            events.Remove("AllowUserToAddRowsChanged");
            events.Remove("AllowUserToDeleteRowsChanged");
            events.Remove("VisibleChanged");
        }
  
    }


}
