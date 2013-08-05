(function (page) {
    
    var MyCaseLoadItemView = Backbone.Marionette.ItemView.extend({
        template: "8",
        tagName: "tr"
    });

    var MyCaseLoadCompositeView = app.Core.CompositeViews.ListPageCompositeView.extend({
        template: "7",
        itemView: MyCaseLoadItemView,
        screenId: 8
    });

    var MyCaseLoadItem = Backbone.Model.extend({});
    var MyCaseLoadItems = app.Core.Collections.ListPageCollection.extend({
        model: MyCaseLoadItem
    });

    page.show(new MyCaseLoadCompositeView({ collection: new MyCaseLoadItems() }));


})(app.pageContent);