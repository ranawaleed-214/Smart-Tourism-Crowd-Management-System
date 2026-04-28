[README.md](https://github.com/user-attachments/files/27182602/README.md)
# TourEgypt — Backend Reference

> This document explains the backend logic for team members working on the UI layer.  
> You do **not** need to read the source code.  
> Everything you need to connect your UI to the backend is here.

---

## Table of Contents

1. [Project Structure](#project-structure)
2. [Enums](#enums)
3. [Models Overview](#models-overview)
   - [User (abstract)](#user-abstract)
   - [Tourist](#tourist)
   - [Employee](#employee)
   - [Admin](#admin)
   - [TouristProfile](#touristprofile)
   - [Reward](#reward)
   - [TouristAttraction](#touristattraction)
   - [Review](#review)
   - [PlaceList](#placelist)
   - [CrowdCounter](#crowdcounter)
   - [PhysicalSensor](#physicalsensor)
   - [AICamera](#aicamera)
   - [DataSimulator](#datasimulator)
   - [TicketQueue](#ticketqueue)
   - [Queue](#queue)
   - [Counter](#counter)
   - [Booking (abstract)](#booking-abstract)
   - [TourBooking](#tourbooking)
   - [TicketBooking](#ticketbooking)
   - [GuideBooking](#guidebooking)
   - [Payments](#payments)
   - [TripPlanner](#tripplanner)
   - [IntelligenceEngine](#intelligenceengine)
4. [Services](#services)
   - [UserManager](#usermanager)
   - [BookingManager](#bookingmanager)
   - [FileHandler](#filehandler)
   - [DataSeeder](#dataseeder)
5. [ID System](#id-system)
6. [Data Storage](#data-storage)
7. [Error Handling](#error-handling)
8. [Inheritance Tree](#inheritance-tree)

---

## Project Structure

```
TourEgypt/
├── Models/
│   ├── Enums.cs              → All enums used across the system
│   ├── Users.cs              → User, Tourist, Employee, Admin
│   ├── TouristProfile.cs     → TouristProfile, Reward (Gamification)
│   ├── TouristAttraction.cs  → TouristAttraction, Review, PlaceList
│   ├── CrowdCounter.cs       → CrowdCounter, PhysicalSensor, AICamera, DataSimulator
│   ├── Queue.cs              → TicketQueue, Queue, Counter
│   ├── Booking.cs            → Booking, TourBooking, TicketBooking, GuideBooking, Payments
│   └── Intelligence.cs       → TripPlanner, IntelligenceEngine
│
└── Services/
    ├── UserManager.cs        → Register, Login, Logout
    ├── BookingManager.cs     → Book tours, tickets, guides + payments
    ├── FileHandler.cs        → Save/Load all data to .txt files
    └── DataSeeder.cs         → Pre-loads sample data for testing
```

---

## Enums

### `CrowdLevel`
| Value | Meaning |
|-------|---------|
| `Low` | Less than 20 people inside |
| `Medium` | 20 – 49 people inside |
| `High` | 50 – 99 people inside |
| `VeryHigh` | 100+ people inside |

### `BookingType`
| Value | Meaning |
|-------|---------|
| `Tour` | A guided tour booking |
| `Ticket` | An entry ticket booking |
| `Guide` | A personal guide booking |

### `UserRole`
| Value | Meaning |
|-------|---------|
| `Tourist` | Regular app user |
| `Employee` | Ticket counter staff |
| `Admin` | System administrator |

### `NotificationStatus`
| Value | Meaning |
|-------|---------|
| `Pending` | Notification not yet sent |
| `Sent` | Notification delivered |
| `Read` | Tourist has read it |

---

## Models Overview

---

### User *(abstract)*

> Base class. Cannot be instantiated directly. Use `Tourist`, `Employee`, or `Admin`.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `UserId` | `int` | Unique user ID (auto-assigned) |
| `Name` | `string` | Full name |
| `Email` | `string` | Login email |
| `Password` | `string` | Login password |
| `Role` | `UserRole` | Tourist / Employee / Admin |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `Login(email, password)` | `bool` | Returns `true` if credentials match |
| `Register(name, email, password)` | `bool` | Sets user data, throws if any field is empty |

---

### Tourist
*Inherits from: `User`*

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `Wishlist` | `List<string>` | Place names the tourist wants to visit |
| `Profile` | `TouristProfile` | Gamification stats (points, badges, etc.) |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `AddToWishlist(placeName)` | `void` | Adds a place to wishlist (no duplicates) |
| `ShowWishlist()` | `void` | Prints wishlist to console |
| `BrowsePlaces(places)` | `void` | Prints all available places |

---

### Employee
*Inherits from: `User`*

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `Department` | `string` | e.g. "Ticket Counter" |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `CallNextTourist(queue)` | `void` | Dequeues next ticket and prints it |

---

### Admin
*Inherits from: `User`*

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `ManagePlaces(placeList, place, action)` | `void` | action = `"add"` or `"remove"` |
| `ViewReports(places)` | `void` | Prints full report of all places to console |

---

### TouristProfile

> Holds gamification data for each tourist. Created automatically when a Tourist is created.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `TouristId` | `int` | Links to the tourist's ID |
| `EfficiencyScore` | `double` | Percentage of time saved (0–100) |
| `TimeSaved` | `int` | Total minutes saved by smart planning |
| `Points` | `int` | Earned points from activity |
| `Badges` | `List<string>` | Unlocked badge names |
| `Rewards` | `List<Reward>` | List of reward objects |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `AddPoints(pts)` | `void` | Adds points and auto-checks for new badges |
| `UpdateEfficiency(saved, total)` | `void` | Recalculates efficiency score |
| `ShowProfile()` | `void` | Prints profile to console |

#### Badge Thresholds
| Points | Badge Unlocked |
|--------|---------------|
| 100 | Explorer |
| 500 | Pro Traveler |
| 1000 | Egypt Champion |

---

### Reward

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `Points` | `int` | Points awarded |
| `Badge` | `string` | Badge name |
| `EarnedDate` | `DateTime` | When it was earned |

---

### TouristAttraction

> Represents a tourist place with crowd data, reviews, and visiting info.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `PlaceId` | `int` | Unique place ID |
| `Name` | `string` | Place name |
| `Location` | `string` | City/area |
| `Description` | `string` | Short description |
| `OpenTime` | `TimeSpan` | Opening time |
| `CloseTime` | `TimeSpan` | Closing time |
| `AverageServiceTime` | `double` | Avg minutes per person at counter |
| `ServiceCounters` | `int` | Number of open service counters |
| `Reviews` | `List<Review>` | Tourist reviews |
| `Counter` | `CrowdCounter` | Live crowd data source |
| `HourlyCrowdForecast` | `Dictionary<int, string>` | Hour → crowd label (e.g. `12 → "Very High Crowd"`) |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `GetEstimatedWaitingTime()` | `double` | Minutes = (CurrentPeople / Counters) × AvgServiceTime |
| `GetBestVisitingTimeToday()` | `string` | Returns first hour where forecast = "Low Crowd" |
| `AddReview(review)` | `void` | Adds a review to the list |
| `GetAverageRating()` | `double` | Average of all review ratings (1–5) |

#### Hourly Forecast Default Values
| Hour | Crowd Level |
|------|-------------|
| 8:00 – 9:00 | Low Crowd |
| 10:00 | Medium Crowd |
| 11:00 | High Crowd |
| 12:00 – 13:00 | Very High Crowd |
| 14:00 | High Crowd |
| 15:00 – 16:00 | Medium Crowd |
| 17:00 – 18:00 | Low Crowd |

---

### Review

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `TouristName` | `string` | Name of reviewer |
| `Rating` | `int` | 1 to 5 stars |
| `Comment` | `string` | Review text |
| `ReviewDate` | `DateTime` | Auto-set to now |

> ⚠️ Throws `ArgumentOutOfRangeException` if rating is not between 1 and 5.

---

### PlaceList

> Stores all tourist places in a fixed array of 100 slots.  
> **This is the Array of Objects requirement.**

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `Count` | `int` | Number of places currently stored |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `AddPlace(place)` | `void` | Adds a place (throws if full) |
| `RemovePlace(placeId)` | `bool` | Removes by ID, returns `false` if not found |
| `GetById(placeId)` | `TouristAttraction` | Returns place or `null` |
| `GetAll()` | `TouristAttraction[]` | Returns all places as array |
| `GetByName(keyword)` | `TouristAttraction[]` | Case-insensitive name search |

---

### CrowdCounter

> Base class for all crowd data sources.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `PeopleEntered` | `int` | Total entries recorded |
| `PeopleExited` | `int` | Total exits recorded |
| `CurrentPeople` | `int` *(computed)* | `PeopleEntered - PeopleExited` |
| `CrowdLevel` | `CrowdLevel` *(computed)* | Auto-calculated from `CurrentPeople` |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `RecordEntry()` | `void` | Increments `PeopleEntered` |
| `RecordExit()` | `void` | Increments `PeopleExited` (won't go negative) |
| `CalculateCurrentCrowd()` | `int` | Returns `CurrentPeople` |
| `Reset()` | `void` | Resets both counters to 0 |

---

### PhysicalSensor
*Inherits from: `CrowdCounter`*

> Simulates Arduino sensor input via USB.

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `ArduinoInput` | `string` | Connection type (default: `"USB"`) |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `ReceiveSignal(direction)` | `void` | direction = `"entry"` or `"exit"` |

---

### AICamera
*Inherits from: `CrowdCounter`*

> Simulates YOLO-based AI camera detection.

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `YOLOModel` | `string` | Model name (default: `"YOLOv8"`) |
| `DetectionInput` | `string` | Input source (default: `"Camera Feed"`) |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `ProcessFrame(detectedPeople)` | `void` | Updates counters based on detected count |

---

### DataSimulator
*Inherits from: `CrowdCounter`*

> Generates random realistic crowd data. Used when hardware is unavailable.

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `RandomGeneratorSeed` | `int` | Random seed for reproducibility |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `SimulateHour()` | `void` | Generates random entries and exits (exits always ≤ entries) |
| `CalculateCurrentCrowd()` | `int` | Calls `SimulateHour()` then returns current count |

---

### TicketQueue

> One entry in the queue — one tourist's virtual ticket.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `TicketNumber` | `int` | Auto-increments from 1000 |
| `TouristName` | `string` | Name of the tourist |
| `Position` | `int` | Current position in queue |
| `EstTimeSlot` | `DateTime` | Estimated time to be served |
| `NotificationStatus` | `NotificationStatus` | Pending / Sent / Read |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `UpdatePosition(newPosition)` | `void` | Updates position; auto-sends notification if position ≤ 3 |
| `SendSmartNotification()` | `void` | Sets status to `Sent` and prints alert |

---

### Queue

> Manages the virtual waiting queue for a place.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `QueueId` | `int` | Unique queue ID |
| `PlaceId` | `int` | Linked attraction ID |
| `CounterCount` | `int` | Number of service counters |
| `WaitingCount` | `int` *(computed)* | How many people are currently waiting |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `JoinQueue(touristName, avgServiceMinutes)` | `TicketQueue` | Adds tourist, returns their ticket |
| `DequeueNext()` | `TicketQueue` | Removes and returns the first in line |
| `GetEstimatedWaitTime(avgServiceMinutes)` | `double` | Estimated minutes for the whole queue |
| `GetWaitingList()` | `List<TicketQueue>` | Returns all waiting tickets |

---

### Counter

> Represents one physical service counter.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `CounterId` | `int` | Counter number |
| `QueueId` | `int` | Linked queue |
| `IsOpen` | `bool` | Whether the counter is active |
| `ServedToday` | `int` | Total tourists served today |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `ServeNext(queue)` | `void` | Calls `DequeueNext()` and serves them |

---

### Booking *(abstract)*

> Base class for all booking types. Cannot be instantiated directly.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `BookingId` | `int` | Auto-increments globally |
| `Type` | `BookingType` | Tour / Ticket / Guide |
| `TouristName` | `string` | Name of the tourist |
| `Date` | `DateTime` | Booking date |
| `Price` | `double` | Total price |
| `CommissionRate` | `double` | Default: 0.10 (10%) |
| `IsPaid` | `bool` | Payment status |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `CalculateCommission()` | `double` | Returns `Price × CommissionRate` |
| `GetBookingDetails()` | `string` | **Abstract** — overridden by each subclass |

---

### TourBooking
*Inherits from: `Booking`*  
*Commission rate: 10%*

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `TourName` | `string` | Name of the tour |
| `DurationHours` | `int` | Duration in hours |
| `TourDetails` | `string` | Auto-generated summary |

`GetBookingDetails()` returns: `"Tour: {TourName} | {DurationHours}h"`

---

### TicketBooking
*Inherits from: `Booking`*  
*Commission rate: 5% (overrides default)*

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `AttractionName` | `string` | Place name |
| `Quantity` | `int` | Number of tickets |
| `TicketDetails` | `string` | Auto-generated summary |

`GetBookingDetails()` returns: `"Ticket: {AttractionName} x{Quantity}"`

---

### GuideBooking
*Inherits from: `Booking`*  
*Commission rate: 10%*

#### Extra Properties
| Property | Type | Description |
|----------|------|-------------|
| `GuideName` | `string` | Guide's name |
| `Language` | `string` | Language (default: `"English"`) |
| `GuideDetails` | `string` | Auto-generated summary |

`GetBookingDetails()` returns: `"Guide: {GuideName} | {Language}"`

---

### Payments

> Handles payment processing for a booking.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `PaymentId` | `int` | Auto-increments globally |
| `BookingId` | `int` | Linked booking ID |
| `Amount` | `double` | Payment amount |
| `Method` | `string` | e.g. `"Credit Card"` |
| `PaymentDate` | `DateTime` | Auto-set to now |
| `IsSuccessful` | `bool` | Result of processing |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `ProcessPayment()` | `bool` | Returns `true` on success; throws if amount ≤ 0 |

---

### TripPlanner

> Sorts selected places by crowd level to build an optimal route.

#### Properties
| Property | Type | Description |
|----------|------|-------------|
| `TouristId` | `int` | Linked tourist ID |
| `Date` | `DateTime` | Trip date |
| `OrderedAttractionsList` | `List<TouristAttraction>` | Sorted results after planning |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `PlanDayBasedOnCrowd(selectedPlaces)` | `void` | Sorts places ascending by crowd level |
| `CalculateRecommendedRoute()` | `void` | Prints route to console |
| `PrintRoute()` | `void` | Prints each stop with time label and crowd info |

---

### IntelligenceEngine

> The smart brain of the system. Provides recommendations and simulations.

#### Constructor
```csharp
new IntelligenceEngine(PlaceList placeList)
```

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `FindBestTimeToVisit(placeId)` | `string` | Returns best hour from forecast (e.g. `"8:00"`) |
| `SuggestAlternativePlaces(crowdedPlaceId)` | `TouristAttraction` | Returns least-crowded alternative |
| `WhatIfSimulate(placeId, hour)` | `string` | Returns crowd + estimated wait for a given hour |
| `GetCrowdStatus(placeId)` | `string` | Returns smart notification message |

#### `WhatIfSimulate` Wait Time Logic
| Forecast | Estimated Wait |
|----------|---------------|
| Low Crowd | 5 min |
| Medium Crowd | 15 min |
| High Crowd | 30 min |
| Very High Crowd | 45 min |

---

## Services

---

### UserManager

> Manages registration, login, and logout. Persists users to file automatically.

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `RegisterTourist(name, email, password)` | `Tourist` | Creates and saves a tourist |
| `RegisterEmployee(name, email, password, dept)` | `Employee` | Creates and saves an employee |
| `RegisterAdmin(name, email, password)` | `Admin` | Creates and saves an admin |
| `Login(email, password)` | `User` | Returns user or throws exception |
| `Logout()` | `void` | Clears `CurrentUser` |
| `FindByEmail(email)` | `User` | Returns user or `null` |
| `GetAllUsers()` | `List<User>` | Returns all registered users |

#### Property
| Property | Type | Description |
|----------|------|-------------|
| `CurrentUser` | `User` | The currently logged-in user (`null` if none) |

#### Errors
| Scenario | Exception |
|----------|-----------|
| Email already exists | `InvalidOperationException` |
| Email not found on login | `Exception("User not found.")` |
| Wrong password | `Exception("Incorrect password.")` |

---

### BookingManager

> Creates and manages all booking types. Handles payment processing.

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `BookTour(name, date, price, tourName, hours)` | `TourBooking` | Creates and saves tour booking |
| `BookTicket(name, date, price, attraction, qty)` | `TicketBooking` | Creates and saves ticket booking |
| `BookGuide(name, date, price, guideName, language)` | `GuideBooking` | Creates and saves guide booking |
| `ProcessPayment(booking, method)` | `bool` | Creates `Payments` object and processes it |
| `GetBookingsByTourist(touristName)` | `List<Booking>` | Filters bookings by tourist name |
| `GetAllBookings()` | `List<Booking>` | Returns all bookings |
| `GetTotalRevenue()` | `double` | Sum of all paid booking prices |
| `GetTotalCommission()` | `double` | Sum of commission from all paid bookings |

---

### FileHandler

> Saves and loads all system data as plain text files.

#### File Locations
| File | Contents |
|------|---------|
| `data/users.txt` | All registered users |
| `data/bookings.txt` | All bookings |
| `data/reviews.txt` | All place reviews |
| `data/queue.txt` | Queue entries |
| `data/log.txt` | Activity log (timestamped) |

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `SaveUser(user)` | `void` | Appends user to users.txt |
| `LoadUsers()` | `List<string[]>` | Reads and splits all user rows |
| `SaveBooking(booking)` | `void` | Appends booking to bookings.txt |
| `LoadBookings()` | `List<string[]>` | Reads and splits all booking rows |
| `SaveReview(placeId, review)` | `void` | Appends review to reviews.txt |
| `SaveQueueEntry(ticket)` | `void` | Appends queue ticket to queue.txt |
| `LogActivity(message)` | `void` | Appends timestamped message to log.txt |
| `ReadLog()` | `string` | Returns full log as string |

> All methods use `try/catch` and fail silently on error (no crashes).

---

### DataSeeder

> Seeds the system with sample data for testing and demo purposes.

#### Methods
| Method | Returns | Description |
|--------|---------|-------------|
| `SeedPlaces()` | `PlaceList` | Returns 5 pre-built Egyptian attractions |
| `SeedUsers()` | `UserManager` | Registers 4 demo users |

#### Demo Users
| Name | Email | Password | Role |
|------|-------|----------|------|
| Alice Johnson | alice@email.com | pass123 | Tourist |
| Bob Hassan | bob@email.com | pass123 | Tourist |
| Mohamed Salah | emp@email.com | emp123 | Employee |
| Admin User | admin@email.com | admin123 | Admin |

#### Demo Places
| ID | Name | Location |
|----|------|---------|
| 1 | Pyramids of Giza | Giza, Cairo |
| 2 | Egyptian Museum | Tahrir Square, Cairo |
| 3 | Khan El Khalili | Islamic Cairo |
| 4 | Luxor Temple | Luxor, Upper Egypt |
| 5 | Saqqara | Saqqara, Giza |

---

## ID System

All IDs are **auto-incremented static counters** inside each class.

| Class | ID Start | Resets on? |
|-------|----------|-----------|
| `User` subclasses | 1 | Never |
| `TouristAttraction` | Manual | Never |
| `Booking` subclasses | 1 | Never |
| `Payments` | 1 | Never |
| `TicketQueue` | 1000 | Never |

> IDs never reset during a session. If you need to reload saved data, call `UpdateCounter(lastId)` on the relevant class to avoid ID conflicts.

---

## Data Storage

All data is stored as **pipe-delimited text** (`|`) in the `data/` folder.

#### Example — users.txt
```
1|Tourist|Alice Johnson|alice@email.com|pass123
2|Admin|Admin User|admin@email.com|admin123
```

#### Example — bookings.txt
```
1|Tour|Alice Johnson|24/04/2026|75|True|Tour: Pyramids Day Tour | 4h
```

#### Example — log.txt
```
[24/04/2026 09:15:30] New tourist registered: Alice Johnson
[24/04/2026 09:16:00] User logged in: Alice Johnson [Tourist]
[24/04/2026 09:20:00] Booking #1 created for Alice Johnson
```

---

## Error Handling

| Scenario | Where | How handled |
|----------|-------|-------------|
| Empty registration fields | `User.Register()` | Throws `ArgumentException` |
| Negative age | `Person.Age` setter | Throws `ArgumentException` |
| Review rating out of range | `Review` constructor | Throws `ArgumentOutOfRangeException` |
| Duplicate email | `UserManager.Register*()` | Throws `InvalidOperationException` |
| Wrong login credentials | `UserManager.Login()` | Throws `Exception` with message |
| Payment amount ≤ 0 | `Payments.ProcessPayment()` | Throws `ArgumentException` |
| PlaceList full (100 places) | `PlaceList.AddPlace()` | Throws `InvalidOperationException` |
| File not found | `FileHandler.*` | Caught silently, returns empty list |
| Invalid appointment ID | `BookingManager` | Returns `null` or shows message |

> All UI `try/catch` blocks should catch `Exception ex` and display `ex.Message` to the user.

---

## Inheritance Tree

```
User (abstract)
├── Tourist
├── Employee
└── Admin

Booking (abstract)
├── TourBooking       → commission 10%
├── TicketBooking     → commission 5%
└── GuideBooking      → commission 10%

CrowdCounter
├── PhysicalSensor    → Arduino input
├── AICamera          → YOLO detection
└── DataSimulator     → Random generation
```

---

*TourEgypt — Smart Tourism Crowd Management System*  
*OOP Project | Faculty of Computer Science | 2025–2026*
