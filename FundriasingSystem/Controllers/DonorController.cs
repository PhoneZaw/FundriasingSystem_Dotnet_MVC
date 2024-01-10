using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Entities;
using FundriasingSystem.Models.Certificate;
using FundriasingSystem.Models.DonorModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class DonorController : Controller
    {
        private readonly DonorService _DonorService;
        private readonly CertificateService _certificateService;
        private readonly CampaignService _campaignService;
        private readonly DonationService _donationService;
        private readonly IMapper _mapper;

        public DonorController(DonorService DonorService,
            CertificateService certificateService,
            CampaignService campaignService,
            DonationService donationService,
            IMapper mapper)
        {
            _DonorService = DonorService;
            _certificateService = certificateService;
            _campaignService = campaignService;
            _donationService = donationService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/donors/")]
        public async Task<IActionResult> ViewDonorAsync()
        {
            var Donor = await _DonorService.GetAllDonorAsync();
            return View(Donor);
        }

        [HttpGet]
        [Route("/donors/register")]
        public ActionResult RegisterDonor()
        {

            return View();
        }


        [HttpPost]
        [Route("/donors/register")]
        public async Task<ActionResult> RegisterDonor(CreateDonorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Donor>(model);

                await _DonorService.CreateDonorAsync(entity, model.Password);

                return Redirect("/");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies")]
        [Route("/donors/edit/{id}")]
        public async Task<ActionResult> EditDonor(string id)
        {
            
            var Donor = await _DonorService.GetByIdAsync(Guid.Parse(id));

            if(Donor is null)
            {
                return NotFound();
            }

            var campaigns = await _campaignService.GetAllCampaignsAsync();

            var editModel = _mapper.Map<EditDonorViewModel>(Donor);

            var donations = (await _donationService.GetAllDonationsAsync(true)).ToList();

            var totalAmount = donations.Where(d => d.IsVerified).Sum(d => d.DonationAmount);

            var certificates = (await _certificateService.GetAllCertificatesAsync()).Where(c => c.DonorId == Donor.Id).Select(c => new CertificateViewModel
            {
                Id = c.Id,
                CampaignId = c.CampaignId,
                Campaign = campaigns.FirstOrDefault(camp => camp.Id == c.CampaignId),
                Donations = donations?.Where(d => d.CertificateId == c.Id).ToList(),
                TotalAmount = totalAmount,
                CreatedAt = c.CreatedAt,
            }).ToList();

            ViewData["Certificates"] = certificates;

            return View(editModel);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Cookies")]
        [Route("/donors/{id}")]
        public async Task<ActionResult> DonorProfile(string id)
        {
            var Donor = await _DonorService.GetByIdAsync(Guid.Parse(id));

            var editModel = _mapper.Map<EditDonorViewModel>(Donor);

            return View(editModel);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies")]
        [Route("/donors/edit/{id}")]
        public async Task<ActionResult> EditDonor(EditDonorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Donor>(model);

                await _DonorService.UpdateDonorAsync(entity);

                return RedirectToAction("ViewDonor");
            }

            return View(model);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/donors/activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _DonorService.ActivateDonorAsync(Guid.Parse(id));

            return Redirect("/Donor/viewDonor");
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/donors/deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _DonorService.DeactivateDonorAsync(Guid.Parse(id));

            return Redirect("/Donor/viewDonor");
        }
    }
}
