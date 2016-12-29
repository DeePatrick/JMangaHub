$(function messager() {
    $(document).ready(function () {
        var chat = $.connection.chatHub;
        chat.client.broadcastMessage = function (name, message) {
            //two lines that HTML encodes the content before displaying it are optional
            //and show a simple way to prevent script injection
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(message).html();
            //add message to page
            $('#discussion').append('<li><strong>' + encodedName + ' </strong>: &nbsp;&nbsp;' + encodedMsg + '</li>');
        };
        $('#displayname').val(prompt('Enter your prefered name: ', ""));
        $('#message').focus();
        //shows how to open a connection with hub. code starts the connection and then
        //passes it a function to handle the click event on the send button
        $.connection.hub.start().done(function () {
            $('#sendmessage').click(function () {
                //call the send method on the hub
                chat.server.send($('#displayname').val(), $('#message').val());
                //clear the textbox and focus for the next comment
                $('#message').val('').focus();
            })
            .fail(function () {
                alert("Something failed")
            });
        });
    });
});