using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Kader.Pages
{
    public class UnAuthorizedModel : PageModel
    {
        /// <summary>
        /// Done By Mahmoud Ashraf
        /// </summary>
        private readonly IStringLocalizer<UnAuthorizedModel> _localizer;

        public UnAuthorizedModel(IStringLocalizer<UnAuthorizedModel> localizer)
        {

            _localizer = localizer;
        }
        public void OnGet()
        {
        }
    }
}
