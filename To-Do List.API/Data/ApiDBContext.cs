using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using To_Do_List.API.Models;

namespace To_Do_List.API.Data
{
    public class ApiDBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Task { get; set; }
        public ApiDBContext(DbContextOptions<ApiDBContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}
