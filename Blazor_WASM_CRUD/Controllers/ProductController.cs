using Blazor_WASM_CRUD.API.Data;
using Blazor_WASM_CRUD.API.Models;
using Blazor_WASM_CRUD.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Blazor_WASM_CRUD.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Blazor_WASM_CRUD.API.Controllers
{

    [ApiController]
    public class ProductController : Controller
    {


        [HttpGet("v1/produtos")]
        public List<Product> GetList([FromServices] AppDbContext context)
        {
            return context.Products.ToList();
        }

        [HttpGet("v1/produtos/{id:int}")]
        public async Task<IActionResult> Get([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var registro = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(registro);
        }

        [HttpGet("v1/por-categoria/{idCategory:int}")]
        public async Task<IActionResult> ByCategory([FromServices] AppDbContext context, [FromRoute] int idCategory)
        {
            var registros = await context.Products.Where(x => x.CategoryId == idCategory)
                 .Select(x => new
                 {
                     Codigo = x.Id,
                     Title = x.Title,
                     Category = x.Cate.Title,
                 }).AsNoTracking().ToListAsync();
            var total = await context.Products.Where(x => x.CategoryId == idCategory).CountAsync();

            var result = new
            {
                Total = total,
                Produtos = registros
            };

            return Ok(result);
        }


        [HttpPost("v1/produtos")]
        public async Task<IActionResult> Post([FromServices] AppDbContext context, ProductViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Product>(ModelState.GetErros()));
                var JaProdutoJaExiste = context.Products.Count(x => x.Title == model.Title && x.CategoryId == model.CategoryId) > 0;
                if (JaProdutoJaExiste)
                    return NotFound(new ResultViewModel<Product>("Já existe um produto com esse nome."));

                var produto = new Product();
                produto.Title = model.Title;
                produto.Price = model.Price;
                produto.CategoryId = model.CategoryId;


                await context.AddAsync(produto);
                await context.SaveChangesAsync();
                return Created($"v1/produtos/{produto.Id}", new ResultViewModel<Product>(produto));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Product>("POST_PRODUCT - Não foi possivel adicionar um novo registro."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Product>("POST_PRODUCT_SERVER Falha interna no servidor"));
            }
        }
        [HttpPost("v1/produtos2")]
        public async Task<IActionResult> Post2([FromServices] AppDbContext context, Product model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResultViewModel<Product>(ModelState.GetErros()));
                var JaProdutoJaExiste = context.Products.Count(x => x.Title == model.Title && x.CategoryId == model.CategoryId) > 0;
                if (JaProdutoJaExiste)
                    return NotFound(new ResultViewModel<Product>("Já existe um produto com esse nome."));

                var produto = new Product();
                produto.Title = model.Title;
                produto.Price = model.Price;
                produto.CategoryId = model.CategoryId;


                await context.AddAsync(produto);
                await context.SaveChangesAsync();
                return Created($"v1/produtos/{produto.Id}", new ResultViewModel<Product>(produto));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Product>("POST_PRODUCT - Não foi possivel adicionar um novo registro."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Product>("POST_PRODUCT_SERVER Falha interna no servidor"));
            }
        }




        [HttpPut("v1/produtos/{id}")]
        public async Task<IActionResult> Put([FromServices] AppDbContext context, [FromBody] ProductViewModel model,
      [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ProductViewModel>(ModelState.GetErros()));

            try
            {
                var registro = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (registro == null)
                    return NotFound(new ResultViewModel<Product>("Registro não encontrado"));

                var JaProdutoJaExiste = context.Products.Count(x => x.Title == model.Title && x.CategoryId == model.CategoryId) > 0;
                if (JaProdutoJaExiste)
                    return NotFound(new ResultViewModel<Product>("Já existe um produto com esse nome."));


                registro.Title = model.Title;
                registro.Price = model.Price;
                registro.CategoryId = model.CategoryId;
                context.Products.Update(registro);
                await context.SaveChangesAsync();

                return Created($"v1/ibge/{registro.Id}", new ResultViewModel<Product>(registro));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Product>("PUT_PRODUCT - Não foi possivel editar um novo registro"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Product>("POST_PRODUCT_SERVER - no servidor"));
            }
        }


   
        [HttpDelete("v1/produtos/{id:int}")]
        public async Task<IActionResult> Delete([FromServices] AppDbContext context,
        [FromRoute] int id)
        {
            var registro = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (registro == null)
                return NotFound(new ResultViewModel<Product>("Registro não encontrado"));

            context.Products.Remove(registro);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Product>(registro));
        }



    }



}
