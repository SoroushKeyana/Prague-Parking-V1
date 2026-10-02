string[] parkingGarage = new string[100];
string[] menu = { "Park", "Move", "Remove", "Search", "Print Slots", "Exit" };

parkingGarage[0] = "MC#321|MC#1234";
parkingGarage[1] = "MC#66G3";
parkingGarage[3] = "MC#634G3";
parkingGarage[10] = "CAR#23Gf";

static void PrintMenu(string[] menu)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("==============================");
    Console.WriteLine("       PRAGUE PARKING");
    Console.WriteLine("==============================");
    for (int i = 0; i < menu.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {menu[i]}");
    }
    Console.WriteLine("-------------------------------\n");
    Console.ResetColor();
}

static int ReadMenuChoice()
{
    Console.Write("Type a number between 1-6: ");

    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int choice) &&
            choice >= 1 && choice <= 6)
        {
            return choice;
        }
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Please enter a number between 1-6: ");
        Console.ResetColor();
    }
}

static void Park(string[] parkingGarage)
{
    Console.Write("Insert the vehicles serial number: ");
    string serialNumber = Console.ReadLine().ToUpper();
    bool parked = false;
    while (true)
    {
        Console.WriteLine("1. Car \n2. Motorcycle");
        Console.Write("Select the vehicle type by entring 1 or 2: ");
        int.TryParse(Console.ReadLine(), out int vehicleType);
        if (vehicleType == 1)
        {
            serialNumber = "CAR#" + serialNumber;
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (parkingGarage[i] == null)
                {
                    parkingGarage[i] = serialNumber;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Parked {serialNumber} in spot {i + 1}");
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
            serialNumber = "MC#" + serialNumber;
            for (int i = 0; i < parkingGarage.Length; i++)
            {
                if (!string.IsNullOrEmpty(parkingGarage[i]) && parkingGarage[i].Contains("MC#"))
                {
                    if (parkingGarage[i].Contains("|"))
                    {
                        continue;
                    }

                    parkingGarage[i] += "|" + serialNumber;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Parked {serialNumber} in slot {i + 1}");
                    Console.ResetColor();
                    parked = true;
                    break;
                }
                else if (parkingGarage[i] == null)
                {
                    parkingGarage[i] = serialNumber;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"Parked {serialNumber} in slot {i + 1}");
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
    Console.Write("Enter serial number of the vehicle you would like to move: ");
    string serialNumber = Console.ReadLine();

    var result = Search(parkingGarage, serialNumber);

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
    Console.Write("Enter serial number: ");
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

    if (vehicles.Length == 1)
    {
        parkingGarage[result.slotIndex] = null;
    }
    else
    {
        parkingGarage[result.slotIndex] = vehicles[1 - result.vehicleIndex];
    }

    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine($"Vehicle with REGNR '{query}' has been removed.");
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
    Console.WriteLine("Serial number doesn't exist!");
    Console.ResetColor();
    return (-1, -1);
}

static void PrintSlot(string[] parkingGarage, int spotNumber)
{
    Console.WriteLine($"{spotNumber,3}. {parkingGarage[spotNumber - 1] ?? "Empty",-25}");
}
static void PrintSlots(string[] parkingGarage)
{

    for (int i = 0; i < parkingGarage.Length; i++)
    {
        Console.Write($"{i + 1,3}. {parkingGarage[i] ?? "Empty",-25}");

        if ((i + 1) % 4 == 0)
        {
            Console.WriteLine();
        }
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
                Console.Write("Enter serial number: ");
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
        case 6: Exit(); break;
    }
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}

