using System;

namespace TourEgypt.Models
{
    // ==================== BASE BOOKING ====================
    public abstract class Booking
    {
        private static int _idCounter = 1;

        public int BookingId { get; set; }
        public BookingType Type { get; protected set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public double CommissionRate { get; set; }
        public string TouristName { get; set; }
        public bool IsPaid { get; set; }

        public Booking(string touristName, DateTime date, double price, double commissionRate)
        {
            BookingId = _idCounter++;
            TouristName = touristName;
            Date = date;
            Price = price;
            CommissionRate = commissionRate;
            IsPaid = false;
        }

        public virtual double CalculateCommission()
        {
            return Math.Round(Price * CommissionRate, 2);
        }

        public abstract string GetBookingDetails();

        public override string ToString()
        {
            return "Booking #" + BookingId + " [" + Type + "] | " + TouristName +
                   " | " + Date.ToString("dd/MM/yyyy") +
                   " | Price: $" + Price +
                   " | Commission: $" + CalculateCommission() +
                   " | Paid: " + IsPaid;
        }
    }

    // ==================== TOUR BOOKING ====================
    public class TourBooking : Booking
    {
        public string TourDetails { get; set; }
        public string TourName { get; set; }
        public int DurationHours { get; set; }

        public TourBooking(string touristName, DateTime date, double price, string tourName, int durationHours)
            : base(touristName, date, price, 0.10)
        {
            Type = BookingType.Tour;
            TourName = tourName;
            DurationHours = durationHours;
            TourDetails = tourName + " - " + durationHours + " hours";
        }

        public override string GetBookingDetails()
        {
            return "Tour: " + TourName + " | Duration: " + DurationHours + "h";
        }
    }

    // ==================== TICKET BOOKING ====================
    public class TicketBooking : Booking
    {
        public string TicketDetails { get; set; }
        public string AttractionName { get; set; }
        public int Quantity { get; set; }

        public TicketBooking(string touristName, DateTime date, double price, string attractionName, int quantity)
            : base(touristName, date, price, 0.05)
        {
            Type = BookingType.Ticket;
            AttractionName = attractionName;
            Quantity = quantity;
            TicketDetails = quantity + " ticket(s) for " + attractionName;
        }

        public override double CalculateCommission()
        {
            return Math.Round(Price * 0.05, 2);
        }

        public override string GetBookingDetails()
        {
            return "Ticket: " + AttractionName + " x" + Quantity;
        }
    }

    // ==================== GUIDE BOOKING ====================
    public class GuideBooking : Booking
    {
        public string GuideDetails { get; set; }
        public string GuideName { get; set; }
        public string Language { get; set; }

        public GuideBooking(string touristName, DateTime date, double price, string guideName, string language)
            : base(touristName, date, price, 0.10)
        {
            Type = BookingType.Guide;
            GuideName = guideName;
            Language = language;
            GuideDetails = "Guide: " + guideName + " | Language: " + language;
        }

        public override string GetBookingDetails()
        {
            return "Guide: " + GuideName + " | Language: " + Language;
        }
    }

    // ==================== PAYMENTS ====================
    public class Payments
    {
        private static int _paymentIdCounter = 1;

        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public double Amount { get; set; }
        public string Method { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }

        public Payments(int bookingId, double amount, string method)
        {
            PaymentId = _paymentIdCounter++;
            BookingId = bookingId;
            Amount = amount;
            Method = method;
            PaymentDate = DateTime.Now;
            IsSuccessful = false;
        }

        public bool ProcessPayment()
        {
            try
            {
                if (Amount <= 0)
                    throw new ArgumentException("Payment amount must be positive.");
                IsSuccessful = true;
                Console.WriteLine("Payment #" + PaymentId + " of $" + Amount + " via " + Method + " processed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Payment failed: " + ex.Message);
                return false;
            }
        }

        public override string ToString()
        {
            return "Payment #" + PaymentId + " | Booking #" + BookingId + " | $" + Amount + " | " + Method + " | " + (IsSuccessful ? "Paid" : "Failed");
        }
    }
}
