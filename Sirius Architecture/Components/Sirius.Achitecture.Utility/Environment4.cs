using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// Provides extra information about the current environment and platform.
    /// </summary>
    public static class Environment4 {

        #region Public Static Properties

        /// <summary>
        /// Returns true if the current process is 32-bit.
        /// </summary>
        public static bool Is32BitProcess { get { return IntPtr.Size == 4; } }

        /// <summary>
        /// Returns true if the current process is 64-bit.
        /// </summary>
        public static bool Is64BitProcess { get { return IntPtr.Size == 8; } }

        /// <summary>
        /// Returns true if the current process is 32-bit, running on a 64-bit system.
        /// </summary>
        public static bool Is32BitProcessOn64BitSystem { get { return IsWow64Process(); } }

        /// <summary>
        /// Returns true if the operating system is 32-bit.
        /// </summary>
        public static bool Is32BitSystem { get { return Is32BitProcess && !Is32BitProcessOn64BitSystem; } }

        /// <summary>
        /// Returns true if the operating system is 64-bit.
        /// </summary>
        public static bool Is64BitSystem { get { return Is64BitProcess || Is32BitProcessOn64BitSystem; } }

        #endregion

        #region Public Types

        /// <summary>
        /// Specifies enumerated constants used to retrieve directory paths to system special folders.
        /// </summary>
        /// <remarks>
        /// This is the .NET 4.0 version of the enum type.
        /// </remarks>
        public enum SpecialFolder {
            /// <summary></summary>
            AdminTools = 0x30,
            /// <summary></summary>
            ApplicationData = 0x1a,
            /// <summary></summary>
            CDBurning = 0x3b,
            /// <summary></summary>
            CommonAdminTools = 0x2f,
            /// <summary></summary>
            CommonApplicationData = 0x23,
            /// <summary></summary>
            CommonDesktopDirectory = 0x19,
            /// <summary></summary>
            CommonDocuments = 0x2e,
            /// <summary></summary>
            CommonMusic = 0x35,
            /// <summary></summary>
            CommonOemLinks = 0x3a,
            /// <summary></summary>
            CommonPictures = 0x36,
            /// <summary></summary>
            CommonProgramFiles = 0x2b,
            /// <summary></summary>
            CommonProgramFilesX86 = 0x2c,
            /// <summary></summary>
            CommonPrograms = 0x17,
            /// <summary></summary>
            CommonStartMenu = 0x16,
            /// <summary></summary>
            CommonStartup = 0x18,
            /// <summary></summary>
            CommonTemplates = 0x2d,
            /// <summary></summary>
            CommonVideos = 0x37,
            /// <summary></summary>
            Cookies = 0x21,
            /// <summary></summary>
            Desktop = 0,
            /// <summary></summary>
            DesktopDirectory = 0x10,
            /// <summary></summary>
            Favorites = 6,
            /// <summary></summary>
            Fonts = 20,
            /// <summary></summary>
            History = 0x22,
            /// <summary></summary>
            InternetCache = 0x20,
            /// <summary></summary>
            LocalApplicationData = 0x1c,
            /// <summary></summary>
            LocalizedResources = 0x39,
            /// <summary></summary>
            MyComputer = 0x11,
            /// <summary></summary>
            MyDocuments = 5,
            /// <summary></summary>
            MyMusic = 13,
            /// <summary></summary>
            MyPictures = 0x27,
            /// <summary></summary>
            MyVideos = 14,
            /// <summary></summary>
            NetworkShortcuts = 0x13,
            /// <summary></summary>
            Personal = 5,
            /// <summary></summary>
            PrinterShortcuts = 0x1b,
            /// <summary></summary>
            ProgramFiles = 0x26,
            /// <summary></summary>
            ProgramFilesX86 = 0x2a,
            /// <summary></summary>
            Programs = 2,
            /// <summary></summary>
            Recent = 8,
            /// <summary></summary>
            Resources = 0x38,
            /// <summary></summary>
            SendTo = 9,
            /// <summary></summary>
            StartMenu = 11,
            /// <summary></summary>
            Startup = 7,
            /// <summary></summary>
            System = 0x25,
            /// <summary></summary>
            SystemX86 = 0x29,
            /// <summary></summary>
            Templates = 0x15,
            /// <summary></summary>
            UserProfile = 40,
            /// <summary></summary>
            Windows = 0x24
        }

        /// <summary>
        /// Specifies options to use for getting the path to a special folder.
        /// </summary>
        /// <remarks>
        /// This is the .NET 4.0 version of the enum type.
        /// </remarks>
        public enum SpecialFolderOption {
            /// <summary>Verifies the folder path. If the folder does not exist, an empty string is returned. This is the default behavior.</summary>
            None = 0,
            /// <summary>Returns the path, but does not verify whether the path exists. If the folder is located on a network, specifying this option can reduce lag time.</summary>
            DoNotVerify = 0x4000,
            /// <summary>Forces the folder to be created if it does not already exist.</summary>
            Create = 0x8000,
        }

        #endregion

        #region Public Static Methods (32-bit only access)

        /// <summary>
        /// Gets the path to the system special folder in the 32-bit filesystem view that is identified by the specified enumeration.
        /// </summary>
        /// <param name="folder">Special folder</param>
        /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string.</returns>
        public static string GetFolderPath32(SpecialFolder folder) {
            return GetFolderPath32(folder, SpecialFolderOption.None);
        }

        /// <summary>
        /// Gets the path to the system special folder in the 32-bit filesystem view that is identified by the specified enumeration, and uses a specified option for accessing special folders.
        /// </summary>
        /// <param name="folder">Special folder</param>
        /// <param name="option">Options for accessing the folder</param>
        /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string.</returns>
        public static string GetFolderPath32(SpecialFolder folder, SpecialFolderOption option) {
            SpecialFolder folderX86;
            switch(folder) {
            case SpecialFolder.CommonProgramFiles:
                folderX86 = SpecialFolder.CommonProgramFilesX86;
                break;
            case SpecialFolder.ProgramFiles:
                folderX86 = SpecialFolder.ProgramFilesX86;
                break;
            case SpecialFolder.System:
                folderX86 = SpecialFolder.SystemX86;
                break;
            default:
                folderX86 = folder;
                break;
            }
            var path = GetFolderPath(folderX86, option);
            if(string.IsNullOrEmpty(path)) {
                path = GetFolderPath(folder, option);
            }
            return path;
        }

        #endregion

        #region Public Static Methods (Any CPU access)

        /// <summary>
        /// Gets the path to the system special folder that is identified by the specified enumeration.
        /// </summary>
        /// <param name="folder">Special folder</param>
        /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string.</returns>
        public static string GetFolderPath(SpecialFolder folder) {
            return GetFolderPath(folder, SpecialFolderOption.None);
        }

        /// <summary>
        /// Gets the path to the system special folder that is identified by the specified enumeration, and uses a specified option for accessing special folders.
        /// </summary>
        /// <param name="folder">Special folder</param>
        /// <param name="option">Options for accessing the folder</param>
        /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string.</returns>
        public static string GetFolderPath(SpecialFolder folder, SpecialFolderOption option) {
            if(!Enum.IsDefined(typeof(SpecialFolder), folder)) {
                throw new ArgumentException(Properties.Resources.Arg_EnumIllegalVal, "folder");
            }
            if(!Enum.IsDefined(typeof(SpecialFolderOption), option)) {
                throw new ArgumentException(Properties.Resources.Arg_EnumIllegalVal, "option");
            }
            if(option == SpecialFolderOption.Create) {
                new FileIOPermission(PermissionState.None) { AllFiles = FileIOPermissionAccess.Write }.Demand();
            }
            const int MAX_PATH = 260;
            var lpszPath = new StringBuilder(MAX_PATH);
            var errorCode = SHGetFolderPath(IntPtr.Zero, (int) (folder | ((SpecialFolder) ((int) option))), IntPtr.Zero, 0, lpszPath);
            switch(errorCode) {
            case -2146233031:
                throw new PlatformNotSupportedException();
            }
            var path = lpszPath.ToString();
            new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path).Demand();
            return path;
        }

        #endregion

        #region Private Static WinAPI Declarations

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(IntPtr hProcess, [Out] out bool wow64Process);

        [DllImport("shfolder.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, StringBuilder lpszPath);

        private static bool IsWow64Process() {
            var wow64Process = false;
            if(GetProcAddress(GetModuleHandle("kernel32"), "IsWow64Process") != IntPtr.Zero) {
                IsWow64Process(GetCurrentProcess(), out wow64Process);
            }
            return wow64Process;
        }

        #endregion
    }
}
