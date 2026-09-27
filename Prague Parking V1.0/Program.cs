string[] parkingGarage = new string[100];

parkingGarage[10] = "CAR#23Gf";

void SetDefaultSlotLabels(string[] parkingGarage)
{
    for (int i = 0; i < parkingGarage.Length; i++)
    {
        if (string.IsNullOrEmpty(parkingGarage[i]))
        {
            parkingGarage[i] = "Empty";
        }
    }
}

void PrintSlots(string[] parkingGarage)
{
    SetDefaultSlotLabels(parkingGarage);

    foreach (string slot in parkingGarage)
    {
        Console.WriteLine(slot);
    }
}

PrintSlots(parkingGarage);

Console.ReadKey(true);