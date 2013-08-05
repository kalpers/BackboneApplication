<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyCaseLoad.ascx.cs" Inherits="BackboneApplication.ActivityPages.Office.ListPages.MyCaseLoad" %>

<div id="pageheader" class="row">
    <div id="pageheadertitle" class="one half">
        <div style="color: #cb0063; margin-top: 4px; margin-left: 4px; font-size: 1.3em;">My Caseload(<span id="ListPageCountOutput"></span>)</div>
    </div>
    <div id="pageheadertoolbar" class="one half">
        <ul style="float: right;" class="button-list">
            <li>
                <a><i class="icon-file" style="color: #abd4fb;"></i></a>
            </li>
            <li>
                <a><i class="icon-save" style="color: #638da9;"></i></a>
            </li>
            <li>
                <a><i class=" icon-remove" style="color: #c92800;"></i></a>
            </li>
        </ul>
    </div>
</div>
<div class="solid-line-red">&nbsp;</div>
<div class="row">
    <div class="row" style="margin-top: 2px; margin-bottom: 2px;">
        <div class="roundedContainer" id="ListPageFilterSection">
            <select id="ClientList" style="width: 18%; min-width: 130px;">
                <option value="1">All My Clients</option>
            </select>
            <select id="LastSeen" style="width: 18%; min-width: 130px;">
                <option value="10">Seen in X days</option>
            </select>
            <select id="ClientLastName" style="width: 18%; min-width: 130px;">
                <option value="75">Last Name</option>
            </select>
            <select id="ProgramList" style="width: 18%; min-width: 130px;">
                <option value="125">All Programs</option>
            </select>
            <select id="OtherList" style="width: 18%; min-width: 130px;">
                <option value="250">Other</option>
            </select>
            <input type="hidden" id="ListPageNumber" value="1" />
            <input type="hidden" id="ListPageSort" value="ClientId asc" />
            <button id="FilterButtonApply" class="small button">Apply Filter</button>
        </div>
    </div>
</div>
<div id="DivListPageHeader" class="row" style="margin-left: 0; width: 1040px; overflow: hidden; margin-right: 18px;">
    <table id="ListPageTable" width="1020px" style="table-layout: fixed;">
        <tr>
            <th width="120px" sortby="ClientId">Client Id<i class="icon-caret-up"></i></th>
            <th width="300px" sortby="ClientName">Name<i></i></th>
            <th width="200px" sortby="ClientPhone">Phone<i></i></th>
            <th width="100px" sortby="AxisV">Axis V<i></i></th>
            <th width="100px" sortby="LastDOS">Last DOS<i></i></th>
            <th width="127px" sortby="LastSeen">Last Seen by Me<i></i></th>
            <th width="73px" sortby="Primary">Primary<i></i></th>
        </tr>
    </table>
</div>
<div id="DivListPageContent" class="row" style="margin-left: 0; height: 400px; overflow: scroll; position: relative; width: 1058px;" onscroll="fnScroll('#DivListPageHeader','#DivListPageContent');">
    <table id="ListPageTableContent" width="1020px" style="table-layout: fixed;">
    </table>
</div>
<div id="DivListPager"></div>
