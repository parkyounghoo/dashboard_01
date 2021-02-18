<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="ionpolis.View.dashboard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;IE=10;IE=9;IE=8;IE=7;" />
    <link rel="SHORTCUT ICON" href="../images/logoicon.ico" type="image/x-icon" />
    <title>이온폴리스 대시보드</title>
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
    <script type="text/javascript">
        function SidodropDownClick(gubun) {
            document.getElementById('<%=HdnDropdownS.ClientID%>').value = gubun;
            document.getElementById('<%=btnDropdownS.ClientID%>').click();
        }
        function MapClick(sido, sigungu) {
            document.getElementById('<%=HdnSido.ClientID%>').value = sido;
            document.getElementById('<%=HdnSigungu.ClientID%>').value = sigungu;
            document.getElementById('<%=btnMap.ClientID%>').click();
        }
        function BrandClick(brand) {
            document.getElementById('<%=HdnBrand.ClientID%>').value = brand;
            document.getElementById('<%=btnBrand.ClientID%>').click();
        }
    </script>

    <style>
        .area {
            position: absolute;
            background: #fff;
            border: 1px solid #888;
            border-radius: 3px;
            font-size: 12px;
            top: -5px;
            left: 15px;
            padding: 2px;
        }

        .info {
            font-size: 12px;
            padding: 5px;
        }

            .info .title {
                font-weight: bold;
            }

        .map_wrap {
            position: relative;
            overflow: hidden;
            width: 100%;
            height: 100%;
        }

        .radius_border {
            border: 1px solid #919191;
            border-radius: 5px;
        }

        .custom_typecontrol {
            position: absolute;
            top: 10px;
            right: 10px;
            overflow: hidden;
            width: 65px;
            height: 30px;
            margin: 0;
            padding: 0;
            z-index: 1;
            font-size: 12px;
            font-family: 'Malgun Gothic', '맑은 고딕', sans-serif;
        }

            .custom_typecontrol span {
                display: block;
                width: 65px;
                height: 30px;
                float: left;
                text-align: center;
                line-height: 30px;
                cursor: pointer;
            }

            .custom_typecontrol .btn {
                background: #fff;
                background: linear-gradient(#fff, #e6e6e6);
            }

                .custom_typecontrol .btn:hover {
                    background: #f5f5f5;
                    background: linear-gradient(#f5f5f5,#e3e3e3);
                }

                .custom_typecontrol .btn:active {
                    background: #e6e6e6;
                    background: linear-gradient(#e6e6e6, #fff);
                }

            .custom_typecontrol .selected_btn {
                color: #fff;
                background: #425470;
                background: linear-gradient(#425470, #5b6d8a);
            }

                .custom_typecontrol .selected_btn:hover {
                    color: #fff;
                }

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
        <cc1:ToolkitScriptManager runat="server" ScriptMode="Release" />
        <input type="hidden" runat="server" id="HdnDateS" />
        <%--<input type="hidden" runat="server" id="HdnDateE" />--%>
        <input type="hidden" runat="server" id="HdnDropdownS" />
        <input type="hidden" runat="server" id="HdnSido" />
        <input type="hidden" runat="server" id="HdnSigungu" />
        <input type="hidden" runat="server" id="HdnDong" />
        <input type="hidden" runat="server" id="HdnBrand" />
        <asp:Button runat="server" ID="btnDropdownS" OnClick="btnDropdownS_Click" Style="display: none" />
        <asp:Button runat="server" ID="btnMap" OnClick="btnMap_Click" Style="display: none" />
        <asp:Button runat="server" ID="btnBrand" OnClick="btnBrand_Click" Style="display: none" />
        <nav class="navbar navbar-expand navbar-dark bg-dark static-top">
            <a class="navbar-brand mr-1" href="dashboard">
                <img src="../images/sublogo.jpg" alt="로고" style="width: 30px; height: 30px; border-radius: 50%" /></a><span style="color: white;">이온폴리스</span>

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

        <div id="wrapper">
            <!-- Sidebar -->
            <ul id="SidebarUl" class="sidebar navbar-nav toggled">
                <li class="nav-item active" runat="server">
                    <a class="nav-link" href="#">
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
            <div id="content-wrapper" style="background-color: rgb(43,43,43); min-width: 1660px">
                <div class="container-fluid" style="font-size: 12px">
                    <ol class="breadcrumb" style="background-color: rgb(62,62,62);">
                        <li class="breadcrumb-item active" style="font-weight: bold; color: white; font-size: 18px; width: 100%; height: 30px">
                            <div style="float: left;">
                                <label style="margin: 0px">대시보드</label>
                            </div>
                            <div style="float: right; width: 20%;">
                                <table>
                                    <tr>
                                        <td>
                                            <input id="monthpickerS" type="text" style="width: 80px; font-size: 13px; text-align: center; height: 24px" runat="server" />
                                        </td>
                                        <%--<td>~
                                        </td>--%>
                                        <%--<td>
                                            <input id="monthpickerE" type="text" style="width: 80px; font-size: 13px; text-align: center; height: 24px" runat="server" />
                                        </td>--%>

                                        <td style="padding-left: 10px">
                                            <asp:Button runat="server" ID="btnSerch" Text="검색" CssClass="btn btn-secondary btn-sm" OnClick="btnSerch_Click" Height="24px" Style="vertical-align: auto; font-size: 11px;" />
                                            <asp:Button runat="server" ID="btnCust" Text="고객현황" CssClass="btn btn-info btn-sm" OnClick="btnCust_Click" Height="24px" Style="vertical-align: auto; font-size: 11px;" />
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
                                    //$('#monthpickerE').monthpicker(options);

                                    /* MonthPicker 선택 이벤트 */
                                    $('#monthpickerS').monthpicker().bind('monthpicker-click-month', function (e, month) {
                                        document.getElementById('<%=HdnDateS.ClientID%>').value = $('#monthpickerS').val();
                                    });
                                    <%--$('#monthpickerE').monthpicker().bind('monthpicker-click-month', function (e, month) {
                                        document.getElementById('<%=HdnDateE.ClientID%>').value = $('#monthpickerE').val();
                                    });--%>
                            </script>
                            </div>
                        </li>
                    </ol>
                    <div class="row">
                        <div style="float: left; width: 29%; padding-left: 12px;">
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
                                <div class="card-header flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: gray;">
                                    <h6 class="m-0 font-weight-bold text" style="font-size: 15px;"><i class="fas fa-map-marked-alt"></i>지도</h6>
                                </div>
                                <div class="card-body" style="height: 550px">
                                    <div class="map_wrap">
                                        <div id="map" style="width: 100%; height: 100%;"></div>
                                        <div class="custom_typecontrol radius_border">
                                            <span id="btnRoadmap" class="selected_btn" onclick="setMapType()">초기화</span>
                                        </div>
                                        <script type="text/javascript" src="//dapi.kakao.com/v2/maps/sdk.js?appkey=696d022ef3c1ecf7bc3f0d05a2d716f3"></script>
                                        <script src="../Scripts/js/map/sido.js"></script>
                                        <script src="../Scripts/js/map/sigungu.js"></script>
                                        <script src="../Scripts/js/map/map.js"></script>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="float: right; width: 24%; padding-left: 24px;">
                            <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                                <div class="card-header flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: gray;">
                                    <h6 class="m-0 font-weight-bold text" style="font-size: 15px;"><i class="fas fa-chart-line"></i>재구매</h6>
                                </div>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="card-body" style="height: 550px">
                                            <canvas id="myChartR" style="height: 100%; width: 100%"></canvas>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnDropdownS" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSerch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div style="float: right; width: 46%; padding-left: 24px;">
                            <div>
                                <div class="row">
                                    <div style="float: left; width: 50%; padding-left: 12px;">
                                        <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                                            <div class="card-header flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: gray;">
                                                <h6 class="m-0 font-weight-bold text" style="font-size: 15px;"><i class="fas fa-chart-line"></i>브랜드</h6>
                                            </div>
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="card-body" style="height: 550px">
                                                        <canvas id="myChartB" style="height: 100%; width: 100%"></canvas>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnDropdownS" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSerch" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div style="float: right; width: 49%; padding-left: 24px;">
                                        <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                                            <div class="card-header flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: gray;">
                                                <h6 class="m-0 font-weight-bold text" style="font-size: 15px;"><i class="fas fa-chart-line"></i>세부지역</h6>
                                            </div>
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="card-body" style="height: 550px">
                                                        <canvas id="myChartG" style="height: 100%; width: 100%"></canvas>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnDropdownS" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSerch" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="padding-left: 12px;">
                        <div style="float: left; width: 663px;">
                            <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: white;">
                                <div class="card-header flex-row align-items-center justify-content-between gradient-color-2" style="color: white; background-color: #4E73DF;">
                                    <h6 class="m-0 font-weight-bold text" style="font-size: 15px; float: left"><i class="fas fa-chart-line"></i>일자별 지역별 판매량</h6>
                                    <%--<div class="dropdown" style="float: right;">
                                        <button class="dropdown-toggle btn-outline-primary" style="height: 20px; background-color: rgb(23,43,77); color: white" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <i class="far fa-list-alt" style="font-size: 10px;"></i>
                                            <span style="font-size: 10px;">목록</span>
                                        </button>
                                        <div runat="server" id="dropDomainList" class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                        </div>
                                    </div>--%>
                                </div>
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="card-body" style="height: 343px">
                                            <canvas id="myChart" style="height: 100%; width: 100%"></canvas>
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnDropdownS" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSerch" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div style="float: right; width: 59%; padding-left: 24px;">
                            <div>
                                <div class="row">
                                    <div style="float: left; width: 50%; padding-left: 12px;">
                                        <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                                            <div class="card-header flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: gray;">
                                                <h6 class="m-0 font-weight-bold text" style="font-size: 15px;"><i class="fas fa-chart-line"></i>브랜드 상세 ( 일자별 )</h6>
                                            </div>
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="card-body" style="height: 343px">
                                                        <canvas id="myChartBB" style="height: 100%; width: 100%"></canvas>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnDropdownS" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSerch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnBrand" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div style="float: right; width: 49%; padding-left: 24px;">
                                        <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                                            <div class="card-header flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: gray;">
                                                <h6 class="m-0 font-weight-bold text" style="font-size: 15px;"><i class="fas fa-chart-line"></i>브랜드 상세 ( 지역별 )</h6>
                                            </div>
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="card-body" style="height: 550px">
                                                        <canvas id="myChartBG" style="height: 100%; width: 100%"></canvas>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnDropdownS" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnMap" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSerch" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnBrand" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <footer class="sticky-footer" style="background-color: black">
                <div class="container my-auto">
                    <div class="copyright text-center my-auto">
                        <span style="color: white">Copyright © 2020 IONPOLIS Co.,Ltd All right reserved.</span>
                    </div>
                </div>
            </footer>
        </div>
    </form>
</body>
</html>