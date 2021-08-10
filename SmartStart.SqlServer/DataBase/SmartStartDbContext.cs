using Elkood.Web.Domain.Util;
using Elkood.Web.Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartStart.Model.Business;
using SmartStart.Model.General;
using SmartStart.Model.Main;
using SmartStart.Model.Security;
using SmartStart.Model.Setting;
using SmartStart.Model.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartStart.SqlServer.DataBase
{
    public class SmartStartDbContext : ElIdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public SmartStartDbContext(DbContextOptions options, IUserResolverService userResolverService) : base(options, userResolverService) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<AppUser>().HasQueryFilter(user=>!user.DateDeleted.HasValue);

            builder.Entity<Code>()
               .HasIndex(b => b.Hash)
               .IsUnique();

            builder.Entity<SubjectFaculty>()
              .HasOne(i => i.Semester)
              .WithMany(x => x.SubjectFacultysSemester)
              .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<SubjectFaculty>()
              .HasOne(i => i.Section)
              .WithMany(x => x.SubjectFacultysSection)
              .OnDelete(DeleteBehavior.Restrict);
        }

        #region -   Business   -
        public DbSet<Code> Codes { get; set; }
        public DbSet<CodePackage> CodePackages { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageSubject> PackageSubjects { get; set; }
        public DbSet<Rate> Rates { get; set; }
        #endregion

        #region -   General   -
        public DbSet<City> Cities { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        #endregion

        #region -   Main   -
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamTag> ExamTags { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionDocument> QuestionDocuments { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectFacultyAppUser> SubjectFacultyAppUsers { get; set; }
        public DbSet<SubjectTag> SubjectTags { get; set; }
        #endregion

        #region -   Security   -
        // public override DbSet<AppUser> Users { get; set; }
        #endregion

        #region -   Setting   -
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        #endregion

        #region -   Shared   -
        public DbSet<Document> Documents { get; set; }
        public DbSet<Tag> Tags { get; set; }
        #endregion
    }
}
