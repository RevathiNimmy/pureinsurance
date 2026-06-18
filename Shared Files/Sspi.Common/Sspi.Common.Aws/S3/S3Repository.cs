using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Sspi.Common.Aws.S3
{
    /// <summary>
    ///  Repository to perform operation on Amazon S3 Bucket.
    /// </summary>
    public class S3Repository : IS3Repository
    {
        private readonly IAmazonS3 _amazonS3Client;
        private readonly string _s3BucketName;
        private object className = "S3Repository";
        private readonly string _userName;
        private object applicationName = "Sspi.Common.Aws";
        dynamic bPMFunc;

        #region public Constructor

        /// <summary>
        /// Constructor for S3 Repository.
        /// </summary>
        /// <param name="s3BucketName">Name of the target S3 bucket.</param>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="awsRegion">Aws region of the bucket.</param>
        /// <param name="userName">Name of the User.</param>
        public S3Repository(string bucketName, string awsRegion, string userName)
        {
            RegionEndpoint awsRegionEndPoint = RegionEndpoint.GetBySystemName(awsRegion);
            string serviceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL");

            if (string.IsNullOrEmpty(serviceUrl))
            {
                _amazonS3Client = new AmazonS3Client(awsRegionEndPoint);
            }
            else
            {
                AmazonS3Config config = new AmazonS3Config { ServiceURL = serviceUrl, ForcePathStyle = true };
                _amazonS3Client = new AmazonS3Client(config);

            }

            _s3BucketName = bucketName.ToLower();
            _userName = userName;
            Assembly externalAssembly = Assembly.LoadFrom("SSP.Shared.dll");
            Type pluginType = externalAssembly.GetType("SSP.Shared.bPMFunc");
             bPMFunc = Activator.CreateInstance(pluginType);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Upload file to S3.
        /// </summary>
        /// <param name="fileUrl">Path of the file to upload.</param>
        /// <param name="content">Content of the file.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        public async Task<int> UploadFile(string fileUrl, byte[] content)
        {
            object methodName = "UploadFile";
            int result = 1;
            try
            {
                if (content.Length != 0)
                {
                    PutObjectRequest putObjectRequest = new PutObjectRequest
                    {
                        BucketName = _s3BucketName,
                        Key = fileUrl,
                        InputStream = new MemoryStream(content)
                    };

                    await _amazonS3Client.PutObjectAsync(putObjectRequest);

                    bPMFunc.LogMessage(_userName, 5, string.Format("S3 file [{0}::{1}] uploaded successfully.", _s3BucketName, fileUrl));
                }
            }
            catch (Exception ex)
            {
                object returnError = 11;
                object message = ex.Message;

                bPMFunc.LogMessage(sUsername: _userName, iType: 2, sMsg: string.Format("Unable to upload S3 file [{0}::{1}]", _s3BucketName, fileUrl),
                                   vApp: ref applicationName,
                                   vClass: ref className,
                                   vMethod: ref methodName,
                                   vErrNo: ref returnError,
                                   vErrDesc: ref message,
                                   excep: ref ex);

                result = 11;
            }
            return await Task.FromResult(result);
        }

        /// <summary>
        /// Get file from S3.
        /// </summary>
        /// <param name="fileUrl">Path of the file to read.</param>
        /// <param name="targetFileLocation">Target of the file to be placed.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        public async Task<int> DownloadFileAsync(string fileUrl, string targetFileLocation)
        {
            object methodName = "DownloadFileAsync";
            int result = 1;
            try
            {
                string targetFile = Path.Combine(@targetFileLocation, @fileUrl.Substring(fileUrl.LastIndexOf("/") + 1));

                GetObjectResponse objectResponse = _amazonS3Client.GetObjectAsync(_s3BucketName, fileUrl, null).GetAwaiter().GetResult();
                objectResponse.WriteResponseStreamToFileAsync(targetFile, false, new System.Threading.CancellationTokenSource().Token).GetAwaiter().GetResult();

                bPMFunc.LogMessage(_userName, 5, string.Format("S3 file [{0}::{1}] downloaded successfully to target file location: {2}.", _s3BucketName, fileUrl, targetFileLocation));
            }
            catch (Exception ex)
            {
                object returnError = 11;
                object message = ex.Message;

                bPMFunc.LogMessage(sUsername: _userName,
                                   iType: 2,
                                   sMsg: string.Format("Unable to get S3 file [{0}::{1}].", _s3BucketName, fileUrl),
                                   vApp: ref applicationName,
                                   vClass: ref className,
                                   vMethod: ref methodName,
                                   vErrNo: ref returnError,
                                   vErrDesc: ref message,
                                   excep: ref ex);

                result = 11;
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Get all files of specified folder from S3.
        /// </summary>
        /// <param name="folderUrl">Path of the folder to download the files.</param>
        /// <param name="targetLocation">Target folder for the downloaded files.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        public async Task<int> DownloadFolderAsync(string folderUrl, string targetLocation)
        {
            object methodName = "DownloadFolderAsync";
            int result = 1;
            try
            {
                folderUrl = folderUrl.EndsWith("/") ? folderUrl : folderUrl + @"/";
                ListObjectsV2Response objectListResponse = _amazonS3Client.ListObjectsV2Async(new ListObjectsV2Request
                {
                    BucketName = _s3BucketName,
                    Prefix = folderUrl
                }).GetAwaiter().GetResult();

                foreach (S3Object s3Object in objectListResponse.S3Objects)
                {
                    string targetFileLocation = MakeTargetFileLocation(targetLocation, s3Object.Key.Substring(folderUrl.Length));
                    await DownloadFileAsync(s3Object.Key, targetFileLocation);
                }
                bPMFunc.LogMessage(_userName, 5, string.Format("Downloaded all files from S3 folder [{0}::{1}] to target location: {2} successfully.", _s3BucketName, folderUrl, targetLocation));
            }
            catch (Exception ex)
            {
                object returnError = 11;
                object message = ex.Message;

                bPMFunc.LogMessage(sUsername: _userName,
                                   iType: 2,
                                   sMsg: string.Format("Unable to get files from S3 folder [{0}::{1}].", _s3BucketName, folderUrl),
                                   vApp: ref applicationName,
                                   vClass: ref className,
                                   vMethod: ref methodName,
                                   vErrNo: ref returnError,
                                   vErrDesc: ref message,
                                   excep: ref ex);

                result = 11;
            }

            return await Task.FromResult(result);
        }

        private string MakeTargetFileLocation(string targetLocation, string key)
        {
            string targetPath = Path.GetDirectoryName(
                Path.Combine(targetLocation, key.Replace("/", @"\"))
                );
            Directory.CreateDirectory(targetPath);
            return targetPath;
        }

        /// <summary>
        /// Delete file from S3.
        /// </summary>
        /// <param name="fileUrl">Path of the file to delete.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        public async Task<int> DeleteFile(string fileUrl)
        {
            object methodName = "DeleteFile";
            int result = 1;
            try
            {
                DeleteObjectRequest _deleteobjectRequest = new DeleteObjectRequest
                {
                    BucketName = _s3BucketName,
                    Key = fileUrl
                };
                _amazonS3Client.DeleteObjectAsync(_deleteobjectRequest);
                bPMFunc.LogMessage(_userName, 5, string.Format("S3 file [{0}::{1}] deleted successfully.", _s3BucketName, fileUrl));
            }
            catch (Exception ex)
            {
                object returnError = 11;
                object message = ex.Message;

                bPMFunc.LogMessage(sUsername: _userName,
                                   iType: 2,
                                   sMsg: string.Format("Unable to delete S3 file [{0}::{1}].", _s3BucketName, fileUrl),
                                   vApp: ref applicationName,
                                   vClass: ref className,
                                   vMethod: ref methodName,
                                   vErrNo: ref returnError,
                                   vErrDesc: ref message,
                                   excep: ref ex);

                result = 11;
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Delete the directory and its contents recursively.
        /// </summary>
        /// <param name="folderUrl">Path of the folder to delete.</param>
        /// <returns>The return code PMTrue indicating successful execution otherwise PMError.</returns>
        public async Task<int> DeleteDirectoryRecursive(string folderUrl)
        {
            object methodName = "DeleteDirectoryRecursive";
            int result = 1;
            try
            {
                DeleteObjectsRequest request2 = new DeleteObjectsRequest();
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = _s3BucketName
                };

                ListObjectsResponse response = await _amazonS3Client.ListObjectsAsync(request);
                // Process response.
                foreach (S3Object entry in response.S3Objects)
                {

                    request2.AddKey(folderUrl);
                }
                request2.BucketName = _s3BucketName;
                DeleteObjectsResponse response2 = await _amazonS3Client.DeleteObjectsAsync(request2);

                //S3DirectoryInfo directoryToDelete = new S3DirectoryInfo(_amazonS3Client, _s3BucketName, folderUrl);
                //directoryToDelete.Delete(true);
                bPMFunc.LogMessage(_userName, 5, string.Format("S3 directory [{0}::{1}] deleted successfully.", _s3BucketName, folderUrl));
            }
            catch (Exception ex)
            {
                object returnError = 11;
                object message = ex.Message;
                bPMFunc.LogMessage(sUsername: _userName,
                                   iType: 2,
                                   sMsg: string.Format("Unable to delete S3 directory [{0}::{1}].", _s3BucketName, folderUrl),
                                   vApp: ref applicationName,
                                   vClass: ref className,
                                   vMethod: ref methodName,
                                   vErrNo: ref returnError,
                                   vErrDesc: ref message,
                                   excep: ref ex);

                result = 11;
            }

            return await Task.FromResult(result);
        }

        #endregion
    }
}
