using System;

namespace TourEgypt.Models
{
    public enum CrowdLevel
    {
        Low,
        Medium,
        High,
        VeryHigh
    }

    public enum BookingType
    {
        Tour,
        Ticket,
        Guide
    }

    public enum NotificationStatus
    {
        Pending,
        Sent,
        Read
    }

    public enum UserRole
    {
        Tourist,
        Employee,
        Admin
    }
}
