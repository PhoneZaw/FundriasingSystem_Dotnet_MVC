using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.Campaign;
using FundriasingSystem.Models.Staff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class CampaignController : Controller
    {
        private readonly CampaignService _CampaignService;
        private readonly CampaignTypeService _campaignTypeService;
        private readonly StaffService _staffService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public CampaignController(CampaignService CampaignService,
            CampaignTypeService CampaignTypeService,
            StaffService staffService,
            IWebHostEnvironment hostEnvironment,
            IMapper mapper)
        {
            _CampaignService = CampaignService;
            _campaignTypeService = CampaignTypeService;
            _staffService = staffService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

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

        public async Task<IActionResult> CampaignListAsync([FromQuery] string search)
        {
            var Campaign = (await _CampaignService.GetAllCampaignsAsync()).Where(c => string.IsNullOrEmpty(search) || c.Title.Contains(search)).ToList();
            var CampaignType = await _campaignTypeService.GetAllCampaignTypesAsync();

            foreach(var item in Campaign)
            {
                item.CampaignType = CampaignType.FirstOrDefault(x => x.Id == item.CampaignTypeId);
                item.Images = $"/img/{item.Images?.Split(',').FirstOrDefault()}";
            }

            return View(Campaign);
        }

        [Route("/campaign/campaignDetail/{id}")]
        public async Task<IActionResult> CampaignDetailAsync(string id)
        {
            if(!Guid.TryParse(id, out var campaignGuid))
            {
                return NotFound();
            }

            var Campaign = await _CampaignService.GetByIdAsync(campaignGuid);

            Campaign.CampaignType = await _campaignTypeService.GetByIdAsync(Campaign.CampaignTypeId);

            if(Campaign.Images is not null)
            {
                Campaign.Images = string.Join(",", Campaign.Images?.Split(',').Select(i => $"/img/{i}"));
            }

            return View(Campaign);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
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

        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateCampaign(CreateCampaignViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;

            if(!Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Campaign>(model);

                entity.StaffId = userGuid;

                foreach (var imgFile in model.ImageFiles)
                {
                    string uniFileName = FileUpload(imgFile);

                    entity.Images += string.IsNullOrEmpty(entity.Images) ? uniFileName : $",{uniFileName}";
                }

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

        private string FileUpload(IFormFile imgFile)
        {
            string uniFileName = null;
            if (imgFile != null)
            {
                string uploadFoler = Path.Combine(_hostEnvironment.WebRootPath, "img");
                uniFileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                string filePath = Path.Combine(uploadFoler, uniFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imgFile.CopyTo(fileStream);
                }
            }

            return uniFileName;
        }
    }
}
