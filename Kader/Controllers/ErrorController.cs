
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Kader.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : Controller
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }
        [Route("error")]
        public IActionResult Index()

        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
        
       
    }
}
