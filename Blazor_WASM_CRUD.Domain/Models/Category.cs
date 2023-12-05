using System.ComponentModel.DataAnnotations;
namespace Blazor_WASM_CRUD.Domain.Models

{
    public class Category
    {

        public Category()
        {

        }
            public Category(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; set; }


        public string Title { get; set; } = string.Empty;

        public List<Product> Products { get; set; } = new();

    }
}
