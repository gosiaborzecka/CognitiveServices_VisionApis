using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CognitiveServices_VisionApis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace CognitiveServices_VisionApis.Controllers
{
    public class ComputerVisionController : Controller
    {
        private readonly AppSettings _appSettings;

        public ComputerVisionController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PictureAnalysis()
        {
            ViewData["Message"] = "Picture analysis";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PictureAnalysis(IFormFile file)
        {
            try
            {
                var vision = new Repository.ComputerVision();

                ViewData["originalImage"] = vision.FileToImgSrcString(file);
                string description;
                string result;
                string confidence;

                using (var httpClient = new HttpClient())
                {
                    string baseUri = _appSettings.VisionApiUri + "/describe";

                    httpClient.BaseAddress = new Uri(baseUri);
                    httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.VisionApiKey);

                    HttpContent content = new StreamContent(file.OpenReadStream());
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var response = await httpClient.PostAsync(baseUri, content);

                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var jresult = JObject.Parse(jsonResponse);

                    description = jresult["description"]["captions"][0]["text"].ToString();
                    confidence = jresult["description"]["captions"][0]["confidence"].ToString();
                    result = jresult.ToString();

                }

                ViewData["description"] = description;
                ViewData["result"] = result;
                ViewData["confidence"] = confidence;
            }
            catch(Exception ex)
            {
                
            }

            return View();
        }
    }
}