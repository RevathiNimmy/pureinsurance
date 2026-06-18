using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// VB.NET source code formatting methods. Use these to write values out as VB.NET literal constants.
    /// </summary>
    public static class VBCodeFormat {

        #region Private Constants

        private const String VBCodeNull = "Nothing";

        #endregion

        #region Public Shared Methods - ToString for basic types

        /// <overloads>
        /// Format an object as a VB.NET code expression.
        /// </overloads>
        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Boolean value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Byte value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Int16 value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Int32 value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Int64 value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Single value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Double value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Decimal value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(DateTime value) {
            // Return value.ToString("\#M/d/yyyy HH:mm:ss\#")
            return String.Format(CultureInfo.InvariantCulture, "New DateTime({0}, {1}, {2}, {3}, {4}, {5}, {6}, System.DateTimeKind.{7})", value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, value.Kind);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(TimeSpan value) {
            return String.Format(CultureInfo.InvariantCulture, "New TimeSpan({0})", value.Ticks);
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(DateTimeOffset value) {
            return String.Format(CultureInfo.InvariantCulture, "New DateTimeOffset({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond, ToString(value.Offset));
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Guid value) {
            return String.Format(@"New Guid(""{0}"")", value.ToString("B", CultureInfo.InvariantCulture).ToUpperInvariant());
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Byte[] value) {
            if(value == null) {
                return VBCodeNull;
            } else {
                StringBuilder text = new StringBuilder("New Byte() {");
                Separator comma = new Separator(", ");
                foreach(Byte valueElement in value) {
                    text.Append(comma);
                    text.Append("&H");
                    text.Append(valueElement.ToString("X2", CultureInfo.InvariantCulture));
                }
                text.Append("}");
                return text.ToString();
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(String value) {
            if(value == null) {
                return VBCodeNull;
            } else {
                return @"""" + value.Replace(@"""", @"""""").Replace("\r\n", @""" & vbCrLf & """).Replace("\r", @""" & vbCr & """).Replace("\n", @""" & vbLf & """).Replace("\t", @""" & vbTab & """) + @"""";
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <typeparam name="T">The enum type to format.</typeparam>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString<T>(T value) where T : struct {
            if(!typeof(T).IsEnum) {
                throw new ArgumentException("Enum type required.");
            } else if(Enum.IsDefined(typeof(T), value)) {
                return typeof(T).Name + "." + value.ToString();
            } else {
                return Convert.ToInt64(value).ToString();
            }
        }

        #endregion

        #region Public Shared Methods - ToString for nullable basic types

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Boolean? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Byte? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Int16? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Int32? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Int64? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Single? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Double? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Decimal? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(DateTime? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(TimeSpan? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString(Guid? value) {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        /// <summary>
        /// Format an object as a VB.NET code expression.
        /// </summary>
        /// <typeparam name="T">The enum type to format.</typeparam>
        /// <param name="value">The value to format.</param>
        /// <returns>A VB.NET code expression representing the object.</returns>
        public static String ToString<T>(T? value) where T : struct {
            if(!value.HasValue) {
                return VBCodeNull;
            } else {
                return ToString(value.Value);
            }
        }

        #endregion

        #region Public Shared Methods - ToString for collections

        /// <summary>
        /// Format a collection in a standard way, using VB.NET-style syntax.
        /// Each item is formatted using a delegate rather than its own <c>ToString</c> method, to allow
        /// custom formatting of objects whose source code is not under your control.
        /// </summary>
        /// <typeparam name="T">The type of object to format.</typeparam>
        /// <param name="value">The collection of objects to format.</param>
        /// <param name="itemToString">The custom formatting method to apply to each item. If <see langword="null"/>, the object's own <c>ToString</c> method is called instead.</param>
        /// <param name="includeItemType">Include the name of each item type in the output?</param>
        /// <returns>A String representation of the whole collection.</returns>
        public static String ToString<T>(ICollection value, Converter<T, string> itemToString, Boolean includeItemType) {

            // Get the trivial case out of the way first.
            if(value.Count == 0) {
                return "{}";
            }

            StringBuilder text = new StringBuilder();
            text.AppendLine("{");

            foreach(T item in value) {
                if(includeItemType) {
                    text.Append(item.GetType().Name).Append(": ");
                }
                if(itemToString == null) {
                    text.Append(item.ToString().TrimEnd());
                } else {
                    text.Append(itemToString(item).TrimEnd());
                }
                text.AppendLine();
            }

            text.Append("}");
            return text.ToString();
        }

        #endregion
    }
}
