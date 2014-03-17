

// this is point of entry 
$(function () {
    var chatViewModel = new ChatMessageViewModel(),
        userViewModel = new UsersViewModel(),
        hub = $.connection.chat;

    $('#displayname').val(prompt('Enter your name:', ''));

    ko.applyBindings(chatViewModel, $("#messageArea")[0]);
    ko.applyBindings(userViewModel, $("#userArea")[0]);

    // these are signalR callback
    hub.client.newChat = function (item) {
        chatViewModel.add(item.user, item.message, item.timestamp);
    }

    hub.client.newUserLogin = function (item) {
        userViewModel.add( item.id, item.username, true);
    }

    $.connection.hub.start()
        .done(function () {
            var displayname = $('#displayname').val();
            console.log('Connection established!');

            // login
            $.ajax({
                url: "api/user",
                data: { 'username': displayname },
                type: "POST"
            });
        });

    $.get("/api/chat", function (items) {
        $.each(items, function (idx, item) {
            chatViewModel.add(item.user, item.message, item.timestamp);
        });
    }, "json");

    $.get("/api/user", function (items) {
        $.each(items, function (idx, item) {
            userViewModel.add(item.id, item.username, true);
        });
    }, "json");
});