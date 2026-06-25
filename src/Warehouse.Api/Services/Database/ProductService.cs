using Microsoft.EntityFrameworkCore;
using Warehouse.Api.Extensions;
using Warehouse.Api.Interfaces;
using Warehouse.Domain.Exception;
using Warehouse.Infrastructure.Data;
using Warehouse.Infrastructure.Dto;

namespace Warehouse.Api.Services.Database;

public class ProductService(IServiceProvider serviceProvider) : IProductsService
{
    public async Task<ProductDto> GetProduct(Guid id, CancellationToken cancellationToken = default)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();
        try
        {
            return (await context.Products.SingleAsync(p => p.Id == id, cancellationToken)).ToDto();
        }
        catch (InvalidOperationException)
        {
            throw new ProductNotFoundException(id);
        }
    }

    public async Task<ProductDto> GetProduct(string sku, CancellationToken cancellationToken = default)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();
        try
        {
            return (await context.Products.SingleAsync(p => p.Sku == sku, cancellationToken)).ToDto();
        }
        catch (InvalidOperationException)
        {
            throw new ProductNotFoundException(sku);
        }
    }

    public async Task<bool> CreateProduct(ProductDto dto, CancellationToken cancellationToken = default)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();
        try
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Sku = dto.Sku,
                Name = dto.Name,
                Barcode = dto.Barcode,
                HeightMm = dto.Height,
                WidthMm = dto.Width,
                IsActive = dto.IsActive,
                LengthMm = dto.Length,
                Weight = dto.Weight,
            };

            await context.Products.AddAsync(product, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateProduct(ProductDto dto, CancellationToken cancellationToken = default)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();

        Product? product = null;

        if (dto.Id != Guid.Empty)
        {
            product = await context.Products.SingleAsync(p => p.Id == dto.Id, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(dto.Sku))
        {
            product = await context.Products.SingleAsync(p => p.Sku == dto.Sku, cancellationToken);
        }

        ArgumentNullException.ThrowIfNull(product);

        try
        {
            product.Sku = dto.Sku;
            product.Name = dto.Name;
            product.Barcode = dto.Barcode;
            product.HeightMm = dto.Height;
            product.WidthMm = dto.Width;
            product.IsActive = dto.IsActive;
            product.LengthMm = dto.Length;
            product.Weight = dto.Weight;

            context.Products.Update(product);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken = default)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<WmsContext>();
        var product = await context.Products.SingleAsync(p => p.Id == id, cancellationToken);
        ArgumentNullException.ThrowIfNull(product);

        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}