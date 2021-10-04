namespace EFCore.AppLib
{
    using EFCore.AppLib.CodeFirst;
    using EFCore.AppLib.CodeFirst.Entities;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using System;
    using System.Linq;

    public class DataGenerator
    {
        public static void Generate(IHost host)
        {
            IServiceScope scope = host.Services.CreateScope();

            IServiceProvider services = scope.ServiceProvider;
            
            IWebHostEnvironment environment = services.GetRequiredService<IWebHostEnvironment>();

            if(environment.IsDevelopment())
            {
                CodeFirstDbContext context = services.GetRequiredService<CodeFirstDbContext>();            

                if (context.Database.EnsureCreated()) 
                    context.Database.Migrate();            
            
                if (context.Iller.Any()) 
                    return;            
                else 
                    PopulateSampleData(environment, context);  
            }
        }

        private static void PopulateSampleData(IWebHostEnvironment environment, CodeFirstDbContext context)
        {
            /// Step1: Read json files as string data
            string ilData = System.IO.File.ReadAllText(System.IO.Path.Combine(environment.WebRootPath, "static", "IL.json"));
            string ilceData = System.IO.File.ReadAllText(System.IO.Path.Combine(environment.WebRootPath, "static", "ILCE.json"));
            string sbbData = System.IO.File.ReadAllText(System.IO.Path.Combine(environment.WebRootPath, "static", "SBB.json"));
            string mahalleData = System.IO.File.ReadAllText(System.IO.Path.Combine(environment.WebRootPath, "static", "MAHALLE.json"));            
            
            /// Step2: Create corresponding anonymous array types for json data
            var ilTemplate = new[] { new { IlKod = "", Il = "" } };
            var ilceTemplate = new[] { new { IlKod = "", Il = "", IlceKod = "", Ilce = "" } };
            var sbbTemplate = new[] { new { IlKod = "", Il = "", IlceKod = "", Ilce = "", SbbKod = "", SemtBucakBelde = "" } };
            var mahalleTemplate = new[] { new { IlKod = "", Il = "", IlceKod = "", Ilce = "", SbbKod = "", SemtBucakBelde = "", MahalleKod = "", Mahalle = "", PostaKod="" } };

            /// Step3: Read json data (array type NOT object type) into anonymous array types
            var illerFromJson = JsonConvert.DeserializeAnonymousType(ilData, ilTemplate);            
            var ilcelerFromJson = JsonConvert.DeserializeAnonymousType(ilceData, ilceTemplate);            
            var sbbFromJson = JsonConvert.DeserializeAnonymousType(sbbData, sbbTemplate);            
            var mahalleFromJson = JsonConvert.DeserializeAnonymousType(mahalleData, mahalleTemplate);            

            /// Step4: Transfer data to database
            foreach (var i in illerFromJson)
                context.Iller.Add(new IL() { IlKodu = i.IlKod, IlAdi = i.Il });
            context.SaveChanges();

            foreach (var i in ilcelerFromJson)
                context.Ilceler.Add(new ILCE() { IlceKodu = i.IlceKod, IlceAdi = i.Ilce, IlKodu = i.IlKod });
            context.SaveChanges();

            foreach (var i in sbbFromJson)
                context.SemtBucakBeldeler.Add(new SBB() { SbbKodu=i.SbbKod, SemtBucakBeldeAdi = i.SemtBucakBelde, IlceKodu = i.IlceKod  });
            context.SaveChanges();

            foreach (var i in mahalleFromJson)
                context.Mahalleler.Add(new MAHALLE() { MahalleKodu = i.MahalleKod, MahalleAdi = i.Mahalle, SbbKodu = i.SbbKod });
            context.SaveChanges();
        }
    }
}
