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
    public class PositionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPosition()
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
            var sqlCommand = "SELECT * FROM Position";
            var departments = dbConnection.Query<object>(sqlCommand);
            // Trả về cho client

            var response = StatusCode(200, departments);
            return response;
        }


        [HttpGet("{positionId}")]
        public IActionResult GetPositionById(Guid positionId)
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
            var sqlCommand = $"SELECT * FROM  Position WHERE positionId = @PositionIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PositionIdParam", positionId);
            var position = dbConnection.QueryFirstOrDefault<object>(sqlCommand, param: parameters);
            // Trả về cho client

            var response = StatusCode(200, position);
            return response;
        }

        [HttpPost]
        public IActionResult InsertPosition([FromBody] Position position)
        {
            position.PositionId = Guid.NewGuid();
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
            var properties = position.GetType().GetProperties();


            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(position);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //thêm param tương ứng với mỗi property của đối tượng
                dynamicParam.Add($"@{propName}", propValue);

                columnsName += $"{propName},";
                columnsParam += $"@{propName},";
            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);
            columnsParam = columnsParam.Remove(columnsParam.Length - 1, 1);
            var sqlCommand = $"INSERT INTO Position ({columnsName}) VALUES ({columnsParam}) ";

            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);
            // Trả về cho client

            var response = StatusCode(200, rowEffects);
            return response;
        }

        [HttpPut("{positionId}")]
        public IActionResult UpdatePosition(Guid positionId, Position position)
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
            var properties = position.GetType().GetProperties();

            //Duyệt từng property:
            foreach (var prop in properties)
            {
                //lấy tên của prop:
                var propName = prop.Name;

                //Lấy value của prop:
                var propValue = prop.GetValue(position);

                //Lấy kiểu dữ liệu của prop:
                var propType = prop.PropertyType;

                //thêm param tương ứng với mỗi property của đối tượng
                dynamicParam.Add($"@{propName}", propValue);

                columnsName += $"{propName} = @{propName},";

            }
            columnsName = columnsName.Remove(columnsName.Length - 1, 1);

            var sqlCommand = $"UPDATE Position SET {columnsName} WHERE positionId = @PositionIdParam ";

            dynamicParam.Add("@PositionIdParam", positionId);
            var rowEffects = dbConnection.Execute(sqlCommand, param: dynamicParam);

            // Trả về cho client
            var response = StatusCode(200, rowEffects);
            return response;
        }

        [HttpDelete("{positionId}")]
        public IActionResult DeleteDepartment(Guid positionId)
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
            var sqlCommand = $"DELETE FROM Position WHERE PositionId = @PositionIdParam";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PositionIdParam", positionId);
            var rowEffects = dbConnection.Execute(sqlCommand, param: parameters);

            // 4.Trả về cho client
            var response = StatusCode(200, rowEffects);
            return response;
        }
    }
}
