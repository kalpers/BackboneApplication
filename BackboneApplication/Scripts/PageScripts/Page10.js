(function (page) {

    var x = Backbone.Marionette.ItemView.extend({
        template: "10"
    });

    page.show(new x());

})(app.pageContent);