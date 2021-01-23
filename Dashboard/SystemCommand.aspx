﻿<%@ Page Title="Admin Panel | PCMC" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="SystemCommand.aspx.cs" Inherits="Dashboard_BroadcastTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        function CheckAll(oCheckbox) {
            debugger;
          //  var GridView2 = document.getElementById("<%=GV_Kiosk_Details.ClientID %>");
            var rows1 = $('[id*=GV_Kiosk_Details] tr:not(:has(th))');
            for (i = 0; i < rows1.length; i++) {
                if ($(rows1[i]).css('display') != 'none') {
                    rows1[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
                }
            }
        }

    </script>


    <script type="text/javascript">
        $(function () {
           
            $('[id*=btnExport]').on('click', function () {
                ExportToExcel('GV_Kiosk_Details');
                //location.reload();
            });
        });
        function ExportToExcel(Id) {

            reportDateTime = FormatedTime(new Date());
            var tab_text = "";
            tab_text += "<table border='2px'><tr><td style='background-color:#367fa9;color:white' align='center' colspan='6'><h2>Windows System Command Report Generated On (" + reportDateTime + ")</h2><br/></td></tr><tr>";
            var textRange;
            var j = 0;
            tab = document.getElementById(Id);
            var headerRow = $('[id*=GV_Kiosk_Details] tr:first').children();

            for (var i = 1; i < headerRow.length; i++) {
                tab_text +="<td align='center' style='font-weight:bold;' >"+ headerRow[i].innerHTML; +"</td>";
            }

            //tab_text += headerRow.html() + '</tr>';
            tab_text +=  '</tr>';

            var rows = $('[id*=GV_Kiosk_Details] tr:not(:has(th))');

            for (j = 0; j < rows.length; j++) {
                
                if ($(rows[j]).css('display') != 'none') {
                    tab_text += "<tr>";
                    var row = rows[j];
                    for (var i = 1, col; col = row.cells[i]; i++) {
                        if(col.innerText.toString().trim()!="")
                            tab_text += "<td align='center' style='font-weight:400;' >" + col.innerHTML + "</td>";
                        else
                            tab_text += "<td align='center' style='font-weight:400;' > N/A </td>";
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
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));
            }
            return (sa);
        }
    </script>


    <script type="text/javascript">
        function Search_Gridview(strKey) {
          
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById("<%=GV_Kiosk_Details.ClientID %>");
            var rowData;
            for (var i = 1; i <= tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>







    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Windows System Command
                    <small>(Command INI Set Panel)</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="Adminpanel.aspx"><i class="fa fa-dashboard"></i>Admin Panel</a></li>
            <li class="active">System Command</li>
        </ol>
    </section>
    <section class="content">
        <div class="row">
            <section class="col-lg-12 ">
                <!--data table box-->
                <div class="box box-info">
                    <div class="box-header with-border ">
                        <div class="col-md-2">
                            <span>Select Type </span>
                            <a href="#" data-toggle="tooltip" title="Select The Command To be Performed By KIOSK">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="tempList" OnSelectedIndexChanged="tempList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </a>
                        </div>

                         <div runat="server" class="col-md-2" id="div1" visible="false">
                            <span>Select Value </span>
                            <a href="#" data-toggle="tooltip" title="Select The Value For Command INI">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="list1" OnSelectedIndexChanged="list1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </a>
                        </div>

                        <div runat="server" class="col-md-2" id="div2" visible="false">
                            <span>Select Value </span>
                            <a href="#" data-toggle="tooltip" title="Select template for broadcast on kiosks">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="list2"></asp:DropDownList>
                            </a>
                        </div>

                        <div runat="server" id="div3" visible="false" class="col-md-2">
                            <span>Select Start/End Date</span>
                            <div class="form-group">

                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <i class="fa fa-clock-o"></i>
                                    </div>
                                    <a href="#" data-toggle="tooltip" title="Select start date/time and end date/time  ">
                                        <input type="text" class="form-control pull-right" name="BroadcastdateTime" id="BroadcastdateTime" />
                                    </a>

                                </div>
                            </div>
                        </div>


                         <div runat="server" class="col-md-2" id="div4" visible="false">
                            <span>Enter Command </span>
                            <a href="#" data-toggle="tooltip" title="Write command in text">
                                <asp:TextBox CssClass="form-control" runat="server" ID="CommadText"></asp:TextBox>
                            </a>
                        </div>

                        <div class="col-md-2">
                            <span>Search</span>
                             <a href="#" data-toggle="tooltip" title="Search for filter Records">
                            <asp:TextBox ID="txtSearch" class="form-control" runat="server" onKeyDown="return (event.keyCode!=13);" onkeyup="Search_Gridview(this)"></asp:TextBox>
                       </a>
                                  </div>

                        <div class="col-md-2">
                            <span>Download</span>
                            <input id="btnExport" type="button" value="Export Report" class="btn btn-success form-control" />

                        </div>

                    </div>

                    <!-- /.box-header -->
                    <div class="box-body" style="max-height: 500px; overflow-y: scroll">


                        <asp:GridView ID="GV_Kiosk_Details" class="table table-bordered table-striped table-hover" runat="server" ClientIDMode="AutoID" HeaderStyle-BackColor=" #eafafa" OnRowDataBound="GV_Kiosk_Details_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <HeaderTemplate>
                                        <a href="#" data-toggle="tooltip" title="Select All Checkbox"> <input id="Checkbox2" tooltip="Show" type="checkbox" onclick="CheckAll(this)" runat="server" /></a>
                                       
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbSelect" CssClass="gridCB" runat="server"></asp:CheckBox>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>
                        </asp:GridView>

                    </div>
                    <!-- /.box-body -->

                    <%-- box-footer--%>
                    <div class="box-footer">
                        <div class="col-md-5">
                           <%-- <asp:Button runat="server" ID="Broadcast" OnClick="Broadcast_Click" Text="Instant Broadcast" CssClass="btn btn-info form-control " />--%>
                        </div>
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="updateini" CssClass="btn btn-success form-control" OnClick="Broadcast_Click" Text="Save Command" />
                        </div>
                         <div class="col-md-5">
                           <%-- <asp:Button runat="server" ID="Button1" OnClick="Broadcast_Click" Text="Instant Broadcast" CssClass="btn btn-info form-control " />--%>
                        </div>
                    </div>
                    <%--/box body--%>

                    <%--   <div class="row">
                       
                        <asp:Button runat="server" ID="Preview" OnClick="Preview_Click" Text="Preview" CssClass="btn btn-danger" />
                <asp:Button runat="server" ID="Modify" OnClick="Modify_Click" Text="Modify This Template" CssClass="btn btn-info" />
                <asp:Button runat="server" ID="Delete" OnClick="Delete_Click" Text="Delete" CssClass="btn btn-warning" />
                    </div>--%>
                </div>

            </section>
        </div>
    </section>
</asp:Content>
