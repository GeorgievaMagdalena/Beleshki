using Beleshki.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace Beleshki.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<BeleshkiStudent>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            BeleshkiStudent user = await UserManager.FindByEmailAsync("admin@beleshki.com");
            if (user == null)
            {
                var User = new BeleshkiStudent();
                User.Email = "admin@beleshki.com";
                User.UserName = "admin@beleshki.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new BeleshkiContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BeleshkiContext>>());

            CreateUserRoles(serviceProvider).Wait();

            if (context.Beleshka.Any() && context.Predmet.Any() && context.Fakultet.Any() && context.Komentar.Any() && context.StudentBeleshki.Any() && context.PredmetFakultet.Any())
            {
                return; 
            }

            context.Predmet.AddRange(
                    new Predmet { /*Id = 1, */ PredmetIme = "Digitalni Telekomunikacii 1", Kod = "FEIT-11", Krediti = 6, Institut = "TKII", StudiskaGodina = "III (treta)" },
                    new Predmet { /*Id = 2, */ PredmetIme = "Histologija i embriologija", Kod = "MEDF-22", Krediti = 5, Institut = "Histologija", StudiskaGodina = "I (prva)" },
                    new Predmet { /*Id = 3, */ PredmetIme = "Fizika 2", Kod = "GF-33", Krediti = 6, Institut = "Fizika", StudiskaGodina = "II (vtora)" }
                );
            context.SaveChanges();


            context.Beleshka.AddRange(
                new Beleshka { BeleshkaIme = "Impulsno Kodna Modulacija", Opis = "Auditoriski vezhbi 5 - resheni", DatumKreiranje = DateTime.Parse("2022-11-12"), DownloadUrl = "https://en.wikipedia.org/wiki/Pulse-code_modulation", PredmetId= 1},
                new Beleshka { BeleshkaIme = "Voved vo Histologija", Opis = "Beleshki od lekcija", DatumKreiranje = DateTime.Parse("2022-02-05"), DownloadUrl = "https://en.wikipedia.org/wiki/Histology", PredmetId = 2},
                new Beleshka { BeleshkaIme = "Mehanika na Fluidi", Opis = "Predavanje", DatumKreiranje = DateTime.Parse("2022-10-15"), DownloadUrl = "https://en.wikipedia.org/wiki/Fluid_mechanics", PredmetId = 3 }
            );
            context.SaveChanges();

            context.Fakultet.AddRange(
                    new Fakultet { /*Id = 1, */FakultetIme = "Fakultet za elektrotehnika i informaciski tehnologii", UniverzitetIme = "UKIM", LogoURL = "https://studenti.mk/wp-content/uploads/2017/06/FEIT-logo.png" },
                    new Fakultet { /*Id = 2, */FakultetIme = "Medicinski fakultet", UniverzitetIme = "UKIM", LogoURL = "https://media.licdn.com/dms/image/C4E0BAQGxnDStc0DxyA/company-logo_200_200/0/1602987641779?e=2147483647&v=beta&t=6AWgephxUit3EPMNY96o0AxPL7lB4e_DdTZw9ZMsAR8" },
                    new Fakultet { /*Id = 2, */FakultetIme = "Gradezhen fakultet", UniverzitetIme = "UKIM", LogoURL = "https://upload.wikimedia.org/wikipedia/mk/thumb/c/c1/Logo_GF.svg/1200px-Logo_GF.svg.png" }
                );
            context.SaveChanges();

            context.Komentar.AddRange(
                    new Komentar { StudentIme = "student1@student.com", komentar = "Greshka vo 3-tata zadacha.", Ocenka = 4, BeleshkaId = 1},
                    new Komentar { StudentIme = "student2@student.com", komentar = "Fala!", Ocenka = 5, BeleshkaId = 2 }
                );
            context.SaveChanges();

            context.StudentBeleshki.AddRange(
                    new StudentBeleshki { StudentIme = "student1@student.com", BeleshkaId = 1},
                    new StudentBeleshki { StudentIme = "student2@student.com", BeleshkaId = 2}
                );
            context.SaveChanges();

            context.PredmetFakultet.AddRange(
                    new PredmetFakultet { PredmetId = 1, FakultetId = 1},
                    new PredmetFakultet { PredmetId = 2, FakultetId = 2},
                    new PredmetFakultet { PredmetId = 3, FakultetId = 3}
                );
            context.SaveChanges();




        }
    }
}
