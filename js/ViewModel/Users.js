function UsersViewModel() {
    var self = this;

    function User(root, username, online) {
        var self = this,
            updating = false;

        
        self.username = username;
        self.online = online;
    }
        
    self.items = ko.observableArray();

    self.add = function (id, username, online) {
        self.items.push(new User(self, username, online));
    }
};