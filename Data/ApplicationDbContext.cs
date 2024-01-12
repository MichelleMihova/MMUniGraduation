using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MMUniGraduation.Models;

namespace MMUniGraduation.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudyProgram> StudyPrograms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<LectureFile> LectureFiles { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<SkippingAssignment> SkippingAssignments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lector> Lectors { get; set; }
        public DbSet<StudentCourses> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>()
               .HasKey(x => new { x.UserId, x.RoleId });

            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => x.UserId);

            builder.Entity<IdentityUserToken<string>>()
                .HasKey(x => x.UserId);

            builder.Entity<StudentCourses>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder.Entity<StudentCourses>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.Passed)
                .HasForeignKey(sc => sc.StudentId);

            builder.Entity<StudentCourses>()
                .HasOne(sc => sc.Course);
        }
    }
}
