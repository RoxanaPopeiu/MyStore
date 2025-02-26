using Microsoft.EntityFrameworkCore;
using MyStore.DTO;
using MyStore.Mapping;
using MyStore.Models;
using static MyStore.Services.PromotionService;

namespace MyStore.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        #region CRUD
        public ProductDto Create(ProductDto productDto)
        {

            if(CheckProductExistence(productDto))
                throw new ProductAlreadyExistsException($"A product with the name '{productDto.Name}' already exists.");
            ValidateProduct(productDto);

            //Retrieve category,brand and promotion from DB
            var existingCategory=_context.Categories.FirstOrDefault(c=>c.Id==productDto.CategoryId || c.Name==productDto.Name);
            var existingBrand=_context.Brands.FirstOrDefault(b=>b.Id == productDto.BrandId || b.Name == productDto.BrandName);
          
            // Ensure category and brand exist before proceeding
            if (existingCategory == null)
                throw new Exception($"Category '{productDto.CategoryName}' does not exist. Please create it first.");

            if (existingBrand == null)
                throw new Exception($"Brand '{productDto.BrandName}' does not exist. Please create it first.");

            //Retrieve Promotion from DB
            var promotionDto =productDto.PromotionId!=null
                ?_context.Promotions.FirstOrDefault(p=>p.Id==productDto.PromotionId)?.ToPromotionDto() : null;

            //ConvertDto to Product(promotion can be null)
            var product = productDto.ToProduct();
            product.Brand = existingBrand;
            product.Category = existingCategory;
            var existingSizes = _context.Sizes.Where(s => productDto.Sizes.Select(sz => sz.Name).Contains(s.Name)).ToList();
            product.ProductSizes = productDto.Sizes.Select(sizeDto =>
            {
                // 🔍 Găsește Size existent
                var existingSize = existingSizes.FirstOrDefault(s => s.Name == sizeDto.Name);

                if (existingSize != null)
                {
                    Console.WriteLine($"🔍 Size {existingSize.Name} EXISTS, reusing ID: {existingSize.Id}");
                    return new ProductSize
                    {
                        Size = existingSize,
                        Product = product
                    };
                }
                else
                {
                    Console.WriteLine($"🆕 Creating NEW Size: {sizeDto.Name}");
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
            var result=_context.Products.Add(product);
            _context.SaveChanges();
            return result.Entity.ToProductDto();


        }
        public List<ProductDto>ReadAllProducts()
        {
            var result = ProductMapping.ToProductDtoList(
                _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.ProductSizes)
                        .ThenInclude(ps => ps.Size) // ✅ Add this line!
            );

            return result;
        }
        public Product ReadOneProduct(int id)
        {
            return  _context.Products
                   .Include(p => p.Brand)
                   .Include(p => p.Category) 
                   .SingleOrDefault(p => p.Id == id);
        }
        public  ProductDto Update(int id, ProductDto productDto)
        {
            ValidateProduct(productDto);
            if (CheckProductExistence(productDto))
                throw new ProductAlreadyExistsException($"A product with the name '{productDto.Name}' already exists.");
            var existingProduct = _context.Products
                .Include(p => p.Category)
                .Include(p=>p.Brand)
                .Include(p=>p.Promotion)
                .Include(p => p.ProductSizes) // Include Many-to-Many relationship for Sizes
                .FirstOrDefault(p=>p.Id==id);

            if (existingProduct == null)
                throw new Exception("Product not found.");

            //Update the existing product with new values from the DTO
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;

            //Update CAtegory and Brand (must exist)
            var category=_context.Categories.FirstOrDefault(c => c.Id==productDto.CategoryId);
            if (category == null)
                throw new Exception("Invalid category.");
            var brand = _context.Brands.FirstOrDefault(b => b.Id == productDto.BrandId);
            if (brand == null)
                throw new Exception("Invalid brand.");

            existingProduct.Category = category;
            existingProduct.Brand = brand;
            // Update Promotion (optional)
            if (productDto.PromotionId.HasValue)
            {
                var promotion = _context.Promotions.FirstOrDefault(p => p.Id == productDto.PromotionId);
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
                var selectedSizes = _context.Sizes.Where(s => productDto.Sizes.Select(sz => sz.Id).Contains(s.Id)).ToList();

                // Clear existing ProductSizes and assign new ones
                existingProduct.ProductSizes.Clear();
                foreach (var size in selectedSizes)
                {
                    existingProduct.ProductSizes.Add(new ProductSize { Product = existingProduct, Size = size });
                }
            }

            //Save changes to the DB
            _context.SaveChanges();
            return existingProduct.ToProductDto();

        }
        public bool Delete(int id)
        {
            var extProduct=ReadOneProduct(id);
            if (extProduct!=null)
            {
                _context.Products.Remove(extProduct);
                _context.SaveChanges();
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

            var categoryExists = _context.Categories.Any(c => c.Id == productDto.CategoryId || c.Name==productDto.CategoryName);
            if (!categoryExists)
                throw new Exception("Category is required and must exist.");

            var brandExists = _context.Brands.Any(b => b.Id == productDto.BrandId || b.Name==productDto.BrandName);
            if (!brandExists)
                throw new Exception("Brand is required and must exist.");

        }
        private bool CheckProductExistence(ProductDto productDto)
        {
            return  _context.Products.Any(x=>x.Name == productDto.Name);
        }
        #endregion

    }
}
