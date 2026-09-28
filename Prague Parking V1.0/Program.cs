string[] parkingGarage = new string[100];
string[] menu = { "Park", "Move", "Remove", "Search", "Print Slots", "Exit" };

parkingGarage[10] = "CAR#23Gf";

static void SetDefaultSlotLabels(string[] parkingGarage)
{
    for (int i = 0; i < parkingGarage.Length; i++)
    {
        if (string.IsNullOrEmpty(parkingGarage[i]))
        {
            parkingGarage[i] = "Empty";
        }
    }
}

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
    PrintMenu(menu);
    Console.Write("Type a number betwee 1-6: ");
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        Console.Write("Please enter a valid number: ");
    }
}

static void Park(string[] parkingGarage)
{

}

static void Move(string[] parkingGarage)
{

}

static void Remove(string[] parkingGarage)
{

}

static void Search(string[] parkingGarage)
{

}

static void PrintSlots(string[] parkingGarage)
{
    SetDefaultSlotLabels(parkingGarage);

    foreach (string slot in parkingGarage)
    {
        Console.WriteLine(slot);
    }
}

static void Exit(string[] parkingGarage)
{

}

ReadMenuChoice();

Console.ReadKey(true);