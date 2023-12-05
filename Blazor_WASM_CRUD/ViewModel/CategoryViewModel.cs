using System.ComponentModel.DataAnnotations;

namespace Blazor_WASM_CRUD.API.ViewModel
{
    public class CategoryViewModel
    {


        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(150, ErrorMessage = "Título deve ter no máximo 150 caracteres")]
        [MinLength(5, ErrorMessage = "Título deve ter no mínimo 5 caracteres")]
        public string Title { get; set; } = string.Empty;



    }

}
