using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MordyGolf.Website.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // public ViewModel model;


        /// <summary>
        /// Constructor for the Index Page Model
        /// </summary>
        /// <param name="logger">Logger dependency</param>
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get the data for the view models and construct them before rendering the page
        /// </summary>
        public void OnGet()
        {
            // Make necessary API calls to get init data

        }
    }
}