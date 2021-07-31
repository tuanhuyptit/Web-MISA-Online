class BaseGrid {
    constructor(gridId, formId, warningFormId) {
        let me = this;

        me.grid = $(gridId);
        me.form = $(formId);
        me.warn = $(warningFormId);

        //Lấy dữ liệu từ server
        me.getDataServer();

        //Khởi tạo các sự kiện
        me.initEvent();
    }

    /**
     * Hàm khởi tạo các sự kiện
     * Dvanh 23/07/2021
     */
    initEvent() {
        let me = this;

        //Khởi tạo sự kiện cho nút thêm mới
        me.add();

        //Khởi tạo sự kiện dblclick để sửa
        me.edit();

        //Khởi tạo sự kiện click chuội trái rồi sang chuột phải thì xóa
        me.delete();

        //Khởi tạo sự kiện refresh trang
        me.refresh();

        me.close();
    }

    /**
     * Hàm lấy tất cả dữ liệu từ Server
     * Dvanh 22/07/2021
     */
    getDataServer() {
        let me = this,
            url = me.grid.attr("Url"),
            urlFull = `http://cukcuk.manhnv.net/${url}`;

        $.ajax({
            url: urlFull,
            method: "GET",
            success: function(response) {
                me.loadData(response);
                updateEntityNumber(response.length);
            },
            error: function(error) {
                console.log(error);
            }
        });
    }

    /**
     * Hàm dùng để render dữ liệu vào bảng
     * Dvanh 22/7/2021
     */
    loadData(data) {
        let me = this,
            table = $("<table></table>"),
            thead = me.renderHeader(),
            tbody = me.renderBody(data);

        table.append(thead);
        table.append(tbody);

        me.grid.find("table").remove();
        me.grid.append(table);

    }

    /**
     * Hàm dùng để render thead
     * Dvanh 22/07/2021
     */
    renderHeader() {
        let me = this,
            thead = $("<thead></thead>"),
            tr = $("<tr></tr>");

        me.grid.find(".column").each(function() {
            let text = $(this).text(),
                th = $("<th></th>");

            th.text(text);
            tr.append(th);
        })

        thead.append(tr);

        return thead;
    }

    /**
     * Hàm dùng để render tbody
     * Dvanh 22/07/2021
     */
    renderBody(data) {

        let me = this,
            tbody = $("<tbody></tbody>"),
            itemId = me.grid.attr("ItemId");

        if (data && data.length > 0) {
            data.forEach(function(item) {
                let tr = $("<tr></tr>");

                tr.attr("itemId", `${item[itemId]}`)

                me.grid.find(".column").each(function() {
                    let td = $("<td></td>"),
                        fieldName = $(this).attr("FieldName"),
                        dataType = $(this).attr("DataType"),
                        data = item[fieldName],
                        className = me.getClassName(dataType),
                        value = me.getValue(data, dataType, $(this));

                    td.text(value);
                    td.addClass(className);
                    tr.append(td);
                });

                tbody.append(tr);
            })
        }
        return tbody
    }

    /**
     * Hàm lấy class format cho từng kiểu dữ liệu
     * Dvanh 22-07-2021
     * @param {Hàm} dataType 
     */
    getClassName(dataType) {
        let me = this,
            className = "";

        switch (dataType) {
            case "Number":
                className = "ta-r";
                break;
            case "Date":
                className = "ta-center";
                break;
            default:
                className = "ta-l";
                break;
        }

        return className;
    }

    /**
     * Hàm format từng giá trị của dữ liệu thô theo convention
     * Dvanh 23/07/2021 
     */
    getValue(data, dataType, column) {
        let me = this;

        switch (dataType) {
            case "Number":
                data = CommonFn.formatMoney(data);
                break;
            case "Date":
                if (CommonFn.isDateFormat(data))
                    data = CommonFn.formatDate(data);
                else
                    data = null;
                console.log("Date", data);
                break;
            case "Enum":
                let enumName = column.attr("EnumName");
                data = CommonFn.getValueEnum(data, enumName);
                break;
        }

        return data;
    }

    /**
     * Hàm thêm mới dữ liệu
     * Dvanh 23/07/2021
     */
    add() {
        let me = this;

        $(".content-header #btnAdd").click(function() {
            me.form.show();
            $(".wrapper").addClass("fade");

            let toolBar = $(this).attr("Toolbar")

            //Hàm reset các trường
            me.resetPopup(toolBar);

            me.SaveData(0);
        });
    }

    /**
     * Hàm tự focus vào ô mã rồi tạo 1 mã mới 
     * Dvanh 23/07/2021
     * (focus chưa hoạt động)
     */
    resetPopup(toolBar) {
        let me = this,
            item = me.form.attr("Item"),
            url = me.grid.attr("Url");
        // formMode = me.form.attr("FormMode");
        $(".first").focus();
        $.ajax({
            url: `http://cukcuk.manhnv.net/${url}/${toolBar}`,
            method: "GET",
        }).done(res => {
            me.form.find(`input[FieldName = ${item}]`).val(res);
            me.form.find(`input[FieldName = ${item}]`).focus();
        }).fail(err => {
            console.log(err);
        })

        me.form.find("[FieldName]").each(function() {
            let cell = $(this),
                dataType = cell.attr("DataType"),
                fieldName = cell.attr("FieldName");

            if (fieldName == item) {
                return;
            } else if (dataType == "Enum") {
                cell.attr("Value", "-1");
                cell.text("Chưa chọn");
            } else {
                cell.val("");
            }
        })
    }


    /**
     * Lấy dữ liệu từng ô trong form dựa vào DataType
     * Dvanh 23/07/2021
     */
    getValueForm(cell, dataType) {
        let me = this,
            value = "";

        switch (dataType) {
            case "Date":
                value = new Date(cell.val());
                break;
            case "Number":
                value = CommonFn.formatNumber(cell.val())
                break;
            case "Enum":
                value = cell.attr("Value");
                break;
            default:
                value = cell.val();
                break;
        }

        return value;
    }

    /**
     * Hàm sửa dữ liệu
     * Dvanh 23/07/2021
     */
    edit() {
        let me = this,
            url = me.grid.attr("Url");

        me.grid.on("dblclick", "table tbody tr", function() {
            let itemId = $(this).attr("itemId");
            me.form.show();
            $(".wrapper").addClass("fade");
            $.ajax({
                url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                method: "GET",
            }).done(res => {
                try {
                    me.form.find("[FieldName]").each(function() {
                        let cell = $(this),
                            fieldName = cell.attr("FieldName"),
                            dataType = cell.attr("DataType"),
                            value = res[fieldName];

                        me.setValueForm(value, dataType, cell)
                    })
                } catch (err) {
                    console.log(err);
                }

            }).fail(function(err) {
                console.log(err);
            });

            me.SaveData(1, itemId);

        })
    }

    /**
     * Hàm xử lí sự kiện bấm nút Lưu
     * Dvanh 23/07/2021
     */
    SaveData(formMode, itemId) {
        let me = this,
            check = 0,
            url = me.grid.attr("Url");
        $('#btnSave').click(function() {
            let entity = {},
                urlFull = "",
                method = "",
                message = "";
            // validate data
            me.form.find("[FieldName]").each(function() {
                let cell = $(this),
                    dataType = cell.attr("DataType"),
                    fieldName = cell.attr("FieldName"),
                    value = me.getValueForm(cell, dataType);
                if (dataType == "Text") {
                    if (value == "") {

                        $(this).focus();
                        $(this).css("title", "Trường này bắt buộc nhập!");
                        $(this).css("border", "1px solid #red");
                        check = 1
                        return false;
                    }
                } else if (dataType == "Phone" || dataType == "IdentityNumber") {
                    if (value == "" || CommonFn.isPhone(value) != true) {
                        $(this).focus();
                        $(this).css("title", "Trường này bắt buộc nhập!");
                        $(this).css("border", "1px solid #red");
                        console.log("phone sai")
                        check = 1;
                        return false;
                    }
                } else if (dataType == "Email") {
                    if (value == "" || CommonFn.isEmailAddress(value) != true) {
                        $(this).focus();
                        $(this).css("title", "Trường này bắt buộc nhập!");
                        $(this).css("border", "1px solid #red");
                        console.log("email sai")
                        check = 1;
                        return false;
                    }
                }
                entity[fieldName] = value;
            })

            console.log(check)
            console.log(entity)
            if (check == 1) {

                console.log("lỗi")
                return;
            }






            if (formMode == 0) {
                urlFull = `http://cukcuk.manhnv.net/${url}`;
                method = "POST";
                message = "Thêm mới thành công"
            } else {
                urlFull = `http://cukcuk.manhnv.net/${url}/${itemId}`;
                method = "PUT";
                message = "Sửa thành công";
            }
            $.ajax({
                url: urlFull,
                method: method,
                data: JSON.stringify(entity),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
            }).done(res => {
                alert(message);
                location.reload();
                //$("#popup").hide();
                //$(".wrapper").removeClass("fade");

            }).fail(function(res) {
                console.log(res);
            });
        })
    }

    /**
     * Hàm lấy dữ liệu thô để gán vào form
     * Dvanh 23/07/2021
     */
    setValueForm(value, dataType, cell) {
        let me = this;

        switch (dataType) {
            case "Date":
                cell.val(CommonFn.convertDate(value));
                break;
            case "Number":
                cell.val(CommonFn.formatMoney(value));
                break;
            case "Enum":
                cell.attr("Value", `${value}`);
                cell.text(cell.parent().parent().find(`.dropdown-item[value='${value}'] .dropdown-text`).text());
                //Các dropdown được tích theo tên tương ứng
                me.dropdownSelected(cell, value);
                break;
            default:
                cell.val(value);
                break;
        }
    }

    /**
     * Hàm check dropdown-item nào có value giống value của div.inp
     * Dvanh 23/07/2021 
     */
    dropdownSelected(cell, value) {
        let me = this;

        cell.parent().parent().find(`.dropdown-item[value='${value}']`).addClass('bg-select');
    }

    /**
     * Hàm xóa dữ liệu
     * Dvanh 23/07/2021
     */
    delete() {
        let me = this,
            url = me.grid.attr("Url");

        me.trToggleSelected();

        me.grid.on("mousedown", "table tbody tr.tr-select", function(e) {
            let allTrSelect = $("table tbody tr.tr-select")

            console.log(allTrSelect.length);
            if (e.which == 3) {
                alert("Mở form Xóa nhân viên");
                me.warn.show();
                $(".wrapper").addClass("fade");
                let itemId = "";
                if (allTrSelect.length == 1) {
                    allTrSelect.each(function() {
                        itemId = $(this).attr("itemId");
                    })
                    $.ajax({
                        url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                        method: "GET",
                    }).done(res => {
                        me.warn.find(".head .head-text").text(`Xóa thông tin nhân viên ${res.FullName}`);
                        me.warn.find(".main .text").html(`Bạn có chắc muốn xóa thông tin của nhân viên <b>${res.FullName}</b> này không`);
                    }).fail(function(err) {
                        console.log(err);
                    });

                    $('#btnConfirm').click(function() {
                        $.ajax({
                            url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                            method: "DELETE",
                        }).done(res => {
                            alert("Xóa thành công");
                            me.warn.hide();
                            $(".wrapper").removeClass("fade");
                            location.reload();
                        }).fail(function(res) {
                            console.log(res);
                        });
                    })
                }

                if (allTrSelect.length > 1) {
                    me.warn.find(".head .head-text").text(`Xóa thông tin của ${allTrSelect.length} nhân viên`);
                    me.warn.find(".main .text").html(`Bạn có chắc muốn xóa thông tin của  <b>${allTrSelect.length}</b> nhân viên này không`);

                    $('#btnConfirm').click(function() {
                        let completed = true;

                        allTrSelect.each(function() {
                            itemId = $(this).attr("itemId");
                            $.ajax({
                                url: `http://cukcuk.manhnv.net/${url}/${itemId}`,
                                method: "DELETE",
                            }).done(res => {
                                me.warn.hide();
                                $(".wrapper").removeClass("fade");
                            }).fail(function(res) {
                                completed = false;
                                console.log("Xóa thất bại");
                                console.log(res);
                            });
                        })

                        if (completed) {
                            alert("Xóa thành công");
                            location.reload();
                        }
                    })
                }

            }

        })
    }

    /**
     * Hàm click chọn tr 
     * Dvanh 21/07/2021
     */
    trToggleSelected() {
        let me = this;
        me.grid.on("click", "table tbody tr", function() {
            $(this).toggleClass("tr-select");
        })
    }


    /**
     * Hàm refresh trang
     * Dvanh 23/07/2021
     */
    refresh() {
        let me = this;
        $(".refresh").click(function() {
            location.reload();
        })
    }

    /**
     * Hàm đóng popup,warning-pop
     * Dvanh 23/07/2021
     */
    close() {
        let me = this;

        $(".head-close, .button.cancel").click(function() {
            //reset border các ô input
            me.form.find(".textbox-default").css('border', '1px solid #bbbbbb');
            //reset bỏ chọn các dropdown-item
            me.form.find(".dropdown-item").removeClass('bg-select');
            me.warn.hide();
            me.form.hide();
            $(".wrapper").removeClass("fade");
            $(".X").attr("style", "visibility: hidden;");
        })
    }

}