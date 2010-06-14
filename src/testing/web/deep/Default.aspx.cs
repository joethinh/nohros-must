using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Nohros.Net;

namespace tdd
{
	public partial class _Default1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            NetResponse r = new NetResponse();
            //ClientFillTable ft = new ClientFillTable("#usr > tbody", "[['asd','asd','asd'],['asd','asd','asd']]");
            ClientShowInfo ft = new ClientShowInfo("teste", "i wanna hard sex");
            
            r.ClientActions.Add(ft);

            r.EmbedInPage(this);
		}
	}
}
