using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp_Sample.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        #region IOC

        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        #endregion


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/admin/api/GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var res = await _userManager.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.RegistrationTime,
                u.EmailConfirmed,
                u.Active,

            }).ToListAsync();

            return Json(res);
        }
    }
}