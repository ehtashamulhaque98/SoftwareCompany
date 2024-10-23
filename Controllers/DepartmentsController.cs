using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareCompany.Models;
using System.Data.SqlClient;

namespace SoftwareCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public DepartmentsController(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]

        public IActionResult GetAllDepartments()
        {

            
            var Departments = new List<Department>();

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("select * from Departments", sqlConnection);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Departments.Add(new Department()
                            {
                                DepartmentID = reader.GetInt32(0),
                                DepartmentName = reader.GetString(1),
                            });
                        }
                    }
                }

                return Ok(Departments);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateDepartment([FromBody] Department newDepartment)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@DepartmentName", newDepartment.DepartmentName);

                    sqlCommand.ExecuteNonQuery();
                }

                return Ok("Department created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] Department updatedDepartment)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("UPDATE Departments SET DepartmentName = @DepartmentName WHERE DepartmentID = @DepartmentID", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@DepartmentID", id);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", updatedDepartment.DepartmentName);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Department not found");
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
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM Departments WHERE DepartmentID = @DepartmentID", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@DepartmentID", id);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Department not found");
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
