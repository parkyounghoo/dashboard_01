<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userAdd.aspx.cs" Inherits="ionpolis.View.userAdd" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;IE=10;IE=9;IE=8;IE=7;" />
    <link rel="SHORTCUT ICON" href="../images/logoicon.ico" type="image/x-icon" />
    <title>이온폴리스 사용자 등록</title>
    <script src="http://ajax.microsoft.com/ajax/jquery/jquery-1.4.2.js" type="text/javascript"></script>
    <link href="../Scripts/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/vendor/datatables/dataTables.bootstrap4.css" rel="stylesheet" />
    <link href="../Scripts/css/sb-admin.css" rel="stylesheet" />
    <script src="../Scripts/vendor/jquery/jquery.min.js"></script>
    <link href="../Scripts/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/jquery-1.12.4.js"></script>
    <script src="../Scripts/js/jquery-ui.js"></script>
    <link href="../Scripts/css/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/css/googlefonts.css" rel="stylesheet" />
    <link href="../Scripts/css/sb-admin-2.min.css" rel="stylesheet" />
    <script src="../Scripts/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="../Scripts/vendor/chart.js/Chart.min.js"></script>
    <style>
        .circleDetail {
            background-color: white;
            width: 10px;
            height: 10px;
            -webkit-border-radius: 10px;
            -moz-border-radius: 10px;
            margin-left: 8px;
        }
    </style>
</head>
<body id="page-top">
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="HdnRadio" />
        <nav class="navbar navbar-expand navbar-dark bg-dark static-top">
            <a class="navbar-brand mr-1" href="dashboard">
                <img src="../images/sublogo.jpg" alt="로고" style="width: 30px; height: 30px; border-radius: 50%" /></a><span style="color: white">이온폴리스</span>
            <span id="spStatus" runat="server" style="color: white;margin-left: 200px;display:none">배치 상태 : </span>
            <div id="dvStatusO" runat="server" class="circleDetail" style="background-color: orange;display:none"></div>
            <div id="dvStatusG" runat="server" class="circleDetail" style="background-color: greenyellow;display:none"></div>
            <!-- Navbar Search -->
            <div class="d-none d-md-inline-block form-inline ml-auto mr-0 mr-md-3 my-2 my-md-0">
                <a href="login" id="userDropdown" role="button" aria-haspopup="true" aria-expanded="false">
                    <i class="fas fa-fw fa-user" style="color: white"></i>
                </a>
                <span style="color: white; font-size: 14px" runat="server" id="sp_userName">관리자님 환영합니다</span>
            </div>
        </nav>
        <div id="wrapper" style="width: 100%; word-break: break-all">
            <!-- Sidebar -->
            <ul id="SidebarUl" class="sidebar navbar-nav toggled">
                <li class="nav-item active" runat="server" id="M02">
                    <a class="nav-link" href="dashboard">
                        <i class="fas fa-fw fa-tachometer-alt"></i>
                        <span>대시보드</span>
                    </a>
                </li>
                <li class="nav-item active" runat="server">
                    <a class="nav-link" href="cust">
                        <i class="fas fa-users"></i>
                        <span>고객현황</span>
                    </a>
                </li>
                <li class="nav-item active" runat="server">
                    <a class="nav-link" href="search">
                        <i class="fas fa-search"></i>
                        <span>상세검색</span>
                    </a>
                </li>
                <li class="nav-item active">
                    <a class="nav-link" href="dataAdd" runat="server">
                        <i class="fas fa-database"></i>
                        <span>데이터 등록</span>
                    </a>
                </li>
                <li class="nav-item active">
                    <a class="nav-link" href="#" runat="server">
                        <i class="fas fa-user-plus"></i>
                        <span>사용자 등록</span>
                    </a>
                </li>
            </ul>
            <div id="content-wrapper" style="background-color: rgb(43,43,43); min-width: 1660px">
                <div class="row" style="color: white; text-align: center">
                    <div class="col-md-3" style="margin-left: 15px">
                        <div class="card mb-3" style="background-color: rgb(245,247,251); color: white;">
                            <div class="card-header flex-row align-items-center justify-content-between bg-gradient-primary" style="color: white; background-color: rgb(22,87,217)">
                                <h6 class="m-0 font-weight-bold text" style="font-size: 13px;"><i class="fas fa-user-plus"></i> 사용자 등록</h6>
                            </div>
                            <div class="card-body" style="color: black; width: 100%; font-size: 13px;">
                                <table style="height: 100px" align="center">
                                    <tr>
                                        <td>
                                            <label class="form-control-label" style="margin-top: 10px">사용자 ID</label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="USER_ID" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="form-control-label" style="margin-top: 10px">사용자 이름</label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="USER_NAME" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <label for="success1" class="form-control-label">※ 초기 비밀번호는 ID와 같습니다.&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="등록" CssClass="btn btn-icon btn-fab btn-success" />
                            </div>
                        </div>
                    </div>
                    <footer class="sticky-footer" style="background-color: black">
                        <div class="container my-auto">
                            <div class="copyright text-center my-auto">
                                <span style="color: white">Copyright © 2019 Korea Productivity Center. All Rights Reserved.</span>
                            </div>
                        </div>
                    </footer>
                </div>
            </div>
        </div>
        <a class="scroll-to-top rounded" href="#page-top">
            <i class="fas fa-angle-up"></i>
        </a>
    </form>
</body>
</html>