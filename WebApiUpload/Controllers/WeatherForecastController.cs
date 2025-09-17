using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApiUpload.Models;

namespace WebApiUpload.Controllers
{
    [Route("[controller]"), Produces("application/json"), EnableCors("myPolicy")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        #region file upload
        // POST: api/WeatherForecast/savecloudticketconversation
        [HttpPost("[action]")]
        public async Task<object> savecloudticketconversation()
        {
            object result = null; DocModel _DocModel = null; bool resState = false;
            try
            {
                var listAttachment = new List<FileModel>();
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                foreach (var attachedFile in Request.Form.Files)
                {
                    //string serverPathAttachmentFile = string.Empty;
                    if (attachedFile != null)
                    {
                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }

                        if (attachedFile.Length > 0)
                        {
                            string fileName = ContentDispositionHeaderValue.Parse(attachedFile.ContentDisposition).FileName.Trim('"');

                            var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            var arrayExtens = fileName.Split(".");
                            var exten = arrayExtens[arrayExtens.Length - 1];
                            fileName = fileName.Substring(0, fileName.Length - (exten.Length + 1)) + "_" + newFileName + "." + exten;

                            string fullPath = Path.Combine(pathToSave, fileName);
                            //serverPathAttachmentFile = fileName;
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                attachedFile.CopyTo(stream);
                            }
                            var oAttachment = new FileModel();
                            oAttachment.FileName = fileName;
                            oAttachment.FileUri = ("\\" + folderName + "\\" + fileName).Replace(@"\", @"/");
                            oAttachment.PhysicalPath = fullPath;
                            listAttachment.Add(oAttachment);
                        }
                    }

                }


                _DocModel = new DocModel()
                {
                    TicketReply = Request.Form["TicketReply"] == "" ? "" : Convert.ToString(Request.Form["TicketReply"]),
                    Files = listAttachment
                };

                // save img uri
                resState = true;
            }
            catch (Exception)
            {

            }

            await Task.Yield();

            return result = new
            {
                resState,
                _DocModel
            };
        }
        #endregion
    }
}
