using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    internal static class CSVImporter
    {
        internal static void ImportCSV(string path)
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] animalInfo = FilterOutQuotationMarks(line).Split(',');
                string name = animalInfo[0];
                int? weight = int.TryParse(animalInfo[1], out int Weight) ? Weight : (int?)null;
                int? age = int.TryParse(animalInfo[2], out int Age) ? Age : (int?)null;
                string demeanor = animalInfo[3];
                bool? kidFriendly = (animalInfo[4] == "1" || animalInfo[4] == "true" || animalInfo[4].ToUpper() == "T"
                                    || animalInfo[4].ToUpper() == "YES" || animalInfo[4].ToUpper() == "Y");
                bool? petFriendly = (animalInfo[5] == "1" || animalInfo[5] == "true" || animalInfo[5].ToUpper() == "T"
                                    || animalInfo[5].ToUpper() == "YES" || animalInfo[5].ToUpper() == "Y");
                string gender = animalInfo[6];
                string adoptionStatus = animalInfo[7];
                int? categoryId = int.TryParse(animalInfo[8], out int CategoryId) ? CategoryId : (int?)null;
                int? dietId = int.TryParse(animalInfo[9], out int Diet) ? Diet : (int?)null;

                Query.AddAnimal(new Animal
                {
                    Name = name,
                    Weight = weight,
                    Age = age,
                    Demeanor = demeanor,
                    KidFriendly = kidFriendly,
                    PetFriendly = petFriendly,
                    Gender = gender,
                    AdoptionStatus = adoptionStatus,
                    CategoryId = categoryId,
                    DietPlanId = dietId,
                    EmployeeId = null
                });
            }
        }
        private static string FilterOutQuotationMarks(string input)
        {
            char[] output = input.ToCharArray();
            output = output.Where(c => c != '"').ToArray();
            StringBuilder sb = new StringBuilder("");
            foreach (char c in output)
                sb.Append(c);
            return sb.ToString();
        }
    }
}
