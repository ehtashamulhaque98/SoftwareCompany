using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftwareCompany.Models;
using System.Data.SqlClient;

namespace SoftwareCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public ProjectsController(IConfiguration configuration)
        {
            _configuration = configuration;

            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]

        public IActionResult GetAllProjects()
        {

           
            var projects = new List<Project>();

            try
            {

                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("select * from Projects", sqlConnection);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projects.Add(new Project()
                            {
                                ProjectID = reader.GetInt32(0),
                                ProjectName = reader.GetString(1),
                                ClientID = reader.GetInt32(2),
                                ServiceID = reader.GetInt32(3),
                                AssignedEmployeeID = reader.GetInt32(4),
                                StartDate = reader.GetDateTime(5),
                                EndDate = reader.GetDateTime(6),
                                Status = reader.GetString(7),

                            });
                        }
                    }
                }

                return Ok(projects);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody] Project newProject)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO Projects (ProjectName, ClientID, ServiceID, AssignedEmployeeID, StartDate, EndDate, Status) " +
                        "VALUES (@ProjectName, @ClientID, @ServiceID, @AssignedEmployeeID, @StartDate, @EndDate, @Status)",
                        sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ProjectName", newProject.ProjectName);
                    sqlCommand.Parameters.AddWithValue("@ClientID", newProject.ClientID);
                    sqlCommand.Parameters.AddWithValue("@ServiceID", newProject.ServiceID);
                    sqlCommand.Parameters.AddWithValue("@AssignedEmployeeID", newProject.AssignedEmployeeID);
                    sqlCommand.Parameters.AddWithValue("@StartDate", newProject.StartDate);
                    sqlCommand.Parameters.AddWithValue("@EndDate", newProject.EndDate);
                    sqlCommand.Parameters.AddWithValue("@Status", newProject.Status);

                    sqlCommand.ExecuteNonQuery();
                }

                return Ok("Project created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, [FromBody] Project updatedProject)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand(
                        "UPDATE Projects SET ProjectName = @ProjectName, ClientID = @ClientID, ServiceID = @ServiceID, " +
                        "AssignedEmployeeID = @AssignedEmployeeID, StartDate = @StartDate, EndDate = @EndDate, Status = @Status " +
                        "WHERE ProjectID = @ProjectID",
                        sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@ProjectID", id);
                    sqlCommand.Parameters.AddWithValue("@ProjectName", updatedProject.ProjectName);
                    sqlCommand.Parameters.AddWithValue("@ClientID", updatedProject.ClientID);
                    sqlCommand.Parameters.AddWithValue("@ServiceID", updatedProject.ServiceID);
                    sqlCommand.Parameters.AddWithValue("@AssignedEmployeeID", updatedProject.AssignedEmployeeID);
                    sqlCommand.Parameters.AddWithValue("@StartDate", updatedProject.StartDate);
                    sqlCommand.Parameters.AddWithValue("@EndDate", updatedProject.EndDate);
                    sqlCommand.Parameters.AddWithValue("@Status", updatedProject.Status);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Project not found");
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
        public IActionResult DeleteProject(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();

                    SqlCommand sqlCommand = new SqlCommand("DELETE FROM Projects WHERE ProjectID = @ProjectID", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@ProjectID", id);

                    int rowsAffected = sqlCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Project not found");
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
