/*

    Initializes the dragging and dropping of attendees into sessions
    for the Assign to Sessions page.
 
*/

$(function () {

    // set the initial line height of all the session boxes
    $.each($(".session"), function (index, item) { ResizeSessionBox(item, true); });

    // set the source of where the names will be dragged from
    $("ul.people li").draggable({ helper: "clone" });

    // setup the targets
    $("ul.session").droppable({
        drop: function (event, ui) { AddPersonToSession(this, ui.draggable); }
    });

    // add all people to a specific session
    $(".add-all").click(function () {
        var session = $("#" + $(this).data("id"));

        $.each($("ul.people li"), function (index, item) {
            AddPersonToSession(session, item);
        });
    });

    // add live click handler for removing people from session
    $(".remove").live('click', function () {

        var session = $(this).parents(".session");
        var person = $(this).parents("li");

        // need to make the call to the server
        UnAssign(session, person);

    });

    $.each($("span.details"), function (index, item) { setBT(item); });
});

/*
Add a person to a session

Parameters:
that - the session ul
personLi - the li object for a person
*/
function AddPersonToSession(that, personLi) {
    //that --> the session
    var person = $(personLi).clone().removeClass("ui-draggable");

    // fix the tool tip
    var details = $(person).find("span.details");
    setBT(details[0]);

    // insert the delete image
    var deleteImg = $("<img>").attr("src", deleteImageUrl).addClass("remove");
    deleteImg.insertBefore(details);

    // prevent adding a duplicate
    if ($(that).find('li[data-id="' + person.data("id") + '"]').length <= 0) {
        
        // make the call to the server to save
        Assign(that, person);

    }
}

/*
Sets the classes so that "single" sessions always
display their attendees correctly

Parameters:
session - the session ul
*/
function AdjustNotFirstClasses(session) {

    // iterate through each object and determine if it needs the "notfirst" class
    $.each($(session).find("li"), function (index, item) {

        // get rid of the class
        $(item).removeClass("notfirst");

        var count = index % 3;

        if (count != 0) $(item).addClass("notfirst");

    });

}

/*
Resizes the session box to adjust for the addition/removal of people
Automatically adjusts for "single" session or regular

Parameters:
that - the session ul
*/
function ResizeSessionBox(that) {

    var lineHeight = 36;

    var attendees = $(that).find(".person").length;
    var boxHeight = 36;

    // adjust the "single" triple row container
    if ($(that).parents("div").hasClass("single")) {

        var rows = Math.floor(attendees / 3) + 1;

        boxHeight = lineHeight * rows;
    }
    // adjusting a standard one column session
    else {

        boxHeight = lineHeight * (attendees + 1);

    }

    $(that).height(boxHeight);
}

/*
    Makes the ajax calls to persist assignments to sessions

    Parameters:
    session - ul object for the particular session
    person - the li object for a particular person
*/
function Assign(session, person) {

    var sessionId = $(session).attr("id");
    var personId = $(person).data("id");

    $.post(assignUrl, { id: sessionId, seminarPersonId: personId, __RequestVerificationToken: antiforgeryToken }, function (result) {

        if (result) {

            person.appendTo(session);

            // for those "single" sessions that span the 3 columns
            // determine if this is not a first column item
            if ($(session).parents("div").hasClass("single")) {
                AdjustNotFirstClasses(session);
            }

            ResizeSessionBox(session);

        }
        else {
            alert("failed to save");
        }

    });
}

/*
    Makes the ajax calls to persist unassignments to sessions

    Parameters:
    session - ul object for the particular session
    person - the li object for a particular person
*/
function UnAssign(session, person) {

    var sessionId = $(session).attr("id");
    var personId = $(person).data("id");

    $.post(unassignUrl, { id: sessionId, seminarPersonId: personId, __RequestVerificationToken: antiforgeryToken }, function (result) {

        if (result) {

            $(person).remove();

            if ($(session).parents("div").hasClass("single")) {
                AdjustNotFirstClasses(session);
            }

            ResizeSessionBox(session);
        }
        else {
            alert("failed to save");
        }
    });
}

/*
    Sets the beauty tips on all the objects refering to people
*/
function setBT(obj) {
    $(obj).bt({
        ajaxPath: ["detailsUrl + '/' + $(this).data('id')"]
    });
}