using FundriasingSystem.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FundriasingSystem.Models.Expense
{
    public class CreateExpenseViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public Guid ExpenseTypeId { get; set; }
        public List<SelectListItem> ExpenseTypes { get; set; }
        [Required]
        public Guid CampaignId { get; set; }
        public List<SelectListItem> Campaigns { get; set; }
    }
}
