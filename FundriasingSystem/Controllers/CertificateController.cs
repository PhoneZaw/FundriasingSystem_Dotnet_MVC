using AutoMapper;
using FundraisingApp.Services;
using FundriasingSystem.Entities;
using FundriasingSystem.Models.Certificate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class CertificateController : Controller
    {
        private readonly CertificateService _certificateService;
        private readonly DonorService _donorService;
        private readonly CampaignService _campaignService;
        private readonly DonationService _donationService;
        private readonly IMapper _mapper;

        public CertificateController(CertificateService certificateService,
            DonorService donorService,
            CampaignService campaignService,
            DonationService donationService,
            IMapper mapper)
            
        {
            _certificateService = certificateService;
            _donorService = donorService;
            _campaignService = campaignService;
            _donationService = donationService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/certificates/{id}")]
        public async Task<IActionResult> ViewCertificateAsync(Guid id)
        {
            var certificate = await _certificateService.GetByIdAsync(id);

            if(certificate is null)
            {
                return NotFound();
            }

            var model = _mapper.Map<CertificateViewModel>(certificate);

            model.Donor = await _donorService.GetByIdAsync(certificate.DonorId);

            model.Campaign = await _campaignService.GetByIdAsync(certificate.CampaignId);

            model.Donations = (await _donationService.GetAllDonationsByCampaignAsync(certificate.CampaignId)).ToList();

            model.TotalAmount = model.Donations.Sum(d => d.DonationAmount);

            if(model.TotalAmount <= 0)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        [Route("/certificates/validate")]
        public IActionResult ValidateCertificate()
        {

            return View();
        }

        [HttpPost]
        [Route("/certificates/validate")]
        public async Task<IActionResult> ValidateCertificate(Certificate model)
        {
            var cert = await _certificateService.GetByIdAsync(model.Id);

            if(cert is not null)
            {
                return Redirect($"/certificates/{cert.Id}");
            }

            ModelState.AddModelError("Id", "Please enter a valid certificate number");
            return View(model);
        }
    }
}
