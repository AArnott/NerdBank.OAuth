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
	public abstract class OAuth1HmacSha1ConsumerTestsBase {
		[TestMethod]
		public void DefaultCtor() {
			var consumer = this.CreateInstance();
		}

		[TestMethod]
		public async Task StartAuthorizationAsyncBeforeInitialization() {
			var consumer = this.CreateInstance();
			try {
				await consumer.StartAuthorizationAsync();
				Assert.Fail("Expected exception not thrown.");
			} catch (InvalidOperationException) { }
		}

		protected abstract OAuth1Consumer CreateInstance();
	}
}
