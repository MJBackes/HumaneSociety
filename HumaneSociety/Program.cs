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
            //PointOfEntry.Run()        
            Animal animal = Query.GetAnimalByID(1);
            Client client = Query.GetClient("AAcompanado", "password");
            List<Adoption> adoptions = Query.GetPendingAdoptions();
            Query.UpdateAdoption(true, adoptions[0]);
            Query.RemoveAdoption(animal.AnimalId, client.ClientId);
            Console.ReadLine();
        }
    }
}
