using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] 
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPut]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            return Ok(await _repository.UpdateBasket(shoppingCart));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasketByUserName(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }

        // Get basket by UserName
        [HttpGet("{UserName:length(24)}", Name = "GetBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasketByUserName(string username)
        {
            var basket = await _repository.GetBasket(username);
            if (basket == null)
            {
                _logger.LogError($"Basket with username: {username}, not found.");
                return NotFound();
            }
            return Ok(basket);
        } 

    }
}
