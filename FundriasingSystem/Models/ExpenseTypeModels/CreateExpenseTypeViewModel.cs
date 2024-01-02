using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.ExpenseType
{
    public class CreateExpenseTypeViewModel
    {
        [Required]
        public string ExpenseTypeName { get; set; }
    }
}
