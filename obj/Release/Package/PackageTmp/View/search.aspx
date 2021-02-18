<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="ionpolis.View.search" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;IE=10;IE=9;IE=8;IE=7;" />
    <link rel="SHORTCUT ICON" href="../images/logoicon.ico" type="image/x-icon" />
    <title>이온폴리스 상세검색</title>
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
<body>
    <form id="form1" runat="server">
        <input type="hidden" runat="server" id="HdnDateS" />
        <input type="hidden" runat="server" id="HdnDateE" />
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
                    <a class="nav-link" href="userAdd" runat="server">
                        <i class="fas fa-user-plus"></i>
                        <span>사용자 등록</span>
                    </a>
                </li>
            </ul>
            <div id="content-wrapper" style="background-color: rgb(43,43,43);">
                <div class="container-fluid" style="font-size: 12px">
                    <style>
                        .header-line {
                            height: 5px;
                            width: 100%;
                            content: '';
                            display: block;
                        }

                        .gradient-color-1 {
                            background: linear-gradient(to top, #1e3c72 0%, #1e3c72 1%, #2a5298 100%);
                        }

                        .gradient-color-2 {
                            background: linear-gradient(to top, #1e3c72 0%, #1e3c72 1%, #2a5298 100%);
                        }

                        .gradient-color-3 {
                            background: linear-gradient(to top, #09203f 0%, #537895 100%);
                        }

                        .gradient-color-4 {
                            background: linear-gradient(to top, #09203f 0%, #537895 100%);
                        }
                    </style>
                    <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                        <div class="card-header border-0 flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: rgb(22,87,217)">
                            <h6 class="m-0 font-weight-bold text" style="font-size: 13px;"><i class="fas fa-search"></i> 상세검색</h6>
                        </div>
                        <div class="card-body" style="height: 100%; width: 1570px; margin-left: 40px">
                            <div style="float: left">
                                <label>지역 : </label>
                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drSido" OnSelectedIndexChanged="drSido_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drSigungu" OnSelectedIndexChanged="drSigungu_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="전체" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drDong">
                                    <asp:ListItem Text="전체" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <label style="padding-left: 10px">브랜드 : </label>
                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drBrend">
                                </asp:DropDownList>
                                <label style="padding-left: 10px">상품 : </label>
                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drProduct">
                                </asp:DropDownList>
                                <label style="padding-left: 10px">채널 : </label>
                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drChannel">
                                </asp:DropDownList>
                            </div>
                            <div style="float: left; padding-left: 10px;margin-bottom:30px">
                                <table>
                                    <tr>
                                        <td>
                                            <input id="monthpickerS" type="text" style="width: 80px; font-size: 13px; text-align: center; height: 24px" runat="server" />
                                        </td>
                                        <td>~
                                        </td>
                                        <td>
                                            <input id="monthpickerE" type="text" style="width: 80px; font-size: 13px; text-align: center; height: 24px" runat="server" />
                                        </td>
                                        <td style="padding-left: 10px">
                                            <asp:Button runat="server" ID="btnSerch" Text="검색" OnClick="btnSerch_Click" CssClass="btn btn-secondary btn-sm" Height="24px" Style="vertical-align: auto; font-size: 11px;" />
                                        </td>
                                    </tr>
                                </table>
                                <script type="text/javascript" src="../Scripts/js/jquery-1.11.1.min.js"></script>
                                <script type="text/javascript" src="../Scripts/js/jquery-ui.min.js"></script>
                                <script type="text/javascript" src="../Scripts/js/jquery.mtz.monthpicker.js"></script>
                                <script>
                                    /* MonthPicker 옵션 */
                                    options = {
                                        pattern: 'yyyy-mm', // Default is 'mm/yyyy' and separator char is not mandatory
                                        selectedYear: new Date().getFullYear(),
                                        startYear: new Date().getFullYear() - 5,
                                        finalYear: new Date().getFullYear() + 5,
                                        monthNames: ['1월', '2월', '3월', '4월', '5월', '6월', '7월', '8월', '9월', '10월', '11월', '12월']
                                    };

                                    /* MonthPicker Set */
                                    $('#monthpickerS').monthpicker(options);
                                    $('#monthpickerE').monthpicker(options);

                                    /* MonthPicker 선택 이벤트 */
                                    $('#monthpickerS').monthpicker().bind('monthpicker-click-month', function (e, month) {
                                        document.getElementById('<%=HdnDateS.ClientID%>').value = $('#monthpickerS').val();
                                    });
                                    $('#monthpickerE').monthpicker().bind('monthpicker-click-month', function (e, month) {
                                        document.getElementById('<%=HdnDateE.ClientID%>').value = $('#monthpickerE').val();
                                    });
                                </script>
                            </div>
                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-centered mb-0" Width="100%" 
                                AllowPaging="True" onpageindexchanging="grdDetail_PageIndexChanging" PagerStyle-HorizontalAlign="Center" AllowSorting="true" OnSorting="grdDetail_Sorting">
                                <EmptyDataTemplate>검색된 데이터가 없습니다.</EmptyDataTemplate>
                                <HeaderStyle Font-Bold="true" Font-Size="13px" ForeColor="Black" BackColor="#ECECED" HorizontalAlign="Center"/>
                                <Columns>
                                    <asp:BoundField HeaderText="일자" DataField="base_date" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="base_date"/>
                                    <asp:BoundField HeaderText="채널" DataField="company" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="company"/>
                                    <asp:BoundField HeaderText="브랜드" DataField="brand" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="brand"/>
                                    <asp:BoundField HeaderText="상품" DataField="product" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="product"/>
                                    <asp:BoundField HeaderText="이름" DataField="cust_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="cust_name"/>
                                    <asp:BoundField HeaderText="전화번호" DataField="tel_no" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="tel_no"/>
                                    <asp:BoundField HeaderText="주소" DataField="address" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="address"/>
                                </Columns>
                            </asp:GridView> 
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <a class="scroll-to-top rounded" href="#page-top">
            <i class="fas fa-angle-up"></i>
        </a>
    </form>
</body>
</html>