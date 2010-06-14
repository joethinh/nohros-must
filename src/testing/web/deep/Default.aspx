<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_Default1.aspx.cs" Inherits="tdd._Default1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript" language="javascript" src="../js/nohrosnet/merge.js.ashx"></script>
    <link rel="stylesheet" type="text/css" href="../css/nohrosnet/merge.css.ashx" />
    <script type="text/javascript" language="javascript">
        jQuery(document).ready(function() {
            jQuery.copy("GALO DOIDO");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
	    <div id='nhnet_alert' class='nhnet_alert'>
            <div id='nh_rtleft'>
              <div id='nh_rtright'>
                <h3 id='nhnet_alerth3'></h3>
                <p id='nhnet_alertp'></p>
              </div>
            </div>
            <div id='nh_rbleft'>
             <div id='nh_rbright'>&nbsp;</div>
            </div>
          </div>
    </form>
</body>
</html>
