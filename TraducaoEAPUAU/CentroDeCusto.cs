using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraducaoEAPUAU
{
    public class CentroDeCusto
    {
        public string Cod { get; set; }
        public string Description { get; set; }

        public IEnumerable<UauComposicao> Composicoes { get; set; }
    }
}
