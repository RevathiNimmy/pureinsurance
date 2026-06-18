using System.Threading.Tasks;

namespace Sspi.Common.Aws.S3
{
    public interface IS3Repository
    {
        /// <summary>
        /// Upload file to S3.
        /// </summary>
        /// <param name="fileUrl">Path of the file to upload.</param>
        /// <param name="content">Content of the file.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        Task<int> UploadFile(string fileUrl, byte[] content);

        /// <summary>
        ///  Get file from S3.
        /// </summary>
        /// <param name="fileUrl">Path of the file to read.</param>
        /// <param name="targetFileLocation">Target of the file to be placed.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        Task<int> DownloadFileAsync(string fileUrl, string targetFileLocation);
        
        /// <summary>
        /// Get all files of specified folder from S3.
        /// </summary>
        /// <param name="folderUrl">Path of the folder to download the files.</param>
        /// <param name="targetLocation">Target folder for the downloaded files.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        Task<int> DownloadFolderAsync(string folderUrl, string targetLocation);

        /// <summary>
        /// Delete file from S3.
        /// </summary>
        /// <param name="fileUrl">Path of the file to delete.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        Task<int> DeleteFile(string fileUrl);

        /// <summary>
        /// Delete the directory and its contents recursively
        /// </summary>
        /// <param name="folderUrl">Path of the folder to delete.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        Task<int> DeleteDirectoryRecursive(string folderUrl);
    }
}
