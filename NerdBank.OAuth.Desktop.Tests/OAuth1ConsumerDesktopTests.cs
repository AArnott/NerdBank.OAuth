namespace NerdBank.OAuth.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class OAuth1ConsumerPlainTextTests : OAuth1ConsumerTestsBase {
		[TestMethod]
		public void InitializingCtor() {
			var consumer = new OAuth1Consumer(TestUtilities.ConsumerKey, TestUtilities.ConsumerSecret);
			Assert.AreEqual(TestUtilities.ConsumerKey, consumer.ConsumerKey);
			Assert.AreEqual(TestUtilities.ConsumerSecret, consumer.ConsumerSecret);
		}
		
		protected override OAuth1Consumer CreateInstance() {
			return new OAuth1Consumer();
		}
	}
}
