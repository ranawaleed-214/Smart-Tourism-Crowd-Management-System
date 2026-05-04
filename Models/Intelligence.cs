using System;
using System.Collections.Generic;

namespace TourEgypt.Models
{
    //   (TripPlanner)
    public class TripPlanner
    {
        public int TouristId { get; set; }
        public DateTime Date { get; set; }
        public List<TouristAttraction> OrderedAttractionsList { get; set; }

        public TripPlanner(int touristId, DateTime date)
        {
            TouristId = touristId;
            Date = date;
            OrderedAttractionsList = new List<TouristAttraction>();
        }

        // بيخطط اليوم عن طريق ترتيب الأماكن حسب الزحمة
        public void PlanDayBasedOnCrowd(List<TouristAttraction> selectedPlaces)
        {
            List<TouristAttraction> sorted = new List<TouristAttraction>(selectedPlaces);
            sorted.Sort(CompareCrowd);
            OrderedAttractionsList = sorted;
        }

        // دالة مقارنة: بتقارن بين مكانين حسب مستوى الزحمة
        private int CompareCrowd(TouristAttraction a, TouristAttraction b)
        {
            return ((int)a.Counter.CrowdLevel).CompareTo((int)b.Counter.CrowdLevel);
        }

        // بيحسب و يطبع المسار المقترح لليوم
        public void CalculateRecommendedRoute()
        {
            Console.WriteLine("\nRecommended Route for Today:");
            PrintRoute();
        }

        // بيطبع المسار المرتب مع التوقيتات ومستوى الزحمة ووقت الانتظار
        public void PrintRoute()
        {
            string[] timeLabels = { "Morning", "Late Morning", "Afternoon", "Late Afternoon", "Evening", "Night" };

            for (int i = 0; i < OrderedAttractionsList.Count; i++)
            {
                string time = i < timeLabels.Length ? timeLabels[i] : "Stop " + (i + 1);
                var place = OrderedAttractionsList[i];

                Console.WriteLine("  " + (i + 1) + ". " + place.Name +
                    " (" + time + ") | Crowd: " + place.Counter.CrowdLevel +
                    " | Wait: ~" + place.GetEstimatedWaitingTime() + " min");
            }
        }
    }

    //  (IntelligenceEngine)
    public class IntelligenceEngine
    {
        private PlaceList _placeList;

        public IntelligenceEngine(PlaceList placeList)
        {
            _placeList = placeList;
        }

        // بيجيب أفضل وقت لزيارة مكان النهاردة حسب التوقعات
        public string FindBestTimeToVisit(int placeId)
        {
            var place = _placeList.GetById(placeId); 
            if (place == null) return "Place not found."; 
            string bestTime = place.GetBestVisitingTimeToday(); 
            return bestTime;
        }

        // بيقترح مكان بديل أقل زحمة من المكان المزدحم
        public TouristAttraction SuggestAlternativePlaces(int crowdedPlaceId)
        {
            var crowded = _placeList.GetById(crowdedPlaceId);
            if (crowded == null) return null;

            TouristAttraction[] all = _placeList.GetAll();
            if (all.Length == 0) return null; 

            TouristAttraction alternative = null;
            int lowestCrowd = 0;

            foreach (var p in all)
            {
                if (p.PlaceId != crowdedPlaceId)
                {
                    if (alternative == null)
                    {
                        alternative = p;
                        lowestCrowd = (int)p.Counter.CrowdLevel;
                    }
                    else
                    {
                        int crowdVal = (int)p.Counter.CrowdLevel;
                        if (crowdVal < lowestCrowd)
                        {
                            lowestCrowd = crowdVal;
                            alternative = p;
                        }
                    }
                }
            }

            return alternative;
        }

        // بيعمل محاكاة "لو رحت الساعة كذا" و بيعرض الزحمة ووقت الانتظار
        public string WhatIfSimulate(int placeId, int hour)
        {
            var place = _placeList.GetById(placeId); 
            if (place == null) return "Place not found."; 

            string crowdLabel;
            if (place.HourlyCrowdForecast.TryGetValue(hour, out crowdLabel))
            {
                double estimatedWait;
                if (crowdLabel == "Low Crowd")
                    estimatedWait = 5;
                else if (crowdLabel == "Medium Crowd")
                    estimatedWait = 15;
                else if (crowdLabel == "High Crowd")
                    estimatedWait = 30;
                else if (crowdLabel == "Very High Crowd")
                    estimatedWait = 45;
                else
                    estimatedWait = 10;

                return hour + ":00 -> " + crowdLabel + " | ~" + estimatedWait + " min wait";
            }
            return "No data for this hour."; 
        }

        // بيجيب حالة الزحمة الحالية للمكان
        public string GetCrowdStatus(int placeId)
        {
            var place = _placeList.GetById(placeId);
            if (place == null) return "Place not found.";

            switch (place.Counter.CrowdLevel)
            {
                case CrowdLevel.Low:
                    return $"Crowd is LOW now at {place.Name}! Great time to visit.";
                case CrowdLevel.Medium:
                    return $"{place.Name} is currently MEDIUM crowd. Consider visiting later.";
                case CrowdLevel.High:
                    return $"{place.Name} is currently HIGH crowd. Better to wait.";
                case CrowdLevel.VeryHigh:
                    return $"{place.Name} is VERY HIGH crowd now. Avoid visiting at this time.";
                default:
                    return $"{place.Name} crowd status unknown.";
            }
        }
    }
}
