using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using Your_Ride.Models;
using Your_Ride.ViewModels.College;
using Your_Ride.ViewModels.University;
using Your_Ride.ViewModels.WalletViewModel;
using Your_Ride.ViewModels.TransactionViewModel;
using Your_Ride.ViewModels.BusViewModel;
using Your_Ride.ViewModels.BookViewModel;
using Your_Ride.Models.Your_Ride.Models;
using Your_Ride.ViewModels.AppointmentviewModel;
using Your_Ride.ViewModels.TimeViewModel;
using Your_Ride.ViewModels.UserTransactionLogViewModel;
using Your_Ride.ViewModels.Account;

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

            CreateMap<Bus, BusVM>().ReverseMap();

            CreateMap<Book, BookVM>().ReverseMap();

            CreateMap<Appointment, AppointmentVM>().ReverseMap();

            // Time <-> TimeVM Mapping
            CreateMap<Time, TimeVM>()
                .ForMember(dest => dest.LocationImages, opt => opt.MapFrom(src => src.LocationsWithPics)) // Correct mapping
                .ReverseMap();

            // Time <-> IFormFileTimeVM Mapping
            CreateMap<Time, IFormFileTimeVM>()
                .ForMember(dest => dest.FormFileLocationsWithPics, opt => opt.MapFrom(src =>
                    src.LocationsWithPics.Select(li => new FormFileLocationPics
                    {
                        Id = li.Id,
                        Location = li.Location,
                        LocationOrder = li.LocationOrder,
                        TimeId = li.TimeId,
                        ImagePath = li.ImagePath
                    }).ToList()))
                .ReverseMap();

            // TimeVM <-> IFormFileTimeVM Mapping
            CreateMap<TimeVM, IFormFileTimeVM>()
                .ForMember(dest => dest.FormFileLocationsWithPics, opt => opt.MapFrom(src =>
                    src.LocationImages.Select(li => new FormFileLocationPics
                    {
                        Id = li.Id,
                        Location = li.Location,
                        LocationOrder = li.LocationOrder,
                        TimeId = li.TimeId,
                        ImagePath = li.ImagePath
                    }).ToList()))
                .ReverseMap();

            // LocationImage <-> FormFileLocationPics Mapping
            CreateMap<LocationImage, FormFileLocationPics>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore()) // Ignore ImageFile since it's a file input
                .ReverseMap();

            CreateMap<UserTransactionLog, UserTransactionLogVM>().ReverseMap();

            CreateMap<ApplicationUser, EditProfileViewModel>().ReverseMap();

            // Use the corrected value converter
            //CreateMap<AddProductVM, Product>()
            //    .ForMember(dest => dest.Img, opt => opt.ConvertUsing(new IFormFileToStringConverter(), src => src.Img));
        }
    }
}
