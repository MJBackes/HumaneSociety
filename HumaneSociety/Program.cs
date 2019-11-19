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
            Employee employee = new Employee() { EmployeeId = 1, FirstName = "a", LastName = "b", UserName = "ab", Password = "pass", EmployeeNumber = 1 };
            Query.RunEmployeeQueries(employee, "update");
            Query.RunEmployeeQueries(employee, "read");
        }
    }
}
