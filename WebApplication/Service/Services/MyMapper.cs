using AutoMapper;
using Repository.Entities;
using Service.Dto;
using Repository.Entities;

namespace Service
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();


            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();

            CreateMap<TaskItem, TaskItemDto>()
            .ForMember(dest => dest.ProjectName,
            opt => opt.MapFrom(src => src.Project.NameProject))
            .ForMember(dest => dest.AssignedTo,
            opt => opt.MapFrom(src => src.User.NameUser))      // int → string (שם)
            .ForMember(dest => dest.AssignedToId,
            opt => opt.MapFrom(src => src.AssignedTo));         // int → int (FK)


            CreateMap<TaskItemDto, TaskItem>()
            .ForMember(dest => dest.AssignedTo,
            opt => opt.MapFrom(src => src.AssignedToId));       // int → int ✅

            CreateMap<SubTask, SubTaskDto>()
            .ForMember(dest => dest.TaskName,opt => opt.MapFrom(src => src.Tasks.Id))
            .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.User.NameUser));
            CreateMap<SubTaskDto, SubTask>();

            CreateMap<History, HistoryDto>();
            CreateMap<HistoryDto, History>();

        }
    }
}