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
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// lấy tất cả các phòng ban
        /// </summary>
        /// <returns>danh sách các phòng ban</returns>
        /// createdby dvanh 12/08/2021
        [HttpGet]
        public IActionResult GetAllDepartment()
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
                var sqlCommand = "SELECT * FROM Department";
                var departments = dbConnection.Query<object>(sqlCommand);
                // Trả về cho client
                //nếu ko có phòng ban nào thì trả về 204
                if(departments.Count() == 0)
                {
                    return StatusCode(204);
                }
                else
                {
                    var response = StatusCode(200, departments);
                    return response;
                }
                
                
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

        /// <summary>
        /// lấy phòng ban theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("{departmentId}")]
        public IActionResult GetDepartmentById(Guid departmentId)
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
                var sqlCommand = $"SELECT * FROM Department WHERE departmentId = @DepartmentIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DepartmentIdParam", departmentId);
                var department = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
                // Trả về cho client

                var response = StatusCode(200, department);
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
        /// thêm phòng ban
        /// </summary>
        /// <param name="department">object phòng ban</param>
        /// <returns></returns>
        /// createdby dvanh 12/08/2021
        [HttpPost]
        public IActionResult InsertDepartment([FromBody] Department department)
        {
            try
            {
                // check tên phòng ban
                if (department.DepartmentName =="" || department.DepartmentName == null)
                {
                    var exception = new
                    {
                        devMsg = "departmentname is blank",
                        userMsg = "Tên phòng ban " + Properties.Resource.Blank_colum,
                        errorCode = "misa-001",
                        moreInfo = "https://openapi.misa.com.vn/errorcode/misa-001",
                        traceId = "ba9587fd-1a79-4ac5-a0ca-2c9f74dfd3fb"

                    };
                    return StatusCode(400, exception);
                }
                department.DepartmentId = Guid.NewGuid();
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
                var properties = department.GetType().GetProperties();


                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(department);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName},";
                    columnsParam += $"@{propName},";
                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);
                columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
                var sqlCommand = $"INSERT INTO Department({columnsName}) VALUES({columnsParam}) ";

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
        /// Sửa thông tin phòng ban
        /// </summary>
        /// <param name="departmentId">id phòng ban</param>
        /// <param name="department">đối tượng phòng ban</param>
        /// <returns></returns>
        /// createdby dvanh 12/08/2021
        [HttpPut("{departmentId}")]
        public IActionResult UpdateDepartment(Guid departmentId, Department department)
        {

            try
            {
                // check tên phòng ban
                if (department.DepartmentName == "" || department.DepartmentName == null)
                {
                    var exception = new
                    {
                        devMsg = "departmentname is blank",
                        userMsg = "Tên phòng ban " + Properties.Resource.Blank_colum,
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
                var properties = department.GetType().GetProperties();

                //Duyệt từng property:
                foreach (var prop in properties)
                {
                    //lấy tên của prop:
                    var propName = prop.Name;

                    //Lấy value của prop:
                    var propValue = prop.GetValue(department);

                    //Lấy kiểu dữ liệu của prop:
                    var propType = prop.PropertyType;

                    //thêm param tương ứng với mỗi property của đối tượng
                    dynamicParam.Add($"@{propName}", propValue);

                    columnsName += $"{propName} = @{propName},";

                }
                columnsName = columnsName.Remove(columnsName.Length - 1, 1);

                var sqlCommand = $"UPDATE Department SET {columnsName} WHERE departmentId = @DepartmentIdParam ";

                dynamicParam.Add("@DepartmentIdParam", departmentId);
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
        /// xóa phòng ban
        /// </summary>
        /// <param name="departmentId">id của phòng ban</param>
        /// <returns></returns>
        /// createby dvanh 12/08/2021
        [HttpDelete("{departmentId}")]
        public IActionResult DeleteDepartment(Guid departmentId)
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
                var sqlCommand = $"DELETE FROM Department WHERE DepartmentId = @DepartmentIdParam";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DepartmentIdParam", departmentId);
                var rowEffects = dbConnection.Execute(sqlCommand, param: parameters);

                // 4.Trả về cho client
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
    }
}
