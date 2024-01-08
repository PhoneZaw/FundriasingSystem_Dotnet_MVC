using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class StaffController : Controller
    {
        private readonly StaffService _staffService;
        private readonly StaffRoleService _staffRoleService;
        private readonly IMapper _mapper;

        public StaffController(StaffService staffService,
            StaffRoleService staffRoleService,
            IMapper mapper)
        {
            _staffService = staffService;
            _staffRoleService = staffRoleService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin")]
        [Route("/admin/staff")]
        public async Task<IActionResult> ViewStaffAsync()
        {
            var staff = await _staffService.GetAllStaffAsync();
            var staffRoles = await _staffRoleService.GetAllRolesAsync();

            foreach(var s in staff)
            {
                s.Role = staffRoles.FirstOrDefault(r => r.Id == s.RoleId);
            }
            return View(staff);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staff/create")]
        public async Task<ActionResult> createStaff()
        {

            List<SelectListItem> item = (await _staffRoleService.GetAllRolesAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.RoleName,
                    Value = d.Id.ToString()
                };
            });

            var model = new CreateStaffViewModel()
            {
                Roles = item
            };

            return View(model);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staff/create")]
        public async Task<ActionResult> CreateStaff(CreateStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Staff>(model);

                await _staffService.CreateStaffAsync(entity, model.Password);

                return RedirectToAction("ViewStaff");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staff/edit/{id}")]
        public async Task<ActionResult> EditStaff(string id)
        {
            var staff = await _staffService.GetByIdAsync(Guid.Parse(id));

            List<SelectListItem> item = (await _staffRoleService.GetAllRolesAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.RoleName,
                    Value = d.Id.ToString()
                };
            });

            var editModel = _mapper.Map<EditStaffViewModel>(staff);

            editModel.Roles = item;

            return View(editModel);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staff/edit/{id}")]
        public async Task<ActionResult> EditStaff(EditStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Staff>(model);

                await _staffService.UpdateStaffAsync(entity);

                return RedirectToAction("ViewStaff");
            }

            return View(model);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staff/Activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _staffService.ActivateStaffAsync(Guid.Parse(id));

            return Redirect("/staff/viewStaff");
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staff/Deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _staffService.DeactivateStaffAsync(Guid.Parse(id));

            return Redirect("/staff/viewStaff");
        }
    }
}
