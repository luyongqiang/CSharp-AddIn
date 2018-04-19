# C#-General-Assembly
C# generic component part of the code from the network, but made some of the package, plug-in module code is mainly from Egeye.Plugin
Because the project used to related components Syncfusion. So put the current program named Syncfusion.Addin. The main component is a plug-in module。

plguin xml 

<?xml version="1.0" encoding="utf-8" ?>
<PlugIn-Metadata xmlns="urn:plug-in-bundle-plugin.addin-2.0"
            Name="Plugin2"
            Copyright="LYACH"
            Url=""
            Description="RBIM.VisualizationViewModule"
            Enabled="true"
            Immediate="true"
            Path="Plugins\Plugin2\">
  <Runtime>
    <Import assembly="Plugins\Plugin2\bin\Plugin2.dll" isweb="false"/>
  </Runtime>
</PlugIn-Metadata>


plugin class 

    [Serializable]
    [AddIn("Activator2")]
    public class Activator2 : AddInBase
    {
        public override void Start(IBundleContext context)
        {
            Console.WriteLine("Plugin2 is Starting!!!!");

            IUserInfo userinfo = new UserInfo();
            userinfo.GetUserName();
            userinfo.userName = "Admin";
            
            context.RegisterService(typeof(IUserInfo), userinfo, user);
            context.RegisterService(typeof (Form), new Form1(), null);  
            IUserInfo _userinfo = (IUserInfo)context.GetRegisterService(typeof(IUserInfo));
            if (_userinfo != null)
            {
                foreach (string str in _userinfo.GetUserName())
                {
                    Console.WriteLine(str);
                }
            }
         var ufbio=   context.GetRegisterService(typeof(UserInfo));
        }

        public override void Stop(IBundleContext context)
        {
            Console.WriteLine("Plugin2 is Stoped!!!");
        }
    }
    
