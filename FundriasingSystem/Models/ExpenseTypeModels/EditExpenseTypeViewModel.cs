using System;

namespace FundriasingSystem.Models.ExpenseType
{
    public class EditExpenseTypeViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ExpenseTypeName { get; set; }
    }
}
