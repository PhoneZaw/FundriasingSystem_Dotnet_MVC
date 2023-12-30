using AutoMapper;
using FundraisingApp.Entities;
using FundriasingSystem.Models.Campaign;
using FundriasingSystem.Models.CampaignType;
using FundriasingSystem.Models.Donation;
using FundriasingSystem.Models.Donor;
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

            CreateMap<EditDonationViewModel, Donation>().ReverseMap();

            CreateMap<CreatePaymentMethodViewModel, PaymentMethod>();

            CreateMap<EditPaymentMethodViewModel, PaymentMethod>().ReverseMap();

            CreateMap<CreateStaffRoleViewModel, StaffRole>();

            CreateMap<EditStaffRoleViewModel, StaffRole>().ReverseMap();
        }
    }
}
