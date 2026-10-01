# Logbook — Prague Parking V1

## 2026-09-26

* Read through the assignment specification.

## 2026-09-27

* Planning how to implement the project.
* Created the `parkingGarage` array.
* Created the `PrintSlots` method to print the spaces in `parkingGarage`.
* Created `SetDefaultSlotLabels` to set default slot labels to `"Empty"`.
* Created the `PrintMenu` method to print options for the user to choose from.
* Created the `ReadMenuChoice` method to get the user's chosen option from the menu.

## 2026-09-28

* Removed the `SetDefaultSlotLabels` method because it was creating unnecessary work. Assigning `"Empty"` to every empty parking slot could also cause issues later. Instead, I made `PrintSlots` display `"Empty"` when a slot is `null`.
* Made the methods static so they can be called from the top-level program.
* Created the `Park`, `Move`, `Remove`, `Search`, and `Exit` methods.
* Implemented the `Exit` method.
* I noticed that I passed the `parkingGarage` argument to `Exit()`, but it was not used, so I removed it.
* Created a `while` loop that prints the menu and asks the user for input.
* Inside the `while` loop, I created a `switch` that calls the related method based on the user's choice.
* Added logic to the `Search` method.
* Added another parameter, `query`, to the `Search` method to get the user's search query and compare it to the serial numbers that exist in the parking slots.

## 2026-09-29

* Started implementing the `Park` method.
* Get the serial number, check for available slots, and store the serial number.
* Added logic so that the program adds the type of the vehicle to the serial number using `TYPE#SERNR`.
* Added extra checks to see if the slot contains a motorcycle and, if so, check if there is space for a second motorcycle.
* Made changes to the `PrintSlots` method to make it more readable.
* Made the menu visually more appealing.
* Implemented the `Remove` method.
* Noticed that I could reuse the `Search` method for removing a vehicle.
* Changed the `Search` method to return values so that it would be useful in the `Remove` method.
* Completed the implementation of the `Remove` method.
* Added the `PrintSlot` method to have the option of printing a single spot.
* Changed case 5 to let the user choose between printing the whole garage or a single spot.

## 2026-10-01

* Started working on the `Move` method.
* Reused the `Search` method for it.
* Made it work. It was easier than I initially thought.
* I noticed that the `Contains` method I used in `Move` and `Remove` could cause issues.
* Changed the `Move` and `Remove` methods to use `result.vehicleIndex` instead of `Contains`. This is a better approach because it reduces code repetition and works better.
* Worked on the README.md file. 
* 
