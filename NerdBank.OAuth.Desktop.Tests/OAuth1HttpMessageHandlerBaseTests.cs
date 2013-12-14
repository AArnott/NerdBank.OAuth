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
	public abstract class OAuth1HttpMessageHandlerBaseTests {
		[TestMethod]
		public void DefaultCtor() {
			var handler = this.CreateInstance();
			Assert.IsNull(handler.AccessToken);
			Assert.IsNull(handler.AccessTokenSecret);
			Assert.IsNull(handler.ConsumerKey);
			Assert.IsNull(handler.ConsumerSecret);
		}

		protected abstract OAuth1HttpMessageHandlerBase CreateInstance();
	}
}
