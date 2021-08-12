using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.API.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //GET,POST,PUT,DELETE
        #region method Get
        [HttpGet]
        public IActionResult GetAllEmployee()
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
            var sqlCommand = "SELECT * FROM Employee";
            var employees = dbConnection.Query<object>(sqlCommand);
            // Trả về cho client

            var response = StatusCode(200, employees);
            return response;
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(Guid employeeId)
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
            var sqlCommand = $"SELECT * FROM Employee WHERE EmployeeId = @EmployeeIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeIdParam", employeeId);
            var employee = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
            // Trả về cho client

            var response = StatusCode(200, employee);
            return response;
        }

        //[HttpGet("NewEmployeeCode")]
        //public IActionResult getNewEmployeeCode()
        //{
        //    //Truy cập vào database:
        //    // 1.Khai báo đối tượng
        //    var connectionString = "Host = 47.241.69.179;" +
        //        "Database = MISA.CukCuk_Demo_NVMANH;" +
        //        "User Id = dev;" +
        //        "Password = 12345678";
        //    // 2.Khởi tạo đối tượng kết nối với database
        //    IDbConnection dbConnection = new MySqlConnection(connectionString);

        //    //Thực hiện query lấy mảng mã nhân viên  từ csdl
        //    string sqlCommand = "SELECT EmployeeCode FROM Employee ORDER BY EmployeeCode DESC LIMIT 1";
        //    var employeeCode = dbConnection.QueryFirstOrDefault<string>(sqlCommand);

        //    //var Heading = ((IDictionary<string, object>)employeeCodeRow).Keys.ToArray();
        //    //var details = ((IDictionary<string, object>)employeeCodeRow);
        //    //var employeeCode = details[Heading[0]];
        //    // Xử lí sinh mã  mới
        //    int currentMax = 0;

        //    try
        //    {
        //        int codeValue = int.Parse(employeeCode.ToString().Split("-")[1]);
        //        if (currentMax < codeValue)
        //        {
        //            currentMax = codeValue;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        var errorResponse = StatusCode(500, 1);
        //        return errorResponse;
        //    }


        //    string newEmployeeCode = "NV-" + (currentMax + 1);
        //    var response = StatusCode(200, newEmployeeCode);
        //    return response;
        //}

        [HttpGet("{pagenumber}/{pagesize}")]
        public IActionResult pagination(int pagenumber, int pagesize)
        {
            //Truy cập vào database:
            // 1.Khai báo đối tượng
            var connectionString = "Host = 47.241.69.179;" +
                "Database = MISA.CukCuk_Demo_NVMANH;" +
                "User Id = dev;" +
                "Password = 12345678";
            // 2.Khởi tạo đối tượng kết nối với database
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            // 3.Bắt đầu phần trang


            var response = StatusCode(200);
            return response;
        }
        #endregion


        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid();
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
            var properties = employee.GetType().GetProperties();


            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(employee);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //thêm param tương ứng với mỗi property của đối tượng
                dynamicParam.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
            var sqlCommand = $"INSERT INTO Employee({columnsName}) VALUES({columnsParam}) ";

            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            // Trả về cho client

            var response = StatusCode(200, rowEffects);
            return response;
        }

        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Guid employeeId, Employee employee)
        {
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
            var properties = employee.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(employee);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //thêm param tương ứng với mỗi property của đối tượng
                dynamicParam.Add($"@{propName}", propValue);

                columnsName += $"{propName} = @{propName},";

            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);

            var sqlCommand = $"UPDATE Employee SET {columnsName} WHERE EmployeeId = @EmployeeIdParam ";

            dynamicParam.Add("@EmployeeIdParam", employeeId);
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

            // Trả về cho client
            var response = StatusCode(200, rowEffects);
            return response;
        }

        [HttpDelete("{employeeId}")]
        public IActionResult DeleteEmployee(Guid employeeId)
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
            var sqlCommand = $"DELETE FROM Employee WHERE EmployeeId = @EmployeeIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@EmployeeIdParam", employeeId);
            var rowEffects = dbConnection.Execute(sqlCommand, param: parameters);

            // 4.Trả về cho client
            var response = StatusCode(200, rowEffects);
            return response;
        }


    }
}
