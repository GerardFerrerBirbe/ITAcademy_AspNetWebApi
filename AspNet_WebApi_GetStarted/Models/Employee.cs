using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public Employee()
        {
        }

        internal MyContext _context { get; set; }
        internal Employee(MyContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            using var cmd = _context.Connection.CreateCommand();
            cmd.CommandText =
                @"SELECT `Id`, `FirstName`, `LastName`, `JobPosition`, `Salary`
                FROM `Employee` WHERE `Id` = @id";

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var employee = await ReadEmployees(await cmd.ExecuteReaderAsync());
            return employee.Count > 0 ? employee[0] : null;        
        }

        public async Task<List<Employee>> GetEmployees()
        {
            using var cmd = _context.Connection.CreateCommand();
            cmd.CommandText =
                @"SELECT `Id`, `FirstName`, `LastName`, `JobPosition`, `Salary`
                FROM `Employee`";

            return await ReadEmployees(await cmd.ExecuteReaderAsync());
        }

        public async Task<List<Employee>> ReadEmployees(DbDataReader reader)
        {
            var employees = new List<Employee>();

            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var employee = new Employee(_context)
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        JobPosition = reader.GetString(3),
                        Salary = reader.GetFloat(4)
                    };
                    employees.Add(employee);
                }
            }
            return employees;
        }        

        public async Task PostEmployee()
        {
            using var cmd = _context.Connection.CreateCommand();
            cmd.CommandText =
                @"INSERT INTO `Employee`
                (`FirstName`, `LastName`,`JobPosition`, `Salary`)
                VALUES (@firstName, @lastName, @jobPosition, @salary);";
            
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            Id = (int)cmd.LastInsertedId;
        }

        public async Task UpdateEmployee()
        {
            using var cmd = _context.Connection.CreateCommand();
            cmd.CommandText =
                @"UPDATE `Employee`
                SET `FirstName` = @firstName, `LastName` = @lastName,`JobPosition` = @jobPosition, `Salary` = @salary
                WHERE `Id` = @id";

            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteEmployee()
        {
            using var cmd = _context.Connection.CreateCommand();
            cmd.CommandText =
                @"DELETE FROM `Employee` WHERE `Id` = @id;";

            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = Id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstName",
                DbType = DbType.String,
                Value = FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastName",
                DbType = DbType.String,
                Value = LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@jobPosition",
                DbType = DbType.String,
                Value = JobPosition,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@salary",
                DbType = DbType.String,
                Value = Salary,
            });
        }
    }    
}
