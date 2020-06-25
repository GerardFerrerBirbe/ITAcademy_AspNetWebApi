using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNet_WebApi_GetStarted.Models;

namespace AspNet_WebApi_GetStarted.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly MyContext _context;

        public EmployeesController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            //SQL Server:
            //return await _context.Employees
            //    .Select(x => EmployeeToDTO(x))
            //    .ToListAsync();

            //MySQL:
            await _context.Connection.OpenAsync();
            var query = new Employee(_context);
            var employees = await query.GetEmployees();
            return new OkObjectResult(employees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            // Opció persistència amb SQL Server:
            //var employee = await _context.Employees.FindAsync(id);

            //Opció persistència amb MySQL:
            await _context.Connection.OpenAsync();
            var query = new Employee(_context);
            var employee = await query.GetEmployeeById(id);
            //---

            if (employee == null)
            {
                return NotFound();
            }

            //Opció amb SQL Server:
            //return EmployeeToDTO(employee);

            //Opció amb MySQL:
            return new OkObjectResult(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, EmployeeDTO employeeDTO)
        {
            // Opció persistència amb SQL Server:
            //if (id != employeeDTO.Id)
            //{
            //    return BadRequest();
            //}            
            //var employee = await _context.Employees.FindAsync(id);

            // Opció persistència amb MySQL:
            await _context.Connection.OpenAsync();
            var query = new Employee(_context);
            var employee = await query.GetEmployeeById(id);
            //---

            if (employee == null)
            {
                return NotFound();
            }

            employee.FirstName = employeeDTO.FirstName;
            employee.LastName = employeeDTO.LastName;
            employee.JobPosition = employeeDTO.JobPosition;
            employee.Salary = employeeDTO.Salary;

            //Opció persitència amb SQL Server:
            //_context.Entry(employee).State = EntityState.Modified;
            //try
            //{                
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmployeeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();

            await employee.UpdateEmployee();
            return new OkObjectResult(employee);
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            //SQL Server:
            //var employee = new Employee
            //{
            //    FirstName = employeeDTO.FirstName,
            //    LastName = employeeDTO.LastName,
            //    JobPosition = employeeDTO.JobPosition,
            //    Salary = employeeDTO.Salary
            //};            
            //_context.Employees.Add(employee);
            //await _context.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, EmployeeToDTO(employee));

            //MySql:
            await _context.Connection.OpenAsync();
            employee._context = _context;            
            await employee.PostEmployee();
            return new OkObjectResult(employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            //SQL Server:
            //var employee = await _context.Employees.FindAsync(id);

            //MySQL:
            await _context.Connection.OpenAsync();
            var query = new Employee(_context);
            var employee = await query.GetEmployeeById(id);
            //---

            if (employee == null)
            {
                return NotFound();
            }

            //SQL Server:
            //_context.Employees.Remove(employee);
            //await _context.SaveChangesAsync();

            //return NoContent();

            //MySQL:
            await employee.DeleteEmployee();
            return new OkResult();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }


        private static EmployeeDTO EmployeeToDTO(Employee employee) =>
            new EmployeeDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                JobPosition = employee.JobPosition,
                Salary = employee.Salary
            };
    }
}
