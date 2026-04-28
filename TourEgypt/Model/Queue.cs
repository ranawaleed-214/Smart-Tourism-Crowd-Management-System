using System;
using System.Collections.Generic;

namespace TourEgypt.Models
{
    // ==================== TICKET QUEUE ====================
    public class TicketQueue
    {
        private static int _ticketCounter = 1000;

        public int TicketNumber { get; set; }
        public int Position { get; set; }
        public DateTime EstTimeSlot { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public string TouristName { get; set; }

        public TicketQueue(string touristName, int position, DateTime estTimeSlot)
        {
            TicketNumber = _ticketCounter++;
            TouristName = touristName;
            Position = position;
            EstTimeSlot = estTimeSlot;
            NotificationStatus = NotificationStatus.Pending;
        }

        public void UpdatePosition(int newPosition)
        {
            Position = newPosition;
            if (Position <= 3)
                SendSmartNotification();
        }

        public void SendSmartNotification()
        {
            NotificationStatus = NotificationStatus.Sent;
            Console.WriteLine("Notification sent to " + TouristName + ": Only " + Position + " people ahead of you!");
        }

        public override string ToString()
        {
            return "Ticket #" + TicketNumber + " | " + TouristName +
                   " | Position: " + Position +
                   " | Est. Time: " + EstTimeSlot.ToString("HH:mm") +
                   " | Status: " + NotificationStatus;
        }
    }

    // ==================== QUEUE ====================
    public class Queue
    {
        public int QueueId { get; set; }
        public int PlaceId { get; set; }
        private List<TicketQueue> _waitingList;
        public int CounterCount { get; set; }

        public int WaitingCount { get { return _waitingList.Count; } }

        public Queue(int queueId, int placeId, int counterCount)
        {
            QueueId = queueId;
            PlaceId = placeId;
            _waitingList = new List<TicketQueue>();
            CounterCount = counterCount;
        }

        public TicketQueue JoinQueue(string touristName, double avgServiceMinutes)
        {
            int position = _waitingList.Count + 1;
            double waitMinutes = (position / (double)CounterCount) * avgServiceMinutes;
            DateTime estSlot = DateTime.Now.AddMinutes(waitMinutes);
            var ticket = new TicketQueue(touristName, position, estSlot);
            _waitingList.Add(ticket);
            Console.WriteLine(touristName + " joined queue. " + ticket.ToString());
            return ticket;
        }

        public double GetEstimatedWaitTime(double avgServiceMinutes)
        {
            return Math.Round((_waitingList.Count / (double)CounterCount) * avgServiceMinutes, 1);
        }

        public TicketQueue DequeueNext()
        {
            if (_waitingList.Count == 0) return null;
            var next = _waitingList[0];
            _waitingList.RemoveAt(0);
            for (int i = 0; i < _waitingList.Count; i++)
                _waitingList[i].UpdatePosition(i + 1);
            return next;
        }

        public List<TicketQueue> GetWaitingList()
        {
            return _waitingList;
        }
    }

    // ==================== COUNTER ====================
    public class Counter
    {
        public int CounterId { get; set; }
        public int QueueId { get; set; }
        public bool IsOpen { get; set; }
        public int ServedToday { get; set; }

        public Counter(int counterId, int queueId)
        {
            CounterId = counterId;
            QueueId = queueId;
            IsOpen = true;
            ServedToday = 0;
        }

        public void ServeNext(Queue queue)
        {
            if (!IsOpen)
            {
                Console.WriteLine("Counter #" + CounterId + " is closed.");
                return;
            }
            var ticket = queue.DequeueNext();
            if (ticket != null)
            {
                ServedToday++;
                Console.WriteLine("Counter #" + CounterId + " serving: " + ticket.TouristName);
            }
            else
            {
                Console.WriteLine("Counter #" + CounterId + ": No one in queue.");
            }
        }

        public override string ToString()
        {
            return "Counter #" + CounterId + " | Status: " + (IsOpen ? "Open" : "Closed") + " | Served Today: " + ServedToday;
        }
    }
}
