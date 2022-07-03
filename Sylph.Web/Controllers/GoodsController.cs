using Microsoft.AspNetCore.Mvc;
using Sylph.Web.Services.Interfaces;
using Sylph.Web.Models;
using Sylph.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sylph.Web.Controllers
{
    public class GoodsController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly IGoodsService _goodsService;

        public GoodsController(ISearchService searchService, IGoodsService goodsService)
        {
            _searchService = searchService;
            _goodsService = goodsService;
        }

        public IActionResult Search(string searchQuery)
        {
            var items = _searchService.Search(searchQuery);
            var result = _goodsService.FilterByName(items);
            var model = new GoodsViewModel
            {
                SearchQuery = searchQuery,
                Result = result
            };
            return View(model);
        }
    }
}
