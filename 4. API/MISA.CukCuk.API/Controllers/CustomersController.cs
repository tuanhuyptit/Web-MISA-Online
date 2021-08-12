using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.API.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// Lấy tất cả khách hàng
        /// </summary>
        /// <returns>list các khách hàng</returns>
        /// createdby Dvanh 12/8/2021
        //GET,POST,PUT,DELETE
        [HttpGet]
        
        public IActionResult GetCustomer()
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                // 3.Lấy dữ liệu
                var sqlCommand = "SELECT * FROM Customer";
                var customers = dbConnection.Query<object>(sqlCommand);
                // Trả về cho client
                // nếu không có bản ghi trả về status code 204
                if(customers.Count() == 0)
                {
                    return StatusCode(204);
                }
                // nếu có bản ghi trả về statuscode 200 cùng list các customer
                var response = StatusCode(200, customers);
                return response;

            }
            catch (Exception ex)
            {
                var exception = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Exception_Message,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                };
                return StatusCode(500, exception);
            }
       


        }
        /// <summary>
        /// lấy khách hàng theo id
        /// </summary>
        /// <param name="customerId">id của khách hàng</param>
        /// <returns>1 khách hàng</returns>
        /// createdby dvanh 12/8/2021
        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(Guid customerId)
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                // 3.Lấy dữ liệu
                var sqlCommand = $"SELECT * FROM Customer WHERE CustomerId = @CustomerIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CustomerIdParam", customerId);
                var customer = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
                // Trả về cho client
                // nếu không có khách hàng thì trả về 204
                if(customer != null)
                {
                    // nếu có khách hàng mã như thế thì trả về 200
                    var response = StatusCode(200, customer);

                    return response;
                }
                else
                {
                    return StatusCode(204);
                }

                
            }
            

             catch (Exception ex)
            {
                var exception = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Exception_Message,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                };
                return StatusCode(500, exception);
            }
        }
        /// <summary>
        /// Thêm 1 customer
        /// </summary>
        /// <param name="customer">object customer</param>
        /// <returns></returns>
        /// createdby dvanh 12/8/2021
        [HttpPost]
        public IActionResult InsertCustomer(Customer customer)
        {
            try
            {

                //Kiểm tra customercode
                if(customer.CustomerCode == "" || customer.CustomerCode == null)
                {
                    var exception = new
                    {
                        devMsg = "CustomerCode is blank",
                        userMsg = "Mã khách hàng " + Properties.Resource.Blank_colum,
                        errorCode = "misa-001",
                        moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                        traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                    };
                    return StatusCode(400, exception);
                }
                //Kiểm tra email

                bool isEmail = Regex.IsMatch(customer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if(isEmail == false)
                {
                    var exception = new
                    {
                        devMsg = "CustomerEmail is not correct format",
                        userMsg = "Email " + Properties.Resource.Fail_format,
                        errorCode = "misa-001",
                        moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                        traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                    };
                    return StatusCode(400, exception);
                }
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();

                // 3.Thêm dữ liệu vào database
                var columnsName = string.Empty;
                var columnsParam = string.Empty;

                //Đọc từng property của object:
                var properties = customer.GetType().GetProperties();


                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(customer);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"@{propName},";
                    columnsParam += $"@{propName},";
                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);
                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
                var sqlCommand = $"INSERT INTO Customer({columnsName}) VALUES({columnsParam}) ";

                // Trả về cho client

                var response = StatusCode(200, customer);
                return response;
            }
            catch (Exception ex)
            {
                var exception = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Exception_Message,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                };
                return StatusCode(500, exception);
            }
            
        }
        /// <summary>
        /// sửa khách hàng
        /// </summary>
        /// <param name="customerId">id khách hàng</param>
        /// <param name="customer">object khách hàng</param>
        /// <returns>số cột thay đổi</returns>
        /// createdby dvanh 12/8/2021
        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Guid customerId, Customer customer)
        {

            try
            {
                //Kiểm tra customercode
                if (customer.CustomerCode == "" || customer.CustomerCode == null)
                {
                    var exception = new
                    {
                        devMsg = "CustomerCode is blank",
                        userMsg = "Mã khách hàng " + Properties.Resource.Blank_colum,
                        errorCode = "misa-001",
                        moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                        traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                    };
                    return StatusCode(400, exception);
                }
                //Kiểm tra email

                bool isEmail = Regex.IsMatch(customer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail == false)
                {
                    var exception = new
                    {
                        devMsg = "CustomerEmail is not correct format",
                        userMsg = "Email " + Properties.Resource.Fail_format,
                        errorCode = "misa-001",
                        moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                        traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                    };
                    return StatusCode(400, exception);
                }

                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);
                //khai báo dynamicParam:
                var dynamicParam = new DynamicParameters();

                // 3.Thêm dữ liệu vào database
                var columnsName = string.Empty;

                //Đọc từng property của object:
                var properties = customer.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(customer);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName} = @{propName},";

                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                var sqlCommand = $"UPDATE Customer SET {columnsName} WHERE EmployeeId = @EmployeeIdParam ";

                dynamicParam.Add("@EmployeeIdParam", customerId);
                var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

                // Trả về cho client
                var response = StatusCode(200, rowEffects);
                return response;
            }
            catch (Exception ex)
            {
                var exception = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Exception_Message,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                };
                return StatusCode(500, exception);
            }

        }
        /// <summary>
        /// xóa khách hàng theo mã
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns> số cột bị thay đổi</returns>
<<<<<<< HEAD
        /// createdby dvanh 12/08/2021
=======
        /// createdby dvanh 12/8/2021
>>>>>>> f424eb411d875d0cac4dfec28f2d27526bc2680f
        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(Guid customerId)
        {
            try
            {
                //Truy cập vào database:
                // 1.Khai báo đối tượng
                var connectionString = "Host = 47.241.69.179;" +
                    "Database = MISA.CukCuk_Demo_NVMANH;" +
                    "User Id = dev;" +
                    "Password = 12345678";
                // 2.Khởi tạo đối tượng kết nối với database
                IDbConnection dbConnection = new MySqlConnection(connectionString);

                // 3.Lấy dữ liệu
                var sqlCommand = $"DELETE FROM Customer WHERE EmployeeId = @EmployeeIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@EmployeeIdParam", customerId);
                var rowEffects = dbConnection.Execute(sqlCommand, param: parameters);

                // 4.Trả về cho client
                var response = StatusCode(200, rowEffects);
                return response;
            }
            catch(Exception ex)
            {
                var exception = new
                {
                    devMsg = ex.Message,
                    userMsg = Properties.Resource.Exception_Message,
                    errorCode = "misa-001",
                    moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                    traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                };
                return StatusCode(500, exception);
            }
           
        }
    }
}
