using Microsoft.Win32;
using Sirius.Architecture.Utility;
using System;

namespace Sirius.Architecture.Configuration.Local {

    /// <summary>
    /// This class reads strongly-typed values from the Windows registry on the local machine.
    /// </summary>
    internal static class SwiftRegistryRead {

        #region Public Methods - GetValue

        public static Boolean GetValueAsBoolean(RegistryKey key, string name, Boolean defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is Int32) {
                // Correct REG_DWORD format.
                return ((Int32) value) != 0;
            } else if(value is Int64) {
                // Also interpret REG_QWORD format.
                return ((Int64) value) != 0;
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Also interpret REG_SZ format written by hand.
                    return SwiftLPFConvert.ToBoolean((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Byte GetValueAsByte(RegistryKey key, string name, Byte defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is Int32) {
                // Correct REG_DWORD format.
                return Convert.ToByte(value);
            } else if(value is Int64) {
                // Also interpret REG_QWORD format.
                return Convert.ToByte(value);
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Also interpret REG_SZ format written by hand.
                    return SwiftLPFConvert.ToByte((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Int16 GetValueAsInt16(RegistryKey key, string name, Int16 defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is Int32) {
                // Correct REG_DWORD format.
                return Convert.ToInt16(value);
            } else if(value is Int64) {
                // Also interpret REG_QWORD format.
                return Convert.ToInt16(value);
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Also interpret REG_SZ format written by hand.
                    return SwiftLPFConvert.ToInt16((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Int32 GetValueAsInt32(RegistryKey key, string name, Int32 defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is Int32) {
                // Correct REG_DWORD format.
                return Convert.ToInt32(value);
            } else if(value is Int64) {
                // Also interpret REG_QWORD format.
                return Convert.ToInt32(value);
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Also interpret REG_SZ format written by hand.
                    return SwiftLPFConvert.ToInt32((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Int64 GetValueAsInt64(RegistryKey key, string name, Int64 defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is Int32) {
                // Also interpret REG_DWORD format.
                return Convert.ToInt64(value);
            } else if(value is Int64) {
                // Correct REG_QWORD format.
                return Convert.ToInt64(value);
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Also interpret REG_SZ format written by hand.
                    return SwiftLPFConvert.ToInt64((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Single GetValueAsSingle(RegistryKey key, string name, Single defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Correct REG_SZ format written by Swift code.
                    return SwiftLPFConvert.ToSingle((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Double GetValueAsDouble(RegistryKey key, string name, Double defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Correct REG_SZ format written by Swift code.
                    return SwiftLPFConvert.ToDouble((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Decimal GetValueAsDecimal(RegistryKey key, string name, Decimal defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Correct REG_SZ format written by Swift code.
                    return SwiftLPFConvert.ToDecimal((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static DateTime GetValueAsDateTime(RegistryKey key, string name, DateTime defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Correct REG_SZ format written by Swift code.
                    return SwiftLPFConvert.ToDateTime((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static TimeSpan GetValueAsTimeSpan(RegistryKey key, string name, TimeSpan defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Correct REG_SZ format written by Swift code.
                    return SwiftLPFConvert.ToTimeSpan((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Guid GetValueAsGuid(RegistryKey key, string name, Guid defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is byte[]) {
                // Correct REG_BINARY format.
                return new Guid((byte[]) value);
            } else if(value is string) {
                if(((string) value).Length == 0) {
                    // Assume blank string means null.
                    return defaultValue;
                } else {
                    // Also interpret REG_SZ format for safety.
                    return SwiftLPFConvert.ToGuid((string) value);
                }
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static Byte[] GetValueAsByteArray(RegistryKey key, string name, Byte[] defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is byte[]) {
                // Correct REG_BINARY format.
                return (byte[]) value;
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        public static String GetValueAsString(RegistryKey key, string name, String defaultValue) {

            if(key == null) {
                // If key doesn't exist, return the default.
                return defaultValue;
            }
            object value = key.GetValue(name);
            if(value == null) {
                // If value doesn't exist, return the default.
                return defaultValue;
            } else if(value is string) {
                // Correct REG_SZ format.
                return ((string) value);
            } else {
                throw new FormatException(Properties.Resources.RegistryReadFormatException);
            }

        }

        #endregion

    }

}
