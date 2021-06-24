using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenMaps.Models
{
    public class PontoColeta
    {
        public int Id { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Imagem { get; set; }
        public TipoPonto TipoPonto { get; set; }
        public ICollection<TipoLixo> TipoLixos { get; set; }
    }
}