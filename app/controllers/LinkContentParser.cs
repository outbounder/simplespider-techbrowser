
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace simplespidertechbrowser.app.controllers
{


	public class LinkContentParser
	{

		public LinkContentParser ()
		{
		}
		
		public List<Uri> getChildLinks(string content) 
		{
			if(content == null || content == "")
				return new List<Uri>();
			
			List<Uri> result = new List<Uri>();
			
			MatchCollection m1 = Regex.Matches(content, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);
			foreach (Match m in m1)
			{
				string value = m.Groups[1].Value;
				Match m2 = Regex.Match(value, @"href=\""(.*?)\""", RegexOptions.Singleline);
				if (m2.Success) {
					result.Add(new Uri(m2.Groups[1].Value));
				}
			}
			
			return result;
		}
	}
}
