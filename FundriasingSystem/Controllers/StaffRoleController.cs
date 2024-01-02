using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.StaffRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class StaffRoleController : Controller
    {
        private readonly StaffRoleService _StaffRoleService;
        private readonly IMapper _mapper;

        public StaffRoleController(StaffRoleService StaffRoleService,
            IMapper mapper)
        {
            _StaffRoleService = StaffRoleService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staffRoles")]
        public async Task<IActionResult> ViewStaffRoleAsync()
        {
            var StaffRole = await _StaffRoleService.GetAllRolesAsync();
            return View(StaffRole);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staffRoles/create")]
        public ActionResult createStaffRole()
        {
            return View();
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staffRoles/create")]
        public async Task<ActionResult> CreateStaffRole(CreateStaffRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<StaffRole>(model);

                await _StaffRoleService.CreateRoleAsync(entity);

                return RedirectToAction("ViewStaffRole");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staffRoles/edit/{id}")]
        public async Task<ActionResult> EditStaffRole(string id)
        {
            var StaffRole = await _StaffRoleService.GetByIdAsync(Guid.Parse(id));

            var editModel = _mapper.Map<EditStaffRoleViewModel>(StaffRole);

            return View(editModel);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staffRoles/edit")]
        public async Task<ActionResult> EditStaffRole(EditStaffRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<StaffRole>(model);

                await _StaffRoleService.UpdateRoleAsync(entity);

                return RedirectToAction("ViewStaffRole");
            }

            return View(model);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staffRoles/activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _StaffRoleService.ActivateRoleAsync(Guid.Parse(id));

            return Redirect("/StaffRole/viewStaffRole");
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/staffRoles/deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _StaffRoleService.DeactivateRoleAsync(Guid.Parse(id));

            return Redirect("/StaffRole/viewStaffRole");
        }
    }
}
