using FundraisingApp.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace FundriasingSystem.Models.Staff
{
    public class CreateStaffViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string HashPassword { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public string Password { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
