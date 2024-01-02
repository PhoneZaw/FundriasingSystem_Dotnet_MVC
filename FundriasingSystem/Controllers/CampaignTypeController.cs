using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.CampaignType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class CampaignTypeController : Controller
    {
        private readonly CampaignTypeService _CampaignTypeService;
        private readonly IMapper _mapper;

        public CampaignTypeController(CampaignTypeService CampaignTypeService,
            IMapper mapper)
        {
            _CampaignTypeService = CampaignTypeService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaignTypes/")]
        public async Task<IActionResult> ViewCampaignTypeAsync()
        {
            var CampaignType = await _CampaignTypeService.GetAllCampaignTypesAsync();
            return View(CampaignType);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaignTypes/create")]
        public ActionResult createCampaignType()
        {
            return View();
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaignTypes/create")]
        public async Task<ActionResult> CreateCampaignType(CreateCampaignTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<CampaignType>(model);

                await _CampaignTypeService.CreateCampaignTypeAsync(entity);

                return RedirectToAction("ViewCampaignType");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaignTypes/edit/{id}")]
        public async Task<ActionResult> EditCampaignType(string id)
        {
            var CampaignType = await _CampaignTypeService.GetByIdAsync(Guid.Parse(id));

            var editModel = _mapper.Map<EditCampaignTypeViewModel>(CampaignType);

            return View(editModel);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaignTypes/edit")]
        public async Task<ActionResult> EditCampaignType(EditCampaignTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<CampaignType>(model);

                await _CampaignTypeService.UpdateCampaignTypeAsync(entity);

                return RedirectToAction("ViewCampaignType");
            }

            return View(model);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaignTypes/Activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _CampaignTypeService.ActivateCampaignTypeAsync(Guid.Parse(id));

            return Redirect("/CampaignType/viewCampaignType");
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaignTypes/deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _CampaignTypeService.DeactivateCampaignTypeAsync(Guid.Parse(id));

            return Redirect("/CampaignType/viewCampaignType");
        }
    }
}
