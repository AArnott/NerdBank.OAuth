namespace NerdBank.OAuth.WinPhone8.Tests {
	using System;
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

	[TestClass]
	public class OAuth1ConsumerWinPhoneTests : OAuth1ConsumerTestsBase {
		protected override OAuth1ConsumerBase CreateInstance() {
			return new OAuth1Consumer();
		}
	}
}
