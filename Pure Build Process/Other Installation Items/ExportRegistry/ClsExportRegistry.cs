using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace Export_Registry
{
    public class Main1
    {
        public string connectionString = null;
        public void GetAndSaveRegistryData()
        {
            // Example root key to read from
            string rootKeyPath = @"SOFTWARE\Pure";
            RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(rootKeyPath);

            if (baseKey != null)
            {
                TraverseRegistryTree(baseKey, rootKeyPath, false);
                baseKey.Close();
            }
            RegistryKey userBaseKey = Registry.CurrentUser.OpenSubKey(rootKeyPath);

            if (userBaseKey != null)
            {
                TraverseRegistryTree(userBaseKey, rootKeyPath, true);
                userBaseKey.Close();
            }
            Console.WriteLine("Done exporting registry.");
        }
        void TraverseRegistryTree(RegistryKey parentKey, string currentPath, bool isUser)
        {

            // Read values under current key
            foreach (var valueName in parentKey.GetValueNames())
            {
                object value = parentKey.GetValue(valueName);
                RegistryValueKind valueType = parentKey.GetValueKind(valueName);

                SaveToDatabase(currentPath, valueName, valueType.ToString(), value?.ToString(), isUser ? Environment.UserName : string.Empty);
            }

            // Recurse into subkeys
            foreach (string subKeyName in parentKey.GetSubKeyNames())
            {
                try
                {
                    using (RegistryKey subKey = parentKey.OpenSubKey(subKeyName))
                    {
                        if (subKey != null)
                        {
                            string fullPath = currentPath + "\\" + subKeyName;
                            TraverseRegistryTree(subKey, fullPath, isUser);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Access denied to {currentPath}\\{subKeyName}: {ex.Message}");
                }
            }
        }
        void SaveToDatabase(string keyPath, string name, string type, string data, string userName)
        {
            //   string connectionString =   "Data Source=RDSQL2022\\SQL2022;Initial Catalog=sandeep;User ID=sa;Encrypt=False;Password=Sirius@India";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                int id = 0;
                string systemQuery = @"Select system_id FROM PmSystem  WHERE system_name='" + Environment.MachineName + "'";
                using (SqlCommand cmd = new SqlCommand(systemQuery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = reader.GetInt16(0);         // Column 1 (Id)
                            if (id <= 0)
                            {
                                Console.WriteLine(Environment.MachineName + " does not exists in PMSystem table");
                                throw new Exception(Environment.MachineName + " does not exists in PMSystem table");
                            }
                            Console.WriteLine($"ID: {id}, Name: {name}");
                        }
                    }

                }
                string query = @"INSERT INTO Registry_Setting (KeyPath, KeyName, KeyType, KeyData,system_id,System_Logged_in_User)
                             VALUES (@KeyPath, @KeyName, @KeyType, @KeyData,@system_id,@System_Logged_in_User)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@KeyPath", keyPath);
                    cmd.Parameters.AddWithValue("@KeyName", name);
                    cmd.Parameters.AddWithValue("@KeyType", type);
                    cmd.Parameters.AddWithValue("@KeyData", data ?? "");
                    cmd.Parameters.AddWithValue("@system_id", id.ToString());
                    cmd.Parameters.AddWithValue("@System_Logged_in_User", userName ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int DataAlreadyExists()
        {
            int id = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string systemQuery = @"Select system_id FROM PmSystem  WHERE system_name='" + Environment.MachineName + "'";
                using (SqlCommand cmd = new SqlCommand(systemQuery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = reader.GetInt16(0);         // Column 1 (Id)
                            if (id <= 0)
                            {

                                Console.WriteLine(Environment.MachineName + " does not exists in PMSystem table");
                                return id;
                            }
                        }
                    }

                }
            }
            return id;
        }
        public void DeleteData(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"Delete From registry_setting  WHERE system_id=@system_id and (system_Logged_in_User=@system_Logged_in_User or isnull(system_Logged_in_User,'')='')";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@system_id", id.ToString());
                    cmd.Parameters.AddWithValue("@System_Logged_in_User", Environment.UserName ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static Dictionary<string, string> ReadPureRegistryValues()
        {
            var results = new Dictionary<string, string>();
            const string registryPath = @"SOFTWARE\Pure\Architecture\Server\Databases\Pure";
            string[] valueNames = { "Database", "Server", "Trusted" };

            try
            {
                // Open the registry key
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, false))
                {
                    if (key == null)
                    {
                        throw new InvalidOperationException($"Registry key '{registryPath}' not found.");
                    }

                    // Retrieve each value
                    foreach (string valueName in valueNames)
                    {
                        try
                        {
                            object value = key.GetValue(valueName);
                            if (value == null)
                            {
                                results[valueName] = "(Not found)";
                            }
                            else
                            {
                                // Store the value as a string
                                string valueStr = value.ToString();
                                // Note: If decryption is needed, call DecryptText(valueStr) here
                                results[valueName] = valueStr;
                            }
                        }
                        catch (Exception ex)
                        {
                            results[valueName] = $"Error accessing value ({ex.Message})";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If the key itself is inaccessible, set error for all values
                foreach (string valueName in valueNames)
                {
                    results[valueName] = $"Error accessing registry key ({ex.Message})";
                }
            }

            return results;
        }

        public static Dictionary<string, string> ReadCommonRegistryValues()
        {
            var results = new Dictionary<string, string>();
            const string registryPath = @"SOFTWARE\Pure\Architecture\Common";
            string[] valueNames = { "SecureKey", "SQLLogin" };

            try
            {
                // Open the registry key
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, false))
                {
                    if (key == null)
                    {
                        throw new InvalidOperationException($"Registry key '{registryPath}' not found.");
                    }

                    // Retrieve each value
                    foreach (string valueName in valueNames)
                    {
                        try
                        {
                            object value = key.GetValue(valueName);
                            if (value == null)
                            {
                                results[valueName] = "(Not found)";
                            }
                            else
                            {
                                // Store the value as a string
                                string valueStr = value.ToString();
                                // Note: If decryption is needed, call DecryptText(valueStr) here
                                results[valueName] = valueStr;
                            }
                        }
                        catch (Exception ex)
                        {
                            results[valueName] = $"Error accessing value ({ex.Message})";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If the key itself is inaccessible, set error for all values
                foreach (string valueName in valueNames)
                {
                    results[valueName] = $"Error accessing registry key ({ex.Message})";
                }
            }

            return results;
        }
    }
}