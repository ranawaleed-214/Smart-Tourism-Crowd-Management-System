using System;
using System.Collections.Generic;
using TourEgypt.Models;

namespace TourEgypt.Services
{
    public class UserManager
    {
        private List<User> _users;
        private int _nextId = 1;

        public User CurrentUser { get; private set; }

        public UserManager()
        {
            _users = new List<User>();
            LoadUsersFromFile();
        }

        public Tourist RegisterTourist(string name, string email, string password)
        {
            if (FindByEmail(email) != null)
                throw new InvalidOperationException("Email already registered.");
            var t = new Tourist(_nextId++, name, email, password);
            _users.Add(t);
            FileHandler.SaveUser(t);
            FileHandler.LogActivity("New tourist registered: " + name);
            return t;
        }

        public Employee RegisterEmployee(string name, string email, string password, string dept)
        {
            if (FindByEmail(email) != null)
                throw new InvalidOperationException("Email already registered.");
            var e = new Employee(_nextId++, name, email, password, dept);
            _users.Add(e);
            FileHandler.SaveUser(e);
            return e;
        }

        public Admin RegisterAdmin(string name, string email, string password)
        {
            if (FindByEmail(email) != null)
                throw new InvalidOperationException("Email already registered.");
            var a = new Admin(_nextId++, name, email, password);
            _users.Add(a);
            FileHandler.SaveUser(a);
            return a;
        }

        public User Login(string email, string password)
        {
            var user = FindByEmail(email);
            if (user == null) throw new Exception("User not found.");
            if (!user.Login(email, password)) throw new Exception("Incorrect password.");
            CurrentUser = user;
            FileHandler.LogActivity("User logged in: " + user.Name + " [" + user.Role + "]");
            return user;
        }

        public void Logout()
        {
            if (CurrentUser != null)
            {
                FileHandler.LogActivity("User logged out: " + CurrentUser.Name);
                CurrentUser = null;
            }
        }

        public User FindByEmail(string email)
        {
            foreach (var u in _users)
                if (u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    return u;
            return null;
        }

        public List<User> GetAllUsers()
        {
            return _users;
        }

        private void LoadUsersFromFile()
        {
            var rows = FileHandler.LoadUsers();
            foreach (var row in rows)
            {
                try
                {
                    if (row.Length < 5) continue;
                    int id = int.Parse(row[0]);
                    string roleStr = row[1];
                    string name = row[2], email = row[3], pass = row[4];

                    if (roleStr == "Tourist") _users.Add(new Tourist(id, name, email, pass));
                    else if (roleStr == "Employee") _users.Add(new Employee(id, name, email, pass, "General"));
                    else if (roleStr == "Admin") _users.Add(new Admin(id, name, email, pass));

                    if (id >= _nextId) _nextId = id + 1;
                }
                catch { }
            }
        }
    }
}
