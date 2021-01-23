<%@ Page Title="Ad History | PCMC" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="~/Dashboard/ScreenSaverReport.aspx.cs" Inherits="ScreenSaverReport" %>

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
        <h1>Kiosk ScreenSaver History</h1>
        <ol class="breadcrumb">
            <li><a href="PieChart.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Kiosk ScreenSaver History</li>
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
                    <div class="col-md-12">
                        <div class="header text-right">
                            <button type="button" class="btn btn-lg" runat="server" id="clientBtnAddService" style="background-color: #367fa9; color: white; display: none; margin-right: 20px;" data-toggle="modal" data-target="#myModal">Add Service <span class="glyphicon glyphicon-plus" style="padding-top: 3px;"></span></button>
                            <asp:DropDownList ID="ddlkiosklist" Width="25%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlkiosklist_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row " style="margin-top: 20px">
                    <div class="col-md-12">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" CssClass="table  table-bordered text-center" OnRowCommand="servicesGrid_RowCommand" ID="servicesGrid" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="SR" DataField="SR" />
                                        <asp:BoundField HeaderText="File" DataField="File" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton CommandName="_view" CommandArgument='<%# Container.DataItemIndex %>' runat="server" Text="VIEW" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
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


    <div id="myModal" runat="server" class="modal fade" role="dialog">
        <div class="modal-dialog ">
            <!-- Modal content-->
            <div class="modal-content" style="overflow: scroll; max-height: 400px; ">
                <div class="modal-header">
                    <div class="row">
                        <div class="col-md-6">
                            <h4 class="modal-title" runat="server" id="modalHeader"></h4>
                        </div>
                        <div class="col-md-6 text-right">
                            <button type="button" class="btn btn-lg  btn-link " onclick="" style="font-size: 16px;" id="btnExportExcel">Download Report<i class="fa fa-file-excel-o" aria-hidden="true"></i></button>
                            <button id="btnClose1" type="button" class="btn btn-sm  btn-danger btnClose">&times;</button>
                        </div>
                    </div>

                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-1"></div>
                        <div class="col-md-10">
                            <asp:GridView runat="server" CssClass="table  table-bordered text-center"  ID="screensgrid" OnRowDataBound="servicesGrid_RowDataBound" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField HeaderText="Name" DataField="screensavername" Visible="false" />
                                    <asp:BoundField HeaderText="Datetime" DataField="patchupdateddatetime" />
                                    <asp:BoundField HeaderText="Command" DataField="commandtype" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="col-md-1"></div>
                </div>
            </div>
            <%--<div class="modal-footer">
                <button id="btnClose2" type="button" class="btn btn-sm btn-outline btn-danger btnClose">Close</button>
            </div>--%>
        </div>

    </div>



    <script type="text/javascript">  


        $(".btnClose").click(function () {
            $("#<%= myModal.ClientID %>").removeClass("show").addClass("fade");
        })

        $(function () {
            $('[id*=btnExportExcel]').on('click', function () {
                ExportToExcel('screensgrid');
            });
        });

        function ExportToExcel(Id) {

            var selectedKioskID = "";
            addname = $("#<%=modalHeader.ClientID %>").text();

            var today = new Date();
            var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
            var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
            var dateTime = date + ' ' + time;
            var tab_text = "";
            tab_text += "<table border='2px'>";
           // var tdLen = $("#<%= servicesGrid.ClientID %> tr:first-child td").length-1;
            debugger;
            tab_text += "<tr> <td colspan='2' style='background-color:#367fa9;' > <h2 style='display:flex;text-align:center;color:white'> Kiosk Individual Ad Report for " + addname + " -(" + dateTime + ")</h2></td></tr>";

            var headerRow = "";

            $('#<%= screensgrid.ClientID %> tr:first').each(function () {
                headerRow = "<tr>";
                $(this).find('th').each(function (ind) {
                    //if (ind > 0) {
                    headerRow += "<td style='text-align:center;'>" + $(this).text() + "</td>";
                    //}
                });
                headerRow += "</tr>";
            });


            tab_text += headerRow;

            $('#<%= screensgrid.ClientID %> tr:not(:first-child)').each(function (ind) {

                tab_text += "<tr>";
                $(this).find('td').each(function (ind) {
                    //if (ind >0 ) {
                    tab_text += "<td style='text-align:center;'>" + $(this).text() + "</td>";
                    //}
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
                    download: "PCMC_Ad_" + addname + "_" + dateTime + ".xlsx"
                }).appendTo("body").get(0).click();
                e.preventDefault();
            }
            return (sa);
        }

    </script>
</asp:Content>

