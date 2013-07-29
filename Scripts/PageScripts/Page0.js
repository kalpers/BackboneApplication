(function() {

    var z = Backbone.Model.extend({});
    
    var x = Backbone.Marionette.ItemView.extend({
        template: "0",
        model: z
    });

    var z1 = new z({ "test": "data" });

    app.pageContent.show(new x({model:z1}));


})();