# Reflection — Prague Parking V1.0 / V1.1

## 1. Summary

This project asked for a console based parking management system for a valet parking lot in Prague, handling up to 100 parking spots for cars and motorcycles (with up to two motorcycles sharing a slot). Version 1.0 covers the core requirements: parking a vehicle, moving it manually, removing it, searching for it by registration number, Printing slots, and a text-based menu, all built on top of a single `string[100]` array as the spec required. Next I built version 1.1 on top of the working 1.0 code, adding two features beyond the minimum: a visual garage overview with color coded status and filtered reports (cars only / motorcycles only / empty spots only), plus automatic timestamping of vehicles with parked-duration shown on removal.

## 2. How I solved the assignment

I started by reading through the specification carefully and translating its requirements directly into a plan before writing any code. a flat list of what the system needed to do, followed by a rough method list based on the "Programstruktur" section of the spec (menu printing, menu reading, search, add, remove, move, print). I didn't lock this list in permanently as the spec itself warned, I discovered more methods as I went (`PrintSlot` alongside `PrintSlots`, and later `FormatSlotForDisplay` once the data got more complex).

The core design decision was the data format: each parking spot stores a string built as `TYPE#REGNR`, for example `CAR#ABC123` or, for a shared motorcycle spot, two vehicles joined with `|`, e.g. `MC#KLM789|MC#FTP666`. This matched the spec's suggestion directly and let `String.Split` do most of the heavy lifting throughout the project.

I built the program incrementally: first the array and a stub menu loop, then `Search` (since every other feature depends on finding a vehicle first), then `Park`, then `Remove`, then `Move`, which I was able to build almost entirely out of the `Search` + remove logic I'd already written. The menu and console output got progressively more polished along the way (spacing, then color).

For 1.1, I copied the working 1.0 project and extended it in two directions: a visualization layer (`PrintOverview` for a color coded 1–100 grid with counts, and `PrintFilteredView` for type-specific reports), and a timestamp system, which meant extending the stored string format to `TYPE#REGNR#TIMESTAMP` and updating `Park` to capture `DateTime.Now` automatically, and `Remove` to calculate and display how long the vehicle had been parked.

## 3. Challenges in the assignment and how they were solved

**Reusing `Search` safely.** Early on, `Move` and `Remove` both needed to identify which specific vehicle in a shared motorcycle spot matched the user's query. My first approach used `.Contains()` on the vehicle strings, which I realized was risky, it's a substring match, not an exact one, so a search for `"12"` could incorrectly match a plate like `"AB123"`. Since `Search` already walks through and finds the exact matching vehicle, I changed its return type to a tuple `(int slotIndex, int vehicleIndex)` so `Move` and `Remove` could reuse that exact index directly, instead of re-deriving it with string matching. This removed duplicate logic and fixed a real correctness bug at the same time.

**A crash risk in `Move`.** My original `Move` method read a destination slot number straight from user input without checking it was in range, so an out-of-range number (e.g. `150`) would cause an `IndexOutOfRangeException` and crash the whole program. I added the same bounds check (`1–100`) that I already had elsewhere, before the array is ever touched. I also noticed `Move` failed silently if the destination was already occupied, nothing told the user why nothing happened, so I added an explicit "That spot is occupied" message.

**Extending the data format without breaking what already worked.** Adding a timestamp field meant every `.Split('#')` call in the program went from 2 parts to 3. Before writing any new code, I went through every method that parses a vehicle string and checked whether it would still work. `Search` and `PrintFilteredView` both only ever read index 1 (the registration number), so neither needed changes but I made sure to verify that rather than assume it. `Park` and `Remove` did need direct changes, since they're the methods responsible for writing and reading the new timestamp field.

**Locale safety.** Since the application has to run correctly on a computer other than my own, I used `DateTime.TryParseExact` with `CultureInfo.InvariantCulture` rather than a plain `DateTime.Parse`, so that date parsing doesn't silently behave differently depending on the regional settings of whichever machine runs the program.

**Readability regression after adding the timestamp.** Once the stored string included a timestamp, printing a slot's raw contents directly (as I had been doing) became unreadable, showing the full `CAR#ABC123#2026-10-03T14:30:00` string instead of something a parking attendant could actually use. I wrote a new method, `FormatSlotForDisplay`, specifically to convert the raw internal format into a clean, human readable line, and updated `PrintSlot` and `PrintSlots` to use it instead of printing the array value directly.

## 4. Methods and models used to solve the assignment

The core data model is a one-dimensional `string[100]` array, as required, with each element encoding one, two, or zero vehicles using `#` to separate a vehicle's type/registration/timestamp fields and `|` to separate two vehicles sharing a motorcycle spot. `String.Split` and string interpolation (`$"..."`) are used throughout to build and take apart these encoded strings.

Rather than one large `Main()` method, the program is broken into many small, single-purpose static methods (`Park`, `Move`, `Remove`, `Search`, `PrintSlot`, `PrintSlots`, `PrintOverview`, `PrintFilteredView`, `FormatSlotForDisplay`), following the spec's own advice to let each method do one well-defined job and to avoid overly large methods. `Search` returns a C# tuple `(int slotIndex, int vehicleIndex)` instead of being `void`, which lets it hand back two pieces of information to its caller at once similar in spirit to how `TryParse()` uses an `out` parameter, which the spec specifically pointed to as a useful pattern.

The menu is driven by a `while (true)` loop with a `switch` statement dispatching to the relevant method. Console colors (`Console.ForegroundColor`) are used throughout to give immediate visual feedback (green for success, yellow for warnings, red for errors or full/occupied spots), which became the basis for the `PrintOverview` visualization feature in 1.1.

## 5. How I would solve the assignment next time, given what I know now

The spec itself flags that packing multiple pieces of information into a single array element (as I did, and as it told me to) is something we'll later learn is a poor long-term design, and I can already feel why: every time I extended the data, I had to carefully re-check every method that parses the string, because nothing in the type system enforces that the format is consistent it's all just string manipulation held together by convention. Now that I have some exposure to how this feels in practice, I'd want to redo this with proper objects a `Vehicle` class with real fields (`Type`, `RegistrationNumber`, `ParkedAt`) instead of a delimited string, and a `ParkingSpot` class or similar that can hold one or two `Vehicle` objects directly. That would have made the timestamp extension in 1.1 something the compiler could help verify, rather than something I had to check manually spot by spot.

I'd also separate user interaction from logic more cleanly from the start. Right now, methods like `Park`, `Move`, and `Remove` mix `Console.ReadLine()`/`Console.Write()` calls directly with the actual parking logic, which the spec specifically warned against. `Search` already does this correctly it takes the query as a parameter instead of asking for it itself and I'd apply that same pattern consistently across the other methods next time, which would also make the logic easier to test independently of the console.

## 6. Conclusion for the home assignment

This assignment gave me a genuinely practical feel for why planning before coding matters deciding the data model and method list up front made almost every feature after that point faster to build, because I wasn't discovering structural problems mid implementation. The VG extension, in particular, taught me something concrete about the cost of a data format change extending the stored string to include a timestamp required me to reverify every single method that touched that string, which is exactly the kind of ripple effect that's much easier to see and reason about on a project this size than it would be to just be told about in the abstract.

## 7. Conclusion for the course, so far

This is my first time learning C#, though I have experience with other languages, and I've found the course genuinely well designed. The curriculum covers what feels like the right things in the right order, and the exercises in particular stand out, they push me to think beyond just reproducing syntax and into actually reasoning about a problem, in a way I don't think I'd get the chance to practice otherwise. I can already tell this is making me a better programmer generally, not just in C#, some of what I'm learning here (thinking more carefully about method responsibilities, data design, and testing incrementally) has already changed how I approach other languages I already knew.

The teacher has been calm, knowledgeable, and genuinely helpful, which makes a real difference when working through something unfamiliar. The flipped-classroom format has also worked well for me so far. I'm looking forward to object-oriented programming and the more advanced topics ahead, especially since I can already see, from this very assignment, exactly where OOP would have made my own code better.
