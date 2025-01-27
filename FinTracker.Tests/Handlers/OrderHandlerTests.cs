using FinTracker.Api.Data;
using FinTracker.Api.Handlers;
using FinTracker.Core.Enums;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Tests.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FinTracker.Tests.Handlers
{
    [TestFixture]
    public class OrderHandlerTests
    {
        private readonly OrderHandler orderHandler;

        public OrderHandlerTests()
        {
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            var stripeHandler = new Mock<IStripeHandler>();
            orderHandler = new OrderHandler(context, stripeHandler.Object);
        }

        [Test]
        public async Task WhenCancellingOrder_IfOrderIsCanceled_ShouldError()
        {
            // Arrange
            var request = new CancelOrderRequest() { Id = 1, UserId = "userteste@hotmail.com" };
            

            // Act
            var result = await orderHandler.CancelAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido já foi cancelado."));

        }

        [Test]
        public async Task WhenCancellingOrder_IfOrderIsPaid_ShouldError()
        {
            // Arrange
            var request = new CancelOrderRequest() { Id = 3, UserId = "userteste@hotmail.com" };


            // Act
            var result = await orderHandler.CancelAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido já foi pago e não pode ser cancelado."));

        }

        [Test]
        public async Task WhenCancellingOrder_IfOrderIsRefunded_ShouldError()
        {
            // Arrange
            var request = new CancelOrderRequest() { Id = 4, UserId = "userteste@hotmail.com" };


            // Act
            var result = await orderHandler.CancelAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido já foi reembolsado."));

        }

        [Test]
        public async Task WhenCancellingOrder_IfOrderIsWaitingPayment_ShouldSuccess()
        {
            // Arrange
            var request = new CancelOrderRequest() { Id = 2, UserId = "userteste@hotmail.com" };


            // Act
            var result = await orderHandler.CancelAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Message, Is.EqualTo("Pedido cancelado com sucesso."));

        }

        [Test]
        public async Task WhenCancellingOrder_IfOrderNotFound_ShouldError()
        {
            // Arrange
            var request = new CancelOrderRequest() { Id = 8, UserId = "usernaoexiste@hotmail.com" };


            // Act
            var result = await orderHandler.CancelAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Pedido não encontrado."));

        }
    }
}
