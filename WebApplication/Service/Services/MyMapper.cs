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

            //CreateMap<TaskItem, TaskItemDto>();
            //.ForMember(dest => dest.ProjectName,
            //opt => opt.MapFrom(src => src.Project.NameProject))
            //.ForMember(dest => dest.AssignedTo,
            //opt => opt.MapFrom(src => src.User.NameUser))      // int → string (שם)
            //.ForMember(dest => dest.AssignedToId,
            //opt => opt.MapFrom(src => src.AssignedTo));         // int → int (FK)

            //CreateMap<TaskItem, TaskItemDto>()
            //.ForMember(dest => dest.ProjectName,
            //opt => opt.MapFrom(src => src.Project.NameProject));

            CreateMap<TaskItem, TaskItemDto>()
    .ForMember(dest => dest.ProjectName,
    opt => opt.MapFrom(src => src.Project != null ? src.Project.NameProject : null));

            CreateMap<TaskItemDto, TaskItem>()
    .ForMember(dest => dest.ProjectId,
    opt => opt.MapFrom(src => src.ProjectId))
    .ForMember(dest => dest.AssignedTo,
    opt => opt.MapFrom(src => src.AssignedTo));

            //CreateMap<TaskItemDto, TaskItem>();
            //.ForMember(dest => dest.AssignedTo,
            //opt => opt.MapFrom(src => src.AssignedToId));       // int → int ✅

            //CreateMap<SubTask, SubTaskDto>();
            //.ForMember(dest => dest.TaskName,opt => opt.MapFrom(src => src.Tasks.Id))
            //.ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.User.NameUser));

            CreateMap<SubTask, SubTaskDto>()
    .ForMember(dest => dest.TaskName,
    opt => opt.MapFrom(src => src.Tasks != null ? src.Tasks.Title : null)); // ✅
            CreateMap<SubTaskDto, SubTask>();
                 

            CreateMap<History, HistoryDto>();
            CreateMap<HistoryDto, History>();

        }
    }
}