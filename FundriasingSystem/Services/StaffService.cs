using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Helpers;
using FundraisingApp.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FundraisingApp.Services
{
    public class StaffService
    {
        private readonly IRepository<Staff> _staffRepository;
        private readonly StaffRoleService _staffRoleService;
        private readonly IMapper _mapper;

        public StaffService(IRepository<Staff> staffRepository,
            StaffRoleService staffRoleService,
            IMapper mapper)
        {
            _staffRepository = staffRepository;
            _staffRoleService = staffRoleService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Staff>> GetAllStaffAsync()
        {
            var staff = await _staffRepository.GetAllAsync();

            return staff;
        }

        public async Task<Staff> GetByIdAsync(Guid id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);

            return staff;
        }

        public async Task<Staff> CreateStaffAsync(Staff newStaff, string password)
        {
            newStaff.HashPassword = HashHelper.GetHash(password);
            newStaff.Id = Guid.NewGuid();
            newStaff.Status = StatusEnum.Active.ToString();

            var staff = await _staffRepository.CreateAsync(newStaff);

            return staff;
        }

        public async Task<Staff> UpdateStaffAsync(Staff newStaff)
        {
            var staff = await _staffRepository.UpdateAsync(newStaff);

            return staff;
        }

        public async Task<Staff> DeactivateStaffAsync(Guid id)
        {
            var existingStaff = await _staffRepository.GetByIdAsync(id);

            existingStaff.Status = StatusEnum.Inactive.ToString();

            var staff = await _staffRepository.UpdateAsync(existingStaff);

            return staff;
        }

        public async Task<Staff> ActivateStaffAsync(Guid id)
        {
            var existingStaff = await _staffRepository.GetByIdAsync(id);

            existingStaff.Status = StatusEnum.Active.ToString();

            var staff = await _staffRepository.UpdateAsync(existingStaff);

            return staff;
        }
    }
}
