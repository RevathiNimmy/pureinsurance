using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Artinsoft.VB6.Gui
{
    /// <summary>
    /// Extender that adds support to special functionality in ComboBoxes and ListBoxes, 
    /// mainly related to ItemData.
    /// </summary>
    [ProvideProperty("ItemData", typeof(System.Windows.Forms.ListControl))]
    public partial class ListControlHelper : Component, IExtenderProvider, ISupportInitialize
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ListControlHelper()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container">The container where to add the controls.</param>
        public ListControlHelper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// Indicates if EndInit hasn't been executed yet after a BeginInit.
        /// </summary>
        private bool OnInitialization = false;

        /// <summary>
        /// Implements BeginInit Method from ISupportInitialize. 
        /// Sets ListControl status to OnInitialization.
        /// </summary>
        public void BeginInit()
        {
            OnInitialization = true;
        }

        /// <summary>
        /// Implements EndInit Method from ISupportInitialize. 
        /// Sets ListControl status to Not OnInitialization.
        /// </summary>
        public void EndInit()
        {
            OnInitialization = false;
            RefreshItemsData();
        }

        /// <summary>
        /// Updates the list of items data of the controls in runtime after the EndInit has been invoked.
        /// </summary>
        private void RefreshItemsData()
        {
            if (!DesignMode)
            {
                foreach (System.Windows.Forms.ListControl lstControl in ItemsData.Keys)
                {
                    if (lstControl is System.Windows.Forms.ComboBox)
                    {
                        for (int i = 0; (i < ((System.Windows.Forms.ComboBox)lstControl).Items.Count) && (i < ItemsData[lstControl].Length); i++)
                            Microsoft.VisualBasic.Compatibility.VB6.Support.SetItemData(lstControl, i, ItemsData[lstControl][i]);
                    }
                    else
                    {
                        for (int i = 0; (i < ((System.Windows.Forms.ListBox)lstControl).Items.Count) && (i < ItemsData[lstControl].Length); i++)
                            Microsoft.VisualBasic.Compatibility.VB6.Support.SetItemData(lstControl, i, ItemsData[lstControl][i]);
                    }
                }
                ItemsData.Clear();
            }
        }

        /// <summary>
        /// Stores the ItemsData for each control temporarely during design time.
        /// </summary>
        private Dictionary<System.Windows.Forms.ListControl, int[]> ItemsData = new Dictionary<System.Windows.Forms.ListControl, int[]>();

        /// <summary>
        /// Determinates which controls can use these extra properties.
        /// </summary>
        /// <param name="extender">The object to test.</param>
        /// <returns>True if the object can extend the properties.</returns>
        public bool CanExtend(object extender)
        {
            return (extender is System.Windows.Forms.ListControl);
        }

        /// <summary>
        /// Gets the ItemData property of a specific list control.
        /// </summary>
        /// <param name="lstControl">The list control to test.</param>
        /// <returns>Returns an int array with the item data list of the control.</returns>
        public int[] GetItemData(System.Windows.Forms.ListControl lstControl)
        {
            int[] res = new int[0];

            if (lstControl is System.Windows.Forms.ComboBox)
                res = GetItemData((System.Windows.Forms.ComboBox)lstControl);
            else
                res = GetItemData((System.Windows.Forms.ListBox)lstControl);

            return res;
        }

        /// <summary>
        /// Gets the ItemData property of a specific list control. 
        /// This specific function applies just for a ComboBox control.
        /// </summary>
        /// <param name="lstControl">The list control to test.</param>
        /// <returns>Returns an int array with the item data list of the control.</returns>
        private int[] GetItemData(System.Windows.Forms.ComboBox lstControl)
        {
            int[] res = new int[lstControl.Items.Count];

            //In design time we will keep the list of itemsData in a separate list 
            //so we don't mess with the VS.NET Designer to display the Items property
            if (DesignMode)
            {
                if (!ItemsData.ContainsKey(lstControl))
                {
                    for (int i = 0; i < lstControl.Items.Count; i++)
                        res[i] = Microsoft.VisualBasic.Compatibility.VB6.Support.GetItemData(lstControl, i);

                    ItemsData.Add(lstControl, res);
                }
                else
                {
                    if (lstControl.Items.Count != ItemsData[lstControl].Length)
                    {
                        for (int i = 0; (i < lstControl.Items.Count) && (i < ItemsData[lstControl].Length); i++)
                            res[i] = ItemsData[lstControl][i];

                        ItemsData[lstControl] = res;
                    }
                    else
                        res = ItemsData[lstControl];
                }
            }
            else
            {
                for (int i = 0; i < lstControl.Items.Count; i++)
                    res[i] = Microsoft.VisualBasic.Compatibility.VB6.Support.GetItemData(lstControl, i);
            }

            return res;
        }

        /// <summary>
        /// Gets the ItemData property of a specific list control. 
        /// This specific function applies just for a ListBox control.
        /// </summary>
        /// <param name="lstControl">The list control to test.</param>
        /// <returns>Returns an int array with the item data list of the control.</returns>
        private int[] GetItemData(System.Windows.Forms.ListBox lstControl)
        {
            int[] res = new int[lstControl.Items.Count];

            //In design time we will keep the list of itemsData in a separate list so 
            //we don't mess with the VS.NET Designer to display the Items property
            if (DesignMode)
            {
                if (!ItemsData.ContainsKey(lstControl))
                {
                    for (int i = 0; i < lstControl.Items.Count; i++)
                        res[i] = Microsoft.VisualBasic.Compatibility.VB6.Support.GetItemData(lstControl, i);

                    ItemsData.Add(lstControl, res);
                }
                else
                {
                    if (lstControl.Items.Count != ItemsData[lstControl].Length)
                    {
                        for (int i = 0; (i < lstControl.Items.Count) && (i < ItemsData[lstControl].Length); i++)
                            res[i] = ItemsData[lstControl][i];

                        ItemsData[lstControl] = res;
                    }
                    else
                        res = ItemsData[lstControl];
                }
            }
            else
            {
                for (int i = 0; i < lstControl.Items.Count; i++)
                    res[i] = Microsoft.VisualBasic.Compatibility.VB6.Support.GetItemData(lstControl, i);
            }

            return res;
        }

        /// <summary>
        /// Sets the ItemData property of a specific list control.
        /// </summary>
        /// <param name="lstControl">The list control.</param>
        /// <param name="itemsData">The Item data list to set.</param>
        public void SetItemData(System.Windows.Forms.ListControl lstControl, int[] itemsData)
        {
            if (lstControl is System.Windows.Forms.ComboBox)
                SetItemData((System.Windows.Forms.ComboBox)lstControl, itemsData);
            else
                SetItemData((System.Windows.Forms.ListBox)lstControl, itemsData);
        }

        /// <summary>
        /// Sets the ItemData property of a specific list control.
        /// This specific function applies just for a ComboBox control.
        /// </summary>
        /// <param name="lstControl">The list control.</param>
        /// <param name="itemsData">The Item data list to set.</param>
        private void SetItemData(System.Windows.Forms.ComboBox lstControl, int[] itemsData)
        {
            int[] items = new int[lstControl.Items.Count];
            if (itemsData != null)
            {
                if (DesignMode || OnInitialization)
                {
                    if (!OnInitialization)
                    {
                        for (int i = 0; (i < lstControl.Items.Count) && (i < itemsData.Length); i++)
                            items[i] = itemsData[i];
                    }
                    else
                        items = itemsData;

                    if (!ItemsData.ContainsKey(lstControl))
                        ItemsData.Add(lstControl, items);
                    else
                        ItemsData[lstControl] = items;
                }
                else
                {
                    for (int i = 0; (i < lstControl.Items.Count) && (i < itemsData.Length); i++)
                        Microsoft.VisualBasic.Compatibility.VB6.Support.SetItemData(lstControl, i, itemsData[i]);
                }
            }
        }

        /// <summary>
        /// Sets the ItemData property of a specific list control.
        /// This specific function applies just for a ListBox control.
        /// </summary>
        /// <param name="lstControl">The list control.</param>
        /// <param name="itemsData">The Item data list to set.</param>
        private void SetItemData(System.Windows.Forms.ListBox lstControl, int[] itemsData)
        {
            int[] items = new int[lstControl.Items.Count];
            if (itemsData != null)
            {
                if (DesignMode || OnInitialization)
                {
                    if (!OnInitialization)
                    {
                        for (int i = 0; (i < lstControl.Items.Count) && (i < itemsData.Length); i++)
                            items[i] = itemsData[i];
                    }
                    else
                        items = itemsData;

                    if (!ItemsData.ContainsKey(lstControl))
                        ItemsData.Add(lstControl, items);
                    else
                        ItemsData[lstControl] = items;
                }
                else
                {

                    for (int i = 0; (i < lstControl.Items.Count) && (i < itemsData.Length); i++)
                        Microsoft.VisualBasic.Compatibility.VB6.Support.SetItemData(lstControl, i, itemsData[i]);
                }
            }
        }
    }
}
