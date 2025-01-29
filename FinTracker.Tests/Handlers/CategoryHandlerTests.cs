using FinTracker.Api.Handlers;
using FinTracker.Core.Requests.Categories;
using FinTracker.Tests.Configuration;

namespace FinTracker.Tests.Handlers
{
    [TestFixture]
    public class CategoryHandlerTests
    {
        private CategoryHandler categoryHandler;

        [SetUp]
        public void Setup()
        {
            var dbInMemory = new DbInMemory();
            var context = dbInMemory.GetContext();
            categoryHandler = new CategoryHandler(context);
        }

        #region GetAllAsyncTests
        [Test]
        public async Task WhenGettingAllCategories_IfHaveCategories_ShouldSuccess()
        {
            // Arrange
            var request = new GetAllCategoriesRequest
            {
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await categoryHandler.GetAllAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
        }

        [Test]
        public async Task WhenGettingAllCategories_IfDoesNotHaveCategories_ShouldError()
        {
            // Arrange
            var request = new GetAllCategoriesRequest
            {
                UserId = "userteste2@hotmail.com"
            };

            // Act
            var result = await categoryHandler.GetAllAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Categorias não encontradas."));
        }
        #endregion
        
        #region GetByIdAsyncTests
        [Test]
        public async Task WhenGettingCategoryById_IfIdFound_ShouldSuccess()
        {
            // Arrange
            var request = new GetCategoryByIdRequest
            {
                Id = 1,
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await categoryHandler.GetByIdAsync(request);
            
            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
        }

        [Test]
        public async Task WhenGettingCategoryById_IfIdNotFound_ShouldError()
        {
            // Arrange
            var request = new GetCategoryByIdRequest
            {
                Id = 999,
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await categoryHandler.GetByIdAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Categoria não encontrada."));
        }
        #endregion

        #region CreateAsyncTests
        [Test]
        public async Task WhenCreatingACategory_IfAllDataPassed_ShouldSuccess()
        {
            // Arrange
            var request = new CreateCategoryRequest
            {
                UserId = "userteste@hotmail.com",
                Title = "Teste",
                Description = "Teste",
            };

            // Act
            var result = await categoryHandler.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(201));
            Assert.That(result.Message, Is.EqualTo("Categoria criada com sucesso!"));
        }

        [Test]
        public async Task WhenCreatingACategory_IfMissingData_ShouldError()
        {
            // Arrange
            var request = new CreateCategoryRequest
            {
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await categoryHandler.CreateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(500));
            Assert.That(result.Message, Is.EqualTo("Não foi possível criar a categoria."));
        }
        #endregion

        #region UpdateAsyncTests
        [Test]
        public async Task WhenUpdatingACategory_IfCategoryNotFound_ShouldError()
        {
            // Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 9999,
                UserId = "userteste@hotmail.com",
            };

            // Act
            var result = await categoryHandler.UpdateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Categoria não encontrada."));
        }

        [Test]
        public async Task WhenUpdatingACategory_IfAllDataPassed_ShouldSuccess()
        {
            // Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                UserId = "userteste@hotmail.com",
                Title = "Update teste",
                Description = "Update teste",
            };

            // Act
            var result = await categoryHandler.UpdateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
        }
        #endregion
        
        #region DeleteAsyncTests
        [Test]
        public async Task WhenDeletingACategory_IfCategoryNotFound_ShouldError()
        {
            // Arrange
            var request = new DeleteCategoryRequest
            {
                Id = 9999,
                UserId = "userteste@hotmail.com"
            };

            // Act
            var result = await categoryHandler.DeleteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Code, Is.EqualTo(404));
            Assert.That(result.Message, Is.EqualTo("Categoria não encontrada."));
        }

        [Test]
        public async Task WhenDeletingACategory_IfCategoryFound_ShouldSuccess()
        {
            // Arrange
            var request = new DeleteCategoryRequest
            {
                Id = 1,
                UserId = "userteste@hotmail.com",
            };

            // Act
            var result = await categoryHandler.DeleteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(200));
        }
        #endregion
    }
}
