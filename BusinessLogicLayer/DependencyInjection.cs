using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BussinesLogicLayer;
public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {

        services.AddAutoMapper(
            cfg => { },
            typeof(ProductAddRequestToProductMappingProfile).Assembly);

        services.AddValidatorsFromAssembly(typeof(ProductAddRequestValidator).Assembly);

        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
