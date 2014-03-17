function ChatMessageViewModel() {
    var self = this;

    function ChatMessage(root, user, message, timestamp) {
        var self = this,
            updating = false;

        self.user = {
            username: ko.observable(user.username)                  
        }
        self.message = message;
        self.timestamp = timestamp;
    }

    self.addMessage = ko.observable("");
    self.items = ko.observableArray();

    self.add = function (user, message, timestamp) {
        self.items.push(new ChatMessage(self, user, message, timestamp));
    }

    self.sendCreate = function () {
        var user = {
            username: $('#displayname').val()
        };

        $.ajax({
            url: "api/chat",
            data: { 'User': user, 'Message': self.addMessage() },
            type: "POST"
        });
    };
};