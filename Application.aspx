<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Application.aspx.cs" Inherits="BackboneApplication.Application" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=9" />
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title></title>
    <link href="css/groundwork.css" rel="stylesheet" />
    <!--[if IE]>
    <link href="css/groundwork-ie.css" rel="stylesheet" />
    <![endif]-->
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/StyleSheet.css" rel="stylesheet" />
    <style>
        div.loginModal
        {
            position: absolute;
            top: 0;
            left: 0;
            z-index: 9998;
            width: 100%;
            height: 100%;
            background-color: white;
        }

        div.loginPopup
        {
            position: absolute;
            top: 0;
            left: 0;
            z-index: 9999;
            width: 100%;
            height: 100%;
            margin: 0;
        }
    </style>
</head>
<body>
    <div id="main" style="display: none;">
        <div id="headerrow" class="row bluewhite top-indent bottom-indent">
            <div class="one third left-indent">
                <span class="small-lx" id="titleBar"></span>
            </div>
            <div id="headerrightsection" class="two thirds">
                <div id="headerddsection" class="nine tenths">
                    <select>
                        <option>Quick Action</option>
                    </select>
                    <select>
                        <option>Open this Client</option>
                    </select>
                    <select>
                        <option>Create Service/Note</option>
                    </select>
                </div>
                <div class="one tenth">
                    <div class="circle"><i class="icon-off" style="color: #fff;"></i></div>
                </div>
            </div>
        </div>
        <div id="tabrow" class="row">
            <div class="two thirds" id="tabSection"></div>
            <div class="one third" id="toolBars"></div>
        </div>
        <div id="tabsep" class="row" style="background-color: #2c689e; height: 7px;"></div>
        <div id="container" class="row" style="background-color: white; height: auto;">
            <div id="leftnavcontainer" class="one fifth" style="background-color: #2c689e; max-width: 250px;">
                <div id="leftnavcontentholder" class="span12" style="padding: 5px;">
                    <div onclick="$(document).trigger('scrolltop');" class="buttomup" style="height: 15px; width: 100%;"></div>
                    <div id="leftNav" class="row" style="height: 300px; background-color: #fff; margin: 0; overflow: hidden;">
                        <ul class="nav nav-tabs leftnav-stacked">
                        </ul>
                    </div>
                    <div onclick="$(document).trigger('scrollbottom');" class="bottomdown" style="width: 100%; height: 15px;"></div>
                </div>
                <div class="row" style="padding: 0 5px 5px 5px; margin: 0;">
                    <div id="messageArea" class="row" style="height: 100px; background-color: #fff; margin: 0; color: red; padding-left: 3px; overflow: auto;">
                    </div>
                </div>
            </div>
            <div id="pageContent" class="four fifths" style="vertical-align: top; padding-top: 2px;">
            </div>
        </div>
    </div>
    <div id="login"></div>
    <script src="Scripts/SystemScripts/jquery-1.9.0.js"></script>
    <script src="Scripts/SystemScripts/bootstrap.js"></script>
    <script src="Scripts/SystemScripts/rsvp.js"></script>
    <script src="Scripts/SystemScripts/basket.js"></script>
    <script src="Scripts/SystemScripts/lodash.min.js"></script>
    <script src="Scripts/SystemScripts/backbone.js"></script>
    <script src="Scripts/SystemScripts/backbone.eventbinder.js"></script>
    <script src="Scripts/SystemScripts/backbone.marionette.js"></script>
    <script src="Scripts/SystemScripts/handlebars.js"></script>
    <script src="Scripts/Application/app.js"></script>
    <script type="text/javascript">
        app.start();
        $("#main").show();
    </script>
</body>
</html>

