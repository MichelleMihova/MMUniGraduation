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
        public DbSet<LearningObject> LearningObjects { get; set; }
        public DbSet<LectureFile> LectureFiles { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<RestrictAccess> RestrictAccess { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lector> Lectors { get; set; }
        public DbSet<StudentStudyProgram> StudentStudyProgram { get; set; }
        public DbSet<LectorStudyProgram> LectorStudyProgram { get; set; }

        //public DbSet<StudentCourse> StudentCourses { get; set; }
        //public DbSet<SkippingAssignment> SkippingAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>()
               .HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => x.UserId);
            builder.Entity<IdentityUserToken<string>>()
                .HasKey(x => x.UserId);
            
            builder.Entity<StudentStudyProgram>()
                .HasKey(sc => new { sc.StudentId, sc.StudyProgramId });
            builder.Entity<StudentStudyProgram>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.CompleatedStudyPrograms)
                .HasForeignKey(sc => sc.StudentId);
            builder.Entity<StudentStudyProgram>()
                .HasOne(sc => sc.StudyProgram)
                .WithMany(c => c.StudentStudyProgram)
                .HasForeignKey(sc => sc.StudyProgramId);

            builder.Entity<LectorStudyProgram>()
                .HasKey(sc => new { sc.LectorId, sc.StudyProgramId });
            builder.Entity<LectorStudyProgram>()
                .HasOne(sc => sc.Lector)
                .WithMany(s => s.LectorStudyPrograms)
                .HasForeignKey(sc => sc.LectorId);
            builder.Entity<LectorStudyProgram>()
                .HasOne(sc => sc.StudyProgram)
                .WithMany(c => c.LectorStudyPrograms)
                .HasForeignKey(sc => sc.StudyProgramId);

            /*
           builder.Entity<StudentCourse>()
               .HasKey(slo => new { slo.CourseId, slo.StudentId });
           builder.Entity<StudentCourse>()
               .HasOne(slo => slo.Student)
               .WithMany(s => s.StudentCourses)
               .HasForeignKey(slo => slo.StudentId);
           builder.Entity<StudentCourse>()
               .HasOne(slo => slo.Course)
               .WithMany(lo => lo.StudentCourses)
               .HasForeignKey(slo => slo.CourseId);
           */

            /*
            builder.Entity<ProgramCourse>()
                .HasKey(sc => new { sc.CourseID, sc.ProgramId });
            builder.Entity<ProgramCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(s => s.Program)
                .HasForeignKey(sc => sc.CourseID);
            builder.Entity<ProgramCourse>()
                .HasOne(sc => sc.Program)
                .WithMany(c => c.Course)
                .HasForeignKey(sc => sc.ProgramId);
            */

        }
    }
}
