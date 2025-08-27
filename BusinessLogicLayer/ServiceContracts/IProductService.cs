using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace BusinessLogicLayer.ServiceContracts;
public interface IProductService
{
    /// <summary>
    /// Retrieves the list of products from the products repository
    /// </summary>
    /// <returns>Returns list of ProductsResponse objects</returns>
    Task<List<ProductResponse?>> GetProducts();

    /// <summary>
    /// Retrieves the list of products matching with given condition
    /// </summary>
    /// <param name="expression"> Expression that represents condition to check</param>
    /// <returns>Return matching products</returns>
    Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> expression);

    /// <summary>
    /// Returns a single product that matches with given condition
    /// </summary>
    /// <param name="expression">Expression that represents condition to check</param>
    /// <returns>Returns matching product or null</returns>
    Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> expression);

    /// <summary>
    /// Adds a new product to the products repository
    /// </summary>
    /// <param name="productAddRequest">product to inserts</param>
    /// <returns>product after inserting or null if unseccessful</returns>
    Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest);

    /// <summary>
    /// Updates the eisting product based on the productID
    /// </summary>
    /// <param name="productUpdateRequest">product data to update</param>
    /// <returns>Return product object after successful update or null otherwhise</returns>
    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);

    /// <summary>
    /// Deletes an existing product based on given productID
    /// </summary>
    /// <param name="productID">ProductID to delete</param>
    /// <returns>returns true if the deletion was successful otherwhise false</returns>
    Task<bool> DeleteProduct(Guid productID);

}
