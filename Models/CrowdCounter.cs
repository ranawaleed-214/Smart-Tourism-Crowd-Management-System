using System;

namespace TourEgypt.Models
{
    // ==================== CROWD COUNTER BASE ====================
    public class CrowdCounter
    {
        public int PeopleEntered { get; protected set; }
        public int PeopleExited { get; protected set; }

        public int CurrentPeople
        {
            get { return PeopleEntered - PeopleExited; }
        }

        public CrowdLevel CrowdLevel
        {
            get
            {
                if (CurrentPeople < 20) return CrowdLevel.Low;
                if (CurrentPeople < 50) return CrowdLevel.Medium;
                if (CurrentPeople < 100) return CrowdLevel.High;
                return CrowdLevel.VeryHigh;
            }
        }

        public virtual void RecordEntry()
        {
            PeopleEntered++;
        }

        public virtual void RecordExit()
        {
            if (PeopleExited < PeopleEntered)
                PeopleExited++;
        }

        public virtual int CalculateCurrentCrowd()
        {
            return CurrentPeople;
        }

        public void Reset()
        {
            PeopleEntered = 0;
            PeopleExited = 0;
        }

        public override string ToString()
        {
            return "Inside: " + CurrentPeople + " | Crowd: " + CrowdLevel + " | Entered: " + PeopleEntered + " | Exited: " + PeopleExited;
        }
    }

    // ==================== PHYSICAL SENSOR ====================
    public class PhysicalSensor : CrowdCounter
    {
        public string ArduinoInput { get; set; }

        public PhysicalSensor() : base()
        {
            ArduinoInput = "USB";
        }

        public void ReceiveSignal(string direction)
        {
            if (direction == "entry")
            {
                PeopleEntered++;
                Console.WriteLine("[Sensor] Entry detected. Total entered: " + PeopleEntered);
            }
            else if (direction == "exit")
            {
                RecordExit();
                Console.WriteLine("[Sensor] Exit detected. Total exited: " + PeopleExited);
            }
        }

        public override int CalculateCurrentCrowd()
        {
            Console.WriteLine("[PhysicalSensor] Reading from Arduino via " + ArduinoInput + "...");
            return base.CalculateCurrentCrowd();
        }
    }

    // ==================== AI CAMERA ====================
    public class AICamera : CrowdCounter
    {
        public string YOLOModel { get; set; }
        public string DetectionInput { get; set; }

        public AICamera() : base()
        {
            YOLOModel = "YOLOv8";
            DetectionInput = "Camera Feed";
        }

        public void ProcessFrame(int detectedPeople)
        {
            int diff = detectedPeople - CurrentPeople;
            if (diff > 0)
                for (int i = 0; i < diff; i++) PeopleEntered++;
            else if (diff < 0)
                for (int i = 0; i < Math.Abs(diff); i++) RecordExit();

            Console.WriteLine("[AI Camera - " + YOLOModel + "] Detected " + detectedPeople + " people in frame.");
        }

        public override int CalculateCurrentCrowd()
        {
            Console.WriteLine("[AICamera] Running " + YOLOModel + " detection on " + DetectionInput + "...");
            return base.CalculateCurrentCrowd();
        }
    }

    // ==================== DATA SIMULATOR ====================
    public class DataSimulator : CrowdCounter
    {
        private static readonly Random _rng = new Random();
        public int RandomGeneratorSeed { get; set; }

        public DataSimulator() : base()
        {
            RandomGeneratorSeed = _rng.Next(1, 9999);
        }

        public void SimulateHour()
        {
            int entered = _rng.Next(5, 40);
            int exited = _rng.Next(0, entered);
            PeopleEntered += entered;
            PeopleExited += exited;
            Console.WriteLine("[Simulator] Hour simulation: +" + entered + " entered, -" + exited + " exited. Now inside: " + CurrentPeople);
        }

        public override int CalculateCurrentCrowd()
        {
            SimulateHour();
            return base.CalculateCurrentCrowd();
        }
    }
}
