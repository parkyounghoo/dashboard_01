<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cust.aspx.cs" Inherits="ionpolis.View.cust" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;IE=10;IE=9;IE=8;IE=7;" />
    <link rel="SHORTCUT ICON" href="../images/logoicon.ico" type="image/x-icon" />
    <title>이온폴리스 고객현황</title>
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
        $(document).on("click", ".fa-plus", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
            $(this).attr("class", "fas fa-minus");
        });
        $(document).on("click", ".fa-minus", function () {
            $(this).attr("class", "fas fa-plus");
            $(this).closest("tr").next().remove();
        });

        function loading(data) {
            document.getElementById("loader").style.display = data;
        }
    </script>
    <style>
        #loader {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            /*Change your loading image here*/
            background: url(../images/ajax-loader.gif) 50% 50% no-repeat gray;
            opacity: 0.3;
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
        <div id="loader" style="display: none">
        </div>
        <cc1:ToolkitScriptManager runat="server" ScriptMode="Release" />
        <input type="hidden" runat="server" id="HdnDateS" />
        <%--<input type="hidden" runat="server" id="HdnDateE" />--%>
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

        <div id="wrapper">
            <!-- Sidebar -->
            <ul id="SidebarUl" class="sidebar navbar-nav toggled">
                <li class="nav-item active" runat="server">
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
            <div id="content-wrapper" style="background-color: rgb(43,43,43); min-width: 1660px">
                <div class="container-fluid" style="font-size: 12px">
                    <ol class="breadcrumb" style="background-color: rgb(62,62,62);">
                        <li class="breadcrumb-item active" style="font-weight: bold; color: white; font-size: 18px; width: 100%; height: 30px">
                            <div style="float: left;">
                                <label style="margin: 0px">고객현황</label>
                            </div>
                            <div style="float: right; width: 40%;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table style="font-size: 13px">
                                                        <tr>
                                                            <td>
                                                                <label style="padding-top: 7px">지역 : </label>
                                                            </td>
                                                            <td style="padding-left: 10px">
                                                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drSido" OnSelectedIndexChanged="drSido_SelectedIndexChanged" AutoPostBack="true">
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drSigungu" OnSelectedIndexChanged="drSigungu_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Text="전체" Value=""></asp:ListItem>
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drDong">
                                                                    <asp:ListItem Text="전체" Value=""></asp:ListItem>
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <label style="padding-left: 10px; padding-top: 7px">브랜드 : </label>
                                                            </td>
                                                            <td style="padding-left: 10px">
                                                                <asp:DropDownList runat="server" CssClass="dropdown" Height="25px" ID="drBrend">
                                                                </asp:DropDownList></td>
                                                            <td style="padding-left: 10px; padding-bottom: 3px">
                                                                <input id="monthpickerS" type="text" style="width: 80px; font-size: 13px; text-align: center; height: 24px" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="drSido" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="drSigungu" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td style="padding-bottom: 6px">
                                            <asp:Button runat="server" ID="btnSerch" Text="검색" OnClick="btnSerch_Click" CssClass="btn btn-secondary btn-sm" Height="24px" Style="vertical-align: auto; font-size: 11px;" OnClientClick="loading('block');" />
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
                                </script>
                            </div>
                        </li>
                    </ol>
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
                            <h6 class="m-0 font-weight-bold text" style="font-size: 13px;"><i class="fas fa-users"></i>고객현황</h6>
                        </div>
                        <div class="card-body" style="height: 450px">
                            <div style="float: left; width: 47%; height: 100%; margin-left: 30px">
                                <canvas id="custChart" style="height: 100%; width: 100%;"></canvas>
                            </div>
                            <div style="float: right; width: 47%; height: 100%; margin-right: 30px">
                                <canvas id="recustChart" style="height: 100%; width: 100%;"></canvas>
                            </div>
                        </div>
                    </div>
                    <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                        <div class="card-header border-0 flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: rgb(22,87,217)">
                            <h6 class="m-0 font-weight-bold text" style="font-size: 13px;"><i class="fas fa-shopping-cart"></i>구매현황</h6>
                        </div>
                        <div class="card-body" style="height: 450px">
                            <div style="float: left; width: 47%; height: 100%; margin-left: 30px">
                                <h6 class="m-0 text" style="font-size: 18px; text-align: center;">신규 구매</h6>

                                <hr style="width: 110px;" />
                                <canvas id="reChart" style="height: 90%; width: 100%;"></canvas>
                            </div>
                            <div style="float: right; width: 47%; height: 100%; margin-right: 30px">
                                <h6 class="m-0 text" style="font-size: 18px; text-align: center;">재구매</h6>
                                <hr style="width: 180px; padding: 0px" />
                                <canvas id="priceChart" style="height: 90%; width: 100%;"></canvas>
                            </div>
                        </div>
                    </div>

                    <div class="card border-0 mb-3" style="background-color: rgb(245,247,251); color: black;">
                        <div class="card-header border-0 flex-row align-items-center justify-content-between gradient-color-1" style="color: white; background-color: rgb(22,87,217)">
                            <h6 class="m-0 font-weight-bold text" style="font-size: 13px;">
                                <i class="fas fa-book-open"></i>고객 리스트
                                <asp:Button runat="server" ID="btnExcel" Text="Excel 다운로드" CssClass="btn btn-warning btn-sm" OnClick="btnExcel_Click" OnClientClick="loading('block');" Height="24px" Style="vertical-align: auto; font-size: 11px; color: black; float: right; font-weight: bold" />
                            </h6>
                        </div>
                        <div class="card-body">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-centered mb-0" Width="100%" OnDataBound="grdDetail_DataBound" DataKeyNames="cust_no"
                                        AllowPaging="True" OnPageIndexChanging="grdDetail_PageIndexChanging" PagerStyle-HorizontalAlign="Center" AllowSorting="true" OnSorting="grdDetail_Sorting">
                                        <EmptyDataTemplate>검색된 데이터가 없습니다.</EmptyDataTemplate>
                                        <HeaderStyle Font-Bold="true" Font-Size="13px" ForeColor="Black" BackColor="#ECECED" HorizontalAlign="Center" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <i class="fas fa-plus" style="cursor: pointer"></i>
                                                    <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                        <asp:GridView ID="grdDetailSub" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-centered mb-0">
                                                            <HeaderStyle Font-Bold="true" Font-Size="13px" ForeColor="Black" BackColor="#ECECED" HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:BoundField HeaderText="구매일자" DataField="base_date" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                                                <asp:BoundField HeaderText="채널" DataField="company" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                                                <asp:BoundField HeaderText="브랜드" DataField="brand_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                                                <asp:BoundField HeaderText="상품" DataField="product_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                                                <asp:BoundField HeaderText="구매금액" DataField="price" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="최근구매일자" DataField="base_date" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="base_date"/>
                                            <asp:BoundField HeaderText="최근채널" DataField="company" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="company"/>
                                            <asp:BoundField HeaderText="최근브랜드" DataField="brand_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="brand_name"/>
                                            <asp:BoundField HeaderText="최근구매상품" DataField="product_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="product_name"/>
                                            <asp:BoundField HeaderText="이름" DataField="cust_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="cust_name"/>
                                            <asp:BoundField HeaderText="전화번호" DataField="tel_no" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="tel_no"/>
                                            <asp:BoundField HeaderText="주소" DataField="address" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="address"/>
                                            <asp:BoundField HeaderText="총구매금액" DataField="sumPrice" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="sumPrice"/>
                                            <asp:BoundField HeaderText="구매횟수" DataField="cnt" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" SortExpression="cnt"/>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdDetail" EventName="PageIndexChanging" />
                                </Triggers>
                            </asp:UpdatePanel>
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
        <script>
            var options = {
                responsive: true,
                scales: {
                    xAxes: [{ stacked: false }],
                    yAxes: [{ stacked: false }]
                },
                tooltips: { enabled: false },
                hover: { animationDuration: 0 },
                animation: {
                    duration: 1,
                    onComplete: function () {
                        var chartInstance = this.chart, ctx = chartInstance.ctx;
                        ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                        ctx.fillStyle = 'balck'; ctx.textAlign = 'center';
                        ctx.textBaseline = 'bottom';
                        this.data.datasets.forEach(function (dataset, i) {
                            var meta = chartInstance.controller.getDatasetMeta(i); meta.data.forEach(function (bar, index) { var data = dataset.data[index]; ctx.fillText(data, bar._model.x, bar._model.y - 5); });
                        });
                    }
                }
            }
        </script>

        <div style="visibility: collapse">
            <asp:GridView ID="grdExcel" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-centered mb-0" Width="100%" OnDataBound="grdExcel_DataBound" DataKeyNames="cust_no">
                <EmptyDataTemplate>검색된 데이터가 없습니다.</EmptyDataTemplate>
                <HeaderStyle Font-Bold="true" Font-Size="13px" ForeColor="Black" BackColor="#ECECED" HorizontalAlign="Center" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <i class="fas fa-plus" style="cursor: pointer"></i>
                            <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                <asp:GridView ID="grdExcelSub" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-centered mb-0">
                                    <HeaderStyle Font-Bold="true" Font-Size="13px" ForeColor="Black" BackColor="#ECECED" HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField HeaderText="구매일자" DataField="base_date" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                        <asp:BoundField HeaderText="채널" DataField="company" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                        <asp:BoundField HeaderText="브랜드" DataField="brand_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                        <asp:BoundField HeaderText="상품" DataField="product_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                        <asp:BoundField HeaderText="구매금액" DataField="price" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="최근구매일자" DataField="base_date" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="최근채널" DataField="company" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="최근브랜드" DataField="brand_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="최근구매상품" DataField="product_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="이름" DataField="cust_name" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="전화번호" DataField="tel_no" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="주소" DataField="address" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="총구매금액" DataField="sumPrice" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                    <asp:BoundField HeaderText="구매횟수" DataField="cnt" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="Black" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>