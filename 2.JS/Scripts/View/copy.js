loadData();
//Load dữ liệu cho vị trí
getPosition();

//Load dữ liệu cho phòng ban
getDepartment();
let employees = [];
/**
 * Event hiện pop up 
 * Thêm mới dữ liệu
 * Dvanh 16/7/2021
 */
$(".content-header #btnAdd").click(function() {
    $("#popup").show();
    $(".wrapper").addClass("fade");

    //Hàm reset các trường
    resetPopup();

    $('#btnSave').click(function() {
        const employeeCode = $('#txtEmployeeCode').val();
        const fullName = $('#txtFullName').val();
        const dateOfBirth = $('#txtDateOfBirth').val();
        const gender = parseInt($('#txtGender').attr("Value"));
        const identityNumber = $('#txtIdentityNumber').val();
        const identityDate = $('#txtIdentityDate').val();
        const identityPlace = $('#txtIdentityPlace').val();
        const email = $('#txtEmail').val();
        const phoneNumber = $('#txtPhoneNumber').val();
        const positionId = $('#txtPositionName').attr("Value");
        const departmentId = $('#txtDepartmentName').attr("Value");
        const personalTaxCode = $('#txtPersonalTaxCode').val();
        const salary = CommonFn.formatNumber($('#txtSalary').val());
        const joinDate = $('#txtJoinDate').val();
        const workStatus = parseInt($('#txtWorkStatus').attr("Value"));

        let employee = {
            "employeeCode": employeeCode,
            "fullName": fullName,
            "gender": gender,
            "dateOfBirth": dateOfBirth,
            "phoneNumber": phoneNumber,
            "email": email,
            "identityNumber": identityNumber,
            "identityDate": identityDate,
            "identityPlace": identityPlace,
            "joinDate": joinDate,
            "departmentId": departmentId,
            "positionId": positionId,
            "workStatus": workStatus,
            "personalTaxCode": personalTaxCode,
            "salary": salary
        }
        $.ajax({
            url: "http://cukcuk.manhnv.net/v1/Employees",
            method: "POST",
            data: JSON.stringify(employee),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
        }).done(res => {
            alert("Thêm mới thành công");
            location.reload();
            $("#popup").hide();
            $(".wrapper").removeClass("fade");

        }).fail(function(res) {
            console.log(res);
        });
    })
});

/**
 * Hàm reset các trường
 * Focus vào ô mã nhân viên và thêm 1 mã nhân viên mới
 * Dvanh 21/07/2021
 */
function resetPopup() {
    $.ajax({
        url: "http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode",
        method: "GET",
    }).done(res => {
        $("#txtEmployeeCode").val(res);
        $("#txtEmployeeCode").focus();
    })
    $('#txtFullName').val("");
    $('#txtDateOfBirth').val("");
    $('#txtGender').attr("Value", "0");
    $('#txtIdentityNumber').val("");
    $('#txtIdentityDate').val("");
    $('#txtIdentityPlace').val("");
    $('#txtEmail').val("");
    $('#txtPhoneNumber').val("");
    $('#txtPositionName').attr("Value", "-1");
    $('#txtPositionName').text(`Chọn vị trí`);
    $('#txtDepartmentName').attr("Value", "-1");
    $('#txtDepartmentName').text(`Chọn phòng ban`);
    $('#txtPersonalTaxCode').val("");
    $('#txtSalary').val("");
    $('#txtJoinDate').val("");
    $('#txtWorkStatus').attr("Value", `0`);
}

/**
 * Hàm lấy dữ liệu vị trí cho dropdown
 * Dvanh 20/7/2021
 */
function getPosition() {
    $.ajax({
        url: "http://cukcuk.manhnv.net/v1/Positions",
        method: "GET",
    }).done(res => {
        res.forEach(position => {
            const positionName = position['PositionName'];
            const positionId = position['PositionId'];
            let dropdown = $(".dropdown.dd-Position");
            let item = `<div class="dropdown-item" Value = "${positionId}">
            <div class="dropdown-icon"></div>
            <div class="dropdown-text" >${positionName}</div>
            </div>`
            dropdown.append(item);
        })

    })
};

/**
 * Hàm lấy dữ liệu phòng ban cho dropdown  
 * Dvanh 20/7/2021
 */
function getDepartment() {
    $.ajax({
        url: "http://cukcuk.manhnv.net/api/Department",
        method: "GET",
    }).done(res => {
        res.forEach(department => {
            const departmentName = department['DepartmentName'];
            const departmentId = department['DepartmentId'];
            let dropdown = $(".dropdown.dd-Department");
            let item = `<div class="dropdown-item" Value = "${departmentId}">
            <div class="dropdown-icon"></div>
            <div class="dropdown-text" >${departmentName}</div>
            </div>`
            dropdown.append(item);
        })
    })
};

/**
 * Event ẩn pop up 
 * Dvanh 16/7/2021
 */
$(".head-close, .button.cancel").click(function() {
    //reset border các ô input
    $("#popup").find(".textbox-default").css('border', '1px solid #bbbbbb');
    //reset bỏ chọn các dropdown-item
    $("#popup").find(".dropdown-item").removeClass('bg-select');
    $("#popup").hide();
    $(".wrapper").removeClass("fade");
    $(".X").attr("style", "visibility: hidden;");
})

/**
 * Hàm lấy all nhân viên từ API
 * Dvanh 18/7/2021
 */
function loadData() {
    $.ajax({
        url: "http://cukcuk.manhnv.net/v1/Employees",
        method: "GET",
        success: function(response) {
            employees = response;
            updateEmployeeNumber(employees.length);
            let tbody = $("table tbody");

            employees.forEach(function(employee, index) {
                let EmployeeCode = employee.EmployeeCode,
                    FullName = employee.FullName,
                    GenderName = employee.GenderName,
                    PhoneNumber = employee.PhoneNumber,
                    Email = employee.Email,
                    PositionName = employee.PositionName,
                    DepartmentName = employee.DepartmentName,
                    Salary = CommonFn.formatMoney(employee.Salary),
                    WorkStatus = employee.WorkStatus,
                    DateOfBirth = CommonFn.formatDate(employee.DateOfBirth);

                let trHTML = `<tr Value = ${employee.EmployeeId}>
                        <td>${EmployeeCode}</td>
                        <td>${FullName}</td>
                        <td>${GenderName}</td>
                        <td >${DateOfBirth}</td>
                        <td>${PhoneNumber}</td>
                        <td>${Email}</td>
                        <td>${PositionName}</td>
                        <td>${DepartmentName}</td>
                        <td >${Salary}</td>
                        <td>${WorkStatus}</td>
                        </tr>`

                tbody.append(trHTML);
            })

        },
        error: function(errormessage) {
            console.log(errormessage.responseText);
        }

    })
}

/**
 * hàm validate dữ liệu (nhập các trường bắt buộc)
 * Dvanh 20/7/2021
 */
$('input[required]').blur(function() {
    let me = $(this)
    let value = me.val();
    if (value == '') {
        me.css('border', '1px solid red');
        me.attr('title', 'Thông tin này bắt buộc nhập');
    } else {
        me.css('border', '1px solid #bbbbbb');
        me.removeAttr('tittle')
    }
    me.focus(function() {
        me.css("border", "1px solid #01B075");
    })
})


/**
 * Hàm bấm đúp vào tr để lên form sửa 
 * Dvanh 21/7/2021
 */

$("tbody").on("dblclick", "tr", function() {
    let me = $(this),
        employeeId = me.attr("Value");

    $("#popup").show();
    for (let i = 0; i < employees.length; i++) {
        let employee = employees[i];
        if (employeeId == employee["EmployeeId"]) {
            $('#txtEmployeeCode').val(employee.EmployeeCode);
            $('#txtFullName').val(employee.FullName);
            $('#txtDateOfBirth').val(employee.DateOfBirth);
            $('#txtGender').attr("Value", `${employee.Gender}`);
            $('#txtIdentityNumber').val(employee.IdentityNumber);
            $('#txtIdentityDate').val(employee.IdentityDate);
            $('#txtIdentityPlace').val(employee.IdentityPlace);
            $('#txtEmail').val(employee.Email);
            $('#txtPhoneNumber').val(employee.PhoneNumber);
            $('#txtPositionName').attr("Value", `${employee.PositionId}`);
            $('#txtPositionName').text(`${employee.PositionName}`);
            $('#txtDepartmentName').attr("Value", `${employee.DepartmentId}`);
            $('#txtDepartmentName').text(`${employee.DepartmentName}`);
            $('#txtPersonalTaxCode').val(employee.PersonalTaxCode);
            $('#txtSalary').val(CommonFn.formatMoney(employee.Salary));
            $('#txtJoinDate').val(employee.JoinDate);
            $('#txtWorkStatus').attr("Value", `${employee.WorkStatus}`);

            $('#txtGender').parent().parent().find(`.dropdown-item[value='${employee.Gender}']`).addClass('bg-select');
            $('#txtPositionName').parent().parent().find(`.dropdown-item[value='${employee.PositionId}']`).addClass('bg-select');
            $('#txtDepartmentName').parent().parent().find(`.dropdown-item[value='${employee.DepartmentId}']`).addClass('bg-select');


        }
    }
    $('#btnSave').click(function() {
        const employeeCode = $('#txtEmployeeCode').val();
        const fullName = $('#txtFullName').val();
        const dateOfBirth = $('#txtDateOfBirth').val();
        const gender = parseInt($('#txtGender').attr("Value"));
        const identityNumber = $('#txtIdentityNumber').val();
        const identityDate = $('#txtIdentityDate').val();
        const identityPlace = $('#txtIdentityPlace').val();
        const email = $('#txtEmail').val();
        const phoneNumber = $('#txtPhoneNumber').val();
        const positionId = $('#txtPositionName').attr("Value");
        const departmentId = $('#txtDepartmentName').attr("Value");
        const personalTaxCode = $('#txtPersonalTaxCode').val();
        const salary = CommonFn.formatNumber($('#txtSalary').val());
        const joinDate = $('#txtJoinDate').val();
        const workStatus = parseInt($('#txtWorkStatus').attr("Value"));

        let employeeEdit = {
            "employeeCode": employeeCode,
            "fullName": fullName,
            "gender": gender,
            "dateOfBirth": dateOfBirth,
            "phoneNumber": phoneNumber,
            "email": email,
            "identityNumber": identityNumber,
            "identityDate": identityDate,
            "identityPlace": identityPlace,
            "joinDate": joinDate,
            "departmentId": departmentId,
            "positionId": positionId,
            "workStatus": workStatus,
            "personalTaxCode": personalTaxCode,
            "salary": salary
        }
        $.ajax({
            url: `http://cukcuk.manhnv.net/v1/Employees/${employeeId}`,
            method: "PUT",
            data: JSON.stringify(employeeEdit),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
        }).done(res => {
            alert("Sửa thành công");
            location.reload();
            $("#popup").hide();
            $(".wrapper").removeClass("fade");

        }).fail(function(res) {
            console.log(res);
        });
    })
})

/**
 * Hàm click chọn tr 
 * Dvanh 21/07/2021
 */
$("tbody").on("click", "tr", function() {
    let me = $(this);
    me.toggleClass("tr-select");
})

/**
 * Hàm bấm chuột phải vào các hàng được click chuột trái để xóa Hàng
 * Dvanh 21/07/2021
 */
$("tbody").on("mousedown", "tr.tr-select", function(e) {
    let me = $(this),
        employeeId = me.attr("Value");
    if (e.which == 3) {
        alert("Mở form Xóa nhân viên");
        $("#warning-popup").show();
        $(".wrapper").addClass("fade");

        for (let i = 0; i < employees.length; i++) {
            let employee = employees[i];
            if (employeeId == employee["EmployeeId"]) {
                $("#warning-popup .head .head-text").text(`Xóa thông tin nhân viên ${employee.FullName}`);
                $("#warning-popup .main .text").html(`Bạn có chắc muốn xóa thông tin của nhân viên <b>${employee.FullName}</b> này không`);
            }
        }

        $('#btnConfirm').click(function() {
            $.ajax({
                url: `http://cukcuk.manhnv.net/v1/Employees/${employeeId}`,
                method: "DELETE",
            }).done(res => {
                alert("Xóa thành công");
                location.reload();
                $("#warning-popup").hide();
                $(".wrapper").removeClass("fade");

            }).fail(function(res) {
                console.log(res);
            });
        })
    }

})


/**
 * Hàm đóng warning-popup
 * Dvanh 21/07/2021 
 */
$(".head-close, .button.cancel").click(function() {
    $("#warning-popup").hide();
    $(".wrapper").removeClass("fade");
})

/**
 * reload lại trang
 * Dvanh 16/7/2021
 */
$(".refresh").click(function() {
    location.reload();
})

/**
 * Hàm mở ảnh trong máy 
 * Dvanh 16/7/2021
 */
$('.image').click(function() {
    $('#myFile').trigger('click');
})


/**
 * Hàm load ảnh trong máy lên form 
 * Dvanh 16/7/2021
 */
$('#myFile').click(function(e) {
    $('#myFile').change(function(e) {
        var img = URL.createObjectURL(e.target.files[0]);
        $('.image').css("background-image", `url(${img})`);
    })
})