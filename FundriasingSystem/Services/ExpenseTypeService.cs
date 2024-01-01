using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Repositories;
using FundriasingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class ExpenseTypeService
    {
        private readonly IRepository<ExpenseType> _ExpenseTypeRepository;
        private readonly IMapper _mapper;

        public ExpenseTypeService(IRepository<ExpenseType> ExpenseTypeRepository,
            IMapper mapper)
        {
            _ExpenseTypeRepository = ExpenseTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseType>> GetAllExpenseTypesAsync()
        {
            var ExpenseTypes = await _ExpenseTypeRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ExpenseType>>(ExpenseTypes);
        }

        public async Task<ExpenseType> GetByIdAsync(Guid id)
        {
            var ExpenseType = await _ExpenseTypeRepository.GetByIdAsync(id);

            return _mapper.Map<ExpenseType>(ExpenseType);
        }

        public async Task<ExpenseType> CreateExpenseTypeAsync(ExpenseType newExpenseType)
        {
            var ExpenseType = await _ExpenseTypeRepository.CreateAsync(newExpenseType);

            return _mapper.Map<ExpenseType>(ExpenseType);
        }

        public async Task<ExpenseType> UpdateExpenseTypeAsync(ExpenseType newExpenseType)
        {

            var ExpenseType = await _ExpenseTypeRepository.UpdateAsync(newExpenseType);

            return _mapper.Map<ExpenseType>(ExpenseType);
        }

        public async Task<ExpenseType> DeactivateExpenseTypeAsync(Guid id)
        {
            var existingExpenseType = await _ExpenseTypeRepository.GetByIdAsync(id);

            existingExpenseType.Status = StatusEnum.Inactive.ToString();

            var ExpenseType = await _ExpenseTypeRepository.UpdateAsync(existingExpenseType);

            return _mapper.Map<ExpenseType>(ExpenseType);
        }

        public async Task<ExpenseType> ActivateExpenseTypeAsync(Guid id)
        {
            var existingExpenseType = await _ExpenseTypeRepository.GetByIdAsync(id);

            existingExpenseType.Status = StatusEnum.Active.ToString();

            var ExpenseType = await _ExpenseTypeRepository.UpdateAsync(existingExpenseType);

            return _mapper.Map<ExpenseType>(ExpenseType);
        }
    }
}
