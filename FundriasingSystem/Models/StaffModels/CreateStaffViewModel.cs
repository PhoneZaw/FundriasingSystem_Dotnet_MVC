using FundraisingApp.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.Staff
{
    public class CreateStaffViewModel
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        [Required]
        public string Password { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
