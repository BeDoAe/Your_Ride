using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using Your_Ride.Models;
using Your_Ride.ViewModels.College;
using Your_Ride.ViewModels.University;
using Your_Ride.ViewModels.WalletViewModel;
using Your_Ride.ViewModels.TransactionViewModel;

namespace Your_Ride.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<College, CollegeVM>()
                    .ForMember(dest => dest.University, opt => opt.MapFrom(src => src.university))
                    .ReverseMap();
            CreateMap<College , CreateCollege>().ReverseMap();    


            CreateMap<University, UniversityVM>().ReverseMap();
            CreateMap<University, CreateUniversityVM>().ReverseMap();

            CreateMap<Wallet, WalletVM>().ReverseMap();

            CreateMap<Transaction,TransactionvM>().ReverseMap();
            CreateMap<Transaction,CreateTransactionvM>().ReverseMap();
            CreateMap<TransactionvM, CreateTransactionvM>().ReverseMap();
            CreateMap<Transaction,AllDataTransactionCreateVM>().ReverseMap();

            // Use the corrected value converter
            //CreateMap<AddProductVM, Product>()
            //    .ForMember(dest => dest.Img, opt => opt.ConvertUsing(new IFormFileToStringConverter(), src => src.Img));
        }
    }
}
