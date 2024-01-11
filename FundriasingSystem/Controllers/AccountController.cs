using FundraisingApp.Entities;
using FundraisingApp.Exceptions;
using FundraisingApp.Helpers;
using FundraisingApp.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using FundriasingSystem.Models.Account;
using System.Collections;
using FundraisingApp.Enums;

namespace FundriasingSystem.Controllers
{
    public class AccountController : Controller
    {
        private IRepository<Staff> _staffRepository;
        private IRepository<Donor> _donorRepository;
        private IRepository<StaffRole> _staffRoleRepository;

        public AccountController(IRepository<Staff> staffRepository,
            IRepository<Donor> donorRepository,
            IRepository<StaffRole> staffRoleRepository)
        {
            _staffRepository = staffRepository;
            _donorRepository = donorRepository;
            _staffRoleRepository = staffRoleRepository;
        }

        [HttpGet]
        [Route("/admin/login")]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/admin/login")]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var staff = (await _staffRepository.GetAllAsync()).FirstOrDefault(s => s.Email == model.Email);

                if (staff is null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Email is not found");
                    return View(model);
                }

                var hashPassword = HashHelper.GetHash(model.Password);

                if (staff.HashPassword != hashPassword)
                {
                    ModelState.AddModelError(nameof(model.Password), "Password is incorrect");
                    return View(model);
                }

                if (staff.Status == StatusEnum.Inactive.ToString())
                {
                    ModelState.AddModelError(nameof(model.Email), "Your account is deactivated");
                    return View(model);
                }

                var role = await _staffRoleRepository.GetByIdAsync(staff.RoleId);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, staff.Email),
                new Claim("UserId", staff.Id.ToString()),
                new Claim("FullName", $"{staff.FirstName} {staff.LastName}"),
                new Claim(ClaimTypes.Role, role != null ? role.RoleName : ""),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, "Admin");

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),

                    IsPersistent = true,

                    IssuedUtc = DateTime.Now,
                };

                await HttpContext.SignInAsync(
                    "Admin",
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Redirect("/admin/staff");
            }

            return View(model);
        }

        [AllowAnonymous]
        [Route("/admin/logout")]
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync(
                "Admin");

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var donor = (await _donorRepository.GetAllAsync()).FirstOrDefault(d => d.Email == model.Email);

                if (donor is null)
                {

                    ModelState.AddModelError(nameof(model.Email), "Email is not found");
                    return View(model);
                }

                var hashPassword = HashHelper.GetHash(model.Password);

                if (donor.HashPassword != hashPassword)
                {
                    ModelState.AddModelError(nameof(model.Password), "Password is incorrect");
                    return View(model);
                }

                if (donor.Status == StatusEnum.Inactive.ToString())
                {
                    ModelState.AddModelError(nameof(model.Email), "Your account is deactivated");
                    return View(model);
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, donor.Email),
                new Claim("UserId", donor.Id.ToString()),
                new Claim("FullName", $"{donor.FirstName} {donor.LastName}"),
                new Claim(ClaimTypes.Role, "donor"),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),

                    IsPersistent = true,

                    IssuedUtc = DateTime.Now,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Redirect("/");
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/");
        }
    }
}
