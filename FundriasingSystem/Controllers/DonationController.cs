using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Exceptions;
using FundraisingApp.Services;
using FundriasingSystem.Models.CampaignModels;
using FundriasingSystem.Models.DonationModels;
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
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class DonationController : Controller
    {
        private readonly DonationService _DonationService;
        private readonly DonorService _donorService;
        private readonly CampaignService _campaignService;
        private readonly PaymentMethodService _paymentMethodService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public DonationController(DonationService DonationService,
            DonorService donorService,
            CampaignService campaignService,
            PaymentMethodService paymentMethodService,
            IWebHostEnvironment hostEnvironment,
            IMapper mapper)
        {
            _DonationService = DonationService;
            _donorService = donorService;
            _campaignService = campaignService;
            _paymentMethodService = paymentMethodService;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/donations/")]
        public async Task<IActionResult> ViewDonationAsync()
        {
            var Donation = await _DonationService.GetAllDonationsAsync();

            var Donors = await _donorService.GetAllDonorAsync();
            var Campaigns = await _campaignService.GetAllCampaignsAsync();
            var PaymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();

            foreach (var item in Donation)
            {
                item.Donor = Donors.FirstOrDefault(d => d.Id == item.DonorId);
                item.Campaign = Campaigns.FirstOrDefault(c => c.Id == item.CampaignId);
                item.PaymentMethod = PaymentMethods.FirstOrDefault(p => p.Id == item.PaymentMethodId);
            }

            return View(Donation);
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [Route("/donations/donate")]
        [HttpGet]
        public async Task<ActionResult> createDonationAsync([FromQuery] CreateDonationViewModel donationQuery)
        {
            List<SelectListItem> campaignItems = (await _campaignService.GetAllCampaignsAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Title,
                    Value = d.Id.ToString()
                };
            });

            var model = new CreateDonationViewModel()
            {
                Campaigns = campaignItems,
                PaymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync(),
                DonationAmount = donationQuery.DonationAmount,
                CampaignId = donationQuery.CampaignId
            };
            return View(model);
        }

        [Authorize(AuthenticationSchemes = "Cookies")]
        [Route("/donations/donate")]
        [HttpPost]
        public async Task<ActionResult> CreateDonation(CreateDonationViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;

            if (!Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Donation>(model);

                entity.DonorId = userGuid;

                if(model.PaymentVoucherFile is not null)
                {
                    string uniFileName = FileUpload(model.PaymentVoucherFile);

                    entity.PaymentVoucherUrl = uniFileName;
                }

                await _DonationService.CreateDonationAsync(entity);

                return RedirectToAction("CampaignList", "Campaign");
            }

            return View(model);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/donations/verify/{id}")]
        [HttpGet]
        public async Task<ActionResult> VerifyDonation(string id)
        {
            if (ModelState.IsValid)
            {
                if(Guid.TryParse(id, out var donationId))
                {
                    await _DonationService.VerifyDonationAsync(donationId);

                    return RedirectToAction("ViewDonation");
                }
            }

            throw new DomainException("Verify Failed");
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
