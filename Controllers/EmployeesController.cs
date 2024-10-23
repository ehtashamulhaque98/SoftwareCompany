using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareCompany.Models;
using System.Data.SqlClient;

namespace SoftwareCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]

        public IActionResult GetAllEmployees()
        {

            
            var Employees = new List<Employee>();

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("select * from Employees", sqlConnection);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employees.Add(new Employee()
                            {
                                EmployeeID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                PhoneNumber = reader.GetString(3),
                                Gender = reader.GetString(4),
                                DepartmentID = reader.GetInt32(5),
                                Department = reader.GetString(6),
                                HireDate = reader.GetDateTime(7),
                                Salary = reader.GetInt32(8),

                            });
                        }
                    }
                }

                return Ok(Employees);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee newEmployee)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO Employees (Name, Email, PhoneNumber, Gender, DepartmentID, HireDate, Salary) " +
                                                           "VALUES (@Name, @Email, @PhoneNumber, @Gender, @DepartmentID, @HireDate, @Salary)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Name", newEmployee.Name);
                    sqlCommand.Parameters.AddWithValue("@Email", newEmployee.Email);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", newEmployee.PhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Gender", newEmployee.Gender);
                    sqlCommand.Parameters.AddWithValue("@DepartmentID", newEmployee.DepartmentID);
                    sqlCommand.Parameters.AddWithValue("@HireDate", newEmployee.HireDate);
                    sqlCommand.Parameters.AddWithValue("@Salary", newEmployee.Salary);

                    sqlCommand.ExecuteNonQuery();
                }

                return Ok("Employee created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("UPDATE Employees SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber, Gender = @Gender, " +
                                                           "DepartmentID = @DepartmentID, HireDate = @HireDate, Salary = @Salary WHERE EmployeeID = @EmployeeID", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@EmployeeID", id);
                    sqlCommand.Parameters.AddWithValue("@Name", updatedEmployee.Name);
                    sqlCommand.Parameters.AddWithValue("@Email", updatedEmployee.Email);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", updatedEmployee.PhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Gender", updatedEmployee.Gender);
                    sqlCommand.Parameters.AddWithValue("@DepartmentID", updatedEmployee.DepartmentID);
                    sqlCommand.Parameters.AddWithValue("@HireDate", updatedEmployee.HireDate);
                    sqlCommand.Parameters.AddWithValue("@Salary", updatedEmployee.Salary);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Employee not found");
                    }

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@EmployeeID", id);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Employee not found");
                    }

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }   
    }
}
