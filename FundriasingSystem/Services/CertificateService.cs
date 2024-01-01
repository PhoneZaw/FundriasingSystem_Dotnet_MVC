using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Repositories;
using FundriasingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class CertificateService
    {
        private readonly IRepository<Certificate> _CertificateRepository;
        private readonly IMapper _mapper;

        public CertificateService(IRepository<Certificate> CertificateRepository,
            IMapper mapper)
        {
            _CertificateRepository = CertificateRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Certificate>> GetAllCertificatesAsync()
        {
            var Certificates = await _CertificateRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Certificate>>(Certificates);
        }

        public async Task<Certificate> GetByIdAsync(Guid id)
        {
            var Certificate = await _CertificateRepository.GetByIdAsync(id);

            return _mapper.Map<Certificate>(Certificate);
        }

        public async Task<Certificate> CreateCertificateAsync(Certificate newCertificate)
        {
            var Certificate = await _CertificateRepository.CreateAsync(newCertificate);

            return _mapper.Map<Certificate>(Certificate);
        }

        public async Task<Certificate> UpdateCertificateAsync(Certificate newCertificate)
        {

            var Certificate = await _CertificateRepository.UpdateAsync(newCertificate);

            return _mapper.Map<Certificate>(Certificate);
        }

        public async Task<Certificate> DeactivateCertificateAsync(Guid id)
        {
            var existingCertificate = await _CertificateRepository.GetByIdAsync(id);

            existingCertificate.Status = StatusEnum.Inactive.ToString();

            var Certificate = await _CertificateRepository.UpdateAsync(existingCertificate);

            return _mapper.Map<Certificate>(Certificate);
        }

        public async Task<Certificate> ActivateCertificateAsync(Guid id)
        {
            var existingCertificate = await _CertificateRepository.GetByIdAsync(id);

            existingCertificate.Status = StatusEnum.Active.ToString();

            var Certificate = await _CertificateRepository.UpdateAsync(existingCertificate);

            return _mapper.Map<Certificate>(Certificate);
        }
    }
}
