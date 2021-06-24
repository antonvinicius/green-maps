using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenMaps.Models
{
    public class TipoPonto
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        public ICollection<PontoColeta> PontoColetas { get; set; }
    }
}