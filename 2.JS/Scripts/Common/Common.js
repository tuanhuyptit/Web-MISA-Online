/**
 * Các hàm dùng chung(common function) toàn chương trình
 * Dvanh 18/7/2021
 */
var CommonFn = CommonFn || {}; // ko dung let
/**
 * hàm check format email 
 *Dvanh 21/7/2021 
 */


CommonFn.isEmailAddress = e => {
    var pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    console.log(String(e).search(pattern) != -1);
    return String(e).search(pattern) != -1;

}

/**
 *hàm check format số điện thoại 
 *Dvanh 21/7/2021
 */
CommonFn.isPhone = p => {
        pattern = /^[+]*[(]{0,1}[0-9]{1,3}[)]{0,1}[-\s\./0-9]*$/g;
        console.log(String(p).search(pattern) != -1);
        return String(p).search(pattern) != -1;

    }
    /**
     * Hàm format số tiền
     * Dvanh 18/7/2021
     */
CommonFn.formatMoney = money => {
    if (money && !isNaN(money)) {
        return money.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
    } else {
        return money;
    }
}

/**
 * Hàm format số tiền về số bth
 * Dvanh 18/7/2021
 */
CommonFn.formatNumber = money => {
    let salary = money.replaceAll(".", "");
    return salary;
}

/**
 * Format ngày tháng
 * Dvanh 18/7/2021
 */
CommonFn.formatDate = dateSrc => {
    let date = new Date(dateSrc),
        year = date.getFullYear().toString(),
        month = (date.getMonth() + 1).toString().padStart(2, '0'),
        day = date.getDate().toString().padStart(2, '0');

    return `${day}/${month}/${year}`;
}

/**
 * Kiểm tra xem có phải dạng date không
 * Dvanh 18/7/2021
 */
CommonFn.isDateFormat = (date) => {
    let regex = new RegExp("([0-9]{4}[-](0[1-9]|1[0-2])[-]([0-2]{1}[0-9]{1}|3[0-1]{1})|([0-2]{1}[0-9]{1}|3[0-1]{1})[-](0[1-9]|1[0-2])[-][0-9]{4})");

    return regex.test(date);
}

/**
 * Chuyển đổi ngày tháng
 * Dvanh 18/7/2021
 */
CommonFn.convertDate = dateSrc => {
    let dateString = "";
    if (dateSrc) {
        let date = new Date(dateSrc),
            year = date.getFullYear().toString(),
            month = (date.getMonth() + 1).toString().padStart(2, '0'),
            day = date.getDate().toString().padStart(2, '0');

        dateString = `${year}-${month}-${day}`;
    } else {
        dateString = "123";
    }
    return dateString;
}

/**
 * Lấy giá trị của một enum
 * Dvanh 18/7/2021
 */
CommonFn.getValueEnum = (data, enumName) => {
    let enumeration = Enumeration[enumName],
        resource = Resource[enumName];

    for (var key in enumeration) {
        if (enumeration[key] == data)
            data = resource[key];
    }

    return data
}

/**
 * Hàm ajax gọi lên server lấy dữ liệu
 * Dvanh 18/7/2021
 */
CommonFn.Ajax = (url, method, data, fnCallBack, async = true) => {
    $.ajax({
        url: url,
        method: method,
        async: async,
        data: JSON.stringify(data),
        headers: {
            "Content-Type": "application/json"
        },
        crossDomain: true,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function(response) {
            fnCallBack(response);
        },
        error: function(errormessage) {
            console.log(errormessage.responseText);
        }
    })
}