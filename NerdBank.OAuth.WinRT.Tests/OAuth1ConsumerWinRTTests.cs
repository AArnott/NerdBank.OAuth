namespace NerdBank.OAuth.WinRT.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

	[TestClass]
	public class OAuth1ConsumerWinRTTests {
		[TestMethod]
		public void DefaultCtor() {
			var consumer = new OAuth1Consumer();
		}
	}
}
