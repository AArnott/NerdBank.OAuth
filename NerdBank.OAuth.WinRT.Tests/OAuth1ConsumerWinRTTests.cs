namespace NerdBank.OAuth.WinRT.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

	[TestClass]
	public class OAuth1ConsumerWinRTTests : OAuth1ConsumerTestsBase {
		protected override OAuth1ConsumerBase CreateInstance() {
			return new OAuth1Consumer();
		}
	}
}
