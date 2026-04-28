using System;
using System.Collections.Generic;

namespace TourEgypt.Models
{
    public class TouristProfile
    {
        public int TouristId { get; set; }
        public double EfficiencyScore { get; set; }
        public int TimeSaved { get; set; }
        public int Points { get; set; }
        public List<string> Badges { get; set; }
        public List<Reward> Rewards { get; set; }

        public TouristProfile(int touristId)
        {
            TouristId = touristId;
            EfficiencyScore = 0;
            TimeSaved = 0;
            Points = 0;
            Badges = new List<string>();
            Rewards = new List<Reward>();
        }

        public void AddPoints(int pts)
        {
            Points += pts;
            Console.WriteLine("You earned " + pts + " points! Total: " + Points);
            CheckBadges();
        }

        public void UpdateEfficiency(int timeSavedMinutes, int totalPossibleMinutes)
        {
            TimeSaved += timeSavedMinutes;
            if (totalPossibleMinutes > 0)
                EfficiencyScore = Math.Round((double)TimeSaved / totalPossibleMinutes * 100, 1);
        }

        private void CheckBadges()
        {
            if (Points >= 100 && !Badges.Contains("Explorer"))
            {
                Badges.Add("Explorer");
                Console.WriteLine("Badge Unlocked: Explorer!");
            }
            if (Points >= 500 && !Badges.Contains("Pro Traveler"))
            {
                Badges.Add("Pro Traveler");
                Console.WriteLine("Badge Unlocked: Pro Traveler!");
            }
            if (Points >= 1000 && !Badges.Contains("Egypt Champion"))
            {
                Badges.Add("Egypt Champion");
                Console.WriteLine("Badge Unlocked: Egypt Champion!");
            }
        }

        public void ShowProfile()
        {
            Console.WriteLine("\n========== TOURIST PROFILE ==========");
            Console.WriteLine("Efficiency Score : " + EfficiencyScore + "%");
            Console.WriteLine("Time Saved       : " + TimeSaved + " minutes");
            Console.WriteLine("Points           : " + Points);
            Console.WriteLine("Badges           : " + (Badges.Count > 0 ? string.Join(", ", Badges) : "None yet"));
            Console.WriteLine("======================================");
        }
    }

    public class Reward
    {
        public int Points { get; set; }
        public string Badge { get; set; }
        public DateTime EarnedDate { get; set; }

        public Reward(int points, string badge)
        {
            Points = points;
            Badge = badge;
            EarnedDate = DateTime.Now;
        }

        public override string ToString()
        {
            return "Reward: " + Badge + " | " + Points + " pts | Earned: " + EarnedDate.ToString("dd/MM/yyyy");
        }
    }
}
