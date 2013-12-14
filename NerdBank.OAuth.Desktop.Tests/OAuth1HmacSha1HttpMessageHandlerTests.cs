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
	public class OAuth1HmacSha1HttpMessageHandlerTests : OAuth1HttpMessageHandlerBaseTests {
		[TestMethod]
		public void SignatureMethod() {
			var derived = new OAuth1HmacSha1HttpMessageHandlerDerived();
			Assert.AreEqual("HMAC-SHA1", derived.SignatureMethod);
		}

		protected override OAuth1HttpMessageHandlerBase CreateInstance() {
			return new OAuth1HmacSha1HttpMessageHandler();
		}

		private class OAuth1HmacSha1HttpMessageHandlerDerived : OAuth1HmacSha1HttpMessageHandler {
			internal new string SignatureMethod {
				get { return base.SignatureMethod; }
			}
		}
	}
}
