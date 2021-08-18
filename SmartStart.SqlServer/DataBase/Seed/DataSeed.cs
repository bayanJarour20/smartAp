using Microsoft.EntityFrameworkCore;
using SmartStart.Model.Business;
using SmartStart.Model.General;
using SmartStart.Model.Main;
using SmartStart.SharedKernel.Enums;
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

        public static async Task FixPackages(SmartStartDbContext context)
        {
            var autoPackages = context.Packages.Include(package => package.PackageSubjectFaculties)
                                               .ThenInclude(subjectPackage => subjectPackage.SubjectFaculty)
                                               .ThenInclude(subjectFaculty => subjectFaculty.Subject)
                                               .ThenInclude(subject => subject.SubjectFaculties)
                                               .Where(x => x.Type == PackageTypes.Auto)
                                               .ToList();

            foreach (var autoPackage in autoPackages)
            {
                //فحص إذا كان يوجد مواد جديدة بالسنة التي اختارها الطالب وغير موجودة ضمن الحزمة
                if(autoPackage.Name.Contains("شاملة"))
                {
                    //فحص السنة التي اختارها الطالب 
                    var year = autoPackage.PackageSubjectFaculties.GroupBy(p => p.SubjectFaculty.Year).OrderByDescending(p => p.Count());
                    if(year.Any())
                    {
                        int ActualCount = context.SubjectFaculties.Count(w => w.FacultyId == year.FirstOrDefault().FirstOrDefault().SubjectFaculty.FacultyId && w.Year == year.FirstOrDefault().Key);
                        int CountInPackage = year.FirstOrDefault().Count();

                        if (ActualCount > CountInPackage)
                        {
                            var InSubjectFaculty = year.FirstOrDefault().Select(w => w.SubjectFacultyId).ToList();

                            var subjectFacultiesToAdd = context.SubjectFaculties.Where(s =>
                                s.FacultyId == year.FirstOrDefault().FirstOrDefault().SubjectFaculty.FacultyId && s.Year == year.FirstOrDefault().Key && !InSubjectFaculty.Contains(s.Id)).ToList();
                            List<PackageSubjectFaculty> packageSubjects = new List<PackageSubjectFaculty>();

                            foreach (var subjectFaculty in subjectFacultiesToAdd)
                            {
                                packageSubjects.Add(new PackageSubjectFaculty()
                                {
                                    SubjectFacultyId = subjectFaculty.Id,
                                    PackageId = autoPackage.Id
                                });
                            }
                            await context.PackageSubjectFaculties.AddRangeAsync(packageSubjects);
                            await context.SaveChangesAsync();
                        }
                    }

                    //تجميع الدورات حسب المواد لفحص عدد الدورات الحقيقي وعدد الدورات الحالية في الحزمة
                    foreach (var autoPackagePackageSubject in autoPackage.PackageSubjectFaculties.GroupBy(w => w.SubjectFaculty.SubjectId))
                    {
                        int ActualCount = context.SubjectFaculties.Count(w => w.SubjectId == autoPackagePackageSubject.Key);
                        int CountInSubject = autoPackage.PackageSubjectFaculties.Count(w => w.SubjectFaculty.SubjectId == autoPackagePackageSubject.Key);

                        //مقارنة بين العدد الفعلي والموجود
                        if (ActualCount > CountInSubject)
                        {
                            var InSubject = autoPackage.PackageSubjectFaculties.Where(w => w.SubjectFaculty.SubjectId == autoPackagePackageSubject.Key)
                                                                               .Select(w => w.SubjectFaculty.Id).ToList();

                            var subjectsToAdd = context.SubjectFaculties.Where(w =>
                                !InSubject.Contains(w.Id) && w.SubjectId == autoPackagePackageSubject.Key).ToList();
                            List<PackageSubjectFaculty> packageSubjects = new List<PackageSubjectFaculty>();

                            foreach (var subject in packageSubjects)
                            {
                                packageSubjects.Add(new PackageSubjectFaculty()
                                {
                                    SubjectFacultyId = subject.Id,
                                    PackageId = autoPackage.Id
                                });
                            }
                            await context.PackageSubjectFaculties.AddRangeAsync(packageSubjects);
                            await context.SaveChangesAsync();
                        }

                    }
                }
            }


        }
    }
}
