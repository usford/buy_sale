using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.AspNetCore.Mvc;

namespace buy_sale.host.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private IRepository<Product> _repository;

        public ProductsController(IRepository<Product> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Список всех продуктов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Получение продукта по индексу
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _repository.SingleOrDefaultAsync(id);

            if (product is null) return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Добавление продукта
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<bool>> Add(string name, decimal price)
        {
            var product = new Product
            {
                Name = name,
                Price = price
            };

            await _repository.Add(product);
            var result = await _repository.SaveChangesAsync();

            if (result < 1) return BadRequest();

            return Ok(true);
        }

        /// <summary>
        /// Обновление продукта
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<bool>> Update(Product product)
        {
            var changedProduct = await _repository.Update(product);
            var result = await _repository.SaveChangesAsync();

            if (!changedProduct || result < 1) return false;

            return true;
        }

        /// <summary>
        /// Удаление продукта
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var deletedProduct = await _repository.Delete(id);
            var result = await _repository.SaveChangesAsync();

            if (!deletedProduct || result < 1) return false;

            return true;
        }
    }
}
