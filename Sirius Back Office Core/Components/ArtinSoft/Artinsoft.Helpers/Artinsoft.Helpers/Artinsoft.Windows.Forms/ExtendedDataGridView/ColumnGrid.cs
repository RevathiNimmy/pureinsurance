// Author: mrojas
// Project: Artinsoft.Windows.Forms
// Path: D:\VbcSPP\src\Helpers\Artinsoft.Windows.Forms\ExtendedDataGridView
// Creation date: 8/7/2009 4:36 PM
// Last modified: 10/10/2009 9:30 PM

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace Artinsoft.Windows.Forms
{
    #region Class DataGridView Extended Column

    /// <summary>
    /// Represents a column in a Artinsoft.Windows.Forms.ExtendedGridView
    /// </summary>
    [ToolboxBitmap(typeof(ResFinder), "Artinsoft.Windows.Forms.Resources.ToolColumn.bmp")]
    public class DataGridViewExtendedColumn : DataGridViewColumn
    {

        #region Constructor
        /// <summary>
        /// Default constructor for DataGridViewExtendedColumn. 
        /// Invokes DataGridExtendedCell default constructor
        /// </summary>
        public DataGridViewExtendedColumn()
            : base(new DataGridExtendedCell())
        {
        }
        #endregion

        #region New Properties

        /// <summary>
        /// Gets the design mode flag.
        /// </summary>
        internal bool InDesignMode
        {
            get
            {
                return Process.GetCurrentProcess().ProcessName == "devenv";
            }
        }

        ValueCollection _DisplayValues;
        /// <summary>
        /// The list of data to be displayed as combo options, or combo images (using the index in the image list)
        /// </summary>
        [Category("Values")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(ValueCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("The list of data to be displayed as combo options, or combo images (using the index in the image list)")]
        public ValueCollection DisplayValues
        {
            get
            {
                if (_DisplayValues == null)
                {
                    _DisplayValues = new ValueCollection();
                }
                return _DisplayValues;
            }
            set
            {
                _DisplayValues = value;
            }
        }

        private bool _dropDownList;
        /// <summary>
        /// If true, the textbox cannot be edited, else the textbox is editable
        /// </summary>
        [Description("If true, the textbox cannot be edited, else the textbox is editable"), Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(false)]
        public bool DropDownList
        {
            get
            {
                return _dropDownList;
            }
            set
            {
                _dropDownList = value;
            }
        }

        private object _defaultNewRowValue;

        /// <summary>
        /// The default value for the column when editing a new row
        /// </summary>
        [Browsable(true), Description("The default value for the column when editing a new row"), Category("Data")]
        public object DefaultNewRowValue
        {
            get
            {
                return _defaultNewRowValue;
            }
            set
            {
                _defaultNewRowValue = value;
            }
        }

        /// <summary>
        /// Flag that determines if the Column has a Display Values table associated
        /// </summary>
        [Browsable(false)]
        public bool HasDisplayValues
        {
            get
            {
                return (DisplayValues != null && DisplayValues.Count > 0);
            }
        }

        private ImageList _imageList;
        /// <summary>
        /// Image list associated with the values of the column
        /// </summary>
        [Browsable(true), Description("Image list associated with the values of the column"), Category("Values")]
        public ImageList ImageList
        {
            get
            {
                return _imageList;
            }
            set
            {
                if (_imageList != value)
                {
                    _imageList = value;
                    if (_imageList != null && InDesignMode)
                    {
                        UseImageList = true;
                        Translate = true;
                    }
                    if (_imageList == null && InDesignMode)
                    {
                        UseImageList = false;
                    }
                }
            }
        }

        private bool _useImageList;
        /// <summary>
        /// Determines if the Image List is going to be used to display the image options
        /// </summary>
        [Description("Determines if the Image List is going to be used to display the options"), Category("Values"), DefaultValue(false)]
        public bool UseImageList
        {
            get
            {
                return _useImageList;
            }
            set
            {
                _useImageList = value;
            }
        }

        Image _buttonImage;
        /// <summary>
        /// Image that will be displayed in the button
        /// </summary>
        [Description("Image that will be displayed in the button"), Category("Design")]
        public Image ButtonImage
        {
            get
            {
                return _buttonImage;
            }
            set
            {
                _buttonImage = value;
            }
        }

        

        private bool _button = true;
        /// <summary>
        /// Enables or disables the button to appear in the column to display the dropdown
        /// </summary>
        [Description("Enables or disables the button to appear in the column to display the dropdown"), Category("Design"), DefaultValue(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Button
        {
            get
            {
                return _button;
            }
            set
            {
                _button = value;
            }
        }

        private bool _buttonAlways = true;
        /// <summary>
        /// The button is displayed for all cells in the column
        /// </summary>
        [Description("The button is displayed for all cells in the column"), Category("Design"), DefaultValue(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ButtonAlways
        {
            get
            {
                return _buttonAlways;
            }
            set
            {
                _buttonAlways = value;
            }
        }

        bool _cycleOnClick;
        /// <summary>
        /// When the user click the cell the value cycle between its Values associated
        /// </summary>
        [Description("When the user click the cell the value cycle between its Values associated"), Category("Values"), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool CycleOnClick
        {
            get
            {
                return _cycleOnClick;
            }
            set
            {
                _cycleOnClick = value;
            }
        }

        bool _SortItems;
        /// <summary>
        /// Sort the items of its table associated
        /// </summary>
        [Description("Sort the items of its table associated"), Category("Values"), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool SortItems
        {
            get
            {
                return _SortItems;
            }
            set
            {
                _SortItems = value;
            }
        }

        bool _translate;
        /// <summary>
        /// The value is displayed according to its table associated
        /// </summary>
        [Description("The value is displayed according to its table associated"), Category("Values"), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Translate
        {
            get
            {
                return _translate;
            }
            set
            {
                _translate = value;
            }
        }

        private DataGridViewDropDown _gridDropDown;
        /// <summary>
        /// The DatagridView DropDown control associated with the column
        /// </summary>
        [Description("The DatagridView DropDown control associated with the column"), Category("Design")]
        public DataGridViewDropDown DropDownGrid
        {
            get
            {
                return _gridDropDown;
            }
            set
            {
                _gridDropDown = value;
            }
        }

        bool _annotatePicture;
        /// <summary>
        /// True when the user specify images the picture is displayed with the current value
        /// </summary>
        [Description("True when the user specify images the picture is displayed with the value"), Category("Values"), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool AnnotatePicture
        {
            get
            {
                return _annotatePicture;
            }
            set
            {
                _annotatePicture = value;
            }
        }

        bool _allowResizeDropDown;
        /// <summary>
        /// Gets/Sets boolean value indicating wether or not the DropDown control can be resized
        /// </summary>
        [Description("Allows the drop down control to be resized"), Category("Design"), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool AllowResizeDropDown
        {
            get
            {
                return _allowResizeDropDown;
            }
            set
            {
                _allowResizeDropDown = value;
            }
        }

        /// <summary>
        /// Gets the currently used DataGridExtendedCell CellTemplate value.
        /// </summary>
        [Browsable(false)]
        protected DataGridExtendedCell ExtendedGridCellTemplate
        {
            get
            {
                return (DataGridExtendedCell)CellTemplate;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Clones the object to a new instance with all the attributes and state
        /// as the current instance
        /// </summary>
        /// <returns>The new cloned object</returns>
        public override object Clone()
        {
            DataGridViewExtendedColumn column = base.Clone() as DataGridViewExtendedColumn;
            if (column != null)
            {
                column.AllowResizeDropDown = AllowResizeDropDown;
                column.AnnotatePicture = AnnotatePicture;
                column.AutoSizeMode = AutoSizeMode;
                column.Button = Button;
                column.ButtonAlways = ButtonAlways;
                column.ButtonImage = ButtonImage;
                column.CycleOnClick = CycleOnClick;
                column.DefaultNewRowValue = DefaultNewRowValue;
                column.DisplayValues = DisplayValues;
                column.DropDownGrid = DropDownGrid;
                column.DropDownList = DropDownList;
                column.ImageList = ImageList;
                column.SortItems = SortItems;
                column.Translate = Translate;
                column.UseImageList = UseImageList;
            }
            return column;
        }

        /// <summary>
        /// Gets/Sets the current DataGridViewCell CellTemplate
        /// The value assigned to this property needs to be an ExtendedGridTextBoxButtonCell
        /// instance, otherwise, an exception is raised.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                DataGridExtendedCell dataGridCell = value as DataGridExtendedCell;
                if (value != null && dataGridCell == null)
                {
                    throw new InvalidCastException("Value for CellTemplate has to be a ExtendedGridTextBoxButtonCell instance or descendant");
                }
                base.CellTemplate = value;
            }
        }
        #endregion
    }

    #endregion

    #region Class DataGrid Extended Cell

    /// <summary>
    /// The class that implements the cell for the Extended GridTextBoxButton Column
    /// It paints the image associated when it is required
    /// </summary>
    public class DataGridExtendedCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// The paint call its base method and draws an image related with the collection associated
        /// when it is required
        /// </summary>
        /// <param name="graphics">The graphics object used to paint</param>
        /// <param name="clipBounds">A rectangle that represents the area to be painted</param>
        /// <param name="cellBounds">A rectangle of the bounds of the cell to be painted</param>
        /// <param name="rowIndex">The row index of the cell that is being painted</param>
        /// <param name="cellState">A bitwise combination of states values that specifies the state of the cell</param>
        /// <param name="value">The data of the cell that is being painted</param>
        /// <param name="formattedValue">The formatted data of the cell that is being painted</param>
        /// <param name="errorText">An error message that is associated with the cell</param>
        /// <param name="cellStyle">A cell style that contains formatting and style about the cell</param>
        /// <param name="advancedBorderStyle">Border styles for the cell that is being painted</param>
        /// <param name="paintParts">A bitwise combination of parts values that specifies which parts of the cell need to be painted</param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            DataGridViewColumn column = DataGridView.Columns[ColumnIndex];
            DataGridViewExtendedColumn extColumn = column as DataGridViewExtendedColumn;
            if (extColumn != null)
            {
                if (extColumn.Translate && extColumn.UseImageList && extColumn.HasDisplayValues)
                {
                    if (value != null)
                    {
                        string strImageIndex = null;
                        foreach (DisplayValue dv in extColumn.DisplayValues)
                        {
                            if (dv.Key != null && dv.Value != null)
                            {
                                object key = dv.Key;
                                if (ExtendedDataGridView.IsNumeric(key))
                                {
                                    string tmpIndex = Convert.ToInt32(key).ToString();
                                    if (tmpIndex.Equals(value))
                                    {
                                        strImageIndex = dv.Value.ToString();
                                        break;
                                    }
                                }
                                else if (dv.Key.ToString().Equals(value))
                                {
                                    strImageIndex = dv.Value.ToString();
                                    break;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(strImageIndex))
                        {
                            int imageIndex;
                            Image img = null;
                            if (!int.TryParse(strImageIndex, out imageIndex))
                            {
                                img = extColumn.ImageList.Images[strImageIndex];
                            }
                            else
                            {
                                if (imageIndex >= 0 && imageIndex < extColumn.ImageList.Images.Count)
                                {
                                    img = extColumn.ImageList.Images[imageIndex];
                                }
                            }
                            if (img != null)
                            {
                                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, string.Empty, string.Empty, errorText, cellStyle, advancedBorderStyle, paintParts);
                                int minWidth = img.Width;
                                int minHeight = img.Height;
                                if (cellBounds.Width - 2 < minWidth)
                                {
                                    minWidth = cellBounds.Width - 2;
                                    minHeight = (minWidth * img.Height) / img.Width;
                                }
                                if (cellBounds.Height - 2 < minHeight)
                                {
                                    minHeight = cellBounds.Height - 2;
                                    minWidth = (minHeight * img.Width) / img.Height;
                                }
                                int top = cellBounds.Top + ((cellBounds.Height - minHeight) / 2);
                                int left = cellBounds.Left + 1;
                                if (!extColumn.AnnotatePicture)
                                {
                                    left = cellBounds.Left + ((cellBounds.Width - minWidth) / 2);
                                }
                                graphics.DrawImage(img, left, top, minWidth, minHeight);
                                if (extColumn.AnnotatePicture)
                                {
                                    Rectangle clippedCellBounds = new Rectangle(cellBounds.Left + minWidth + 1, cellBounds.Top, cellBounds.Width - minWidth - 1, cellBounds.Height);
                                    base.Paint(graphics, clipBounds, clippedCellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                                }
                                return;
                            }
                        }
                    }
                }
            }
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        /// <summary>
        /// Gets the Type instance for DataGridExtendedEditingControl
        /// </summary>
        public override Type EditType
        {
            get
            {
                return typeof(DataGridExtendedEditingControl);
            }
        }

        internal Type cellType;
        /// <summary>
        /// Gets the current ValueType as a Type instance.  String is used by default.
        /// </summary>
        public override Type ValueType
        {
            get
            {
                if (cellType == null)
                    return typeof(object);
                else
                    return cellType;
            }
        }

        /// <summary>
        /// Gets the value used by default when adding a new row.  If DefaultNewRowValue
        /// cannot be properly determined locally, a call to the base DefaultNewRowValue is made.
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                if (DataGridView != null && DataGridView.Columns != null)
                {
                    DataGridViewColumn column = DataGridView.Columns[ColumnIndex];
                    DataGridViewExtendedColumn extColumn = column as DataGridViewExtendedColumn;
                    if (extColumn != null && extColumn.DefaultNewRowValue != null)
                    {
                        return extColumn.DefaultNewRowValue;
                    }
                }
                return base.DefaultNewRowValue;
            }
        }

        /// <summary>
        /// Gets the value of the cell as formatted for display.
        /// If the cell is using value translation, then the formatted translated value is returned.
        /// </summary>
        /// <param name="value">The value to be formatted.</param>
        /// <param name="rowIndex">The index of the cell's parent row.</param>
        /// <param name="cellStyle">The DataGridViewCellStyle in effect for the cell.</param>
        /// <param name="valueTypeConverter">A TypeConverter associated with the value type that provides custom conversion to the formatted value type, or a null reference (Nothing in Visual Basic) if no such custom conversion is needed.</param>
        /// <param name="formattedValueTypeConverter">A TypeConverter associated with the formatted value type that provides custom conversion from the value type, or a null reference (Nothing in Visual Basic) if no such custom conversion is needed.</param>
        /// <param name="context">A bitwise combination of DataGridViewDataErrorContexts values describing the context in which the formatted value is needed.</param>
        /// <returns>The formatted value of the cell or a null reference (Nothing in Visual Basic) if the cell does not belong to a DataGridView control.</returns>
        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            DataGridViewColumn column = DataGridView.Columns[ColumnIndex];
            DataGridViewExtendedColumn extColumn = column as DataGridViewExtendedColumn;
            if (extColumn != null)
            {
                if (extColumn.Translate && extColumn.HasDisplayValues ||
                    (extColumn.HasDisplayValues && extColumn.DropDownList)
                    )
                {
                    string basevalue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context).ToString();
                    if (value != null)
                    {
                        if (context == DataGridViewDataErrorContexts.Formatting)
                        {
                            string translated = LookupValueByKeyAsString(extColumn, basevalue);
                            if (!string.IsNullOrEmpty(translated))
                                return translated;
                            else 
                                return basevalue;
                        }
                        if (!extColumn.UseImageList)
                        {
                            string translated = LookupValueByKeyAsString(extColumn, basevalue);
                            if (!string.IsNullOrEmpty(translated))
                                return translated;
                        }
                        return basevalue;
                    }
                }
            }
            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }

        /// <summary>
        /// Lookup value by key
        /// </summary>
        internal static object LookupValueByKey(DataGridViewExtendedColumn extColumn, string basevalue)
        {
            object translated = null;
            foreach (DisplayValue dv in extColumn.DisplayValues)
            {
                if (dv.Key != null && dv.Key.ToString().Equals(basevalue))
                {
                    translated = dv.Value;
                    break;
                }
            }
            return translated;
        } // LookupValueByKey(extColumn, basevalue)

        internal static string LookupValueByKeyAsString(DataGridViewExtendedColumn extColumn, string basevalue)
        {
            object val = LookupValueByKey(extColumn, basevalue);
            if (val != null) return val.ToString();
            return null;
        }

        internal static object LookupKeyByDisplayValue(DataGridViewExtendedColumn extColumn, string basevalue)
        {
            object translated = null;
            foreach (DisplayValue dv in extColumn.DisplayValues)
            {
                if (dv.Value != null && dv.Value.ToString().Equals(basevalue))
                {
                    translated = dv.Key;
                    break;
                }
            }
            return translated;
        }

        internal static object LookupKeyByKeyAsString(DataGridViewExtendedColumn extColumn, string basevalue)
        {
            object translated = null;
            foreach (DisplayValue dv in extColumn.DisplayValues)
            {
                if (dv.Key != null && dv.Key.ToString().Equals(basevalue))
                {
                    translated = dv.Key;
                    break;
                }
            }
            return translated;
        }



        /// <summary>
        /// Initializes the control before the edition starts
        /// </summary>
        /// <param name="rowIndex">The row index of the cell that is being painted</param>
        /// <param name="initialFormattedValue">The formatted data of the cell that is being painted</param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DataGridExtendedEditingControl control = base.DataGridView.EditingControl as DataGridExtendedEditingControl;
            if (control != null)
            {
                control.cellReference = this;
                control.EditingControlFormattedValue = initialFormattedValue;
                DataGridViewColumn column = DataGridView.Columns[ColumnIndex];
                DataGridViewExtendedColumn extColumn = column as DataGridViewExtendedColumn;
                if (extColumn != null)
                {
                    //control._TextBox.extColumn = extColumn;
                    control.columnReference = extColumn;
                    control.ApplySettingsFromColumn(extColumn);

                    //Behaviours:
                    //1. Drop down
                    //2. Images
                    //3. Images with display values
                    //4. Combo with display values 
                    //5. Combo with direct value
                    if (extColumn.DropDownGrid != null)
                    {
                        //Behaviour 1
                        control.SetDropDown(extColumn.DropDownGrid);
                    }
                    else if (extColumn.Translate)
                    {
                        if (extColumn.UseImageList)
                        {
                            if (extColumn.ImageList != null && extColumn.HasDisplayValues)
                            {
                                //Behaviour 2 & 3
                                control.SetImageList(extColumn);
                            }
                            else
                            {
                                control.DisplayButton = false;
                                //throw new ArgumentException("To use Translate, needs and Imagelist and the DisplayValues set");
                                //control.SetList(new string[] { "Translate|Images Not values were set", "Please review on designer" });
                            }
                        }
                        else
                        {
                            if (extColumn.HasDisplayValues)
                            {
                                //Behaviour 4
                                control.SetTranslateList(extColumn);
                            }
                            else
                            {
                                //throw new ArgumentException("Not DisplayValues were set");
                                //control.SetList(new string[] { "Translate Not values were set", "Please review on designer" });
                                control.DisplayButton = false;
                            }
                        }
                    }
                    else
                    {
                        if (extColumn.HasDisplayValues)
                        {
                            //Behaviour 5
                            control.SetList(extColumn);
                        }
                        else
                        {
                            //control.SetList(new string[] { "Not values were set", "Please review on designer" });
                            //throw new ArgumentException("Not DisplayValues were set");
                            control.DisplayButton = false;
                        }
                    }
                    control.AutoSelectCurrentValue(extColumn, initialFormattedValue);
                }
            }
        }

        Image _buttonImage;
        /// <summary>
        /// Image that will be displayed in the button for the cell
        /// By default it displays the image for the owning column
        /// </summary>
        [Description("Image that will be displayed in the button"), Category("Design"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue(null)]
        public Image ButtonImage
        {
            get
            {
                if (OwningColumn == null || OwningExtendedColumn == null)
                {
                    return _buttonImage;
                }
                else if (_buttonImage != null)
                {
                    return _buttonImage;
                }
                else
                {
                    return OwningExtendedColumn.ButtonImage;
                }
            }
            set
            {
                if (_buttonImage != value)
                {
                    _buttonImage = value;
                }
            }
        }

        /// <summary>
        /// Clones the object and all its properties
        /// </summary>
        /// <returns>The new cloned object</returns>
        public override object Clone()
        {
            DataGridExtendedCell cell = base.Clone() as DataGridExtendedCell;
            if (cell != null)
            {
                cell.ButtonImage = ButtonImage;
            }
            return cell;
        }

        private DataGridViewExtendedColumn OwningExtendedColumn
        {
            get { return this.OwningColumn as DataGridViewExtendedColumn; }
        }
    }

    #endregion

    #region Class Extended GridTextButton Editing Control

    /// <summary>
    /// Control in charge of editing values on the DataGridExtendedCell associated with an
    /// DataGridExtendedColumn
    /// </summary>
    internal class DataGridExtendedEditingControl : Panel, IDataGridViewEditingControl, IPopupControlHost
    {
        internal DataGridViewCell cellReference;
        internal DataGridViewExtendedColumn columnReference;
        /// <summary>
        /// Pass the settings from the column to the editing control
        /// </summary>
        /// <param name="column">The column with the respective settings</param>
        internal void ApplySettingsFromColumn(DataGridViewExtendedColumn column)
        {
            ButtonImage = column.ButtonImage;
            _TextBox.ReadOnly = column.DropDownList;
            AllowResizeDropDown = column.AllowResizeDropDown;
            Translate = column.Translate;
            AnnotatePicture = column.AnnotatePicture;
            columnReference = column;
            _Button.Visible = column.Button;
            _pBox.SendToBack();
            if (column.DropDownList && column.HasDisplayValues && column.ImageList != null)
            {//We need to put an imagelist here
                object value = EditingControlFormattedValue;
                if (value!=null)
                {
                    string strValue = value.ToString();
                    if (!String.IsNullOrEmpty(strValue))
                        if (TryToGetImageInPictureBox(strValue))
                            _pBox.BringToFront();
                } 
            }
        }

        bool _translate;
        /// <summary>
        /// The value is displayed according to its table associated
        /// </summary>
        public bool Translate
        {
            get
            {
                return _translate;
            }
            set
            {
                _translate = value;
            }
        }

        bool _annotatePicture;
        /// <summary>
        /// Indicates if the picture is going to display its current value
        /// </summary>
        public bool AnnotatePicture
        {
            get
            {
                return _annotatePicture;
            }
            set
            {
                _annotatePicture = value;
            }
        }

        /// <summary>
        /// Raises the GotFocus event.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            _TextBox.Focus();
        }

        #region Private Members

        internal TextBox _TextBox;
        private Button _Button;
        private PictureBox _pBox;
        private ListBox _ListBox;
        private ListView _ListView;
        private DataGridViewDropDown _DataGridDropDown;
        private DataGridView _DataGridView;
        private ExtendedDataGridView _ExtendedGridView;
        private int _RowIndex;
        private bool _ValueChanged;
        #endregion


        private Image _ButtonImage;
        /// <summary>
        /// Gets or sets the button image to the current cell.
        /// </summary>
        public Image ButtonImage
        {
            get
            {
                return _ButtonImage;
            }
            set
            {
                bool resize = false;
                if (value == null)
                {
                    _ButtonImage = _DefaultButtonImage;
                }
                else
                {
                    _ButtonImage = value;
                    resize = true;
                }
                if (resize)
                {
                    Image resizeImage = _ButtonImage;
                    int minWidth = resizeImage.Width;
                    int minHeight = resizeImage.Height;
                    if (_Button.Width - 2 < minWidth)
                    {
                        minWidth = _Button.Width - 2;
                        minHeight = (minWidth * resizeImage.Height) / resizeImage.Width;
                    }
                    if (_Button.Height - 2 < minHeight)
                    {
                        minHeight = _Button.Height - 2;
                        minWidth = (minHeight * resizeImage.Width) / resizeImage.Height;
                    }
                    Bitmap resized = new Bitmap(minWidth, minHeight);
                    using (Graphics g = Graphics.FromImage(resized))
                    {
                        //Higher quality but the transparent not so good
                        //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(resizeImage, 0, 0, minWidth, minHeight);
                        resized.MakeTransparent();
                    }
                    _Button.Image = resized;
                }
                else
                {
                    _Button.Image = _ButtonImage;
                }
            }
        }

        private Image _DefaultButtonImage;

        /// <summary>
        /// Displays the button of the cell
        /// </summary>
        public bool DisplayButton
        {
            get
            {
                return _Button.Visible;
            }
            set
            {
                _Button.Visible = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DataGridExtendedEditingControl class.
        /// </summary>
        public DataGridExtendedEditingControl()
        {
            _TextBox = new TextBox();
            _TextBox.TabIndex = 0;
            _Button = new Button();
            _pBox = new PictureBox();
            _pBox.Dock = DockStyle.Fill;
            _pBox.SizeMode = PictureBoxSizeMode.CenterImage;
            _Button.TabIndex = 1;
            _Button.Dock = DockStyle.Right;
            _Button.BackColor = SystemColors.ButtonFace;
            _DefaultButtonImage = Properties.Resources.DownArrow;
            ((Bitmap)_DefaultButtonImage).MakeTransparent();
            _Button.Image = _DefaultButtonImage;

            _Button.Click -= _Button_Click;
            _Button.Click += _Button_Click;

            _TextBox.Dock = DockStyle.Fill;
            _TextBox.BorderStyle = BorderStyle.None;

            _TextBox.TextChanged -= _TextBox_TextChanged;
            _TextBox.TextChanged += _TextBox_TextChanged;

            _Button.Dock = DockStyle.Right;
            _Button.MaximumSize = new Size(16, 18);
            _Button.MinimumSize = new Size(16, 18);
            
            Controls.Add(_pBox);
            Controls.Add(_TextBox);
            Controls.Add(_Button);
            Width = 100;
            Height = 100;
            SizeChanged -= ExtendedGridTextBoxButtonEditingControl_SizeChanged;
            SizeChanged += ExtendedGridTextBoxButtonEditingControl_SizeChanged;

            _ListBox = new ListBox();
            _ListView = new ListView();

            _ListBox.SelectedValueChanged -= _ListBox_SelectedValueChanged;
            _ListBox.SelectedValueChanged += _ListBox_SelectedValueChanged;

            _ListView.ItemSelectionChanged -= _ListView_ItemSelectionChanged;
            _ListView.ItemSelectionChanged += _ListView_ItemSelectionChanged;

            _ListView.Click -= _ListView_Click;
            _ListView.Click += _ListView_Click;

            _ListBox.SelectionMode = System.Windows.Forms.SelectionMode.One;
            _ListView.MultiSelect = false;


            DropSize = new Size(Width, Height);
            _popupCtrl.Closing -= _popupCtrl_Closing;
            _popupCtrl.Closing += _popupCtrl_Closing;
        }

        void ExtendedGridTextBoxButtonEditingControl_SizeChanged(object sender, EventArgs e)
        {
            int pixels = (Height - _TextBox.Height) / 2;
            Padding = new Padding(3, pixels, 0, pixels);
        }

        void _ListView_Click(object sender, EventArgs e)
        {
            HideDropDown();
        }

        void _ListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (_ListView.SelectedIndices.Count > 0 && PopupVisible)
            {
                ListViewItem selected = _ListView.SelectedItems[0];
                if (AnnotatePicture)
                    EditingControlFormattedValue = selected.Text;
                else
                    EditingControlFormattedValue = selected.Name;
                if (_ExtendedGridView != null)
                    _ExtendedGridView.RaiseComboSelectEvent(sender, columnReference.Index, _RowIndex);
                NotifyDataGridViewOfValueChange();
                HideDropDown();
            }
        }

        void _ListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_ListBox.SelectedItem != null && PopupVisible)
            {
                if (Translate)
                {
                    if (columnReference != null && columnReference.DropDownList)
                    {
                        EditingControlFormattedValue = _ListBox.SelectedItem;
                    } 
                    else
                    EditingControlFormattedValue = ((Item)_ListBox.SelectedItem).Key;
                }
                else
                {
                    EditingControlFormattedValue = _ListBox.SelectedItem;
                }
                if (_ExtendedGridView != null)
                    _ExtendedGridView.RaiseComboSelectEvent(sender, columnReference.Index, _RowIndex);
                NotifyDataGridViewOfValueChange();
            }
            HideDropDown();
        }

        void _popupCtrl_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _lastHideTime = DateTime.Now;
        }

        private DateTime _lastHideTime = DateTime.Now;
        private Size _sizeControl;
        /// <summary>
        /// Gets or sets the size of the DropDown control.
        /// </summary>
        public Size DropSize
        {
            get { return _sizeControl; }
            set
            {
                _sizeControl = value;
                if (DropDownSizeMode == SizeMode.UseDropDownSize)
                    AutoSizeDropDown();
            }
        }

        SizeMode _sizeMode = SizeMode.UseControlSize;

        /// <summary>
        /// Gets or sets a value indicating whether the DropDown control is automatically resized to display its entire contents.
        /// </summary>
        protected void AutoSizeDropDown()
        {
            if (DropDownControl != null)
            {
                switch (DropDownSizeMode)
                {
                    case SizeMode.UseControlCallerSize:
                        DropDownControl.Size = new Size(Width, DropSize.Height);
                        break;

                    case SizeMode.UseControlSize:
                        //DropDownControl.Size = DropDownControl.Size;
                        break;

                    case SizeMode.UseDropDownSize:
                        DropDownControl.Size = DropSize;
                        break;
                }
            }
        }

        /// <summary>
        /// Releases the resources used by the DataGridExtendedEditingControl
        /// </summary>
        /// <param name="disposing"> true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_timerAutoFocus != null)
                {
                    _timerAutoFocus.Dispose();
                    _timerAutoFocus = null;
                }
            }
            base.Dispose(disposing);
        }

        void _Button_Click(object sender, EventArgs e)
        {
            if (_ExtendedGridView != null)
                _ExtendedGridView.RaiseButtonClickEvent(sender, columnReference.Index, _RowIndex);

            //Needed so that when the user types a value and clicks the dropdown button
            //the typed value will be selected if found in the appropriate Coulmn.
            if (DropDownControl == _DataGridDropDown)
            {
                _DataGridDropDown.SelectedValue = EditingControlFormattedValue;
            }
            ShowDropDown();
        }


        internal void AutoSelectCurrentValue(DataGridViewExtendedColumn column, object initialValue)
        {
            if (DropDownControl == _ListBox)
            {
                _ListBox.SelectedItems.Clear();
                string strInitialValue = initialValue.ToString();
                if (column.Translate)
                {
                    foreach (Item item in _ListBox.Items)
                    {
                        if (item.Value.ToString() == strInitialValue)
                        {
                            _ListBox.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    _ListBox.SelectedItems.Add(strInitialValue);
                }
            }
            else if (DropDownControl == _ListView)
            {
                _ListView.SelectedItems.Clear();
                foreach (ListViewItem item in _ListView.Items)
                {
                    if (column.AnnotatePicture)
                    {
                        if (item.Text == EditingControlFormattedValue.ToString())
                        {
                            item.Selected = true;
                            item.Focused = true;
                            break;
                        }
                    }
                    else
                    {
                        if (item.Name == EditingControlFormattedValue.ToString())
                        {
                            item.Selected = true;
                            item.Focused = true;
                            break;
                        }
                    }
                }
            }
            else if (DropDownControl == _DataGridDropDown)
            {
                if (_DataGridDropDown != null)
                {
                    _DataGridDropDown.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    //This was causing the incorrect value to sometimes show when the popup was displayed
                    //_DataGridDropDown.SelectedValue = initialValue;
                }
            }
        }

        /// <summary>
        /// Load a set of values into the ListBox control.
        /// </summary>
        /// <param name="column">The column with the respective settings</param>
        public void LoadValuesToListBox(DataGridViewExtendedColumn column)
        {
            if (_loadedListBoxValues != column.GetHashCode())
            {
                _ListBox.Items.Clear();
                foreach (DisplayValue dv in column.DisplayValues)
                {
                    if (dv.Value != null)
                    {
                        _ListBox.Items.Add(dv.Value);
                    }
                }
                _loadedListBoxValues = column.GetHashCode();
                _ListBox.Sorted = column.SortItems;
            }
        }

        /// <summary>
        /// Load an array values into the ListBox control.
        /// </summary>
        /// <param name="listValues">Array values to load.</param>
        public void LoadValuesToListBox(string[] listValues)
        {
            _ListBox.Items.Clear();
            _ListBox.Items.AddRange(listValues);
        }

        #region Popup Functionality
        private PopupControl _popupCtrl = new PopupControl();
        private Timer _timerAutoFocus;


        private void AutoDropDown()
        {
            if (_popupCtrl != null && PopupVisible)
                HideDropDown();
            else if ((DateTime.Now - _lastHideTime).Milliseconds > 50)
                ShowDropDown();
        }

        bool _isResizable = true;
        /// <summary>
        /// Gets a value indicating whether the DropDown control can be resized.
        /// </summary>
        public bool AllowResizeDropDown
        {
            get { return _isResizable; }
            set { _isResizable = value; }
        }

        bool _droppedDown;
        /// <summary>
        /// Determines whether the button displays the ComboBox value list.
        /// </summary>
        [Browsable(false)]
        public bool IsDroppedDown
        {
            get { return _droppedDown; }
        }

        /// <summary>
        /// Occurs when the drop-down portion of a ComboBox is shown.
        /// </summary>
        public event EventHandler DropDown;


        /// <summary>
        /// Raise the DropDown Event programmatically.
        /// </summary>
        public void RaiseDropDownEvent()
        {
            EventHandler eventHandler = DropDown;
            if (eventHandler != null)
                DropDown(this, EventArgs.Empty);
        }

        /// <summary>
        /// Displays drop-down area of combo box, if not already shown.
        /// </summary>
        public void ShowDropDown()
        {
            if (_popupCtrl != null && !IsDroppedDown)
            {
                // Raise drop-down event.
                RaiseDropDownEvent();

                // Restore original control size.
                AutoSizeDropDown();

                Point location = PointToScreen(new Point(0, Height));

                // Actually show popup.
                PopupResizeMode resizeMode = (AllowResizeDropDown ? PopupResizeMode.BottomRight : PopupResizeMode.None);
                _popupCtrl.Show(DropDownControl, location.X, location.Y, Width, Height, resizeMode);
                _droppedDown = true;

                _popupCtrl.PopupControlHost = this;

                // Initialize automatic focus timer?
                if (_timerAutoFocus == null)
                {
                    _timerAutoFocus = new Timer();
                    _timerAutoFocus.Interval = 10;
                    _timerAutoFocus.Tick -= timerAutoFocus_Tick;
                    _timerAutoFocus.Tick += timerAutoFocus_Tick;
                }
                // Enable the timer!
                _timerAutoFocus.Enabled = true;
            }
        }

        private void timerAutoFocus_Tick(object sender, EventArgs e)
        {
            if (PopupVisible && !DropDownControl.Focused)
            {
                DropDownControl.Focus();
                _timerAutoFocus.Enabled = false;
            }
        }

        private bool PopupVisible
        {
            get
            {
                return _popupCtrl.Visible;
            }
        }

        private void _dropDown_LostFocus(object sender, EventArgs e)
        {
            _lastHideTime = DateTime.Now;
        }


        /// <summary>
        /// Hides drop-down area of combo box, if shown.
        /// </summary>
        public void HideDropDown()
        {
            if (_popupCtrl != null && IsDroppedDown)
            {
                // Hide drop-down control.
                _popupCtrl.Hide();
                _droppedDown = false;

                //Reset selected value to default for next appereance
                if (_DataGridDropDown != null)
                {
                    if (_DataGridDropDown.Rows.Count > 0)
                    {
                        _DataGridDropDown.SelectedValue = _DataGridDropDown.Rows[0].Cells[_DataGridDropDown.GetDataFieldColumnIndex()].Value;
                    }
                }


                // Disable automatic focus timer.
                if (_timerAutoFocus != null && _timerAutoFocus.Enabled)
                    _timerAutoFocus.Enabled = false;
            }
        }

        private Control _dropDownControl;
        private Control DropDownControl
        {
            get
            {
                return _dropDownControl;
            }
            set
            {
                _dropDownControl = value;
            }
        }


        /// <summary>
        /// Size mode constants used to specify the behavior of the control.
        /// </summary>
        public enum SizeMode
        {
            /// <summary>
            /// Use the size of the control caller.
            /// </summary>
            UseControlCallerSize,
            /// <summary>
            /// Use the size of the container control.
            /// </summary>
            UseControlSize,
            /// <summary>
            /// Use the current size of the DropDown.
            /// </summary>
            UseDropDownSize,
        }

        /// <summary>
        /// Gets or sets a value indicating the SizeMode used by the DropDown control.
        /// </summary>
        public SizeMode DropDownSizeMode
        {
            get { return _sizeMode; }
            set
            {
                if (value != _sizeMode)
                {
                    _sizeMode = value;
                    AutoSizeDropDown();
                }
            }
        }

        #endregion

        void _TextBox_TextChanged(object sender, EventArgs e)
        {
            string typedTest = _TextBox.Text;
            //If dropdownlist is true we must translate and set that value in the cell
            if (columnReference!=null)
            {

                if (columnReference.ImageList!=null && columnReference.HasDisplayValues && columnReference.Translate && columnReference.DropDownList)
                {
                    if (UpdatePictureBox(typedTest))
                    {
                        _pBox.BringToFront();
                    }
                    else
                        _pBox.SendToBack();
                } // if
                    if (_TextBox.ReadOnly)
                    {
                        //Now let's see if we have a display value
                        object realValue = DataGridExtendedCell.LookupKeyByDisplayValue(columnReference, typedTest);
                        if (realValue != null)
                        {
                            if (cellReference != null)
                            {
                                cellReference.Value = realValue;
                                DataGridExtendedCell cellEx = cellReference as DataGridExtendedCell;
                                if (cellEx != null) cellEx.cellType = realValue.GetType();
                                return;
                            } // if
                        } // if
                    }
                    else
                    {
                        if (columnReference.HasDisplayValues)
                        {
                            //First let's see if what the user typed was a key or a it is a displayvalue
                            object realValue = DataGridExtendedCell.LookupKeyByKeyAsString(columnReference, typedTest);
                            if (realValue != null)
                            {
                                if (cellReference != null)
                                {
                                    cellReference.Value = realValue;
                                    DataGridExtendedCell cellEx = cellReference as DataGridExtendedCell;
                                    if (cellEx != null) cellEx.cellType = realValue.GetType();
                                    return;
                                } // if
                            } // if
                            else
                            {
                                //Now let's see if we have a display value
                                realValue = DataGridExtendedCell.LookupKeyByDisplayValue(columnReference, typedTest);
                                if (realValue != null)
                                {
                                    if (cellReference != null)
                                    {
                                        cellReference.Value = realValue;
                                        DataGridExtendedCell cellEx = cellReference as DataGridExtendedCell;
                                        if (cellEx != null) cellEx.cellType = realValue.GetType();
                                        return;
                                    } // if
                                } // if
                            } // else
                            //Value was not found in list and it has a dropdown. So let's set it

                            cellReference.Value = typedTest;
                            DataGridExtendedCell cellEx2 = cellReference as DataGridExtendedCell;
                            if (cellEx2 != null) cellEx2.cellType = typedTest.GetType();


                        } // if
                    } // else
            } // if
            else
            NotifyDataGridViewOfValueChange();
        }

        private bool UpdatePictureBox(string typedTest)
        {
            object res = DataGridExtendedCell.LookupValueByKey(columnReference, typedTest);
            String strImageIndex = null;
            if (res != null)
                strImageIndex = res.ToString();
            return TryToGetImageInPictureBox(strImageIndex);
        }

        private bool TryToGetImageInPictureBox(String strImageIndex)
        {
            if (!string.IsNullOrEmpty(strImageIndex))
            {
                int imageIndex;
                Image img = null;
                if (!int.TryParse(strImageIndex, out imageIndex))
                {
                    img = columnReference.ImageList.Images[strImageIndex];
                } // if
                else
                {
                    if (imageIndex >= 0 && imageIndex < columnReference.ImageList.Images.Count)
                    {
                        img = columnReference.ImageList.Images[imageIndex];
                    } // if
                } // else
                if (img != null)
                {
                    _pBox.Image = img;
                    return true;
                } // if

            } // if
            return false;
        }

        private void NotifyDataGridViewOfValueChange()
        {
            _DataGridView.NotifyCurrentCellDirty(true);
        }


        #region IDataGridViewEditingControl Members
        /// <summary>
        /// Changes the control's user interface (UI) to be consistent with the specified cell style.
        /// </summary>
        /// <param name="dataGridViewCellStyle">The DataGridViewCellStyle to use as the model for the UI.</param>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            _TextBox.Font = dataGridViewCellStyle.Font;
            ForeColor = dataGridViewCellStyle.ForeColor;
            BackColor = dataGridViewCellStyle.BackColor;
        }

        /// <summary>
        /// Gets or sets the DataGridView that contains the cell.
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return _DataGridView;
            }
            set
            {
                _DataGridView = value;
                _ExtendedGridView = _DataGridView as ExtendedDataGridView;
            }
        }

        /// <summary>
        /// Gets or sets the formatted value of the cell being modified by the editor.
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return _TextBox.Text;
            }
            set
            {
                if (value != null)
                {
                    _TextBox.Text = value.ToString();
                }
                else
                {
                    _TextBox.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the hosting cell's parent row.
        /// </summary>
        public int EditingControlRowIndex
        {
            get
            {
                return _RowIndex;
            }
            set
            {
                _RowIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the value of the editing control differs from the value of the hosting cell.
        /// </summary>
        public bool EditingControlValueChanged
        {
            get
            {
                return _ValueChanged;
            }
            set
            {
                _ValueChanged = value;
            }
        }

        /// <summary>
        /// Determines whether the specified key is a regular input key that the editing control should process or a special key that the DataGridView should process.
        /// </summary>
        /// <param name="keyData">A Keys that represents the key that was pressed.</param>
        /// <param name="dataGridViewWantsInputKey"> true when the DataGridView wants to process the Keys in keyData; otherwise, false.</param>
        /// <returns> true if the specified key is a regular input key that should be handled by the editing control; otherwise, false.</returns>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            if (dataGridViewWantsInputKey)
            {
                switch (keyData)
                {
                    case Keys.Left:
                    case Keys.Right:
                        {
                            if (_TextBox.Text.Length == 0)
                                return false;
                            if (_TextBox.SelectionStart == 0 && keyData == Keys.Left)
                                return false;
                            if (_TextBox.SelectionStart == _TextBox.Text.Length && keyData == Keys.Right)
                                return false;
                            break;
                        }
                    case Keys.PageUp:
                    case Keys.PageDown:
                    case Keys.Tab:
                    case Keys.Escape:
                    case Keys.Up:
                    case Keys.Down:
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the cursor used when the mouse pointer is over the DataGridView..::.EditingPanel but not over the editing control.
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get
            {
                return _TextBox.Cursor;
            }
        }

        /// <summary>
        /// Retrieves the formatted value of the cell.
        /// </summary>
        /// <param name="context">A bitwise combination of DataGridViewDataErrorContexts values that specifies the context in which the data is needed.</param>
        /// <returns>An Object that represents the formatted version of the cell contents.</returns>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            string typedText = _TextBox.Text;
            if (columnReference != null && columnReference.HasDisplayValues)
            {
                if (columnReference.DropDownList)
                {
                    //First let's see if the user typed the real value
                    foreach (DisplayValue dv in columnReference.DisplayValues)
                    {
                        if (dv.Key != null && dv.Key.ToString().Equals(typedText))
                        {
                            return dv.Key.ToString();
                        } // if
                    } // foreach
                    foreach (DisplayValue dv in columnReference.DisplayValues)
                    {
                        if (dv.Value != null && dv.Value.ToString().Equals(typedText))
                        {
                            return dv.Key.ToString();
                        } // if
                    } // foreach
                } // if
                else
                {
                    //the user cannot type a value
                    foreach (DisplayValue dv in columnReference.DisplayValues)
                    {
                        if (dv.Value != null && dv.Value.ToString().Equals(typedText))
                        {
                            return dv.Key.ToString();
                        } // if
                    } // foreach
                } // else
            } // if

            return typedText;
        }

        /// <summary>
        /// Prepares the currently selected cell for editing.
        /// </summary>
        /// <param name="selectAll"> true to select all of the cell's content; otherwise, false.</param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
                _TextBox.SelectAll();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell contents need to be repositioned whenever the value changes.
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        #endregion


        private int _loadedMapListViewValues;
        internal void SetImageList(DataGridViewExtendedColumn column)
        {
            DropDownControl = _ListView;
            if (_loadedMapListViewValues != column.GetHashCode())
            {
                _ListView.Items.Clear();
                _ListView.LargeImageList = column.ImageList;
                _ListView.SmallImageList = column.ImageList;
                _ListView.View = View.List;
                if (column.AnnotatePicture)
                {
                    foreach (DisplayValue item in column.DisplayValues)
                    {
                        int nVal;
                        if (item.Value != null && item.Key != null)
                        {
                            if (int.TryParse(item.Value.ToString(), out nVal))
                            {
                                _ListView.Items.Add(item.Key.ToString(), nVal);
                            }
                            else
                            {
                                _ListView.Items.Add(item.Key.ToString(), item.Value.ToString());
                            }
                        }
                    }
                }
                else
                {
                    foreach (DisplayValue item in column.DisplayValues)
                    {
                        if (item.Value != null && item.Key != null)
                        {
                            int nVal;
                            if (int.TryParse(item.Value.ToString(), out nVal))
                            {
                                _ListView.Items.Add(item.Key.ToString(), "", nVal);
                            }
                            else
                            {
                                _ListView.Items.Add(item.Key.ToString(), "", item.Value.ToString());
                            }
                        }
                    }
                }
                _ListView.Sorting = column.SortItems ? SortOrder.Ascending : SortOrder.None;
                _loadedMapListViewValues = column.GetHashCode();
            }
        }

        internal void SetTranslateList(DataGridViewExtendedColumn column)
        {
            DropDownControl = _ListBox;
            LoadTranslateValuesToListBox(column);
        }

        private int _loadedListBoxValues;
        private void LoadTranslateValuesToListBox(DataGridViewExtendedColumn column)
        {
            if (_loadedListBoxValues != column.GetHashCode())
            {
                _ListBox.Items.Clear();
                if (_ListBox != null)
                {
                    foreach (DisplayValue item in column.DisplayValues)
                    {
                        if (item.Key != null && item.Value != null)
                        {
                            _ListBox.Items.Add(new Item(item));
                        }
                    }
                }
                _ListBox.Sorted = column.SortItems;
                _loadedListBoxValues = column.GetHashCode();
            }
            DropDownControl = _ListBox;
        }

        internal void SetList(DataGridViewExtendedColumn column)
        {
            DropDownControl = _ListBox;
            LoadValuesToListBox(column);
        }

        internal void SetList(string[] stringList)
        {
            DropDownControl = _ListBox;
            LoadValuesToListBox(stringList);
        }

        internal void SetDropDown(DataGridViewDropDown dropdown)
        {
            if (dropdown != null)
            {
                DropDownControl = dropdown;
                if (dropdown != _DataGridDropDown)
                {
                    _DataGridDropDown = dropdown;

                    _DataGridDropDown.CellClick -= _DataGridDropDown_CellClick;
                    _DataGridDropDown.CellClick += _DataGridDropDown_CellClick;

                    _DataGridDropDown.KeyDown -= _DataGridDropDown_KeyDown;
                    _DataGridDropDown.KeyDown += _DataGridDropDown_KeyDown;
                }
            }
        }

        void _DataGridDropDown_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (_DataGridDropDown.CurrentCell != null && PopupVisible)
                {
                    EditingControlFormattedValue = _DataGridDropDown.SelectedValue;
                    NotifyDataGridViewOfValueChange();
                    HideDropDown();
                }
            }
        }

        void _DataGridDropDown_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    {
                        EditingControlFormattedValue = _DataGridDropDown.SelectedValue;
                        NotifyDataGridViewOfValueChange();
                        HideDropDown();
                        //needed to avoid the enter key from moving to the next row
                        e.SuppressKeyPress = true;
                        break;
                    }
                case Keys.Escape:
                    {
                        EditingControlFormattedValue = _DataGridDropDown.SelectedValue;
                        NotifyDataGridViewOfValueChange();
                        HideDropDown();
                        break;
                    }
            }
        }

        internal class Item
        {
            private string _Key;
            private string _Value;
            public string Key
            {
                get
                {
                    return _Key;
                }
                set
                {
                    _Key = value;
                }
            }

            public string Value
            {
                get
                {
                    return _Value;
                }
                set
                {
                    _Value = value;
                }
            }

            public Item(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public Item(DisplayValue entry)
                : this((entry.Key != null) ? entry.Key.ToString() : string.Empty,
                (entry.Value != null) ? entry.Value.ToString() : string.Empty) { }

            public override string ToString()
            {
                return Value;
            }
        }
    }
    #endregion
}
