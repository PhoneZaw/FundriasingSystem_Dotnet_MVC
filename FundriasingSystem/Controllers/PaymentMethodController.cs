using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Services;
using FundriasingSystem.Models.PaymentMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly PaymentMethodService _PaymentMethodService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public PaymentMethodController(PaymentMethodService PaymentMethodService,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _PaymentMethodService = PaymentMethodService;
            _hostEnvironment = webHostEnvironment;
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

                if (model.IconFile is not null)
                {
                        string uniFileName = FileUpload(model.IconFile);

                        entity.PaymentMethodIconUrl = uniFileName;
                }

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

        private string FileUpload(IFormFile imgFile)
        {
            string uniFileName = null;
            if (imgFile != null)
            {
                string uploadFoler = Path.Combine(_hostEnvironment.WebRootPath, "img");
                uniFileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                string filePath = Path.Combine(uploadFoler, uniFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imgFile.CopyTo(fileStream);
                }
            }

            return uniFileName;
        }
    }
}
