using System;

namespace FundriasingSystem.Models.StaffRole
{
    public class EditStaffRoleViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RoleName { get; set; }
    }
}
