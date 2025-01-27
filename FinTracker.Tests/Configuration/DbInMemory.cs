using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Api.Data;
using FinTracker.Core.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinTracker.Tests.Configuration
{
    public class DbInMemory
    {
        private AppDbContext _context;

        public DbInMemory()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            _context = new AppDbContext(options);

            InsertData();
        }

        public AppDbContext GetContext() => _context;

        private void InsertData() 
        {
            if (_context.Database.EnsureCreated())
            {
                _context.Vouchers.AddRange(
                    new Voucher
                    {
                        Id = 1,
                        Code = "1234",
                        Amount = 150,
                        Title = "Teste Ativo",
                        Description = "Teste Ativo",
                        IsActive = true,
                    },
                    new Voucher
                    {
                        Id = 2,
                        Code = "5678",
                        Amount = 200,
                        Title = "Teste Inativo",
                        Description = "Teste Inativo",
                        IsActive = false,
                    }
                    );

                _context.SaveChanges();
            }
        }
    }
}
