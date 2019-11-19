using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }

        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            Employee employeeFromDB;
            try
            {
                switch (crudOperation)
                {
                    case "create":
                        db.Employees.InsertOnSubmit(employee);
                        db.SubmitChanges();
                        break;
                    case "read":
                        employeeFromDB = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();
                        UserInterface.DisplayEmployeeInfo(employeeFromDB);
                        break;
                    case "update":
                        employeeFromDB = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();
                        employeeFromDB.EmployeeNumber = employee.EmployeeNumber;
                        employeeFromDB.FirstName = employee.FirstName;
                        employeeFromDB.LastName = employee.LastName;
                        employeeFromDB.UserName = employee.UserName;
                        employeeFromDB.Password = employee.Password;
                        employeeFromDB.Email = employee.Email;
                        db.SubmitChanges();
                        break;
                    case "delete":
                        employeeFromDB = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();
                        db.Employees.DeleteOnSubmit(employeeFromDB);
                        db.SubmitChanges();
                        break;
                }
            }
            catch(NullReferenceException nullRef)
            {
                Console.WriteLine(nullRef);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            try
            {
                db.Animals.InsertOnSubmit(animal);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal static Animal GetAnimalByID(int id)
        {
            try
            {
                return db.Animals.Where(a => a.AnimalId == id).FirstOrDefault();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            Animal animalFromDb;
            animalFromDb = db.Animals.Where(e => e.AnimalId == animalId).FirstOrDefault();
            string input;
            if(updates.TryGetValue(1, out input))
            {
                int inputInt;
                if (int.TryParse(input, out inputInt))
                {
                    animalFromDb.CategoryId = inputInt;
                }
            }
            if(updates.TryGetValue(2, out input))
            {
                animalFromDb.Name = input;
            }
            if(updates.TryGetValue(3, out input))
            {
                int inputInt;
                if(int.TryParse(input, out inputInt))
                {
                    animalFromDb.Age = inputInt;
                }
            }
            if(updates.TryGetValue(4, out input))
            {
                animalFromDb.Demeanor = input;
            }
            if(updates.TryGetValue(5, out input))
            {
                if(input == "false")
                {
                    animalFromDb.KidFriendly = false;
                }
                else
                {
                    animalFromDb.KidFriendly = true;
                }               
            }
            if (updates.TryGetValue(6, out input))
            {
                if (input == "false")
                {
                    animalFromDb.PetFriendly = false;
                }
                else
                {
                    animalFromDb.PetFriendly = true;
                }

            }     
            if(updates.TryGetValue(7, out input))
            {
                int inputInt;
                if(int.TryParse(input, out inputInt))
                {
                    animalFromDb.Weight = inputInt;
                } 
            }
            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            try
            {
                db.Animals.DeleteOnSubmit(animal);
                db.SubmitChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
           
        }
        
        // TODO: Animal Multi-Trait Search
        internal static List<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> traits) // parameter(s)?
        {
            List<Animal> output = db.Animals.ToList();
            string input;
            if(traits.TryGetValue(1,out input))
            {
                int inputInt;
                if (int.TryParse(input, out inputInt))
                {
                    output = output.Where(a => a.CategoryId == inputInt).ToList() ;
                }
            }
            if (traits.TryGetValue(2, out input))
            {
                output = output.Where(a => a.Name == input).ToList();
            }
            if(traits.TryGetValue(3,out input))
            {
                int inputInt;
                if (int.TryParse(input, out inputInt))
                {
                    output = output.Where(a => a.Age == inputInt).ToList();
                }
            }
            if (traits.TryGetValue(4, out input))
            {
                output = output.Where(a => a.Demeanor == input).ToList();
            }
            if(traits.TryGetValue(5, out input))
            {
                bool inputBool;
                if (bool.TryParse(input, out inputBool))
                {
                    output = output.Where(a => a.KidFriendly == inputBool).ToList();
                }
            }
            if (traits.TryGetValue(6, out input))
            {
                bool inputBool;
                if (bool.TryParse(input, out inputBool))
                {
                    output = output.Where(a => a.PetFriendly == inputBool).ToList();
                }
            }
            if(traits.TryGetValue(7,out input))
            {
                int inputInt;
                if (int.TryParse(input, out inputInt))
                {
                    output = output.Where(a => a.Weight == inputInt).ToList();
                }
            }
            if (traits.TryGetValue(8,out input))
            {
                int inputInt;
                if (int.TryParse(input, out inputInt))
                {
                    output = output.Where(a => a.AnimalId == inputInt).ToList();
                }
            }
            return output.ToList();
        }
         
        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {
            throw new NotImplementedException();
        }
        
        internal static Room GetRoom(int animalId)
        {
            throw new NotImplementedException();
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            throw new NotImplementedException();
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            throw new NotImplementedException();
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            throw new NotImplementedException();
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            throw new NotImplementedException();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            throw new NotImplementedException();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}