using InventoryManagement.Application.Interfaces;
using InventoryManagement.Application.Services;
using InventoryManagement.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementController : Controller
    {
        #region Dependency Injection
        private readonly IStockMovementApplication _stockMovementApplication;
        public StockMovementController(IStockMovementApplication IStockMovementApplication)
        {
            _stockMovementApplication = IStockMovementApplication;
        }
        #endregion

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var retorno = new vmResult();
            try
            {
                retorno.Data = await _stockMovementApplication.GetAllAsync();
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.FriendlyErrorMessage = "Unexpected erro";
                retorno.StackTrace = ex.Message + "\n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int stockMovementId)
        {
            var retorno = new vmResult();
            try
            {
                retorno.Data = await _stockMovementApplication.GetByIdAsync(stockMovementId);
                if (retorno.Data == null)
                {
                    retorno.FriendlyErrorMessage = "Stock Movement not found";
                    return NotFound(retorno);
                }
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.FriendlyErrorMessage = "Unexpected erro";
                retorno.StackTrace = ex.Message + "\n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] vmStockMovement stockMovement)
        {
            var retorno = new vmResult();
            try
            {
                await _stockMovementApplication.AddAsync(stockMovement);
                retorno.Data = stockMovement;
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.FriendlyErrorMessage = "Unexpected erro";
                retorno.StackTrace = ex.Message + "\n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }
    }
}
