using buy_sale.database.Interfaces;
using buy_sale.database.Models;
using Microsoft.AspNetCore.Mvc;

namespace buy_sale.host.Controllers
{
    [ApiController]
    [Route("api/buyers")]
    public class BuyersController : Controller
    {
        private IRepository<Buyer> _repository;

        public BuyersController(IRepository<Buyer> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Список всех покупателей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all")]
        public async Task<IEnumerable<Buyer>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Получение покупателя по индексу
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<Buyer>> Get(int id)
        {
            var buyer = await _repository.SingleOrDefaultAsync(id);

            if (buyer is null) return NotFound();

            return Ok(buyer);
        }

        /// <summary>
        /// Добавление покупателя
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<bool>> Add(string name)
        {
            var buyer = new Buyer
            {
                Name = name
            };

            await _repository.Add(buyer);
            var result = await _repository.SaveChangesAsync();

            if (result < 1) return BadRequest();

            return Ok(true);
        }

        /// <summary>
        /// Обновление покупателя
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<bool>> Update(int id, string name)
        {
            var buyer = new Buyer
            {
                Id = id,
                Name = name
            };

            var changedBuyer = await _repository.Update(buyer);
            var result = await _repository.SaveChangesAsync();

            if (!changedBuyer || result < 1) return false;

            return true;
        }

        /// <summary>
        /// Удаление покупателя
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            //TODO проблема с удалением из-за связи с Sale
            var deletedBuyer = await _repository.Delete(id);
            var result = await _repository.SaveChangesAsync();

            if (!deletedBuyer || result < 1) return false;

            return true;
        }
    }
}
