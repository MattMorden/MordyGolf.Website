using MordyGolf.Website.ViewComponentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MordyGolf.Website.Services.Interfaces
{
    public interface IComponentService
    {
        HomeViewComponentModel GetHomeViewComponentModel();
        FAQsViewComponentModel GetFAQViewComponentModel();
    }
}
