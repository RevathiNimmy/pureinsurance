using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.DirectoryServices.AccountManagement;
using System.IO;

namespace SSPValidateUser
{
    class Program
    {
        static void Main(string[] args)
        {            
            string loginusername = args[0];
            string password = args[1];
            string logfile = args[2];
            string domain = string.Empty;
            string username = string.Empty;
            string servername = System.Environment.MachineName.ToUpper();
            string[] logindetails = loginusername.Split('\\');

            File.WriteAllText(logfile, "FileInitilized");

            if (logindetails.Length > 0)
            {
                domain = logindetails[0].ToUpper();
            }

            if (logindetails.Length > 1)
            {
                username = logindetails[1];
            }

            if ((servername == domain) || (domain == "LOCALHOST") || domain == ".")
            {
                domain = string.Empty;
            }

            bool valid = false;
            try
            {
                if (domain.Length == 0)
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Machine))
                    {
                        valid = pc.ValidateCredentials(username, password);
                        if (valid)
                        {
                            File.WriteAllText(logfile, "LogOnSuccessfull");
                        }
                        else
                        {
                            File.WriteAllText(logfile, "LogOnFailed");
                        }
                    }
                }
                else
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
                    {
                        valid = pc.ValidateCredentials(username, password);
                        if (valid)
                        {
                            File.WriteAllText(logfile, "LogOnSuccessfull");
                        }
                        else
                        {
                            File.WriteAllText(logfile, "LogOnFailed");
                        }
                     }
                }

            }
            catch (Exception ex)
            {
                File.WriteAllText(logfile, ex.ToString());
            }
        }        
            
    }
}
