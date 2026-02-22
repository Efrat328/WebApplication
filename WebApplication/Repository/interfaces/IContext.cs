using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IContext
    {
        DbSet<User> Users { get; }
        DbSet<Project> Projects { get; }
        DbSet<TaskItem> Tasks { get; }
        DbSet<SubTask> SubTasks { get; }
        DbSet<History> Histories { get; }
        void Save();
    }
}