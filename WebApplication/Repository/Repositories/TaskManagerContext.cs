using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class TaskManagerContext : DbContext, IContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<History> Histories { get; set; }

        public void Save()
        {
            SaveChanges();
        }
    }
}