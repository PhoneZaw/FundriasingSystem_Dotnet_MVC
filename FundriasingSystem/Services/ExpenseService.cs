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
    public class ExpenseService
    {
        private readonly IRepository<Expense> _ExpenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IRepository<Expense> ExpenseRepository,
            IMapper mapper)
        {
            _ExpenseRepository = ExpenseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            var Expenses = await _ExpenseRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<Expense>>(Expenses);
        }

        public async Task<Expense> GetByIdAsync(Guid id)
        {
            var Expense = await _ExpenseRepository.GetByIdAsync(id);

            return _mapper.Map<Expense>(Expense);
        }

        public async Task<Expense> CreateExpenseAsync(Expense newExpense)
        {
            var Expense = await _ExpenseRepository.CreateAsync(newExpense);

            return _mapper.Map<Expense>(Expense);
        }

        public async Task<Expense> UpdateExpenseAsync(Expense newExpense)
        {

            var Expense = await _ExpenseRepository.UpdateAsync(newExpense);

            return _mapper.Map<Expense>(Expense);
        }

        public async Task DeleteExpenseAsync(Guid id)
        {

            await _ExpenseRepository.DeleteAsync(id);

            return;
        }


    }
}
