using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestTelematikaService.Models
{
    public class CassetteModel
    {
        public int Id { get; set; }

        [Display(Name = "Количество")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите количество кассет")]
        [Range(0,100,ErrorMessage = "Значение для количества должно быть между {1} и {2}.")]
        public int Quantity { get; set; }

        [Display(Name = "Исправный")]
        public bool Serviceable { get; set; }

        [Display(Name = "Номинал банкноты")]
        [ForeignKey("NominalId")]
        public virtual NominalModel NominalValue { get; set; }

    }
}
