// Reference: https://github.com/DavidSuescunPelegay/jQuery-datatable-server-side-net-core

function BindDataTable(idWithoutSharp, url, columns, columnDefs, languageJson = "") {
    $(document).ready(function () {
        $("#" + idWithoutSharp).DataTable({
            language: {
                url: languageJson
            },
            scrollX: true,
            pagingType: "full_numbers",
            // Design Assets
            stateSave: true,
            autoWidth: true,
            // ServerSide Setups
            processing: true,
            serverSide: true,
            // Paging Setups
            paging: true,
            // Searching Setups
            searching: { regex: false },
            // Ajax Filter
            ajax: {
                url: url,
                type: "post",
                contentType: "application/json",
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: columns,
            // Column Definitions
            columnDefs: columnDefs
        });
    });
}