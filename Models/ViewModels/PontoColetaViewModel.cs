using Microsoft.AspNetCore.Http;

namespace GreenMaps.Models.ViewModels
{
    public class PontoColetaViewModel
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Nome { get; set; }
        public IFormFile Imagem { get; set; }

        public int TipoPontoId { get; set; }
        public int[] TipoLixoIds { get; set; }
    }
}