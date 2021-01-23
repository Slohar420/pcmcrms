<%@ Page Title="Admin Panel | PCMC" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="Replenishment.aspx.cs" Inherits="Replenishment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="content-header">
        <h1>Device Replenishment Panel
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Device Replenishment</li>
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
                        <div class="header text-center">
                            <h3></h3>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12" >
                         <div class="row">
                    <div class="col-md-12" >
                       
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" CssClass="table  table-bordered text-center"  ID="servicesGrid" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderText="Kiosk ID" DataField="ID" />
                                <asp:BoundField HeaderText="" DataField="SERVICE" />
                                <asp:BoundField HeaderText="STATUS" DataField="STATUS" />
                                <asp:ButtonField HeaderText="CHANGE STATUS" ButtonType="Button" CommandName="changestatus" Text="Operational" />
                                <asp:ButtonField HeaderText="" ButtonType="Button" CommandName="delete_service" Text="Delete" />
                            </Columns>
                        </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        
                    </div>
                    <div class="col-md-12 text-center" runat="server" id="Div1" style="padding-left: 50px; padding-right: 50px">
                        <div style="background-color: red;">
                            <label runat="server" id="Label1" style="width: auto; font-size: 15px; color: white"></label>
                        </div>
                    </div>
                </div>
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
           

    <script type="text/javascript">  

       
       
    </script>
</asp:Content>

