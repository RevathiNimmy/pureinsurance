using System;
using System.IO;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        //Console.Write("Enter the environment variable name: ");
        string name = "Client_Secret";

        Console.Write("Enter the environment variable value for Client Secret: ");
        string value = Console.ReadLine();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Persistent for current user on Windows
            Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Machine);
            Console.WriteLine($"✅ {name} set successfully in Windows environment variables.");
        }
        else
        {
            // On Linux/macOS, append to ~/.bashrc for persistence
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string bashrc = Path.Combine(home, ".bashrc");

            string exportLine = $"export {name}=\"{value}\"";

            // Avoid duplicates: remove old entry if exists
            string[] lines = File.Exists(bashrc) ? File.ReadAllLines(bashrc) : Array.Empty<string>();
            using (var writer = new StreamWriter(bashrc, false))
            {
                foreach (var line in lines)
                {
                    if (!line.TrimStart().StartsWith($"export {name}="))
                        writer.WriteLine(line);
                }
                writer.WriteLine(exportLine);
            }

            Console.WriteLine($"✅ {name} set successfully in {bashrc}");
            Console.WriteLine("👉 Run `source ~/.bashrc` or restart terminal to apply.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
