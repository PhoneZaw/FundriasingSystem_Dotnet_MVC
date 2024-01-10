using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.CampaignModels;
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
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class CampaignController : Controller
    {
        private readonly CampaignService _CampaignService;
        private readonly CampaignTypeService _campaignTypeService;
        private readonly StaffService _staffService;
        private readonly DonationService _donationService;
        private readonly SuggestionService _suggestionService;
        private readonly DonorService _donorService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public CampaignController(CampaignService CampaignService,
            CampaignTypeService CampaignTypeService,
            StaffService staffService,
            DonationService donationService,
            SuggestionService suggestionService,
            DonorService donorService,
            IWebHostEnvironment hostEnvironment,
            IMapper mapper)
        {
            _CampaignService = CampaignService;
            _campaignTypeService = CampaignTypeService;
            _staffService = staffService;
            _donationService = donationService;
            _suggestionService = suggestionService;
            _donorService = donorService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        [Route("/admin/campaigns/")]
        [Authorize(AuthenticationSchemes = "Admin")]
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

        [Route("/campaigns")]
        public async Task<IActionResult> CampaignListAsync([FromQuery] string search)
        {
            var Campaign = (await _CampaignService.GetAllCampaignsAsync())
                .Where(c => c.TargetDate >= DateTime.Now)
                .Where(c => string.IsNullOrEmpty(search) || c.Title.Contains(search))
                .ToList();

            var CampaignType = await _campaignTypeService.GetAllCampaignTypesAsync();

            var Donations = await _donationService.GetAllDonationsAsync();

            foreach(var item in Campaign)
            {
                item.CampaignType = CampaignType.FirstOrDefault(x => x.Id == item.CampaignTypeId);
                item.Images = $"/img/{item.Images?.Split(',').FirstOrDefault()}";

                item.Description = Donations.Where(d => d.CampaignId == item.Id).Sum(d => d.DonationAmount).ToString();
            }

            Campaign = Campaign.Where(c => c.TargetAmount > Convert.ToDecimal(c.Description)).ToList();

            return View(Campaign);
        }

        [HttpPost]
        [Route("/campaigns/search")]
        public IActionResult SearchCampaign(string search)
        {
            return Redirect($"/campaigns?search={search}");
        }

        [Route("/campaigns/{id}")]
        public async Task<IActionResult> CampaignDetailAsync(string id)
        {
            if(!Guid.TryParse(id, out var campaignGuid))
            {
                return NotFound();
            }

            var Campaign = await _CampaignService.GetByIdAsync(campaignGuid);

            if(Campaign is null)
            {
                return NotFound();
            }

            Campaign.CampaignType = await _campaignTypeService.GetByIdAsync(Campaign.CampaignTypeId);

            var donors = await _donorService.GetAllDonorAsync();

            if(Campaign.Images is not null)
            {
                Campaign.Images = string.Join(",", Campaign.Images?.Split(',').Select(i => $"/img/{i}"));
            }

            var donations = await _donationService.GetAllDonationsByCampaignAsync(Campaign.Id);

            foreach (var item in donations)
            {
                item.Donor = donors.FirstOrDefault();
            }

            var suggestions = await _suggestionService.GetAllSuggestionsByCampaignIdAsync(Campaign.Id);

            foreach (var item in suggestions)
            {
                item.Donor = donors.FirstOrDefault(d => d.Id == item.DonorId);

                item.TimeDiff = $"{(int)(DateTime.Now - item.CreatedAt).TotalMinutes} min";

                item.TotalDonationAmount = donations.Where(d => d.CampaignId == Campaign.Id && d.DonorId == item.DonorId).Sum(d => d.DonationAmount);
            }

            ViewData["suggestions"] = suggestions;

            ViewData["donations"] = donations.Take(3).ToList();

            ViewData["donationCount"] = donations.Count();

            ViewData["CurrentAmount"] = donations.Sum(d => d.DonationAmount);

            return View(Campaign);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpGet]
        [Route("/admin/campaigns/create")]
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
                CampaignTypes = campaignTypeItems,
                TargetDate = DateTime.Now.AddDays(30),
                TargetAmount = 10000
            };
            return View(model);
        }

        [Route("/admin/campaigns/create")]
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

                if(model.ImageFiles is not null)
                {
                    foreach (var imgFile in model.ImageFiles)
                    {
                        string uniFileName = FileUpload(imgFile);

                        entity.Images += string.IsNullOrEmpty(entity.Images) ? uniFileName : $",{uniFileName}";
                    }
                }
                

                await _CampaignService.CreateCampaignAsync(entity);

                return RedirectToAction("ViewCampaign");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaigns/edit/{id}")]
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
        //[Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/campaigns/edit/{id}")]
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

        [Route("/admin/Campaigns/Activate/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Activate(string id)
        {

            await _CampaignService.ActivateCampaignAsync(Guid.Parse(id));

            return Redirect("/Campaign/viewCampaign");
        }

        [Route("/admin/Campaigns/Deactivate/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
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
