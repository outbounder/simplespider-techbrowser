
using System;
using ITDM;
using System.Collections.Generic;

namespace simplespidertechbrowser.app.controllers
{


	public class Shell
	{
		public Shell ()
		{
		}
		
		public void boot(string[] args)
		{
			DateTime startTime = DateTime.Now;
			
			ConsoleCmdLine c = new ConsoleCmdLine();
			CmdLineInt depth = new CmdLineInt("depth", true, "depth value", 1,1000);
			CmdLineString url = new CmdLineString("url", true, "url value");
			CmdLineString mode = new CmdLineString("mode", true, "mode value (childs, all)");
			CmdLineInt verboseLevel = new CmdLineInt("verbose",false,"verbose level(0,1)");
			c.RegisterParameter(depth);
			c.RegisterParameter(url);
			c.RegisterParameter(mode);
			c.RegisterParameter(verboseLevel);
			c.Parse(args);
			
			controllers.Link linkController;
			if(verboseLevel == 1)
				linkController = new controllers.Link(true);
			else
				linkController = new controllers.Link(false);
			
			LinksHandler linksHandler = delegate (object sender, List<models.Link> links){
				for(var i = 0; i<links.Count; i++)
					Console.WriteLine(links[i].url);
				Console.WriteLine("time: "+(DateTime.Now - startTime).TotalMilliseconds+"ms, count: "+links.Count);
			};
			
			if(mode == "childs") {
				linkController.fetchChilds(new Uri(url), depth, linksHandler);
				return;
			}
			if(mode == "all") {
				linkController.fetchAll(new Uri(url), depth, linksHandler);
				return;
			}
			if(mode == "content") {
				linkController.fetchLink(new Uri(url), delegate (object sender, models.Link link) {
					Console.WriteLine(link.content);
					Console.WriteLine("time: "+(DateTime.Now - startTime).TotalMilliseconds+"ms, count: 1");
				});
				return;
			}
			
			Console.WriteLine("unrecognized mode. Available: childs, all, content");
		}
	}
}
