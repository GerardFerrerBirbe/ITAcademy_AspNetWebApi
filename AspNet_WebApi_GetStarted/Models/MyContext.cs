using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet_WebApi_GetStarted.Models
{
    public class MyContext : DbContext, IDisposable
    {
        public MySqlConnection Connection { get; set; }
                
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        //Constructor per la connexió a MySql:
        public MyContext(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose() => Connection.Dispose();

        public DbSet<Employee> Employees { get; set; }

        //Llista per la persistència amb MySQL:
        public List<Employee> EmployeesList { get; set; }
    }
}
