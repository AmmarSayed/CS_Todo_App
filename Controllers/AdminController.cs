// AdminController.cs

using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class AdminController : Controller
    {
        private const string AdminDataFilePath = "Data/AdminData.txt";
        private const string UserDataFilePath = "Data/UserData.txt";
        private static readonly string AdminDataDirectory = Path.GetDirectoryName(AdminDataFilePath);
        private static readonly string UserDataDirectory = Path.GetDirectoryName(UserDataFilePath);

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (!Directory.Exists(AdminDataDirectory) || !System.IO.File.Exists(AdminDataFilePath))
            {
                ViewBag.ErrorMessage = "Admin data file not found.";
                return View();
            }

            string[] adminDataLines = System.IO.File.ReadAllLines(AdminDataFilePath);

            // Check if credentials match
            bool isAdminCredentialsValid = adminDataLines
                .Any(line =>
                {
                    string[] adminData = line.Split(',');
                    return adminData[0] == username && adminData[1] == password;
                });

            if (isAdminCredentialsValid)
            {
                // Redirect to admin panel after successful login
                return RedirectToAction("AddUser");
            }

            // Invalid credentials, show error message
            ViewBag.ErrorMessage = "Invalid username or password";
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (!Directory.Exists(UserDataDirectory))
            {
                Directory.CreateDirectory(UserDataDirectory);
            }

            using (StreamWriter sw = new StreamWriter(UserDataFilePath, true))
            {
                sw.WriteLine($"{user.Username},{user.Password}");
            }

            // Redirect to admin panel after adding the user
            return RedirectToAction("AddUser");
        }
    }
}
