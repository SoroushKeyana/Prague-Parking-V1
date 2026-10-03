
using System.Globalization;

string TimestampFormat = "yyyy-MM-ddTHH:mm:ss";

string[] parkingGarage = new string[100];
string[] menu = { "Park", "Move", "Remove", "Search", "Print Slots","Garage Overview", "Filtered View", "Exit" };

parkingGarage[0] = $"MC#321#{DateTime.Now.AddHours(-3):yyyy-MM-ddTHH:mm:ss}|MC#1234#{DateTime.Now.AddHours(-1):yyyy-MM-ddTHH:mm:ss}";
parkingGarage[1] = $"MC#66G3#{DateTime.Now.AddDays(-1).AddHours(-5):yyyy-MM-ddTHH:mm:ss}";
parkingGarage[3] = $"MC#634G3#{DateTime.Now.AddMinutes(-20):yyyy-MM-ddTHH:mm:ss}";
parkingGarage[10] = $"CAR#23Gf#{DateTime.Now.AddDays(-2).AddHours(-7):yyyy-MM-ddTHH:mm:ss}";

static void PrintMenu(string[] menu)
{
    Console.ForegroundColor = ConsoleColor.Black;
    Console.BackgroundColor = ConsoleColor.Cyan;
    Console.WriteLine("==============================");
    Console.WriteLine("       PRAGUE PARKING         ");
    Console.WriteLine("==============================\n");
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Cyan;
    for (int i = 0; i < menu.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {menu[i],-26}|");
    }
    Console.WriteLine("------------------------------\n");
    Console.ResetColor();
}

static int ReadMenuChoice()
{
    Console.Write("Type a number between 1-8: ");

    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int choice) &&
            choice >= 1 && choice <= 8)
        {
            return choice;
        }
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Please enter a number between 1-8: ");
        Console.ResetColor();
    }
}

static void Park(string[] parkingGarage)
{
    string regNumber;
    while (true)
    {
        Console.Write("Insert the vehicles registration number: ");
        regNumber = Console.ReadLine().ToUpper();
        

        if (regNumber.Length > 0 && regNumber.Length <= 10)
        {
            break;
        }
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("Invalid registration number. Registration number should be maximum 10 charachters.");
        Console.ResetColor();
    }

    bool parked = false;
    string timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
    while (true)
    {
        Console.WriteLine("1. Car \n2. Motorcycle");
        Console.Write("Select the vehicle type by entring 1 or 2: ");
        int.TryParse(Console.ReadLine(), out int vehicleType);
        if (vehicleType == 1)
        {
            string entry = $"CAR#{regNumber}#{timestamp}";
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (parkingGarage[i] == null)
                {
                    parkingGarage[i] = entry;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Parked {regNumber} in spot {i + 1}");
                    Console.ResetColor();
                    parked = true;
                    break;
                }
            }
            if (!parked)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("No free parking slots available.");
                Console.ResetColor();
            }
            break;
        }
        else if (vehicleType == 2)
        {
            regNumber = "MC#" + regNumber;
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (!string.IsNullOrEmpty(parkingGarage[i]) && parkingGarage[i].StartsWith("MC#"))
                {
                    if (parkingGarage[i].Contains("|"))
                    {
                        continue;
                    }

                    parkingGarage[i] += "|" + regNumber;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Parked {regNumber} in slot {i + 1}");
                    Console.ResetColor();
                    parked = true;
                    break;
                }
                else if (parkingGarage[i] == null)
                {
                    parkingGarage[i] = regNumber;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Parked {regNumber} in slot {i + 1}");
                    Console.ResetColor();
                    parked = true;
                    break;
                }
            }
            if (!parked)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No free parking slots available.");
                Console.ResetColor();
            }
            break;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Wrong input try again!");
            Console.ResetColor();
        }
    }
}

static void Move(string[] parkingGarage)
{
    Console.Write("Enter registration number of the vehicle you would like to move: ");
    string regNumber = Console.ReadLine();

    var result = Search(parkingGarage, regNumber);

    if (result.slotIndex == -1)
    {
        return;
    }

    Console.Write("Enter destination slot 1-100: ");
    if (!int.TryParse(Console.ReadLine(), out int destinationSlot) ||
        destinationSlot < 1 || destinationSlot > 100)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Invalid slot number.");
        Console.ResetColor();
        return;
    }

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("Type 'm' to move the vehicle: ");
    Console.ResetColor();
    if (Console.ReadLine() != "m")
    {
        return;
    }

    if (!string.IsNullOrEmpty(parkingGarage[destinationSlot - 1]))
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("That spot is occupied.");
        Console.ResetColor();
        return;
    }

    if (parkingGarage[result.slotIndex].Contains('|'))
    {
        string[] vehicles = parkingGarage[result.slotIndex].Split('|');
        string movingVehicle = vehicles[result.vehicleIndex];
        string remainingVehicle = vehicles[1 - result.vehicleIndex];

        parkingGarage[destinationSlot - 1] = movingVehicle;
        parkingGarage[result.slotIndex] = remainingVehicle;
    }
    else
    {
        parkingGarage[destinationSlot - 1] = parkingGarage[result.slotIndex];
        parkingGarage[result.slotIndex] = null;
    }

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine($"Moved to slot {destinationSlot}");
    Console.ResetColor();
}

static void Remove(string[] parkingGarage)
{
    Console.Write("Enter registration number: ");
    string query = Console.ReadLine();

    var result = Search(parkingGarage, query);

    if (result.slotIndex == -1)
    {
        return;
    }

    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write("Type 'r' to remove the vehicle: ");
    Console.ResetColor();
    if (Console.ReadLine() != "r")
    {
        return;
    }

    string[] vehicles = parkingGarage[result.slotIndex].Split('|');
    string removedVehicle = vehicles[result.vehicleIndex];
    string[] removedParts = removedVehicle.Split('#');

    if (vehicles.Length == 1)
    {
        parkingGarage[result.slotIndex] = null;
    }
    else
    {
        parkingGarage[result.slotIndex] = vehicles[1 - result.vehicleIndex];
    }

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine($"\nVehicle with REGNR '{query}' has been removed.");
    if (removedParts.Length >= 3 && DateTime.TryParseExact(removedParts[2], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parkedAt))
    {
        TimeSpan duration = DateTime.Now - parkedAt;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"Parked for: {duration.Days} days, {duration.Hours} hours, {duration.Minutes} minutes");
    }
    Console.ResetColor();
}

static (int slotIndex, int vehicleIndex) Search(string[] parkingGarage, string query)
{
    query = query.Trim();

    for (int i = 0; i < parkingGarage.Length; i++)
    {
        if (parkingGarage[i] == null)
            continue;

        string[] vehicles = parkingGarage[i].Split('|');

        for (int j = 0; j < vehicles.Length; j++)
        {
            string[] vehicleData = vehicles[j].Split('#');

            if (vehicleData[1].Equals(query, StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"The vehicle is in parking slot {i + 1}");
                Console.ResetColor();
                return (i, j);
            }
        }
    }

    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine("Registration number doesn't exist!");
    Console.ResetColor();
    return (-1, -1);
}

static string FormatSlotForDisplay(string slot)
{
    if (string.IsNullOrEmpty(slot))
    {
        return "Empty";
    }

    string[] vehicles = slot.Split('|');
    string[] display = new string[vehicles.Length];

    for (int i = 0; i < vehicles.Length; i++)
    {
        string[] parts = vehicles[i].Split('#');
        display[i] = $"{parts[0]}#{parts[1]}";
    }

    return string.Join(" | ", display);
}

static void PrintSlot(string[] parkingGarage, int spotNumber)
{
    string slot = parkingGarage[spotNumber - 1];
    Console.WriteLine($"{spotNumber,3}. {FormatSlotForDisplay(slot)}");
}
static void PrintSlots(string[] parkingGarage)
{

    for (int i = 0; i < parkingGarage.Length; i++)
    {
        Console.Write($"{i + 1,3}. {FormatSlotForDisplay(parkingGarage[i]),-25}");
        if ((i + 1) % 3 == 0)

            if ((i + 1) % 4 == 0)
        {
            Console.WriteLine();
        }
    }
}

static void PrintOverview(string[] parkingGarage)
{
    int empty = 0, halfFull = 0, full = 0;

    Console.WriteLine("Green = empty, Yellow = 1 MC (room for another), Red = full\n");

    for (int i = 0; i < parkingGarage.Length; i++)
    {
        string slot = parkingGarage[i];

        if (string.IsNullOrEmpty(slot))
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            empty++;
        }
        else if (slot.StartsWith("MC#") && !slot.Contains('|'))
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            halfFull++;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            full++;
        }

        Console.Write($"{i + 1,4}");
        Console.ResetColor();

        if ((i + 1) % 10 == 0)
        {
            Console.WriteLine();
        }
    }

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.Write($"\nEmpty: {empty} ");
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.Write($" Half full: {halfFull}  ");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.Write($"Full: {full}");
    Console.ResetColor();
}

static void PrintFilteredView(string[] parkingGarage, string filter)
{
    Console.WriteLine($"--- {filter} ---");
    bool anyFound = false;

    for (int i = 0; i < parkingGarage.Length; i++)
    {
        string slot = parkingGarage[i];

        if (string.IsNullOrEmpty(slot))
        {
            if (filter == "Empty spots")
            {
                Console.WriteLine($"{i + 1,3}. Empty");
                anyFound = true;
            }
            continue;
        }

        string[] vehicles = slot.Split('|');
        foreach (string vehicle in vehicles)
        {
            string[] parts = vehicle.Split('#');
            string type = parts[0];
            string regnr = parts[1];

            if ((filter == "Cars" && type == "CAR") ||
                (filter == "Motorcycles" && type == "MC"))
            {
                string parkedSince = "";
                if (parts.Length >= 3 &&
                    DateTime.TryParseExact(parts[2], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parkedAt))
                {
                    TimeSpan duration = DateTime.Now - parkedAt;
                    parkedSince = $" (parked {duration.Days}d {duration.Hours}h {duration.Minutes}m)";
                }

                Console.WriteLine($"{i + 1,3}. {type}#{regnr}{parkedSince}");
                anyFound = true;
            }
        }
    }

    if (!anyFound)
    {
        Console.WriteLine("Nothing to show.");
    }
}

static void Exit()
{
    Console.WriteLine("Exiting the program...");
    Environment.Exit(0);
}

while (true)
{
    Console.Clear();

    PrintMenu(menu);
    int choice = ReadMenuChoice();
    switch (choice)
    {
        case 1: Park(parkingGarage); break;
        case 2: Move(parkingGarage); break;
        case 3: Remove(parkingGarage); break;
        case 4:
            {
                Console.Write("Enter registration number: ");
                string query = Console.ReadLine();
                Search(parkingGarage, query);
                break;
            }
        case 5:
            {
                Console.WriteLine("1. Print all spots");
                Console.WriteLine("2. Print specific spot");
                Console.Write("Choose: ");

                int printChoice = int.Parse(Console.ReadLine());

                if (printChoice == 1)
                {
                    PrintSlots(parkingGarage);
                }
                else if (printChoice == 2)
                {
                    Console.Write("Enter spot number (1-100): ");
                    int spotNumber = int.Parse(Console.ReadLine());

                    if (spotNumber >= 1 && spotNumber <= 100)
                    {
                        PrintSlot(parkingGarage, spotNumber);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Invalid spot number.");
                        Console.ResetColor();
                    }
                }

                break;
            }
        case 6: PrintOverview(parkingGarage); break;
        case 7:
            {
                Console.WriteLine("1. Cars");
                Console.WriteLine("2. Motorcycles");
                Console.WriteLine("3. Empty spots");
                Console.Write("Choose: ");

                int reportChoice = int.Parse(Console.ReadLine());
                string filter = reportChoice switch
                {
                    1 => "Cars",
                    2 => "Motorcycles",
                    3 => "Empty spots",
                    _ => ""
                };

                if (filter != "")
                {
                    PrintFilteredView(parkingGarage, filter);
                }
                break;
            }
        case 8: Exit(); break;
    }
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}

