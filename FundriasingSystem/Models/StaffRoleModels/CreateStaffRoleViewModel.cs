using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.StaffRole
{
    public class CreateStaffRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
