namespace NerdBank.OAuth.Tests {
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
	public class OAuth1PlainTextMessageHandlerTests {
		[TestMethod]
		public void SignatureMethod() {
			var derived = new OAuth1PlainTextMessageHandlerDerived();
			Assert.AreEqual("PLAINTEXT", derived.SignatureMethod);
		}

		private class OAuth1PlainTextMessageHandlerDerived : OAuth1PlainTextMessageHandler {
			internal new string SignatureMethod {
				get { return base.SignatureMethod; }
			}
		}
	}
}
