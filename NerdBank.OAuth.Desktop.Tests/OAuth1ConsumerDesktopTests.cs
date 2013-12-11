namespace NerdBank.OAuth.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class OAuth1ConsumerDesktopTests : OAuth1ConsumerTestsBase {
		protected override OAuth1ConsumerBase CreateInstance() {
			return new OAuth1Consumer();
		}
	}
}
