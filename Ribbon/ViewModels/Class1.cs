using System;
using System.Collections.Generic;
using System.Linq;
public class Class1
{
	public Class1()
	{
		var allSystems = new List<System>();
		var search = allSystems.Where(x => (x.industry == Industry.Aerospace 
											|| x.industry == Industry.Automotive) 
											&& x.author.Contains("Samuel"));
	}

	class System
	{
		public string author;
		public Industry industry;
	}

	public enum Industry
	{
		Aerospace,
		Automotive
	}
}