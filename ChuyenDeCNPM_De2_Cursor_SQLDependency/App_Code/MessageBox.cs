using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for MessageBox
/// </summary>
public class MessageBox
{
    public static void Show(string strMsg, Page pge, Type type)
    {
        string csname1 = "Message Box";
        ClientScriptManager cs = pge.ClientScript;
        string cstext1 = "alert('" + strMsg + "');";
        cs.RegisterStartupScript(type, csname1, cstext1, true);
    }
}


