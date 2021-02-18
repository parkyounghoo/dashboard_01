<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dataAdd.aspx.cs" Inherits="ionpolis.View.dataAdd" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge;IE=10;IE=9;IE=8;IE=7;" />
    <link rel="SHORTCUT ICON" href="../images/logoicon.ico" type="image/x-icon" />
    <title>이온폴리스 데이터 등록</title>
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
        #dropzone {
            border: 2px dotted #3292A2;
            width: 90%;
            height: 50px;
            color: #92AAB0;
            text-align: center;
            font-size: 24px;
            padding-top: 12px;
            margin-top: 10px;
        }
    </style>
    <script>
        var files;
        function handleDragOver(event) {
            event.stopPropagation();
            event.preventDefault();
            var dropZone = document.getElementById('dropzone');
            $(this).css('border', '2px solid #5272A0');
        }

        function handleDnDFileSelect(event) {
            event.stopPropagation();
            event.preventDefault();
            $(this).css('border', '2px dotted #8296C2');
            /* Read the list of all the selected files. */

            if (event.dataTransfer != undefined) {
                files = event.dataTransfer.files;
                /* Consolidate the output element. */
                var form = document.getElementById('form1');
                var data = new FormData(form);
                
                if (files.length > 1) {
                    alert('하나의 파일만 올려 주세요.');
                }
                else {
                    var div = document.getElementById("dropText");
                    div.style.visibility = 'hidden';

                    var image = document.getElementById('<%= ibdrop.ClientID %>');
                    image.style.visibility = 'visible';

                    var label = document.getElementById('<%= lbdrop.ClientID %>');
                    label.style.visibility = 'visible';
                    label.innerText = files[0].name;

                    for (var i = 0; i < files.length; i++) {
                        data.append(files[i].name, files[i]);
                    }
                    var xhr = new XMLHttpRequest();
                    //xhr.open('POST', "https://localhost:44339/View/dataAdd");
                    xhr.open('POST', "http://13.125.164.59/View/dataAdd");
                    xhr.send(data);
                }
            }
        }
    </script>
    <style>
    #loader
    {
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
         <div id="loader" style="display:none" runat="server">
        </div>
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
                    <a class="nav-link" href="#" runat="server">
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
            <div id="content-wrapper" style="background-color: rgb(43,43,43); min-width: 1660px;">
                <div class="col-md-12" style="color: white;">
                    <div class="card mb-3" style="background-color: rgb(245,247,251); color: white;">
                        <div class="card-header flex-row align-items-center justify-content-between bg-gradient-primary" style="color: white; background-color: rgb(22,87,217)">
                            <h6 class="m-0 font-weight-bold text" style="font-size: 13px;"><i class="fas fa-database"></i>데이터 등록</h6>
                        </div>
                        <div class="card-body" style="color: black; width: 100%; font-size: 13px;height:600px">
                            <div id="dropzone" style="width: 30%; height: 200px;" runat="server">
                                <div id="dropText">
                                    Drag & Drop Files Here
                                </div>
                                <asp:ImageButton runat="server" Width="50px" Height="50px" ImageUrl="~/images/excel.jpg" ID="ibdrop" Style="visibility: hidden" OnClick="ibdrop_Click" />
                                <br />
                                <asp:Label runat="server" Font-Size="13px" ID="lbdrop" Style="visibility: hidden"></asp:Label>
                            </div>
                            <br />
                            <div style="width: 100%;padding-top:10px">
                                <label style="color: red; font-weight: bold" class="form-control-label" runat="server" id="upLabel"></label>
                                <asp:GridView runat="server" ID="grdTable" AutoGenerateColumns="false" CssClass="table table-striped table-dark">
                                    <Columns>
                                        <asp:BoundField DataField="company" HeaderText="수집처" />
                                        <asp:BoundField DataField="order_number" HeaderText="주문번호" />
                                        <asp:BoundField DataField="package_number" HeaderText="묶음주문번호" />
                                        <asp:BoundField DataField="payment_date" HeaderText="결제일자" />
                                        <asp:BoundField DataField="product_name" HeaderText="쇼핑몰상품명" />
                                        <asp:BoundField DataField="product_option" HeaderText="주문옵션" />
                                        <asp:BoundField DataField="amount" HeaderText="수량" />
                                        <asp:BoundField DataField="customer" HeaderText="주문자" />
                                        <asp:BoundField DataField="tel" HeaderText="주문자연락처" />
                                        <asp:BoundField DataField="address" HeaderText="주소" />
                                        <asp:BoundField DataField="order_memo" HeaderText="배송요청사항" />
                                        <asp:BoundField DataField="price" HeaderText="주문금액" />
                                        <asp:BoundField DataField="state" HeaderText="주문상태" />
                                        <asp:BoundField DataField="shipping_fee" HeaderText="배송비금액" />
                                        <asp:BoundField DataField="state_func" HeaderText="상태별처리기능" />
                                        <asp:BoundField DataField="shipping_code" HeaderText="배송방법코드" />
                                        <asp:BoundField DataField="shipping_company" HeaderText="배송방법" />
                                        <asp:BoundField DataField="shipping_number" HeaderText="송장번호" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div style="width:100%" align="center">
                                <asp:Button runat="server" Text=">>배치실행<<" CssClass="btn btn-icon btn-fab btn-info" ID="btnAdd" Visible="false" OnClick="btnAdd_Click"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
<script>
    if (window.File && window.FileList && window.FileReader) {
        var dropZone = document.getElementById('dropzone');

        if (dropZone != null) {
            dropZone.addEventListener('dragover', handleDragOver, false);
            dropZone.addEventListener('drop', handleDnDFileSelect, false);
        }
    }
    else {
        alert('Sorry! this browser does not support HTML5 File APIs.');
    }
</script>
</html>