using Application.Commands.Sales;
using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;

public static class SaleDataGenerator
{
    public static List<Sale> GenerateFakeSales(int count = 5)
    {
        var saleItemsFaker = new Faker<SaleItem>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.ProductId, f => Guid.NewGuid())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(5, 100))
            .RuleFor(i => i.Discount, f => f.Random.Decimal(0, 0.2m))
            .RuleFor(i => i.Total, (f, i) => i.Quantity * i.UnitPrice * (1 - i.Discount))
            .RuleFor(i => i.SaleId, f => Guid.NewGuid())
            .RuleFor(i => i.Canceled, f => f.Random.Bool());

        var salesFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(6))
            .RuleFor(s => s.SaleDate, f => f.Date.Past())
            .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
            .RuleFor(s => s.BranchId, f => Guid.NewGuid())
            .RuleFor(s => s.TotalAmount, f => f.Finance.Amount(100, 1000))
            .RuleFor(s => s.Canceled, f => f.Random.Bool())
            .RuleFor(s => s.Items, f => saleItemsFaker.Generate(f.Random.Int(1, 5)));

        return salesFaker.Generate(count);
    }

    public static CreateSaleCommand GenerateFakeCreateSaleCommand()
    {
        var saleItemCommandFaker = new Faker<CreateSaleItemCommand>()
            .RuleFor(i => i.ProductId, f => Guid.NewGuid())
            .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 100));

        var createSaleCommandFaker = new Faker<CreateSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(8))
            .RuleFor(s => s.SaleDate, f => f.Date.Recent())
            .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
            .RuleFor(s => s.BranchId, f => Guid.NewGuid())
            .RuleFor(s => s.Items, f => saleItemCommandFaker.Generate(f.Random.Int(2, 5)));

        return createSaleCommandFaker.Generate();
    }

    public static Sale GenerateFakeSale(CreateSaleCommand command)
    {
        var saleItemFaker = new Faker<SaleItem>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.ProductId, f => f.PickRandom(command.Items).ProductId)
            .RuleFor(i => i.Quantity, f => f.PickRandom(command.Items).Quantity)
            .RuleFor(i => i.UnitPrice, f => f.PickRandom(command.Items).UnitPrice)
            .RuleFor(i => i.Discount, (f, i) => i.Quantity >= 4 && i.Quantity < 10 ? 0.10m : (i.Quantity >= 10 ? 0.20m : 0.00m))
            .RuleFor(i => i.Total, (f, i) => i.Quantity * i.UnitPrice * (1 - i.Discount))
            .RuleFor(i => i.SaleId, f => Guid.NewGuid())
            .RuleFor(i => i.Canceled, f => false);

        var saleFaker = new Faker<Sale>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.SaleNumber, f => command.SaleNumber)
            .RuleFor(s => s.SaleDate, f => command.SaleDate)
            .RuleFor(s => s.CustomerId, f => command.CustomerId)
            .RuleFor(s => s.BranchId, f => command.BranchId)
            .RuleFor(s => s.Canceled, f => false)
            .RuleFor(s => s.Items, f => saleItemFaker.Generate(command.Items.Count))
            .RuleFor(s => s.TotalAmount, (f, s) => s.Items.Sum(i => i.Total));

        return saleFaker.Generate();
    }

    public static CreateSaleCommandResponse GenerateFakeCreateSaleResponse(Sale sale)
    {
        var saleItemResponseFaker = new Faker<CreateSaleItemCommandResponse>()
            .RuleFor(i => i.Id, f => f.PickRandom(sale.Items).Id)
            .RuleFor(i => i.ProductId, f => f.PickRandom(sale.Items).ProductId)
            .RuleFor(i => i.Quantity, f => f.PickRandom(sale.Items).Quantity)
            .RuleFor(i => i.UnitPrice, f => f.PickRandom(sale.Items).UnitPrice)
            .RuleFor(i => i.Discount, f => f.PickRandom(sale.Items).Discount)
            .RuleFor(i => i.Total, f => f.PickRandom(sale.Items).Total)
            .RuleFor(i => i.SaleId, f => sale.Id)
            .RuleFor(i => i.IsCancelled, f => false);

        var createSaleResponseFaker = new Faker<CreateSaleCommandResponse>()
            .RuleFor(s => s.Id, f => sale.Id)
            .RuleFor(s => s.SaleNumber, f => sale.SaleNumber)
            .RuleFor(s => s.TotalAmount, f => sale.TotalAmount)
            .RuleFor(s => s.Items, f => saleItemResponseFaker.Generate(sale.Items.Count));

        return createSaleResponseFaker.Generate();
    }

}
