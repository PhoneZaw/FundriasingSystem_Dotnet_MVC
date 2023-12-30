using AutoMapper;
using FundraisingApp.Entities;
using FundraisingApp.Enums;
using FundraisingApp.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundraisingApp.Services
{
    public class PaymentMethodService
    {
        private readonly IRepository<PaymentMethod> _PaymentMethodRepository;
        private readonly IMapper _mapper;

        public PaymentMethodService(IRepository<PaymentMethod> PaymentMethodRepository,
            IMapper mapper)
        {
            _PaymentMethodRepository = PaymentMethodRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var PaymentMethods = await _PaymentMethodRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<PaymentMethod>>(PaymentMethods);
        }

        public async Task<PaymentMethod> GetByIdAsync(Guid id)
        {
            var PaymentMethod = await _PaymentMethodRepository.GetByIdAsync(id);

            return _mapper.Map<PaymentMethod>(PaymentMethod);
        }

        public async Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethod newPaymentMethod)
        {
            var PaymentMethod = await _PaymentMethodRepository.CreateAsync(newPaymentMethod);

            return _mapper.Map<PaymentMethod>(PaymentMethod);
        }

        public async Task<PaymentMethod> UpdatePaymentMethodAsync(PaymentMethod newPaymentMethod)
        {

            var PaymentMethod = await _PaymentMethodRepository.UpdateAsync(newPaymentMethod);

            return _mapper.Map<PaymentMethod>(PaymentMethod);
        }

        public async Task<PaymentMethod> DeactivatePaymentMethodAsync(Guid id)
        {
            var existingPaymentMethod = await _PaymentMethodRepository.GetByIdAsync(id);

            //if (await IsActiveUserExistAsync(existingPaymentMethod.Name))
            //{
            //    throw new Exception("User is in PaymentMethod");
            //}

            existingPaymentMethod.Status = StatusEnum.Inactive.ToString();

            var PaymentMethod = await _PaymentMethodRepository.UpdateAsync(existingPaymentMethod);

            return _mapper.Map<PaymentMethod>(PaymentMethod);
        }

        public async Task<PaymentMethod> ActivatePaymentMethodAsync(Guid id)
        {
            var existingPaymentMethod = await _PaymentMethodRepository.GetByIdAsync(id);

            existingPaymentMethod.Status = StatusEnum.Active.ToString();

            var PaymentMethod = await _PaymentMethodRepository.UpdateAsync(existingPaymentMethod);

            return _mapper.Map<PaymentMethod>(PaymentMethod);
        }
    }
}
