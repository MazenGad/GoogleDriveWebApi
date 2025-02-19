using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

public class GoogleDriveService
{
	private static readonly string[] Scopes = { DriveService.Scope.Drive };
	private readonly DriveService _driveService;
	private readonly string DefaultFolderId = "1ckm_9aJgcqFYsXv7YoF0nFO9DkOZmP5X"; 

	public GoogleDriveService()
	{
		GoogleCredential credential;
		var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Credentials", "service_account.json");
		using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
		{
			credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
		}

		_driveService = new DriveService(new BaseClientService.Initializer()
		{
			HttpClientInitializer = credential,
			ApplicationName = "GoogleDriveAPI"
		});
	}

	public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
	{
		var fileMetadata = new Google.Apis.Drive.v3.Data.File()
		{
			Name = fileName,
			Parents = new List<string> { DefaultFolderId } 
		};

		var request = _driveService.Files.Create(fileMetadata, fileStream, "image/jpeg");
		request.Fields = "id";
		await request.UploadAsync();

		return request.ResponseBody?.Id;
	}

	public string GetImageUrl(string fileId)
	{
		return $"https://drive.google.com/uc?id={fileId}";
	}
}
