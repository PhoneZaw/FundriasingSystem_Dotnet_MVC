using AutoMapper;
using FundraisingApp.Repositories;
using FundriasingSystem.Entities;
using FundriasingSystem.Models.SuggestionModels;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class SuggestionService
    {
        private readonly IRepository<Suggestion> _SuggestionRepository;
        private readonly IMapper _mapper;

        public SuggestionService(IRepository<Suggestion> SuggestionRepository,
            IMapper mapper)
        {
            _SuggestionRepository = SuggestionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Suggestion>> GetAllSuggestionsAsync()
        {
            var Suggestions = await _SuggestionRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Suggestion>>(Suggestions);
        }

        public async Task<IEnumerable<SuggestionViewModel>> GetAllSuggestionsByCampaignIdAsync(Guid campaignId)
        {
            var Suggestions = (await _SuggestionRepository.GetAllAsync()).Where(s => s.CampaignId == campaignId).ToList();

            var suggestionViewModel = _mapper.Map<IEnumerable<SuggestionViewModel>>(Suggestions);

            return suggestionViewModel;
        }

        public async Task<Suggestion> GetByIdAsync(Guid id)
        {
            var Suggestion = await _SuggestionRepository.GetByIdAsync(id);

            return _mapper.Map<Suggestion>(Suggestion);
        }

        public async Task<Suggestion> CreateSuggestionAsync(Suggestion newSuggestion)
        {
            var Suggestion = await _SuggestionRepository.CreateAsync(newSuggestion);

            return _mapper.Map<Suggestion>(Suggestion);
        }
    }
}
