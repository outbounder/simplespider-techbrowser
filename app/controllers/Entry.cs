
using System;

namespace simplespidertechbrowser.app.controllers
{


	public class Entry
	{
		public Entry ()
		{
		}
		
		public void createFromLink(string url)
		{
			
		}
		
		public void saveAt(simplespidertechbrowser.app.models.Entry entry, string host)
		{
			host = host+"/entries/submit?url="+entry.url+"&tags="+string.Join(" ",entry.tagsRaw.ToArray());
		}
		
	}
}
