using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories;
public class ProductsRepository : IProductsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> AddProduct(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productID);

        if (existingProduct == null)
        {
            return false;
        }
        else
        {
            _dbContext.Products.Remove(existingProduct);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> expression)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<Product?>> GetProducts()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> expression)
    {
        return await _dbContext.Products.Where(expression).ToListAsync();
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = _dbContext.Products.FirstOrDefault(temp => temp.ProductID == product.ProductID); 

        if(existingProduct == null)
        {
            return null;
        }
        else
        {
            existingProduct.ProductName= product.ProductName;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.QuantityInStock = product.QuantityInStock;
            existingProduct.Category = product.Category;

            await _dbContext.SaveChangesAsync();

            return existingProduct;
        }
    }
}
