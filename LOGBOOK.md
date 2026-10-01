# Logbook — Prague Parking V1

## 2026-09-26
- Read through the assignment specification.

## 2026-09-27
- Planing on how to implement the project.
- Created parkingGarage array.
- Created PrintSlots method to print the spaces in parkingGarage.
- Created SetDefaultSlotLabels to set default slot labels to "Empty".
- Created PrintMenu method to print options for user to choose from.
- Created ReadMenuChoice method to get user's chosen option from the menu.

## 2026-09-28
- Removed the SetDefaultSlotLabels method because it was creating unnecessary work. Assigning "Empty" to every empty parking slot could also cause issues later. Instead, I made PrintSlots display "Empty" when a slot is null. 
- Made the methods static so they can be called from the top-level program.
- Created Park, Move, Remove, Search, and Exit methods.
- Implemented Exit Method.
- I noticed that i pass parkingGarage argument to Exit() method and we have no use for that so I removed it. 
- Created a while loop that prints menu and asks user for input.
- Inside the while loop I created a switch where it calls the related method to execute the user's choice.
- Added logic to Search method. 
- Added another parameter 'query' to Search method to get user's search query and compare it to the serial numbers that exist in the parking slots. 

## 2026-09-29
- Started Implementing the Park method. 
- Get serial number, check for available slots and store the serial number.
- Adding logic so that the program adds the type of the vehicle to the serial number using TYPE#SERNR.
- Added extra checks if the slot contains a motorcycle and if yes see if there is available spot for second motorcycle.
- Made changes to PrintSlots method to make it more readable. 
- Made menu visually more appealing. 
- Implementing Remove method.
- Noticed that I can reuse the Search method for removing a vehicle. 
- Changin Search method to return values so that it will be useful in Remove method. 
- Completed the implemetaton of Remove method. 
- Added PrintSpot method for having the option of printing a single spot. 
- Changed case 5 to have the user choose between printing whole garage or single spot. 

## 2026-10-01
- Started working on Move method.
- Reused the Search method for it.
- Made it work. It was easier than I initially tought.
- I noticed that the Contains method I used in Move and Remove can cause issues. 
- I changed the Move and Remove methods, used result.vehicleIndex instead of Contains. It is better appraoch as it reduces code repetation and also works better. 