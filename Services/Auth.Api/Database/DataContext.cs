using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Api.Database
{
    public class DataContext : DbContext
    {
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenType> TokenTypes { get; set; }
        public DbSet<UserAgent> UserAgents { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TokenType>().HasData(new TokenType()
            {
                Id = 1,
                MultiUsage = false,
                Value = "MiracleWorld"
            });

            modelBuilder.Entity<TokenType>().HasData(new TokenType()
            {
                Id = 2,
                MultiUsage = true,
                Value = "Web.Global"
            });
        }
    }

    public class Token
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// UserId located in CoreDataContext.Users.Id
        /// </summary>
        public int UserId { get; set; }

        public string Value { get; set; }
        public string RefreshValue { get; set; }

        public DateTime? Expire { get; set; }
        public DateTime? RefreshExpire { get; set; }


        [ForeignKey("TokenType")]
        public int TokenTypeId { get; set; }
        public TokenType TokenType { get; set; }
    }

    public class TokenType
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public bool MultiUsage { get; set; }

        public ICollection<Token> Tokens { get; set; }

        public TokenType()
        {
            Tokens = new Collection<Token>();
        }
    }

    public class UserAgent
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }
    }







    // Token tablosu var
    // Token Type tablosu var
    // Token type tablosu bir client dan çoklu giriş izinlerini düzenliyor.
    // Sıradaki ise metodlara erişim izni? Ama roller zaten bu işi yapıyor.
    // 
}