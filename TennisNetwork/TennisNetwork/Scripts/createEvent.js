$(function () { // will trigger when the document is ready

    $('#start-datepicker').datepicker(
        startDateInit()
    );

    $('#end-datepicker').datepicker(
        endDateInit()
        );

    //$('#start-clockpicker').clockpicker()
    var input = $('#start-clockpicker').clockpicker({
        //donetext: 'Done',
        //init: function () {
        //    console.log("colorpicker initiated");
        //},
        //beforeShow: function () {
        //    console.log("before show");
        //},
        //afterShow: function () {
        //    console.log("after show");
        //},
        //beforeHide: function () {
        //    console.log("before hide");
        //},
        //afterHide: function () {
        //    console.log("after hide");
        //},
        //beforeHourSelect: function () {
        //    console.log("before hour selected");
        //},
        //afterHourSelect: function () {
        //    console.log("after hour selected");
        //},
        //beforeDone: function () {
        //    console.log("before done");
        //},
        afterDone: function () {
            sdt = getStartDateTime();
            edt = getEndDateTime();
            if (sdt > edt) {

                fixEndDate();
                fixEndTime();
            }
        }
    });

    $('#end-clockpicker').clockpicker({
        afterDone: function () {
            sdt = getStartDateTime();
            edt = getEndDateTime();
            if (sdt > edt) {

                fixStartDate();
                fixStartTime();
            }
        }
    });
});

function fixEndDate() {
    startDate = $('#start-datepicker').datepicker('getDate').valueOf();
    endDate = $('#end-datepicker').datepicker('getDate').valueOf();
    if (startDate > endDate) {
        year = startDate = $('#start-datepicker').datepicker('getDate').getFullYear();
        month = startDate = $('#start-datepicker').datepicker('getDate').getMonth();
        day = startDate = $('#start-datepicker').datepicker('getDate').getDate();
        $('#end-datepicker').datepicker('setDate', new Date(year, month, day)).datepicker('update');
    }
}

function fixStartDate() {
    startDate = $('#start-datepicker').datepicker('getDate').valueOf();
    endDate = $('#end-datepicker').datepicker('getDate').valueOf();
    if (startDate > endDate) {
        year = startDate = $('#end-datepicker').datepicker('getDate').getFullYear();
        month = startDate = $('#end-datepicker').datepicker('getDate').getMonth();
        day = startDate = $('#end-datepicker').datepicker('getDate').getDate();
        $('#start-datepicker').datepicker('setDate', new Date(year, month, day)).datepicker('update');
    }
}

function getStartDateTime() {
    year = startDate = $('#start-datepicker').datepicker('getDate').getFullYear();
    month = startDate = $('#start-datepicker').datepicker('getDate').getMonth();
    day = startDate = $('#start-datepicker').datepicker('getDate').getDate();

    time = $('#StartTime').val();
    timeArr = time.split(':', 2);

    return new Date(year, month, day, timeArr[0], timeArr[1], 0);
}

function getEndDateTime() {
    year = endDate = $('#end-datepicker').datepicker('getDate').getFullYear();
    month = endDate = $('#end-datepicker').datepicker('getDate').getMonth();
    day = endDate = $('#end-datepicker').datepicker('getDate').getDate();

    time = $('#EndTime').val();
    timeArr = time.split(':', 2);

    return new Date(year, month, day, timeArr[0], timeArr[1], 0);
}

function fixEndTime() {
    st = $('#StartTime').val();
    $('#EndTime').val(st);
}

function fixStartTime() {
    et = $('#EndTime').val()
    $('#StartTime').val(et)
}

var dateFormat = 'dd.mm.yyyy';

function startDateInit() {
    $('#start-datepicker').datepicker({//Initialise any date pickers
        autoclose: true,
        todayHighlight: true,
        format: dateFormat
        //format: 'dd.mm.yyyy'
    }).on('changeDate', function (e) {
        fixEndDate();
        sdt = getStartDateTime();
        edt = getEndDateTime();
        if (sdt > edt) {

            fixEndTime();
        }
    });
}

function endDateInit() {
    $('#end-datepicker').datepicker({//Initialise any date pickers
        autoclose: true,
        todayHighlight: true,
        format: dateFormat
        //format: 'dd.mm.yyyy'
    }).on('changeDate', function (e) {
        fixStartDate();
        sdt = getStartDateTime();
        edt = getEndDateTime();
        if (sdt > edt) {
            fixStartTime();
        }
    });
}