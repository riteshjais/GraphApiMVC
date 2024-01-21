using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Azure.Core;

namespace GraphApiMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OneDriveController : ControllerBase
    {
        private static string accessToken = "AccessToken";
       [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var graphService = new OneDriveService(accessToken);
                var fileName = "TextFile.txt";
                var currentFolder = System.IO.Directory.GetCurrentDirectory();
                var filePath = Path.Combine(currentFolder, fileName);
                await graphService.UploadFileToDrive(filePath, fileName);
                return Ok("File uploaded successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error uploading file: {ex.Message}");
            }
        }
        [HttpGet("downloadUserFiles")]
        public async Task<IActionResult> DownloadUserFiles()
        {
            try
            {
                var oneDriveService = new OneDriveService(accessToken);
                var files = await oneDriveService.DownloadUserFilesFromOneDrive();
                return Ok(files);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading files: {ex.Message}");
            }
        }
        [HttpGet("downloadFileById")]
        public async Task<IActionResult> DownloadFileById([FromQuery] string fileId)
        {
            try
            {
                string filePath = "C:\\Users\\nineleaps\\Desktop\\Asp.Net Projects\\GraphApiMVC\\GraphApiMVC\\NewFolder\\";
                var oneDriveService = new OneDriveService(accessToken);
                var files = await oneDriveService.DownloadFileById(fileId, filePath);
                return Ok(files);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading files: {ex.Message}");
            }
        }
        [HttpPost("uploadLargeFile")]
        public async Task<IActionResult> UploadLargeFile()
        {
            try
            {
                var fileName = "Vaaranam-Aayiram-128kbps-MassTamilan.com.zip";
                var currentFolder = System.IO.Directory.GetCurrentDirectory();
                var filePath = Path.Combine(currentFolder, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var oneDriveService = new OneDriveService(accessToken);
                    await oneDriveService.UploadLargeFileToDrive(fileName, fileStream);
                }
                return Ok("Large file uploaded successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error uploading large file: {ex.Message}");
            }
        }
    }
}