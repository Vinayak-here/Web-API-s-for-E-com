using E_Com.Data;
using E_Com.DTO_s.ResponseDTO_s;
using E_Com.Models;
using E_Com.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace E_Com.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EComDbContext _dbContext;
        private readonly NpgsqlConnection _connection;


        public UserRepository(EComDbContext dbContext)
        {
            _dbContext = dbContext;
            _connection = _connection;


        }

        public async Task<List<TblUsers>> RegistrationAsync(TblUsers users)
        {
            var reg = _dbContext.TblUsers.Add(users);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.TblUsers.ToListAsync();
        }
        public async Task<TblUsers> LoginAsync(string username, string password)
        {
            return await _dbContext.TblUsers.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<TblUsers> FindinguserAsync(string email)
        {
            var user = await _dbContext.TblUsers.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
        public async Task<TblUsers> FindinguserById(int id)
        {
            var user = await _dbContext.TblUsers.FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }


        public async Task<bool> CheckIfUserExists(int id)
        {
            var exists = await _dbContext.TblUsers.AnyAsync(u => u.UserId == id);
            return exists;
        }

        public async Task<TblUsers> UpdatingUserAsync(TblUsers user)
        {
            _dbContext.TblUsers.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }


        public async Task<TblUsers> SellerRegistrationAPI(int id)
        {
            try
            {
                var user = await _dbContext.TblUsers.FirstOrDefaultAsync(u => u.UserId == id);
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Database update error occurred while fetching the user.", ex);
            }
            catch (Exception ex)

            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddToCart(TblCartItem tblCartItem)
        {
            try
            {
                _dbContext.TblCartItem.Add(tblCartItem);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                return false;
            }
        }

        public async Task<string> GetCartDetailsByCartId(int cartId)
        {
            string jsonResult = string.Empty;

            try
            {
                // Open a connection to the database
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        // Set up the stored procedure call without passing a value for the OUT parameter
                        command.CommandText = "CALL GetProductDetailsByCartIdUsingLoopReturningInJson(@cartId, NULL)";
                        command.CommandType = CommandType.Text;

                        // Add the cart ID parameter (input parameter)
                        var cartIdParam = command.CreateParameter();
                        cartIdParam.ParameterName = "@cartId";
                        cartIdParam.Value = cartId;
                        command.Parameters.Add(cartIdParam);

                        // Define the output parameter for the JSON result (OUT parameter)
                        var jsonResultParam = command.CreateParameter();
                        jsonResultParam.ParameterName = "@json_result";
                        jsonResultParam.DbType = DbType.String;  // Ensure the parameter is of the correct type
                        jsonResultParam.Direction = ParameterDirection.Output;
                        jsonResultParam.Size = -1;  // Use -1 for large output strings like JSON
                        command.Parameters.Add(jsonResultParam);

                        // Execute the procedure
                        await command.ExecuteNonQueryAsync();

                        // Retrieve the JSON result from the output parameter
                        jsonResult = jsonResultParam.Value?.ToString() ?? string.Empty; // Safely handle null values
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching cart details: {ex.Message}", ex);
            }

            return jsonResult; // Return the JSON string directly
        }


        public async Task<bool> CheckIfCartExists(int id)
        {
            return await _dbContext.TblCart
                .AnyAsync(u => u.CartId == id);
        }

        public async Task<bool> CheckIfCategoryExistsById(int id)
        {
            return await _dbContext.TblCategory
                .AnyAsync(u => u.CategoryId == id);
        }

        public async Task<string> GetProductsByCategoryId(int categoryId)
        {
            string jsonResult = string.Empty;

            try
            {
                // Open a connection to the database
                using (var connection = _dbContext.Database.GetDbConnection())
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        // Set up the stored procedure call with a placeholder for the OUT parameter
                        command.CommandText = "CALL getproductsbycategoryid(@category_id, NULL);";
                        command.CommandType = CommandType.Text;

                        // Add the category ID parameter (input parameter)
                        var categoryIdParam = command.CreateParameter();
                        categoryIdParam.ParameterName = "@category_id";
                        categoryIdParam.Value = categoryId;
                        categoryIdParam.DbType = DbType.Int32;
                        command.Parameters.Add(categoryIdParam);

                        // Define the output parameter for the JSON result (OUT parameter)
                        var jsonResultParam = command.CreateParameter();
                        jsonResultParam.ParameterName = "@json_result";
                        jsonResultParam.DbType = DbType.String;  // Ensure the parameter is of the correct type (string for JSON)
                        jsonResultParam.Direction = ParameterDirection.Output;
                        jsonResultParam.Size = -1;  // Use -1 for large output strings like JSON
                        command.Parameters.Add(jsonResultParam);

                        // Execute the procedure
                        await command.ExecuteNonQueryAsync();

                        // Retrieve the JSON result from the output parameter
                        jsonResult = jsonResultParam.Value?.ToString() ?? string.Empty; // Safely handle null values
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching products by category: {ex.Message}", ex);
            }

            return jsonResult; // Return the JSON string directly
        }


        const string functionQuery = "SELECT * FROM Fn_GetActiveFlashSaleProducts()";
        public async Task<List<FlashSalesProductsResponseDTO>> GetFlashSalesProducts()
        {
            // Reusing the injected connection
            var products = await _connection.QueryAsync<FlashSalesProductsResponseDTO>(functionQuery);
            return products.ToList();
        }


    }
}
