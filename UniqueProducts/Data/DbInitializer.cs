using UniqueProducts.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UniqueProducts.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(UniqueProductsContext db, HttpContext context)
        {
            db.Database.EnsureCreated();

            int single_link = 50;
            int many_link = 100;
            Random random = new Random(1);

            string[] streetNames = { "ул.Профсоюзная", "ул.Ленина", "ул.Пушкинская", "ул.Гагарина", "ул.Московская" };
            string[] cities = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };
            string[] zipCodes = { "123456", "654321", "987654", "456789", "321654" };
            string[] firstNames = { "Иван", "Александр", "Дмитрий", "Антон", "Степан", "Виктор" };
            string[] lastNames = { "Филатов", "Потапов", "Кузнецов", "Поддубный", "Понамарёв", "Степанов" };
            string[] middleNames = { "Иванович", "Александрович", "Дмитриевич", "Андреевич", "Петрович", "Михайлович" };
            string[] positions = { "Экономист", "Товаровед", "Продавец", "Продавец-консультант" };

            var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = context.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();

            //Названия ролевых политик
            var roles = new[] { "SuperAdmin", "Admin", "User" };

            //Логин-пароли для админов
            string superadminEmail = "superadmin@gmail.com";
            string superadminPass = "Superadmin_333";
            string adminEmail = "admin@gmail.com";
            string adminPass = "Admin_333";
            
            // Добавление ролей пользователей
            if (!roleManager.Roles.Any())
            {
                foreach (var roleName in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }

            //Создание администарторов 
            //Admin-просмтор и редактирование таблиц (не может работать с пользователями)
            //SuperAdmin-crud во всех таблицах

            if (!userManager.Users.Any())
            {
                // Создание пользователя с ролью SuperAdmin 
                var superAdminUser = await userManager.FindByEmailAsync(superadminEmail);
                if (superAdminUser == null)
                {
                    superAdminUser = new IdentityUser { UserName = superadminEmail, Email = superadminEmail };
                    await userManager.CreateAsync(superAdminUser, superadminPass);
                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                }

                // Создание пользователя с ролью Admin 
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                    await userManager.CreateAsync(adminUser, adminPass);
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                // Создание пользователя с ролью User 
                var userEmail = "user@gmail.com";
                var user = await userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    user = new IdentityUser { UserName = userEmail, Email = userEmail };
                    await userManager.CreateAsync(user, "User_333");
                    await userManager.AddToRoleAsync(user, "User");
                }
            };
           
            // Генерация случайного адреса (русский язык)
            string GetRandomAddress()
            {
                string street = streetNames[random.Next(streetNames.Length)];
                string city = cities[random.Next(cities.Length)];
                string zipCode = zipCodes[random.Next(zipCodes.Length)];
                return $"{street}, {city}, {zipCode}";
            }

            // Генерация случайного номера телефона в формате +375 __ ___ ____
            string GetRandomPhoneNumber()
            {
                string phoneNumber = "+375 ";
                for (int i = 0; i < 2; i++){phoneNumber += random.Next(0, 9).ToString();}
                for (int i = 0; i < 3; i++){ phoneNumber += random.Next(0, 9).ToString();}
                for (int i = 0; i < 4; i++) { phoneNumber += random.Next(0, 9).ToString(); }
                return phoneNumber;
            }

            // Генерация случайной даты в промежутке от 01.01.2010 до текущей даты
            DateTime GetRandomDate()
            {
                DateTime startDate = new(2010, 1, 1);
                DateTime endDate = DateTime.Now;
                int totalDays = (endDate - startDate).Days;

                return startDate.AddDays(random.Next(totalDays));
            }

            // Генерация случайного числа определенной длины
            string GetRandomNumbers(int length)
            {
                string number = "";

                for (int i = 0; i < length; i++)
                {
                    number += random.Next(0, 9).ToString();
                }

                return number;
            }

            // Заполнение таблицы Клиенты
            if (!db.Clients.Any())
            {
                for (int i = 0; i < many_link; i++)
                {
                    string companyName = $"Company{i + 1}";
                    string representative = $"Representative{i + 1}";
                    string phone = GetRandomPhoneNumber();
                    string address = GetRandomAddress();

                    db.Clients.Add(new Client
                    {
                        Company = companyName,
                        Representative = representative,
                        Phone = phone,
                        CompanyAddress = address
                    });
                }
                db.SaveChanges();
            }

            // Заполнение таблицы Cотрудники
            if (!db.Employees.Any())
            {
                for (int i = 0; i < many_link; i++)
                {
                    db.Employees.Add(new Employee
                    {
                        EmployeeName = firstNames[random.Next(firstNames.Length)],
                        EmployeeSurname = lastNames[random.Next(lastNames.Length)],
                        EmployeeMidname = middleNames[random.Next(middleNames.Length)],
                        EmployeePosition = positions[random.Next(positions.Length)]
                    });
                }
                db.SaveChanges();
            }

            // Заполнение таблицы Материалы
            if (!db.Materials.Any())
            {
                for (int i = 0; i < single_link; i++)
                {
                    string materialName = $"Material{i + 1}";
                    string materialDescription = $"Description{i + 1}";

                    db.Materials.Add(new Material
                    {
                        MaterialName = materialName,
                        MaterialDescript = materialDescription
                    });
                }
                db.SaveChanges();
            }

            // Заполнение таблицы Products
            if (!db.Products.Any())
            {
                int materialCount = db.Materials.Count();

                for (int i = 0; i < single_link; i++)
                {
                    string productName = "Product " + GetRandomNumbers(4);
                    string productDescript = "Description of " + productName;
                    double productWeight = random.NextDouble() * 10;
                    double productDiameter = random.NextDouble() * 5;
                    string productColor = "Color " + GetRandomNumbers(3);
                    int materialId = random.Next(1, materialCount + 1);
                    decimal productPrice = random.Next(100, 1000);

                    db.Products.Add(new Product
                    {
                        ProductName = productName,
                        ProductDescript = productDescript,
                        ProductWeight = (float?)productWeight,
                        ProductDiameter = (float?)productDiameter,
                        ProductColor = productColor,
                        MaterialId = materialId,
                        ProductPrice = productPrice
                    });
                }
                db.SaveChanges();
            }

            // Заполнение таблицы Заказы
            if (!db.Orders.Any())
            {
                int clientCount = db.Clients.Count();
                int productCount = db.Products.Count();
                int employeeCount = db.Employees.Count();

                for (int i = 0; i < many_link; i++)
                {
                    DateTime orderDate = GetRandomDate();
                    int clientId = random.Next(1, clientCount + 1);
                    int productId = random.Next(1, productCount + 1);
                    int orderAmount = random.Next(1, 10);
                    decimal totalPrice = orderAmount * (decimal)db.Products.Find(productId).ProductPrice;
                    int employeeId = random.Next(1, employeeCount + 1);

                    db.Orders.Add(new Order
                    {
                        OrderDate = orderDate,
                        ClientId = clientId,
                        ProductId = productId,
                        OrderAmount = orderAmount,
                        TotalPrice = totalPrice,
                        IsCompleted = random.Next(2) == 0 ? false : true,
                        EmployeeId = employeeId
                    });
                }
                db.SaveChanges();
            }
        }
    }
}