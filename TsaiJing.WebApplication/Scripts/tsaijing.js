/// <reference path="jquery-3.1.1.min.js" />
/// <reference path="kendo/2017.2.621/kendo.all.min.js" />

function defaultError(e) {
    var response = e.xhr.responseJSON;
    if (response) {
        if (response.ErrorMessage)
            alert(response.ErrorMessage);
    }
    else if (e.status && e.errorThrown) {
        alert(e.status + ':' + e.errorThrown);
    }
}

function timeEditor(container, options) {
    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
        .appendTo(container)
        .kendoTimePicker({});
}

function dateTimeEditor(container, options) {
    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
        .appendTo(container)
        .kendoDateTimePicker({});
}

function yearMonthEditor(container, options) {
    $('<input data-text-field="' + options.field + '" data-value-field="' + options.field + '" data-bind="value:' + options.field + '" data-format="' + options.format + '"/>')
        .appendTo(container)
        .kendoDatePicker({});
}