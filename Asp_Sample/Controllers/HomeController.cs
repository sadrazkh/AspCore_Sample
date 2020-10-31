using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Asp_Sample.Models;
using DataLayer.Context;
using DataLayer.Entity.Select_2_Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Asp_Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {

            ViewBag.Parent = new SelectList( _dbContext.Lessons.Where(e => e.ParentId == null), "Id", "Title");
            ViewBag.Child = new SelectList(_dbContext.Lessons.Where(e => e.ParentId != null), "Id", "Title");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LessonViewModel model)
        {
            var test = model;
            return  RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LessonApi(int worksGroupId)
        {
            var res = await _dbContext.Lessons.Where(e => e.ParentId != worksGroupId).ToListAsync();
            var result = new List<Lesson>();
            foreach (var field in res)
            {
                result.Add(new Lesson()
                {
                    Id = field.Id,
                    Title = field.Title
                });
            }


            var rws = Json(result);

            return Json(result);
        }
    }
}
