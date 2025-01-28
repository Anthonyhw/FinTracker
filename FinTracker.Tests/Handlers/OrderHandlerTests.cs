using FinTracker.Api.Data;
using FinTracker.Api.Handlers;
using FinTracker.Core.Enums;
using FinTracker.Core.Handlers;
using FinTracker.Core.Models;
using FinTracker.Core.Requests.Orders;
using FinTracker.Core.Requests.Stripe;
using FinTracker.Core.Responses;
using FinTracker.Core.Responses.Stripe;
using FinTracker.Tests.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stripe.Climate;

namespace FinTracker.Tests.Handlers
{
    [TestFixture]
    public class OrderHandlerTests
    {
        private OrderHandler orderHandler;

        public OrderHandlerTests()
        {

        }

        [SetUp]
        public void Setup()
        {
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            var stripeHandler = new Mock<IStripeHandler>();
            orderHandler = new OrderHandler(context, stripeHandler.Object);
            context.ChangeTracker.Clear();
        }

        #region CancelAsyncTests
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
        #endregion

        #region CreateAsyncTests
        [Test]
        public async Task WhenCreatingOrder_IfProductIsValid_ShouldSuccess()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                UserId = "usuarioteste@hotmail.com",
                ProductId = 1,
                VoucherId = 1,
            };
            // Act
            var result = await orderHandler.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(201));
            Assert.That(result.Message, Is.EqualTo($"Pedido {result.Data.Code} realizado com sucesso."));
        }

        [Test]
        public async Task WhenCreatingOrder_IfProductIsNotFound_ShouldError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                UserId = "usuarioteste@hotmail.com",
                ProductId = 5, // Product doesn't exist
                VoucherId = 2,
            };
            // Act
            var result = await orderHandler.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Produto não encontrado."));
        }

        [Test]
        public async Task WhenCreatingOrder_IfProductIsNotActive_ShouldError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                UserId = "usuarioteste@hotmail.com",
                ProductId = 2, // Product is not active
                VoucherId = 2,
            };
            // Act
            var result = await orderHandler.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Produto não encontrado."));
        }

        [Test]
        public async Task WhenCreatingOrder_IfVoucherIsNotPassed_ShouldContinue()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                UserId = "usuarioteste@hotmail.com",
                ProductId = 1,
                VoucherId = null,
            };
            // Act
            var result = await orderHandler.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(201));
            Assert.That(result.Message, Is.EqualTo($"Pedido {result.Data.Code} realizado com sucesso."));
        }

        [Test]
        public async Task WhenCreatingOrder_IfVoucherIsNotActive_ShouldError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                UserId = "usuarioteste@hotmail.com",
                ProductId = 1,
                VoucherId = 2,
            };
            // Act
            var result = await orderHandler.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Cupom inválido ou não encontrado."));
        }
        #endregion

        #region GetAllAsyncTests
        [Test]
        public async Task WhenGettingAllOrders_ShouldReturnAllOrdersOfUser()
        {
            // Arrange
            var request = new GetAllOrdersRequest
            {
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await orderHandler.GetAllAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Count.Equals(4));
            Assert.That(result.Code, Is.EqualTo(200));
        }
        #endregion

        #region GetByNumberAsyncTests
        [Test]
        public async Task WhenGettingOrderByNumber_IfCodeExists_ShouldReturnOrder()
        {
            // Arrange
            var request = new GetOrderByNumberRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "1234"
            };

            // Act
            var result = await orderHandler.GetByNumberAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
        }

        [Test]
        public async Task WhenGettingOrderByNumber_IfCodeDoesNotExists_ShouldError()
        {
            // Arrange
            var request = new GetOrderByNumberRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "1411"
            };

            // Act
            var result = await orderHandler.GetByNumberAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Pedido não encontrado."));
        }
        #endregion

        #region PayAsyncTests
        [Test]
        public async Task WhenPayingOrder_IfOrderIsNotPaid_ShouldSuccess()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "5678",
            };
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            var stripeHandler = new Mock<IStripeHandler>();
            var stripeTransactionResponse = new List<StripeTransactionResponse>()
            {
                new StripeTransactionResponse()
                {
                    Id = "5678",
                    Email = "userteste@hotmail.com",
                    Amount = 150,
                    AmountCaptured = 150,
                    Refunded = false,
                    Status = "",
                    Paid = true
                }
            };
            stripeHandler.Setup(s => s.GetTransactionsByOrderNumberAsync(It.IsAny<GetTransactionsByOrderNumberRequest>())).Returns(Task.FromResult(new Response<List<StripeTransactionResponse>>(stripeTransactionResponse, 200, "")));
            orderHandler = new OrderHandler(dbInMemory.GetContext(), stripeHandler.Object);
            context.ChangeTracker.Clear();

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
            Assert.That(result.Message, Is.EqualTo("Pedido pago com sucesso!"));
        }

        [Test]
        public async Task WhenPayingOrder_IfOrderIsPaid_ShouldError()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "9876",
            };

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido já está pago."));
        }

        [Test]
        public async Task WhenPayingOrder_IfOrderIsCanceled_ShouldError()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "1234",
            };

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido foi cancelado e não pode ser pago."));
        }

        [Test]
        public async Task WhenPayingOrder_IfOrderIsRefunded_ShouldError()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "5432",
            };

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido foi reembolsado e não pode ser pago."));
        }

        [Test]
        public async Task WhenPayingOrder_IfOrderNotFound_ShouldError()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "9999",
            };

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Pedido não encontrado."));
        }

        [Test]
        public async Task WhenPayingOrder_IfOrderIsNotPaidInStripe_ShouldError()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "5678",
            };
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            var stripeHandler = new Mock<IStripeHandler>();
            var stripeTransactionResponse = new List<StripeTransactionResponse>()
            {
                new StripeTransactionResponse()
                {
                    Id = "5678",
                    Email = "userteste@hotmail.com",
                    Amount = 150,
                    AmountCaptured = 150,
                    Refunded = false,
                    Status = "",
                    Paid = false
                }
            };
            stripeHandler.Setup(s => s.GetTransactionsByOrderNumberAsync(It.IsAny<GetTransactionsByOrderNumberRequest>())).Returns(Task.FromResult(new Response<List<StripeTransactionResponse>>(stripeTransactionResponse, 200, "")));
            orderHandler = new OrderHandler(dbInMemory.GetContext(), stripeHandler.Object);
            context.ChangeTracker.Clear();

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido não foi pago."));
        }

        [Test]
        public async Task WhenPayingOrder_IfOrderIsRefundedInStripe_ShouldError()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "5678",
            };
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            var stripeHandler = new Mock<IStripeHandler>();
            var stripeTransactionResponse = new List<StripeTransactionResponse>()
            {
                new StripeTransactionResponse()
                {
                    Id = "5678",
                    Email = "userteste@hotmail.com",
                    Amount = 150,
                    AmountCaptured = 150,
                    Refunded = true,
                    Status = "",
                    Paid = true
                }
            };
            stripeHandler.Setup(s => s.GetTransactionsByOrderNumberAsync(It.IsAny<GetTransactionsByOrderNumberRequest>())).Returns(Task.FromResult(new Response<List<StripeTransactionResponse>>(stripeTransactionResponse, 200, "")));
            orderHandler = new OrderHandler(dbInMemory.GetContext(), stripeHandler.Object);
            context.ChangeTracker.Clear();

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(400));
            Assert.That(result.Message, Is.EqualTo("Este pedido já teve o pagamento reembolsado."));
        }

        [Test]
        public async Task WhenPayingOrder_IfOrderIsNotFoundInStripe_ShouldError()
        {
            // Arrange
            var request = new PayOrderRequest
            {
                UserId = "userteste@hotmail.com",
                Number = "5678",
            };
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            var stripeHandler = new Mock<IStripeHandler>();
            stripeHandler.Setup(s => s.GetTransactionsByOrderNumberAsync(It.IsAny<GetTransactionsByOrderNumberRequest>())).Returns(Task.FromResult(new Response<List<StripeTransactionResponse>>(null, 200, "")));
            orderHandler = new OrderHandler(dbInMemory.GetContext(), stripeHandler.Object);
            context.ChangeTracker.Clear();

            // Act
            var result = await orderHandler.PayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(500));
            Assert.That(result.Message, Is.EqualTo("Não foi possível localizar pagamento."));
        }
        #endregion
    }
}
