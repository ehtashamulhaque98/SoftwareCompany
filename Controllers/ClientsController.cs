using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareCompany.Models;
using System.Data.SqlClient;

namespace SoftwareCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public ClientsController(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]

        public IActionResult GetAllClients()
        {

            
            var Clients = new List<Client>();

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("select * from Clients", sqlConnection);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Clients.Add(new Client()
                            {
                                ClientID = reader.GetInt32(0),
                                ClientName = reader.GetString(1),
                                Email = reader.GetString(2),
                                PhoneNumber = reader.GetString(3),
                                Address = reader.GetString(4),
                                City = reader.GetString(5),
                                Country = reader.GetString(6),

                            });
                        }
                    }
                }

                return Ok(Clients);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] Client newClient)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("INSERT INTO Clients (ClientName, Email, PhoneNumber, Address, City, Country) " +
                                                           "VALUES (@ClientName, @Email, @PhoneNumber, @Address, @City, @Country)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ClientName", newClient.ClientName);
                    sqlCommand.Parameters.AddWithValue("@Email", newClient.Email);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", newClient.PhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Address", newClient.Address);
                    sqlCommand.Parameters.AddWithValue("@City", newClient.City);
                    sqlCommand.Parameters.AddWithValue("@Country", newClient.Country);

                    sqlCommand.ExecuteNonQuery();
                }

                return Ok("Client created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] Client updatedClient)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("UPDATE Clients SET ClientName = @ClientName, Email = @Email, PhoneNumber = @PhoneNumber, " +
                                                           "Address = @Address, City = @City, Country = @Country WHERE ClientID = @ClientID", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ClientID", id);
                    sqlCommand.Parameters.AddWithValue("@ClientName", updatedClient.ClientName);
                    sqlCommand.Parameters.AddWithValue("@Email", updatedClient.Email);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", updatedClient.PhoneNumber);
                    sqlCommand.Parameters.AddWithValue("@Address", updatedClient.Address);
                    sqlCommand.Parameters.AddWithValue("@City", updatedClient.City);
                    sqlCommand.Parameters.AddWithValue("@Country", updatedClient.Country);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Client not found");
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
        public IActionResult DeleteClient(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM Clients WHERE ClientID = @ClientID", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@ClientID", id);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Client not found");
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
