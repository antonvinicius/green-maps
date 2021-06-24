using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenMaps.Models;
using Microsoft.AspNetCore.Identity;

namespace GreenMaps.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the Usuario class
    public class Usuario : IdentityUser
    {
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public ICollection<PontoColeta> PontoColeta { get; set; }
    }
}
