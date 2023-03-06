using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace buy_sale.host.Controllers
{
    [ApiController]
    [Route("api/salespoints")]
    public class SalesPointsController : Controller
    {
        private IRepository<SalesPoint> _repository;

        public SalesPointsController(IRepository<SalesPoint> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Список всех точек продажи
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IEnumerable<SalesPoint>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Получение точки продажи по индексу
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<SalesPoint>> Get(int id)
        {
            var product = await _repository.SingleOrDefaultAsync(id);

            if (product is null) return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Добавление точки продажи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<SalesPoint>> Add(string name, List<PostProvidedProduct> postProvidedProducts)
        {
            var providedProducts = new List<ProvidedProducts>();

            foreach(var pp in postProvidedProducts)
            {
                providedProducts.Add(new ProvidedProducts
                {
                    ProductId = pp.ProductId,
                    ProductQuantity = pp.ProductQuantity
                });
            }
            var salesPoint = new SalesPoint
            {
                Name = name,
                ProvidedProducts = providedProducts
            };

            var stateAdd =  await _repository.Add(salesPoint);
            if (!stateAdd) return BadRequest();

            var result = await _repository.SaveChangesAsync();

            if (result < 1) return BadRequest();

            return salesPoint;
        }

        /// <summary>
        /// Обновление точки продажи
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<bool>> Update(SalesPoint product)
        {
            var changedSalesPoint = await _repository.Update(product);
            var result = await _repository.SaveChangesAsync();

            if (!changedSalesPoint || result < 1) return false;

            return true;
        }

        /// <summary>
        /// Удаление точки продажи
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var deletedSalesPoint = await _repository.Delete(id);
            var result = await _repository.SaveChangesAsync();

            if (!deletedSalesPoint || result < 1) return false;

            return true;
        }
    }
}
