using AutoMapper;
using Repository.Entities;
using Service.Dto;

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

            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TaskItemDto, TaskItem>();

            CreateMap<SubTask, SubTaskDto>();
            CreateMap<SubTaskDto, SubTask>();

            CreateMap<History, HistoryDto>();
            CreateMap<HistoryDto, History>();
            
        }
    }
}