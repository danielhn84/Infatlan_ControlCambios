var currentUpdateEvent;
var addStartDate;
var addEndDate;
var globalAllDay;

function updateEvent(event, element) {
    //alert(event.description);

    if ($(this).data("qtip")) $(this).qtip("destroy");

    currentUpdateEvent = event;

    $('#updatedialog').dialog('open');
    $("#eventName").val(event.title);
    $("#eventDesc").val(event.description);
    $("#eventId").val(event.id);
    $("#eventStart").text("" + event.start.toLocaleString());

    if (event.end === null) {
        $("#eventEnd").text("");
    }
    else {
        $("#eventEnd").text("" + event.end.toLocaleString());
    }

    return false;
}

function updateSuccess(updateResult) {
    //alert(updateResult);
}

function deleteSuccess(deleteResult) {
    //alert(deleteResult);
}

function addSuccess(addResult) {
// if addresult is -1, means event was not added
//    alert("added key: " + addResult);

    if (addResult != -1) {
        $('#calendar').fullCalendar('renderEvent',
						{
						    title: $("#addEventName").val(),
						    start: addStartDate,
						    end: addEndDate,
						    id: addResult,
						    description: $("#addEventDesc").val(),
						    allDay: globalAllDay
						},
						false // make the event "stick"
					);


        $('#calendar').fullCalendar('unselect');
    }
}

function UpdateTimeSuccess(updateResult) {
    //alert(updateResult);
}

function selectDate(start, end, allDay) {

    $('#addDialog').dialog('open');
    $("#addEventStartDate").text("" + start.toLocaleString());
    $("#addEventEndDate").text("" + end.toLocaleString());

    addStartDate = start;
    addEndDate = end;
    globalAllDay = allDay;
    //alert(allDay);
}

function updateEventOnDropResize(event, allDay) {

    //alert("allday: " + allDay);
    var eventToUpdate = {
        id: event.id,
        start: event.start
    };

    if (event.end === null) {
        eventToUpdate.end = eventToUpdate.start;
    }
    else {
        eventToUpdate.end = event.end;
    }

    var endDate;
    if (!event.allDay) {
        endDate = new Date(eventToUpdate.end + 60 * 60000);
        endDate = endDate.toJSON();
    }
    else {
        endDate = eventToUpdate.end.toJSON();
    }

    eventToUpdate.start = eventToUpdate.start.toJSON();
    eventToUpdate.end = eventToUpdate.end.toJSON(); //endDate;
    eventToUpdate.allDay = event.allDay;

    PageMethods.UpdateEventTime(eventToUpdate, UpdateTimeSuccess);
}

function eventDropped(event, dayDelta, minuteDelta, allDay, revertFunc) {
    if ($(this).data("qtip")) $(this).qtip("destroy");

    updateEventOnDropResize(event);
}

function eventResized(event, dayDelta, minuteDelta, revertFunc) {
    if ($(this).data("qtip")) $(this).qtip("destroy");

    updateEventOnDropResize(event);
}

function checkForSpecialChars(stringToCheck) {
    var pattern = /[^A-Za-z0-9 ]/;
    return pattern.test(stringToCheck); 
}

function isAllDay(startDate, endDate) {
    var allDay;

    if (startDate.format("HH:mm:ss") == "00:00:00" && endDate.format("HH:mm:ss") == "00:00:00") {
        allDay = true;
        globalAllDay = true;
    }
    else {
        allDay = false;
        globalAllDay = false;
    }
    
    return allDay;
}

function qTipText(start, end, description, status) {
    var text;

    if (end !== null)
        text = "<strong>Inicio:</strong> " + start.format("MM/DD/YYYY hh:mm T") + "<br/><strong>Final:</strong> " + end.format("MM/DD/YYYY hh:mm T") + "<br/><br/>" + "<strong>Estado: </strong>" + (status == 1 ? "Implementación" : "Terminado") + "<br/><br/>" + description;
    else
        text = "<strong>Inicio:</strong> " + start.format("MM/DD/YYYY hh:mm T") + "<br/><strong>Final:</strong><br/><br/>" + "<strong>Estado: </strong>" + (status == 1 ? "Implementación" : "Terminado") + "<br/><br/>" + description;

    return text;
}

$(document).ready(function() {
    // update Dialog
    $('#updatedialog').dialog({
        autoOpen: false,
        width: 470
        //,buttons: {
        //    "update": function() {
        //        //alert(currentUpdateEvent.title);
        //        var eventToUpdate = {
        //            id: currentUpdateEvent.id,
        //            title: $("#eventName").val(),
        //            description: $("#eventDesc").val()
        //        };

        //        if (checkForSpecialChars(eventToUpdate.title) || checkForSpecialChars(eventToUpdate.description)) {
        //            alert("please enter characters: A to Z, a to z, 0 to 9, spaces");
        //        }
        //        else {
        //            PageMethods.UpdateEvent(eventToUpdate, updateSuccess);
        //            $(this).dialog("close");

        //            currentUpdateEvent.title = $("#eventName").val();
        //            currentUpdateEvent.description = $("#eventDesc").val();
        //            $('#calendar').fullCalendar('updateEvent', currentUpdateEvent);
        //        }

        //    },
        //    "delete": function() {

        //        if (confirm("do you really want to delete this event?")) {

        //            PageMethods.deleteEvent($("#eventId").val(), deleteSuccess);
        //            $(this).dialog("close");
        //            $('#calendar').fullCalendar('removeEvents', $("#eventId").val());
        //        }
        //    }
        //}
    });

    //add dialog
    $('#addDialog').dialog({
        autoOpen: false,
        width: 470,
        buttons: {
            "Add": function() {
                //alert("sent:" + addStartDate.format("dd-MM-yyyy hh:mm:ss tt") + "==" + addStartDate.toLocaleString());
                var eventToAdd = {
                    title: $("#addEventName").val() ,
                    description: $("#addEventDesc").val(),
                    start: addStartDate.toJSON(),
                    end: addEndDate.toJSON(),

                    allDay: isAllDay(addStartDate, addEndDate)
                };
                
                if (checkForSpecialChars(eventToAdd.title) || checkForSpecialChars(eventToAdd.description)) {
                    alert("Por favor ingrese caracteres: A to Z, a to z, 0 to 9, spaces");
                }
                else {
                    //alert("sending " + eventToAdd.title);

                    PageMethods.addEvent(eventToAdd, addSuccess);
                    $(this).dialog("close");
                }
            }
        }
    });

    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();
    var options = {
        weekday: "long", year: "numeric", month: "short",
        day: "numeric", hour: "2-digit", minute: "2-digit"
    };

    $('#calendar').fullCalendar({
        theme: true,
        header: {
            left: 'prev,next',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        customButtons: {
            customBtn: {
                text: 'Custom Button',
                click: function () {
                    alert('Test Daniel Henriquez');
                }
            }
        },
        defaultView: 'month', //Month, agendaWeek
        eventClick: updateEvent,
        selectable: false,
        selectHelper: true,
        select: selectDate,
        editable: false,
        events: "JsonResponse.ashx",
        eventDrop: eventDropped,
        eventResize: eventResized,
        eventRender: function (event, element) {
            if (event.status == 2) {
                element.css({
                    'background-color': 'darkseagreen',
                    'border-color': 'darkseagreen'
                });
            }
            else {
                element.css({
                    'background-color': 'indianred',
                    'border-color': 'indianred'
                });
            }
            //alert(event.status);
            element.qtip({
                content: {
                    text: qTipText(event.start, event.end, event.description, event.status),
                    title: '<strong >' + event.title + '</strong>'
                },
                position: {
                    my: 'bottom left',
                    at: 'top center'
                },
                style: { classes: 'qtip-shadow qtip-rounded' }
            });
        }
    });
});
