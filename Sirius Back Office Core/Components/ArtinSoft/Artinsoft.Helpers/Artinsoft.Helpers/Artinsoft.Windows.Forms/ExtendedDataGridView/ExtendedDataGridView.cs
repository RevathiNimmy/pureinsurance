// Author: mrojas
// Project: Artinsoft.Windows.Forms
// Path: D:\VbcSPP\src\Helpers\Artinsoft.Windows.Forms\ExtendedDataGridView
// Creation date: 8/6/2009 2:29 PM
// Last modified: 10/8/2009 10:32 AM

#region Using directives
using Artinsoft.Windows.Forms.Properties;
using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
#endregion

namespace Artinsoft.Windows.Forms
{
    /// <summary>
    /// This is class implements a component that extends the
    /// System.Windows.Forms.DataGridView control.  It adds new properties and also
    /// provides &quot;Compatibility&quot; support for some Grid controls commonly used
    /// in  VB6: MSFlexGrid and APEX TrueDBGrid
    /// </summary>
    partial class ExtendedDataGridView
    {

        /// <summary>
        /// Initializes a new instance of the Artinsoft.Windows.Forms.ExtendedGridView class with the corresponding container
        /// </summary>
        /// <param name="container">The container where the Grid is going to be hosted</param>
        public ExtendedDataGridView(IContainer container)
        {
            InitializeComponent();

            //At this point compatibility is DataGridView
            //this is to make sure that we have a valid currentbehaviour
            //and the DataGridView is the less complicated
            SetupCompatibility();


            _controlKeyDown = new KeyEventHandler(control_KeyDown);
            _controlKeyUp = new KeyEventHandler(control_KeyUp);
            _controlKeyPress = new KeyPressEventHandler(control_KeyPress);
            _EditingControlShowing = new DataGridViewEditingControlShowingEventHandler(ExtendedDataGridView_EditingControlShowing);
            EditingControlShowing -= _EditingControlShowing;
            EditingControlShowing += _EditingControlShowing;
            this.CellFormatting += new DataGridViewCellFormattingEventHandler(ExtendedDataGridView_CellFormatting);
            #region Designer related code

            IServiceContainer serviceContainer = container as IServiceContainer;
            if (serviceContainer != null)
            {
                ExtendedDataGridViewPropertyFilter newMyFilter = new ExtendedDataGridViewPropertyFilter();
                DesignerActionService designerActionService = serviceContainer.GetService(typeof(DesignerActionService)) as DesignerActionService;
                //DesignerActionUIService designerActionUIService = serviceContainer.GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
                newMyFilter.oldService = (ITypeDescriptorFilterService)serviceContainer.GetService(typeof(ITypeDescriptorFilterService));
                newMyFilter.designerActionService = designerActionService;
                //newMyFilter.designerActionUIService = designerActionUIService;
                if (newMyFilter.oldService != null)
                {
                    serviceContainer.RemoveService(typeof(ITypeDescriptorFilterService));
                }

                serviceContainer.AddService(typeof(ITypeDescriptorFilterService), newMyFilter);
            }

            // Acquire a reference to IComponentChangeService
            //This service is used during design to make sure that we do not allow the user
            //to edit Columns properties when the grid is in MSFlexGrid compatibility
            IComponentChangeService changeService = container as IComponentChangeService;
            if (changeService != null)
            {
                changeEventHandler = new ComponentChangingEventHandler(changeService_ComponentChanging);
                changeService.ComponentChanging -= changeEventHandler;
                changeService.ComponentChanging += changeEventHandler;
            }
            #endregion
        }

        void ExtendedDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Debug.WriteLine("aca estoy (" + e.RowIndex + "," + e.ColumnIndex + ") [" +  e.Value + "]" );
        }

        void ExtendedDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control != null)
            {
                DataGridExtendedEditingControl dataGridExtendedEditingControl = e.Control as DataGridExtendedEditingControl;
                if (dataGridExtendedEditingControl!=null)
                {
                    AttachKeyEventsToControl(dataGridExtendedEditingControl._TextBox);
                }
                else
                    AttachKeyEventsToControl(e.Control);
            } 
        }

        /// <summary>
        /// Base on selection changed
        /// </summary>
        internal void BaseOnSelectionChanged(EventArgs e)
        {
            base.OnSelectionChanged(e);
        } // BaseOnSelectionChanged()

        /// <summary>
        /// Overrides base OnSelectionChanged and delegates the handling of the event
        /// down onto the specific grid behaviour implementation.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectionChanged(EventArgs e)
        {
            currentBehaviour.OnSelectionChanged(e);
        }


        class ExtendedDataGridViewPropertyFilter : ITypeDescriptorFilterService
        {

            internal ITypeDescriptorFilterService oldService;
            internal DesignerActionService designerActionService;
            //internal DesignerActionUIService designerActionUIService;
            DesignerActionList columnEditing;
            bool columnEditingRemoved;

            #region ITypeDescriptorFilterService Members

            public bool FilterAttributes(IComponent component, System.Collections.IDictionary attributes)
            {
                if (oldService != null)
                    oldService.FilterAttributes(component, attributes);
                return true;
            }

            public bool FilterEvents(IComponent component, System.Collections.IDictionary events)
            {
                if (oldService != null)
                    oldService.FilterEvents(component, events);
                return true;
            }

            public bool FilterProperties(IComponent component, System.Collections.IDictionary properties)
            {
                ExtendedDataGridView grid = component as ExtendedDataGridView;
                if (grid != null)
                {
                    //Initialize ColumnEditing actions
                    CacheColumnEditingActionList(component);
                    if (!grid.isInitializing)
                    {
                        if (grid.Compatibility == GridCompatibility.DataGridView)
                            SetPropertiesForDataGridView(component, properties);
                        else if (grid.Compatibility == GridCompatibility.MSFlexGrid)
                            SetPropertiesForFlexGrid(component, properties);
                        //else if (grid.Compatibility == GridCompatibility.TrueDBGrid)
                        //    SetPropertiesForTrueDBGrid(component, properties);
                    }
                    return false;
                }
                else
                    if (oldService != null)
                        return oldService.FilterProperties(component, properties);
                    else
                        return true;
            }
            #endregion

            //private void SetPropertiesForTrueDBGrid(IComponent component, System.Collections.IDictionary properties)
            //{
            //}

            private void SetPropertiesForFlexGrid(IComponent component, System.Collections.IDictionary properties)
            {
                foreach (PropertyInfo propertyInfo in typeof(ITrueDBGridBehaviour).GetProperties())
                {
                    properties.Remove(propertyInfo.Name);
                }
                properties.Remove("Columns");
                if (designerActionService != null && columnEditing != null && !columnEditingRemoved)
                {
                    designerActionService.Remove(component, columnEditing);
                    columnEditingRemoved = true;
                }
            }
            private void SetPropertiesForDataGridView(IComponent component, System.Collections.IDictionary properties)
            {
                foreach (PropertyInfo propertyInfo in typeof(IFlexGridBehaviour).GetProperties())
                {
                    properties.Remove(propertyInfo.Name);
                }
                foreach (PropertyInfo propertyInfo in typeof(ITrueDBGridBehaviour).GetProperties())
                {
                    properties.Remove(propertyInfo.Name);
                }
               if (designerActionService != null && columnEditing != null && columnEditingRemoved)
                {
                    designerActionService.Add(component, columnEditing);
                    columnEditingRemoved = false;

                }
            }
            private void CacheColumnEditingActionList(IComponent component)
            {
                if (designerActionService != null && columnEditing == null)
                {
                    try
                    {
                        DesignerActionListCollection designerActionList = designerActionService.GetComponentActions(component, ComponentActionsType.Component);
                        foreach (System.ComponentModel.Design.DesignerActionList dList in designerActionList)
                        {
                            if (dList.GetType().Name.Equals("DataGridViewColumnEditingActionList"))
                            {
                                this.columnEditing = dList;
                                break;
                            }
                        }
                    }
                    catch { }
                }
            }

        }

        /// <summary>
        /// ExtendedDataGridView destructor.
        /// Removes associations between the ComponentChanging event and its handler
        /// when the control is in design mode.
        /// </summary>
        ~ExtendedDataGridView()
        {
            if (DesignMode)
            {
                IComponentChangeService changeService =
                    GetService(typeof(IComponentChangeService))
                    as IComponentChangeService;
                if (changeService != null)
                    changeService.ComponentChanging -= changeEventHandler;

            }
        }

        /// <summary>
        /// Change handler used in the designer to intercept changes in certains properties
        /// </summary>
        private ComponentChangingEventHandler changeEventHandler;



        /// <summary>
        /// Determines the total number of columns or rows in a FlexGrid.
        /// </summary>
        [Description("Set or gets the total number of columns"),
        Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ColumnsCount
        {
            get
            {
                return currentBehaviour.ColumnsCount;
            }
            set
            {
                currentBehaviour.ColumnsCount = value;
            }
        }

		/// <summary>
        /// Sets/Gets the index for the Column that currently has the focus
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentColumnIndex
        {
            get
            {
                return currentBehaviour.CurrentColumnIndex;
            }
            set
            {
                currentBehaviour.CurrentColumnIndex = value;
            }
        }



        /// <summary>
        /// Sets/Gets the index for the Row that currently has the focus
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentRowIndex
        {
            get
            {
                return currentBehaviour.CurrentRowIndex;
            }
            set
            {
                currentBehaviour.CurrentRowIndex = value;
            }
        }


        /// <summary>
        /// Overrides base FirstDisplayedScrollingColumnIndex and delegates the handling of the 
        /// property down onto the specific grid behaviour implementation.
        /// </summary>
        public new int FirstDisplayedScrollingColumnIndex
        {
            get
            {
                return currentBehaviour.FirstDisplayedScrollingColumnIndex;
            }
            set
            {
                currentBehaviour.FirstDisplayedScrollingColumnIndex = value;
            }
        }

        /// <summary>
        /// Overrides base FirstDisplayedScrollingRowIndex and delegates the handling of the 
        /// property down onto the specific grid behaviour implementation.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int FirstDisplayedScrollingRowIndex
        {
            get
            {
                return currentBehaviour.FirstDisplayedScrollingRowIndex;
            }
            set
            {
                currentBehaviour.FirstDisplayedScrollingRowIndex = value;
            }
        }


        /// <summary>
        /// Returns/sets the width in Pixels of the gridlines for the control.
        /// </summary>
        [Description("Returns/sets the width in Pixels of the gridlines for the control."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int GridLineWidth
        {
            get { return currentBehaviour.GridLineWidth; }
            set
            {
                currentBehaviour.GridLineWidth = value;
            }
        }


        /// <summary>
        /// Returns/sets over which row the mouse pointer is. Not available at design time.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MouseRow
        {
            get { return currentBehaviour.MouseRow; }
        }

        /// <summary>
        /// Returns/sets over which column the mouse pointer is. Not available at design time.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MouseCol
        {
            get { return currentBehaviour.MouseCol; }
        }

        /// <summary>
        /// Returns/sets a minimum row height for the entire control
        /// </summary>
        [Description("Returns/sets a minimum row height for the entire control"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int RowHeightMin
        {
            get { return currentBehaviour.RowHeightMin; }
            set
            {
                currentBehaviour.RowHeightMin = value;
            }
        }

        /// <summary>
        /// Gets/Sets the current amount of rows based on the current grid's behaviour
        /// </summary>
        public int RowsCount
        {
            get
            {
                return currentBehaviour.RowsCount;
            }
            set
            {
                currentBehaviour.RowsCount = value;
            }
        }

        /// <summary>
        /// This overload is maded to provide another indexer that 
        /// will eliminate the need for generating narrowing operator in expressions like
        /// grid(10 / ColumnsCount,10 mod ColumnsCount)
        /// </summary>
        /// <param name="rowindex"></param>
        /// <param name="columnindex"></param>
        /// <returns></returns>
        public DataGridViewCell this[Double rowindex, Double columnindex]
        {
            get
            {
                return currentBehaviour[(int)columnindex, (int)rowindex];
            }
            set
            {
                currentBehaviour[(int)columnindex, (int)rowindex] = value;
            }
        }

        /// <summary>
        /// Overrides base 'this' and delegates the handling of the 
        /// property down onto the specific grid behaviour implementation.
        /// </summary>
        public new DataGridViewCell this[int rowindex, int columnindex]
        {
            get
            {
                return currentBehaviour[columnindex, rowindex];
            }
            set
            {
                currentBehaviour[columnindex, rowindex] = value;
            }
        }



        /// <summary>
        /// Returns/sets whether a FlexGrid should allow regular cell selection, selection by rows, or selection by columns.
        /// </summary>
        [Description("Returns/sets whether a FlexGrid should allow regular cell selection, selection by rows, or selection by columns."), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new DataGridViewSelectionMode SelectionMode
        {
            get
            {
                return currentBehaviour.SelectionMode;
            }
            set
            {
                currentBehaviour.SelectionMode = value;
            }
        }



        /// <summary>
        /// Overrides base ClearSelection and delegates the method
        /// down onto the specific grid behaviour implementation.
        /// </summary>
        public new void ClearSelection()
        {
            currentBehaviour.ClearSelection();
        }

        internal void BaseClearSelection()
        {
            base.ClearSelection();
        }

        internal void BaseOnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDown(e);
        }

        /// <summary>
        /// Allows access to the base data source
        /// </summary>
        internal object BaseDataSource
        {
            get
            {
                return base.DataSource;
            } // get
            set
            {
                base.DataSource = value;
            } // set
        } // BaseDataSource


        /// <summary>
        /// Allows access to the base data source
        /// </summary>
        public new object DataSource
        {
            get
            {
                return currentBehaviour.DataSource;
            } // get
            set
            {
                currentBehaviour.DataSource = value;
            } // set
        } // BaseDataSource


        #region ISupportInitialize Members and Behaviour Switching Management

        GridCompatibility _compatibility = GridCompatibility.DataGridView;
        GridCompatibility tempCompatibility = GridCompatibility.DataGridView;
        /// <summary>
        /// The compatibility used for this grid
        /// </summary>
        [Category("Design"), Description("The compatibility used for this grid"), RefreshProperties(RefreshProperties.All)]
        public GridCompatibility Compatibility
        {
            get
            {
                if (isInitializing)
                {
                    return tempCompatibility;
                }
                else
                    return _compatibility;
            }
            set
            {
                if (isInitializing)
                {
                    tempCompatibility = value;
                }
                else
                {
                    if (_compatibility != value || currentBehaviour is ValueHolderBehaviour)
                    {
                        _compatibility = value;
                        SetupCompatibility();
                    }
                }
            }

        }


        private bool isInitializing;

        internal IGridBehaviour currentBehaviour;

        private void SetupCompatibility()
        {
            switch (Compatibility)
            {
                case GridCompatibility.DataGridView:
                    currentBehaviour = new StandardDataGridViewBehaviour(this, currentBehaviour);
                    break;
                case GridCompatibility.MSFlexGrid:
                    currentBehaviour = new FlexGridBehaviour(this, currentBehaviour);
                    break;
                case GridCompatibility.TrueDBGrid:
                    currentBehaviour = new TrueDBGridBehaviour(this, currentBehaviour);
                    break;
            }
        }



        /// <summary>
        /// Implements the ISupportInitialize.BeginInit method.
        /// It sets up a temporal ValueHolderBehavior during the component initialization to 
        /// hold values until the EndInit is called.
        /// </summary>
        public virtual void BeginInit()
        {
            isInitializing = true;
            currentBehaviour.IsInitializing = true;
            currentBehaviour = new ValueHolderBehaviour();
            currentBehaviour.BeginInit();
        }
        /// <summary>
        /// Implements the ISupportInitialize.EndInit method.
        /// It sets the grid behavior according the Compatibility mode and delegates EndInit logic to the behaviour.
        /// </summary>
        public virtual void EndInit()
        {
            Debug.Assert(currentBehaviour != null, "BEHAVIOUR IS UNSET");
            isInitializing = false;
            Compatibility = tempCompatibility;
            currentBehaviour.EndInit();
            backupNormal = RowsDefaultCellStyle.Clone();
            backupAlternating = AlternatingRowsDefaultCellStyle.Clone();
            AlternatingRows = _alternatingRows;
        }

        #endregion


        bool allowRowSelection;

        /// <summary>
        /// If set selects the full row when the user clicks the row header
        /// </summary>
        [Browsable(true),DefaultValue(true),Description("If set selects the full row when the user clicks the row header")]
        public bool AllowRowSelection
        {
            get { return allowRowSelection; }
            set { allowRowSelection = value; }
        }

        void ExtendedDataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (AllowRowSelection)
            {
                ClearSelection();
                foreach (DataGridViewColumn col in Columns)
                {
                    if (col.Visible)
                    {
                        CurrentCell = this[e.RowIndex, col.Index];
                        break;
                    } 
                    else
                        continue;
                    
                } // foreach
                DataGridViewRow currentRow = Rows[e.RowIndex];
                foreach (DataGridViewCell cell in currentRow.Cells)
                {
                    cell.Selected = true;
                }
            }
        }

        /// <summary>
        /// Returns the left position of the current cell
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellLeft
        {
            get
            {
                return currentBehaviour.CellLeft;
            }
        }

        /// <summary>
        /// Returns the height of the current cell.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellHeight
        {
            get
            {
                return currentBehaviour.CellHeight;
            }
        }



        /// <summary>
        /// Returns or sets the top position of the current cell
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellTop
        {
            get
            {
                return currentBehaviour.CellTop;
            }
        }

        /// <summary>
        /// Returns the width of the current cell
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellWidth
        {
            get
            {
                return currentBehaviour.CellWidth;
            }
        }

        /// <summary>
        /// Returns/sets an image to be displayed in the currently selected cells
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image CellPicture
        {
            get
            {
                return currentBehaviour.CellPicture;
            }
            set
            {
                currentBehaviour.CellPicture = value;
            }
        }

        internal void BaseOnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);
        }

        DataGridViewCellStyle _oddStyle = null;
        /// <summary>
        /// Provides an style that will be used for odd rows (1,3,5,...) when the
        /// AlternatingRows property is true
        /// </summary>
        [Category("Appearance")]
        [Description("Provides an style that will be used for odd rows (1,3,5,...) when the AlternatingRows property is true")]
        public DataGridViewCellStyle OddStyle
        {
            get
            {
                if (_oddStyle == null)
                {
                    _oddStyle = AlternatingRowsDefaultCellStyle.Clone();
                } // if
                return _oddStyle;

            }
            set
            {
                if (_oddStyle == null || !_oddStyle.Equals(value))
                {
                    _oddStyle = value;
                    if (!isInitializing && _alternatingRows)
                    {
                        AlternatingRowsDefaultCellStyle = value;
                    } // if
                } // if

            }
        }

        DataGridViewCellStyle _evenStyle = null;
        /// <summary>
        /// Provides an style that will be used for even rows (0,2,4,...) when the
        /// AlternatingRows property is true
        /// </summary>
        [Category("Appearance")]
        [Description("Provides an style that will be used for even rows (0,2,4,...) when the AlternatingRows property is true")]
        public DataGridViewCellStyle EvenStyle
        {
            get
            {
                if (_evenStyle == null)
                {
                    _evenStyle = RowsDefaultCellStyle.Clone();
                } // if
                return _evenStyle;
            }
            set
            {
                if (_evenStyle == null || !_evenStyle.Equals(value))
                {
                    _evenStyle = value;
                    if (!isInitializing && _alternatingRows)
                    {
                        RowsDefaultCellStyle = value;
                    } // if
                } // if            
            }
        }

        DataGridViewCellStyle backupNormal = new DataGridViewCellStyle();
        DataGridViewCellStyle backupAlternating = new DataGridViewCellStyle();
        bool _alternatingRows;
        /// <summary>
        /// If true EvenStyle and OddStyle will be used to format the cells in the grid
        /// </summary>
        [Category("Appearance")]
        [Description("If true EvenStyle and OddStyle will be used to format the cells in the grid")]
        public bool AlternatingRows
        {
            get { return _alternatingRows; }
            set
            {
                _alternatingRows = value;
                if (!isInitializing)
                {
                    if (_alternatingRows)
                    {
                        backupNormal = RowsDefaultCellStyle.Clone();
                        backupAlternating = AlternatingRowsDefaultCellStyle.Clone();
                        RowsDefaultCellStyle = EvenStyle;
                        AlternatingRowsDefaultCellStyle = OddStyle;
                    } // if
                    else
                    {
                        RowsDefaultCellStyle = backupNormal;
                        AlternatingRowsDefaultCellStyle = backupAlternating;
                    } // else
                } // if
            }
        }

        /// <summary>
        /// Processes the Up and Down Keys to trigger KeyUp and KeyDown handlers
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
//            const int WM_KEYUP = 0x101;
            KeyEventArgs keyevent;
            if (msg.Msg == WM_KEYDOWN)
            {
                keyevent = GetKeyEventArgs(keyData);
                if (keyevent != null)
                {
                    foreach (KeyEventHandler handler in keydownevents)
                    {
                        handler.Invoke(this, keyevent);
                    } // foreach
                    if (keyevent.Handled)
                        return true;
                    if (keyevent != null)
                    {
                        foreach (KeyEventHandler handler in keyupevents)
                        {
                            handler.Invoke(this, keyevent);
                        } // foreach
                        if (keyevent.Handled)
                            return true;
                    } // if
                } // if
            }
            return base.ProcessCmdKey(ref msg, keyData);

        }

        private static KeyEventArgs GetKeyEventArgs(Keys keyData)
        {
            KeyEventArgs keyevent = null;
            switch (keyData)
            {
                case Keys.Tab:
                    keyevent = new KeyEventArgs(Keys.Tab);
                    break;
                case Keys.Down:
                    keyevent = new KeyEventArgs(Keys.Down);
                    break;
                case Keys.Up:
                    keyevent = new KeyEventArgs(Keys.Up);
                    break;

            }
            return keyevent;
        }


        KeyEventHandler _controlKeyDown;
        KeyEventHandler _controlKeyUp;
        KeyPressEventHandler _controlKeyPress;
        DataGridViewEditingControlShowingEventHandler _EditingControlShowing;


        List<KeyEventHandler> keydownevents = new List<KeyEventHandler>();


        /// <summary>
        /// Hides DataGridView KeyDown Implementation to provide a functionality closer to the
        /// KeyDown event in VB6
        /// </summary>
        public new event KeyEventHandler KeyDown
        {
            add
            {
                keydownevents.Add(value);
                base.KeyDown += value;
            }
            remove
            {
                try { keydownevents.Remove(value); }
                catch { } // catch
                base.KeyDown -= value;
            }
        }



        List<KeyEventHandler> keyupevents = new List<KeyEventHandler>();


        /// <summary>
        /// Hides DataGridView KeyUp Implementation to provide a functionality closer to the
        /// KeyUp event in VB6
        /// </summary>
        public new event KeyEventHandler KeyUp
        {
            add
            {
                keyupevents.Add(value);
                base.KeyUp += value;
            }
            remove
            {
                try { keyupevents.Remove(value); }
                catch { } // catch
                base.KeyUp -= value;
            }
        }
 

        

        internal void AttachKeyEventsToControl(Control control)
        {
            control.KeyDown -= _controlKeyDown;
            control.KeyPress -= _controlKeyPress;
            control.KeyUp -= _controlKeyUp;


            control.KeyDown += _controlKeyDown;
            control.KeyPress += _controlKeyPress;
            control.KeyUp += _controlKeyUp;
        }

        void control_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (KeyEventHandler handler in keyupevents)
            {
                handler.Invoke(sender, e);
            } // foreach

        }

        void control_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        void control_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (KeyEventHandler handler in keydownevents)
            {
                handler.Invoke(sender, e);
            } // foreach
        }
    }

    /// <summary>
    /// Compatitility types supported
    /// </summary>
    public enum GridCompatibility
    {
        /// <summary>
        /// Microsoft Flex Grid Compatibility
        /// </summary>
        MSFlexGrid,
        /// <summary>
        /// True Database Grid Compatibility
        /// </summary>
        TrueDBGrid,
        /// <summary>
        /// DataGridView Grid Compatibility
        /// </summary>
        DataGridView
    }
}