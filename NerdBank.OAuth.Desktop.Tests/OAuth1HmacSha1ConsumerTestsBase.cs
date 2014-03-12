namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
#if DESKTOP
	using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

	[TestClass]
	public abstract class OAuth1HmacSha1ConsumerTestsBase : OAuth1ConsumerTestsBase {
		[TestMethod]
		public void InitializingCtor() {
			var consumer = new OAuth1HmacSha1Consumer(TestUtilities.ConsumerKey, TestUtilities.ConsumerSecret);
			Assert.AreEqual(TestUtilities.ConsumerKey, consumer.ConsumerKey);
			Assert.AreEqual(TestUtilities.ConsumerSecret, consumer.ConsumerSecret);
		}
	}
}
