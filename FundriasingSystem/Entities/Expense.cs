using FundraisingApp.Entities;
using System;

namespace FundriasingSystem.Entities
{
    public class Expense : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public Guid CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public Guid StaffId { get; set; }
    }
}
