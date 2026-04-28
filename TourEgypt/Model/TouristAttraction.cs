using System;
using System.Collections.Generic;

namespace TourEgypt.Models
{
    // ==================== REVIEW ====================
    public class Review
    {
        public string TouristName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public Review(string touristName, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException("Rating must be between 1 and 5.");
            TouristName = touristName;
            Rating = rating;
            Comment = comment;
            ReviewDate = DateTime.Now;
        }

        public override string ToString()
        {
            return Rating + "/5 by " + TouristName + ": \"" + Comment + "\" [" + ReviewDate.ToString("dd/MM/yyyy") + "]";
        }
    }

    // ==================== TOURIST ATTRACTION ====================
    public class TouristAttraction
    {
        public int PlaceId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public double AverageServiceTime { get; set; }
        public int ServiceCounters { get; set; }
        public List<Review> Reviews { get; set; }
        public CrowdCounter Counter { get; set; }
        public Dictionary<int, string> HourlyCrowdForecast { get; set; }

        public TouristAttraction(int placeId, string name, string location, string description,
                                  TimeSpan openTime, TimeSpan closeTime,
                                  double avgServiceTime, int serviceCounters)
        {
            PlaceId = placeId;
            Name = name;
            Location = location;
            Description = description;
            OpenTime = openTime;
            CloseTime = closeTime;
            AverageServiceTime = avgServiceTime;
            ServiceCounters = serviceCounters;
            Reviews = new List<Review>();
            Counter = new DataSimulator();
            HourlyCrowdForecast = GenerateDefaultForecast();
        }

        public double GetEstimatedWaitingTime()
        {
            if (ServiceCounters <= 0) return 0;
            return Math.Round((double)Counter.CurrentPeople / ServiceCounters * AverageServiceTime, 1);
        }

        public string GetBestVisitingTimeToday()
        {
            foreach (var h in HourlyCrowdForecast)
            {
                if (h.Value == "Low Crowd")
                    return h.Key + ":00";
            }
            return "No low-crowd time found today.";
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        public double GetAverageRating()
        {
            if (Reviews.Count == 0) return 0;
            double total = 0;
            foreach (var r in Reviews) total += r.Rating;
            return Math.Round(total / Reviews.Count, 1);
        }

        private Dictionary<int, string> GenerateDefaultForecast()
        {
            Dictionary<int, string> forecast = new Dictionary<int, string>();
            forecast.Add(8,  "Low Crowd");
            forecast.Add(9,  "Low Crowd");
            forecast.Add(10, "Medium Crowd");
            forecast.Add(11, "High Crowd");
            forecast.Add(12, "Very High Crowd");
            forecast.Add(13, "Very High Crowd");
            forecast.Add(14, "High Crowd");
            forecast.Add(15, "Medium Crowd");
            forecast.Add(16, "Medium Crowd");
            forecast.Add(17, "Low Crowd");
            forecast.Add(18, "Low Crowd");
            return forecast;
        }

        public override string ToString()
        {
            return "[" + PlaceId + "] " + Name + " | " + Location +
                   " | Crowd: " + Counter.CrowdLevel +
                   " | Wait: ~" + GetEstimatedWaitingTime() + " min" +
                   " | Rating: " + GetAverageRating();
        }
    }

    // ==================== PLACE LIST (Array of Objects) ====================
    public class PlaceList
    {
        private TouristAttraction[] _places;
        private int _count;
        private const int MaxCapacity = 100;

        public int Count { get { return _count; } }

        public PlaceList()
        {
            _places = new TouristAttraction[MaxCapacity];
            _count = 0;
        }

        public void AddPlace(TouristAttraction place)
        {
            if (_count >= MaxCapacity)
                throw new InvalidOperationException("Place list is full.");
            _places[_count++] = place;
        }

        public bool RemovePlace(int placeId)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_places[i].PlaceId == placeId)
                {
                    for (int j = i; j < _count - 1; j++)
                        _places[j] = _places[j + 1];
                    _places[--_count] = null;
                    return true;
                }
            }
            return false;
        }

        public TouristAttraction GetById(int placeId)
        {
            for (int i = 0; i < _count; i++)
                if (_places[i].PlaceId == placeId) return _places[i];
            return null;
        }

        public TouristAttraction[] GetAll()
        {
            TouristAttraction[] result = new TouristAttraction[_count];
            Array.Copy(_places, result, _count);
            return result;
        }

        public TouristAttraction[] GetByName(string keyword)
        {
            List<TouristAttraction> results = new List<TouristAttraction>();
            for (int i = 0; i < _count; i++)
                if (_places[i].Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    results.Add(_places[i]);
            return results.ToArray();
        }
    }
}
