using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using MordyGolf.Website.Services.Interfaces;
using MordyGolf.Website.ViewComponentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MordyGolf.Website.Services
{
    public class ComponentService : IComponentService
    {
        private readonly ILogger<ComponentService> _logger;
        private ICompositeViewEngine _compositeViewEngine;
        private IViewComponentHelper _viewComponentHelper;
        
        public ComponentService(ILogger<ComponentService> logger, ICompositeViewEngine compositeViewEngine, IViewComponentHelper viewComponentHelper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _compositeViewEngine = compositeViewEngine ?? throw new ArgumentNullException(nameof(compositeViewEngine));
            _viewComponentHelper = viewComponentHelper ?? throw new ArgumentNullException(nameof(viewComponentHelper));
        }

        public HomeViewComponentModel GetHomeViewComponentModel()
        {
            return new HomeViewComponentModel();
        }
    }
}
