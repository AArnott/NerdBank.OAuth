namespace NerdBank.OAuth.WinPhone8.Tests {
	using System;
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

	[TestClass]
	public class OAuth1HmacSha1ConsumerWinPhoneTests : OAuth1HmacSha1ConsumerTestsBase {
		protected override OAuth1Consumer CreateInstance() {
			return new OAuth1HmacSha1Consumer();
		}
	}
}
