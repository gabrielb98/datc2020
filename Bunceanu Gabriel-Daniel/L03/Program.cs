using System;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Drive.v3;
using Google.Apis.Auth.OAuth2;
using System.Net;

namespace GDriveAPI
{
    class Program
    {
        public static DriveService _my_service;
        public static string _provided_token;
         static string ApplicationName = "DATC GDrive API";
        static void Main(string[] args)
        {
            initializer();
            Get_files();
            Upload().GetAwaiter().GetResult();
        }

        static void initializer(){
            string[] scopes = new string[]{
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile
            };

            var client_id = "489382490929-52h1im501ea4e4hc1flkcinkn0nerrsq.apps.googleusercontent.com";
            var client_secret = "evPRvtNGNDr8cldqMZBuCbtw";

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets{
                    ClientId = client_id,
                    ClientSecret = client_secret
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,
                null
            ).Result;
        _provided_token  = credential.Token.AccessToken;
        _my_service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
            Console.WriteLine("Credential file saved to: " +  _provided_token);
        }

        static void Get_files()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _provided_token );
            using(var response = request.GetResponse())
            {
                using(Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach(var file in myData["files"])
                    {
                        if(file["mimeType"].ToString()!="application/vnd/google-apps.folder")
                        {
                            Console.WriteLine("File Name: " + file["name"]);
                        }
                    }
                }
            }
        }

        //Upload file to Google Drive
        public static async Task<Google.Apis.Drive.v3.Data.File> Upload(string documentId="root")
        {
            var name = ($"{DateTime.UtcNow.ToString()}.txt");
            var mimeType = "text/plain"; // txt type upload

            var fMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = mimeType,
                Parents = new[] { documentId }
            };

            FilesResource.CreateMediaUpload request;

            FileStream mystream = new FileStream("datc_l03.txt", FileMode.Open, FileAccess.Read);
                request = _my_service.Files.Create(
                    fMetadata, mystream, mimeType
                );
                request.Fields = "id, name, parents, createdTime, modifiedTime, mimeType, thumbnailLink";
                await request.UploadAsync();

                return request.ResponseBody;
        }
    }
}
