using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartStart.Model.Business;
using SmartStart.Model.General;
using SmartStart.Model.Main;
using SmartStart.Model.Setting;
using Microsoft.Extensions.DependencyInjection;
using SmartStart.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartStart.Model.Security;
using SmartStart.SharedKernel.Security;
using SmartStart.Model.Shared;

namespace SmartStart.SqlServer.DataBase.Seed
{
    public class DataSeed
    {
        private static Random random = new Random();
        private static string[] faculties = { "كلية الهندسة المعلوماتية", "كلية الطب البشري", "كلية الهندسة الكهربائية",
                                              "كلية الميكانيك", "كلية الصيدلة", "كلية الهندسة المعمارية",
                                              "كلية الفنون", "كلية التربية", "المعهد التقاني الطبي",
                                              "المعهد التقاني الهندسي", "كلية الهندسة الزراعية", "كلية التمريض" };
        public static async System.Threading.Tasks.Task InitializeAsync(IServiceProvider services)
        {
            var context = (SmartStartDbContext)services.GetService(typeof(SmartStartDbContext));
            var roleManager = services.GetService<RoleManager<IdentityRole<Guid>>>();
            var userManager = services.GetService<UserManager<AppUser>>();
            await CitySeed(context);
            await AdvertisementSeed(context);
            await UniversitySeed(context);
            await FacultySeed(context);
            await UserSeed(context, userManager);
            await NotificationSeed(context);
            await FeedbackSeed(context);
            await TagSeed(context);
            await SubjectSeed(context);
            await PackageSeed(context);
            await QuestionSeed(context);
            await ExamAndBankSeed(context);
            await InterviewSeed(context);

        }

        private static async Task CitySeed(SmartStartDbContext context)
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
        private static async Task AdvertisementSeed(SmartStartDbContext context)
        {
            if (!context.Advertisements.Where(e => !e.DateDeleted.HasValue).Any())
            {
                string[] titles = { "تعريف بالتطبيق", "كلية الاقتصاد",  "كلية الاقتصاد", "كلية الهندسة الزراعية", "كلية الآداب والعلوم الإنسانية", "جامعة حلب" };
                var res = new List<Advertisement>(); 
                for (int i = 0; i < titles.Length; i++)
                {
                    int f = RandomInteger(0, 1);
                    res.Add(new Advertisement
                    {
                        Title = titles[i], 
                        StartDate = new DateTime(2021, RandomInteger(1, 6), RandomInteger(1, 29)),
                        EndDate = new DateTime(2021, RandomInteger(7, 12), RandomInteger(1, 29)),
                        ImagePath = "",
                        Type = f == 0? AdvertisementTypes.Offer : AdvertisementTypes.Offer, 
                        Price = f == 0? RandomInteger(0, 100) : null,
                        Description = f == 0? RandomString(RandomInteger(20, 30)) : null
                    });
                }
                await context.AddRangeAsync(res);
                await context.SaveChangesAsync();
            }
        }
        private static async Task UniversitySeed(SmartStartDbContext context)
        {
            if (!context.Universities.Where(e => !e.DateDeleted.HasValue).Any())
            {
                string[] names = { "حلب", "دمشق", "حماة", "طرطوس", "الحسكة", "الرقة" };
                var res = new List<University>();
                for (int i = 0; i < names.Length; i++)
                {
                    res.Add(new University
                    {
                        Name = "جامعة " + names[i],
                        CityId = context.Cities.SingleOrDefault(c => c.Name == names[i]).Id
                    });
                }
                await context.AddRangeAsync(res);
                await context.SaveChangesAsync();
            }
        }
        private static async Task FacultySeed(SmartStartDbContext context)
        {
            if (!context.Faculties.Where(e => !e.DateDeleted.HasValue).Any())
            {
                
                int[] num = { 5, 6, 5, 5, 5, 5, 4, 4, 2, 2, 5, 4 };
                int numOfUniv = context.Universities.Where(u => !u.DateDeleted.HasValue).Count();
                var res = new List<Faculty>();
                for (int i = 0; i < faculties.Length; i++)
                {
                    res.Add(new Faculty
                    {
                        Name = faculties[i], 
                        ImagePath = "", 
                        NumberOfYear = num[i], 
                        UniversityId = context.Universities.Where(u => !u.DateDeleted.HasValue).Skip(RandomInteger(0, numOfUniv - 1)).First().Id
                    });
                }
                await context.AddRangeAsync(res);
                await context.SaveChangesAsync();
            }
        }
        private static async Task UserSeed(SmartStartDbContext context, UserManager<AppUser> userManager)
        {   
            string[] names = { "عبد القادر نجار", "حسام حجار", "شهد عتيق" };
            string[] engNames = { "AbdAlqader", "Husam", "Shahed" };
            string[] count = { "الأول", "الثاني", "الثالث" };
            var res = new List<AppUser>();
            for (int i = 0; i < names.Length; i++)
            {
                var user = await userManager.FindByNameAsync(engNames[i]);
                if (user != null) continue;
                user = new AppUser
                {
                    Address = "الموقع " + count[i],
                    Birthday = new DateTime(1999, 10, 10),
                    CityId = context.Cities.SingleOrDefault(c => !c.DateDeleted.HasValue && c.Name == "حلب").Id,
                    DateActivated = DateTime.Now,
                    Email = engNames[i] + "@gmail.com",
                    FacultyId = context.Faculties.SingleOrDefault(f => !f.DateDeleted.HasValue && f.Name == "كلية الهندسة المعلوماتية").Id,
                    Gender = (i > 1),
                    Name = names[i],
                    PhoneNumber = PhoneNumberGenerator(),
                    Type = UserTypes.User,
                    UserName = PhoneNumberGenerator()
                };
                var createResult = await userManager.CreateAsync(user, "1234");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, SmartStartRoles.User.ToString());
                }
            }
            await context.SaveChangesAsync();
        }
        private static async Task NotificationSeed(SmartStartDbContext context)
        {
            if (!context.Notifications.Where(e => !e.DateDeleted.HasValue && e.Type == NotificationTypes.User).Any())
            { 
                var users = await context.Users.Where(u => !u.DateDeleted.HasValue
                                                        && u.Type == UserTypes.User).ToListAsync();
                for (int i = 0; i < faculties.Length; i++)
                {
                    var notification = new Notification
                    {
                        Title = "تفعيل تطبيق ترفع الإلكتروني 🔑",
                        Body = "أهلا وسهلا طلاب " + faculties[i] + " في تطبيق ترفع الالكتروني 😍",
                        HasReaded = false,
                        Type = NotificationTypes.User,
                        Date = DateTime.Now
                    }; 
                    await context.AddAsync(notification);
                    await context.SaveChangesAsync();
                    var temp = new List<UserNotification>(); 
                    for(int j = 0; j < users.Count(); j++)
                    {
                        temp.Add(new UserNotification
                        {
                            AppUserId = users[j].Id,
                            NotificationId = notification.Id
                        });
                    }
                    await context.UserNotifications.AddRangeAsync(temp);
                }
                await context.SaveChangesAsync();
            }
        }
        private static async Task FeedbackSeed(SmartStartDbContext context)
        {
            if (!context.Feedbacks.Where(e => !e.DateDeleted.HasValue).Any())
            {
                var users = await context.Users.Where(u => !u.DateDeleted.HasValue
                                                        && u.Type == UserTypes.User).ToListAsync();
                var userCount = users.Count();
                var res = new List<Feedback>();
                for (int i = 0; i < 13; i++)
                {
                    var flag = RandomInteger(0, 1);
                    res.Add(new Feedback
                    {
                        AppUserId = users.Skip(RandomInteger(0, userCount - 1)).First().Id, 
                        Title = "دعم التطبيق", 
                        Body = "ايمت رح يدعم التطبيق " + faculties[RandomInteger(0, faculties.Length - 1)] + " ??!!",
                        ReplyDate = flag == 0? null : DateTime.Now,
                        Reply = flag == 0? "" : "قريباً!!"
                    });
                }
                await context.AddRangeAsync(res);
                await context.SaveChangesAsync();
            }
        }
        private static async Task TagSeed(SmartStartDbContext context)
        {
            if (!context.Tags.Where(e => !e.DateDeleted.HasValue).Any())
            {
                string[] doctors = { "دكتور زيزو", "دكتور بيبو", "دكتور ميمو", "دكتور تيتو", "دكتور جيجو", "دكتور حلاوة" };
                string[] sections = { "قسم الذكاء الصنعي", "قسم البرمجيات", "قسم الالكترون", "قسم هندسة الطيران",
                                     "قسم البغاضة", "قسم الحلاوة", "قسم الفشل", "قسم الأقسام"};
                string[] semesters = { "الفصل الأول", "الفصل الثاني", "الفصل الثالث" };
                string[] teams = { "فريق الفا", "فريق بيتا", "فريق زيتا", "فريق الحلاوة" };
                string[] tags = { "هام", "غير هام", "صعب", "سهل", "حديث", "قديم" };
                var res = new List<Tag>();
                for (int i = 0; i < doctors.Length; i++)
                {
                    res.Add(new Tag
                    {
                        Name = doctors[i],
                        Type = TagTypes.Doctor,
                    });
                }
                for (int i = 0; i < sections.Length; i++)
                {
                    res.Add(new Tag
                    {
                        Name = sections[i],
                        Type = TagTypes.Section,
                    });
                }
                for (int i = 0; i < semesters.Length; i++)
                {
                    res.Add(new Tag
                    {
                        Name = semesters[i],
                        Type = TagTypes.Semester,
                    });
                }
                for (int i = 0; i < teams.Length; i++)
                {
                    res.Add(new Tag
                    {
                        Name = teams[i],
                        Type = TagTypes.Team,
                    });
                }
                for (int i = 0; i < tags.Length; i++)
                {
                    res.Add(new Tag
                    {
                        Name = tags[i],
                        Type = TagTypes.Tag,
                    });
                }
                await context.AddRangeAsync(res);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SubjectSeed(SmartStartDbContext context)
        {
            if (!context.Subjects.Where(e => !e.DateDeleted.HasValue).Any())
            {
                var res = new List<Subject>();
                string[] desc = { "حلوة", "سهلة", "بغيضة" };
                string[] subjectsIt = { "المعالج المصغر", "أمن المعلومات", "إدارة المشاريع" };
                string[] subjectsPl = { "تربية نبات	", "ري وصرف زراعي", "إنتاج محاصيل" };
                var doctors = await context.Tags.Where(t => !t.DateDeleted.HasValue
                                                         && t.Type == TagTypes.Doctor).ToListAsync();
                var doctorsCount = doctors.Count(); 
                var sections = await context.Tags.Where(t => !t.DateDeleted.HasValue
                                                          && t.Type == TagTypes.Section).ToListAsync();
                var sectionsCount = sections.Count(); 
                var semesters = await context.Tags.Where(t => !t.DateDeleted.HasValue
                                                           && t.Type == TagTypes.Semester).ToListAsync();
                var semestersCount = semesters.Count();
                var teams = await context.Tags.Where(t => !t.DateDeleted.HasValue
                                                       && t.Type == TagTypes.Team).ToListAsync();
                var teamsCount = teams.Count();
                var _faculties = await context.Faculties.Where(f => !f.DateDeleted.HasValue).ToListAsync();
                var facultiesCount = teams.Count();
                var tags = await context.Tags.Where(t => !t.DateDeleted.HasValue
                                                      && t.Type == TagTypes.Tag).ToListAsync();
                var tagsCount = tags.Count();
                for (int i = 0; i < subjectsIt.Length ; i++)
                {
                    var subject = new Subject
                    {
                        Description = "المادة " + desc[RandomInteger(0, desc.Length - 1)],
                        ImagePath = "",
                        IsFree = (RandomInteger(0, 4) == 0),
                        Name = subjectsIt[i],
                        Type = SubjectTypes.Academic,
                    };
                    await context.Subjects.AddAsync(subject);
                    await context.SaveChangesAsync(); 
                    await context.SubjectFaculties.AddAsync(new SubjectFaculty
                    {
                        DoctorId = doctors.Skip(RandomInteger(0, doctorsCount - 1)).First().Id,
                        FacultyId = _faculties.SingleOrDefault(f => f.Name == "كلية الهندسة المعلوماتية").Id,
                        Price = 500 * RandomInteger(2, 10),
                        SectionId = sections.Skip(RandomInteger(0, sectionsCount - 1)).First().Id,
                        SemesterId = semesters.Skip(RandomInteger(0, semestersCount - 1)).First().Id,
                        SubjectId = subject.Id,
                        Year = RandomInteger(1, 5)
                    });
                    for(int j = 0; j < tagsCount; j += 2)
                    {
                        await context.SubjectTags.AddAsync(new SubjectTag
                        {
                            SubjectId = subject.Id,
                            TagId = tags[i].Id
                        });
                    }
                }
                for (int i = 0; i < subjectsPl.Length; i++)
                {
                    var subject = new Subject
                    {
                        Description = "المادة " + desc[RandomInteger(0, desc.Length - 1)],
                        ImagePath = "",
                        IsFree = (RandomInteger(0, 4) == 0),
                        Name = subjectsPl[i],
                        Type = SubjectTypes.Academic,
                    };
                    await context.Subjects.AddAsync(subject);
                    await context.SaveChangesAsync();
                    await context.SubjectFaculties.AddAsync(new SubjectFaculty
                    {
                        DoctorId = doctors.Skip(RandomInteger(0, doctorsCount - 1)).First().Id,
                        FacultyId = _faculties.SingleOrDefault(f => f.Name == "كلية الهندسة الزراعية").Id,
                        Price = 500 * RandomInteger(2, 10),
                        SectionId = sections.Skip(RandomInteger(0, sectionsCount - 1)).First().Id,
                        SemesterId = semesters.Skip(RandomInteger(0, semestersCount - 1)).First().Id,
                        SubjectId = subject.Id,
                        Year = RandomInteger(1, 5)
                    }); 
                    for (int j = 0; j < tagsCount; j += 2)
                    {
                        await context.SubjectTags.AddAsync(new SubjectTag
                        {
                            SubjectId = subject.Id,
                            TagId = tags[i].Id
                        });
                    }
                }
                await context.SaveChangesAsync();
            }
        }
        private static async Task PackageSeed(SmartStartDbContext context)
        {
            if (!context.Packages.Where(e => !e.DateDeleted.HasValue).Any())
            {
                string[] packages = { "كلية الهندسة المعلوماتية", "كلية الهندسة الزراعية" };

                for (int i = 0; i < 3 * packages.Length; i++)
                {
                    var faculity = await context.Faculties.SingleOrDefaultAsync(f => !f.DateDeleted.HasValue
                                                                                  && f.Name == packages[i % 2]);

                    var package = new Package
                    {
                        Description = (i > 3? "تحوي الحزمة مواد " : "تحوي الحزمة بعض مواد ")+ packages[i % 2],
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(RandomInteger(30, 60)),
                        IsHidden = false,
                        Name = "حزمة " + packages[i % 2] + (i > 3? " الكلية" : " الجزئية " + (i + 2) / 2),
                        Price = context.SubjectFaculties.Where(s => !s.DateDeleted.HasValue
                                                                 && s.FacultyId == faculity.Id)
                                                        .Take((i + 2) / 2)
                                                        .Sum(s => s.Price),
                        Type = PackageTypes.Auto
                    };
                    await context.Packages.AddAsync(package);
                    await context.SaveChangesAsync();
                    var subjectFaculities = await context.SubjectFaculties.Where(sf => !sf.DateDeleted.HasValue
                                                                                    && sf.FacultyId == faculity.Id)
                                                                          .Take((i + 2) / 2)
                                                                          .ToListAsync();
                    var res = new List<PackageSubjectFaculty>();
                    foreach (var subjectFaculity in subjectFaculities)
                    {
                        res.Add(new PackageSubjectFaculty
                        {
                            PackageId = package.Id,
                            Price = subjectFaculity.Price,
                            SubjectFacultyId = subjectFaculity.Id
                        });
                    }
                    await context.AddRangeAsync(res);
                }
                await context.SaveChangesAsync();
            }
        }
        private static async Task QuestionSeed(SmartStartDbContext context)
        {
            if (!context.Questions.Where(e => !e.DateDeleted.HasValue).Any())
            {
                var res = new List<Question>();

                await context.AddAsync(new Question
                {
                    DateCreated = DateTime.Now,
                    AnswerType = AnswerTypes.MultiChoice,
                    Hint = "تذكر التشريح كلمة عربية",
                    QuestionType = QuestionTypes.Multi,
                    Title = "يعتبر الحمص من النباتات",
                    Answers = new List<Answer>()
                    {
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "النهار الطويل"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "النهار القصير"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "المحايدة"
                        },
                        new Answer()
                        {
                            IsCorrect = true,
                            Title = "جميع الإجابات صحيحة"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "جميع الإجابات خاطئة"
                        },
                    }
                });
                await context.AddAsync(new Question
                {
                    DateCreated = DateTime.Now,
                    AnswerType = AnswerTypes.MultiChoice,
                    QuestionType = QuestionTypes.Multi,
                    Title = "يزرع العدس في معظم المحافظات السورية	",
                    Answers = new List<Answer>()
                    {
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "شهر تشرين الثاني"
                        },
                        new Answer()
                        {
                            IsCorrect = true,
                            Title = "عروة ربيعية"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "في منتصف شهر نيسان"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "أيلول"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "جميع الإجابات خاطئة"
                        },
                    }
                });
                await context.AddAsync(new Question
                {
                    DateCreated = DateTime.Now,
                    AnswerType = AnswerTypes.MultiChoice,
                    QuestionType = QuestionTypes.Multi,
                    Title = "التقيد بموعد زراعة القطن يؤثر في",
                    Answers = new List<Answer>()
                    {
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "تجنب تاثير موجات الحر العالية خلال شهر تموز واب"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "الحصول على اقطان نظيفة"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "الإجابة A و B"
                        },
                        new Answer()
                        {
                            IsCorrect = true,
                            Title = "إضافة اسمدة"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "تقليل الري"
                        },
                    }
                });
                await context.AddAsync(new Question
                {
                    DateCreated = DateTime.Now,
                    AnswerType = AnswerTypes.MultiChoice,
                    QuestionType = QuestionTypes.Multi,
                    Title = "يتم ترقيع القطن",
                    Answers = new List<Answer>()
                    {
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "يعد15 يوما من الزراعة"
                        },
                        new Answer()
                        {
                            IsCorrect = true,
                            Title = "أيام من الزراعة"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "بعد التعشيب"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "بعد التفريد"
                        },
                        new Answer()
                        {
                            IsCorrect = false,
                            Title = "جميع الإجابات خاطئة"
                        },
                    }
                });
                for (int i = 0; i < 56; i++)
                {
                    var idx = RandomInteger(0, 4);
                    await context.AddAsync(new Question
                    {
                        DateCreated = DateTime.Now,
                        AnswerType = AnswerTypes.MultiChoice,
                        QuestionType = QuestionTypes.Multi,
                        Title = RandomString(35),
                        Answers = new List<Answer>()
                        {
                            new Answer()
                            {
                                IsCorrect = (idx == 0),
                                Title = RandomString(RandomInteger(12, 20))
                            },
                            new Answer()
                            {
                                IsCorrect = (idx == 1),
                                Title = RandomString(RandomInteger(12, 20))
                            },
                            new Answer()
                            {
                                IsCorrect = (idx == 2),
                                Title = RandomString(RandomInteger(12, 20))
                            },
                            new Answer()
                            {
                                IsCorrect = (idx == 3),
                                Title = RandomString(RandomInteger(12, 20))
                            },
                            new Answer()
                            {
                                IsCorrect = (idx == 4),
                                Title = RandomString(RandomInteger(12, 20))
                            },
                        }
                    });
                }
                for (int i = 0; i < 20; i++)
                {
                    await context.AddAsync(new Question
                    {
                        DateCreated = DateTime.Now,
                        AnswerType = AnswerTypes.Text,
                        QuestionType = QuestionTypes.Single,
                        Title = RandomString(RandomInteger(30, 40)),
                        Answers = new List<Answer>()
                        {
                            new Answer()
                            {
                                Title = RandomString(RandomInteger(50, 70))
                            },
                        }
                    });
                }
                await context.SaveChangesAsync();
            }
        }
        private static async Task ExamAndBankSeed(SmartStartDbContext context)
        {
            if (!context.Exams.Where(e => !e.DateDeleted.HasValue && (e.Type == TabTypes.Bank || e.Type == TabTypes.Exam)).Any())
            {
                var questions = context.Questions.Where(q => !q.DateDeleted.HasValue
                                                          && q.AnswerType == AnswerTypes.MultiChoice);
                var res = new List<ExamQuestion>();
                var exam = new Exam
                {
                    DateCreated = DateTime.Now,
                    Name = "فحص البغاضة",
                    SubjectId = context.Subjects.Where(s => !s.DateDeleted.HasValue).Skip(RandomInteger(0, context.Subjects.Where(s => !s.DateDeleted.HasValue).Count() - 1)).First().Id,
                    Type = TabTypes.Exam,
                    Year = 2020,
                };
                short i = 1; 
                await context.Exams.AddAsync(exam);
                await context.SaveChangesAsync(); 
                foreach (var item in questions.Take(30))
                {
                    res.Add(new ExamQuestion
                    {
                        ExamId = exam.Id,
                        QuestionId = item.Id,
                        Order = i
                    });
                    i++; 
                }
                var bank = new Exam
                {
                    DateCreated = DateTime.Now,
                    Name = "بنك البغاضة",
                    SubjectId = context.Subjects.Where(s => !s.DateDeleted.HasValue).Skip(RandomInteger(0, context.Subjects.Where(s => !s.DateDeleted.HasValue).Count() - 1)).First().Id,
                    Type = TabTypes.Bank,
                    Year = 2019,
                };
                i = 1;
                await context.Exams.AddAsync(bank);
                foreach (var item in questions.Skip(30).Take(30))
                {
                    res.Add(new ExamQuestion
                    {
                        ExamId = bank.Id,
                        QuestionId = item.Id,
                        Order = i
                    });
                    i++;
                }
                await context.ExamQuestions.AddRangeAsync(res);
                await context.SaveChangesAsync();
            }
        }
        private static async Task InterviewSeed(SmartStartDbContext context)
        {
            if (!context.Exams.Where(e => !e.DateDeleted.HasValue && (e.Type == TabTypes.Interview)).Any())
            {
                var questions = context.Questions.Where(q => !q.DateDeleted.HasValue
                                                          && q.AnswerType == AnswerTypes.Text);
                var res = new List<ExamQuestion>();
                var exam = new Exam
                {
                    DateCreated = DateTime.Now,
                    Name = "مقابلة البغاضة",
                    SubjectId = context.Subjects.Where(s => !s.DateDeleted.HasValue).Skip(RandomInteger(0, context.Subjects.Where(s => !s.DateDeleted.HasValue).Count() - 1)).First().Id,
                    Type = TabTypes.Interview,
                    Year = 2020,
                };
                short i = 1;
                await context.Exams.AddAsync(exam);
                await context.SaveChangesAsync();
                foreach (var item in questions)
                {
                    res.Add(new ExamQuestion
                    {
                        ExamId = exam.Id,
                        QuestionId = item.Id,
                        Order = i
                    });
                    i++;
                }
                await context.ExamQuestions.AddRangeAsync(res);
                await context.SaveChangesAsync();
            }
        }


        #region Helper Functions
        public static int RandomInteger(int start, int end)
        {
            return random.Next(start, end + 1);
        }
        public static string PhoneNumberGenerator(int length = 8)
        {
            const string chars = "0123456789";
            return "09" + new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789   ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion

    }
}
