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
        private readonly IMapper _mapper;

        public ExpenseController(ExpenseService ExpenseService,
            ExpenseTypeService expenseTypeService,
            CampaignService campaignService,
            StaffService staffService,
            IMapper mapper)
        {
            _ExpenseService = ExpenseService;
            _expenseTypeService = expenseTypeService;
            _campaignService = campaignService;
            _staffService = staffService;
            _mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/expenses")]
        public async Task<IActionResult> ViewExpenseAsync()
        {
            var Expenses = await _ExpenseService.GetAllExpensesAsync();

            var Campaigns = await _campaignService.GetAllCampaignsAsync();
            var ExpenseTypes = await _expenseTypeService.GetAllExpenseTypesAsync();
            var Staff = await _staffService.GetAllStaffAsync();

            foreach (var item in Expenses)
            {
                item.Campaign = Campaigns.FirstOrDefault(x => x.Id == item.CampaignId);
                item.ExpenseType = ExpenseTypes.FirstOrDefault(x => x.Id == item.ExpenseTypeId);
                //item.Staff = Staff.FirstOrDefault(x => x.Id == item.StaffId);
            }

            return View(Expenses);
        }

        [Authorize(AuthenticationSchemes = "Admin")]
        [Route("/admin/expenses/create")]
        [HttpGet]
        public async Task<ActionResult> createExpenseAsync()
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
                ExpenseTypes = expenseTypeItems
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
        [Route("/admin/expenses/edit")]
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
