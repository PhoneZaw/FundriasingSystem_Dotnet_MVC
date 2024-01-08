using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Entities;
using FundriasingSystem.Models.CampaignModels;
using FundriasingSystem.Models.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ExpenseService _ExpenseService;
        private readonly ExpenseTypeService _expenseTypeService;
        private readonly CampaignService _campaignService;
        private readonly StaffService _staffService;
        private readonly DonationService _donationService;
        private readonly IMapper _mapper;

        public ExpenseController(ExpenseService ExpenseService,
            ExpenseTypeService expenseTypeService,
            CampaignService campaignService,
            StaffService staffService,
            DonationService donationService,
            IMapper mapper)
        {
            _ExpenseService = ExpenseService;
            _expenseTypeService = expenseTypeService;
            _campaignService = campaignService;
            _staffService = staffService;
            _donationService = donationService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/expenses")]
        public async Task<IActionResult> ViewExpenseAsync([FromQuery] Guid? CampaignId)
        {
            var Campaigns = await _campaignService.GetAllCampaignsAsync();

            if (CampaignId is null)
            {
                CampaignId = Campaigns.FirstOrDefault()?.Id;
            }

            var Expenses = (await _ExpenseService.GetAllExpensesAsync()).Where(e => e.CampaignId == CampaignId);

            var ExpenseTypes = await _expenseTypeService.GetAllExpenseTypesAsync();
            var Staff = await _staffService.GetAllStaffAsync();

            var donations = await _donationService.GetAllDonationsByCampaignAsync(CampaignId.Value);

            foreach (var item in Expenses)
            {
                item.Campaign = Campaigns.FirstOrDefault(x => x.Id == item.CampaignId);
                item.ExpenseType = ExpenseTypes.FirstOrDefault(x => x.Id == item.ExpenseTypeId);
                //item.Staff = Staff.FirstOrDefault(x => x.Id == item.StaffId);
            }


            ViewData["CampaignList"] = Campaigns;
            ViewData["CampaignId"] = CampaignId;
            ViewData["totalDonationAmount"] = donations.Sum(d => d.DonationAmount);
            ViewData["campaignTargetAmount"] = Campaigns.FirstOrDefault(x => x.Id == CampaignId).TargetAmount;

            return View(Expenses);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/expenses/create")]
        [HttpGet]
        public async Task<ActionResult> createExpenseAsync([FromQuery] Guid CampaignId)
        {
            List<SelectListItem> expenseTypeItems = (await _expenseTypeService.GetAllExpenseTypesAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.ExpenseTypeName,
                    Value = d.Id.ToString()
                };
            });

            List<SelectListItem> campaignItems = (await _campaignService.GetAllCampaignsAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Title,
                    Value = d.Id.ToString()
                };
            });

            var model = new CreateExpenseViewModel()
            {
                Campaigns = campaignItems,
                ExpenseTypes = expenseTypeItems,
                CampaignId = CampaignId
            };

            return View(model);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/expenses/create")]
        [HttpPost]
        public async Task<ActionResult> CreateExpense(CreateExpenseViewModel model)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;

            if (!Guid.TryParse(userId, out var userGuid))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Expense>(model);

                entity.StaffId = userGuid;

                await _ExpenseService.CreateExpenseAsync(entity);

                return RedirectToAction("ViewExpense");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/expenses/edit/{id}")]
        public async Task<ActionResult> EditExpense(string id)
        {
            var Expense = await _ExpenseService.GetByIdAsync(Guid.Parse(id));

            List<SelectListItem> expenseTypeItems = (await _expenseTypeService.GetAllExpenseTypesAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.ExpenseTypeName,
                    Value = d.Id.ToString()
                };
            });

            List<SelectListItem> campaignItems = (await _campaignService.GetAllCampaignsAsync()).ToList().ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Title,
                    Value = d.Id.ToString()
                };
            });

            var editModel = _mapper.Map<EditExpenseViewModel>(Expense);

            editModel.Campaigns = campaignItems;

            editModel.ExpenseTypes = expenseTypeItems;

            return View(editModel);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/expenses/edit/{id}")]
        public async Task<ActionResult> EditExpense(EditExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Expense>(model);

                await _ExpenseService.UpdateExpenseAsync(entity);

                return RedirectToAction("ViewExpense");
            }

            return View(model);
        }
    }
}
