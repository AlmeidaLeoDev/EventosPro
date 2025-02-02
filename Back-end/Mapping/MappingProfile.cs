using AutoMapper;
using EventosPro.Models;
using EventosPro.ViewModels.Events;
using EventosPro.ViewModels.Users;

namespace EventosPro.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Event Mappings
            CreateMap<Event, EventDetailsViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.EventUserName, opt => opt.MapFrom(src => src.EventUser.Name))
                .ForMember(dest => dest.Invites, opt => opt.MapFrom(src => src.EventInvites));

            CreateMap<CreateEventViewModel, Event>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => EventStatus.Active))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.EventInvites, opt => opt.Ignore());

            CreateMap<UpdateEventViewModel, Event>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.EventInvites, opt => opt.Ignore());

            // EventInvite Mappings
            CreateMap<EventInvite, EventInviteViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.InvitedUserName, opt => opt.MapFrom(src => src.InvitedUser.Name))
                .ForMember(dest => dest.InvitedUserEmail, opt => opt.MapFrom(src => src.InvitedUser.Email));

            CreateMap<CreateEventInviteViewModel, EventInvite>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => InviteStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ResponseAt, opt => opt.Ignore())
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.InvitedUser, opt => opt.Ignore());

            // User Mappings
            CreateMap<User, UserProfileViewModel>();

            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsConfirmed, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Events, opt => opt.Ignore())
                .ForMember(dest => dest.EventInvites, opt => opt.Ignore());
        }
    }
}
