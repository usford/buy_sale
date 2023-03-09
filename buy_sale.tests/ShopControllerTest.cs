using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using buy_sale.host;
using buy_sale.host.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace buy_sale.tests
{
    public class ShopControllerTest
    {
        HttpClient _httpClient;
        WebApplicationFactory<Program> _webAppFactory;
        public ShopControllerTest()
        {
            _webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = _webAppFactory.CreateDefaultClient();
        }

        /// <summary>
        /// Успешная покупка
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async void ShopSale()
        {
            var salePointId = 1;
            var buyerId = 1;

            var listSalesData = new List<PostSalesData> {
                new PostSalesData { ProductId = 1, ProductQuantity = 1 }
            };

            using var scope = _webAppFactory.Services.CreateScope();
            var repositorySalesPoint = scope.ServiceProvider.GetRequiredService<IRepository<SalesPoint>>();
            var repositorySales = scope.ServiceProvider.GetRequiredService<IRepository<Sale>>();
            var salesPoint = await repositorySalesPoint.GetAllAsync();

            var oldSalesPointQuantity = salesPoint.SingleOrDefault(sp => sp.Id == 1)
                .ProvidedProducts.SingleOrDefault(pp => pp.ProductId == 1).ProductQuantity;

            var json = JsonSerializer.Serialize(listSalesData);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://localhost:7076/api/shop/sale?salePointId={salePointId}&buyerId={buyerId}"),
                Method = HttpMethod.Post,
                Content = new StringContent(json)
                {
                    Headers =
                    {
                        ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
                    }
                }
            };

            var response = await _httpClient.SendAsync(request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new DateOnlyConverterJson());
            options.Converters.Add(new TimeOnlyConverterJson());        

            var sale = await response.Content.ReadFromJsonAsync<Sale>(options);

            Assert.Single(sale.SalesData);

            salesPoint = await repositorySalesPoint.GetAllAsync();
            var newSalesPointQuantity = salesPoint.SingleOrDefault(sp => sp.Id == 1)
                .ProvidedProducts.SingleOrDefault(pp => pp.ProductId == 1).ProductQuantity;

            var sales = await repositorySales.GetAllAsync();

            Assert.Equal(2, sales.Count());
            Assert.Equal(oldSalesPointQuantity, newSalesPointQuantity + 1);
        }
    } 
}