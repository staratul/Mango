var dataTable;

$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": { url: "/order/getall" },
        "columns": [
            {data: 'orderHeaderId', "width": "5%"},
            {data: 'name', "width": "20%"},
            {data: 'email', "width": "25%"},
            {data: 'phone', "width": "10%"},
            {data: 'status', "width": "10%"},
            {data: 'orderTotal', "width": "10%"}
        ]
    });
}