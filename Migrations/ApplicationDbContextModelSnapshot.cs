﻿// <auto-generated />
using System;
using MMUniGraduation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MMUniGraduation.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MMUniGraduation.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentCourseId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeacherToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CourseStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Exam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ExamGrade")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("FinalHomeworkGrade")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("LectorID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NextCourseId")
                        .HasColumnType("int");

                    b.Property<int>("ParetntId")
                        .HasColumnType("int");

                    b.Property<string>("Signature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SkipCoursEndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("SkipCourse")
                        .HasColumnType("bit");

                    b.Property<string>("SkippingCourseSignature")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudyProgramId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LectorID");

                    b.HasIndex("StudyProgramId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Homework", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Grade")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("HomeworkName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeworkTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LectureId")
                        .HasColumnType("int");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LectureId");

                    b.ToTable("Homeworks");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Image", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StudyProgramId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudyProgramId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("MMUniGraduation.Models.LearningObject", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("VideoMaterial")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("LearningObjects");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Lector", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Lectors");
                });

            modelBuilder.Entity("MMUniGraduation.Models.LectorStudyProgram", b =>
                {
                    b.Property<int>("LectorId")
                        .HasColumnType("int");

                    b.Property<int>("StudyProgramId")
                        .HasColumnType("int");

                    b.HasKey("LectorId", "StudyProgramId");

                    b.HasIndex("StudyProgramId");

                    b.ToTable("LectorStudyProgram");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Lecture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AssignmentGrade")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Assignments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTimeToShow")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDateTimeForHW")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NextLectureId")
                        .HasColumnType("int");

                    b.Property<int>("ParetntLectureId")
                        .HasColumnType("int");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("MMUniGraduation.Models.LectureFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Extension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LectureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LectureId");

                    b.ToTable("LectureFiles");
                });

            modelBuilder.Entity("MMUniGraduation.Models.RestrictAccess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CourseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("RestrictAccess");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CurrentCourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Town")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentCourseId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("MMUniGraduation.Models.StudentStudyProgram", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("StudyProgramId")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "StudyProgramId");

                    b.HasIndex("StudyProgramId");

                    b.ToTable("StudentStudyProgram");
                });

            modelBuilder.Entity("MMUniGraduation.Models.StudyProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("StudyPrograms");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Course", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Lector", null)
                        .WithMany("Courses")
                        .HasForeignKey("LectorID");

                    b.HasOne("MMUniGraduation.Models.StudyProgram", "StudyProgram")
                        .WithMany("Courses")
                        .HasForeignKey("StudyProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StudyProgram");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Homework", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Lecture", "Lecture")
                        .WithMany("Homeworks")
                        .HasForeignKey("LectureId");

                    b.Navigation("Lecture");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Image", b =>
                {
                    b.HasOne("MMUniGraduation.Models.StudyProgram", "StudyProgram")
                        .WithMany("Images")
                        .HasForeignKey("StudyProgramId");

                    b.Navigation("StudyProgram");
                });

            modelBuilder.Entity("MMUniGraduation.Models.LectorStudyProgram", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Lector", "Lector")
                        .WithMany("LectorStudyPrograms")
                        .HasForeignKey("LectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MMUniGraduation.Models.StudyProgram", "StudyProgram")
                        .WithMany("LectorStudyPrograms")
                        .HasForeignKey("StudyProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lector");

                    b.Navigation("StudyProgram");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Lecture", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Course", "Course")
                        .WithMany("Lectures")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("MMUniGraduation.Models.LectureFile", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Lecture", "Lecture")
                        .WithMany("TextMaterials")
                        .HasForeignKey("LectureId");

                    b.Navigation("Lecture");
                });

            modelBuilder.Entity("MMUniGraduation.Models.RestrictAccess", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Student", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Course", "CurrentCourse")
                        .WithMany()
                        .HasForeignKey("CurrentCourseId");

                    b.Navigation("CurrentCourse");
                });

            modelBuilder.Entity("MMUniGraduation.Models.StudentStudyProgram", b =>
                {
                    b.HasOne("MMUniGraduation.Models.Student", "Student")
                        .WithMany("CompleatedStudyPrograms")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MMUniGraduation.Models.StudyProgram", "StudyProgram")
                        .WithMany("StudentStudyProgram")
                        .HasForeignKey("StudyProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("StudyProgram");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Course", b =>
                {
                    b.Navigation("Lectures");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Lector", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("LectorStudyPrograms");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Lecture", b =>
                {
                    b.Navigation("Homeworks");

                    b.Navigation("TextMaterials");
                });

            modelBuilder.Entity("MMUniGraduation.Models.Student", b =>
                {
                    b.Navigation("CompleatedStudyPrograms");
                });

            modelBuilder.Entity("MMUniGraduation.Models.StudyProgram", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Images");

                    b.Navigation("LectorStudyPrograms");

                    b.Navigation("StudentStudyProgram");
                });
#pragma warning restore 612, 618
        }
    }
}
