function fnScroll(header, content) {
    $(header).scrollLeft($(content).scrollLeft());
}
if (!window.localStorage) {
    window.localStorage = {
        getItem: function (sKey) {
            if (!sKey || !this.hasOwnProperty(sKey)) { return null; }
            return unescape(document.cookie.replace(new RegExp("(?:^|.*;\\s*)" + escape(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=\\s*((?:[^;](?!;))*[^;]?).*"), "$1"));
        },
        key: function (nKeyId) { return unescape(document.cookie.replace(/\s*\=(?:.(?!;))*$/, "").split(/\s*\=(?:[^;](?!;))*[^;]?;\s*/)[nKeyId]); },
        setItem: function (sKey, sValue) {
            if (!sKey) { return; }
            document.cookie = escape(sKey) + "=" + escape(sValue) + "; path=/";
            this.length = document.cookie.match(/\=/g).length;
        },
        length: 0,
        removeItem: function (sKey) {
            if (!sKey || !this.hasOwnProperty(sKey)) { return; }
            var sExpDate = new Date();
            sExpDate.setDate(sExpDate.getDate() - 1);
            document.cookie = escape(sKey) + "=; expires=" + sExpDate.toGMTString() + "; path=/";
            this.length--;
        },
        hasOwnProperty: function (sKey) { return (new RegExp("(?:^|;\\s*)" + escape(sKey).replace(/[\-\.\+\*]/g, "\\$&") + "\\s*\\=")).test(document.cookie); }
    };
    window.localStorage.length = (document.cookie.match(/\=/g) || window.localStorage).length;
}

if (!window.JackTemplateLoader) {

    function JackTemplateLoader(params) {
        if (typeof params === 'undefined') params = {};
        var TEMPLATE_DIR = params.dir || '';

        var file_cache = {};

        function get_filename(name) {
            if (name.indexOf('-') > -1) name = name.substring(0, name.indexOf('-'));
            return TEMPLATE_DIR + name;
        }

        this.get_template = function (name) {
            var template;
            var file = get_filename(name);
            var file_content;
            var result;
            if (!(file_content = file_cache[name])) {
                $.ajax({
                    url: file,
                    async: false,
                    success: function (data) {
                        file_content = data; // wrap top-level templates for selection
                        file_cache[name] = file_content;
                    }
                });
            }
            //return file_content.find('#' + name).html();
            return file_content;
        }

        this.clear_cache = function () {
            template_cache = {};
        };

    }
}

$.ajaxSetup({
    statusCode: {
        401: function (xhr, textStatus, errorThrown) {
            app.vent.trigger('App:Core', { 'caller': 'LoginScreen', "data": { "response": "UnAuthorized" } });
        },
        403: function (xhr, textStatus, errorThrown) {
            // 403 -- Access denied
            app.vent.trigger('App:Core', { 'caller': 'LoginScreen', "data": { "response": JSON.parse(xhr.responseText) } });
        }
    }
});

var app = app || new Backbone.Marionette.Application();

if (!app.Core) {
    app.addRegions({
        leftNav: "#leftNav",
        messageArea: "#messageArea",
        pageContent: "#pageContent",
        tabSection: "#tabSection",
        toolBars: "#toolBars",
        login: "#login",
        titleBar: "#titleBar"
    });

    app.module('Core', function (Core, App, Backbone, Marionette, $, _) {
        Core.Controller = Backbone.Marionette.Controller.extend({
            GoHome: function () {
                App.Core.models.page.set({
                    "TabId": 1,
                    "ScreenId": 0
                });
            },
            GotoTab: function (tabId, screenId) {
                if (App.Core.models.login.get("SessionId") == undefined || App.Core.models.login.get("SessionId") == "") {
                    app.vent.trigger('App:Core', { 'caller': 'LoginScreen' });
                } else {
                    if (tabId != App.Core.models.page.get("TabId") || screenId != App.Core.models.page.get("ScreenId")) {
                        App.Core.models.page.set({ "TabId": tabId, "ScreenId": screenId }, { silent: true });
                    }
                    this.LoadTab(tabId, screenId);
                }
            },
            initialize: function (options) {
                this.setupTriggerHandlers();
                App.Core.models.page.bind('change', this.Navigate, this);
                App.titleBar.show(new App.Core.ItemViews.TitleBar({ model: App.Core.models.login }));
            },
            Navigate: function (model) {
                var gotoURL = model.get("TabId") + "/" + model.get("ScreenId");
                App.router.navigate(gotoURL, { trigger: true });
            },
            LoadTab: function (tabId, screenId) {
                if (tabId != App.Core.models.page.get("PrevTabId")) {
                    App.Core.collections.tabs.activateTab(tabId);
                    App.Core.collections.leftNavItems.getLeftNavItemsByTabId(tabId);
                    App.leftNav.show(new App.Core.CompositeViews.LeftNavItems({ collection: App.Core.collections.leftNavItems }));
                }
                if (screenId != App.Core.models.page.get("PrevScreenId")) {
                    basket.require({ url: '/ApplicationScripts.axd?screenid=' + screenId, key: screenId });
                }
            },
            setupTriggerHandlers: function() {
                App.vent.on('App:Core:NavItem:Clicked', function (model) {
                    App.Core.models.page.set({ "TabId": model.get("TabId"), "ScreenId": model.get("ScreenId") });
                });
                App.vent.on('App:Region:Close', function (data) {
                    switch (data.caller) {
                        case 'Login':
                            App.login.close();
                            break;
                    }
                    data = null;
                });
                App.vent.on('App:Core', function (data) {
                    switch (data.caller) {
                        case 'LoginScreen':
                            App.router.navigate("Login", false);
                            App.login.show(new app.Core.ItemViews.LoginView());
                            if (data.data != null)
                                App.login.currentView.showErrorMessage(data.data.response);
                            break;
                        case 'Login':
                            $.ajax({
                                type: "POST",
                                dataType: "json",
                                data: data.data,
                                url: '/api/auth',
                                success: function (result) {
                                    App.vent.trigger('App:Core', { 'caller': 'LoginSuccess', "data": { "response": result } });
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    App.vent.trigger('App:Core', { "caller": "Loginfailed", "data": { "response": JSON.parse(xhr.responseText) } });
                                }
                            });
                            break;
                        case 'LoginSuccess':
                            App.Core.models.login.set({ "SessionId": data.data.response.SessionId, "UserName": data.data.response.UserName })
                            App.vent.trigger('App:Region:Close', { 'caller': 'Login' });
                            App.Core.models.page.trigger('change', App.Core.models.page);
                            break;
                        case 'Loginfailed':
                            App.login.currentView.showErrorMessage(data.data.response.ResponseStatus.Message);
                            break;
                    }
                    data = null;
                });
            }
        });
        Core.Router = Backbone.Marionette.AppRouter.extend({
            appRoutes: {
                ":TabId/:ScreenId": "GotoTab",
                "*defaults": "GoHome"
            }
        });
    });

    app.module('Core.Models', function(Models, App, Backbone, Marionette, $, _) {
        Models.Login = Backbone.Model.extend({
            defaults: {
                "SessionId": "",
                "UserName": "",
                "OrgName": "© BackboneApplication"
            }
        });
        Models.PagerModel = Backbone.Model.extend({});
        Models.Page = Backbone.Model.extend({
            defaults: {
                "TabId": -1,
                "PrevTabId": -1,
                "ScreenId": -1,
                "PrevScreenId": -1
            },
            initialize: function () {
            },
            set: function (attributes, options) {
                if (options == undefined || options.silent != true) {
                    attributes = _.extend(attributes, { "PrevTabId": this.get("TabId"), "PrevScreenId": this.get("ScreenId") });
                }
                Backbone.Model.prototype.set.call(this, attributes, options);
                return this;
            }
        });
        Models.TabItem = Backbone.Model.extend({
            defaults: {
                "DisplayName": "",
                "IsActive": false,
                "TabId": -1,
                "IsVisible": true,
                "ScreenId": 0
            }
        });
        Models.LeftNavItem = Backbone.Model.extend({
            defaults: {
                "DisplayName": "",
                "ScreenId": -1,
                "TabId": -1,
                "HasSubMenu": false,
                "IsSubMenu": false
            }
        });
        Models.MessageItem = Backbone.Model.extend({
            defaults: {
                "DisplayName": "",
                "Message": "",
                "IsPopup": false
            }
        });
    })

    app.module('Core.Collections', function(Collections, App, Backbone, Marionette, $, _) {
        Collections.PagerCollection = Backbone.Collection.extend({ model: App.Core.Models.PagerModel });
        Collections.Tabs = Backbone.Collection.extend({
            url: 'api/CoreData/MainTabBar/',
            model: App.Core.Models.TabItem,
            parse: function (data) {
                return data.CoreDataList;
            },
            activateTab: function (tabid) {
                if (this.length == 0) {
                    this.fetch({ url: this.url + tabid });
                } else {
                    this.each(function (tab) {
                        if (tab.get('TabId') == tabid) {
                            tab.set({ 'IsActive': true, 'IsVisible': true });
                        } else {
                            tab.set({ 'IsActive': false });
                        }
                    });
                }
            }
        });
        Collections.LeftNavItems = Backbone.Collection.extend({
            url: 'api/CoreData/LeftNav/',
            model: App.Core.Models.LeftNavItem,
            parse: function (data) {
                return data.CoreDataList;
            },
            getLeftNavItemsByTabId: function (tabid) {
                this.fetch({ url: this.url + tabid });
            }
        });
        Collections.Messages = Backbone.Collection.extend({
            model: App.Core.Models.MessageItem
        });
        Collections.ListPageCollection = Backbone.Collection.extend({
            url: 'api/CoreDataList/',
            totalRows: 0,
            pagerCollection: [],
            parse: function (data) {
                this.totalRows = data.TotalRows;
                this.pagerCollection = data.PagerData;
                return data.DataList;
            }
        });
    });

    app.module('Core.ItemViews', function(ItemViews, App, Backbone, Marionette, $, _) {
        ItemViews.PagerItemView = Backbone.Marionette.ItemView.extend({
            template: "9",
            tagName: "li",
            onRender: function () {
                if (this.model.get('IsCurrent') == true) {
                    this.$el.find("a").addClass("disabled");
                }
            }
        });
        ItemViews.LoginView = Backbone.Marionette.ItemView.extend({
            template: "1",
            ui: {
                username: "#username",
                password: "#password",
                errormessage: "#loginError"
            },
            initialize: function (options) { },
            events: {
                "click #loginSubmit": "loginToApplication",
                "click #loginCancel": "cancelLogin"
            },
            showErrorMessage: function (message) {
                this.ui.errormessage.text(message).show();
            },
            loginToApplication: function () {
                this.ui.errormessage.text("").hide();
                App.vent.trigger('App:Core', { "caller": "Login", "data": { "username": this.ui.username.val(), "password": this.ui.password.val() } });
            },
            cancelLogin: function () {
                this.ui.errormessage.text("").hide();
            }
        });
        ItemViews.LeftNavItem = Backbone.Marionette.ItemView.extend({
            template: "5",
            tagName: 'li',
            model: App.Core.Models.LeftNavItem,
            events: {
                "click": "leftNavClicked"
            },
            leftNavClicked: function (e) {
                App.vent.trigger("App:Core:NavItem:Clicked", this.model);
            }

        });
        ItemViews.TitleBar = Backbone.Marionette.ItemView.extend({
            template: "4",
            model: App.Core.Models.Login,
            tagName: 'span',
            initialize: function () {
                this.listenTo(this.model, "change", this.render);
            }
        });
        ItemViews.MainTab = Backbone.Marionette.ItemView.extend({
            template: "2",
            tagName: 'button',
            events: {
                "click": "tabClicked"
            },
            initialize: function () {
                this.listenTo(this.model, "change:IsActive", this.render);
            },
            tabClicked: function () {
                App.vent.trigger("App:Core:NavItem:Clicked", this.model);
            },
            onRender: function () {
                if (this.model.get('IsActive') == true) {
                    this.$el.removeClass().addClass('btn btn-primary');
                } else if (this.model.get('IsVisible') == false) {
                    this.$el.removeClass().addClass('nodisplay');
                } else {
                    this.$el.removeClass().addClass('btn');
                }
            }
        });
        ItemViews.PageContent = Backbone.Marionette.ItemView.extend({
            model: App.Core.Models.Page,
            serializeData: function () {
                return {};
            }
        });
    });

    app.module('Core.CompositeViews', function(CompositeViews, App, Backbone, Marionette, $, _) {
        CompositeViews.ListPageCompositeView = Backbone.Marionette.CompositeView.extend({
            tagName: "div",
            itemViewContainer: "#ListPageTableContent",
            pager: {},
            screenId: 0,
            ui: {
                "Filters": "#ListPageFilterSection",
                "PageNumber": "#ListPageNumber",
                "Sort": "#ListPageSort",
                "ListHeader": "#ListPageTable",
                "ListFooter": "#DivListPager",
                "CountOutput": "#ListPageCountOutput"
            },
            initialize: function (options) {
                this.collection = options.collection;
                this.pager = new App.Core.Collections.PagerCollection();
            },
            onShow: function () {
                var pagerContainer = new App.Core.CollectionViews.PagerCollectionView({ collection: this.pager });
                this.ui.ListFooter.html(pagerContainer.render().el);
                this.handleFilterButtonClick();
            },
            onRender: function () {
                this.handleCollectionLoaded();
            },
            onBeforeRender: function () {
                this.$("#ListPageTableContent").empty();
            },
            events: {
                "click #pageheadertoolbar": "handlePageHeaderClick",
                "click #FilterButtonApply": "handleFilterButtonClick",
                "click #ListPageTable": "handleListHeaderClick",
                "click #DivListPager": "handlePagingClick"
            },
            handlePagingClick: function (evt) {
                var pageId = evt.target.getAttribute('pageid');
                if (pageId) {
                    this.ui.PageNumber.val(pageId);
                    this.handleFilterButtonClick();
                }
            },
            handleCollectionLoaded: function () {
                this.pager.reset(this.collection.pagerCollection);
                this.ui.CountOutput.text(this.collection.totalRows);
            },
            handleListHeaderClick: function (evt) {
                var sortby = evt.target.getAttribute('sortby');
                if (sortby) {
                    var origSortBy = this.ui.Sort.val().split(' ');
                    if (sortby == origSortBy[0]) {
                        if (origSortBy.length > 1 && origSortBy[1].toLowerCase() == "desc") {
                            sortby = sortby + " asc";
                            $(evt.target).find("i").removeClass().addClass("icon-caret-up");
                        } else {
                            sortby = sortby + " desc";
                            $(evt.target).find("i").removeClass().addClass("icon-caret-down");
                        }
                    } else {
                        this.ui.ListHeader.find("i").removeClass();
                        $(evt.target).find("i").addClass("icon-caret-up");
                    }
                    this.ui.Sort.val(sortby);
                    this.handleFilterButtonClick();
                }
            },
            handlePageHeaderClick: function () { },
            handleFilterButtonClick: function () {
                var filters = _.map(this.ui.Filters.find("select,input"), function (n, i) { return n.id + '=' + n.value; }).join("^");
                this.collection.fetch({ url: this.collection.url + this.screenId + "?filters=" + encodeURI(filters) });
            }
        });
        CompositeViews.LeftNavItems = Backbone.Marionette.CompositeView.extend({
            tagName: "ul",
            className: "nav nav-tabs leftnav-stacked",
            itemView: App.Core.ItemViews.LeftNavItem,
            appendHtml: function (collectionView, itemView) {
                collectionView.$el.append(itemView.el);
            },
            initialize: function () {
                this.listenTo(this.collection, "reset", this.render, this);
            }
        });
        CompositeViews.MainTabs = Backbone.Marionette.CompositeView.extend({
            template: "3",
            class: "span12",
            itemView: App.Core.ItemViews.MainTab,
            appendHtml: function (collectionView, itemView) {
                collectionView.$("#maintabsectiontabgroup").append(itemView.el);
            }
        });
    });

    app.module('Core.CollectionViews', function(CollectionViews, App, Backbone, Marionette, $, _) {
        CollectionViews.PagerCollectionView = Backbone.Marionette.CollectionView.extend({
            itemView: App.Core.ItemViews.PagerItemView,
            tagName: "ul",
            className: "button-list",
            itemViewContainer: "ul"
        });
    });

    app.module('Core.models', function (models, App, Backbone, Marionette, $, _) {
        models.login = new App.Core.Models.Login();
        models.page = new App.Core.Models.Page();
    });

    app.module('Core.collections', function (collections, App, Backbone, Marionette, $, _) {
        collections.tabs = new App.Core.Collections.Tabs();
        collections.leftNavItems = new App.Core.Collections.LeftNavItems();
    });

    app.module('Core.layout', function (layout, App, Backbone, Marionette, $, _) {
        layout.tabs = new App.Core.CompositeViews.MainTabs({
            collection: App.Core.collections.tabs
        });
    });

    app.addInitializer(function (options) {
        app.JackTemplateLoader = new JackTemplateLoader({ dir: "/api/ApplicationScreens/", ext: '' });
        Backbone.Marionette.TemplateCache.prototype.loadTemplate = function (name) {
            if (name == undefined) {
                return "";
            } else {
                var template = app.JackTemplateLoader.get_template(name);
                return template;
            }
        };
        // compiling
        Backbone.Marionette.TemplateCache.prototype.compileTemplate = function (rawTemplate) {
            var compiled = Handlebars.compile(rawTemplate);
            return compiled;
        };
        // rendering
        Backbone.Marionette.Renderer.render = function (template, data) {
            var template = Marionette.TemplateCache.get(template);
            return template(data);
        }
        app.controller = new app.Core.Controller();
        app.router = new app.Core.Router({ "controller": app.controller });
        app.tabSection.show(app.Core.layout.tabs);
        Backbone.history.start();
    });

}