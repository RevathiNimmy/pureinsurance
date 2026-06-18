using System.ComponentModel;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Artinsoft.Windows.Forms
{
    /// <summary>
    /// The value collection that is used in the DataGridView Extended Column
    /// </summary>
    public class ValueCollection : CollectionBase
    {
        /// <summary>
        /// Default constructor for the ValueCollection
        /// </summary>
        public ValueCollection() {}

        /// <summary>
        /// Adds a new item to the collection
        /// </summary>
        /// <param name="key">The key for the the item to be added</param>
        /// <param name="value">The value associated with the given key</param>
        /// <returns></returns>
        public DisplayValue Add(object key,object value)
        {
            DisplayValue newValue = new DisplayValue(key, value);
            return Add(newValue);
        }

        /// <summary>
        /// Adds a new item to the collection
        /// </summary>
        /// <param name="dv">The new item to be added</param>
        /// <returns>The item added</returns>
        public DisplayValue Add(DisplayValue dv)
        {
            this.InnerList.Add(dv);
            return dv;
        }

        /// <summary>
        /// Removes an item of the collection
        /// </summary>
        /// <param name="dv">The item to be removed</param>
        public void Remove(DisplayValue dv)
        {
            this.InnerList.Remove(dv);
        }

        /// <summary>
        /// It determines if the item is in the collection
        /// </summary>
        /// <param name="dv">The item to be found</param>
        /// <returns>True if the item is in the collection</returns>
        public bool Contains(DisplayValue dv)
        {
            return this.InnerList.Contains(dv);
        }

        /// <summary>
        /// Gets or sets the element of the collection at the specified index
        /// </summary>
        /// <param name="i">The zero-based index of the element to set or get</param>
        /// <returns>The element of the collection </returns>
        public DisplayValue this[int i]
        {
            get { return (DisplayValue)this.InnerList[i]; }
            set { this.InnerList[i] = value; }
        }


        /// <summary>
        /// Adds the elements of the parameter collection to the end of the collection
        /// </summary>
        /// <param name="dv">The array of elements to be added</param>
        public void AddRange(DisplayValue[] dv)
        {
            this.InnerList.AddRange(dv);
        }

        /// <summary>
        /// Returns an array of elements of the values of the collection
        /// </summary>
        /// <returns>The array of element values</returns>
        public DisplayValue[] GetValues()
        {
            DisplayValue[] dv = new DisplayValue[this.InnerList.Count];
            this.InnerList.CopyTo(0, dv, 0, this.InnerList.Count);
            return dv;
        }

        /// <summary>
        /// Performs custom processing before adding an element to the collection
        /// </summary>
        /// <param name="index">The index where the element is going to be added</param>
        /// <param name="value">The new value to be added</param>
        protected override void OnInsert(int index, object value)
        {
            base.OnInsert(index, value);
        }
    }


    /// <summary>
    /// Tuple of values to be inserted in a collection
    /// </summary>
    public class DisplayValue
    {
        internal static NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
        
        /// <summary>
        /// Default constructor initializing with default values
        /// </summary>
        public DisplayValue() { }
        /// <summary>
        /// Constructor initializing with a object key and a object value
        /// </summary>
        /// <param name="key">Sets the key with the object passed</param>
        /// <param name="value">Sets the value with the object passed</param>
        public DisplayValue(object key, object value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// The key of the tuple
        /// </summary>
        protected object _Key;

        /// <summary>
        /// The key property of the tuple
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        /// <summary>
        /// The value of the tuple
        /// </summary>
        protected object _Value;

        /// <summary>
        /// The value property of the tuple
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }

    /// <summary>
    /// Tuple of value where the value is a string value
    /// </summary>
    public class DisplayValueString: DisplayValue
    {
        /// <summary>
        /// Default constructor initializing with default values
        /// </summary>
        public DisplayValueString() { StringValue = string.Empty; }
        /// <summary>
        /// Constructor initializing with a string value
        /// </summary>
        /// <param name="value">Sets the value with the string passed</param>
        public DisplayValueString(string value)
        {
            StringValue = value;
        }

        /// <summary>
        /// The key and the value is the string value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public String StringValue
        {
            get { return Convert.ToString(_Value); }
            set { _Value = value; _Key = value; }
        }

        /// <summary>
        /// Returns a string representing the object
        /// </summary>
        /// <returns>The string value representing the object</returns>
        public override string ToString()
        {
            return StringValue;
        }
    }

    /// <summary>
    /// Tuple of value where the value is a double value
    /// </summary>
    public class DisplayValueDouble : DisplayValue
    {
        /// <summary>
        /// Default constructor initializing with default values
        /// </summary>
        public DisplayValueDouble(){}
        /// <summary>
        /// Constructor initializing with a double value
        /// </summary>
        /// <param name="value">Sets the value with the double passed</param>
        public DisplayValueDouble(double value)
        {
            Value = value;
        }
        /// <summary>
        /// The key and the value is the double value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double DoubleValue
        {
            get { return Convert.ToDouble(_Value, DisplayValue.numberFormat); }
            set { _Value = value; _Key = value; }
        }

        /// <summary>
        /// Returns a string representing the object
        /// </summary>
        /// <returns>The string value representing the object</returns>
        public override string ToString()
        {
            return DoubleValue.ToString(numberFormat);
        }
    }


    /// <summary>
    /// Tuple of value where an image can be used to display the value
    /// </summary>
    public class DisplayValueTranslate : DisplayValue
    {
        /// <summary>
        /// Image list associated with the values of the column
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Browsable(true), Description("Image list associated with the values of the column"), Category("Design")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImageList ImageList
        {
            get
            {
                if (ValueCollectionEditor.CurrentColumn != null)
                {
                    return ValueCollectionEditor.CurrentColumn.ImageList;
                }
                return null;
            }
            set
            {
                if (ValueCollectionEditor.CurrentColumn != null)
                {
                    ValueCollectionEditor.CurrentColumn.ImageList = value;
                }
            }
        }

        /// <summary>
        /// Image associated with the index value, if the imagelist is being used
        /// </summary>
        [Description("Image associated with the index value, if the imagelist is associated")]
        public Image Image
        {
            get
            {
                if (ValueCollectionEditor.CurrentColumn != null)
                {
                    DataGridViewExtendedColumn column = ValueCollectionEditor.CurrentColumn;
                    if (column != null && column.ImageList != null)
                    {
                        Image referencedImage = null;
                        string strValue = Value as string;
                        if (strValue != null)
                        {
                            referencedImage = column.ImageList.Images[strValue];
                            if (referencedImage == null)
                            {
                                int indexValue;
                                if (int.TryParse(strValue, out indexValue) && indexValue < column.ImageList.Images.Count)
                                {
                                    referencedImage = column.ImageList.Images[indexValue];
                                }
                            }
                        }
                        else if (Value is double)
                        {
                            double dValue = (double)Value;
                            if (dValue == Math.Truncate(dValue))
                            {
                                int indexValue = (int)Math.Truncate(dValue);
                                if (indexValue < column.ImageList.Images.Count)
                                {
                                    referencedImage = column.ImageList.Images[indexValue];
                                }
                            }
                        }
                        if (referencedImage != null)
                        {
                            return referencedImage;
                        }
                    }
                }
                return null;
            }
        }
    }

    /// <summary>
    /// Tuple of value where the pair is a string to a string value
    /// </summary>
    public class DisplayValueTranslateStringString : DisplayValueTranslate
    {
        /// <summary>
        /// Default constructor initializing with default values
        /// </summary>
        public DisplayValueTranslateStringString() { StringKey = string.Empty; StringValue = string.Empty; }
        /// <summary>
        /// Constructor initializing with a string key and a string value
        /// </summary>
        /// <param name="key">Sets the key with the string passed</param>
        /// <param name="value">Sets the value with the string passed</param>
        public DisplayValueTranslateStringString(string key, string value)
        {
            StringKey = key;
            StringValue = value;
        }
        /// <summary>
        /// The key is a double value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string StringKey
        {
            get { return Convert.ToString(_Key); }
            set { _Key = value; }
        }
        /// <summary>
        /// The value is a string value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string StringValue
        {
            get { return Convert.ToString(_Value); }
            set { _Value = value; }
        }

        /// <summary>
        /// Returns a string representing the object
        /// </summary>
        /// <returns>The string value representing the object</returns>
        public override string ToString()
        {
            return StringKey + " -> " + StringValue;
        }
    }


    /// <summary>
    /// Tuple of value where the pair is a double to a string value
    /// </summary>
    public class DisplayValueTranslateDoubleString : DisplayValueTranslate
    {
        /// <summary>
        /// Default constructor initializing with default values
        /// </summary>
        public DisplayValueTranslateDoubleString() { DoubleKey = 0.0; StringValue = string.Empty; }
        /// <summary>
        /// Constructor initializing with a double key and a string value
        /// </summary>
        /// <param name="key">Sets the key with the double passed</param>
        /// <param name="value">Sets the value with the string passed</param>
        public DisplayValueTranslateDoubleString(double key, string value)
        {
            DoubleKey = key;
            StringValue = value;
        }
        /// <summary>
        /// The key is a double value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double DoubleKey
        {
            get { return Convert.ToDouble(_Key); }
            set { _Key = value; }
        }
        /// <summary>
        /// The value is a string value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string StringValue
        {
            get { return Convert.ToString(_Value); }
            set { _Value = value; }
        }
        /// <summary>
        /// Returns a string representing the object
        /// </summary>
        /// <returns>The string value representing the object</returns>
        public override string ToString()
        {
            return DoubleKey.ToString() + " -> " + StringValue;
        }
    }

    /// <summary>
    /// Tuple of value where the pair is a string to a double value
    /// </summary>
    public class DisplayValueTranslateStringDouble : DisplayValueTranslate
    {
        /// <summary>
        /// Default constructor initializing with default values
        /// </summary>
        public DisplayValueTranslateStringDouble() { StringKey = string.Empty; DoubleValue = 0.0; }
        /// <summary>
        /// Constructor initializing with a string key and a double value
        /// </summary>
        /// <param name="key">Sets the key with the string passed</param>
        /// <param name="value">Sets the value with the double passed</param>
        public DisplayValueTranslateStringDouble(string key, double value)
        {
            StringKey = key;
            DoubleValue = value;
        }
        /// <summary>
        /// The key is a string value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string StringKey
        {
            get { return Convert.ToString(_Key); }
            set { _Key = value; }
        }
        /// <summary>
        /// The value is a double value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double DoubleValue
        {
            get { return Convert.ToDouble(_Value); }
            set { _Value = value; }
        }
        /// <summary>
        /// Returns a string representing the object
        /// </summary>
        /// <returns>The string value representing the object</returns>
        public override string ToString()
        {
            return StringKey + " -> " + DoubleValue;
        }
    }

    /// <summary>
    /// Tuple of value where the pair is a double to a double value
    /// </summary>
    public class DisplayValueTranslateDoubleDouble : DisplayValueTranslate
    {
        /// <summary>
        /// Default constructor initializing with default values
        /// </summary>
        public DisplayValueTranslateDoubleDouble() { DoubleKey = DoubleValue = 0.0; }
        /// <summary>
        /// Constructor initializing with a double key and a double value
        /// </summary>
        /// <param name="key">Sets the key with the double passed</param>
        /// <param name="value">Sets the value with the double passed</param>
        public DisplayValueTranslateDoubleDouble(double key, double value)
        {
            DoubleKey = key;
            DoubleValue = value;
        }
        /// <summary>
        /// The key is a double value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double DoubleKey
        {
            get { return Convert.ToDouble(_Key); }
            set { _Key = value; }
        }
        /// <summary>
        /// The value is a double value
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double DoubleValue
        {
            get { return Convert.ToDouble(_Value); }
            set { _Value = value; }
        }
        /// <summary>
        /// Returns a string representing the object
        /// </summary>
        /// <returns>The string value representing the object</returns>
        public override string ToString()
        {
            return DoubleKey.ToString() + " -> " + DoubleValue.ToString();
        }
    }


    /// <summary>
    /// Editor for the Value Collection
    /// </summary>
    public partial class ValueCollectionEditor : CollectionEditor
    {
        // A collection editor for the classes that inherits from DisplayValue

        private Type[] types;

        /// <summary>
        /// The constructor for the editor that gets the type of the current value
        /// </summary>
        /// <param name="type">Gets the type we are editing</param>
        public ValueCollectionEditor(Type type)
            : base(type)
        {
            
            types = new Type[] { typeof(DisplayValueString), typeof(DisplayValueDouble), typeof(DisplayValueTranslateDoubleDouble), typeof(DisplayValueTranslateDoubleString), typeof(DisplayValueTranslateStringDouble), typeof(DisplayValueTranslateStringString) };
        }

        /// <summary>
        /// Returns an array of the types that can be used to edit
        /// </summary>
        /// <returns>The array of types</returns>
        protected override Type[] CreateNewItemTypes()
        {
            //Debugger.Break();
            return types;
        }

        static private DataGridViewExtendedColumn _CurrentColumn;

        internal static DataGridViewExtendedColumn CurrentColumn
        {
            get { return ValueCollectionEditor._CurrentColumn; }
            set { ValueCollectionEditor._CurrentColumn = value; }
        }

        /// <summary>
        /// Edits the value of the specified object using the specified service provider and context. 
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <param name="provider">A service provider object through which editing services can be obtained. </param>
        /// <param name="value">The object to edit the value of.</param>
        /// <returns>The new value of the object. If the value of the object has not changed, this should return the same object it was passed. </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //Debugger.Break();
            //System.Windows.Forms.Design.DataGridViewColumnCollectionDialog.ListBoxItem a;
            if (context != null)
            {
                try
                {
                    CurrentColumn = null;
                    PropertyInfo pinfo = context.GetType().GetProperty("Instance");
                    object instance = pinfo.GetValue(context, null);
                    if (instance != null)
                    {
                        PropertyInfo pinfoColumn = instance.GetType().GetProperty("DataGridViewColumn");
                        CurrentColumn = pinfoColumn.GetValue(instance, null) as Artinsoft.Windows.Forms.DataGridViewExtendedColumn;
                    }
                }
                catch { }
            }
            return base.EditValue(context, provider, value);
        }
        /*
        protected override void CancelChanges()
        {
            base.CancelChanges();
        }

        protected override bool CanRemoveInstance(object value)
        {
            return base.CanRemoveInstance(value);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override bool CanSelectMultipleInstances()
        {
            return base.CanSelectMultipleInstances();
        }

        protected override CollectionForm CreateCollectionForm()
        {
            return base.CreateCollectionForm();
        }

        protected override Type CreateCollectionItemType()
        {
            return base.CreateCollectionItemType();
        }

        protected override object CreateInstance(Type itemType)
        {
            return base.CreateInstance(itemType);
        }

        protected override void DestroyInstance(object instance)
        {
            base.DestroyInstance(instance);
        }

        protected override string GetDisplayText(object value)
        {
            return base.GetDisplayText(value);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return base.GetEditStyle(context);
        }

        protected override object[] GetItems(object editValue)
        {
            return base.GetItems(editValue);
        }

        protected override IList GetObjectsFromInstance(object instance)
        {
            return base.GetObjectsFromInstance(instance);
        }

        public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
        {
            base.PaintValue(e);
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return base.GetPaintValueSupported(context);
        }

        protected override object SetItems(object editValue, object[] value)
        {
            return base.SetItems(editValue, value);
        }*/
    }

}