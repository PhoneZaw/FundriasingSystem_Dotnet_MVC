using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.Campaign;
using FundriasingSystem.Models.Donation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public DonationController(DonationService DonationService,
            DonorService donorService,
            CampaignService campaignService,
            PaymentMethodService paymentMethodService,
            IMapper mapper)
        {
            _DonationService = DonationService;
            _donorService = donorService;
            _campaignService = campaignService;
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
        }

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

        [HttpGet]
        public async Task<ActionResult> createDonationAsync()
        {

            List<SelectListItem> campaignItems = (await _campaignService.GetAllCampaignsAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Title,
                    Value = d.Id.ToString()
                };
            });

            List<SelectListItem> paymentMethodItems = (await _paymentMethodService.GetAllPaymentMethodsAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.PaymentMethodName,
                    Value = d.Id.ToString()
                };
            });

            var model = new CreateDonationViewModel()
            {
                Campaigns = campaignItems,
                PaymentMethods = paymentMethodItems
            };
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> CreateDonation(CreateDonationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Donation>(model);

                await _DonationService.CreateDonationAsync(entity);

                return RedirectToAction("ViewDonation");
            }

            return View(model);
        }
    }
}
