<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="BackboneApplication.ActivityPages.Login" %>

<div class="loginModal">&nbsp;</div>
<div class="loginPopup">
    <div style="text-align: center; margin: auto; height: 250px; width: 250px;">
        <input type='hidden' name='Continue' id="Continue" />
        <label>User Name:</label><input type='text' name='username' id='username' />
        <label>Password:</label><input type='password' name='password' id='password' />
        <span style="color: red; padding: 3px; display: none;" id="loginError"></span>
        <input type='submit' id='loginSubmit' value='Sign In' class="small button" style="margin-top: 4px;" />
        <input type='button' id='loginCancel' value='Cancel' class="small button" style="margin-top: 4px;" />
        <p class="small">Copyright © <%= DateTime.Now.Year %> BackboneApplication All Rights Reserved.</p>
    </div>
</div>
