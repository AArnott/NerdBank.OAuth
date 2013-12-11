namespace NerdBank.OAuth.WinRT.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

	[TestClass]
	public class OAuth1HmacSha1ConsumerWinRTTests : OAuth1HmacSha1ConsumerTestsBase {
		protected override OAuth1Consumer CreateInstance() {
			return new OAuth1HmacSha1Consumer();
		}
	}
}
