using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace DataAccessLayer.RepositoryContracts;


public interface IProductsRepository
{
    /// <summary>
    /// Retrieves all products asymchronously
    /// </summary>
    /// <returns>Returns all products from the table</returns>
    Task <IEnumerable<Product?>> GetProducts();

    /// <summary>
    /// Asynchronously retrieves all products that satisfy the given condition
    /// </summary>
    /// <param name="expression"> the condition to filter</param>
    /// <returns>returns a collection of products that satisfies the condition</returns>
    Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> expression);

    /// <summary>
    /// Retrieves a product that matches the specified condition.
    /// </summary>
    /// <param name="expression">An expression that defines the condition to evaluate. The condition is used to filter the product to be
    /// retrieved.</param>
    /// <returns>returns a single product or null if no matching product is found.</returns>
    Task <Product?> GetProductByCondition(Expression<Func<Product, bool>> expression);

    /// <summary>
    /// Adds a new product into the products table asynchronously
    /// </summary>
    /// <param name="product"> the product to be added</param>
    /// <returns>returns the added product object or null if unsuccessful</returns>
    Task<Product?> AddProduct(Product product);

    /// <summary>
    /// updates an existing product asynchronously
    /// </summary>
    /// <param name="product">the product to be updated</param>
    /// <returns>returns the updated product or null if not found</returns>
    Task<Product?> UpdateProduct(Product product);

    /// <summary>
    /// deletes the product asynchronously
    /// </summary>
    /// <param name="productID">the product ID to be deleted</param>
    /// <returns>returns true if the deletion was successful, false otherwise</returns>
    Task<bool> DeleteProduct(Guid productID);
}
