using GraphApiMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System;
using System.IO;
using System.Threading.Tasks;
namespace GraphApiMVC
{
    public class OneDriveService
    {
        private readonly string accessToken;
        public OneDriveService(string accessToken)
        {

            this.accessToken = accessToken;
        }
        public async Task UploadFileToDrive(string filePath, string destinationFolder)
        {
            try
            {
                var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    return Task.CompletedTask;
                }));
                // Read the file content
                byte[] fileContent = await System.IO.File.ReadAllBytesAsync(filePath);
                // get a stream of the local file
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                // upload the file to OneDrive
                var uploadedFile = graphClient.Me.Drive.Root
                                              .ItemWithPath(destinationFolder)
                                              .Content
                                              .Request()
                                              .PutAsync<DriveItem>(fileStream)
                                              .Result;
                Console.WriteLine("upload session created");
                Console.WriteLine("file uploadede");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file: {ex.Message}");
            }
        }
        public async Task<List<DriveItemInfo>> DownloadUserFilesFromOneDrive()
        {
            try
            {
                var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    return Task.CompletedTask;
                }));
                // Retrieve the items in the root folder of OneDrive
                var oneDriveRoot = await graphClient.Me.Drive.Root
                    .Children
                    .Request()
                    .GetAsync();
                // Extract relevant information (name and ID) for each DriveItem
                var result = new List<DriveItemInfo>();
                foreach (var driveItem in oneDriveRoot)
                {
                    result.Add(new DriveItemInfo
                    {
                        Id = driveItem.Id,
                        Name = driveItem.Name
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving files from OneDrive: {ex.Message}");
            }
        }
        public async Task<IEnumerable<string>> DownloadFileById(string fileId, string folderPath)
        {
            try
            {
                var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    return Task.CompletedTask;
                }));
                // Get reference to stream of file in OneDrive
                var fileStream = await graphClient.Me.Drive.Items[fileId]
                    .Content
                    .Request()
                    .GetAsync();
                // Ensure the target folder exists
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                // Combine folder path with the file name
                var localFilePath = Path.Combine(folderPath, fileId + ".downloaded");
                // Save stream to the local file
                using (var driveItemFile = System.IO.File.Create(localFilePath))
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    await fileStream.CopyToAsync(driveItemFile);
                }
                var downloadedFiles = new List<string> { fileId + ".downloaded" };
                return downloadedFiles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error downloading file: {ex.Message}");
            }
        }
        public async Task UploadLargeFileToDrive(string fileName, Stream fileStream)
        {
            try
            {
                var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    return Task.CompletedTask;
                }));
                var uploadSession = await graphClient.Me.Drive.Root
                    .ItemWithPath(fileName)
                    .CreateUploadSession()
                    .Request()
                    .PostAsync();
                var maxChunkSize = 320 * 1024;
                var largeUploadTask = new LargeFileUploadTask<DriveItem>(uploadSession, fileStream, maxChunkSize);
                var uploadProgress = new Progress<long>(uploadBytes =>
                {
                    Console.WriteLine($"Uploaded {uploadBytes} bytes of {fileStream.Length} bytes");
                });
                var uploadResult = await largeUploadTask.UploadAsync(uploadProgress);
                if (uploadResult.UploadSucceeded)
                {
                    Console.WriteLine("File uploaded to user's OneDrive root folder.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading large file: {ex.Message}");
            }
        }

    }


}