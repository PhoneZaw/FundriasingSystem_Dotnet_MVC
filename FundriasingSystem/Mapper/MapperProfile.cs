using AutoMapper;
using FundraisingApp.Entities;
using FundriasingSystem.Entities;
using FundriasingSystem.Models.CampaignModels;
using FundriasingSystem.Models.CampaignType;
using FundriasingSystem.Models.Certificate;
using FundriasingSystem.Models.DonationModels;
using FundriasingSystem.Models.DonorModels;
using FundriasingSystem.Models.Expense;
using FundriasingSystem.Models.ExpenseType;
using FundriasingSystem.Models.PaymentMethod;
using FundriasingSystem.Models.Staff;
using FundriasingSystem.Models.StaffRole;

namespace FundraisingApp.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateStaffViewModel, Staff>();

            CreateMap<EditStaffViewModel, Staff>().ReverseMap();

            CreateMap<CreateDonorViewModel, Donor>();

            CreateMap<EditDonorViewModel, Donor>().ReverseMap();

            CreateMap<CreateCampaignTypeViewModel, CampaignType>();

            CreateMap<EditCampaignTypeViewModel, CampaignType>().ReverseMap();

            CreateMap<CreateCampaignViewModel, Campaign>();

            CreateMap<EditCampaignViewModel, Campaign>().ReverseMap();

            CreateMap<CreateDonationViewModel, Donation>();

            CreateMap<CreatePaymentMethodViewModel, PaymentMethod>();

            CreateMap<EditPaymentMethodViewModel, PaymentMethod>().ReverseMap();

            CreateMap<CreateStaffRoleViewModel, StaffRole>();

            CreateMap<EditStaffRoleViewModel, StaffRole>().ReverseMap();

            CreateMap<CreateExpenseTypeViewModel, ExpenseType>();

            CreateMap<EditExpenseTypeViewModel, ExpenseType>().ReverseMap();

            CreateMap<CreateExpenseViewModel, Expense>();

            CreateMap<EditExpenseViewModel, Expense>().ReverseMap();

            CreateMap<Certificate, CertificateViewModel>();
        }
    }
}
