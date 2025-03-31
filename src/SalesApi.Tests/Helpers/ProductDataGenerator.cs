using Application.Queries.Products;
using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;

public static class ProductDataGenerator
{
    public static List<GetProductsQueryResponse> GenerateFakeProducts(int count = 2)
    {
        var productFaker = new Faker<GetProductsQueryResponse>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Finance.Amount(10, 500))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl());

        return productFaker.Generate(count);
    }

    public static List<Product> GenerateFakeEntityProducts(int count = 2)
    {
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Finance.Amount(10, 500))
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl());

        return productFaker.Generate(count);
    }
}

