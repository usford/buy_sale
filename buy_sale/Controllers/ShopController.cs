using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using buy_sale.database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace buy_sale.host.Controllers
{
    [ApiController]
    [Route("api/shop")]
    public class ShopController : Controller
    {
        IRepository<SalesPoint> _salesPointRepository;
        IRepository<Sale> _salesRepository;
        ILogger<ShopController> _logger;

        public ShopController(
            IRepository<SalesPoint> salesPointRepository,
            IRepository<Sale> salesRepository,
            ILogger<ShopController> logger
            )
        {
            _salesPointRepository = salesPointRepository ?? throw new ArgumentNullException(nameof(salesPointRepository));
            _salesRepository = salesRepository ?? throw new ArgumentNullException(nameof(salesRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.LogCritical($"Тест логирования");
        }

        /// <summary>
        /// Покупка товара
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("sale")]
        public async Task<ActionResult<Sale>> Sale(
            int salePointId,
            int? buyerId,
            List<PostSalesData> postSalesData
            )
        {
            var salePoint = await _salesPointRepository.SingleOrDefaultAsync(salePointId);
            if (salePoint is null) return BadRequest();

            var salesData = new List<SalesData>();
            var totalAmount = 0m;

            foreach (var saleData in postSalesData)
            {
                var product = salePoint.ProvidedProducts.Find(x => x.ProductId == saleData.ProductId);
                if (product is null) return BadRequest();

                if (product.ProductQuantity < saleData.ProductQuantity) return BadRequest();

                var productAmount = product.Product.Price * saleData.ProductQuantity;

                salesData.Add(new SalesData
                {
                    ProductId = product.ProductId,
                    ProductQuantity = saleData.ProductQuantity,
                    ProductAmount = productAmount
                });

                totalAmount += productAmount;
                product.ProductQuantity -= saleData.ProductQuantity;

                var updated = await _salesPointRepository.Update(salePoint);
                if (!updated) return BadRequest();
            }

            var sale = new Sale
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                SalesPointId = salePointId,
                BuyerId = buyerId,
                SalesData = salesData,
                TotalAmount = totalAmount
            };

            var added = await _salesRepository.Add(sale);

            if (!added) return BadRequest();

            var result = await _salesRepository.SaveChangesAsync();

            if (result < 1) return BadRequest();

            return Ok(sale);
        }
    }
}
