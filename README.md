# TodoApp

TodoApp is a simple todo application developed using C# and .NET framework. It allows users to manage their todos by adding, searching, and clearing them.

## Getting Started

To run the TodoApp locally on your machine, make sure you have the following prerequisites installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version X.X or later)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) or any other compatible code editor

### Installation

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/your-username/TodoApp.git
   ```

2. Open the solution file (TodoApp.sln) in Visual Studio or your preferred code editor.

3. Build the solution to restore the NuGet packages.

4. Run the application by pressing F5 or using the debugging options in Visual Studio.

5. Open your web browser and navigate to http://localhost:<port>/User/Login to access the TodoApp.

### Features

- User authentication: Users can log in using their username and password.
- Add Todo: Logged-in users can add new todos.
- Search Todo: Users can search for specific todos within their existing list of todos.
- Display Todos: Users can view their list of todos.
- Clear Todos: Users can clear all their todos at once.

### Project Structure

The project follows the following structure:

- `TodoApp/Controllers`: Contains the controllers that handle the application's routes and logic.
- `TodoApp/Views`: Contains the Razor views responsible for rendering the application's user interface.
- `TodoApp/Data`: Contains the data files used to store user information and todos.
- `TodoApp/Models`: Contains the models used to represent user and todo data.
