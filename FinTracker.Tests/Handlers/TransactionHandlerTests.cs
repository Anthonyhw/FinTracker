using FinTracker.Api.Handlers;
using FinTracker.Core.Requests.Transactions;
using FinTracker.Tests.Configuration;

namespace FinTracker.Tests.Handlers
{
    [TestFixture]
    public class TransactionHandlerTests
    {
        private TransactionHandler transactionHandler;

        [SetUp]
        public void Setup()
        {
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            transactionHandler = new TransactionHandler(context);
        }

        #region GetByIdAsyncTests
        [Test]
        public async Task WhenGettingTransactionById_IfIdFound_ShouldSuccess()
        {
            // Arrange
            var request = new GetTransactionByIdRequest
            {
                Id = 1,
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await transactionHandler.GetByIdAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Code, Is.EqualTo(200));
            });
        }

        [Test]
        public async Task WhenGettingTransactionById_IfIdNotFound_ShouldError()
        {
            // Arrange
            var request = new GetTransactionByIdRequest
            {
                Id = 999,
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await transactionHandler.GetByIdAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Code, Is.EqualTo(404));
                Assert.That(result.Message, Is.EqualTo("Transação não encontrada."));
            });
        }
        #endregion

        #region GetByPeriodAsyncTests
        [Test]
        public async Task WhenGettingTransactionsByPeriod_IfPeriodHaveTransactions_ShouldSuccess()
        {
            // Arrange
            var request = new GetTransactionsByPeriodRequest
            {
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await transactionHandler.GetByPeriodAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Code, Is.EqualTo(200));
            });
        }

        [Test]
        public async Task WhenGettingTransactionsByPeriod_IfPeriodDoesNotHaveTransactions_ShouldError()
        {
            // Arrange
            var request = new GetTransactionsByPeriodRequest
            {
                UserId = "userteste@hotmail.com",
                StartDate = DateTime.UtcNow.AddMonths(-3),
                EndDate= DateTime.UtcNow.AddMonths(-2),
            };

            // Act
            var result = await transactionHandler.GetByPeriodAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Code, Is.EqualTo(404));
                Assert.That(result.Message, Is.EqualTo("Transações não encontradas."));
            });
        }
        #endregion

        #region CreateAsyncTests
        [Test]
        public async Task WhenCreatingATransaction_IfAllDataPassed_ShouldSuccess()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                UserId = "userteste@hotmail.com",
                Amount = 100,
                CategoryId = 1,
                Title = "Teste",
                Type = Core.Enums.EtransactionType.Deposit
            };

            // Act
            var result = await transactionHandler.CreateAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Code, Is.EqualTo(201));
                Assert.That(result.Message, Is.EqualTo("Transação criada com sucesso!"));
            });
        }

        [Test]
        public async Task WhenCreatingATransaction_IfMissingData_ShouldError()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await transactionHandler.CreateAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Code, Is.EqualTo(500));
                Assert.That(result.Message, Is.EqualTo("Não foi possível criar a transação."));
            });
        }
        #endregion

        #region UpdateAsyncTests
        [Test]
        public async Task WhenUpdatingATransaction_IfTransactionNotFound_ShouldError()
        {
            // Arrange
            var request = new UpdateTransactionRequest
            {
                Id = 9999,
                UserId = "userteste@hotmail.com",
                Amount = 100,
                CategoryId = 1,
                Title = "Teste",
                Type = Core.Enums.EtransactionType.Deposit
            };

            // Act
            var result = await transactionHandler.UpdateAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Code, Is.EqualTo(404));
                Assert.That(result.Message, Is.EqualTo("Transação não encontrada."));
            });
        }

        [Test]
        public async Task WhenUpdatingATransaction_IfAllDataPassed_ShouldSuccess()
        {
            // Arrange
            var request = new UpdateTransactionRequest
            {
                Id = 1,
                UserId = "userteste@hotmail.com",
                Amount = 100,
                CategoryId = 1,
                Title = "Teste65",
                Type = Core.Enums.EtransactionType.Deposit,
                PaidOrReceivedAt = DateTime.Now
            };

            // Act
            var result = await transactionHandler.UpdateAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Code, Is.EqualTo(200));
                Assert.That(result.Message, Is.EqualTo("Transação atualizada com sucesso!"));
            });
        }
        #endregion

        #region DeleteAsyncTests
        [Test]
        public async Task WhenDeletingATransaction_IfTransactionNotFound_ShouldError()
        {
            // Arrange
            var request = new DeleteTransactionRequest
            {
                Id = 9999,
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await transactionHandler.DeleteAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
                Assert.That(result.Code, Is.EqualTo(404));
                Assert.That(result.Message, Is.EqualTo("Transação não encontrada."));
            });
        }

        [Test]
        public async Task WhenDeletingATransaction_IfTransactionFound_ShouldSuccess()
        {
            // Arrange
            var request = new DeleteTransactionRequest
            {
                Id = 1,
                UserId = "userteste@hotmail.com",
            };

            // Act
            var result = await transactionHandler.DeleteAsync(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Code, Is.EqualTo(200));
                Assert.That(result.Message, Is.EqualTo("Transação removida com sucesso!"));
            });
        }
        #endregion
    }
}
