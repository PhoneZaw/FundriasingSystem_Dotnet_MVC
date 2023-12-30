using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Exceptions;
using FundraisingApp.Repositories;
using Microsoft.AspNetCore.Identity;

namespace FundraisingApp.Services
{
    public class StaffRoleService
    {
        private readonly IRepository<StaffRole> _staffRoleRepository;
        private readonly IRepository<Staff> _staffRepository;
        private readonly IMapper _mapper;

        public StaffRoleService(IRepository<StaffRole> staffRoleRepository,
            IRepository<Staff> staffRepository,
            IMapper mapper)
        {
            _staffRoleRepository = staffRoleRepository;
            _staffRepository = staffRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StaffRole>> GetAllRolesAsync()
        {
            var roles = await _staffRoleRepository.GetAllAsync();

            return roles;
        }

        public async Task<StaffRole> GetByIdAsync(Guid id)
        {
            var role = await _staffRoleRepository.GetByIdAsync(id);

            return role;
        }

        public async Task<StaffRole> CreateRoleAsync(StaffRole newStaffRole)
        {
            if(await IsDuplicateRoleNameExistAsync(newStaffRole))
            {
                throw new DomainException("Duplicate role name exist");
            };

            var role = await _staffRoleRepository.CreateAsync(newStaffRole);

            return role;
        }

        public async Task<StaffRole> UpdateRoleAsync(StaffRole newStaffRole)
        {
            if (await IsDuplicateRoleNameExistAsync(newStaffRole))
            {
                throw new Exception("Duplicate role name exist");
            }

            var role = await _staffRoleRepository.UpdateAsync(newStaffRole);

            return role;
        }

        public async Task<StaffRole> DeactivateRoleAsync(Guid id)
        {
            var existingRole = await _staffRoleRepository.GetByIdAsync(id);

            if (await IsActiveUserExistAsync(existingRole))
            {
                throw new Exception("User is in role");
            }

            existingRole.Status = StatusEnum.Inactive.ToString();

            var role = await _staffRoleRepository.UpdateAsync(existingRole);

            return role;
        }

        public async Task<StaffRole> ActivateRoleAsync(Guid id)
        {
            var existingRole = await _staffRoleRepository.GetByIdAsync(id);

            existingRole.Status = StatusEnum.Active.ToString();

            var role = await _staffRoleRepository.UpdateAsync(existingRole);

            return role;
        }

        public async Task<bool> IsDuplicateRoleNameExistAsync(StaffRole staffRole)
        {
            var existingRole = (await _staffRoleRepository.GetAllAsync()).FirstOrDefault(r => r.RoleName == staffRole.RoleName && r.Id != staffRole.Id);

            return existingRole != null;
        }

        public async Task<bool> IsActiveUserExistAsync(StaffRole staffRole)
        {
            var existingUsers = (await _staffRepository.GetAllAsync()).FirstOrDefault(s => s.RoleId == staffRole.Id);

            return existingUsers != null;
        }
    }
}
