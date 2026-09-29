string[] parkingGarage = new string[100];
string[] menu = { "Park", "Move", "Remove", "Search", "Print Slots", "Exit" };

parkingGarage[0] = "MC#66Gf|MC#1234";
parkingGarage[1] = "MC#66G3";
parkingGarage[3] = "MC#634G3";
parkingGarage[10] = "CAR#23Gf";

static void PrintMenu(string[] menu)
{
    Console.WriteLine("==============================");
    Console.WriteLine("       PRAGUE PARKING");
    Console.WriteLine("==============================");
    for (int i = 0; i < menu.Length; i++)
    {
        Console.WriteLine($"{i+1}. {menu[i]}");
    }
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

        Console.Write("Please enter a number between 1-6: ");
    }
}

static void Park(string[] parkingGarage)
{
    Console.Write("Insert the vehicles serial number: ");
    string serialNumber = Console.ReadLine().ToUpper();
    bool parked = false;
    while (true) {
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
                    Console.WriteLine($"Parked {serialNumber} in spot {i + 1}");
                    parked = true;
                    break;
                }
            }
            if (!parked)
            {
                Console.WriteLine("No free parking slots available.");
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
                    Console.WriteLine($"Parked {serialNumber} in slot {i + 1}");
                    parked = true;
                    break;
                }
                else if (parkingGarage[i] == null)
                {
                    parkingGarage[i] = serialNumber;
                    Console.WriteLine($"Parked {serialNumber} in slot {i + 1}");
                    parked = true;
                    break;
                }
            }
            if (!parked)
            {
                Console.WriteLine("No free parking slots available.");
            }
            break;
        }
        else
        {
            Console.WriteLine("Wrong input try again!");
        }
    }
}

static void Move(string[] parkingGarage)
{
    Console.WriteLine("Moving");
}

static void Remove(string[] parkingGarage)
{
    Console.WriteLine("Removing");
}

static void Search(string[] parkingGarage, string query)
{
    bool found = false;
    query = query.Trim().ToLower();
    Console.WriteLine("Searching...");

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
                Console.WriteLine($"The vehicle is in parking slot {i+1}");
                found = true; 
                break;
            }
        }
    }
    if (!found)
    {
        Console.WriteLine("Serial number doesn't exist!");
    }
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
    switch (choice) {
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
        case 5: PrintSlots(parkingGarage); break;
        case 6: Exit(); break;
    }
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}

