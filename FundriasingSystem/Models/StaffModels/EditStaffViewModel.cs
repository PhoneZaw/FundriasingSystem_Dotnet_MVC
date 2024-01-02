using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.Staff
{
    public class EditStaffViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string HashPassword { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
