using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Artinsoft.VB6.Utils
{
    ///<summary>
    ///This simulates the XarrayDbObject funcionality based on DataTable class.
    ///</summary>
    ///<remarks>
    ///This class only supports two-dimensional arrays. Multi-dimensional arrays are not supported.
    ///</remarks>
    public class XArrayHelper : DataTable
    {
        ///<summary>
        /// Stores the LowerBounds to handle indexes.
        ///</summary>
        private int[] DimensionLowerBounds = null;

        ///<summary>
        /// Stores the lenghts to handle indexes.
        ///</summary>
        private int[] DimensionLengths = null;

        ///<summary>
        /// Constructor for the XArrayHelper.
        ///</summary>
        public XArrayHelper()
        {
            DimensionLowerBounds = null;
            DimensionLengths = null;
        }

        ///<summary>
        ///This function is a Factory to create Xarray instances. 
        ///</summary>
        ///<param name="Lengths">The length of each dimension.</param>
        ///<param name="LowerBounds">The lower bounds to use for each dimension.</param>
        ///<returns>A new XArrayHelper instance.</returns>
        public static XArrayHelper CreateInstanceXarray(int[] Lengths, int[] LowerBounds)
        {
            XArrayHelper xarr = new XArrayHelper();

            xarr.DimensionLengths = Lengths;
            xarr.DimensionLowerBounds = LowerBounds;
            for (int col = 0; col <= Lengths[1]; col++)
            {
                xarr.Columns.Add(new DataColumn(null,typeof(object)));
            }

            for (int i = 0; i <= Lengths[0]; i++)
            {
                DataRow row = xarr.NewRow();
                xarr.Rows.Add(row);
            }
            return xarr;
        }

        ///<summary>
        ///This function redimensions a Xarray instance.
        ///</summary>
        ///<param name="Lengths">The length of each dimension.</param>
        ///<param name="LowerBounds">The lower bounds to use for each dimension.</param>
        ///<returns>It returns a redimensioned instance of itself.</returns>
        ///<remarks></remarks>
        public XArrayHelper RedimXArray(int[] Lengths, int[] LowerBounds)
        {

            DimensionLengths = Lengths;
            DimensionLowerBounds = LowerBounds;

            if (this.Columns.Count == 0)
            {
                for (int colIndex = 0; colIndex <= Lengths[1]; colIndex++)
                {
                    this.Columns.Add(new DataColumn(null,typeof(object)));
                }
            }
            else if (this.Columns.Count < (Lengths[1] + 1))
            {
                for (int colIndex = this.Columns.Count; colIndex <= Lengths[1]; colIndex++)
                {
                    this.Columns.Add(new DataColumn(null,typeof(object)));
                }
            }
            else if (this.Columns.Count > (Lengths[1] + 1))
            {
                for (int colIndex = Lengths[1] + 1; colIndex <= this.Columns.Count - 1; colIndex++)
                {
                    this.Columns.RemoveAt(colIndex);
                }
            }

            if (this.Rows.Count == 0)
            {
                for (int rowIndex = 0; rowIndex <= Lengths[0]; rowIndex++)
                {
                    DataRow row = this.NewRow();
                    this.Rows.Add(row);
                }
            }
            else if (this.Rows.Count < (Lengths[0] + 1))
            {
                for (int rowIndex = this.Rows.Count; rowIndex <= Lengths[0]; rowIndex++)
                {
                    DataRow row = this.NewRow();
                    this.Rows.Add(row);
                }
            }
            else if (this.Rows.Count > (Lengths[0] + 1))
            {
                for (int rowIndex = Lengths[0] + 1; rowIndex <= this.Rows.Count - 1; rowIndex++)
                {
                    this.Rows.RemoveAt(rowIndex);
                }
            }
            return this;

        }

        ///<summary>
        ///Gets the upper bound of the specified dimension.
        ///</summary>
        ///<param name="Dimension">A zero-based dimension whose upper bound needs to be determined.</param>
        ///<returns>The upper bound for the specificed dimension.</returns>
        public int GetUpperBound(int Dimension)
        {
            return DimensionLengths[Dimension];
        }

        ///<summary>
        ///Gets the Lower bound of the specified dimension.
        ///</summary>
        ///<param name="Dimension">A zero-based dimension whose lower bound needs to be determined.</param>
        ///<returns>The lower bound for the specificed dimension.</returns>
        public int GetLowerBound(int Dimension)
        {
            return DimensionLowerBounds[Dimension];
        }

        ///<summary>
        ///Gets the number of elements in the specified dimension.
        ///</summary>
        ///<param name="Dimension">A zero-based dimension whose length needs to be determined.</param>
        ///<returns>The length of elements of the specified dimension.</returns>
        public int GetLength(int Dimension)
        {
            return DimensionLengths[Dimension];
        }

        ///<summary>
        ///Returns the element at the specified row and column.
        ///</summary>
        ///<param name="row">Row index where the element is located.</param>
        ///<param name="column">Column index where the element is located.</param>
        ///<value>Value for the specified element.</value>
        ///<returns>The element at the specified index.</returns>
        public Object this[int row, int column]
        {
            get
            {
                return GetValue(row, column);
            }
            set
            {
                SetValue(value, row, column);
            }
        }

        ///<summary>
        ///Gets the value at the specified position.
        ///</summary>
        ///<param name="row">Index row where the element is located.</param>
        ///<param name="column">Index column where the element is located.</param>
        ///<returns>The value at the specified position.</returns>
        public Object GetValue(int row, int column)
        {
            return this.Rows[row - this.DimensionLowerBounds[0]][column - this.DimensionLowerBounds[1]];
        }

        ///<summary>
        ///Sets a value to the element at the specified position.
        ///</summary>
        ///<param name="value">The new value for the specified element.</param>
        ///<param name="row">Index row where the element is located.</param>
        ///<param name="column">Index column where the element is located.</param>
        public void SetValue(Object value, int row, int column)
        {
            this.Rows[row - this.DimensionLowerBounds[0]][column - this.DimensionLowerBounds[1]] = value;
        }

        ///<summary>
        ///Clears a range of elements in the XArrayHelper.
        ///</summary>
        ///<param name="arr">XArrayHelper whose elements need to be cleared.</param>
        ///<param name="index">The starting index of the range of elements.</param>
        ///<param name="length">The number of elements to be cleared.</param>
        public static void Clear(XArrayHelper arr, int index, int length)
        {

            int realIndexi = arr.GetLowerBound(0);
            int realIndexj = arr.GetLowerBound(1);

            index = index - arr.GetLowerBound(0);

            while (index > 0)
            {
                if (index > arr.GetUpperBound(1))
                {
                    realIndexi = realIndexi + 1;
                    index = index - arr.GetLength(1);
                }
                else
                {
                    realIndexj = realIndexj + index;
                    index = 0;
                }
            }

            for (int j = realIndexj; j <= arr.GetUpperBound(1); j++)
            {
                if (length < 0) return;
                arr[realIndexi, j] = null;
                length = length - 1;
            }

            realIndexi = realIndexi + 1;

            for (int i = realIndexi; i <= arr.GetUpperBound(0); i++)
            {
                for (int j = arr.GetLowerBound(1); j <= arr.GetUpperBound(1); j++)
                {
                    if (length < 1) return;
                    arr[i, j] = null;
                    length = length - 1;
                }
            }
        }


        ///<summary>
        ///Creates a cleared a XArrayHelper.
        ///</summary>
        ///<param name="arr">XArrayHelper whose elements need to be cleared.</param>
        public void Clear(ref XArrayHelper arr)
        {
            int[] length = new int[] { 1, 0 };
            int[] lowerB = new int[] { arr.DimensionLowerBounds[0], arr.DimensionLowerBounds[1] };
            this.Clear();
            arr.RedimXArray(length, lowerB);
        }

        ///<summary>
        ///Adds a new row to the current instance of XArrayHelper.
        ///</summary>
        public void AppendRows()
        {
            int[] length = new int[] { this.DimensionLengths[0] + 1, this.DimensionLengths[1] };
            int[] lowerB = new int[] { this.DimensionLowerBounds[0], this.DimensionLowerBounds[1] };
            this.RedimXArray(length, lowerB);
        }

        ///<summary>
        ///Adds a new row to the current instance of XArrayHelper and sets a value to the specified
        ///row and column.
        ///</summary>
        ///<param name="value">The value to be set the specified position.</param>
        ///<param name="row">The row in the XArrayHelper where to be set the value.</param>
        ///<param name="column">The column in the XArrayHelper where to be set the value.</param>
        public void AppendRows(Object value, int row, int column)
        {
            int[] length = new int[] { this.DimensionLengths[0] + 1, this.DimensionLengths[1] };
            int[] lowerB = new int[] { this.DimensionLowerBounds[0], this.DimensionLowerBounds[1] };
            this.RedimXArray(length, lowerB);

            this.Rows[row - this.DimensionLowerBounds[0]][column - this.DimensionLowerBounds[1]] = value;
        }

        ///<summary>
        ///Deletes a row in the specified position and redimensions the XArrayHelper.
        ///</summary>
        ///<param name="row">The row in the XArrayHelper to be deleted.</param>
        ///<returns>number of rows that were successfully deleted</returns>
        public int DeleteRows(int row)
        {
            int result = 0;
            if (row >= 0 && row < this.Rows.Count)
            {
                this.Rows[row - this.DimensionLowerBounds[0]].Delete();
                int[] length = new int[] { this.DimensionLengths[0] - 1, this.DimensionLengths[1] };
                int[] lowerB = new int[] { this.DimensionLowerBounds[0], this.DimensionLowerBounds[1] };
                this.RedimXArray(length, lowerB);
                result = 1;
            }
            return result;
        }

        /// <summary>
        /// Deletes the amount of rows specified by count starting from startRow
        /// </summary>
        /// <param name="startRow">Row Index for the first row that must be deleted</param>
        /// <param name="count">Amount of rows to delete</param>
        /// <returns>number of rows that were successfully deleted</returns>
        public int DeleteRows(int startRow, int count)
        {
            int result = 0;
            if (count > 0)
            {
                for (int i = startRow; i < startRow + count; i++)
                {
                    DeleteRows(startRow);
                    result++;
                }
            }
            return result;
        }

        ///<summary>
        ///Creates a XArrayHelper and copies the values from an object array.
        ///</summary>
        ///<param name="array">The source array to be copied.</param>
        public void LoadRows(Object[,] array)
        {
            this.RedimXArray(new int[] { array.GetUpperBound(0), array.GetUpperBound(1) }, new int[] { array.GetLowerBound(0), array.GetLowerBound(1) });
            for (int row = array.GetLowerBound(0); row <= array.GetUpperBound(0); row++)
            {
                for (int col = array.GetLowerBound(1); col <= array.GetUpperBound(1); col++)
                {
                    this.SetValue(array[row, col], row, col);
                }
            }
        }

        ///<summary>
        ///Creates a XArrayHelper and copies the values from a XArrayHelper.
        ///</summary>
        ///<param name="table">The source XArrayHelper to be copied.</param>
        public void LoadRows(XArrayHelper table)
        {
            this.RedimXArray(new int[] { table.GetUpperBound(0), table.GetUpperBound(1) }, new int[] { table.GetLowerBound(0), table.GetLowerBound(1) });
            for (int row = table.GetLowerBound(0); row <= table.GetUpperBound(0); row++)
            {
                for (int col = table.GetLowerBound(1); col <= table.GetUpperBound(1); col++)
                {
                    this.SetValue(table.GetValue(row, col), row, col);
                }
            }
        }

        ///<summary>
        ///Finds a value into a XArrayHelper.
        ///</summary>
        ///<param name="value">The value to be found.</param>
        ///<returns>True if the value is found into the XArrayHelper.</returns>
        public Object Find(Object value)
        {
            Boolean result = false;
            for (int row = this.GetLowerBound(0); row <= this.GetUpperBound(0); row++)
            {
                for (int col = this.GetLowerBound(1); col <= this.GetUpperBound(1); col++)
                {
                    if (this.GetValue(row, col) == value)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        ///<summary>
        ///Finds a value into a XArrayHelper from a specified position.
        ///</summary>
        ///<param name="value">The value to be found.</param>
        ///<param name="lowerBound">The lowerbound where to start searching.</param>
        ///<param name="upperBound">The upperbound where to finish searching.</param>
        ///<returns>The index where the values is found or -1 if it is not found.</returns>
        public Object Find(Object value, int lowerBound, int upperBound)
        {
            long index = -1;
            for (int row = lowerBound; row <= this.GetUpperBound(0); row++)
            {
                for (int col = upperBound; col <= this.GetUpperBound(1); col++)
                {
                    if (this.GetValue(row, col) == value)
                    {
                        index = row;
                        break;
                    }
                }
            }
            return index;
        }

        /// <summary>
        /// The Find method searches for a specfied value within a column of an XArrayDB object, 
        /// starting at a particular row. If a match is found, this method returns the corresponding 
        /// row index; otherwise, the value returned is equal to: LowerBound(1) - 1
        /// </summary>
        /// <param name="row">row index for the first row on which the value must be searched for</param>
        /// <param name="column">index for the column on which to search for the value</param>
        /// <param name="value">value that is being searched for</param>
        /// <returns>index for the first row containing the given value</returns>
        public int Find(int row, int column, Object value)
        {
            int index = this.GetLowerBound(1) - 1;
            if ((row >= 0 && row < this.Rows.Count) 
                && (column >= 0 && column < this.Columns.Count))
            {
                for (int rowIndex = row; rowIndex < this.Rows.Count; rowIndex++)
                {
                    if (this.Rows[rowIndex][column].Equals(value))
                    {
                        return rowIndex;
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// Enum that holds all the different types of data used to sort a column
        /// </summary>
        public enum XArrayColumnTypes
        {
            /// <summary>
            /// Boolean data type
            /// </summary>
            Boolean,
            /// <summary>
            /// Byte data type
            /// </summary>
            Byte,
            /// <summary>
            /// Currency data type
            /// </summary>
            Currency,
            /// <summary>
            /// Date data type
            /// </summary>
            Date,
            /// <summary>
            /// Default data type
            /// </summary>
            Default,
            /// <summary>
            /// Double data type
            /// </summary>
            Double,
            /// <summary>
            /// Integer data type
            /// </summary>
            Integer,
            /// <summary>
            /// Long data type
            /// </summary>
            Long,
            /// <summary>
            /// Number data type
            /// </summary>
            Number,
            /// <summary>
            /// Single data type
            /// </summary>
            Single,
            /// <summary>
            /// String data type
            /// </summary>
            String,
            /// <summary>
            /// StringCaseSensitive data type
            /// </summary>
            StringCaseSensitive
        }

        /// <summary>
        /// Enum containing possible ordering values
        /// </summary>
        public enum XArraySortOrder
        {
            /// <summary>
            /// Ascending order
            /// </summary>
            ASCENDING = 1,
            /// <summary>
            /// Descending order
            /// </summary>
            DESCENDING = -1
        }


        class DataTableComparer : IComparer<DataRow>
        {
            ColumnComparer[] comparers = new ColumnComparer[0];
            /// <summary>
            /// Create data table comparer
            /// </summary>
            public DataTableComparer(params int[] settings)
            {
                for (int i = 0; i < settings.Length; i++)
                {
                    int columnIndex = settings[i];
                    //TODO index checking, default values, etc
                    int sortOrder = settings[i + 1];
                    int columnType = settings[i + 2];
                    i += 2;
                    int oldLength = comparers.Length;
                    Array.Resize<ColumnComparer>(ref comparers, oldLength + 1);
                    comparers[oldLength] = new ColumnComparer((XArrayColumnTypes)columnType, columnIndex, (XArraySortOrder)sortOrder);
                } // for

            } // DataTableComparer(settings)
            /// <summary>
            /// Column comparer
            /// </summary>

            public class ColumnComparer : IComparer<DataRow>
            {
                XArrayColumnTypes myType;
                int columnIndex;
                int sortOrder = 1;

                public ColumnComparer(XArrayColumnTypes myType, int columnIndex, XArraySortOrder sortOrder)
                {
                    this.myType = myType;
                    this.columnIndex = columnIndex;
                    this.sortOrder = (int)sortOrder;
                } // ColumnComparer(type)

                #region IComparer<DataColumn> Members

                public int Compare(DataRow x, DataRow y)
                {
                    int res = 0;
                    //WARNING: Conversion might not work as it does in VB6 for all cases
                    //consider using GetValueForcedToType function if more conmplex conversions
                    //are needed
                    switch (myType)
                    {
                        case XArrayColumnTypes.Boolean:
                            res = 0;
                            bool boolValue1 = Convert.ToBoolean(x[columnIndex]);
                            bool boolValue2 = Convert.ToBoolean(y[columnIndex]);
                            if (boolValue1 != boolValue2)
                            {
                                if (boolValue1)
                                    res = 1;
                                else
                                    res = -1;
                            }
                            break;
                        case XArrayColumnTypes.Byte:
                            res = 0;
                            byte xByte = Convert.ToByte(x[columnIndex]);
                            byte yByte = Convert.ToByte(y[columnIndex]);
                            if (xByte < yByte)
                                res = -1;
                            else if (xByte > yByte)
                            {
                                res = 1;
                            }
                            break;
                        case XArrayColumnTypes.Currency:
                            res = 0;
                            Decimal xDecimal = Convert.ToDecimal(x[columnIndex]);
                            Decimal yDecimal = Convert.ToDecimal(y[columnIndex]);
                            if (xDecimal < yDecimal)
                                res = -1;
                            else if (xDecimal > yDecimal)
                            {
                                res = 1;
                            }
                            break;
                        case XArrayColumnTypes.Date:
                            DateTime xDate = DateTime.MinValue;
                            DateTime yDate = DateTime.MinValue;
                            if (x[columnIndex] != DBNull.Value)
                                xDate = Convert.ToDateTime(x[columnIndex]);
                            if (y[columnIndex] != DBNull.Value)
                                yDate = Convert.ToDateTime(y[columnIndex]);
                            res = DateTime.Compare(xDate, yDate);
                            break;
                        //TODO: Add Support for Default
                        case XArrayColumnTypes.Default:
                            break;
                        case XArrayColumnTypes.Double:
                            res = 0;
                            double xDouble = Convert.ToDouble(x[columnIndex]);
                            double yDouble = Convert.ToDouble(y[columnIndex]);
                            if (xDouble < yDouble)
                                res = -1;
                            else if (xDouble > yDouble)
                            {
                                res = 1;
                            }
                            break;
                        case XArrayColumnTypes.Integer:
                            res = 0;
                            int xInteger = Convert.ToInt32(x[columnIndex]);
                            int yInteger = Convert.ToInt32(y[columnIndex]);
                            if (xInteger < yInteger)
                                res = -1;
                            else if (xInteger > yInteger)
                            {
                                res = 1;
                            }
                            break;
                        case XArrayColumnTypes.Long:
                            res = 0;
                            long xLong = Convert.ToInt64(x[columnIndex]);
                            long yLong = Convert.ToInt64(y[columnIndex]);
                            if (xLong < yLong)
                                res = -1;
                            else if (xLong > yLong)
                            {
                                res = 1;
                            }
                            break;
                        //TODO: Add Support for Number
                        case XArrayColumnTypes.Number:
                            break;
                        case XArrayColumnTypes.Single:
                            res = 0;
                            Single xSingle = Convert.ToSingle(x[columnIndex]);
                            Single ySingle = Convert.ToSingle(y[columnIndex]);
                            if (xSingle < ySingle)
                                res = -1;
                            else if (xSingle > ySingle)
                            {
                                res = 1;
                            }
                            break;
                        case XArrayColumnTypes.String:
                            res = String.Compare(Convert.ToString(x[columnIndex]), Convert.ToString(y[columnIndex]));
                            break;
                        case XArrayColumnTypes.StringCaseSensitive:
                            res = String.Compare(Convert.ToString(x[columnIndex]), Convert.ToString(y[columnIndex]), false);
                            break;
                        
                    } // switch
                    res = res * sortOrder;
                    return res;
                }


                #endregion

            } // class ColumnComparer
            #region IComparer<DataRow> Members
            public int Compare(DataRow x, DataRow y)
            {
                int res = 0;
                foreach (ColumnComparer comparer in comparers)
                {
                    res = comparer.Compare(x, y);
                    if (res != 0)
                        return res;
                } // foreach
                return res;
            }

            #endregion

        } // class DataTableComparer



        /// <summary>
        /// -
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <param name="restParams"></param>
        public void QuickSort(int startRow, int endRow, params int[] restParams)
        {
            int length = endRow - startRow + 1;
            DataRow[] rows = Select();
            Array.Sort<DataRow>(rows, startRow, length, new DataTableComparer(restParams));
            int i = 0;
            object[] data = new object[length];
            //Put the pointer to the ItemArray in a temp array
            for (int z = startRow; z <= endRow; z++)
            {
                data[i++] = rows[z].ItemArray;
            } // foreach
            //Modify the DataTable
            for (int y = startRow; y <= endRow; y++)
            {
                Rows[y].ItemArray = data[y - startRow] as object[];
            } // foreach
            //enableEvents = true;
        }
    }
}

