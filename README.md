# Prague Parking V1

The customer wants a support system for a parking lot near the castle in Prague.

The parking lot uses valet parking. The customer gives the vehicle and its key to the staff and receives a receipt that they can use to collect their vehicle later.

The parking lot is operated by “tech-savvy students” and pensioners, so the system should be simple and easy to use.

The parking lot accepts cars and motorcycles.

Currently, all vehicles are collected before 00:00, when the parking lot closes. Vehicles that are not collected are moved to a parking lot outside the city. Customers must pay a penalty fee to get their vehicle back. This is not handled by the current system.

## Vehicle Registration Number

All vehicles use the `TYPE#REGNR` format for their registration number.

For example:

* A car with registration number `A343` becomes `CAR#A343`.
* A motorcycle with registration number `BF483` becomes `MC#BF483`.

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

For example:

```text
Spot 1: MC#321|MC#1234
Spot 2: MC#66G3
Spot 3: Empty
Spot 4: MC#634G3
Spot 5: Empty
```

The `|` character is used to separate two motorcycles that are sharing the same parking spot.

## Menu

The system has a menu with the following options:

1. Park
2. Move
3. Remove
4. Search
5. Print Slots
6. Exit

The user selects an option by entering a number.

## Park

When parking a vehicle, the user enters the vehicle's registration number and chooses whether it is a car or motorcycle.

The system adds the vehicle type to the registration number using the `TYPE#REGNR` format.

For cars, the system searches for the first empty parking spot.

For motorcycles, the system first checks if there is a parking spot with one motorcycle. If there is, the new motorcycle is added to that spot. If there is no available shared motorcycle spot, the system searches for an empty spot.

The system then tells the user which parking spot the vehicle was placed in.

## Search

The user can search for a vehicle by entering its registration number.

The `Search` method loops through the parking garage and checks the vehicles in each parking spot.

If the vehicle is found, the system displays its parking spot.

The method also returns the spot index and vehicle index so that other methods can reuse the search result.

## Remove

The user can remove a vehicle by entering its registration number.

The system first uses the `Search` method to find the vehicle.

If the spot contains only one vehicle, the spot is set to `null`.

If two motorcycles are sharing a spot, only the selected motorcycle is removed and the other motorcycle remains in the spot.

## Move

The user can move a vehicle by entering its registration number and the destination parking spot.

The system first uses the `Search` method to find the vehicle.

The destination spot is checked to make sure it is empty.

If the vehicle is the only vehicle in the spot, its value is moved to the new spot and the old spot is set to `null`.

If two motorcycles are sharing a spot, only the selected motorcycle is moved and the other motorcycle stays in the original spot.

## Print Slots

The user can choose to print either the whole parking garage or one specific parking spot.

When printing the whole garage, all 100 spots are displayed.

Empty spots are shown as `"Empty"` instead of displaying `null`.

The user can also enter a specific spot number between 1 and 100 to see its contents.

## Exit

The Exit option closes the program.

The system displays a message before exiting.
