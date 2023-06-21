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
            // Check if AdminData.txt file exists
            if (!Directory.Exists(AdminDataDirectory) || !System.IO.File.Exists(AdminDataFilePath))
            {
                ViewBag.ErrorMessage = "Admin data file not found.";
                return View();
            }

            // Read the lines from AdminData.txt file
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
            // Check if UserData.txt file exists
            if (!Directory.Exists(UserDataDirectory))
            {
                Directory.CreateDirectory(UserDataDirectory);
            }

            // Check if the user already exists in the UserData.txt file
            string[] existingUsers = System.IO.File.ReadAllLines(UserDataFilePath);
            foreach (string existingUser in existingUsers)
            {
                string[] credentials = existingUser.Split(',');
                if (credentials[0] == user.Username)
                {
                    ViewBag.ErrorMessage = "Username already exists.";
                    return View();
                }
            }

            // Write the user credentials to the UserData.txt file
            using (StreamWriter sw = new StreamWriter(UserDataFilePath, true))
            {
                sw.WriteLine($"{user.Username},{user.Password}");
            }

            // Redirect to admin panel after adding the user
            return RedirectToAction("AddUser");
        }
    }
}
