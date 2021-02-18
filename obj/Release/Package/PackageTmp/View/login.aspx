<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ionpolis.View.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;IE=10;IE=9;IE=8;IE=7;" />
    <link rel="SHORTCUT ICON" href="../images/logoicon.ico" type="image/x-icon" />
    <title>Login page</title>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css" rel="stylesheet" />
    <script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/js/bootstrap.min.js"></script>
</head>
<body style="background-color: #A6A6A6">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row" style="margin-top: 200px;">
                <a href="#">
                    <img src="../images/symbol.png" alt="로고" style="width: 30%;" /></a>
                <div class="form-horizontal" style="margin-top: 20px">
                    <fieldset>

                        <legend></legend>
                        <!-- Text input-->
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="email_login">ID:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtid" runat="server" class="form-control input-md"></asp:TextBox>
                                <span class="help-block">Enter your
								ID</span>
                            </div>
                            <div class="col-md-6"></div>
                        </div>
                        <!-- Password input-->
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="password_login">Password:</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtpw" runat="server" class="form-control input-md" TextMode="Password"></asp:TextBox>
                                <span class="help-block">Enter your password</span>
                            </div>
                            <div class="col-md-6"></div>
                        </div>
                        <!-- Button -->
                        <div class="form-group">
                            <label class="col-md-2 control-label" for="singlebutton"></label>
                            <div class="col-md-4">
                                <asp:Button ID="btnLogin" CssClass="btn btn-primary btnLogin" Text="Login" runat="server" OnClick="btnLogin_Click" />
                            </div>
                            <div class="col-md-6"></div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </form>
</body>
</html>