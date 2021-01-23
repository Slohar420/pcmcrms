<%@ Page Title="Transaction Details | PCMC" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="TxnReport.aspx.cs" Inherits="Dashboard_TxnDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript">

        timer = 120000;
        interval = setInterval(reload, timer);

        document.onkeypress = function () {
            timer = 120000;
            clearInterval(interval);
            interval = setInterval(reload, timer);
        };


        function reload() {
            window.location.reload();

        }


        var filter = 0;

        function Search_Gridview(strKey) {
            filter = 0;
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("<%=GV_Kiosk_Health.ClientID %>");
            var rowData;

            var totalRow = tblData.rows.length;

            for (var i = 1; i < tblData.rows.length; i++) {

                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';

                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0) {
                        styleDisplay = '';
                        filter++;
                        tblData.rows[i].cells[0].innerHTML = filter;
                    }
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;

                //for (var i = 1; i < tblData.rows.length; i++) {
                //    if (tblData.rows[i].style.display!='none')
                //        tblData.rows[i].cells[0].innerHTML = i;
                //}
            }
         <%--   var rowsCount = <%=GV_Kiosk_Health.Rows.Count %>;--%>
            // document.getElementById("Show").innerText = filter + ' Out of ';
        }
    </script>
    <script type="text/javascript">
        $(function () {

            $('[id*=btnExport]').on('click', function () {
                ExportToExcel('GV_Kiosk_Health');
                // location.reload();
            });
        });
        function ExportToExcel(Id) {

            var today = new Date();
            var colcount = document.getElementById('<%= GV_Kiosk_Health.ClientID %>').rows[0].cells.length;
            dateTime = FormatedTime(today);  //Return datetime formated 24 hour

            var tab_text = "";
            var userRole = $('#hdnRole').val();

            if (userRole == 'watertax')
                tab_text += "<table border='2px'><tr><td colspan='" + colcount + "' align='center' style='background-color:#367fa9;color:white' ><h2> Kiosk WaterTax Transaction Report Generated On -(" + dateTime + ")</h2><br/></td><tr>";
            else if (userRole == 'propertytax')
                tab_text += "<table border='2px'><tr><td colspan='" + colcount + "' align='center' style='background-color:#367fa9;color:white' ><h2> Kiosk ProperttTax Transaction Report Generated On -(" + dateTime + ")</h2><br/></td><tr>";
            else
                tab_text += "<table border='2px'><tr><td colspan='" + colcount + "' align='center' style='background-color:#367fa9;color:white' ><h2> Kiosk Transaction Report Generated On -(" + dateTime + ")</h2><br/></td><tr>";

            var j = 0;
            tab = document.getElementById(Id);
            var headerRow = $('[id*=GV_Kiosk_Health] tr:first');

            tab_text += headerRow.html() + '</tr><tr>';
            var rows = $('[id*=GV_Kiosk_Health] tr:not(:has(th))');

            var totalRows = rows > 100 ? rows.length - 2 : rows.length;
            var iExcelIndex = 0;
            for (j = 0; j < totalRows; j++) {
                if ($(rows[j]).css('display') != 'none') {
                    iExcelIndex++;
                    for (var i = 0; i < rows[j].cells.length; i++) {
                        if (i == 0) {
                            tab_text = tab_text + "<td align='center'>" + iExcelIndex + "</td>";
                        } else {
                            tab_text = tab_text + "<td align='center'>" + rows[j].cells[i].innerHTML + "</td>";
                        }
                    }
                    tab_text += "</tr>";
                }
            }
            tab_text = tab_text + "</table>";
            // tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, ""); //remove if u want links in your table
            // tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
            // tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
            {
                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(tab_text);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", true, Id + ".xlsx");
            }
            else {                 //other browser not tested on IE 11

                var file = new Blob([tab_text], { type: "application/vnd.ms-excel" });
                var uri = URL.createObjectURL(file);
                let a = $("<a />", {
                    href: uri,
                    download: "PCMC_TxnDetails" + dateTime + ".xlsx"
                }).appendTo("body").get(0).click();
                e.preventDefault();//other browser not tested on IE 11
                //  sa = window.open(data_type + encodeURIComponent(tab_text));
            }
            // return (sa);
        }
    </script>


    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Report Panel</h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Transaction Report</li>
        </ol>
    </section>



    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">

            <div id="div1" runat="server" class="col-lg-2" visible="false">
                <!-- small box -->
                <div class="small-box bg-green">
                    <div class="inner">
                        <h4>
                            <asp:Label Visible="false" ID="l1" runat="server" Text="Label">D1 (Cash Acceptor)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l2" runat="server" Text="Label">D2 (Receipt Printer)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l3" runat="server" Text="Label">D3 (BarCode Reader)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l4" runat="server" Text="Label">D4 (Doc Scanner)</asp:Label>
                        </h4>

                    </div>

                </div>
            </div>
            <!-- ./col -->
            <div id="div2" runat="server" class="col-lg-2" visible="false">
                <!-- small box -->
                <div class="small-box bg-yellow">
                    <div class="inner">
                        <h4>
                            <asp:Label Visible="false" ID="l5" runat="server" Text="Label">D5 (Camera)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l6" runat="server" Text="Label">D6 (VC Camera)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l7" runat="server" Text="Label">D7 (Card Reader)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l8" runat="server" Text="Label">D8 (Laser Printer)</asp:Label>
                        </h4>
                    </div>
                </div>
            </div>

            <!-- ./col -->
            <!-- ./col -->
            <div id="div3" runat="server" class="col-lg-2" visible="false">
                <!-- small box -->
                <div class="small-box bg-aqua">
                    <div class="inner">
                        <h4>
                            <asp:Label Visible="false" ID="l9" runat="server" Text="Label">D9 (Finger Scanner)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l10" runat="server" Text="Label">D10 (KeyPadWithMouse)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l11" runat="server" Text="Label">D11 (Signage TV)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l12" runat="server" Text="Label">D12 (TouchScreen)</asp:Label>
                        </h4>
                    </div>
                </div>
            </div>
            <!-- ./col -->
            <%-- <div class="col-lg-2">
                <!-- small box -->
                <div class="small-box bg-yellow">
                    <div class="inner">
                        <h4> <asp:Label ID="Label3" runat="server" Text="Label">D4</asp:Label> </h4>
                        <p>Doc Scanner</p>
                    </div> 
                </div>
            </div>--%>
        </div>
        <!-- /.row -->
        <div class="row">
            <section class="col-lg-12 ">
                <%--connectedSortable--%>
                <!--data table box-->
                <div class="box box-info">
                    <div class="box-header with-border ">
                        <div class="row">
                            <div class="col-md-12">
                                <h3 class="box-title">Kiosk Transaction Details</h3>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-3">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label Text="From Date" runat="server" CssClass="label label-info text-uppercase" />
                                            </div>
                                            <div class="col-md-12">
                                                <div class="btn-group">
                                                    <input placeholder="From Date" readonly style="background-color: white" type="text" runat="server" class="date form-control" id="datefrom">
                                                    <span id="searchclear" class="glyphicon glyphicon-remove-circle txnsearchclear"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label Text="To Date" runat="server" autocomplete="off" CssClass="label label-info text-uppercase" />
                                            </div>
                                            <div class="col-md-12">
                                                <div class="btn-group">
                                                    <input placeholder="To Date" readonly autocomplete="off" type="text" style="background-color: white" runat="server" class="date form-control" id="dateto">
                                                    <span id="searchclear1" class="glyphicon glyphicon-remove-circle txnsearchclear"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3" style="display: none">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label Text="Search By Kiosk ID" runat="server" CssClass="label label-info text-uppercase" />
                                            </div>
                                            <div class="col-md-12">
                                                <asp:DropDownList CssClass="form-control" ID="ddlKioskIDs" AutoPostBack="true" OnSelectedIndexChanged="ddlKioskIDs_SelectedIndexChanged" runat="server" Width="100%">
                                                    <asp:ListItem Text="Select Kiosk ID" Selected="True" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label Text="Services" runat="server" CssClass="label label-info text-uppercase" />
                                            </div>
                                            <div class="col-md-12">
                                                <%--<input placeholder="From Date" readonly type="text" runat="server" class="date form-control" id="datefrom">--%>
                                                <asp:DropDownList CssClass="form-control" ID="ddlServices" OnSelectedIndexChanged="ddlServices_SelectedIndexChanged" AutoPostBack="false" runat="server" Width="100%">
                                                    <asp:ListItem Text="Select Service" Selected="True" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <div class="row" runat="server" id="divServiceStatus">
                                            <div class="col-md-12">
                                                <asp:Label Text="Status" runat="server" autocomplete="off" CssClass="label label-info text-uppercase" />
                                            </div>
                                            <div class="col-md-12">
                                                <%--<input placeholder="To Date" readonly autocomplete="off" type="text" runat="server" class="date form-control" id="dateto">--%>
                                                <asp:DropDownList CssClass="form-control" ID="ddlServiceStatus" AutoPostBack="false" runat="server" OnSelectedIndexChanged="ddlServiceStatus_SelectedIndexChanged" Width="100%">
                                                    <asp:ListItem Text="Select Status" Selected="True" />
                                                    <asp:ListItem Text="Fail" />
                                                    <asp:ListItem Text="Success" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-12">
                                <div class="row">
                                    <div class="col-md-12" style="margin-top: 18px;">
                                        <asp:Button Text="Fetch Record" CssClass="btn btn-primary btn-block" OnClick="btnFetchRecords_Click" ID="btnFetchRecords" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label Text="Search/Filter" runat="server" CssClass="label label-info text-uppercase" />
                                    </div>
                                    <div class="col-md-12">
                                        <asp:TextBox ID="txtSearch" onkeydown="return (event.keyCode!=13);" placeholder="Search Text " class="form-control" runat="server" onkeyup="Search_Gridview(this)"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-12">
                                <div class="row">
                                    <div class="col-md-6 ">
                                        <div class="btn-group">
                                            <%--  <asp:Label Text="Download PDF" runat="server" CssClass="label label-info " /><asp:Label Text="Download Excel" runat="server" CssClass="label label-info " />--%>
                                        </div>
                                    </div>
                                    <div class="col-md-12 text-center">
                                        <div class="btn-group btn-group-sm btn-group-lg" role="group">
                                            <asp:Button CssClass="btn btn-danger" runat="server" ID="btn1" type="button" Text="Export PDF" OnClick="btn1_Click" ToolTip="Download PDF" />
                                            <input id="btnExport" type="button" runat="server" value="Export Report" class="btn btn-success " title="Download Excel" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="box-body" runat="server" style="max-height: 500px; overflow-y: scroll">

                        <asp:GridView AllowPaging="true" OnPageIndexChanging="GV_Kiosk_Health_PageIndexChanging" PageSize="100" ID="GV_Kiosk_Health" class="table grid" runat="server" ClientIDMode="AutoID" HeaderStyle-BackColor=" #eafafa" OnRowDataBound="GV_Kiosk_Health_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="SR. No">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings FirstPageText="First" LastPageText="Last" PageButtonCount="4" NextPageText="Next" Mode="NextPreviousFirstLast" />

                        </asp:GridView>
                        <div class="row">
                            <div class="col-md-12 text-center">
                                <div class="" id="divError" runat="server" style="height: 50px;" visible="false">
                                    <label class="" id="lblError" style="width: 100%; font-size: 16px; height: 100%" runat="server">Error Text</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                    <%-- </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="ObjTimer_Tick">
                    </asp:Timer>--%>
                    <%--    <div class="box-footer">
                        Showing :  <span id="Show"></span><span><%=GV_Kiosk_Health.Rows.Count %></span> Entries.
                    </div>--%>
                </div>

            </section>
        </div>

    </section>
    <style>
        .grid {
            font-family: Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }

            .grid td, th {
                border: 1px solid #ddd;
                padding: 8px;
                text-align: center;
                font-weight: 500;
                color: #4c526f;
                text-transform: uppercase;
                background: white;
            }



            .grid tr:nth-child(even) {
                background-color: #f2f2f2;
            }

            .grid tr:hover td {
                background: linear-gradient(to bottom, #33ccff 0%, #ff99cc 100%);
                color: black;
                font-weight: 500;
            }

            .grid th {
                padding-top: 12px;
                padding-bottom: 12px;
                text-align: left;
                background-color: #367fa9;
                color: white;
                font-weight: 600;
                text-align: center;
                position: sticky;
                top: -12px; /* required for the stickiness */
            }
    </style>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>

        $(document).ready(function () {
            $(".date").datepicker(function () {
                maxDate: "-1d"
            });
        });

        $("#<%=datefrom.ClientID %>").change(function (e) {
            var dateStringf = document.getElementById('<%=datefrom.ClientID %>').value;
             var dateStringt = document.getElementById('<%=dateto.ClientID %>').value;
            var fromDate = new Date(dateStringf);
            var todate = new Date(dateStringt);

            if (dateStringt != "")
                if (fromDate > todate) {
                    alert('From date  cannot be greater than to date!');
                    $(this).val('');
                }
        });
        $("#<%=dateto.ClientID %>").change(function (e) {
            var dateStringt = document.getElementById('<%=dateto.ClientID %>').value;
              var dateStringf = document.getElementById('<%=datefrom.ClientID %>').value;
            var fromDate = new Date(dateStringf);
            var toDate = new Date(dateStringt);
            if (fromDate > toDate) {
                alert('From date  cannot be greater than to date!');
               $('#<%=datefrom.ClientID %>').val('');
                e.preventDefault();
            }
        });

    </script>

</asp:Content>

