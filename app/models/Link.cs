
using System;

namespace simplespidertechbrowser.app.models
{


	public class Link
	{
		public Uri url;
		public string content;
		
		public Link (Uri url, string content)
		{
			this.url = url;
			this.content = content;
		}
	}
}
