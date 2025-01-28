using FinTracker.Api.Handlers;
using FinTracker.Core.Requests.Orders;
using FinTracker.Tests.Configuration;

namespace FinTracker.Tests.Handlers
{
    [TestFixture]
    public class ProductHandlerTests
    {
        private ProductHandler productHandler;
        
        [SetUp]
        public void Setup()
        {
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            productHandler = new ProductHandler(context);
        }
        #region GetAllAsyncTests
        [Test]
        public async Task WhenGettingAllProducts_ShouldSuccess()
        {
            // Arrange
            var request = new GetAllProductsRequest();

            // Act
            var result = await productHandler.GetAllAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
        }
        #endregion

        #region GetBySlugAsync
        [Test]
        public async Task WhenGettingProductBySlug_IfSlugExists_ShouldSuccess()
        {
            // Arrange
            var request = new GetProductBySlugRequest
            {
                Slug = "teste-1",
            };

            // Act
            var result = await productHandler.GetBySlugAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
        }

        [Test]
        public async Task WhenGettingProductBySlug_IfSlugDoesNotExists_ShouldError()
        {
            // Arrange
            var request = new GetProductBySlugRequest
            {
                Slug = "teste-invalido",
            };

            // Act
            var result = await productHandler.GetBySlugAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Produto não encontrado."));
        }
        #endregion
    }
}
