using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAPSONAR
{
    public class UauComposicao
    {
        public Guid Id { get; set; }
        public string CodCentro { get; set; }

        public string CodComp { get; set; }

        public string Description { get; set; }

        public IEnumerable<UauInsumo> Insumos { get; set; }
    }
}
