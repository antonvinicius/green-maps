using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GreenMaps.Models.ViewModels
{
    public class PontoColetaViewModel
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        // [Required(ErrorMessage = "A imagem é obrigatória")]
        // public IFormFile Imagem { get; set; }

        [Required(ErrorMessage = "Selecione um tipo de ponto")]
        public int TipoPontoId { get; set; }

        [Required(ErrorMessage = "Selecione pelo menos um tipo de lixo")]
        public int[] TipoLixoIds { get; set; }
    }
}