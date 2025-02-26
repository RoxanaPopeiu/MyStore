using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Interfaces.Services;
using MyStore.Mapping;
using MyStore.Models;
using static MyStore.Services.PromotionService;

namespace MyStore.Services
{
    public class ProductService(ApplicationDbContext context): IProductService
    {
        #region CRUD
        public async Task<ProductDto> Create(ProductDto productDto)
        {

            if(await CheckProductExistence(productDto))
                throw new ProductAlreadyExistsException($"A product with the name '{productDto.Name}' already exists.");
            ValidateProduct(productDto);

            //Retrieve category,brand and promotion from DB
            var existingCategory=await context.Categories.FirstOrDefaultAsync(c=>c.Id==productDto.CategoryId || c.Name==productDto.Name);
            var existingBrand=await context.Brands.FirstOrDefaultAsync(b=>b.Id == productDto.BrandId || b.Name == productDto.BrandName);
          
            // Ensure category and brand exist before proceeding
            if (existingCategory == null)
                throw new Exception($"Category '{productDto.CategoryName}' does not exist. Please create it first.");

            if (existingBrand == null)
                throw new Exception($"Brand '{productDto.BrandName}' does not exist. Please create it first.");

            //Retrieve Promotion from DB
            var promotion = productDto.PromotionId != null
                ? await context.Promotions.FirstOrDefaultAsync(p => p.Id == productDto.PromotionId)
                : null;

            //ConvertDto to Product(promotion can be null)
            var product = productDto.ToProduct();
            product.Brand = existingBrand;
            product.Category = existingCategory;
            var existingSizes = await context.Sizes.Where(s => productDto.Sizes.Select(sz => sz.Name).Contains(s.Name)).ToListAsync();
            product.ProductSizes = productDto.Sizes.Select(sizeDto =>
            {
  
                var existingSize = existingSizes.FirstOrDefault(s => s.Name == sizeDto.Name);

                if (existingSize != null)
                {
                   
                    return new ProductSize
                    {
                        Size = existingSize,
                        Product = product
                    };
                }
                else
                {
                    
                    return new ProductSize
                    {
                        Size = new Size
                        {
                            Name = sizeDto.Name,
                            Description = sizeDto.Description,
                            CategoryId = productDto.CategoryId
                        },
                        Product = product
                    };
                }
            }).ToList();
            var result=context.Products.Add(product);
            await context.SaveChangesAsync();
            return result.Entity.ToProductDto();


        }
        public async Task<List<ProductDto>> ReadAllProducts()
        {
            var products = await context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductSizes)
                    .ThenInclude(ps => ps.Size)
                .ToListAsync(); 

            return ProductMapping.ToProductDtoList(products);
        }

        public async Task <Product> ReadOneProduct(int id)
        {
            return await context.Products
                   .Include(p => p.Brand)
                   .Include(p => p.Category) 
                   .SingleOrDefaultAsync(p => p.Id == id);
        }
        public async Task<ProductDto> Update(int id, ProductDto productDto)
        {
            ValidateProduct(productDto);
            if (await CheckProductExistence(productDto))
                throw new ProductAlreadyExistsException($"A product with the name '{productDto.Name}' already exists.");
            var existingProduct =await context.Products
                .Include(p => p.Category)
                .Include(p=>p.Brand)
                .Include(p=>p.Promotion)
                .Include(p => p.ProductSizes) 
                .FirstOrDefaultAsync(p=>p.Id==id);

            if (existingProduct == null)
                throw new Exception("Product not found.");

            //Update the existing product with new values from the DTO
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            //Update CAtegory and Brand (must exist)
            var category=await context.Categories.FirstOrDefaultAsync(c => c.Id==productDto.CategoryId);
            if (category == null)
                throw new Exception("Invalid category.");
            var brand = await context.Brands.FirstOrDefaultAsync(b => b.Id == productDto.BrandId);
            if (brand == null)
                throw new Exception("Invalid brand.");

            existingProduct.Category = category;
            existingProduct.Brand = brand;
            // Update Promotion (optional)
            if (productDto.PromotionId.HasValue)
            {
                var promotion =await  context.Promotions.FirstOrDefaultAsync(p => p.Id == productDto.PromotionId);
                existingProduct.Promotion = promotion; // If not found, it will be set to null
            }
            else
            {
                existingProduct.Promotion = null; // Remove promotion if none is provided
            }
            // Update Sizes (Many-to-Many Relationship)
            if (productDto.Sizes != null && productDto.Sizes.Any())
            {
                // Find existing sizes in the database
                var selectedSizes =await context.Sizes.Where(s => productDto.Sizes.Select(sz => sz.Id).Contains(s.Id)).ToListAsync();

                // Clear existing ProductSizes and assign new ones
                existingProduct.ProductSizes.Clear();
                foreach (var size in selectedSizes)
                {
                    existingProduct.ProductSizes.Add(new ProductSize { Product = existingProduct, Size = size });
                }
            }

            //Save changes to the DB
            await context.SaveChangesAsync();
            return existingProduct.ToProductDto();

        }
        public async Task<bool> Delete(int id)
        {
            var extProduct=await ReadOneProduct(id);
            if (extProduct!=null)
            {
                context.Products.Remove(extProduct);
                await context.SaveChangesAsync();
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
        private void ValidateProduct(ProductDto productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto), "Product cannot be null.");

            if (productDto.Price <= 0)
                throw new Exception("Price cannot be 0.");

            var categoryExists = context.Categories.Any(c => c.Id == productDto.CategoryId || c.Name==productDto.CategoryName);
            if (!categoryExists)
                throw new Exception("Category is required and must exist.");

            var brandExists = context.Brands.Any(b => b.Id == productDto.BrandId || b.Name==productDto.BrandName);
            if (!brandExists)
                throw new Exception("Brand is required and must exist.");

        }
        private async Task<bool> CheckProductExistence(ProductDto productDto)
        {
            return await context.Products.AnyAsync(x => x.Name == productDto.Name);
        }

        #endregion

    }
}
