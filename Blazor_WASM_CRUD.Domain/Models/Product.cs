using System.ComponentModel.DataAnnotations;
namespace Blazor_WASM_CRUD.Domain.Models
{
    public class Product
    {

        public int Id { get; set; }


        public string Title { get; set; } = string.Empty;


        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category Cate { get; set; } = null!;
    }
}
