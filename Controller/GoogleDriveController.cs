using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/google-drive")]
public class GoogleDriveController : ControllerBase
{
	private readonly GoogleDriveService _driveService;

	public GoogleDriveController(GoogleDriveService driveService)
	{
		_driveService = driveService;
	}

	[HttpPost("upload")]
	[Consumes("multipart/form-data")]
	public async Task<IActionResult> UploadImage( IFormFile file)
	{
		try
		{
			if (file == null || file.Length == 0)
				return BadRequest(new { message = "No file uploaded." });

			using (var stream = file.OpenReadStream())
			{
				string fileId = await _driveService.UploadImageAsync(stream, file.FileName);
				string fileUrl = _driveService.GetImageUrl(fileId);
				return Ok(new { fileId, fileUrl });
			}
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Error uploading file.", error = ex.Message });
		}
	}

	[HttpGet("download/{fileId}")]
	public IActionResult DownloadImage(string fileId)
	{
		try
		{
			if (string.IsNullOrEmpty(fileId))
				return BadRequest(new { message = "File ID is required." });

			string fileUrl = _driveService.GetImageUrl(fileId);
			return Ok(new { fileUrl });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "Error retrieving file URL.", error = ex.Message });
		}
	}
}
