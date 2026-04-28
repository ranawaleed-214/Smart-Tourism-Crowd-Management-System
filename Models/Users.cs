using System;
using System.Collections.Generic;

namespace TourEgypt.Models
{
    // ==================== ABSTRACT USER ====================
    public abstract class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; protected set; }

        public User(int userId, string name, string email, string password)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Password = password;
        }

        public virtual bool Login(string email, string password)
        {
            return Email == email && Password == password;
        }

        public virtual bool Register(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("All fields are required.");
            Name = name;
            Email = email;
            Password = password;
            return true;
        }

        public override string ToString()
        {
            return "[" + Role + "] " + Name + " | " + Email;
        }
    }

    // ==================== TOURIST ====================
    public class Tourist : User
    {
        public List<string> Wishlist { get; set; }
        public TouristProfile Profile { get; set; }

        public Tourist(int userId, string name, string email, string password)
            : base(userId, name, email, password)
        {
            Role = UserRole.Tourist;
            Wishlist = new List<string>();
            Profile = new TouristProfile(userId);
        }

        public void BrowsePlaces(List<TouristAttraction> places)
        {
            Console.WriteLine("\n=== Places Available for " + Name + " ===");
            foreach (var place in places)
                Console.WriteLine(place.ToString());
        }

        public void AddToWishlist(string placeName)
        {
            if (!Wishlist.Contains(placeName))
            {
                Wishlist.Add(placeName);
                Console.WriteLine("'" + placeName + "' added to wishlist.");
            }
        }

        public void ShowWishlist()
        {
            Console.WriteLine("\n=== " + Name + "'s Wishlist ===");
            if (Wishlist.Count == 0)
                Console.WriteLine("Wishlist is empty.");
            else
                foreach (var item in Wishlist)
                    Console.WriteLine("- " + item);
        }
    }

    // ==================== EMPLOYEE ====================
    public class Employee : User
    {
        public string Department { get; set; }

        public Employee(int userId, string name, string email, string password, string department)
            : base(userId, name, email, password)
        {
            Role = UserRole.Employee;
            Department = department;
        }

        public void CallNextTourist(Queue queue)
        {
            var next = queue.DequeueNext();
            if (next != null)
                Console.WriteLine("Employee " + Name + " is now serving ticket #" + next.TicketNumber);
            else
                Console.WriteLine("Queue is currently empty.");
        }
    }

    // ==================== ADMIN ====================
    public class Admin : User
    {
        public Admin(int userId, string name, string email, string password)
            : base(userId, name, email, password)
        {
            Role = UserRole.Admin;
        }

        public void ManagePlaces(PlaceList placeList, TouristAttraction place, string action)
        {
            try
            {
                if (action == "add")
                    placeList.AddPlace(place);
                else if (action == "remove")
                    placeList.RemovePlace(place.PlaceId);
                Console.WriteLine("Admin " + Name + ": Place '" + place.Name + "' " + action + "ed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Admin error: " + ex.Message);
            }
        }

        public void ViewReports(List<TouristAttraction> places)
        {
            Console.WriteLine("\n========== ADMIN REPORT ==========");
            foreach (var place in places)
            {
                Console.WriteLine("Place: " + place.Name);
                Console.WriteLine("  Crowd: " + place.Counter.CrowdLevel + " | People Inside: " + place.Counter.CurrentPeople);
                Console.WriteLine("  Est. Wait: " + place.GetEstimatedWaitingTime() + " min");
                Console.WriteLine("-----------------------------------");
            }
        }
    }
}
