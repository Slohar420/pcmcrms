<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="CreatePartnerRole.aspx.cs" Inherits="Dashboard_CreatePartnerRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function fn_validateNumeric() {
            if (window.event) keycode = window.event.keyCode;
            else if (e) keycode = e.which;
            else return true;
            if (((keycode >= 65) && (keycode <= 90)) || ((keycode >= 97) && (keycode <= 122))) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function RestrictSpace() {
            if (event.keyCode == 32) {
                return false;
            }
        }


    </script>





    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>Partner Management
                  
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dasshboard"></i>Home</a></li>
            <li><a href="#">Partner Management</a></li>
            <li class="active">Create Partner Role</li>
        </ol>
    </section>
    <!-- Main content -->
    <section class="content" style="margin-top: 40px;">
        <div class="row">
            <!-- Default box -->
            <div class="col-md-3"></div>
            <div class="col-md-6">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">Partner Role</h3>
                    </div>
                    <div class="row" style="padding-top: 50px;" runat="server" id="gridDiv">
                        <div class="col-md-1"></div>
                        <div class="col-md-10">
                            <div class="customcard">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="header text-right">
                                            <button type="button" class="btn btn-lg" runat="server" id="clientBtnAddRole" style="background-color: #367fa9; color: white;padding:5px " data-toggle="modal" data-target="#myModal">Add Role<span class="glyphicon glyphicon-plus" style="padding-top: 3px;padding-left:5px"></span></button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">                                       
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:GridView runat="server" CssClass="table  table-bordered text-center"  ID="rolesGrid" OnRowCommand="rolesGrid_RowCommand" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="ID" DataField="ID" Visible="false" />
                                                        <asp:BoundField HeaderText="ROLE" DataField="ROLE" ItemStyle-Font-Bold="true"  />
                                                        <asp:BoundField HeaderText="CREATED BY" Visible="false" DataField="CREATEDBY"  />
                                                        <asp:ButtonField HeaderText="" ButtonType="Button" ControlStyle-CssClass="btn btn-danger" CommandName="delete_role" Text="Delete" />
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnAddRole" />
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
                        <div class="col-md-1"></div>
                    </div>


                    <!-- Service addition modal -->
                    <div id="myModal" class="modal  fade" role="dialog">
                        <div class="modal-dialog ">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    <h3 class="modal-title">PARTNER ROLE CREATION</h3>
                                </div>
                                <div class="modal-body">
                                    <div class="row">
                                        <div class="col-md-1"></div>
                                        <div class="col-md-10">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label class="label label-info headinglabels">Partner Role</label>
                                                    <asp:TextBox runat="server" placeholder="Enter Role (eg. WaterTax)" ID="txtPartnerRole" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-12 " style="display: none">
                                                    <label class="label label-info headinglabels">Initial Status</label><br />
                                                    <div class="btn-group" style="padding-left: 10px;">
                                                        <asp:RadioButton CssClass="btn radio-inline rdbtn" GroupName="r" Text="Operational" ID="radioOperational" runat="server" />
                                                        <asp:RadioButton CssClass="btn radio-inline rdbtn" Text="Disable" GroupName="r" ID="radioDisable" runat="server" />
                                                        <asp:RadioButton CssClass="btn radio-inline rdbtn" Text="InPipeline" GroupName="r" ID="radioInPipeline" runat="server" />
                                                    </div>
                                                </div>

                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <div class="col-md-8 " runat="server" id="divError" style="margin-top: 5px;">
                                                            <asp:Label Text="" Style="font-weight: 600; background-color: darkorange; color: white" ID="lblerror" runat="server" />
                                                        </div>
                                                        <div class="col-md-4 text-right " style="margin-top: 5px;">
                                                            <asp:Button Text="ADD" ID="btnAddRole" type="button" OnClientClick="javascript:return validate()" CssClass="btn btn-success " Style="width: 100px; margin: auto; position: relative" OnClick="btnAddRole_Click" runat="server" />
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnAddRole" />
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

                </div>
            </div>

        </div>
    </section>
    <!-- /.content -->



</asp:Content>

