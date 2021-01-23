﻿<%@ Page Title="Terminal Details | PCMC" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="KioskDetails.aspx.cs" Inherits="KioskDetails" %>

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

            $('[id*=btnExport]').on('click', function () {
                ExportToExcel('GV_Kiosk_Health');
                // location.reload();
            });
        });
        function ExportToExcel(Id) {

            var today = new Date();

            var colcount = document.getElementById('<%= GV_Kiosk_Health.ClientID %>').rows[0].cells.length;

            //$('tr:nth-child(1) td').each(function () {
            //    if ($(this).attr('colspan')) {
            //        colCount += +$(this).attr('colspan');
            //    } else {
            //        colCount++;
            //    }
            //    alert(colCount);
            //});
            //var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            //var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            //var dateTime = date + ' ' + time;

            dateTime = FormatedTime(today);

            var tab_text = "";
            tab_text += "<table border='2px'><tr><td colspan='" + colcount + "' align='center' style='background-color:#367fa9;color:white' ><h2> Kiosk Detail Report Generated On -(" + dateTime + ")</h2><br/></td><tr>";
            var textRange;
            var j = 0;
            tab = document.getElementById(Id);
            var headerRow = $('[id*=GV_Kiosk_Health] tr:first');

            tab_text += headerRow.html() + '</tr><tr>';
            var rows = $('[id*=GV_Kiosk_Health] tr:not(:has(th))');
            for (j = 0; j < rows.length; j++) {
                if ($(rows[j]).css('display') != 'none') {
                    tab_text = tab_text + rows[j].innerHTML + "</tr>";
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
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));
            }
            return (sa);
        }
    </script>


    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Total Number Of Registered Kiosk</h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Report</li>
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
                        <div class="col-md-6">
                            <h3 class="box-title">Kiosk Details</h3>
                        </div>
                        <div class="col-md-2">
                            <span>Search/Filter</span>
                            <a href="#" data-toggle="tooltip" title="Search for filter Records">
                                <asp:TextBox ID="txtSearch" onKeyDown="return (event.keyCode!=13);" class="form-control" runat="server" onkeyup="Search_Gridview(this)"></asp:TextBox>
                            </a>
                        </div>
                        <div class="col-md-2" style="display:none">
                            <span>Download PDF</span>
                            <asp:Button Style="background-color: #ec5e5e;" runat="server" ID="btn1" type="button" Visible="false" Text="Export PDF" OnClick="btn1_Click" class="btn btn-success form-control" />
                        </div>
                        <div class="col-md-2">
                            <span>Download Excel</span>
                            <input id="btnExport" type="button" value="Export Report" class="btn btn-success form-control" />
                        </div>
                        <%--  <div class="col-md-2">
                             <span >Download PDF</span>
                             <input id="btnPDFExport" type="button" onclick="javascript: demoFromHTML();" value="Export Report" class="btn btn-success form-control" />
                         </div>--%>
                    </div>


                    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"
                        ViewStateMode="Enabled">--%>
                    <%--<ContentTemplate--%>
                    <div class="box-body" runat="server" style="max-height: 500px; overflow-y: scroll">

                        <asp:GridView ID="GV_Kiosk_Health" Width="100%"  CssClass="table table-bordered text-center table-striped" runat="server" ClientIDMode="AutoID" HeaderStyle-BackColor=" #eafafa" OnRowDataBound="GV_Kiosk_Health_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="SR. NO.">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <%-- <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkView" Text="View" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Kiosk IP" HeaderText="Kiosk IP" ReadOnly="True" SortExpression="Kiosk IP" />
                                        <asp:BoundField DataField="Kiosk ID" HeaderText="Kiosk ID" SortExpression="Kiosk ID" />
                                        <asp:BoundField DataField="CashAcceptor" HeaderText="CashAcceptor" SortExpression="CashAcceptor" />
                                        <asp:BoundField DataField="RecieptPrinter" HeaderText="RecieptPrinter" SortExpression="RecieptPrinter" />
                                        <asp:BoundField DataField="BarcodeReader" HeaderText="BarcodeReader" SortExpression="BarcodeReader" />
                                        <asp:BoundField DataField="DocScanner" HeaderText="DocScanner" SortExpression="DocScanner" />
                                        <asp:BoundField DataField="CameraStatus" HeaderText="CameraStatus" SortExpression="CameraStatus" />
                                        <asp:BoundField DataField="DateTime" HeaderText="DateTime" SortExpression="DateTime" />
                                    </Columns>--%>
                        </asp:GridView>

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
    <!--page script-->
    <style>
        .table {
        font-size:14px;
        }
    </style>


</asp:Content>

