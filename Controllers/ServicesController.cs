using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareCompany.Models;
using System.Data.SqlClient;

namespace SoftwareCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public ServicesController(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]

        public IActionResult GetAllServices()
        {


            var services = new List<Service>();

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("select * from Services", sqlConnection);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            services.Add(new Service()
                            {
                                ServiceID = reader.GetInt32(0),
                                ServiceName = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetDecimal(3),

                            });
                        }
                    }
                }

                return Ok(services);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateService([FromBody] Service newService)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO Services (ServiceName, Description, Price) VALUES (@ServiceName, @Description, @Price)",
                        sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ServiceName", newService.ServiceName);
                    sqlCommand.Parameters.AddWithValue("@Description", newService.Description);
                    sqlCommand.Parameters.AddWithValue("@Price", newService.Price);

                    sqlCommand.ExecuteNonQuery();
                }

                return Ok("Service created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateService(int id, [FromBody] Service updatedService)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(
                        "UPDATE Services SET ServiceName = @ServiceName, Description = @Description, Price = @Price WHERE ServiceID = @ServiceID",
                        sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ServiceID", id);
                    sqlCommand.Parameters.AddWithValue("@ServiceName", updatedService.ServiceName);
                    sqlCommand.Parameters.AddWithValue("@Description", updatedService.Description);
                    sqlCommand.Parameters.AddWithValue("@Price", updatedService.Price);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Service not found");
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
        public IActionResult DeleteService(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM Services WHERE ServiceID = @ServiceID", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@ServiceID", id);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Service not found");
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
