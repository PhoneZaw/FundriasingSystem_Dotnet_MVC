using System;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.ExpenseType
{
    public class EditExpenseTypeViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string ExpenseTypeName { get; set; }
    }
}
