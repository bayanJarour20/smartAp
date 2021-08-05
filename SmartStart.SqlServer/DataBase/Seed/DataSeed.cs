using SmartStart.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartStart.SqlServer.DataBase.Seed
{
    public class DataSeed
    {

        public static async System.Threading.Tasks.Task InitializeAsync(IServiceProvider services)
        {
            var context = (SmartStartDbContext)services.GetService(typeof(SmartStartDbContext));
            await CitySeed(context);

            //await FolderSeed(context);
        }

        private static async System.Threading.Tasks.Task CitySeed(SmartStartDbContext context)
        {

            if(! context.Cities.Where(e => e.DateDeleted == null).Any())
            {
                List<City> citesModel = new List<City>()
                {
                    new City { Name = "درعا"},
                    new City { Name = "دير الزور"},
                    new City { Name = "حلب"},
                    new City { Name = "حماة"},
                    new City { Name = "الحسكة"},
                    new City { Name = "حمص"},
                    new City { Name = "إدلب"},
                    new City { Name = "القنيطرة"},
                    new City { Name = "اللاذقية"},
                    new City { Name = "الرقة"},
                    new City { Name = "ريف دمشق"},
                    new City { Name = "السويداء"},
                    new City { Name = "دمشق"},
                    new City { Name = "طرطوس"},
                };

                context.AddRange(citesModel);
                await context.SaveChangesAsync();
            }
            

        }
    }
}
