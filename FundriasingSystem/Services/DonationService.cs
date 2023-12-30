using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class DonationService
    {
        private readonly IRepository<Donation> _DonationRepository;
        private readonly IMapper _mapper;

        public DonationService(IRepository<Donation> DonationRepository,
            IMapper mapper)
        {
            _DonationRepository = DonationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Donation>> GetAllDonationsAsync()
        {
            var Donations = await _DonationRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Donation>>(Donations);
        }

        public async Task<IEnumerable<Donation>> GetAllDonationsByCampaignAsync(Guid campaignId)
        {
            var Donations = (await _DonationRepository.GetAllAsync()).Where(d => d.CampaignId == campaignId);

            return _mapper.Map<IEnumerable<Donation>>(Donations);
        }

        public async Task<Donation> GetByIdAsync(Guid id)
        {
            var Donation = await _DonationRepository.GetByIdAsync(id);

            return _mapper.Map<Donation>(Donation);
        }

        public async Task<Donation> CreateDonationAsync(Donation newDonation)
        {
            var Donation = await _DonationRepository.CreateAsync(newDonation);

            return _mapper.Map<Donation>(Donation);
        }

        public async Task<Donation> VerifyDonationAsync(Guid donationId)
        {
            var Donation = await _DonationRepository.GetByIdAsync(donationId);

            if(Donation is not null)
            {
                Donation.IsVerified = true;
                Donation = await _DonationRepository.UpdateAsync(Donation);
            }

            return _mapper.Map<Donation>(Donation);
        }
    }
}
