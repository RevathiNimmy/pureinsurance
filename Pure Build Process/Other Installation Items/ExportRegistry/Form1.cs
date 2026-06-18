using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ExportRegistry
{
    public partial class Form1 : Form
    {
        Export_Registry.Main1 erm;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            try
            {
                // Update connection string from registry
                FixConfig();

                // Initialize Main1 and set connection string
                erm = new Export_Registry.Main1();
                erm.connectionString = ConfigurationManager.ConnectionStrings["PureConnection"]?.ConnectionString;
                if (string.IsNullOrEmpty(erm.connectionString))
                {
                    MessageBox.Show("Error: PureConnection string not found in configuration.", "Configuration Error");
                    button1.Enabled = false;
                    button1.Text = "Export Unavailable";
                    return;
                }

                // Check for existing registry data
                int id = erm.DataAlreadyExists();
                if (id > 0)
                //{
                //    if (MessageBox.Show($"Registry data for {Environment.MachineName} already exists in the database.\nDo you want to overwrite it?",
                //        "Registry Entry Already Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                //    {
                //        erm.DeleteData(id);
                //        button1.Enabled = true;
                //        button1.Text = "Export";
                //    }
                //    else
                //    {
                //        button1.Enabled = false;
                //        button1.Text = "Export Disabled";
                //    }
                //}
                //else
                //{
                //    button1.Enabled = true;
                //    button1.Text = "Export";
                    
                //}
                erm.DeleteData(id);
                button1.Enabled = true;
                button1.Text = "Export";

                ExportRegistry();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during form load: {ex.Message}", "Form Load Error");
                button1.Enabled = false;
                button1.Text = "Export Unavailable";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportRegistry();
        }
        
        private void  ExportRegistry()
        {
            button1.Enabled = false;
            try
            {
                erm.GetAndSaveRegistryData();
                button1.BackColor = Color.Green;
                button1.Text = "Exported Successfully";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting registry data: {ex.Message}", "Export Error");
                button1.Text = "Export Failed";
            }
            Application.Exit();
        }

        private void GetRegistryConfig_Click(object sender, EventArgs e)
        {
            FixConfig();
        }

        private void GetRegistryConfig_Click1(object sender, EventArgs e)
        {
            // Placeholder: Implement if needed
        }

        private void FixConfig()
        {
            try
            {
                // Retrieve registry values from Pure key
                Dictionary<string, string> pureValues = Export_Registry.Main1.ReadPureRegistryValues();

                // Validate Server and Database values
                if (pureValues["Server"].Contains("Error accessing") || pureValues["Server"] == "(Not found)")
                {
                    throw new InvalidOperationException($"Failed to retrieve Server value: {pureValues["Server"]}");
                }
                if (pureValues["Database"].Contains("Error accessing") || pureValues["Database"] == "(Not found)")
                {
                    throw new InvalidOperationException($"Failed to retrieve Database value: {pureValues["Database"]}");
                }

                // Retrieve registry values from Common key
                Dictionary<string, string> commonValues = Export_Registry.Main1.ReadCommonRegistryValues();

                // Validate SecureKey and SQLLogin values
                if (commonValues["SecureKey"].Contains("Error accessing") || commonValues["SecureKey"] == "(Not found)")
                {
                    throw new InvalidOperationException($"Failed to retrieve SecureKey value: {commonValues["SecureKey"]}");
                }
                if (commonValues["SQLLogin"].Contains("Error accessing") || commonValues["SQLLogin"] == "(Not found)")
                {
                    throw new InvalidOperationException($"Failed to retrieve SQLLogin value: {commonValues["SQLLogin"]}");
                }

                // Decrypt SecureKey and SQLLogin
                string decryptedPassword;
                string decryptedUserId;
                try
                {
                    decryptedPassword = DecryptText(commonValues["SecureKey"]);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to decrypt SecureKey: {ex.Message}", ex);
                }
                try
                {
                    decryptedUserId = DecryptText(commonValues["SQLLogin"]);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to decrypt SQLLogin: {ex.Message}", ex);
                }

                // Open the config file for the running executable
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Access the connection string section
                ConnectionStringsSection csSection = config.ConnectionStrings;

                // Ensure PureConnection exists
                if (csSection.ConnectionStrings["PureConnection"] == null)
                {
                    throw new ConfigurationErrorsException("PureConnection string not found in configuration.");
                }

                // Build the connection string using registry values
                string connectionString = $"Data Source={pureValues["Server"]};" +
                                         $"Initial Catalog={pureValues["Database"]};" +
                                         $"User ID={decryptedUserId};" +
                                         $"Password={decryptedPassword};" +
                                         "Encrypt=False;TrustServerCertificate=True;";
                csSection.ConnectionStrings["PureConnection"].ConnectionString = connectionString;

                // Save the changes
                config.Save(ConfigurationSaveMode.Modified);

                // Refresh the section so the app picks it up immediately
                ConfigurationManager.RefreshSection("connectionStrings");

               // MessageBox.Show("Connection string updated successfully!");
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error updating config: {ex.Message}", "Configuration Error");
            }
        }


        private static string DecryptText(string sEncryptedText)
        {
            const DataProtectionScope kScope = DataProtectionScope.LocalMachine;
            if (string.IsNullOrEmpty(sEncryptedText))
                throw new ArgumentNullException(nameof(sEncryptedText));

            byte[] bKeys = Encoding.ASCII.GetBytes("SiriusArchitecture");
            try
            {
                byte[] aEncrypted = Convert.FromBase64String(sEncryptedText);
                byte[] aDecrypted = ProtectedData.Unprotect(aEncrypted, bKeys, kScope);
                return Encoding.Unicode.GetString(aDecrypted);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException($"Failed to decrypt data: {ex.Message}", ex);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid base64 format in encrypted text", ex);
            }
        }
    }
}