// See https://aka.ms/new-console-template for more information


using StreetwearTracker;
using System.IO;
using System.Linq;
using System.Xml.Linq;




while (true)
{
    Console.WriteLine("1: Add New Item");
    Console.WriteLine("2: View Inventory");
    Console.WriteLine("3: Exit");
    Console.WriteLine("4: Mark Item as Sold / Delete");
    Console.WriteLine("5: Search Inventory by Brand");
    Console.WriteLine("6: View Portfolio Financial Summary");
    Console.WriteLine("7: Sort Inventory by Price");
    Console.WriteLine("8: Filter by Minimum Profit");
   
   
    

    string input = Console.ReadLine();
    int inp = int.Parse(input);

    

    switch (inp)
    {
        case 1:
            Console.Write("Enter Brand: ");
            string brand = Console.ReadLine();

            Console.Write("Enter Item Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Buy Price: ");
            decimal buy = decimal.Parse(Console.ReadLine());

            Console.Write("Enter Target Price: ");
            decimal target = decimal.Parse(Console.ReadLine());

            // Connect to the database and save permanently
            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();

                ResaleItem newItem = new ResaleItem
                {
                    Brand = brand,
                    ItemName = name,
                    BuyPrice = buy,
                    TargetPrice = target
                };

                context.Items.Add(newItem); // Stage the item on our table
                context.SaveChanges();      // Commit it permanently to inventory.db
            }

            Console.WriteLine("Item successfully saved to database!");
            break;

        case 2:
            decimal totalInvestment = 0;
            decimal totalProfit = 0;
            using (var context = new AppDbContext())
            {
                var con = context.Items.ToList();
                foreach (var item in con)
                {
                    Console.WriteLine
                        ($"Brand: {item.Brand}, " +
                        $"Item name: {item.ItemName}, " +
                        $"Buy Price: {item.BuyPrice}, " +
                        $"Target Price: {item.TargetPrice}");
                }

            }
            break;


        case 3:
            Console.WriteLine("You sure you want to quit? y or n");
            string yorn = Console.ReadLine();
            if (yorn == "y")
            {
                Console.WriteLine("Goodbye!");
                return;
            }
            else if (yorn == "n")
            {
                continue;
            }
            break;

        case 4:
            Console.WriteLine("What item name would you like to be removed?");
            string put = Console.ReadLine();
            
            using (var context = new AppDbContext())
            {
               var con = context.Items.FirstOrDefault(x => x.ItemName == put );
                if (con == null)
                {
                    Console.WriteLine("Your input does not exist");
                    continue;
                }
                else
                {
                    context.Items.Remove(con);
                    Console.WriteLine($"{con} got removed off the inventory!");
                    context.SaveChanges();
                }

                    
                
            }
            
            
            break;


        case 5:
            Console.WriteLine("Enter brand to search for:");
            string type = Console.ReadLine();

            using (var context = new AppDbContext())
            {
               var c = context.Items.ToList();
               bool found = false;

                foreach (var k in c )
                {
                    if (k.Brand.ToLower() == type.ToLower())
                    {
                        Console.WriteLine($"Your brand - {k.Brand}, Item name - {k.ItemName}, Target Price: {k.TargetPrice} ");
                        found = true;
                    }

                }
                if (!found)
                {
                    Console.WriteLine("Cant find that name try again.");
                }
            }

            break;

        case 6:
        
        
            using (var context = new AppDbContext())
            {
                var items = context.Items.ToList();

                decimal totalInvested = items.Sum(x => x.BuyPrice);
                decimal totalTargetValue = items.Sum(x => x.TargetPrice);
                decimal total = totalTargetValue - totalInvested;

                Console.WriteLine($"Total Money Invested: £{totalInvested}");
                Console.WriteLine($"Total Target Value: £{totalTargetValue}");
                Console.WriteLine($"Your Projected Profit: £{total}");
            }
       
            break;


        case 7:
            using (var context = new AppDbContext())
            {

                var order = context.Items.ToList().OrderByDescending(x => x.TargetPrice);
                
                Console.WriteLine("Your items from highest to lowest price");
                foreach (ResaleItem p in order)
                {
                    Console.WriteLine($"Item Name: {p.ItemName}, Brand: {p.Brand}, Buy Price: {p.BuyPrice}, Target Price: {p.TargetPrice}");
                }
            }
            
           
            break;

        case 8:
            Console.WriteLine("Enter your minimum profit amount: ");
            string xinp = Console.ReadLine();
            decimal mp = decimal.Parse(xinp);
            using (var context = new AppDbContext())
            {
                var mpa = context.Items.Where(x => x.TargetPrice - x.BuyPrice >= mp);
                if (!mpa.Any())
                {
                    Console.WriteLine("Nothing was found");

                }
                else
                {
                    foreach (ResaleItem o in mpa)

                    {
                        Console.WriteLine($"Item Name: {o.ItemName}, Brand: {o.Brand}, Buy Price: {o.BuyPrice}, Target Price: {o.TargetPrice}");
                    }

                }

            }
            
                
            break;



    }

}
void SaveInventory(List<ResaleItem> inventory)
{
    string json = System.Text.Json.JsonSerializer.Serialize(inventory);
    File.WriteAllText("inventory.json", json);
}
public class ResaleItem
{

    public int Id { get; set; }
    public string ItemName { get; set; }
    public string Brand { get; set; }

    public decimal BuyPrice { get; set; }

    public decimal TargetPrice { get; set; }
}
  
