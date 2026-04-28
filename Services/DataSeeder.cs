using System;
using TourEgypt.Models;

namespace TourEgypt.Services
{
    public static class DataSeeder
    {
        public static PlaceList SeedPlaces()
        {
            PlaceList list = new PlaceList();

            var pyramids = new TouristAttraction(1, "Pyramids of Giza", "Giza, Cairo",
                "One of the Seven Wonders of the Ancient World.",
                new TimeSpan(8, 0, 0), new TimeSpan(17, 0, 0), 5, 8);
            ((DataSimulator)pyramids.Counter).SimulateHour();
            pyramids.AddReview(new Review("John Smith", 5, "Absolutely breathtaking!"));
            pyramids.AddReview(new Review("Sara Ali", 4, "Amazing place, very hot though."));
            list.AddPlace(pyramids);

            var museum = new TouristAttraction(2, "Egyptian Museum", "Tahrir Square, Cairo",
                "Home to the world largest collection of ancient Egyptian artifacts.",
                new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0), 3, 6);
            ((DataSimulator)museum.Counter).SimulateHour();
            museum.AddReview(new Review("Anna Lee", 4, "Amazing history, a bit crowded."));
            list.AddPlace(museum);

            var khan = new TouristAttraction(3, "Khan El Khalili", "Islamic Cairo",
                "Famous bazaar and souq in the heart of historic Cairo.",
                new TimeSpan(10, 0, 0), new TimeSpan(22, 0, 0), 2, 4);
            ((DataSimulator)khan.Counter).SimulateHour();
            khan.AddReview(new Review("Mike Johnson", 5, "Best shopping experience ever!"));
            list.AddPlace(khan);

            var luxor = new TouristAttraction(4, "Luxor Temple", "Luxor, Upper Egypt",
                "Ancient temple complex on the east bank of the Nile.",
                new TimeSpan(6, 0, 0), new TimeSpan(22, 0, 0), 4, 5);
            ((DataSimulator)luxor.Counter).SimulateHour();
            list.AddPlace(luxor);

            var saqqara = new TouristAttraction(5, "Saqqara", "Saqqara, Giza",
                "Step Pyramid of Djoser and ancient necropolis.",
                new TimeSpan(8, 0, 0), new TimeSpan(16, 0, 0), 4, 3);
            ((DataSimulator)saqqara.Counter).SimulateHour();
            list.AddPlace(saqqara);

            return list;
        }

        public static UserManager SeedUsers()
        {
            UserManager mgr = new UserManager();
            try { mgr.RegisterTourist("Alice Johnson", "alice@email.com", "pass123"); } catch { }
            try { mgr.RegisterTourist("Bob Hassan", "bob@email.com", "pass123"); } catch { }
            try { mgr.RegisterEmployee("Mohamed Salah", "emp@email.com", "emp123", "Ticket Counter"); } catch { }
            try { mgr.RegisterAdmin("Admin User", "admin@email.com", "admin123"); } catch { }
            return mgr;
        }
    }
}
