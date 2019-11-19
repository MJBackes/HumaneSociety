using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
            //PointOfEntry.Run();
            Dictionary<int, string> input = new Dictionary<int, string>();
            input.Add(1, "1");
            UserInterface.DisplayAnimals(Query.SearchForAnimalsByMultipleTraits(input).ToList());
            Console.ReadLine();
        }
    }
}
