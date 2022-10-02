using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTelematikaService.Models
{
    public class NominalModel
    {
        public int Id { get; set; }
        public int NominalValue { get; set; }
        public virtual ICollection<CassetteModel> Cassette { get; set; }
    }
}
