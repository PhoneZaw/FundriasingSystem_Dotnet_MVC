using AutoMapper;
using FundraisingApp.Services;
using FundriasingSystem.Entities;
using FundriasingSystem.Models.SuggestionModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FundriasingSystem.Controllers
{
    public class SuggestionController : Controller
    {
        private readonly SuggestionService _SuggestionService;
        private readonly IMapper _mapper;

        public SuggestionController(SuggestionService SuggestionService,
            IMapper mapper)
        {
            _SuggestionService = SuggestionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("/admin/Suggestions/create")]
        public async Task<ActionResult> CreateSuggestion(CreateSuggestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Suggestion>(model);

                await _SuggestionService.CreateSuggestionAsync(entity);

                return Redirect($"/campaigns/{model.CampaignId}");
            }

            return Redirect($"/campaigns/{model.CampaignId}");
        }
    }
}
