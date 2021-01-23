<%@ Page Title="Precentive Maintenance | PCMC" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="PreventiveMaintence.aspx.cs" Inherits="Dashboard_PreventiveMaintence" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        var filter = 0;

        function Search_Gridview(strKey) {

            filter = 0;
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("<%=GV_Kiosk_Health.ClientID %>");
            var rowData;

            var totalRow = tblData.rows.length;
            debugger;
            for (var i = 1; i < tblData.rows.length; i++) {

                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';

                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0) {
                        styleDisplay = '';
                        filter++;

                    }
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }

                tblData.rows[i].style.display = styleDisplay;

            }
         <%--   var rowsCount = <%=GV_Kiosk_Health.Rows.Count %>;--%>
            document.getElementById("Show").innerText = filter + ' Out of ';
        }
    </script>
    <script type="text/javascript">

        $(function () {

            $('#btnExport').on('click', function () {
                ExportToExcel('<%=GV_Kiosk_Health.ClientID %>');
                // location.reload();
            });
        });

        function ExportToExcel(Id) {

            var selectedKioskID = "";
            selectedKioskID = $("#ContentPlaceHolder1_ddlkiosklist").children("option:selected").text();

            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var dateTime = date + ' ' + time;
            var tab_text = "";
            tab_text += "<table border='2px'>";
           // var tdLen = $("#<%= GV_Kiosk_Health.ClientID %> tr:first-child td").length-1;

            tab_text += "<tr> <td colspan='5' style='background-color:#367fa9;'> <h2 style='display:flex;text-align:center;color:white'> Kiosk Preventive Maintenance Report -(" + dateTime + ")</h2></td></tr>";

            var headerRow = "";

            $('#<%= GV_Kiosk_Health.ClientID %> tr:first').each(function () {
                headerRow = "<tr>";
                $(this).find('th').each(function (ind) {

                    headerRow += "<td style='text-align:center;'>" + $(this).text() + "</td>";

                });
                headerRow += "</tr>";
            });


            tab_text += headerRow;

            $('#<%= GV_Kiosk_Health.ClientID %> tr:not(:first-child)').each(function (ind) {
                tab_text += "<tr>";
                $(this).find('td').each(function (ind) {
                    if (ind == 4) {
                        if ($(this).attr('title').toLowerCase().match("passed"))
                            tab_text += "<td style='text-align:center; background:red;color:white;fotn-weight:600'>" + $(this).attr('title') + "</td>";
                        else
                            tab_text += "<td style='text-align:center; background:green;color:white;fotn-weight:600'>" + $(this).attr('title') + "</td>";
                    } else {
                        tab_text += "<td style='text-align:center;'>" + $(this).text() + "</td>";
                    }
                });
                tab_text += "</tr>";

            });
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
                    download: "PCMC_PreventiveMaintenance_" + selectedKioskID + "_" + dateTime + ".xlsx"
                }).appendTo("body").get(0).click();
                e.preventDefault();
            }
            return (sa);
        }
    </script>



    <section class="content-header">
        <h1>Preventive Maintenance Dashboard </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-th"></i>Section</a></li>
            <li class="active">Preventive Maintenance Dashboard </li>
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
                            <asp:Label Visible="false" ID="l1" runat="server" Text="Label">D1 (CashAcceptor)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l2" runat="server" Text="Label">D2 (ReceiptPrinter)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l3" runat="server" Text="Label">D3 (BarCodeReader)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l4" runat="server" Text="Label">D4 (DocScanner)</asp:Label>
                        </h4>

                    </div>

                </div>
            </div>

            <div id="div2" runat="server" class="col-lg-2" visible="false">
                <!-- small box -->
                <div class="small-box bg-yellow">
                    <div class="inner">
                        <h4>
                            <asp:Label Visible="false" ID="l5" runat="server" Text="Label">D5 (Camera)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l6" runat="server" Text="Label">D6 (VCCamera)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l7" runat="server" Text="Label">D7 (CardReader)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l8" runat="server" Text="Label">D8 (LaserPrinter)</asp:Label>
                        </h4>
                    </div>
                </div>
            </div>

            <div id="div3" runat="server" class="col-lg-2" visible="false">
                <!-- small box -->
                <div class="small-box bg-aqua">
                    <div class="inner">
                        <h4>
                            <asp:Label Visible="false" ID="l9" runat="server" Text="Label">D9 (FingerScanner)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l10" runat="server" Text="Label">D10 (KeyPadMouse)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l11" runat="server" Text="Label">D11 (SignageTV)</asp:Label>
                        </h4>
                        <h4>
                            <asp:Label Visible="false" ID="l12" runat="server" Text="Label">D12 (TouchScreen)</asp:Label>
                        </h4>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <section class="col-lg-12 ">

                <div class="box box-info">
                    <div class="box-header with-border ">
                        <div class="col-md-4">
                            <h3 class="box-title">Preventive Maintenance Status</h3>
                        </div>

                        <div class="col-md-2 " style="display: none">
                            <span>Search</span>
                            <a href="#" data-toggle="tooltip" title="Search for filter Records">
                                <asp:TextBox ID="txtSearch" onKeyDown="return (event.keyCode!=13);" class="form-control" runat="server" onkeyup="Search_Gridview(this)"></asp:TextBox>
                            </a>
                        </div>
                        <div class="col-md-2" id="filter" runat="server" visible="false" style="display: none">
                            <span>Select Filter </span>
                            <a href="#" data-toggle="tooltip" title="Select Type For Filter">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="filterlist" OnSelectedIndexChanged="filterlist_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </a>
                        </div>
                        <div class="col-md-2" id="location" runat="server" visible="false">
                            <asp:Label Text="Select Location" ID="lbllocation" runat="server"></asp:Label>
                            <a href="#" data-toggle="tooltip" title="Select Location For Filter">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="locationlist" OnSelectedIndexChanged="locationlist_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </a>
                        </div>
                        <div class="col-md-2" style="display: none">
                            <span>Download PDF</span>
                            <asp:Button Style="background-color: #ec5e5e;" runat="server" ID="btn1" type="button" Text="Export PDF" OnClick="btn1_Click" class="btn btn-success form-control" />
                        </div>
                        <div class="col-md-2">
                            <span>Download Excel</span>
                            <input id="btnExport" type="button" value="Export Excel" class="btn btn-success form-control" />
                        </div>

                        <div class="col-md-4">
                            <div class="row">
                                <div style="position: absolute; top: -12px; left: 185px; font-weight: 700; font-size: 12PX; text-transform: uppercase;">
                                    PM DUE DATE FILTER
                                </div>
                                <div class="col-md-6 col-sm-6" style="">
                                    <div class="btn-group">
                                        <span>Form Date</span>
                                        <input placeholder="From Date" readonly type="text" runat="server" class="date form-control " style="background-color: white" id="datefrom" />
                                        <span id="searchclear" class="glyphicon glyphicon-remove-circle searchclear"></span>
                                    </div>
                                </div>

                                <div class="col-md-6 col-sm-6">

                                    <div class="btn-group">
                                        <span>To Date</span>
                                        <input placeholder="To Date" readonly autocomplete="off" type="text" runat="server" style="background-color: white" class="date form-control" id="dateto">
                                        <span id="searchclear1" class="glyphicon glyphicon-remove-circle searchclear"></span>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="col-md-2 ">

                            <span>Search</span>
                            <asp:Button ID="Searchid" runat="server" class="btn btn-success form-control" OnClick="Button1_Click" Text="Search" />
                            </a>
                        </div>
                    </div>


                    <div class="box-body" runat="server" style="max-height: 500px; overflow-y: scroll">
                        <asp:GridView ID="GV_Kiosk_Health" class="table table-bordered table-fixed table-striped table-hover text-center " Font-Size="Small" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium" HeaderStyle-BackColor=" #eafafa" runat="server" ClientIDMode="AutoID" OnRowDataBound="GV_Kiosk_Health_RowDataBound1">
                        </asp:GridView>
                        <img runat="server" id="ErrorImg" src="~/Dashboard/images/download.png" style="padding-left: 40%" visible="false" />
                    </div>
                </div>

            </section>
        </div>

    </section>
    <!--page script-->
    <%-- <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>

        $(document).ready(function () {
            $(".date").datepicker(function () {
                format: "yyyy-mm-dd"
               
            });
        });

    </script>--%>




    <script src="date/formden.js"></script>
    <script src="date/jquery-1.11.3.min.js"></script>
    <link rel="stylesheet" href="https://formden.com/static/cdn/font-awesome/4.4.0/css/font-awesome.min.css" />

    <!-- Include Date Range Picker -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.4.1/js/bootstrap-datepicker.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.4.1/css/bootstrap-datepicker3.css" />

    <script>
        $(document).ready(function () {

            var date_input = $('input[name="ctl00$ContentPlaceHolder1$datefrom"]'); //our date input has the name "date"
            var date_input1 = $('input[name="ctl00$ContentPlaceHolder1$dateto"]'); //our date input has the name "date"
            var container = $('.bootstrap-iso form').length > 0 ? $('.bootstrap-iso form').parent() : "body";
            date_input.datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
            })
            date_input1.datepicker({
                format: 'yyyy-mm-dd',
                todayHighlight: true,
            })

            $("#<%=datefrom.ClientID %>").change(function (e) {
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
        });
    </script>

</asp:Content>

