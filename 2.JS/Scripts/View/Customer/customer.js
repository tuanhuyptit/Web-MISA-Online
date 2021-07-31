// Trang Khách Hàng
class CustomerPage extends BaseGrid {
    constructor(gridId, formId, warningFormId) {
        super(gridId, formId, warningFormId);

    }
}
$(document).ready(function() {
    let customerPage = new CustomerPage("#gridEmployee", "#popup", "#warning-popup");
});