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
                var voucher1 = new Voucher
                {
                    Id = 1,
                    Code = "1234",
                    Amount = 150,
                    Title = "Teste Ativo",
                    Description = "Teste Ativo",
                    IsActive = true,
                };
                var voucher2 = new Voucher
                {
                    Id = 2,
                    Code = "5678",
                    Amount = 200,
                    Title = "Teste Inativo",
                    Description = "Teste Inativo",
                    IsActive = false,
                };
                _context.Vouchers.AddRange(voucher1, voucher2);

                var product1 = new Product
                {
                    Id = 1,
                    Title = "Produto teste 1",
                    Description = "Produto teste 1",
                    Slug = "teste-1",
                    IsActive = true,
                    Price = 250
                };

                var product2 = new Product
                {
                    Id = 2,
                    Title = "Produto teste 2",
                    Description = "Produto teste 2",
                    Slug = "teste-2",
                    IsActive = false,
                    Price = 500
                };
                _context.Products.AddRange(product1, product2);

                var order1 = new Order
                {
                    Id = 1,
                    Code = "1234",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Product = product1,
                    ProductId = product1.Id,
                    Status = Core.Enums.EOrderStatus.Canceled,
                    UserId = "userteste@hotmail.com",
                    Voucher = voucher1,
                    VoucherId = voucher1.Id,
                    Gateway = Core.Enums.EPaymentGateway.Stripe
                };

                var order2 = new Order
                {
                    Id = 2,
                    Code = "5678",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Product = product2,
                    ProductId = product2.Id,
                    Status = Core.Enums.EOrderStatus.WaitingPayment,
                    UserId = "userteste@hotmail.com",
                    Voucher = voucher2,
                    VoucherId = voucher2.Id,
                    Gateway = Core.Enums.EPaymentGateway.Stripe
                };

                var order3 = new Order
                {
                    Id = 3,
                    Code = "9876",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Product = product2,
                    ProductId = product2.Id,
                    Status = Core.Enums.EOrderStatus.Paid,
                    UserId = "userteste@hotmail.com",
                    Voucher = voucher2,
                    VoucherId = voucher2.Id,
                    Gateway = Core.Enums.EPaymentGateway.Stripe
                };

                var order4 = new Order
                {
                    Id = 4,
                    Code = "5432",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Product = product1,
                    ProductId = product1.Id,
                    Status = Core.Enums.EOrderStatus.Refunded,
                    UserId = "userteste@hotmail.com",
                    Voucher = voucher1,
                    VoucherId = voucher1.Id,
                    Gateway = Core.Enums.EPaymentGateway.Stripe
                };

                _context.Orders.AddRange(order1, order2, order3, order4);

                var category1 = new Category
                {
                    Id = 1,
                    UserId = "userteste@hotmail.com",
                    Title = "Categoria teste 1",
                    Description = "Categoria teste 1",
                };

                var category2 = new Category
                {
                    Id = 2,
                    UserId = "userteste@hotmail.com",
                    Title = "Categoria teste 2",
                    Description = "Categoria teste 2",
                };

                _context.Categories.AddRange(category1, category2);

                var transaction1 = new Transaction
                {
                    Id = 1,
                    UserId = "userteste@hotmail.com",
                    Amount = 100,
                    CreatedAt = DateTime.Now,
                    PaidOrReceivedAt = DateTime.Now,
                    Category = category1,
                    CategoryId = category1.Id,
                    Title = "Transação Teste 1",
                    Type = Core.Enums.EtransactionType.Deposit,
                };

                var transaction2 = new Transaction
                {
                    Id = 2,
                    UserId = "userteste@hotmail.com",
                    Amount = 200,
                    CreatedAt = DateTime.Now,
                    PaidOrReceivedAt = DateTime.Now,
                    Category = category2,
                    CategoryId = category2.Id,
                    Title = "Transação Teste 2",
                    Type = Core.Enums.EtransactionType.Withdraw,
                };
                _context.Transactions.AddRange(transaction1, transaction2);

                _context.SaveChanges();
            }
        }
    }
}
