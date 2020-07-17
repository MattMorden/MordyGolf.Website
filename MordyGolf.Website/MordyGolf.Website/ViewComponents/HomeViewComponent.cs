using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MordyGolf.Website.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MordyGolf.Website.ViewComponents
{
    /// <summary>
    /// View Component representing the Home page of MordyGolf.
    /// </summary>
    public class HomeViewComponent : ViewComponent
    {
        private readonly ILogger<HomeViewComponent> _logger;
        private readonly IComponentService _componentService;

        /// <summary>
        /// Constructor for HomeViewComponent
        /// </summary>
        /// <param name="logger">Logger dependency</param>
        /// <param name="componentService">ComponentService dependency</param>
        public HomeViewComponent(ILogger<HomeViewComponent> logger, IComponentService componentService)
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
            return View("Home", _componentService.GetHomeViewComponentModel());
        }
    }
}
