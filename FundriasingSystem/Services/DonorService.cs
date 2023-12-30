using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Helpers;
using FundraisingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class DonorService
    {
        private readonly IRepository<Donor> _DonorRepository;
        private readonly IMapper _mapper;

        public DonorService(IRepository<Donor> DonorRepository,
            IMapper mapper)
        {
            _DonorRepository = DonorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Donor>> GetAllDonorAsync()
        {
            var Donor = await _DonorRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Donor>>(Donor);
        }

        public async Task<Donor> GetByIdAsync(Guid id)
        {
            var Donor = await _DonorRepository.GetByIdAsync(id);

            return _mapper.Map<Donor>(Donor);
        }

        public async Task<Donor> CreateDonorAsync(Donor newDonor, string password)
        {
            newDonor.HashPassword = HashHelper.GetHash(password);
            newDonor.Id = Guid.NewGuid();
            newDonor.Status = StatusEnum.Active.ToString();

            var Donor = await _DonorRepository.CreateAsync(newDonor);

            return _mapper.Map<Donor>(Donor);
        }

        public async Task<Donor> UpdateDonorAsync(Donor newDonor)
        {
            var entity = await _DonorRepository.GetByIdAsync(newDonor.Id);

            var Donor = await _DonorRepository.UpdateAsync(newDonor);

            Donor.HashPassword = entity.HashPassword;

            return _mapper.Map<Donor>(Donor);
        }

        public async Task<Donor> DeactivateDonorAsync(Guid id)
        {
            var existingDonor = await _DonorRepository.GetByIdAsync(id);

            existingDonor.Status = StatusEnum.Inactive.ToString();

            var Donor = await _DonorRepository.UpdateAsync(existingDonor);

            return _mapper.Map<Donor>(Donor);
        }

        public async Task<Donor> ActivateDonorAsync(Guid id)
        {
            var existingDonor = await _DonorRepository.GetByIdAsync(id);

            existingDonor.Status = StatusEnum.Active.ToString();

            var Donor = await _DonorRepository.UpdateAsync(existingDonor);

            return _mapper.Map<Donor>(Donor);
        }
    }
}
