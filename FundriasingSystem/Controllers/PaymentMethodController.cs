using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.PaymentMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly PaymentMethodService _PaymentMethodService;
        private readonly IMapper _mapper;

        public PaymentMethodController(PaymentMethodService PaymentMethodService,
            IMapper mapper)
        {
            _PaymentMethodService = PaymentMethodService;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> ViewPaymentMethodAsync()
        {
            var PaymentMethod = await _PaymentMethodService.GetAllPaymentMethodsAsync();
            return View(PaymentMethod);
        }

        [HttpGet]
        public ActionResult createPaymentMethod()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> CreatePaymentMethod(CreatePaymentMethodViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<PaymentMethod>(model);

                await _PaymentMethodService.CreatePaymentMethodAsync(entity);

                return RedirectToAction("ViewPaymentMethod");
            }

            return View(model);
        }

        [HttpGet]
        [Route("/PaymentMethod/editPaymentMethod/{id}")]
        public async Task<ActionResult> EditPaymentMethod(string id)
        {
            var PaymentMethod = await _PaymentMethodService.GetByIdAsync(Guid.Parse(id));

            var editModel = _mapper.Map<EditPaymentMethodViewModel>(PaymentMethod);

            return View(editModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditPaymentMethod(EditPaymentMethodViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<PaymentMethod>(model);

                await _PaymentMethodService.UpdatePaymentMethodAsync(entity);

                return RedirectToAction("ViewPaymentMethod");
            }

            return View(model);
        }

        [Route("/PaymentMethod/Activate/{id}")]
        public async Task<IActionResult> Activate(string id)
        {

            await _PaymentMethodService.ActivatePaymentMethodAsync(Guid.Parse(id));

            return Redirect("/PaymentMethod/viewPaymentMethod");
        }

        [Route("/PaymentMethod/Deactivate/{id}")]
        public async Task<IActionResult> Deactivate(string id)
        {

            await _PaymentMethodService.DeactivatePaymentMethodAsync(Guid.Parse(id));

            return Redirect("/PaymentMethod/viewPaymentMethod");
        }
    }
}
