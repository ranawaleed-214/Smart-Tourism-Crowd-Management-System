using System;
using System.Collections.Generic;
using TourEgypt.Models;

namespace TourEgypt.Services
{
    public class BookingManager
    {
        private List<Booking> _bookings;

        public BookingManager()
        {
            _bookings = new List<Booking>();
        }

        public TourBooking BookTour(string touristName, DateTime date, double price, string tourName, int durationHours)
        {
            var booking = new TourBooking(touristName, date, price, tourName, durationHours);
            _bookings.Add(booking);
            FileHandler.SaveBooking(booking);
            return booking;
        }

        public TicketBooking BookTicket(string touristName, DateTime date, double price, string attraction, int qty)
        {
            var booking = new TicketBooking(touristName, date, price, attraction, qty);
            _bookings.Add(booking);
            FileHandler.SaveBooking(booking);
            return booking;
        }

        public GuideBooking BookGuide(string touristName, DateTime date, double price, string guideName, string language)
        {
            var booking = new GuideBooking(touristName, date, price, guideName, language);
            _bookings.Add(booking);
            FileHandler.SaveBooking(booking);
            return booking;
        }

        public bool ProcessPayment(Booking booking, string method)
        {
            var payment = new Payments(booking.BookingId, booking.Price, method);
            bool success = payment.ProcessPayment();
            if (success)
            {
                booking.IsPaid = true;
                FileHandler.LogActivity("Payment processed for booking #" + booking.BookingId);
            }
            return success;
        }

        public List<Booking> GetBookingsByTourist(string touristName)
        {
            List<Booking> result = new List<Booking>();
            foreach (var b in _bookings)
                if (b.TouristName.Equals(touristName, StringComparison.OrdinalIgnoreCase))
                    result.Add(b);
            return result;
        }

        public double GetTotalRevenue()
        {
            double total = 0;
            foreach (var b in _bookings)
                if (b.IsPaid) total += b.Price;
            return total;
        }

        public double GetTotalCommission()
        {
            double total = 0;
            foreach (var b in _bookings)
                if (b.IsPaid) total += b.CalculateCommission();
            return total;
        }

        public List<Booking> GetAllBookings()
        {
            return _bookings;
        }
    }
}
