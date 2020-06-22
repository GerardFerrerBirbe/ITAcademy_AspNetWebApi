using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet_WebApi_GetStarted.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobPosition { get; set; }
        public double Salary { get; set; }
        public string Secret { get; set; } //Camp 'secret', per explicar la part del tutorial de microsoft docs de prevenció d'excés de publicació
    }
    
}
