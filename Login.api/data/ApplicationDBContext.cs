using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.models;
using Microsoft.EntityFrameworkCore;

namespace Login.api.data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<StudentTeacher> StudentTeachers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ nhiều-nhiều giữa Student và Teacher
            modelBuilder.Entity<StudentTeacher>()
                .HasKey(st => new { st.StudentId, st.TeacherId });

            modelBuilder.Entity<StudentTeacher>()
            .HasOne(st => st.Student)
            .WithMany(s => s.StudentTeachers)
            .HasForeignKey(st => st.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentTeacher>()
            .HasOne(st => st.Teacher)
            .WithMany(t => t.StudentTeachers)
            .HasForeignKey(st => st.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}