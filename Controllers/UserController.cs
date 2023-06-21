using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TodoApp.Controllers
{
    public class UserController : Controller
    {
        private readonly string UserDataFilePath = Path.Combine("Data", "UserData.txt");
        private readonly string TodoDataFilePath = Path.Combine("Data", "TodoData.txt");

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            string[] allLines = System.IO.File.ReadAllLines(UserDataFilePath);
            foreach (string line in allLines)
            {
                string[] credentials = line.Split(',');
                if (credentials.Length == 2 && credentials[0] == username && credentials[1] == password)
                {
                    HttpContext.Session.SetString("Username", username);
                    return RedirectToAction("AddTodo");
                }
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        public IActionResult AddTodo()
        {
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            List<string> todos = GetTodos(username);
            return View(todos);
        }

        [HttpPost]
        public IActionResult AddTodoItem(string todo)
        {
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            string todoItem = $"{username}:{todo}{System.Environment.NewLine}";

            using (StreamWriter writer = new StreamWriter(TodoDataFilePath, append: true, encoding: Encoding.UTF8))
            {
                writer.Write(todoItem);
            }

            return RedirectToAction("AddTodo");
        }

        // search for a todo
        [HttpPost]
        public IActionResult SearchTodo(string searchTerm)
        {
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            bool todoExists = GetTodos(username).Any(todo => todo == searchTerm);

            if (todoExists)
            {
                ViewBag.SearchResult = "Todo exists.";
            }
            else
            {
                ViewBag.SearchResult = "Todo does not exist.";
            }

            List<string> todos = GetTodos(username);
            return View("AddTodo", todos);
        }



        // Display todos

        public IActionResult DisplayTodos()
        {
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            List<string> todos = GetTodos(username);
            return View(todos);
        }

        public IActionResult ClearTodos()
        {
            string username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            List<string> remainingTodos = new List<string>();

            using (StreamReader reader = new StreamReader(TodoDataFilePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] todoData = line.Split(':');
                    if (todoData.Length >= 2 && todoData[0] != username)
                    {
                        remainingTodos.Add(line);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(TodoDataFilePath, false, Encoding.UTF8))
            {
                foreach (string todo in remainingTodos)
                {
                    writer.WriteLine(todo);
                }
            }

            return RedirectToAction("AddTodo");
        }

        private List<string> GetTodos(string username)
        {
            List<string> todos = new List<string>();

            using (StreamReader reader = new StreamReader(TodoDataFilePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] todoData = line.Split(':');
                    if (todoData.Length >= 2 && todoData[0] == username)
                    {
                        string todo = string.Join(":", todoData.Skip(1));
                        todos.Add(todo);
                    }
                }
            }

            return todos;
        }
    }
}
