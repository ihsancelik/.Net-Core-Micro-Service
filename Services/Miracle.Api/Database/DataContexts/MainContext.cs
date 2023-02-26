using Library.Helpers.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Miracle.Api.Database.Models;

namespace Miracle.Api.Database
{
    public class MainContext : BaseContext
    {
        public DbSet<About> Abouts { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<LiveChat> LiveChats { get; set; }
        public DbSet<LiveChatContent> LiveChatContents { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SmtpSetting> SmtpSettings { get; set; }
        public DbSet<TicketGroup> TicketGroups { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }


        public MainContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();

            optionsBuilder.UseMySql("");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LiveChat>()
                .HasMany(s => s.LiveChatContents)
                .WithOne(s => s.LiveChat);
            // RelationSettings(modelBuilder);
            DefaultDatas(modelBuilder);
        }

        private void DefaultDatas(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<About>()
                .HasData(
                new About
                {
                    Id = 1,
                    ImageName = "",
                    Title = "Hakkımızda",
                    Text = "İçerik"
                });

            modelBuilder.Entity<ContactInfo>()
                .HasData(
                new ContactInfo
                {
                    Id = 1,
                    Email = "mail@mail.mail",
                    Phone = "+905550000000",
                    Address = "Adres",
                    Location = ""
                });

            modelBuilder.Entity<SmtpSetting>()
                .HasData(
                new SmtpSetting
                {
                    Id = 1,
                    Host = "Host",
                    Port = 587,
                    Email = "username",
                    Password = "password",
                    EnableSSL = false
                });
        }
    }
}