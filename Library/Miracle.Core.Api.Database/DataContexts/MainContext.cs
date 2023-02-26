using Library.Helpers.Constraints;
using Library.Helpers.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Miracle.Core.Api.Database.Models;
using System.Collections.Generic;

namespace Miracle.Core.Api.Database
{
    public class MainContext : BaseContext
    {
        #region Table
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ResetPassword> ResetPasswords { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<VersionInfo> VersionInfos { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<SMTPSetting> SMTPSettings { get; set; }
        public DbSet<ProductLimitation> ProductLimitations { get; set; }
        public DbSet<SetupInfo> SetupInfos { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductUsageInfo> ProductUsageInfos { get; set; }
        public DbSet<UserWatch> UserWatch { get; set; }
        public DbSet<ProductModule> ProductModules { get; set; }
        #endregion

        #region ManyToMany
        public DbSet<Platform_Product> Platform_Products { get; set; }
        public DbSet<User_Product> User_Products { get; set; }
        public DbSet<User_ProductInfo> User_ProductInfos { get; set; }
        public DbSet<User_Version> User_Versions { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }
        public DbSet<ProductSetting> ProductSettings { get; set; }
        public DbSet<Product_Module> Product_Modules { get; set; }
        public DbSet<User_Product_Module> User_Product_Modules { get; set; }
        #endregion
        public DbSet<AppLib> AppLibs { get; set; }

        public MainContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            RelationSettings(modelBuilder);
            DefaultDatas(modelBuilder);
        }

        private void RelationSettings(ModelBuilder modelBuilder)
        {
            #region User many to many Role
            modelBuilder.Entity<User_Role>()
                .HasKey(s => new { s.UserId, s.RoleId });
            modelBuilder.Entity<User_Role>()
                .HasOne(s => s.User)
                .WithMany(s => s.User_Roles)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User_Role>()
                .HasOne(s => s.Role)
                .WithMany(s => s.User_Roles)
                .HasForeignKey(s => s.RoleId);
            #endregion User many to many Role

            #region Platform many to many Products
            modelBuilder.Entity<Platform_Product>()
                .HasKey(s => new { s.PlatformId, s.ProductId });
            modelBuilder.Entity<Platform_Product>()
                .HasOne(s => s.Platform)
                .WithMany(s => s.PlatformProducts)
                .HasForeignKey(s => s.PlatformId);
            modelBuilder.Entity<Platform_Product>()
                .HasOne(s => s.Product)
                .WithMany(s => s.Platform_Products)
                .HasForeignKey(s => s.ProductId);
            #endregion Platform many to many Products

            #region User many to many Product
            modelBuilder.Entity<User_Product>()
                .HasKey(s => new { s.UserId, s.ProductId });
            modelBuilder.Entity<User_Product>()
                .HasOne(s => s.User)
                .WithMany(s => s.User_Products)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User_Product>()
                .HasOne(s => s.Product)
                .WithMany(s => s.User_Products)
                .HasForeignKey(s => s.ProductId);
            #endregion

            #region User many to many User_ProductInfo
            modelBuilder.Entity<User_ProductInfo>()
           .HasKey(s => new { s.UserId, s.ProductId, s.ProductUsageInfoId });
            modelBuilder.Entity<User_ProductInfo>()
                .HasOne(s => s.User)
                .WithMany(s => s.User_ProductInfos)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User_ProductInfo>()
                .HasOne(s => s.Product)
                .WithMany(s => s.User_ProductInfos)
                .HasForeignKey(s => s.ProductId);
            #endregion

            #region User many to many Version
            modelBuilder.Entity<User_Version>()
                .HasKey(s => new { s.UserId, s.ProductId, s.VersionInfoId });
            modelBuilder.Entity<User_Version>()
                .HasOne(s => s.User)
                .WithMany(s => s.User_Versions)
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User_Version>()
                .HasOne(s => s.Product)
                .WithMany(s => s.User_Versions)
                .HasForeignKey(s => s.ProductId);
            modelBuilder.Entity<User_Version>()
                .HasOne(s => s.VersionInfo)
                .WithMany(s => s.User_Versions)
                .HasForeignKey(s => s.VersionInfoId);
            #endregion 

            #region Product many to many Settings
            modelBuilder.Entity<ProductSetting>()
                .HasKey(s => new { s.ProductId, s.VersionInfoId });
            modelBuilder.Entity<ProductSetting>()
                .HasOne(s => s.Product)
                .WithMany(s => s.ProductSettings)
                .HasForeignKey(s => s.ProductId);
            modelBuilder.Entity<ProductSetting>()
                .HasOne(s => s.VersionInfo)
                .WithMany(s => s.ProductSettings)
                .HasForeignKey(s => s.VersionInfoId);
            modelBuilder.Entity<ProductSetting>()
                .HasOne(s => s.Priority)
                .WithMany(s => s.ProductSettings)
                .HasForeignKey(s => s.PriorityId);
            #endregion   

            #region Product many to many ProductModule
            modelBuilder.Entity<Product_Module>()
                .HasKey(s => new { s.ProductId, s.ProductModuleId });
            modelBuilder.Entity<Product_Module>()
              .HasOne(s => s.Product)
              .WithMany(s => s.Product_Modules)
              .HasForeignKey(s => s.ProductId);
            modelBuilder.Entity<Product_Module>()
                .HasOne(s => s.Module)
                .WithMany(s => s.Product_Modules)
                .HasForeignKey(s => s.ProductModuleId);
            #endregion

            #region User many to many Product Module
            modelBuilder.Entity<User_Product_Module>()
                .HasKey(s => new { s.UserId, s.ProductId, s.ProductModuleId });
            modelBuilder.Entity<User_Product_Module>()
              .HasOne(s => s.User)
              .WithMany(s => s.User_Product_Modules)
              .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<User_Product_Module>()
                .HasOne(s => s.Product)
                .WithMany(s => s.User_Product_Modules)
                .HasForeignKey(s => s.ProductId);
            #endregion
        }

        private void DefaultDatas(ModelBuilder modelBuilder)
        {
            Company company = new Company()
            {
                Id = 1,
                Address = "Adres",
                Location = "https://g.page/test?share",
                PhoneNumber = "+905550000000",
                Name = "Şirket",
            };
            modelBuilder.Entity<Company>().HasData(company);

            var priorityList = new List<Priority>()
            {
                 new Priority()
                    {
                        Id = 1,
                        Name = "First Class",
                        Description = "For long term support(release) products",
                        State = "A",
                    },
                 new Priority()
                    {
                        Id = 2,
                        Name = "Second Class",
                        Description = "For Preview-Builds products",
                        State = "B",
                    },
                 new Priority()
                    {
                        Id = 2,
                        Name = "Third Class",
                        Description = "For Open-Beta products",
                        State = "C",
                    }
             };
            modelBuilder.Entity<Priority>().HasData(priorityList);

            var roles = new List<Role>()
            {
                new Role()
                {
                    Id = 1,
                    Value = RoleConstraints.Roles.SD
                },
                new Role()
                {
                    Id=2,
                    Value= RoleConstraints.Roles.Admin
                }
            };
            modelBuilder.Entity<Role>().HasData(roles);

            var user = new User()
            {
                Id = 1,
                Guid = "",
                Name = "Test",
                Surname = "Test",
                PhoneNumber = "05550000000",
                Email = "test@test.test",
                Username = "admin",
                Password = SHA512Encryptor.Encrypt("testttt"),
                IsActive = true,
                RefreshToken = null,
                RefreshTokenExpire = null,
                Token = null,
                TokenExpire = null,
                CompanyId = 1,
                PriorityId = 1,
            };
            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<User_Role>().HasData(new User_Role()
            {
                UserId = 1,
                RoleId = 1
            });

            modelBuilder.Entity<Platform>().HasData(new List<Platform>()
            {
                new Platform()
                {
                    Id = 1,
                    Name = "Linux"
                },
                new Platform()
                {
                    Id = 2,
                    Name = "Mac"
                },
                new Platform()
                {
                    Id = 3,
                    Name = "Windows"
                }
             });

            modelBuilder.Entity<ProductTag>().HasData(
                new ProductTag() { Id = 1, Tag = "product1" },
                new ProductTag() { Id = 2, Tag = "product2" },
                new ProductTag() { Id = 3, Tag = "product3" });
        }
    }
}