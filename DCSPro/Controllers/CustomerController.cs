using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DCSPro.Models;

namespace DCSPro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // A. Create Customer (POST)
        [HttpPost("create")]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Customer (UserId, Username, Email, FirstName, LastName, CreatedOn, IsActive) VALUES (@UserId, @Username, @Email, @FirstName, @LastName, @CreatedOn, @IsActive)", con);

                    cmd.Parameters.AddWithValue("@UserId", customer.UserId);
                    cmd.Parameters.AddWithValue("@Username", customer.Username);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                    cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);

                    cmd.ExecuteNonQuery();
                }

                return Ok("Customer created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // B. Get All Customers (GET)
        [HttpGet("all")]
        public IActionResult GetAllCustomers()
        {
            try
            {
                List<Customer> customerList = new List<Customer>();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            UserId = Guid.Parse(reader["UserId"].ToString()),
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString()),
                            IsActive = bool.Parse(reader["IsActive"].ToString())
                        };

                        customerList.Add(customer);
                    }
                }

                return Ok(customerList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // C. Update Customer (POST)
        [HttpPost("update")]
        public IActionResult UpdateCustomer([FromBody] Customer customer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE Customer SET Username = @Username, Email = @Email, FirstName = @FirstName, LastName = @LastName, CreatedOn = @CreatedOn, IsActive = @IsActive WHERE UserId = @UserId", con);

                    cmd.Parameters.AddWithValue("@UserId", customer.UserId);
                    cmd.Parameters.AddWithValue("@Username", customer.Username);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                    cmd.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                    cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);

                    cmd.ExecuteNonQuery();
                }

                return Ok("Customer updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // D. DELETE Customer (DELETE)
        [HttpDelete("delete/{userId}")]
        public IActionResult DeleteCustomer(Guid userId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE UserId = @UserId", con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Customer deleted successfully.");
                    }
                    else
                    {
                        return NotFound("No customer found with the given UserId.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // E. Active Orders by Customer (GET)
        [HttpGet("active-orders/{userId}")]
        public IActionResult GetActiveOrdersByCustomer(Guid userId)
        {
            try
            {
                List<Order> activeOrders = new List<Order>();
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("SQLConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [Order] WHERE OrderBy = @UserId AND IsActive = 1", con);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Order order = new Order
                        {
                            OrderId = Guid.Parse(reader["OrderId"].ToString()),
                            ProductId = Guid.Parse(reader["ProductId"].ToString()),
                            OrderStatus = int.Parse(reader["OrderStatus"].ToString()),
                            OrderType = int.Parse(reader["OrderType"].ToString()),
                            OrderBy = Guid.Parse(reader["OrderBy"].ToString()),
                            OrderedOn = DateTime.Parse(reader["OrderedOn"].ToString()),
                            ShippedOn = reader["ShippedOn"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["ShippedOn"].ToString()),
                            IsActive = bool.Parse(reader["IsActive"].ToString())
                        };

                        activeOrders.Add(order);
                    }
                }

                return Ok(activeOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
