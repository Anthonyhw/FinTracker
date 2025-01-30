using System;
using FinTracker.Api.Data;
using FinTracker.Api.Handlers;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Responses;
using FinTracker.Tests.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FinTracker.Tests.Handlers
{
    [TestFixture]
    public class VoucherServiceTests
    {
        private readonly VoucherHandler voucherHandler;

        public VoucherServiceTests()
        {
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            voucherHandler = new VoucherHandler(context);
        }

        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public async Task Should_ReturnVoucher_When_CodeIsValidAndActive()
        {
            // Arrange
            var request = new GetVoucherByNumberRequest { Code = "1234" };

            // Act
            var result = await voucherHandler.GetByNumberAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Code, Is.EqualTo(200));
                Assert.That(result.Data?.Code, Is.EqualTo("1234"));
            });
        }

        [Test]
        public async Task Should_Return404_When_VoucherNotFound()
        {
            // Arrange
            var request = new GetVoucherByNumberRequest { Code = "invalido" };

            // Act
            var result = await voucherHandler.GetByNumberAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Code, Is.EqualTo(404));
                Assert.That(result.Message, Is.EqualTo("Cupom não encontrado."));
            });
        }

        [Test]
        public async Task Should_Return400_When_VoucherIsInactive()
        {
            // Arrange
            var request = new GetVoucherByNumberRequest { Code = "5678" };

            // Act
            var result = await voucherHandler.GetByNumberAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Code, Is.EqualTo(400));
                Assert.That(result.Message, Is.EqualTo("Cupom inativo."));
            });
        }
    }
}
