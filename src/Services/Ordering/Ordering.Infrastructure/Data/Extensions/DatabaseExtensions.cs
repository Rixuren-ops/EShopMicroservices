using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(context);
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
            var x = await SeedCustomerAsync(context);
            var y = await SeedProductAsync(context);
            await SeedOrdersWithItemsAsync(context, x, y);
        }



        private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context, IEnumerable<Customer> customers, IEnumerable<Product> products)
        {
            if (!await context.Orders.AnyAsync())
            {
                
                await context.Orders.AddRangeAsync(InitialData.OrdersWithItems(customers.ToList(), products.ToList()));
                await context.SaveChangesAsync();
            };
        }

        private static async Task<IEnumerable<Product>> SeedProductAsync(ApplicationDbContext context)
        {
            var products = InitialData.Products;

            if (!await context.Products.AnyAsync())
            {

                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            };

            return products;
        }

        private static async Task<IEnumerable<Customer>> SeedCustomerAsync(ApplicationDbContext context)
        {
            var customers = InitialData.Customers;

            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            };

            return customers;
        }
    }
}
