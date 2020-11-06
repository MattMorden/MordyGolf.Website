using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MordyGolf.Website.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MordyGolf.Website.ViewComponents
{
    public class FAQsViewComponent : ViewComponent
    {
        private readonly ILogger<FAQsViewComponent> _logger;
        private readonly IComponentService _componentService;

        public FAQsViewComponent(ILogger<FAQsViewComponent> logger, IComponentService componentService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _componentService = componentService ?? throw new ArgumentNullException(nameof(componentService));
        }

        /// <summary>
        /// Method to generate the view component
        /// </summary>
        /// <returns>Compiled view component</returns>
        public IViewComponentResult Invoke()
        {
            return View("FAQs", _componentService.GetFAQViewComponentModel());
        }
    }
}
