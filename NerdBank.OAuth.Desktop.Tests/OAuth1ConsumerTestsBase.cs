namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
#if DESKTOP
	using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

	[TestClass]
	public abstract class OAuth1ConsumerTestsBase {
		protected Func<HttpRequestMessage, Task<HttpResponseMessage>> MockHandler { get; set; }

		protected HttpMessageHandler MockMessageHandler { get; private set; }

		[TestInitialize]
		public virtual void Initialize() {
			this.MockMessageHandler = new MockMessageHandlerClass(this);
		}

		[TestMethod]
		public void DefaultCtor() {
			var consumer = this.CreateInstance();
		}

		[TestMethod]
		public async Task StartAuthorizationAsyncBeforeTempCredEndpointInitialization() {
			var consumer = this.CreateInstance();
			consumer.ConsumerKey = TestUtilities.ConsumerKey;
			consumer.ConsumerSecret = TestUtilities.ConsumerSecret;
			try {
				await consumer.StartAuthorizationAsync("oob");
				Assert.Fail("Expected exception not thrown.");
			} catch (InvalidOperationException) {
			}
		}

		[TestMethod]
		public async Task StartAuthorizationAsyncBeforeConsumerSecretEndpointInitialization() {
			var consumer = this.CreateInstance();
			consumer.ConsumerKey = TestUtilities.ConsumerKey;
			consumer.TemporaryCredentialsEndpoint = TestUtilities.TemporaryCredentialsEndpoint;
			try {
				await consumer.StartAuthorizationAsync("oob");
				Assert.Fail("Expected exception not thrown.");
			} catch (InvalidOperationException) {
			}
		}

		[TestMethod]
		public async Task StartAuthorizationAsync() {
			this.MockHandler = async req => {
				Assert.AreEqual(TestUtilities.TemporaryCredentialsEndpoint, req.RequestUri);
				Assert.AreEqual(HttpMethod.Post, req.Method);
				var oauthArgs = TestUtilities.ParseAuthorizationHeader(req.Headers.Authorization.Parameter);
				Assert.AreEqual("oob", oauthArgs["oauth_callback"]);
				Assert.AreEqual(TestUtilities.ConsumerKey, oauthArgs["oauth_consumer_key"]);
				Assert.IsTrue(string.IsNullOrEmpty(oauthArgs["oauth_token"]));
				Assert.IsFalse(string.IsNullOrEmpty(oauthArgs["oauth_signature_method"]));
				Assert.IsFalse(string.IsNullOrEmpty(oauthArgs["oauth_timestamp"]));
				Assert.IsFalse(string.IsNullOrEmpty(oauthArgs["oauth_nonce"]));
				Assert.AreEqual("1.0", oauthArgs["oauth_version"]);

				string requestContent = await req.Content.ReadAsStringAsync();
				Assert.AreEqual(string.Empty, requestContent);

				return new HttpResponseMessage {
					Content = new FormUrlEncodedContent(
						new Dictionary<string, string> {
							{ "oauth_callback_confirmed", "true" },
							{ "oauth_token", TestUtilities.TempCredToken },
							{ "oauth_token_secret", TestUtilities.TempCredTokenSecret },
						}),
				};
			};

			var consumer = this.CreateInstance();
			consumer.HttpMessageHandler = this.MockMessageHandler;
			consumer.ConsumerKey = TestUtilities.ConsumerKey;
			consumer.ConsumerSecret = TestUtilities.ConsumerSecret;
			consumer.TemporaryCredentialsEndpoint = TestUtilities.TemporaryCredentialsEndpoint;
			consumer.AuthorizationEndpoint = TestUtilities.AuthorizationEndpoint;
			var authUri = await consumer.StartAuthorizationAsync("oob");
			Assert.AreEqual(TestUtilities.AuthorizationEndpoint.AbsoluteUri + "?oauth_token=" + Uri.EscapeDataString(TestUtilities.TempCredToken), authUri.AbsoluteUri);
		}

		protected abstract OAuth1Consumer CreateInstance();

		private class MockMessageHandlerClass : HttpMessageHandler {
			private readonly OAuth1ConsumerTestsBase tests;

			internal MockMessageHandlerClass(OAuth1ConsumerTestsBase tests) {
				this.tests = tests;
			}

			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) {
				return this.tests.MockHandler(request);
			}
		}
	}
}
