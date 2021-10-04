using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.AppLib.CodeFirst.Entities
{
    public class IL
    {
        /// [Key] attribute is required since the key property name does not end with ...Id or ...ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IlKodu { get; set; }

        public string IlAdi { get; set; }
    }
}
