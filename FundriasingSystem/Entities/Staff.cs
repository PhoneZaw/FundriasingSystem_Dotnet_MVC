using FundraisingApp.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace FundraisingApp.Entities
{
    public class Staff : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string HashPassword { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public StaffRole Role { get; set; }
    }
}
