# Logbook — Prague Parking V1

## 2026-09-26
- Read through the assignment specification

## 2026-09-27
- Planing on how to implement the project
- Created parkingGarage array
- Created PrintSlots method to print the spaces in parkingGarage
- Created SetDefaultSlotLabels to set default slot labels to "Empty"
- Created PrintMenu method to print options for user to choose from
- Created ReadMenuChoice method to get user's chosen option from the menu

## 2026-09-28
- Removed SetDefaultSlotsLabel method as it was creating unnecessary work and assigning each parking slot to "Empty" may cause issues later. I instead made it display "Empty" for slots that are null instead. 
- Made methods static
- I noticed that i pass parkingGarage argument to Exit() method and we have no use for that so I removed it. 
- Created a while loop that prints menu and asks user for input.
- Inside the while loop I created a switch where it calls the related method to execute the user's choice.
- 