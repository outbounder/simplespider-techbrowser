using System;
using simplespidertechbrowser.app.controllers;

namespace simplespidertechbrowser
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Shell shell = new Shell();
			shell.boot(args);
		}
	}
}
