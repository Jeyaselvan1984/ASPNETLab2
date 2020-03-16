using CareerCloud.Pocos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CareerCloud.EntityFrameworkDataAccess
{
    public class CareerCloudContext : DbContext
    {

        public CareerCloudContext(DbContextOptions<CareerCloudContext> options) : base(options)
        {


        }
        
        public DbSet<ApplicantEducationPoco> ApplicantEducations { get; set; }
        public DbSet<ApplicantJobApplicationPoco> ApplicantJobApplications{ get; set; }
        public DbSet<ApplicantProfilePoco> ApplicantProfiles { get; set; }
        public DbSet<ApplicantResumePoco> ApplicantResumes { get; set; }
        public DbSet<ApplicantSkillPoco> ApplicantSkills { get; set; }
        public DbSet<ApplicantWorkHistoryPoco> ApplicantWorkHistorys { get; set; }
        public DbSet<CompanyDescriptionPoco> CompanyDescriptions { get; set; }
        public DbSet<CompanyJobDescriptionPoco> CompanyJobDescriptions { get; set; }
        public DbSet<CompanyJobEducationPoco> CompanyJobEducations { get; set; }
        public DbSet<CompanyJobPoco> CompanyJobs { get; set; }
        public DbSet<CompanyJobSkillPoco> CompanyJobSkills { get; set; }
        public DbSet<CompanyLocationPoco> CompanyLocations { get; set; }
        public DbSet<CompanyProfilePoco> CompanyProfiles { get; set; }
        public DbSet<SecurityLoginPoco> SecurityLogins { get; set; }
        public DbSet<SecurityLoginsLogPoco> SecurityLoginsLogs { get; set; }
        public DbSet<SecurityLoginsRolePoco> SecurityLoginsRoles { get; set; }
        public DbSet<SecurityRolePoco> SecurityRoles { get; set; }
        public DbSet<SystemCountryCodePoco> SystemCountryCodes { get; set; }
        public DbSet<SystemLanguageCodePoco> systemLanguageCodes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicantEducationPoco>
            (entity =>
            {
                //entity.ToTable("Applicant_Educations");
                //entity.Property(_ => _.StartDate.HasColumnName("Start_Date").HasColumnType("date");
                //For single column primary key
                entity.HasKey(e => e.Id);
    //MultiColumn Composite key
  //  entity.HasKey(e => new { e.Id, e.Major });
                entity.HasOne(e => e.ApplicantProfiles)
    .WithMany(p => p.ApplicantEducation)
    .HasForeignKey(e => e.Applicant);
                entity.Property(e => e.TimeStamp).IsRowVersion();
    //Alternatively in Poco class use //[NotMapped]
});
            modelBuilder.Entity<ApplicantProfilePoco>
           (entity =>
           {
               //entity.HasMany(e => e.ApplicantEducation)
               // .WithOne(p => p.ApplicantProfiles).HasForeignKey(e => e.Applicant);


               entity.HasOne(e => e.SystemCountryCode)
              .WithMany(p => p.ApplicantProfile).HasForeignKey(e => e.Country);

     
               entity.Property(e => e.TimeStamp).IsRowVersion();
                //Alternatively in Poco class use //[NotMapped]
            });

            modelBuilder.Entity<ApplicantJobApplicationPoco>
         (entity =>
         {
         
               //entity.HasKey(e => e.Id);

               entity.HasOne(e => e.CompanyJob)
              .WithMany(p => p.ApplicantJobApplication).HasForeignKey(e=> e.Job);

             entity.HasOne(e => e.ApplicantProfile)
             .WithMany(p => p.ApplicantJobApplication).HasForeignKey(p=>p.Applicant);


             entity.Property(e => e.TimeStamp).IsRowVersion();
  
           });


            modelBuilder.Entity<ApplicantResumePoco>
         (entity =>
         {
             entity.HasKey(e => e.Id);
             entity.HasOne(e => e.ApplicantProfile)
            .WithMany(f => f.ApplicantResume).HasForeignKey(e => e.Applicant);        

         });

            modelBuilder.Entity<ApplicantSkillPoco>
      (entity =>
      {
          entity.HasKey(e => e.Id);
          entity.HasOne(e => e.ApplicantProfile)
         .WithMany(f => f.ApplicantSkill).HasForeignKey(e => e.Applicant);
          entity.Property(e => e.TimeStamp).IsRowVersion();

      });

       modelBuilder.Entity<ApplicantWorkHistoryPoco>
        (entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.ApplicantProfile)
            .WithMany(f => f.ApplicantWorkHistory).HasForeignKey(f=> f.Applicant );
            entity.HasOne(e => e.SystemCountryCode).
            WithMany(f => f.ApplicantWorkHistory).HasForeignKey(f => f.CountryCode);
            entity.Property(e => e.TimeStamp).IsRowVersion();

        });

            modelBuilder.Entity<CompanyJobDescriptionPoco>
(entity =>
{
   entity.Property(e => e.TimeStamp).IsRowVersion();
});
            modelBuilder.Entity<CompanyJobEducationPoco>
(entity =>
{
   entity.Property(e => e.TimeStamp).IsRowVersion();
});
            modelBuilder.Entity<CompanyJobSkillPoco>
(entity =>
{
entity.Property(e => e.TimeStamp).IsRowVersion();
});

            modelBuilder.Entity<CompanyJobPoco>
     (entity =>
     {
         entity.HasKey(e => e.Id);
         entity.HasOne(e => e.CompanyProfile)
         .WithMany(f => f.CompanyJob).HasForeignKey(f => f.Company );

         entity.HasMany(e => e.CompanyJobEducation).WithOne(f =>f.CompanyJob ).HasForeignKey(f=>f.Job);

         entity.HasMany(e => e.CompanyJobDescription).WithOne(f => f.CompanyJob).HasForeignKey(f=>f.Job);
         entity.Property(e => e.TimeStamp).IsRowVersion();

         entity.HasMany(e => e.CompanyJobSkill).WithOne(f => f.CompanyJob).HasForeignKey(f => f.Job);

     });

            modelBuilder.Entity<CompanyJobDescriptionPoco>
    (entity =>
    {
        entity.Property(e => e.TimeStamp).IsRowVersion();
    });
        modelBuilder.Entity<CompanyProfilePoco>
(entity =>
{
    entity.HasMany(e => e.CompanyJob).WithOne(f => f.CompanyProfile);
    entity.Property(e => e.TimeStamp).IsRowVersion();
});

 modelBuilder.Entity<CompanyDescriptionPoco>
(entity =>
{
entity.HasKey(e => e.Id);
entity.HasOne(e => e.CompanyProfile)
.WithMany(f => f.CompanyDescription).HasForeignKey(f => f.Company);

entity.HasOne(e => e.SystemLanguageCode)
.WithMany(f => f.CompanyDescription).HasForeignKey(f => f.LanguageId);
    entity.Property(e => e.TimeStamp).IsRowVersion();

});
            modelBuilder.Entity<CompanyLocationPoco>
(entity =>
{
    entity.HasKey(e => e.Id);
    entity.HasOne(e => e.CompanyProfile)
    .WithMany(f => f.CompanyLocation).HasForeignKey(f => f.Company );

    entity.Property(e => e.TimeStamp).IsRowVersion();

});

            modelBuilder.Entity<SecurityLoginPoco>
(entity =>
{
   entity.HasKey(e => e.Id);
   entity.HasMany (e => e.ApplicantProfile)
   .WithOne(f => f.SecurityLogin).HasForeignKey(f => f.Login);

    entity.HasMany(e => e.SecurityLoginsLog)
    .WithOne(f => f.SecurityLogin).HasForeignKey(f => f.Login);
    entity.HasMany(e => e.SecurityLoginsRole)
    .WithOne(f => f.SecurityLogin).HasForeignKey(f => f.Login);

   entity.Property(e => e.TimeStamp).IsRowVersion();

});
            modelBuilder.Entity<SecurityLoginsRolePoco>
           (entity =>
           {
               entity.HasOne(e => e.SecurityRole)
               .WithMany(f => f.SecurityLoginsRole).HasForeignKey(e => e.Role);
              
               entity.Property(e => e.TimeStamp).IsRowVersion();

           });


            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var config = new ConfigurationBuilder();
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            //config.AddJsonFile(path, false);
            //var root = config.Build();
            //string _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            //optionsBuilder.UseSqlServer(_connStr);
            //base.OnConfiguring(optionsBuilder);
        }
      
      
    }
}
