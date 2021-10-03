using EFCore.AppLib.CodeFirst.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.ViewModels
{
    public class AdresViewModel
    {
        public List<IL> Iller { get; set; }
        public List<ILCE> Ilceler { get; set; }
        public List<MAHALLE> Mahalleler { get; set; }
        public List<SBB> Sbbler { get; set; }
    }
}
