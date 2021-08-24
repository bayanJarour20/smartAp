using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmartStart.Model.Security;
using SmartStart.Model.Business;
using SmartStart.SharedKernel.Security;
using SmartStart.Model.Main;
using SmartStart.Model.General;
using SmartStart.Model.Shared;
using SmartStart.SharedKernel.Enums;

namespace SmartStart.SqlServer.DataBase.Seed
{
    public class SecuritySeedData
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetService<RoleManager<IdentityRole<Guid>>>();
            var userManager = services.GetService<UserManager<AppUser>>();

            var newRole = await CreateNewRoles(roleManager);
            await ClearRoles(roleManager);

            await InsureCreateSuperAdminAsync(userManager, roleManager, newRole);
            await InsureCreateSellerAsync(userManager, roleManager, newRole);
             
            //await InsureCreateUserAsync(userManager, roleManager, newRole);
            await InsureCreateGuidSellerAsync(userManager);
        }

        private static async Task<IEnumerable<string>> CreateNewRoles(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = Enum.GetValues(typeof(SmartStartRoles)).Cast<SmartStartRoles>().Select(a => a.ToString());
            var identityRoles = roleManager.Roles.Select(a => a.Name).ToList();
            var newRoles = roles.Except(identityRoles);// .Where(a => !identityRoles.Contains(a));

            foreach (var @new in newRoles)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>() { Name = @new });
            }

            return newRoles;
        }
        private static async Task<IEnumerable<string>> ClearRoles(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = Enum.GetValues(typeof(SmartStartRoles)).Cast<SmartStartRoles>().Select(a => a.ToString());
            var identityRoles = roleManager.Roles.ToList();

            var clearRoles = identityRoles.Where(x => !roles.Contains(x.Name));

            foreach (var @new in clearRoles)
            {
                await roleManager.DeleteAsync(@new);
            }

            return clearRoles.Select(x => x.Name);
        }
        private static async Task InsureCreateSuperAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IEnumerable<string> newRoles)
        {
            var superAdmin = await userManager.FindByNameAsync("Admin");
            if (superAdmin is null)
            {
                superAdmin = new AppUser()
                {
                    UserName = "Admin",
                    Name = "Admin",
                    Email = "Admin@admin.admin",
                    Type = SharedKernel.Enums.UserTypes.Dashboard,
                };
                var createResult = await userManager.CreateAsync(superAdmin, "1234");
                if (createResult.Succeeded)
                {
                    var identityRoles = roleManager.Roles.Select(a => a.Name).ToList();
                    var roleResult = await userManager.AddToRoleAsync(superAdmin, SmartStartRoles.Admin.ToString());
                    if (roleResult.Succeeded) return;
                    throw new Exception(String.Join("\n", roleResult.Errors.Select(error => error.Description)));
                }
                throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
            }
            var superAdminElkood = await userManager.FindByNameAsync("Elkood");
            if (superAdminElkood is null)
            {
                superAdminElkood = new AppUser()
                {
                    UserName = "Elkood",
                    Name = "Elkood",
                    Email = "Elkood@admin.admin",
                    Type = SharedKernel.Enums.UserTypes.Dashboard,
                };
                var createResult = await userManager.CreateAsync(superAdminElkood, "elkooduser");
                if (createResult.Succeeded)
                {
                    var identityRoles = roleManager.Roles.Select(a => a.Name).ToList();
                    var roleResult = await userManager.AddToRoleAsync(superAdminElkood, SmartStartRoles.Admin.ToString());
                    if (roleResult.Succeeded) return;
                    throw new Exception(String.Join("\n", roleResult.Errors.Select(error => error.Description)));
                }
                throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
            }
        }
        private static async Task InsureCreateSellerAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IEnumerable<string> newRole)
        {
            var seller = await userManager.FindByNameAsync("Seller");
            if (seller is null)
            {
                seller = new AppUser()
                {
                    UserName = "Seller",
                    Name = "Seller",
                    Email = "Seller@Seller.Seller",
                    Type = SharedKernel.Enums.UserTypes.Seller,
                    MoneyLimit = 10000,
                    Rates = new List<Rate>() { new Rate() { DiscountRate = 0.1f } },
                    //Codes = new List<Code>()
                    //{
                    //    new Code()
                    //    {
                    //        DiscountRate = 0,
                    //        Value = 1000,
                    //        Hash = "J3a3aO",
                    //        CodePackages = new List<CodePackage>()
                    //        {
                    //            new CodePackage()
                    //            {
                    //                Package = new Package()
                    //                {
                    //                    Description = "وصف البكج المقدم",
                    //                    Name = "اسم البكج",
                    //                    StartDate = DateTime.Now,
                    //                    EndDate = DateTime.Now.AddDays(15),
                    //                    Price = 1000,
                    //                },
                    //            } 
                    //        }  
                    //    } 
                    //},
                };
                var createResult = await userManager.CreateAsync(seller, "1234");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(seller, SmartStartRoles.Seller.ToString());
                    return;
                }
                throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
            }
        }
        private static async Task InsureCreateUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IEnumerable<string> newRole)
        {
            var user = await userManager.FindByNameAsync("Student@Student.Student");

            if (user is null)
            {
                user = new AppUser()
                {
                    UserName = "Student@Student.Student",
                    Name = "Student",
                    Email = "Student@Student.Student",
                    Type = SharedKernel.Enums.UserTypes.User,
                    Rates = new List<Rate>() 
                    { 
                        new Rate() 
                        { 
                            DiscountRate = 0.1f 
                        } 
                    },
                    Codes = new List<Code>()
                    {
                        new Code()
                        {
                            DiscountRate = 0,
                            Value = 1000,
                            Hash = "rtyk6q",
                            SellerId = (await userManager.FindByNameAsync("Seller")).Id,
                            CodePackages = new List<CodePackage>()
                            {
                                new CodePackage()
                                {
                                    Package = new Package()
                                    {
                                        Description = "وصف البكج المقدم",
                                        Name = "اسم البكج",
                                        StartDate = DateTime.Now,
                                        EndDate = DateTime.Now.AddDays(15),
                                        Price = 1000,
                                        PackageSubjectFaculties = new List<PackageSubjectFaculty>()
                                        {
                                            new PackageSubjectFaculty()
                                            {
                                                Price = 400,
                                                SubjectFaculty = new SubjectFaculty ()
                                                {
                                                    Faculty = new Faculty()
                                                    {
                                                        Name = "كلية الهندسة المعلوماتية",
                                                        University =new University()
                                                        {
                                                            Name ="جامعة حلب",
                                                            City= new City()
                                                            {
                                                                Name ="حلب"
                                                            }
                                                        }
                                                    },
                                                    Section = new Tag()
                                                    {
                                                        Name = "القسم الافتراضي",
                                                        Type = TagTypes.Section
                                                    },
                                                    Semester = new Tag()
                                                    {
                                                        Name = "الفصل الافتراضي",
                                                        Type = TagTypes.Semester
                                                    },
                                                    Subject = new Subject()
                                                    {
                                                        Name = "البرمجة 1",
                                                        Description = "وصف مادة البرمجة 1",
                                                        IsFree = false,
                                                        SubjectTags = new List<SubjectTag>()
                                                        {
                                                            new SubjectTag()
                                                            {
                                                                Tag = new Tag()
                                                                {
                                                                    Name = "دكتور اسامه",
                                                                    Type = TagTypes.Doctor,
                                                                },
                                                            },
                                                            new SubjectTag()
                                                            {
                                                                Tag = new Tag()
                                                                {
                                                                    Name = "الفصل الاول",
                                                                    Type = TagTypes.Semester,
                                                                },
                                                            }
                                                        },
                                                        Type = SubjectTypes.Academic,
                                                        Exams = new List<Exam>()
                                                        {
                                                            new Exam()
                                                            {
                                                                Name = Guid.NewGuid() + "دورة",
                                                                Year = 2019,
                                                                Price = 500,
                                                                ExamTags = new List<ExamTag>()
                                                                {
                                                                    new ExamTag()
                                                                    {
                                                                        Tag = new Tag()
                                                                        {
                                                                            Name = "وسم",
                                                                            Type = TagTypes.Tag,
                                                                        }
                                                                    },
                                                                    new ExamTag()
                                                                    {
                                                                        Tag = new Tag()
                                                                        {
                                                                            Name = "دكتور اسامه",
                                                                            Type = TagTypes.Doctor,
                                                                        },
                                                                    },
                                                                    new ExamTag()
                                                                    {
                                                                        Tag = new Tag()
                                                                        {
                                                                            Name = "الفصل الاول",
                                                                            Type = TagTypes.Semester,
                                                                        }
                                                                      },
                                                                },
                                                                Type = TabTypes.Microscope
                                                            },
                                                            new Exam()
                                                            {
                                                                Name = Guid.NewGuid() + "دورة",
                                                                Year = 2018,
                                                                Price = 600,
                                                                ExamTags = new List<ExamTag>()
                                                                {
                                                                    new ExamTag()
                                                                    {
                                                                        Tag = new Tag()
                                                                        {
                                                                            Name = "وسم",
                                                                            Type = TagTypes.Tag,
                                                                        }
                                                                    },
                                                                    new ExamTag()
                                                                    {
                                                                        Tag = new Tag()
                                                                        {
                                                                            Name = "دكتور اسامه",
                                                                            Type = TagTypes.Doctor,
                                                                        },
                                                                    },
                                                                    new ExamTag()
                                                                    {
                                                                        Tag = new Tag()
                                                                        {
                                                                            Name = "الفصل الاول",
                                                                            Type = TagTypes.Semester,
                                                                        }
                                                                      },
                                                                },
                                                                Type = TabTypes.Exam,
                                                                ExamQuestions = new  List<ExamQuestion>()
                                                                {
                                                                    new ExamQuestion()
                                                                    {
                                                                        Question = new Question()
                                                                        {
                                                                            Title = "السؤال الاول",
                                                                            Hint = "مساعدة",
                                                                            IsCorrected = false,
                                                                            AnswerType = AnswerTypes.MultiChoice,
                                                                            Answers = new List<Answer>()
                                                                            {
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 1",
                                                                                    IsCorrect = true,
                                                                                },
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 2",
                                                                                    IsCorrect = true,
                                                                                },
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 3",
                                                                                    IsCorrect = true,
                                                                                },
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 4",
                                                                                    IsCorrect = true,
                                                                                },
                                                                            }
                                                                        },
                                                                    },
                                                                    new ExamQuestion()
                                                                    {
                                                                        Question = new Question()
                                                                        {
                                                                            Title = "السؤال الثاني",
                                                                            Hint = "مساعدة",
                                                                            IsCorrected = false,
                                                                            AnswerType = AnswerTypes.MultiChoice,
                                                                            Answers = new List<Answer>()
                                                                            {
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 1",
                                                                                    IsCorrect = true,
                                                                                },
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 2",
                                                                                    IsCorrect = true,
                                                                                },
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 3",
                                                                                    IsCorrect = true,
                                                                                },
                                                                                new Answer()
                                                                                {
                                                                                    Title = "اجابة 4",
                                                                                    IsCorrect = true,
                                                                                },
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        },
                                                    }
                                                }
                                            }
                                        },
                                        IsHidden = false,
                                        Type = PackageTypes.Sale
                                    }
                                }
                            }
                        }
                    }
                };

                var createResult = await userManager.CreateAsync(user, "1234");

                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SmartStartRoles.User.ToString());

                    return;
                }
                throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
            }
        }
        private static async Task InsureCreateGuidSellerAsync(UserManager<AppUser> userManager)
        {
            var seller = await userManager.FindByIdAsync("AC140878-B8C0-47F6-AAC7-6E6E601A238B");
            if (seller is null)
            {
                seller = new AppUser()
                {
                    Id = new("{AC140878-B8C0-47F6-AAC7-6E6E601A238B}"),
                    UserName = "SmartStartSeller",
                    Name = "SmartStartSeller",
                    Email = "SmartStartSeller@SmartStart.com",
                    Type = UserTypes.Seller,
                    MoneyLimit = 9000000,
                };
                var createResult = await userManager.CreateAsync(seller, "smartstartseller1234");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(seller, SmartStartRoles.Seller.ToString());
                    return;
                }
                throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
            }
        }
    }
}
