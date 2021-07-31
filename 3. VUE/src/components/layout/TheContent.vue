
<template>
 <div class="content">
            <div class="content-header">
                <div class="title">Danh sách nhân viên</div>
                <Button iconName= "icon-add" buttonText = "Thêm nhân viên" id = "btnAdd"/>

            </div>
            <div class="filter">
                <div class="d-flex">
                    <FieldInputIcon />
                    <div class="department">
                        <Dropdown DropdownText = "Tất cả phòng ban" />
                        <DropdownDetail dd_dropdown = "dd-Department" Url = "api/Department" itemId = "DepartmentId" itemName = "DepartmentName"/>
                    </div>
                    <div class="position">
                        <Dropdown DropdownText = "Tất cả vị trí" />
                        <DropdownDetail  dd_dropdown = "dd-Position" Url = "v1/Positions" itemId = "PositionId" itemName = "PositionName"/>
                    </div>
                </div>
                <div class="refresh"></div>
            </div>
            <!-- Phần dùng riêng từng màn hình -->
            <!-- <div id="gridEntity" Url="v1/Employees" ItemId="EmployeeId">
                <div class="d-none">
                    <div class="column" FieldName="EmployeeCode">Mã nhân viên</div>
                    <div class="column" FieldName="FullName">Họ và tên</div>
                    <div class="column" FieldName="Gender" DataType="Enum" EnumName="Gender">Giới tính</div>
                    <div class="column" FieldName="DateOfBirth" DataType="Date">Ngày sinh</div>
                    <div class="column" FieldName="PhoneNumber">Điện thoại</div>
                    <div class="column" FieldName="Email">Email</div>
                    <div class="column" FieldName="PositionName" DataType="Enum">Chức vụ</div>
                    <div class="column" FieldName="DepartmentName" DataType="Enum">Phòng ban</div>
                    <div class="column" FieldName="Salary" DataType="Number">Mức lương cơ bản</div>
                    <div class="column" FieldName="WorkStatus" DataType="Enum">Tình trạng công việc</div>
                </div>
            </div> -->
            <div id="gridEntity">
            <table>
                <thead>
                    <tr>
                        <th>Mã nhân viên</th>
                        <th>Họ và tên</th>
                        <th>Giới tính</th>
                        <th>Ngày sinh</th>
                        <th>Điện thoại</th>
                        <th>Email</th>
                        <th>Chức vụ</th>
                        <th>Phòng ban</th>
                        <th>Mức lương cơ bản</th>
                        <th>Tình trạng công việc</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for = "employee in employees" :key = "employee.EmployeeId">
                        <td>{{employee.EmployeeCode}}</td>
                        <td>{{employee.FullName}}</td>
                        <td>{{employee.GenderName}}</td>
                        <td>{{employee.DateOfBirth}}</td>
                        <td>{{employee.PhoneNumber}}</td>
                        <td>{{employee.Email}}</td>
                        <td>{{employee.PositionName}}</td>
                        <td>{{employee.DepartmentName}}</td>
                        <td>{{employee.Salary}}</td>
                        <td>{{employee.WorkStatus}}</td>
                    </tr>
                   
                </tbody>
            </table>
        </div>
            <div class="page-navigator">
                <!-- <div class="ml-10" id="div1-paging"></div> -->
                 <div class="ml-10 ">Hiển thị 1-10/1000 nhân viên</div>
                <div class="paging">
                    <div class="btn common-page first-page"></div>
                    <div class="btn common-page prev-page"></div>
                    <div class="btn page-number">1</div>
                    <div class="btn page-number">2</div>
                    <div class="btn page-number">3</div>
                    <div class="btn page-number">4</div>
                    <div class="btn common-page next-page"></div>
                    <div class="btn common-page last-page"></div>
                </div>
                <div class="mr-10" id="div2-paging">10 nhân viên/trang</div>
            </div>
        </div>
</template>

<script>
import axios from "axios";

import Button from "../base/BaseButton.vue";
import Dropdown from "../base/BaseDropdown.vue";
import DropdownDetail from "../base/BaseDropdownDetail";
import FieldInputIcon from "../base/BaseFieldInputIcon.vue";
export default {
    name: 'Content',
    props: {
        msg: String
    },
    components: {
        Button,
        Dropdown,
        DropdownDetail,
        FieldInputIcon,
    },

    created(){
        let me = this;
        axios.get("http://cukcuk.manhnv.net/v1/Employees").then(res => {
            me.employees = res.data;
        }).catch(res => {
            console.log(res)
        })
    },
    
    data(){
        return {
            employees : [],
        }
    }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
      
</style>