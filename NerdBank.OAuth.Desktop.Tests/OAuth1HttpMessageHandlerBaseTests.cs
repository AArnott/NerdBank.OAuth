namespace NerdBank.OAuth.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Text;
	using System.Threading.Tasks;
#if DESKTOP
	using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

	[TestClass]
	public abstract class OAuth1HttpMessageHandlerBaseTests {
		protected static readonly Uri RemoteUri = new Uri("http://localhost/path");

		[TestMethod]
		public void DefaultCtor() {
			var handler = this.CreateInstance();
			Assert.IsNull(handler.AccessToken);
			Assert.IsNull(handler.AccessTokenSecret);
			Assert.IsNull(handler.ConsumerKey);
			Assert.IsNull(handler.ConsumerSecret);
			Assert.IsNull(handler.InnerHandler);
			Assert.AreEqual(8, handler.NonceLength);
			Assert.AreEqual(OAuth1HttpMessageHandlerBase.OAuthParametersLocation.AuthorizationHttpHeader, handler.ParametersLocation);
		}

		[TestMethod, ExpectedException(typeof(ArgumentNullException))]
		public void ApplyAuthorizationNull() {
			var handler = this.CreateInstance();
			handler.ApplyAuthorization(null);
		}

		[TestMethod, ExpectedException(typeof(InvalidOperationException))]
		public void ApplyAuthorizationUninitializedConsumerKey() {
			var handler = this.CreateInstance();
			handler.ConsumerSecret = "some secret";
			var request = new HttpRequestMessage(HttpMethod.Get, RemoteUri);
			handler.ApplyAuthorization(request);
		}

		[TestMethod, ExpectedException(typeof(InvalidOperationException))]
		public void ApplyAuthorizationUninitializedConsumerSecret() {
			var handler = this.CreateInstance();
			handler.ConsumerKey = "some key";
			var request = new HttpRequestMessage(HttpMethod.Get, RemoteUri);
			handler.ApplyAuthorization(request);
		}

		[TestMethod]
		public void ApplyAuthorization() {
			var handler = this.CreateInstance();
			handler.ConsumerKey = "some key";
			handler.ConsumerSecret = "some secret";
			var request = new HttpRequestMessage(HttpMethod.Get, RemoteUri);
			handler.ApplyAuthorization(request);
			Assert.IsNotNull(request.Headers.Authorization);

			var args = TestUtilities.ParseAuthorizationHeader(request.Headers.Authorization.Parameter);
			Assert.AreEqual("1.0", args["oauth_version"]);
		}

		protected abstract OAuth1HttpMessageHandlerBase CreateInstance();
	}
}
