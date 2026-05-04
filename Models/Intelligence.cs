using System;
using System.Collections.Generic;

namespace TourEgypt.Models
{
    //  TRIP PLANNER
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

        public void PlanDayBasedOnCrowd(List<TouristAttraction> selectedPlaces)
        {
            // Sort by crowd level ascending (least crowded first)
            List<TouristAttraction> sorted = new List<TouristAttraction>(selectedPlaces);
            sorted.Sort(delegate (TouristAttraction a, TouristAttraction b)
            {
                return ((int)a.Counter.CrowdLevel).CompareTo((int)b.Counter.CrowdLevel);
            });
            OrderedAttractionsList = sorted;
        }

        public void CalculateRecommendedRoute()
        {
            Console.WriteLine("\nRecommended Route for Today:");
            PrintRoute();
        }

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

    //  INTELLIGENCE ENGINE
    public class IntelligenceEngine
    {
        private PlaceList _placeList;

        public IntelligenceEngine(PlaceList placeList)
        {
            _placeList = placeList;
        }

        public string FindBestTimeToVisit(int placeId)
        {
            var place = _placeList.GetById(placeId);
            if (place == null) return "Place not found.";
            string bestTime = place.GetBestVisitingTimeToday();
            return bestTime;
        }

        public TouristAttraction SuggestAlternativePlaces(int crowdedPlaceId)
        {
            var crowded = _placeList.GetById(crowdedPlaceId);
            if (crowded == null) return null;

            TouristAttraction[] all = _placeList.GetAll();
            TouristAttraction alternative = null;
            int lowestCrowd = int.MaxValue;

            foreach (var p in all)
            {
                if (p.PlaceId != crowdedPlaceId)
                {
                    int crowdVal = (int)p.Counter.CrowdLevel;
                    if (crowdVal < lowestCrowd)
                    {
                        lowestCrowd = crowdVal;
                        alternative = p;
                    }
                }
            }
            return alternative;
        }

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

        public string GetCrowdStatus(int placeId)
        {
            var place = _placeList.GetById(placeId);
            if (place == null) return "Place not found.";
            if (place.Counter.CrowdLevel == CrowdLevel.Low)
                return "Crowd is LOW now at " + place.Name + "! Great time to visit.";
            else
                return place.Name + " is currently " + place.Counter.CrowdLevel + ". Consider visiting later.";
        }
    }
}
