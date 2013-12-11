namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
#if DESKTOP
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.Net.Http;
#else
	using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#endif

	[TestClass]
	public abstract class OAuth1ConsumerTestsBase {
		protected static readonly Uri TemporaryCredentialsEndpoint = new Uri("http://localhost/tempcred");
		protected static readonly Uri AuthorizationEndpoint = new Uri("http://localhost/auth");
		protected const string ConsumerKey = "some key";
		protected const string ConsumerSecret = "some secret";
		protected const string TempCredToken = "tempcred token";
		protected const string TempCredTokenSecret = "tempcred tokensecret";

		protected Func<HttpRequestMessage, HttpResponseMessage> MockHandler { get; set; }

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
			consumer.ConsumerKey = ConsumerKey;
			consumer.ConsumerSecret = ConsumerSecret;
			try {
				await consumer.StartAuthorizationAsync();
				Assert.Fail("Expected exception not thrown.");
			} catch (InvalidOperationException) { }
		}

		[TestMethod]
		public async Task StartAuthorizationAsyncBeforeConsumerSecretEndpointInitialization() {
			var consumer = this.CreateInstance();
			consumer.ConsumerKey = ConsumerKey;
			consumer.TemporaryCredentialsEndpoint = TemporaryCredentialsEndpoint;
			try {
				await consumer.StartAuthorizationAsync();
				Assert.Fail("Expected exception not thrown.");
			} catch (InvalidOperationException) { }
		}

		[TestMethod]
		public async Task StartAuthorizationAsync() {
			this.MockHandler = req => {
				Assert.AreEqual(TemporaryCredentialsEndpoint, req.RequestUri);
				Assert.AreEqual(HttpMethod.Post, req.Method);
				return new HttpResponseMessage {
					Content = new FormUrlEncodedContent(
						new Dictionary<string, string> {
							{ "oauth_callback_confirmed", "true" },
							{ "oauth_token", TempCredToken },
							{ "oauth_token_secret", TempCredTokenSecret },
						}),
				};
			};

			var consumer = this.CreateInstance();
			consumer.HttpMessageHandler = this.MockMessageHandler;
			consumer.ConsumerKey = ConsumerKey;
			consumer.ConsumerSecret = ConsumerSecret;
			consumer.TemporaryCredentialsEndpoint = TemporaryCredentialsEndpoint;
			consumer.AuthorizationEndpoint = AuthorizationEndpoint;
			var authUri = await consumer.StartAuthorizationAsync();
			Assert.IsNotNull(authUri);
		}

		protected abstract OAuth1Consumer CreateInstance();

		private class MockMessageHandlerClass : HttpMessageHandler {
			private readonly OAuth1ConsumerTestsBase tests;

			internal MockMessageHandlerClass(OAuth1ConsumerTestsBase tests) {
				this.tests = tests;
			}

			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) {
				return Task.FromResult(this.tests.MockHandler(request));
			}
		}
	}
}
