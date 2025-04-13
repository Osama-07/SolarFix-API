using AutoMapper;
using SolarFix.DTO;
using SolarFix.Entities;
using SolarFix.Enums;

namespace SolarFix.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, UserDTO>()
				.ReverseMap()
				.ForMember(dest => dest.Orders, opt => opt.Ignore())
				.ForMember(dest => dest.Technician, opt => opt.Ignore());
			
			CreateMap<User, UserResponseDTO>()
				.ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType == 0 ? "Technician" : "Customer"))
				.ReverseMap()
				.ForMember(dest => dest.UserType, opt => opt.MapFrom(src => string.Equals(src.UserType, "Technician", StringComparison.OrdinalIgnoreCase) ? 0 : 1))
				.ForMember(dest => dest.Orders, opt => opt.Ignore())
				.ForMember(dest => dest.Technician, opt => opt.Ignore());

			CreateMap<SignUpDTO, User>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
				.ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
				.ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

			CreateMap<Technician, TechnicianDTO>()
				.ReverseMap()
				.ForMember(dest => dest.User, opt => opt.Ignore())
				.ForMember(dest => dest.Orders, opt => opt.Ignore());

			CreateMap<Technician, TechnicianDetailsDTO>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone));

			CreateMap<SignUpDTO, Technician>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => 0)) // because new Technician.
				.ForMember(dest => dest.ExperienceYears, opt => opt.MapFrom(src => src.ExperienceYears))
				.ForMember(dest => dest.PricePerHour, opt => opt.MapFrom(src => src.PricePerHour))
				.ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 5));

			CreateMap<Order, OrderDTO>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == enOrderStatus.Pending ? "Pending" : src.Status == enOrderStatus.Accepted ? "Accepted" : "Completed"))
				.ReverseMap()
				.ForMember(dest => dest.Customer, opt => opt.Ignore())
				.ForMember(dest => dest.Technician, opt => opt.Ignore())
				.ForMember(dest => dest.Review, opt => opt.Ignore());
			
			CreateMap<OrderRequestDTO, Order>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => enOrderStatus.Pending))
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
				.ForMember(dest => dest.Customer, opt => opt.Ignore())
				.ForMember(dest => dest.Technician, opt => opt.Ignore())
				.ForMember(dest => dest.Review, opt => opt.Ignore());

			CreateMap<Order, OrderDetailsDTO>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status == enOrderStatus.Pending ? "Pending" : src.Status == enOrderStatus.Accepted ? "Accepted" : "Completed"))
				.ForMember(dest => dest.TechnicianName, opt => opt.MapFrom(src => src.Technician.User.FullName))
				.ForMember(dest => dest.ExperienceYears, opt => opt.MapFrom(src => src.Technician.ExperienceYears))
				.ForMember(dest => dest.PricePerHour, opt => opt.MapFrom(src => src.Technician.PricePerHour))
				.ForMember(dest => dest.TechnicianEmail, opt => opt.MapFrom(src => src.Technician.User.Email))
				.ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
				.ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
				.ForMember(dest => dest.IsRated, opt => opt.MapFrom(src => src.Review != null));


			CreateMap<Review, ReviewDTO>()
				.ReverseMap()
				.ForMember(dest => dest.Order, opt => opt.Ignore());

			CreateMap<ReviewRequestDTO, Review>()
				.ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
		}

	}
}
