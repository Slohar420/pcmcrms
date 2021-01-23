<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard/DSMaster.master" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="Dashboard_CreateUser" %>

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
            <li class="active">Create Partner</li>
        </ol>
    </section>
    <!-- Main content -->
    <section class="content" style="margin-top: 40px;">
        <div class="row">
            <!-- Default box -->
            <div class="col-md-2"></div>

            <div class="col-md-8">
                <div class="box box-info">
                    <div class="box-header with-border">
                        <h3 class="box-title">User Creation</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->

                    <div class="box-body ">

                        <div class="input-group">
                            <span class="input-group-addon">@</span>
                            <asp:TextBox ID="txtusername" class="form-control " placeholder="Enter Username" runat="server" MaxLength="30" onkeypress="return RestrictSpace()" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                        </div>
                        <br />
                        <div class="input-group">
                            <span class="input-group-addon">$</span>
                            <asp:TextBox ID="txtPassword" class="form-control " placeholder="Enter Password" runat="server" MaxLength="30" onkeypress="return RestrictSpace()" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RegularExpressionValidator ID="RegExp1" runat="server"
                                ErrorMessage="Password length must be between 7 to 10 characters atleast 1 UpperCase & 1 LowerCase Alphabet, 1 Number & 1 Special Character" ForeColor="Red"
                                ControlToValidate="txtPassword"
                                ValidationExpression="^.*(?=.{7,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$" />
                        </div>

                        <%--<div class="input-group">      ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,10}" 
                            <span>Select Question </span>
                            <a href="#" data-toggle="tooltip" title="Select Question of Your Choice">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="filterlist" OnSelectedIndexChanged="filterlist_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </a>
                        </div>--%>

                        <div class="form-group" style="">
                            <div class="row">
                                <div class="col-xs-6">
                                    <span class="" style="">Select Question</span>
                                    <a href="#" data-toggle="tooltip" title="Select Question of Your Choice">
                                        <asp:DropDownList CssClass="form-control" runat="server" ID="filterlist" OnSelectedIndexChanged="filterlist_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </a>
                                </div>

                                <div class="col-xs-6">
                                    <span for="exampleInputusername1">Answer</span>
                                    <asp:TextBox ID="txtAnswer" class="form-control" placeholder="Enter the Answer For Desired Question" runat="server" onkeypress="return fn_validateNumeric();" AutoCompleteType="Disabled" autocomplete="off"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <br />

                        <div class="row">
                            <div class="col-md-3">
                                <span for="exampleInputAssignRole">Assign User Role</span>
                                <div class="form-group">
                                    <div class="checkbox ">
                                        <label>
                                            <asp:CheckBox ID="chkAdmin" runat="server" class="flat-red" Text="Administrator" OnCheckedChanged="chkAdmin_CheckedChanged" AutoPostBack="true" />

                                        </label>
                                    </div>
                                    <div class="checkbox" style="display: none">
                                        <label>
                                            <asp:CheckBox ID="chkUser" class="flat-red" runat="server" Text="User" OnCheckedChanged="chkCreator_CheckedChanged" AutoPostBack="true" />

                                        </label>
                                    </div>
                                    <div class="checkbox">
                                        <label>
                                            <asp:CheckBox ID="chkLocation" class="flat-red" Text="Location" runat="server" OnCheckedChanged="chkLocation_CheckedChanged" AutoPostBack="true" />

                                        </label>
                                    </div>
                                    <%--  <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="chkManager" class="flat-red" Text="Manager" runat="server" />
                                    
                                </label>
                            </div>--%>
                                </div>
                            </div>

                            <div class="col-md-4 text-left">
                                <style>
                                     .collapsible {
                                            background-color: #eee;
                                            color: #444;
                                            cursor: pointer;
                                            padding: 18px;
                                            width: 100%;
                                            border: none;
                                            text-align: left;
                                            outline: none;
                                            font-size: 15px;
                                        }
                                </style>
                                <div class="input-group" runat="server" id="divLocation" visible="false">
                                    <label class="label label-success">SELECT MULTIPLE LOCATION </label>

                                    <asp:CheckBoxList runat="server" Style="overflow: auto"  ToolTip="SELECT MULTIPLE LOCATION" Height="100" SelectionMode="Multiple" ID="locationChkList" CssClass="form-control"></asp:CheckBoxList>
                                </div>
                            </div>
                            <div class="col-md-5">
                                <span for="exampleInputAssignRole">Assign Partner Role <small>(select if it is a partner user)</small> </span>
                                <div class="form-group">
                                    <style>                                       

                                        .ddldiv {
                                            width: 150px;
                                            border: 1px solid #cfcfcf;
                                            height: auto;
                                            border-radius: 5px;
                                            padding: 3px 4px 4px;
                                            position: relative;
                                            z-index: 0;
                                            overflow: hidden;
                                        }

                                            .ddldiv select {
                                                border: none;
                                                background-color: transparent;
                                                outline: none;
                                                padding: 0;
                                                width: 150px;
                                                /* background: url(http://i57.tinypic.com/nnmgpy_th.png) no-repeat right; */
                                                /* background-position: 55%; */
                                            }

                                        /* not 100% sure this next option rule is needed */
                                        .dropdown option {
                                            width: 100%;
                                        }
                                    </style>
                                    <div class="ddldiv" style="width: 60%; margin: 20px 10px 10px 0px;">
                                        <asp:DropDownList runat="server" Style="border: none;" ID="ddlPartnerRole" CssClass="dropdown">
                                            <asp:ListItem Text="Select Partner Role" />
                                        </asp:DropDownList>
                                    </div>
                                    <%--<div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="chkPT" class="flat-red" runat="server" Text="PropertyTax" OnCheckedChanged="chkCreator_CheckedChanged" AutoPostBack="true" />

                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="checkWT" class="flat-red" Text="WaterTax" runat="server" OnCheckedChanged="chkCreator_CheckedChanged" AutoPostBack="true" />

                                </label>
                            </div>--%>
                                    <%--  <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="chkManager" class="flat-red" Text="Manager" runat="server" />
                                    
                                </label>
                            </div>--%>
                                </div>
                            </div>
                        </div>


                        <br />
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <!-- /.box-body -->
                                <div class="form-group">
                                    <div class="" style="text-align: center">
                                        <asp:Button ID="btnsubmit" class="btn btn-primary" runat="server" Text="Submit" OnClick="btnsubmit_Click"></asp:Button>
                                        <asp:Button ID="btnCancel" class="btn btn-primary" runat="server" Text="Clear" OnClick="btnCancel_Click"></asp:Button>
                                        <asp:Button ID="btnBack" class="btn btn-primary" runat="server" Text="Go back" OnClick="btnBack_Click"></asp:Button>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- /.box -->

                        <div class="box-footer">
                            <asp:Label ID="lblPassword" runat="server" Font-Bold="True" Visible="true"></asp:Label>



                        </div>
                    </div>
                </div>

            </div>
            <div class="col-md-2"></div>
    </section>
    <!-- /.content -->

    <script>

</script>

</asp:Content>

