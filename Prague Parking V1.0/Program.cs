string[] parkingGarage = new string[100];
string[] menu = { "Park", "Move", "Remove", "Search", "Print Slots", "Exit" };

parkingGarage[10] = "CAR#23Gf";
parkingGarage[0] = "MS#66Gf|MS#1234";

static void PrintMenu(string[] menu)
{
    Console.WriteLine("Please choose an option: ");
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
    Console.WriteLine("Parking");
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
    Console.WriteLine("Searching...");

    for (int i = 0; i < parkingGarage.Length; i++)
    {
        if (parkingGarage[i] == null)
            continue;

        string[] vehicles = parkingGarage[i].Split('|');

        for (int j = 0; j < vehicles.Length; j++)
        {
            string[] vehicleData = vehicles[j].Split('#');

            if (vehicleData[1] == query)
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

    foreach (string slot in parkingGarage)
    {
        Console.WriteLine(slot ?? "Empty");
    }
}

static void Exit()
{
    Console.WriteLine("Exiting the program...");
    Environment.Exit(0);
}

while (true)
{
    PrintMenu(menu);
    int choice = ReadMenuChoice();
    switch (choice) {
        case 1: Park(parkingGarage); break;
        case 2: Move(parkingGarage); break;
        case 3: Remove(parkingGarage); break;
        case 4: 
            {
                Console.Write("Enter registration number or serial number: ");
                string query = Console.ReadLine();
                Search(parkingGarage, query);
                break;
            }
        case 5: PrintSlots(parkingGarage); break;
        case 6: Exit(); break;
    }
}

