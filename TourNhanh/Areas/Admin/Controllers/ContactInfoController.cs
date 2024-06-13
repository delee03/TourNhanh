using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using TourNhanh.ViewModel;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactInfoController : Controller
    {
        private readonly IConfiguration _configuration;

        public ContactInfoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            var contactInfo = new ContactInfo
            {
                ZaloUrl = _configuration["ContactInfo:ZaloUrl"],
                PhoneUrl = _configuration["ContactInfo:PhoneUrl"],
                PhoneNumber = _configuration["ContactInfo:PhoneNumber"]
            };
            return View(contactInfo);
        }

        [HttpGet]
        public IActionResult EditContactInfo()
        {
            var contactInfo = new ContactInfo
            {
                ZaloUrl = _configuration["ContactInfo:ZaloUrl"],
                PhoneUrl = _configuration["ContactInfo:PhoneUrl"],
                PhoneNumber = _configuration["ContactInfo:PhoneNumber"]
            };
            return View(contactInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditContactInfo(ContactInfo model)
        {
            if (ModelState.IsValid)
            {
                var configFilePath = "appsettings.json";
                var json = await System.IO.File.ReadAllTextAsync(configFilePath);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                jsonObj["ContactInfo"]["ZaloUrl"] = model.ZaloUrl;
                jsonObj["ContactInfo"]["PhoneUrl"] = model.PhoneUrl;
                jsonObj["ContactInfo"]["PhoneNumber"] = model.PhoneNumber;

                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(configFilePath, output);

                TempData["Message"] = "Contact information updated successfully!";
                // Chờ đợi cho tới khi ghi vào tập tin hoàn thành
                await Task.Delay(500); // Thời gian chờ ngắn nhất có thể
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


    }
}
