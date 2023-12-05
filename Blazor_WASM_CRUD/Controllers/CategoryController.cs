using Blazor_WASM_CRUD.API.Data;
using Blazor_WASM_CRUD.API.Extensions;
using Blazor_WASM_CRUD.API.Models;
using Blazor_WASM_CRUD.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blazor_WASM_CRUD.API.Controllers
{

    [ApiController]
    public class CategoryController : Controller
    {
        [HttpGet("v1/categories")]
        public List<Category> GetList([FromServices] AppDbContext context)
        {
            return context.Categories.ToList();
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> Get([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var registro = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(registro);
        }

        [HttpGet("v1/categoria-com-produtos/{idCategory:int}")]
        public async Task<IActionResult> ByCategory([FromServices] AppDbContext context, [FromRoute] int idCategory)
        {
            var registros = await context.Categories.Where(x => x.Id == idCategory).Include(x => x.Products)
                 .AsNoTracking().ToListAsync() ;
            return Ok(registros);
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> Post([FromServices] AppDbContext context, CategoryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<CategoryViewModel>(ModelState.GetErros()));
                var CategoriaJaExiste = context.Categories.Count(x => x.Title == model.Title) > 0;
                if (CategoriaJaExiste)
                    return NotFound(new ResultViewModel<Category>("Já existe uma categiria com esse nome."));

                var categoria = new Category();
                categoria.Title = model.Title;

                await context.AddAsync(categoria);
                await context.SaveChangesAsync();
                //return Created($"v1/categories/{categoria.Id}", new ResultViewModel<Category>(categoria));
                return Json(categoria);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("POST_CATEGORY - Não foi possivel adicionar um novo registro."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("POST_CATEGORY_SERVER Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id}")]
        public async Task<IActionResult> Put([FromServices] AppDbContext context, [FromBody] CategoryViewModel model,
     [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<CategoryViewModel>(ModelState.GetErros()));

            try
            {
                var registro = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (registro == null)
                    return NotFound(new ResultViewModel<Product>("Registro não encontrado"));

                var CategoriaJaExiste = context.Categories.Count(x => x.Title == model.Title) > 0;
                if (CategoriaJaExiste)
                    return NotFound(new ResultViewModel<Category>("Já existe uma categiria com esse nome."));


                registro.Title = model.Title;
                context.Categories.Update(registro);
                await context.SaveChangesAsync();

                //return Created($"v1/categories/{registro.Id}", new ResultViewModel<Category>(registro));
                return Json(registro);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("PUT_CATEGORY - Não foi possivel adicionar um novo registro."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("PUT_CATEGORY_SERVER Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> Delete([FromServices] AppDbContext context,
   [FromRoute] int id)
        {
            var registro = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (registro == null)
                return NotFound(new ResultViewModel<Category>("Registro não encontrado"));

            context.Categories.Remove(registro);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(registro));
        }


    }
}
