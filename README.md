# Prague Parking V1 / V1.1

The customer wants a support system for a parking lot near the castle in Prague.

The parking lot uses valet parking. The customer gives the vehicle and its key to the staff and receives a receipt that they can use to collect their vehicle later.

The parking lot is operated by "tech-savvy students" and pensioners, so the system should be simple and easy to use.

The parking lot accepts cars and motorcycles.

Currently, all vehicles are collected before 00:00, when the parking lot closes. Vehicles that are not collected are moved to a parking lot outside the city. Customers must pay a penalty fee to get their vehicle back. This is not handled by the current system.

## Versions

* **V1** — the core system: park, move, remove, search, print, exit. 
* **V1.1** — built on top of V1: a color-coded garage overview, filtered reports by vehicle type, and automatic timestamping with parked-duration tracking.

## Vehicle Registration Format

Each vehicle stored in the garage is encoded as a single string.

**In V1**, the format is `TYPE#REGNR`:

* A car with registration number `A343` becomes `CAR#A343`.
* A motorcycle with registration number `BF483` becomes `MC#BF483`.

**In V1.1**, a timestamp is added automatically when the vehicle is parked, so the format becomes `TYPE#REGNR#TIMESTAMP`:

* A car parked on October 3rd, 2026 at 14:30 becomes `CAR#A343#2026-10-03T14:30:00`.

The timestamp is captured automatically from the system clock (`DateTime.Now`) when the vehicle is parked, the user never enters it manually. The format `yyyy-MM-ddTHH:mm:ss` is used so it can be parsed consistently regardless of the computer's regional settings.

## Parking Garage

The parking garage has 100 parking spots.

Each spot can contain:

* One car.
* One motorcycle.
* Two motorcycles.

A car always needs its own parking spot. Two motorcycles can share the same parking spot.

The parking spots are represented using a string array:

```csharp
string[] parkingGarage = new string[100];
```

An empty spot is represented by `null`.

For example, in V1.1:

```text
Spot 1: MC#321#2026-10-03T11:30:00|MC#1234#2026-10-03T13:30:00
Spot 2: MC#66G3#2026-10-02T09:15:00
Spot 3: Empty
Spot 4: MC#634G3#2026-10-03T14:10:00
Spot 5: Empty
```

The `|` character separates two motorcycles sharing the same parking spot. The `#` character separates a vehicle's type, registration number, and (in V1.1) its timestamp.

When vehicle details are displayed to the user (e.g. via Print Slots or the filtered view), this raw internal format is converted into a readable line through a dedicated formatting method rather than shown as-is.

## Menu

The system has a menu with the following options:

1. Park
2. Move
3. Remove
4. Search
5. Print Slots
6. Garage Overview *(V1.1)*
7. Filtered View *(V1.1)*
8. Exit

The user selects an option by entering a number.

## Park

When parking a vehicle, the user enters the vehicle's registration number and chooses whether it is a car or motorcycle.

The registration number must be between 1 and 10 characters, the user is asked again if it isn't.

The system adds the vehicle type to the registration number using the `TYPE#REGNR` format (plus a timestamp in V1.1, added automatically).

For cars, the system searches for the first empty parking spot.

For motorcycles, the system first checks if there is a parking spot with one motorcycle. If there is, the new motorcycle is added to that spot. If there is no available shared motorcycle spot, the system searches for an empty spot.

The system then tells the user which parking spot the vehicle was placed in.

## Search

The user can search for a vehicle by entering its registration number.

The `Search` method loops through the parking garage and checks the vehicles in each parking spot.

If the vehicle is found, the system displays its parking spot.

The method returns a tuple, `(int slotIndex, int vehicleIndex)`, so that other methods (`Move`, `Remove`) can reuse the exact result of the search instead of rederiving it. If a spot holds two motorcycles, `vehicleIndex` identifies exactly which of the two matched.

## Remove

The user can remove a vehicle by entering its registration number.

The system first uses the `Search` method to find the vehicle, using its returned `vehicleIndex` to identify exactly which vehicle to remove if the spot holds two motorcycles.

If the spot contains only one vehicle, the spot is set to `null`.

If two motorcycles are sharing a spot, only the selected motorcycle is removed and the other motorcycle remains in the spot.

**V1.1:** before the vehicle record is deleted, its timestamp is read and compared to the current time, and the system displays how long it was parked, in days, hours, and minutes.

## Move

The user can move a vehicle by entering its registration number and the destination parking spot.

The system first uses the `Search` method to find the vehicle. The destination spot number must be between 1 and 100, and is checked to make sure it is empty, if either check fails, the user is told why and nothing is moved.

If the vehicle is the only vehicle in its current spot, its value (including its timestamp, in V1.1) is moved to the new spot and the old spot is set to `null`.

If two motorcycles are sharing a spot, only the selected motorcycle (identified via `vehicleIndex`) is moved and the other motorcycle stays in the original spot.

Moving a vehicle does not reset its parked timestamp, the system carries it over to the new spot, since relocating a vehicle isn't the same as it newly arriving.

## Print Slots

The user can choose to print either the whole parking garage or one specific parking spot.

When printing the whole garage, all 100 spots are displayed.

Empty spots are shown as `"Empty"` instead of displaying `null`. Occupied spots are shown in a readable format (vehicle type and registration number) rather than the raw internal string.

The user can also enter a specific spot number between 1 and 100 to see its contents.

## Garage Overview *(V1.1)*

Displays all 100 spots as a compact, color-coded grid:

* **Green** — empty spot
* **Yellow** — one motorcycle parked, room for a second
* **Red** — full (a car, or two motorcycles)

A summary count of empty / half-full / full spots is shown below the grid.

## Filtered View *(V1.1)*

Lets the user choose one of three reports:

1. All cars
2. All motorcycles
3. All empty spots

Each matching entry is listed with its spot number, vehicle type, registration number, and how long it has been parked (days/hours/minutes), calculated from its stored timestamp.

## Exit

The Exit option closes the program.

## Running the Project

The project can be run directly from Visual Studio with F5 or Ctrl-F5. No additional setup or configuration is required.