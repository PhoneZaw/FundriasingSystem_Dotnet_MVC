using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.Campaign;
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
    public class CampaignController : Controller
    {
        private readonly CampaignService _CampaignService;
        private readonly CampaignTypeService _campaignTypeService;
        private readonly StaffService _staffService;
        private readonly IMapper _mapper;

        public CampaignController(CampaignService CampaignService,
            CampaignTypeService CampaignTypeService,
            StaffService staffService,
            IMapper mapper)
        {
            _CampaignService = CampaignService;
            _campaignTypeService = CampaignTypeService;
            _staffService = staffService;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> ViewCampaignAsync()
        {
            var Campaign = await _CampaignService.GetAllCampaignsAsync();
            var CampaignType = await _campaignTypeService.GetAllCampaignTypesAsync();
            var Staff = await _staffService.GetAllStaffAsync();

            foreach(var item in Campaign)
            {
                item.CampaignType = CampaignType.FirstOrDefault(x => x.Id == item.CampaignTypeId);
                item.Staff = Staff.FirstOrDefault(x => x.Id == item.StaffId);
            }

            return View(Campaign);
        }

        [HttpGet]
        public async Task<ActionResult> createCampaignAsync()
        {
            List<SelectListItem> campaignTypeItems = (await _campaignTypeService.GetAllCampaignTypesAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.CampaignTypeName,
                    Value = d.Id.ToString()
                };
            });

            var model = new CreateCampaignViewModel()
            {
                CampaignTypes = campaignTypeItems
            };
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> CreateCampaign(CreateCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Campaign>(model);

                await _CampaignService.CreateCampaignAsync(entity);

                return RedirectToAction("ViewCampaign");
            }

            return View(model);
        }

        [HttpGet]
        [Route("/Campaign/editCampaign/{id}")]
        public async Task<ActionResult> EditCampaign(string id)
        {
            var Campaign = await _CampaignService.GetByIdAsync(Guid.Parse(id));

            List<SelectListItem> campaignTypeItems = (await _campaignTypeService.GetAllCampaignTypesAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.CampaignTypeName,
                    Value = d.Id.ToString()
                };
            });

            var editModel = _mapper.Map<EditCampaignViewModel>(Campaign);
            editModel.CampaignTypes = campaignTypeItems;

            return View(editModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditCampaign(EditCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Campaign>(model);

                await _CampaignService.UpdateCampaignAsync(entity);

                return RedirectToAction("ViewCampaign");
            }

            return View(model);
        }

        [Route("/Campaign/Activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _CampaignService.ActivateCampaignAsync(Guid.Parse(id));

            return Redirect("/Campaign/viewCampaign");
        }

        [Route("/Campaign/Deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _CampaignService.DeactivateCampaignAsync(Guid.Parse(id));

            return Redirect("/Campaign/viewCampaign");
        }
    }
}
