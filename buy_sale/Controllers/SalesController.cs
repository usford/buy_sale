using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.AspNetCore.Mvc;

namespace buy_sale.host.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesController : Controller
    {
        private IRepository<Sale> _repositorySales;
        private IRepository<Product> _repositoryProducts;

        public SalesController(IRepository<Sale> repositorySales, IRepository<Product> repositoryProduct)
        {
            _repositorySales = repositorySales ?? throw new ArgumentNullException(nameof(repositorySales));
            _repositoryProducts = repositoryProduct ?? throw new ArgumentNullException(nameof(repositoryProduct));
        }

        /// <summary>
        /// Список всех продаж
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IEnumerable<Sale>> GetAll()
        {
            return await _repositorySales.GetAllAsync();
        }

        /// <summary>
        /// Получение продажи по id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<Sale>> Get(int id)
        {
            var sale = await _repositorySales.SingleOrDefaultAsync(id);

            if (sale is null) return NotFound();

            return Ok(sale);
        }

        /// <summary>
        /// Добавление продажи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<bool>> Add(
            DateOnly date,
            TimeOnly time,
            int salesPointId,
            int buyerId,
            List<PostSalesData> postSalesData
            )
        {
            if (date == DateOnly.MinValue) date = DateOnly.FromDateTime(DateTime.Now);
            if (time == TimeOnly.MinValue) time = TimeOnly.FromDateTime(DateTime.Now);

            var salesData = new List<SalesData>();
            var totalAmount = 0m;

            foreach (var saleData in postSalesData)
            {
                var product = await _repositoryProducts.SingleOrDefaultAsync(saleData.ProductId);

                if (product is null) return BadRequest();

                var productAmount = product.Price * saleData.ProductQuantity;

                salesData.Add(new SalesData
                {
                    ProductId = saleData.ProductId,
                    ProductQuantity = saleData.ProductQuantity,
                    ProductAmount = productAmount
                });

                totalAmount += productAmount;
            }
            var sale = new Sale
            {
                Date = date,
                Time = time,
                SalesPointId = salesPointId,
                BuyerId = buyerId,
                SalesData = salesData,
                TotalAmount = totalAmount
            };

            var added = await _repositorySales.Add(sale);

            if (!added) return BadRequest();

            var result = await _repositorySales.SaveChangesAsync();

            if (result < 1) return BadRequest();

            return Ok(true);
        }
        /// <summary>
        /// Обновление продажи
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<bool>> Update(Sale sale)
        {
            var changedBuyer = await _repositorySales.Update(sale);
            var result = await _repositorySales.SaveChangesAsync();

            if (!changedBuyer || result < 1) return false;

            return true;
        }

        /// <summary>
        /// Удаление продажи
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            //TODO проблема с удалением из-за связи с Sale
            var deletedBuyer = await _repositorySales.Delete(id);
            var result = await _repositorySales.SaveChangesAsync();

            if (!deletedBuyer || result < 1) return false;

            return true;
        }
    }
}
