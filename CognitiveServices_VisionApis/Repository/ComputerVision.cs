using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CognitiveServices_VisionApis.Repository
{
    public class ComputerVision
    {
        private string BytesToSrcString(byte[] bytes) => "data:image/jpg;base64," + Convert.ToBase64String(bytes);

        public string FileToImgSrcString(IFormFile file)
        {
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }
            return BytesToSrcString(fileBytes);
        }
    }
}
