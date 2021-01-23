<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>


<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        //RouteTable.Routes.Ignore("{*allaspx}", new {allaspx=@".*\.aspx(/.*)?"});

        //RouteTable.Routes.MapPageRoute("Dashboard", "PCMC/Dashboard", "~/Dashboard/PieChart.aspx");
        //RouteTable.Routes.MapPageRoute("HealthDetails", "PCMC/HealthDetails", "~/Dashboard/ReportDetails.aspx");

        //RouteCollection routes = new RouteCollection();
        // var settings = new FriendlyUrlSettings();
        //settings.AutoRedirectMode = 
        //routes.EnableFriendlyUrls(settings);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
