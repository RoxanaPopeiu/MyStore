using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Repositories;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;
using MyStore.Repositories;
using static MyStore.Services.PromotionService;

namespace MyStore.Services
{
    public class ProductService(IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IBrandRepository brandRepository,
        IPromotionRepository promotionRepository,
        ISizeRepository sizeRepository) : IProductService
    {
        #region CRUD
        public async Task<ProductDto> Create(ProductDto productDto)
        {

            if(await productRepository.GetByNameAsync(productDto.Name) != null)
                throw new ProductAlreadyExistsException($"A product with the name '{productDto.Name}' already exists.");

            ValidateProduct(productDto);

            //Retrieve category,brand and promotion from DB
            var existingCategory=await categoryRepository.GetByIdAsync(productDto.CategoryId);
            var existingBrand=await brandRepository.GetByIdAsync(productDto.BrandId);

            // Ensure category and brand exist before proceeding
            if (existingCategory == null)
                throw new Exception($"Category '{productDto.CategoryName}' does not exist. Please create it first.");

            if (existingBrand == null)
                throw new Exception($"Brand '{productDto.BrandName}' does not exist. Please create it first.");

            //Retrieve Promotion from DB
            var promotion = productDto.PromotionId != null
                ? await promotionRepository.GetByIdAsync(productDto.PromotionId.Value)
                : null;

            //ConvertDto to Product(promotion can be null)
            var product = productDto.ToProduct();
            product.Brand = existingBrand;
            product.Category = existingCategory;
            var existingSizes = await sizeRepository.GetByNameAsync(productDto.Sizes.Select(sz => sz.Name));
            product.ProductSizes = productDto.Sizes.Select(sizeDto =>
            {
                var existingSize = existingSizes.FirstOrDefault(s => s.Name == sizeDto.Name);
                return new ProductSize
                    {
                        Size = existingSize ?? new Size
                        {
                            Name = sizeDto.Name,
                            Description = sizeDto.Description,
                            CategoryId = productDto.CategoryId
                        },
                        Product = product
                    }; 
                 }).ToList();
            var savedProduct = await productRepository.AddAsync(product);
            return savedProduct.ToProductDto();

        }
        public async Task<List<ProductDto>> ReadAllProducts()
        {
            var products = await productRepository.GetAllWithDetailsAsync();
            return products.ToProductDtoList();
        }
        public async Task <Product> ReadOneProduct(int id)
        {
            return await productRepository.GetByIdWithDetailsAsync(id);
        }
        public async Task<ProductDto> Update(int id, ProductDto productDto)
        {
            ValidateProduct(productDto);

            bool nameExists = await productRepository.ExistsByNameAsync(productDto.Name);
            if (nameExists)
                throw new ProductAlreadyExistsException($"A product with the name '{productDto.Name}' already exists.");

            var existingProduct =await productRepository.GetByIdWithDetailsAsync(id);
            if (existingProduct == null)
                throw new Exception("Product not found.");

            //Update the existing product with new values from the DTO
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            //Update CAtegory and Brand (must exist)
            var category=await categoryRepository.GetByIdAsync(productDto.CategoryId);
            if (category == null)
                throw new Exception("Invalid category.");
            var brand = await brandRepository.GetByIdAsync(productDto.BrandId);
            if (brand == null)
                throw new Exception("Invalid brand.");

            existingProduct.Category = category;
            existingProduct.Brand = brand;
            // Update Promotion (optional)
            existingProduct.Promotion=productDto.PromotionId.HasValue ? await promotionRepository.GetByIdAsync(productDto.PromotionId.Value) : null;

            // Update Sizes (Many-to-Many Relationship)
            if (productDto.Sizes != null && productDto.Sizes.Any())
            {
                // Find existing sizes in the database
                var selectedSizes =await sizeRepository.GetByNameAsync(productDto.Sizes.Select(sz => sz.Name));

                // Clear existing ProductSizes and assign new ones
                existingProduct.ProductSizes.Clear();
                foreach (var size in selectedSizes)
                {
                    existingProduct.ProductSizes.Add(new ProductSize { Product = existingProduct, Size = size });
                }
            }

            //Save changes
            await productRepository.UpdateAsync(existingProduct);
            return existingProduct.ToProductDto();

        }
        public async Task<bool> Delete(int id)
        {
            var extProduct=await productRepository.GetByIdAsync(id);
            if (extProduct!=null)
            {
                await productRepository.DeleteAsync(extProduct);
                return true;
            }
            return false;
        }
        #endregion

        #region Exception Handling
        public class ProductAlreadyExistsException : Exception
        {
            public ProductAlreadyExistsException(string message) : base(message) { }
        }
        #endregion

        #region Private Methods

          private async Task ValidateProduct(ProductDto productDto)
          {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto), "Product cannot be null.");

            if (productDto.Price <= 0)
                throw new Exception("Price cannot be 0.");

            var categoryTask = categoryRepository.ExistsByIdOrNameAsync(productDto.CategoryId, productDto.CategoryName);
            var brandTask = brandRepository.ExistsByIdOrNameAsync(productDto.BrandId, productDto.BrandName);

            await Task.WhenAll(categoryTask, brandTask);

            if (!categoryTask.Result)
                throw new Exception("Category is required and must exist.");

            if (!brandTask.Result)
                throw new Exception("Brand is required and must exist.");
          }
    
        #endregion

    }
}
