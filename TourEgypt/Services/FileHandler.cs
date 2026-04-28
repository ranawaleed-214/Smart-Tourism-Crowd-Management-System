using System;
using System.Collections.Generic;
using System.IO;
using TourEgypt.Models;

namespace TourEgypt.Services
{
    public static class FileHandler
    {
        private static readonly string DataFolder = "data";
        private static readonly string UsersFile = Path.Combine(DataFolder, "users.txt");
        private static readonly string BookingsFile = Path.Combine(DataFolder, "bookings.txt");
        private static readonly string ReviewsFile = Path.Combine(DataFolder, "reviews.txt");
        private static readonly string QueueFile = Path.Combine(DataFolder, "queue.txt");
        private static readonly string LogFile = Path.Combine(DataFolder, "log.txt");

        static FileHandler()
        {
            if (!Directory.Exists(DataFolder))
                Directory.CreateDirectory(DataFolder);
        }

        // USERS 
        public static void SaveUser(User user)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(UsersFile, true))
                {
                    sw.WriteLine(user.UserId + "|" + user.Role + "|" + user.Name + "|" + user.Email + "|" + user.Password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FileHandler] Error saving user: " + ex.Message);
            }
        }

        public static List<string[]> LoadUsers()
        {
            List<string[]> result = new List<string[]>();
            try
            {
                if (!File.Exists(UsersFile)) return result;
                foreach (var line in File.ReadAllLines(UsersFile))
                    if (!string.IsNullOrWhiteSpace(line))
                        result.Add(line.Split('|'));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FileHandler] Error loading users: " + ex.Message);
            }
            return result;
        }

        // ---- BOOKINGS ----
        public static void SaveBooking(Booking booking)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(BookingsFile, true))
                {
                    sw.WriteLine(booking.BookingId + "|" + booking.Type + "|" + booking.TouristName + "|" +
                                 booking.Date.ToString("dd/MM/yyyy") + "|" + booking.Price + "|" + booking.IsPaid + "|" +
                                 booking.GetBookingDetails());
                }
                LogActivity("Booking #" + booking.BookingId + " created for " + booking.TouristName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FileHandler] Error saving booking: " + ex.Message);
            }
        }

        public static List<string[]> LoadBookings()
        {
            List<string[]> result = new List<string[]>();
            try
            {
                if (!File.Exists(BookingsFile)) return result;
                foreach (var line in File.ReadAllLines(BookingsFile))
                    if (!string.IsNullOrWhiteSpace(line))
                        result.Add(line.Split('|'));
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FileHandler] Error loading bookings: " + ex.Message);
            }
            return result;
        }

        // ---- REVIEWS ----
        public static void SaveReview(int placeId, Review review)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ReviewsFile, true))
                {
                    sw.WriteLine(placeId + "|" + review.TouristName + "|" + review.Rating + "|" +
                                 review.Comment + "|" + review.ReviewDate.ToString("dd/MM/yyyy"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FileHandler] Error saving review: " + ex.Message);
            }
        }

        // ---- QUEUE ----
        public static void SaveQueueEntry(TicketQueue ticket)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(QueueFile, true))
                {
                    sw.WriteLine(ticket.TicketNumber + "|" + ticket.TouristName + "|" +
                                 ticket.Position + "|" + ticket.EstTimeSlot.ToString("HH:mm") + "|" +
                                 ticket.NotificationStatus);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FileHandler] Error saving queue: " + ex.Message);
            }
        }

        // ---- LOG ----
        public static void LogActivity(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(LogFile, true))
                {
                    sw.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "] " + message);
                }
            }
            catch { }
        }

        public static string ReadLog()
        {
            try
            {
                if (!File.Exists(LogFile)) return "No logs yet.";
                return File.ReadAllText(LogFile);
            }
            catch (Exception ex)
            {
                return "Could not read log: " + ex.Message;
            }
        }
    }
}
