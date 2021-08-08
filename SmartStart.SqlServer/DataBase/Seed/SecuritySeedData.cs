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
            //await InsureCreateSellerAsync(userManager, roleManager, newRole);
            //await InsureCreateUserAsync(userManager, roleManager, newRole);
            //await InsureCreateGuidSellerAsync(userManager);
        }

        //private static async Task InsureCreateUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IEnumerable<string> newRole)
        //{
        //    var user = await userManager.FindByNameAsync("Student@Student.Student");

        //    if (user is null)
        //    {
        //        user = new AppUser()
        //        {
        //            UserName = "Student@Student.Student",
        //            Name = "Student",
        //            Email = "Student@Student.Student",
        //            Type = SharedKernel.Enums.UserTypes.User,
        //            Rates = new List<Rate>() { new Rate() { DiscountRate = 0.1f } },
        //            Codes = new List<Code>() {

        //                new Code() { DiscountRate=0, Value=1000 , Hash="rtyk6q" ,


        //            SellerId = (await userManager.FindByNameAsync("Seller")).Id,
        //                                    CodePackages = new List<CodePackage>(){
        //                                      new CodePackage(){
        //                                          Package = new Package(){
        //                                          Description =  "وصف البكج المقدم",
        //                                          Name ="اسم البكج",StartDate = DateTime.Now,
        //                                          EndDate = DateTime.Now.AddDays(15),
        //                                          Price = 1000,
        //                                          //PackageExams = new List<PackageExam>(){

        //                                          //    new PackageExam()
        //                                          //    {
        //                                          //        Price=400,
        //                                          //        Exam = new Exam()
        //                                          //        {
        //                                          //            Name =Guid.NewGuid() + "دورة",
        //                                          //            Year=2019,
        //                                          //            Price=500,
        //                                          //            Subject = new Subject()
        //                                          //            {
        //                                          //                Faculty= new Faculty(){
        //                                          //                Name = "كلية ما",
        //                                          //                University =new University()
        //                                          //                {
        //                                          //                    Name ="جامعة ما",
        //                                          //                    City= new City()
        //                                          //                    {
        //                                          //                        Name ="مدينة ما"
        //                                          //                    }
        //                                          //                }
        //                                          //                },
        //                                          //                Type=SharedKernel.Enums.SubjectTypes.Academic,
        //                                          //                Name ="تشريح",
        //                                          //                SubjectTags = new List<SubjectTag>(){
        //                                          //                new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور محمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                 new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    },
        //                                          //                }
        //                                          //                },

        //                                          //            },
        //                                          //            ExamTags = new List<ExamTag>()
        //                                          //            {
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="وسم",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Tag,
        //                                          //                    }
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور أحمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    }
        //                                          //                },
        //                                          //            },

        //                                          //            Type =SharedKernel.Enums.TabTypes.Microscope,
        //                                          //            //QuestionDocuments = new List<QuestionDocument>(){
        //                                          //            //new QuestionDocument()
        //                                          //            //{
        //                                          //            //    Document =new Model.Shared.Document()
        //                                          //            //    {
        //                                          //            //        Name = "صورة تلسسكوب ما",
        //                                          //            //        Path= @"\Documents\Telescopeblahblah\asda.jpg",
        //                                          //            //        Type=SharedKernel.Enums.DocumentTypes.Image,
        //                                          //            //    },
        //                                          //            //},
        //                                          //            // new QuestionDocument()
        //                                          //            //{
        //                                          //            //    Document =new Model.Shared.Document()
        //                                          //            //    {
        //                                          //            //        Name = "صورة تلسسكوب2 ما",
        //                                          //            //        Path= @"\Documents\Telescopeblahblah\sadasd.jpg",
        //                                          //            //        Type=SharedKernel.Enums.DocumentTypes.Image,
        //                                          //            //    },
        //                                          //            //}

        //                                          //            //},


        //                                          //        },




        //                                          //    },

        //                                          //    new PackageExam()
        //                                          //    {
        //                                          //        Price=234,

        //                                          //           Exam = new Exam()
        //                                          //        {
        //                                          //            Name =Guid.NewGuid() + "دورة",
        //                                          //            Year=2019,
        //                                          //            Price=500,
        //                                          //            Subject = new Subject()
        //                                          //            {
        //                                          //                Faculty= new Faculty(){
        //                                          //                Name = "كلية ما",
        //                                          //                University =new University()
        //                                          //                {
        //                                          //                    Name ="جامعة ما",
        //                                          //                    City= new City()
        //                                          //                    {
        //                                          //                        Name ="مدينة ما"
        //                                          //                    }
        //                                          //                }
        //                                          //                },
        //                                          //                Type=SharedKernel.Enums.SubjectTypes.Academic,
        //                                          //                Name ="تشريح",
        //                                          //                SubjectTags = new List<SubjectTag>(){
        //                                          //                new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور محمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                 new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    },
        //                                          //                }
        //                                          //                },

        //                                          //            },
        //                                          //            ExamTags = new List<ExamTag>()
        //                                          //            {
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="وسم",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Tag,
        //                                          //                    }
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور أحمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    }
        //                                          //                },
        //                                          //            },

        //                                          //            Type =SharedKernel.Enums.TabTypes.Exam,

        //                                          //            ExamQuestions = new  List<ExamQuestion>()
        //                                          //            {
        //                                          //                new ExamQuestion()
        //                                          //                {

        //                                          //                    Question = new Question()
        //                                          //                    {
        //                                          //                        Title ="السوال الاول",
        //                                          //                        Hint = "مساعدة",
        //                                          //                        IsCorrected=false,
        //                                          //                        AnswerType =SharedKernel.Enums.AnswerTypes.MultiChoice,
        //                                          //                        Answers =new List<Answer>()
        //                                          //                        {
        //                                          //                            new Answer(){
        //                                          //                            Title ="اجابة 1",
        //                                          //                            IsCorrect =true,
        //                                          //                            },
        //                                          //                             new Answer(){
        //                                          //                            Title ="اجابة 2",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                              new Answer(){
        //                                          //                            Title ="اجابة 3",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                               new Answer(){
        //                                          //                            Title ="اجابة 4",
        //                                          //                            IsCorrect =false,
        //                                          //                            //CorrectAnswer= new Answer()
        //                                          //                            //{
        //                                          //                            //      Title ="اجابة 4 تصحيح",
        //                                          //                            //    Option ="false",
        //                                          //                            //    IsCurrect =false,
        //                                          //                            //    }
        //                                          //                            },
        //                                          //                        }
        //                                          //                    }

        //                                          //                    ,

        //                                          //                },


        //                                          //                new ExamQuestion()
        //                                          //                {

        //                                          //                    Question = new Question()
        //                                          //                    {
        //                                          //                        Title ="السوال الثاني",
        //                                          //                        Hint = "مساعدة",
        //                                          //                        IsCorrected=false,
        //                                          //                        AnswerType =SharedKernel.Enums.AnswerTypes.MultiChoice,
        //                                          //                        Answers =new List<Answer>()
        //                                          //                        {
        //                                          //                            new Answer(){
        //                                          //                            Title ="اجابة 1",
        //                                          //                            IsCorrect =true,
        //                                          //                            },
        //                                          //                             new Answer(){
        //                                          //                            Title ="اجابة 2",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                              new Answer(){
        //                                          //                            Title ="اجابة 3",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                               new Answer(){
        //                                          //                            Title ="اجابة 4",
        //                                          //                            IsCorrect =false,
        //                                          //                            //CorrectAnswer= new Answer()
        //                                          //                            //{
        //                                          //                            //      Title ="اجابة 4 تصحيح",
        //                                          //                            //    Option ="false",
        //                                          //                            //    IsCurrect =false,
        //                                          //                            //    }
        //                                          //                            },
        //                                          //                        }
        //                                          //                    }

        //                                          //                    ,

        //                                          //                },
        //                                          //            }


        //                                          //        },
        //                                          //    },




        //                                          //          new PackageExam()
        //                                          //    {
        //                                          //        Price=234,

        //                                          //           Exam = new Exam()
        //                                          //        {
        //                                          //            Name =Guid.NewGuid() + "دورة",
        //                                          //            Year=2019,
        //                                          //            Price=500,
        //                                          //            Subject = new Subject()
        //                                          //            {
        //                                          //                Faculty= new Faculty(){
        //                                          //                Name = "كلية ما",
        //                                          //                University =new University()
        //                                          //                {
        //                                          //                    Name ="جامعة ما",
        //                                          //                    City= new City()
        //                                          //                    {
        //                                          //                        Name ="مدينة ما"
        //                                          //                    }
        //                                          //                }
        //                                          //                },
        //                                          //                Type=SharedKernel.Enums.SubjectTypes.Academic,
        //                                          //                Name ="تشريح",
        //                                          //                SubjectTags = new List<SubjectTag>(){
        //                                          //                new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور محمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                 new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    },
        //                                          //                }
        //                                          //                },

        //                                          //            },
        //                                          //            ExamTags = new List<ExamTag>()
        //                                          //            {
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="وسم",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Tag,
        //                                          //                    }
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور أحمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    }
        //                                          //                },
        //                                          //            },

        //                                          //            Type =SharedKernel.Enums.TabTypes.Interview,

        //                                          //            ExamQuestions = new  List<ExamQuestion>()
        //                                          //            {
        //                                          //                new ExamQuestion()
        //                                          //                {

        //                                          //                    Question = new Question()
        //                                          //                    {
        //                                          //                        Title ="السوال",
        //                                          //                        Hint = "وصف",
        //                                          //                        IsCorrected=true,
        //                                          //                        AnswerType =SharedKernel.Enums.AnswerTypes.Text,
        //                                          //                        Answers =new List<Answer>()
        //                                          //                        {
        //                                          //                            new Answer(){
        //                                          //                            Title ="الجوااب للجواب",
        //                                          //                            IsCorrect =true,
        //                                          //                            },
        //                                          //                        }
        //                                          //                    }
        //                                          //                }
        //                                          //            }


        //                                          //        },
        //                                          //    }
        //                                          //          ,

        //                                          //             new PackageExam()
        //                                          //    {
        //                                          //        Price=234,

        //                                          //           Exam = new Exam()
        //                                          //        {
        //                                          //            Name =Guid.NewGuid() + "دورة",
        //                                          //            Year=2019,
        //                                          //            Price=500,
        //                                          //            Subject = new Subject()
        //                                          //            {
        //                                          //                Faculty= new Faculty(){
        //                                          //                Name = "كلية ما",
        //                                          //                University =new University()
        //                                          //                {
        //                                          //                    Name ="جامعة ما",
        //                                          //                    City= new City()
        //                                          //                    {
        //                                          //                        Name ="مدينة ما"
        //                                          //                    }
        //                                          //                }
        //                                          //                },
        //                                          //                Type=SharedKernel.Enums.SubjectTypes.Academic,
        //                                          //                Name ="تشريح",
        //                                          //                SubjectTags = new List<SubjectTag>(){
        //                                          //                new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور محمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                 new SubjectTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    },
        //                                          //                }
        //                                          //                },

        //                                          //            },
        //                                          //            ExamTags = new List<ExamTag>()
        //                                          //            {
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="وسم",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Tag,
        //                                          //                    }
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                     Tag =new Model.Shared.Tag(){
        //                                          //                    Name="ديكتور أحمد",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Doctor,
        //                                          //                    },
        //                                          //                },
        //                                          //                new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفصل الاول",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Semester,
        //                                          //                    }
        //                                          //                },
        //                                          //                        new ExamTag()
        //                                          //                {
        //                                          //                    Tag =new Model.Shared.Tag(){
        //                                          //                    Name="الفريق",
        //                                          //                    Type = SharedKernel.Enums.TagTypes.Team
        //                                          //                    }
        //                                          //                },
        //                                          //            },

        //                                          //            Type =SharedKernel.Enums.TabTypes.Bank,

        //                                          //            ExamQuestions = new  List<ExamQuestion>()
        //                                          //            {
        //                                          //                new ExamQuestion()
        //                                          //                {

        //                                          //                    Question = new Question()
        //                                          //                    {
        //                                          //                        Title ="السوال الاول",
        //                                          //                        Hint = "مساعدة",
        //                                          //                        IsCorrected=false,
        //                                          //                        AnswerType =SharedKernel.Enums.AnswerTypes.MultiChoice,
        //                                          //                        Answers =new List<Answer>()
        //                                          //                        {
        //                                          //                            new Answer(){
        //                                          //                            Title ="اجابة 1",
        //                                          //                            IsCorrect =true,
        //                                          //                            },
        //                                          //                             new Answer(){
        //                                          //                            Title ="اجابة 2",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                              new Answer(){
        //                                          //                            Title ="اجابة 3",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                               new Answer(){
        //                                          //                            Title ="اجابة 4",
        //                                          //                            IsCorrect =false,
        //                                          //                            //CorrectAnswer= new Answer()
        //                                          //                            //{
        //                                          //                            //      Title ="اجابة 4 تصحيح",
        //                                          //                            //    Option ="false",
        //                                          //                            //    IsCurrect =false,
        //                                          //                            //    }
        //                                          //                            },
        //                                          //                        }
        //                                          //                    }

        //                                          //                    ,

        //                                          //                },

        //                                          //                  new ExamQuestion()
        //                                          //                {

        //                                          //                    Question = new Question()
        //                                          //                    {
        //                                          //                        Title ="السوال الاول",
        //                                          //                        Hint = "مساعدة",
        //                                          //                        IsCorrected=false,
        //                                          //                        AnswerType =SharedKernel.Enums.AnswerTypes.MultiChoice,
        //                                          //                        Answers =new List<Answer>()
        //                                          //                        {
        //                                          //                            new Answer(){
        //                                          //                            Title ="اجابة 1",
        //                                          //                            IsCorrect =true,
        //                                          //                            },
        //                                          //                             new Answer(){
        //                                          //                            Title ="اجابة 2",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                              new Answer(){
        //                                          //                            Title ="اجابة 3",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                               new Answer(){
        //                                          //                            Title ="اجابة 4",
        //                                          //                            IsCorrect =false,
        //                                          //                            //CorrectAnswer= new Answer()
        //                                          //                            //{
        //                                          //                            //      Title ="اجابة 4 تصحيح",
        //                                          //                            //    Option ="false",
        //                                          //                            //    IsCurrect =false,
        //                                          //                            //    }
        //                                          //                            },
        //                                          //                        }
        //                                          //                    }

        //                                          //                    ,

        //                                          //                },

        //                                          //                    new ExamQuestion()
        //                                          //                {

        //                                          //                    Question = new Question()
        //                                          //                    {
        //                                          //                        Title ="السوال الاول",
        //                                          //                        Hint = "مساعدة",
        //                                          //                        IsCorrected=false,
        //                                          //                        AnswerType =SharedKernel.Enums.AnswerTypes.MultiChoice,
        //                                          //                        Answers =new List<Answer>()
        //                                          //                        {
        //                                          //                            new Answer(){
        //                                          //                            Title ="اجابة 1",
        //                                          //                            IsCorrect =true,
        //                                          //                            },
        //                                          //                             new Answer(){
        //                                          //                            Title ="اجابة 2",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                              new Answer(){
        //                                          //                            Title ="اجابة 3",
        //                                          //                            IsCorrect =false,
        //                                          //                            },
        //                                          //                               new Answer(){
        //                                          //                            Title ="اجابة 4",
        //                                          //                            IsCorrect =false,
        //                                          //                            //CorrectAnswer= new Answer()
        //                                          //                            //{
        //                                          //                            //      Title ="اجابة 4 تصحيح",
        //                                          //                            //    Option ="false",
        //                                          //                            //    IsCurrect =false,
        //                                          //                            //    }
        //                                          //                            },
        //                                          //                        }
        //                                          //                    }

        //                                          //                    ,

        //                                          //                },
        //                                          //            }


        //                                          //        },
        //                                          //    }
        //                                          //},
                                                  

        //                                          },

        //                                    },

        //                                       new CodePackage(){
        //                                          Package = new Package(){
        //                                          Description = Guid.NewGuid()+ " وصف البكج المقدم",
        //                                          Name =Guid.NewGuid()+"اسم البكج",StartDate = DateTime.Now,
        //                                          EndDate = DateTime.Now.AddDays(15),
        //                                          Price = 1000,},}


        //                                    }  } },
        //        };

        //        var createResult = await userManager.CreateAsync(user, "1234");

        //        if (createResult.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(user, TarafouaRoles.User.ToString());

        //            return;
        //        }
        //        throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
        //    }
        //}

        //private static async Task InsureCreateSellerAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IEnumerable<string> newRole)
        //{
        //    var seller = await userManager.FindByNameAsync("Seller");

        //    if (seller is null)
        //    {
        //        seller = new AppUser()
        //        {
        //            UserName = "Seller",
        //            Name = "Seller",
        //            Email = "Seller@Seller.Seller",
        //            Type = SharedKernel.Enums.UserTypes.Seller,
        //            MoneyLimit = 10000,
        //            Rates = new List<Rate>() { new Rate() { DiscountRate = 0.1f } },
        //            Codes = new List<Code>() { new Code() { DiscountRate=0, Value=1000 , Hash="J3a3aO" ,
        //                                    CodePackages = new List<CodePackage>(){
        //                                      new CodePackage(){
        //                                          Package = new Package(){
        //                                          Description =  "وصف البكج المقدم",
        //                                          Name ="اسم البكج",StartDate = DateTime.Now,
        //                                          EndDate = DateTime.Now.AddDays(15),
        //                                          Price = 1000,
        //                                           },
        //                                    } }  } },
        //        };

        //        var createResult = await userManager.CreateAsync(seller, "1234");

        //        if (createResult.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(seller, TarafouaRoles.Seller.ToString());

        //            return;
        //        }
        //        throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
        //    }
        //}

        //private static async Task InsureCreateGuidSellerAsync(UserManager<AppUser> userManager)
        //{
        //    var seller = await userManager.FindByIdAsync("AC140878-B8C0-47F6-AAC7-6E6E601A238B");

        //    if (seller is null)
        //    {
        //        seller = new AppUser()
        //        {
        //            Id = new("{AC140878-B8C0-47F6-AAC7-6E6E601A238B}"),
        //            UserName = "TarafouaSeller",
        //            Name = "TarafouaSeller",
        //            Email = "TarafouaSeller@Tarafoua.com",
        //            Type = SharedKernel.Enums.UserTypes.Seller,
        //            MoneyLimit = 9000000,
        //        };

        //        var createResult = await userManager.CreateAsync(seller, "tarafouaseller1234");

        //        if (createResult.Succeeded)
        //        {
        //            await userManager.AddToRoleAsync(seller, TarafouaRoles.Seller.ToString());
        //            return;
        //        }
        //        throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));
        //    }
        //}

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
                    // var roleResult = await userManager.AddToRolesAsync(superAdmin, identityRoles);
                    var roleResult = await userManager.AddToRoleAsync(superAdmin, SmartStartRoles.Admin.ToString());

                    if (roleResult.Succeeded)
                        return;
                    throw new Exception(String.Join("\n", roleResult.Errors.Select(error => error.Description)));
                }
                throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));

            }

            //if (newRoles.Any())
            //{
            //    // var addRoleResult = await userManager.AddToRolesAsync(superAdmin, newRoles);
            //    var addRoleResult = await userManager.AddToRoleAsync(superAdmin, "Admin");
            //    if (addRoleResult.Succeeded)
            //        return;
            //    else
            //        throw new Exception(String.Join("\n", addRoleResult.Errors.Select(error => error.Description)));
            //}
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
                    // var roleResult = await userManager.AddToRolesAsync(superAdmin, identityRoles);
                    var roleResult = await userManager.AddToRoleAsync(superAdminElkood, SmartStartRoles.Admin.ToString());

                    if (roleResult.Succeeded)
                        return;
                    throw new Exception(String.Join("\n", roleResult.Errors.Select(error => error.Description)));
                }
                throw new Exception(String.Join("\n", createResult.Errors.Select(error => error.Description)));

            }

            //if (newRoles.Any())
            //{
            //    // var addRoleResult = await userManager.AddToRolesAsync(superAdmin, newRoles);
            //    var addRoleResult = await userManager.AddToRoleAsync(superAdminElkood, "Admin");
            //    if (addRoleResult.Succeeded)
            //        return;
            //    else
            //        throw new Exception(String.Join("\n", addRoleResult.Errors.Select(error => error.Description)));
            //}

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

    }
}
