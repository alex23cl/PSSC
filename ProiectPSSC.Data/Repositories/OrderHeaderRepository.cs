﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;
using ProiectPSSC.Domain.Models;
using ProiectPSSC.Domain.Repositories;
using LanguageExt;
using ProiectPSSC.Data.Models;

namespace ProiectPSSC.Data.Repositories
{
    /*
     CREATE TABLE [dbo].[OrderHeader](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[TotalPrice] [decimal] NOT NULL,
	[PaymentOption] [varchar](20) NOT NULL,
	[CardDetails] [varchar](50),
     */

    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly OrderContext dbContext;

        public TryAsync<List<CalculatedOrderTotalPrice>> TryGetExistingClientOrders() => async () => (await (
            from p in dbContext.Products
            from c in dbContext.Clients
            from ol in dbContext.OrderLines
            join oh in dbContext.OrderHeaders on c.ClientId equals oh.ClientId
            select new { c.Email, p.Price, oh.TotalPrice, oh.ClientId })
                .AsNoTracking()
                .ToListAsync())
                .Select(result => new CalculatedOrderTotalPrice(
                    clientEmail: new(result.Email),
                totalPrice: new(result.TotalPrice))
        {
            ClientId = result.ClientId
        })
        .ToList();


        public TryAsync<Unit> TrySaveOrders(OrderProducts.PlacedOrderProducts order) => async () =>
        {
            var clients = (await dbContext.Clients.ToListAsync()).ToLookup(client => client.Email);
            var newOrderProducts = order.ProductList
            .Where(p => p.IsUpdated && p.ProductId == 0)
            .Select(p => new OrderHeaderDto()
            {

            });

            var updatedOrderProducts = order.ProductList.Where(p => p.IsUpdated && p.ProductId == 0)
                .Select(p => new OrderHeaderDto()
                {

                });

            dbContext.AddRange(newOrderProducts);
            foreach (var entity in updatedOrderProducts)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}
