using System;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.StaffRole
{
    public class EditStaffRoleViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
