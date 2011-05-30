
using System;
using System.Collections.Generic;

using System.Net;

namespace simplespidertechbrowser.app.controllers
{
	
	public delegate void LinkHandler(object sender, models.Link link);
	public delegate void LinksHandler(object sender, List<models.Link> link);

	public class Link
	{
		public int fetchCount = 0;
		public bool verbose = false;
		List<models.Link> result = new List<models.Link>();
		
		public Link (bool verbose)
		{
			this.verbose = verbose;
		}
		
		public WebClient fetchLink(Uri url, LinkHandler handler)
		{
			DateTime startTime = DateTime.Now;
			this.fetchCount += 1;
			if(this.verbose)
				System.Console.WriteLine("fetching content for: "+url.AbsoluteUri+" "+this.fetchCount);
			WebClient cl = new WebClient();
			
			cl.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e) {
				if(this.verbose)
					System.Console.WriteLine("got content for "+url.AbsoluteUri+" time: "+(DateTime.Now-startTime));
				this.fetchCount -= 1;
				handler(sender, new models.Link(url, e.Result));
			};
			
			cl.DownloadStringAsync(url);
			return cl;
		}
		
		/*
		 * starts fetching targetUrl & its child links. Only links which are under target are returned
		 */
		public void fetchChilds(Uri targetUrl, int depth, LinksHandler handler)
		{
			LinkContentParser contentParser = new LinkContentParser();
			
			LinkHandler handleFetchLinkResult = delegate (object sender, models.Link link) {
				if(this.result.Find(delegate(models.Link item){
							return item.url.AbsoluteUri == link.url.AbsoluteUri;
					}) == null)
					this.result.Add(link);

				
				if(depth - 1 < 0 && fetchCount == 0) {
					handler(this, this.result);
					return;
				}
				
				if(depth -1 < 0)
					return;
					
				List<Uri> linkChilds = contentParser.getChildLinks(link.content);
				for(var i = 0; i<linkChilds.Count; i++)
					if(linkChilds[i].Host == targetUrl.Host	&& 
				   		this.result.Find(delegate(models.Link item){
							return item.url.AbsoluteUri == linkChilds[i].AbsoluteUri;
						}) == null){
						this.fetchChilds(linkChilds[i], depth-1, handler);
					}
				if(linkChilds.Count == 0 && fetchCount == 0)
					handler(this, this.result);
			};
			
			this.fetchLink(targetUrl, handleFetchLinkResult);
		}
		
		/*
		 * starts walking targetUrl & its related links. Links all related to target are returned
		 */
		public void fetchAll(Uri targetUrl, int depth, LinksHandler handler)
		{
			LinkContentParser contentParser = new LinkContentParser();
			
			LinkHandler handleFetchLinkResult = delegate (object sender, models.Link link) {
				if(this.result.Find(delegate(models.Link item){
							return item.url.AbsoluteUri == link.url.AbsoluteUri;
					}) == null)
					this.result.Add(link);
				
				if(depth - 1 < 0 && fetchCount == 0) {
					handler(this, this.result);
					return;
				}
				
				if(depth -1 < 0)
					return;
				
				List<Uri> linkChilds = contentParser.getChildLinks(link.content);
				for(var i = 0; i<linkChilds.Count; i++) {
					if(this.result.Find(delegate(models.Link item){
							return item.url.AbsoluteUri == linkChilds[i].AbsoluteUri;
					}) == null)
						this.fetchAll(linkChilds[i], depth-1, handler);
				}
				if(linkChilds.Count == 0 && fetchCount == 0)
					handler(this, this.result);
			};
			
			this.fetchLink(targetUrl, handleFetchLinkResult);
		}
	}
}
