using FundriasingSystem.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace FundriasingSystem.Models.Expense
{
    public class CreateExpenseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public List<SelectListItem> ExpenseTypes { get; set; }
        public Guid CampaignId { get; set; }
        public List<SelectListItem> Campaigns { get; set; }
    }
}
