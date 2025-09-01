using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace ProductsMicroService.API.APIEndpoints;
public static class ProductAPIEndpoints
{
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        //GET /api/products
        app.MapGet("/api/products", async(IProductService productService) =>
        {
            List<ProductResponse?> products =  await productService.GetProducts();
            return Results.Ok(products);
        });

        //GET /api/products/search/product-id/{ProductID}
        app.MapGet("/api/products/search/product-id/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
        {
            ProductResponse? product = await productService.GetProductByCondition(temp => temp.ProductID == ProductID);

            if (product is null)
            {
                return Results.NotFound($"Product with ID: {ProductID} not found");
            }

            return Results.Ok(product);
        });

        //GET /api/products/search/{SearchString}
        app.MapGet("/api/products/search/{SearchString}", async (IProductService productService, string SearchString) =>
        {
            List<ProductResponse?> productsByProductName = await productService.GetProductsByCondition(temp => temp.ProductName != null && temp.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            List<ProductResponse?> productsByCategory = await productService.GetProductsByCondition(temp => temp.Category != null && temp.Category.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            var products = productsByProductName.Union(productsByCategory);

            return Results.Ok(products);
        });

        //DELETE /api/products/{ProductID}
        app.MapDelete("/api/products/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
        {
            bool isDeleted = await productService.DeleteProduct(ProductID);

            if (isDeleted)
            {
                return Results.Ok(true);
            }
            else
            {
                return Results.Problem($"Error deleting product with ID: {ProductID}");
            }
        });

        //POST /api/products
        app.MapPost("/api/products", async (IProductService productService, IValidator<ProductAddRequest> productAddRequestValidator, ProductAddRequest product) =>
        {
            //VAlidate the productAddRequest

            ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(product);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return Results.ValidationProblem(errors);
            }

            ProductResponse? addedProduct = await productService.AddProduct(product);

            if (addedProduct is null)
            {
                return Results.Problem("Unable to add the product");
            }
            else
            {
                return Results.Created($"/api/products/search/product-id/{addedProduct.ProductID}", addedProduct);
            }
        });

        //PUT /api/products
        app.MapPut("/api/products", async (IProductService productService, IValidator<ProductUpdateRequest> productUpdateRequestValidator, ProductUpdateRequest product) =>
        {

            //VAlidate the productUpdateRequest

            ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(product);

            if (!validationResult.IsValid)
            {
                Dictionary<string, string[]> errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return Results.ValidationProblem(errors);
            }

            ProductResponse? updatedProduct = await productService.UpdateProduct(product);
            if (updatedProduct is null)
            {
                return Results.Problem("Unable to update the product");
            }
            else
            {
                return Results.Ok(updatedProduct);
            }
        });

        return app;
    }
}
