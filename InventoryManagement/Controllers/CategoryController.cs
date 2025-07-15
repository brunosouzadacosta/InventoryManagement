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
    public class CategoryController : Controller
    {
        #region Dependency Injection
        private readonly ICategoryApplication _categoryApplication;
        public CategoryController(ICategoryApplication ICategoryApplication) 
        {
            _categoryApplication = ICategoryApplication;
        }
        #endregion

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var retorno = new vmResult();
            try
            {
                retorno.Data = await _categoryApplication.GetAllAsync();
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
        public async Task<IActionResult> GetById(int categoryId)
        {
            var retorno = new vmResult();
            try
            {
                retorno.Data = await _categoryApplication.GetByIdAsync(categoryId);
                if (retorno.Data == null)
                {
                    retorno.FriendlyErrorMessage = "Category not found";
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
        public async Task<IActionResult> Insert([FromBody] vmCategory category)
        {
            var retorno = new vmResult();
            try
            {
                await _categoryApplication.AddAsync(category);
                retorno.Data = category;
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                retorno.FriendlyErrorMessage = "Unexpected error";
                retorno.StackTrace = ex.ToString(); 
                return BadRequest(retorno);
            }
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update(int categoryId, [FromBody] vmCategory category)
        {
            var retorno = new vmResult();
            try
            {
                var result = await _categoryApplication.UpdateAsync(categoryId, category);
                if (!result)
                {
                    retorno.FriendlyErrorMessage = "Category not found to update";
                    return NotFound(retorno);
                }

                retorno.Data = category;
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
        public async Task<IActionResult> Delete(int categoryId)
        {
            var retorno = new vmResult();
            try
            {
                var result = await _categoryApplication.DeleteAsync(categoryId);
                if (!result)
                {
                    retorno.FriendlyErrorMessage = "Category not found to delete";
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
