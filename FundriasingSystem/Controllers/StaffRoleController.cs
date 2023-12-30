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

        [Authorize]
        public async Task<IActionResult> ViewStaffRoleAsync()
        {
            var StaffRole = await _StaffRoleService.GetAllRolesAsync();
            return View(StaffRole);
        }

        [HttpGet]
        public ActionResult createStaffRole()
        {
            return View();
        }


        [HttpPost]
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
        [Route("/StaffRole/editStaffRole/{id}")]
        public async Task<ActionResult> EditStaffRole(string id)
        {
            var StaffRole = await _StaffRoleService.GetByIdAsync(Guid.Parse(id));

            var editModel = _mapper.Map<EditStaffRoleViewModel>(StaffRole);

            return View(editModel);
        }

        [HttpPost]
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

        [Route("/StaffRole/Activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _StaffRoleService.ActivateRoleAsync(Guid.Parse(id));

            return Redirect("/StaffRole/viewStaffRole");
        }

        [Route("/StaffRole/Deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _StaffRoleService.DeactivateRoleAsync(Guid.Parse(id));

            return Redirect("/StaffRole/viewStaffRole");
        }
    }
}
