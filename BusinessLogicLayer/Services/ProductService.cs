using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace BusinessLogicLayer.Services;
public class ProductService : IProductService
{
    private readonly IValidator<ProductAddRequest> _productAddRequestValidator; 
    private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
    private readonly IMapper _mapper;
    private readonly IProductsRepository _productsRepository;

    public ProductService(
        IValidator<ProductAddRequest> productAddRequestValidator,
        IValidator<ProductUpdateRequest> productUpdateRequestValidator,
        IMapper mapper,
        IProductsRepository productsRepository)
    {
        _productAddRequestValidator = productAddRequestValidator;
        _productUpdateRequestValidator = productUpdateRequestValidator;
        _mapper = mapper;
        _productsRepository = productsRepository;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
    {
        if(productAddRequest == null)
        {
            throw new ArgumentNullException(nameof(productAddRequest));
        }

        //Validate the productAddRequest using FluentValidation
        ValidationResult validationResult = await _productAddRequestValidator.ValidateAsync(productAddRequest);

        if(!validationResult.IsValid)
        {
            throw new ValidationException(string.Join("; ",validationResult.Errors.Select(temp=>temp.ErrorMessage)));
        }

        //Add product to the database
        Product productInput = _mapper.Map<Product>(productAddRequest);

        Product? addedProduct = await _productsRepository.AddProduct(productInput);

        if (addedProduct is null)
        {
            return null;
        }

        ProductResponse addedProductResponse = _mapper.Map<ProductResponse>(addedProduct);

        return addedProductResponse;
    }

    public async Task<bool> DeleteProduct(Guid productID)
    {
        if(productID == Guid.Empty)
        {
            throw new ArgumentException("ProductID cannot be empty", nameof(productID));
        }

        //If product with given productId does not exist, return false
        if (await _productsRepository.GetProductByCondition(temp => temp.ProductID == productID) is null)
        {
            return false;
        }

        return await _productsRepository.DeleteProduct(productID);
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> expression)
    {
        Product? retrievedProduct = await _productsRepository.GetProductByCondition(expression);

        if (retrievedProduct is null)
        {
            return null;
        }

        ProductResponse productResponse = _mapper.Map<ProductResponse>(retrievedProduct);

        return productResponse;
    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = (await _productsRepository.GetProducts());

        IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse?>>(products);

        return productResponses.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> expression)
    {
        IEnumerable<Product?> products = (await _productsRepository.GetProductsByCondition(expression));

        IEnumerable<ProductResponse?> productResponses = _mapper.Map<List<ProductResponse?>>(products);

        return productResponses.ToList();
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        //If product with given productId does not exist, return false
        if (await _productsRepository.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductID) is null)
        {
            throw new ArgumentNullException("Invalid productID");
        }

        //Validate the productUpdateRequest using FluentValidation
        ValidationResult validationResult = await _productUpdateRequestValidator.ValidateAsync(productUpdateRequest);

        if(!validationResult.IsValid)
        {
            throw new ValidationException(string.Join("; ",validationResult.Errors.Select(temp=>temp.ErrorMessage)));
        }


        //Update product in the database
        Product productInput = _mapper.Map<Product>(productUpdateRequest);

        Product? updatedProduct = await _productsRepository.UpdateProduct(productInput);

        ProductResponse updatedProductResponse = _mapper.Map<ProductResponse>(updatedProduct);

        return updatedProductResponse;
    }
}
