// Trang nhân viên
class EmployeePage extends BaseGrid {
    constructor(gridId, formId, warningFormId) {
        super(gridId, formId, warningFormId);

        // this.getPosition();
        // this.getDepartment();
        //this.resetPopup();
    }

    /**
     * Hàm lấy dữ liệu phòng ban cho dropdown
     * Dvanh 20/7/2021
     */
    getPosition() {
        $.ajax({
            url: "http://cukcuk.manhnv.net/v1/Positions",
            method: "GET",
        }).done((res) => {
            res.forEach((position) => {
                const positionName = position["PositionName"];
                const positionId = position["PositionId"];
                let dropdown = $(".dropdown.dd-Position");
                let item = `<div class="dropdown-item" Value = "${positionId}">
                <div class="dropdown-icon"></div>
                <div class="dropdown-text" >${positionName}</div>
                </div>`;
                dropdown.append(item);
            });
        });
    }

    /**
     * Hàm lấy dữ liệu phòng ban cho dropdown
     * Dvanh 20/7/2021
     */
    getDepartment() {
        $.ajax({
            url: "http://cukcuk.manhnv.net/api/Department",
            method: "GET",
        }).done((res) => {
            res.forEach((department) => {
                const departmentName = department["DepartmentName"];
                const departmentId = department["DepartmentId"];
                let dropdown = $(".dropdown.dd-Department");
                let item = `<div class="dropdown-item" Value = "${departmentId}">
                <div class="dropdown-icon"></div>
                <div class="dropdown-text" >${departmentName}</div>
                </div>`;
                dropdown.append(item);
            });
        });
    }



}

let employeePage = new EmployeePage("#gridEmployee", "#popup", "#warning-popup");
employeePage.getDepartment();
employeePage.getPosition();
//employeePage.resetPopup();