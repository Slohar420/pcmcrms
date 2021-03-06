﻿<%@ Page Title="Ad Management | PCMC" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="~/Dashboard/ScreenSaverDelation.aspx.cs" Inherits="Dashboard_ScreenSDelation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        function CheckAll(oCheckbox) {
          <%--//  var GridView2 = document.getElementById("<%=GV_Kiosk_Details.ClientID %>");--%>
            var rows1 = $('[id*=GV_Kiosk_Details] tr:not(:has(th))');
            for (i = 0; i < rows1.length; i++) {
                if ($(rows1[i]).css('display') != 'none') {
                    rows1[i].cells[0].getElementsByTagName("INPUT")[0].checked = oCheckbox.checked;
                }
            }
        }

    </script>

    <section class="content-header">
        <h1>Kiosk Ad Management</h1>
        <ol class="breadcrumb">
            <li><a href="PieChart.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Kiosk Ad Management</li>
        </ol>
    </section>

    <style>
        .customcard {
            width: auto;
            margin: auto;
            padding: 10px;
            background-color: white;
            border-radius: 10px;
            height: auto;
        }

            .customcard:hover {
                box-shadow: 2px 2px 2px 1px rgba(0, 0, 0, 0.2);
            }

        .header {
            margin: 5px;
            width: 100%;
        }

        .btnhover:hover {
            background-color: #0026ff;
            color: white;
            font-weight: 600;
            border: none;
        }

        .btnhover {
            background-color: white;
            border: none;
            color: black;
            font-weight: 600;
        }

        .headinglabels {
            font-size: 12px;
            font-weight: 500;
            font-family: 'Times New Roman';
        }
    </style>

    <div class="row" style="padding-top: 50px;" runat="server" id="gridDiv">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="customcard">
                <div class="row">
                    <div class="col-md-6">
                        <div class="header text-right" style="width: 100%">
                            <button type="button" class="btn btn-lg" runat="server" id="clientBtnAddService" style="background-color: #367fa9; color: white; display: none; margin-right: 20px;" data-toggle="modal" data-target="#myModal">Add Service <span class="glyphicon glyphicon-plus" style="padding-top: 3px;"></span></button>
                            <asp:DropDownList ID="ddlkiosklist" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlkiosklist_SelectedIndexChanged" Width="50%" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="header text-right">
                            <button type="button" class="btn btn-lg  btn-link " onclick="" style="" id="btnExportExcel">Download Report<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 20px">
                    <div class="col-md-12">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" CssClass="table  table-bordered text-center" OnRowCommand="servicesGrid_RowCommand" ID="servicesGrid" OnRowDataBound="servicesGrid_RowDataBound" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="SR" DataField="SR" />
                                        <asp:BoundField HeaderText="File" DataField="File" />
                                        <asp:BoundField HeaderText="Updated Datetime" DataField="Datetime" />
                                        <asp:ButtonField HeaderText="" ButtonType="Link" ControlStyle-CssClass="btn btn-link" ControlStyle-Width="100%" ItemStyle-Width="100px" CommandName="_delete" Text="Delete" />
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddService" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-12 text-center" runat="server" id="gridErrorDiv" style="padding-left: 50px; padding-right: 50px">
                        <div style="background-color: red;">
                            <label runat="server" id="lblGridError" style="width: auto; font-size: 15px; color: white"></label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-2"></div>

    </div>






    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div runat="server" id="confirmmodal" style="display: none; margin: auto; width: 60%; padding: 10px; position: absolute; top: 200px; left: 300px; background-color: #367fa9; border-radius: 20px;">
                <div class="row text-uppercase">
                    <div class="col-md-2"></div>
                    <div class="col-md-8">
                        <div class="row">
                            <div class="col-lg-12 ">
                                <h3 style="color: white">Do you want to delete this service?</h3>
                            </div>
                            <div class="col-md-4"></div>
                            <div class="col-md-2">
                                <asp:Button Text="YES" runat="server" ID="btnyes" OnClick="btnyes_Click" CssClass="btn btn-success" />
                            </div>

                            <div class="col-md-2">
                                <asp:Button Text="NO" runat="server" ID="btnno" OnClick="btnno_Click" CssClass="btn btn-danger" />
                            </div>
                        </div>
                        <div class="col-md-4"></div>
                    </div>
                    <div class="col-md-2"></div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="servicesGrid" />
        </Triggers>
    </asp:UpdatePanel>


    <!-- Service addition modal -->
    <div id="myModal" class="modal  fade" role="dialog">
        <div class="modal-dialog ">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title">SERVICE ADDITION</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-1"></div>
                        <div class="col-md-10">
                            <div class="row">
                                <div class="col-md-12">
                                    <label class="label label-info headinglabels">Service Name</label>
                                    <asp:TextBox runat="server" placeholder="Service Name (eg. WaterTax)" ID="txtServiceName" CssClass="form-control" />
                                </div>
                                <div class="col-md-12">
                                    <label class="label label-info headinglabels">Initial Status</label><br />
                                    <div class="btn-group" style="padding-left: 10px;">
                                        <asp:RadioButton CssClass="btn radio-inline rdbtn" GroupName="r" Text="Operational" ID="radioOperational" runat="server" />
                                        <asp:RadioButton CssClass="btn radio-inline rdbtn" Text="Disable" GroupName="r" ID="radioDisable" runat="server" />
                                        <asp:RadioButton CssClass="btn radio-inline rdbtn" Text="InPipeline" GroupName="r" ID="radioInPipeline" runat="server" />
                                    </div>
                                </div>

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-8 " runat="server" id="divError">
                                            <asp:Label Text="" Style="font-weight: 600; background-color: darkorange; color: white" ID="lblerror" runat="server" />
                                        </div>
                                        <div class="col-md-4 text-right">
                                            <asp:Button Text="ADD" ID="btnAddService" type="button" OnClientClick="javascript:return validate()" CssClass="btn btn-success " Style="width: 100px; margin: auto; position: relative" OnClick="btnAddService_Click" runat="server" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAddService" />
                                    </Triggers>
                                </asp:UpdatePanel>


                            </div>
                        </div>
                        <div class="col-md-1"></div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-default" data-dismiss="modal">Close </button>
                </div>
            </div>

        </div>
    </div>
    <!--Modal close-->


    <script type="text/javascript">  




        function validate() {
            var returnArg = false;
            try {

                var servicename = $('#' + '<%= txtServiceName.ClientID %>').val();

                if (servicename != "") {
                    if ($('#' +'<%= radioOperational.ClientID %>').prop('checked') || $('#' +'<%= radioDisable.ClientID %>').prop('checked') || $('#' +'<%= radioInPipeline.ClientID %>').prop('checked')) {

                        returnArg = true;
                    }
                    else {
                        returnArg = false;
                        alert("Please check service initial status");
                    }
                } else {
                    returnArg = false;
                    alert("Please enter service name");
                }
            } catch (e) {

            }
            return returnArg;
        }
        function customalert(data) {
            alert(data);
        }

        function hideErrorDiv() {

        }

        $("#clientBtnAddService").click(function () {
            clearfields();
        });

        function clearfields() {

            document.getElementById('<%= lblerror.ClientID %>').style.display = 'none';
            $('#' + '<%= txtServiceName.ClientID %>').val('');
            $('#' + '<%= radioOperational.ClientID %>').prop('checked', false);
            $('#' + '<%= radioDisable.ClientID %>').prop('checked', false);
            $('#' + '<%= radioInPipeline.ClientID %>').prop('checked', false);
        }

        $(function () {

            $('[id*=btnExportExcel]').on('click', function () {


                ExportToExcel('servicesGrid');
                // location.reload();
            });
        });

        $('#<%= ddlkiosklist.ClientID %>').change(function () {
            checkIndex();
        });

        checkIndex();

        function checkIndex() {
            debugger;
            if ($('#<%= ddlkiosklist.ClientID %>').prop("selectedIndex") < 1) {
                $("#btnExportExcel").attr("disabled","disabled");
            } else {
                $("#btnExportExcel").removeAttr("disabled");
            }
        }

        function ExportToExcel(Id) {

            var selectedKioskID = "";
            selectedKioskID = $("#ContentPlaceHolder1_ddlkiosklist").children("option:selected").text();

            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var dateTime = date + ' ' + time;
            var tab_text = "";
            tab_text += "<table border='2px'>";
           // var tdLen = $("#<%= servicesGrid.ClientID %> tr:first-child td").length-1;
            debugger;
            tab_text += "<tr> <td colspan='3' style='background-color:#367fa9;' > <h2 style='display:flex;text-align:center;color:white'> Kiosk Screen-Saver Report for " + selectedKioskID + " -(" + dateTime + ")</h2></td></tr>";
           
            var headerRow = "";

            $('#<%= servicesGrid.ClientID %> tr:first').each(function () {
                headerRow = "<tr>";
                $(this).find('th').each(function (ind) {
                    if (ind < 3) {
                        headerRow += "<td style='text-align:center;'>" + $(this).text() + "</td>";
                    }
                });
                headerRow += "</tr>";
            });
           

            tab_text += headerRow;

             $('#<%= servicesGrid.ClientID %> tr:not(:first-child)').each(function (ind) {
               
                    tab_text += "<tr>";
                    $(this).find('td').each(function (ind) {
                        if (ind < 3) {
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
                    download: "PCMC_Ad_" + selectedKioskID + "_" + dateTime + ".xlsx"
                }).appendTo("body").get(0).click();
                e.preventDefault();
            }
            return (sa);
        }
    </script>
</asp:Content>

