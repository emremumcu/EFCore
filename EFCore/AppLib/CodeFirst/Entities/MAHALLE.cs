using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.AppLib.CodeFirst.Entities
{
    public class MAHALLE
    {
        /// [Key] attribute is required since the key property name does not end with ...Id or ...ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MahalleKod { get; set; }

        public string Mahalle { get; set; }

        /// This attribute refers to the following property and tells the EF-Core that  it is a Foreign Key
        /// This prevents duplicate creation of the columns
        [ForeignKey("SbbKod")]
        public SBB SemtBucakBelde { get; set; } /// reference navigation property

        public string SbbKod { get; set; }
    }
}
