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
* Started working on Prague Parking 1.1.

## 2026-10-02

* Copied the files from Prague Parking V1.0 to Prague Parking V1.1.
* Started planning on which features to add and how to do it.
* Decided to go with working on visualization and adding Timestamp and duration parked as they do not require a data format redesign.
* Started with visualization. Apllied colors to garage print. Green for empty, red for full, and yellow for half full. 
* Added some colors to make the console visually more appealing. 
* Started working on printing an `Overview` of the garage. 

## 2026-10-03

* Created `PrintOverview` method.
* Added variables to have the count of empty, half full and full slots. 
* The method will print slots 1-100. If slot is red means full, if yellow means half full and if green means empty. 
* Added `Overview` to the `Menu`. 
* Noticed that the number of choices is hard coded. Meaning that when I add a new item to the menu I have to change several things.
* Added a new switch case for `Overview`.
* Created `PrintFilteredView` for user to be able to have a filtered view to see cars, motorcycles and empty slots seperatley. 
* Decided on data format for timestamps: changed each vehicle entry from `TYPE#REGNR` to `TYPE#REGNR#TIMESTAMP`, using format `yyyy-MM-ddTHH:mm:ss` so it parses consistently regardless of machine locale.
* Updated `Park` to automatically capture `DateTime.Now` and store it as part of the vehicle string. User never types a timestamp in manually, as required.
* Checked every method that does `.Split('#')` to see if adding a 3rd field would break anything. `Search` and `PrintFilteredView` only read index 1 (reg number), so both kept working without changes.
* Updated `Remove` to parse out the removed vehicle's timestamp, calculate parked duration using `DateTime.Now - parkedAt`, and print it in days/hours/minutes.
* Used `DateTime.TryParseExact` with `CultureInfo.InvariantCulture` instead of a plain parse, so the timestamp won't be read incorrectly if the app runs on a computer with different regional date settings.
* Checked `Move` and found it needed no changes at all since it copies the whole vehicle string as is, the timestamp carries over automatically when a vehicle is relocated. Decided this is correct: moving a vehicle shouldn't reset how long it's been parked.
* Noticed that once the stored string included a timestamp, `PrintSlot` and `PrintSlots` became unreadable (showing the raw `CAR#ABC123#2026-10-03T14:30:00` string). Created a new method, `FormatSlotForDisplay`, to convert the stored string into a clean readable line, and updated both print methods to use it.
* Extended `PrintFilteredView` to show "(parked Xd Yh Zm)" next to each vehicle, reusing the timestamp parsing already being done there.
* Noticed `Park` was checking for an existing motorcycle in a slot using `.Contains("MC#")`. Changed it to `.StartsWith("MC#")`, which is the more correct check since a slot's type marker is always at the start of the string.