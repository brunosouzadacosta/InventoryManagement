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
    public class ProductController : Controller
    {
        #region Dependency Injection
        private readonly IProductApplication _productApplication;
        public ProductController(IProductApplication IProductApplication)
        {
            _productApplication = IProductApplication;
        }
        #endregion

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var retorno = new vmResult();
            try
            {
                retorno.Data = await _productApplication.GetAllAsync();
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
        public async Task<IActionResult> GetById(int productId)
        {
            var retorno = new vmResult();
            try
            {
                retorno.Data = await _productApplication.GetByIdAsync(productId);
                if (retorno.Data == null)
                {
                    retorno.FriendlyErrorMessage = "Product not found";
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
        public async Task<IActionResult> Insert([FromBody] vmProduct product)
        {
            var retorno = new vmResult();
            try
            {
                await _productApplication.AddAsync(product);
                retorno.Data = product;
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                retorno.FriendlyErrorMessage = "Unexpected error";
                retorno.StackTrace = $"{ex.Message}\nInner Exception: {inner}\n{ex.StackTrace}";
                return BadRequest(retorno);
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(int productId, [FromBody] vmProduct product)
        {
            var retorno = new vmResult();
            try
            {
                var result = await _productApplication.UpdateAsync(productId, product);
                if (!result)
                {
                    retorno.FriendlyErrorMessage = "Product not found to update";
                    return NotFound(retorno);
                }

                retorno.Data = product;
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.FriendlyErrorMessage = "Unexpected erro";
                retorno.StackTrace = ex.Message + "\n" + ex.StackTrace;
                return BadRequest(retorno);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int productId)
        {
            var retorno = new vmResult();
            try
            {
                var result = await _productApplication.DeleteAsync(productId);
                if (!result)
                {
                    retorno.FriendlyErrorMessage = "Product not found to delete";
                    return NotFound(retorno);
                }

                retorno.Data = true;
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
