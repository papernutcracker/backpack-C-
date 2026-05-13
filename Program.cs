namespace BackpackApp
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Backpack myBackpack = new Backpack();

            myBackpack.ItemAdded += delegate (BackpackItem item)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[EVENT] Added to backpack: {item.Name} (Volume: {item.Volume} L)");
                Console.ResetColor();
            };

            myBackpack.ItemRemoved += delegate (BackpackItem item)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[EVENT] Removed from backpack: {item.Name}");
                Console.ResetColor();
            };

            myBackpack.ItemChanged += delegate (BackpackItem item, double oldVolume)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n[EVENT] Volume of '{item.Name}' changed from {oldVolume} L to {item.Volume} L");
                Console.ResetColor();
            };

            Console.WriteLine("=== BACKPACK CREATION ===");
            Console.Write("Color: ");
            string color = Console.ReadLine();
            Console.Write("Brand: ");
            string brand = Console.ReadLine();
            Console.Write("Fabric: ");
            string fabric = Console.ReadLine();

            double weight = GetValidDouble("Weight (kg): ");
            double maxVolume = GetValidDouble("Max Volume (liters): ");

            myBackpack.SetCharacteristics(color, brand, fabric, weight, maxVolume);
            Console.WriteLine("\nBackpack successfully created and configured!");

            Console.WriteLine("Press any key to open the menu...");
            Console.ReadKey();

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();

                Console.WriteLine($"=== MENU (Occupied: {myBackpack.GetCurrentVolume()}/{myBackpack.MaxVolume} L) ===");
                Console.WriteLine("1. Add item");
                Console.WriteLine("2. Remove item");
                Console.WriteLine("3. Change item volume");
                Console.WriteLine("4. View backpack contents");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an action: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Item name: ");
                            string addName = Console.ReadLine();
                            double addVol = GetValidDouble("Item volume (liters): ");
                            myBackpack.AddItem(new BackpackItem(addName, addVol));
                            break;

                        case "2":
                            Console.Write("Enter the name of the item to remove: ");
                            string removeName = Console.ReadLine();
                            var itemToRemove = myBackpack.Contents.FirstOrDefault(i => i.Name.Equals(removeName, StringComparison.OrdinalIgnoreCase));

                            if (itemToRemove != null)
                                myBackpack.RemoveItem(itemToRemove);
                            else
                                Console.WriteLine("Item not found!");
                            break;

                        case "3":
                            Console.Write("Enter the name of the item to change: ");
                            string changeName = Console.ReadLine();
                            var itemToChange = myBackpack.Contents.FirstOrDefault(i => i.Name.Equals(changeName, StringComparison.OrdinalIgnoreCase));

                            if (itemToChange != null)
                            {
                                double newVol = GetValidDouble("New item volume (liters): ");
                                myBackpack.ChangeItemVolume(itemToChange, newVol);
                            }
                            else
                            {
                                Console.WriteLine("Item not found!");
                            }
                            break;

                        case "4":
                            Console.WriteLine("--- Backpack Contents ---");
                            if (myBackpack.Contents.Count == 0)
                            {
                                Console.WriteLine("The backpack is empty.");
                            }
                            else
                            {
                                foreach (var item in myBackpack.Contents)
                                {
                                    Console.WriteLine($"- {item.Name} ({item.Volume} L)");
                                }
                            }
                            Console.WriteLine("-------------------------");
                            break;

                        case "0":
                            isRunning = false;
                            Console.WriteLine("Program terminated.");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (BackpackOverflowException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[ERROR] {ex.Message}");
                    Console.ResetColor();
                }

                if (isRunning)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static double GetValidDouble(string prompt)
        {
            double result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                input = input.Replace(".", ",");

                if (double.TryParse(input, out result) && result > 0)
                {
                    return result;
                }
                Console.WriteLine("Invalid value. Please enter a positive number.");
            }
        }
    }
}
