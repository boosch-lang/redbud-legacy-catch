using System;
using System.Web.UI;

namespace Maddux.Catch.LocalClasses
{
    public class Utils
    {
        public void RegisterStartupScriptBlock(string blockName, string script, Page page)
        {
            string scriptBlock;
            ClientScriptManager scriptManager = page.ClientScript;
            Type pageType = page.GetType();

            scriptBlock = "\n<script type='text/javascript'>\n";
            scriptBlock += script;
            scriptBlock += "\n</script>\n";

            if (!scriptManager.IsStartupScriptRegistered(pageType, blockName))
            {
                scriptManager.RegisterStartupScript(pageType, blockName, scriptBlock, false);
            }
        }

        public void RegisterClientScriptBlock(string blockName, string script, Page page)
        {
            string scriptBlock;
            ClientScriptManager scriptManager = page.ClientScript;
            Type _csType = page.GetType();

            scriptBlock = "\n<script type='text/javascript'>\n";
            scriptBlock += script;
            scriptBlock += "\n</script>\n";

            if (!scriptManager.IsClientScriptBlockRegistered(_csType, blockName))
            {
                scriptManager.RegisterClientScriptBlock(_csType, blockName, scriptBlock, false);
            }
        }
    }
}