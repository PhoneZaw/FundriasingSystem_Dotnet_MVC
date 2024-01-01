using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Entities;
using FundriasingSystem.Models.ExpenseType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class ExpenseTypeController : Controller
    {
        private readonly ExpenseTypeService _ExpenseTypeService;
        private readonly IMapper _mapper;

        public ExpenseTypeController(ExpenseTypeService ExpenseTypeService,
            IMapper mapper)
        {
            _ExpenseTypeService = ExpenseTypeService;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> ViewExpenseTypeAsync()
        {
            var ExpenseType = await _ExpenseTypeService.GetAllExpenseTypesAsync();
            return View(ExpenseType);
        }

        [HttpGet]
        public ActionResult createExpenseType()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> CreateExpenseType(CreateExpenseTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<ExpenseType>(model);

                await _ExpenseTypeService.CreateExpenseTypeAsync(entity);

                return RedirectToAction("ViewExpenseType");
            }

            return View(model);
        }

        [HttpGet]
        [Route("/ExpenseType/editExpenseType/{id}")]
        public async Task<ActionResult> EditExpenseType(string id)
        {
            var ExpenseType = await _ExpenseTypeService.GetByIdAsync(Guid.Parse(id));

            var editModel = _mapper.Map<EditExpenseTypeViewModel>(ExpenseType);

            return View(editModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditExpenseType(EditExpenseTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<ExpenseType>(model);

                await _ExpenseTypeService.UpdateExpenseTypeAsync(entity);

                return RedirectToAction("ViewExpenseType");
            }

            return View(model);
        }

        [Route("/ExpenseType/Activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _ExpenseTypeService.ActivateExpenseTypeAsync(Guid.Parse(id));

            return Redirect("/ExpenseType/viewExpenseType");
        }

        [Route("/ExpenseType/Deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _ExpenseTypeService.DeactivateExpenseTypeAsync(Guid.Parse(id));

            return Redirect("/ExpenseType/viewExpenseType");
        }
    }
}
