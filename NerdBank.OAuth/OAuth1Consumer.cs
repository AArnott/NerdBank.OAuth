//-----------------------------------------------------------------------
// <copyright file="OAuth1Consumer.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Validation;

	/// <summary>
	/// An OAuth 1.0 Consumer that signs requests using the PLAINTEXT signing algorithm.
	/// </summary>
	public class OAuth1Consumer {
		/// <summary>
		/// Initializes a new instance of the <see cref="OAuth1Consumer"/> class.
		/// </summary>
		public OAuth1Consumer() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuth1Consumer"/> class.
		/// </summary>
		/// <param name="consumerKey">The consumer key previously obtained from the service provider.</param>
		/// <param name="consumerSecret">The consumer secret previously obtained from the service provider.</param>
		public OAuth1Consumer(string consumerKey, string consumerSecret) {
			Requires.NotNullOrEmpty(consumerKey, "consumerKey");
			Requires.NotNull(consumerSecret, "consumerSecret");

			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
		}

		/// <summary>
		/// Gets or sets the consumer key previously obtained from the service provider.
		/// </summary>
		public string ConsumerKey { get; set; }

		/// <summary>
		/// Gets or sets the consumer secret previously obtained from the service provider.
		/// </summary>
		public string ConsumerSecret { get; set; }

		/// <summary>
		/// Gets or sets the URL from which to obtain temporary credentials
		/// prior to user authorization.
		/// </summary>
		public Uri TemporaryCredentialsEndpoint { get; set; }

		/// <summary>
		/// Gets or sets the base URL to direct users to so the user can authorize
		/// this consumer to access their network resources.
		/// </summary>
		public Uri AuthorizationEndpoint { get; set; }

		/// <summary>
		/// Gets or sets the URL that exchanges temporary credentials for an access token and secret
		/// after a successful user authorization.
		/// </summary>
		public Uri AccessTokenEndpoint { get; set; }

		/// <summary>
		/// Gets or sets the temporary credential username (i.e. request token).
		/// </summary>
		/// <remarks>
		/// This property is set by the <see cref="StartAuthorizationAsync"/> method,
		/// and consumed by the <see cref="CompleteAuthorizationAsync"/> method.
		/// </remarks>
		public string TemporaryToken { get; set; }

		/// <summary>
		/// Gets or sets the temporary credential password (i.e. request token secret).
		/// </summary>
		/// <remarks>
		/// This property is set by the <see cref="StartAuthorizationAsync"/> method,
		/// and consumed by the <see cref="CompleteAuthorizationAsync"/> method.
		/// </remarks>
		public string TemporarySecret { get; set; }

		/// <summary>
		/// Gets or sets the access token that may be used to authorize requests from
		/// this consumer to the service provider.
		/// </summary>
		/// <remarks>
		/// This property is set by the <see cref="CompleteAuthorizationAsync"/> method,
		/// and consumed by the <see cref="CreateOAuthMessageHandler"/> method.
		/// </remarks>
		public string AccessToken { get; set; }

		/// <summary>
		/// Gets or sets the access token secret that may be used to authorize requests from
		/// this consumer to the service provider.
		/// </summary>
		/// <remarks>
		/// This property is set by the <see cref="CompleteAuthorizationAsync"/> method,
		/// and consumed by the <see cref="CreateOAuthMessageHandler"/> method.
		/// </remarks>
		public string AccessTokenSecret { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance has an initialized value for
		/// the <see cref="AccessToken"/> property.
		/// </summary>
		public bool IsAuthorized {
			get { return !string.IsNullOrEmpty(this.AccessToken); }
		}

		/// <summary>
		/// Obtains temporary credentials and a start URL to direct the user to
		/// in order to authorize the consumer to access the user's resources.
		/// </summary>
		/// <param name="callbackUri">The URL the service provider should redirect the user agent to after authorization. This becomes the value of the oauth_callback parameter.</param>
		/// <param name="cancellationToken">A token that may be canceled to abort.</param>
		/// <returns>A task whose result will be the authorization URL.</returns>
		/// <remarks>
		/// This method sets the <see cref="TemporaryToken"/> and <see cref="TemporarySecret"/>
		/// properties. These values should be preserved (or restored) till the
		/// <see cref="CompleteAuthorizationAsync"/> method is called.
		/// </remarks>
		public async Task<Uri> StartAuthorizationAsync(Uri callbackUri = null, CancellationToken cancellationToken = default(CancellationToken)) {
			var authorizingHandler = CreateOAuthMessageHandler();
			using (var httpClient = new HttpClient(authorizingHandler)) {
				var requestUri = new UriBuilder(TemporaryCredentialsEndpoint);
				if (callbackUri != null) {
					requestUri.AppendQueryArgument("oauth_callback", callbackUri.AbsoluteUri);
				}

				var response = await httpClient.PostAsync(requestUri.Uri, new ByteArrayContent(new byte[0]), cancellationToken);
				var urlEncodedArgsResponse = await response.Content.ReadAsStringAsync();
				var argsResponse = PortableUtilities.ParseUrlEncodedString(urlEncodedArgsResponse);
				response.EnsureSuccessStatusCode();
				ProtocolException.ThrowIfNot("true" == argsResponse["oauth_callback_confirmed"], "oauth_callback_confirmed parameter not provided or does not match the expected \"true\" value in response from service provider.");
				this.TemporaryToken = argsResponse["oauth_token"];
				ProtocolException.ThrowIf(string.IsNullOrEmpty(this.TemporaryToken), "Unexpected empty or missing oauth_token parameter in response from service provider.");
				this.TemporarySecret = argsResponse["oauth_token_secret"];

				var authorizationBuilder = new UriBuilder(AuthorizationEndpoint);
				authorizationBuilder.AppendQueryArgument("oauth_token", this.TemporaryToken);
				return authorizationBuilder.Uri;
			}
		}

		/// <summary>
		/// Finalizes authorization after the user has completed the authorization steps
		/// at the user agent.
		/// </summary>
		/// <param name="callbackUri">The final URL that the service provider redirected back to to signal that authorization is complete.</param>
		/// <param name="cancellationToken">A token that may be canceled to abort.</param>
		/// <returns>
		/// A task that completes when the authorization has been finalized.
		/// The access token and token secret obtained from the authorization are then
		/// available in the <see cref="AccessToken"/> and <see cref="AccessTokenSecret"/> properties.
		/// </returns>
		public async Task CompleteAuthorizationAsync(Uri callbackUri, CancellationToken cancellationToken = default(CancellationToken)) {
			Requires.NotNull(callbackUri, "callbackUri");
			Verify.Operation(!string.IsNullOrEmpty(this.TemporaryToken), "TemporaryToken and TemporaryTokenSecret properties must be initialized first.");

			var redirectArgs = PortableUtilities.ParseQueryString(callbackUri);
			Assumes.True(String.Equals(this.TemporaryToken, redirectArgs["oauth_token"], StringComparison.Ordinal));
			string verifier = redirectArgs["oauth_verifier"];

			var handler = this.CreateOAuthMessageHandler();
			handler.AccessToken = this.TemporaryToken;
			handler.AccessTokenSecret = this.TemporarySecret;

			using (var httpClient = new HttpClient(handler)) {
				var accessTokenEndpointBuilder = new UriBuilder(AccessTokenEndpoint);
				accessTokenEndpointBuilder.AppendQueryArgument("oauth_verifier", verifier);
				var response = await httpClient.PostAsync(accessTokenEndpointBuilder.Uri, new ByteArrayContent(new byte[0]), cancellationToken);
				var urlEncodedArgsResponse = await response.Content.ReadAsStringAsync();
				var argsResponse = PortableUtilities.ParseUrlEncodedString(urlEncodedArgsResponse);
				response.EnsureSuccessStatusCode();
				this.AccessToken = argsResponse["oauth_token"];
				this.AccessTokenSecret = argsResponse["oauth_token_secret"];

				this.TemporaryToken = null;
				this.TemporarySecret = null;
			}
		}

		/// <summary>
		/// Creates an <see cref="HttpMessageHandler"/> that automatically applies the required
		/// HTTP headers to request messages to apply OAuth 1.0 obtained authorization.
		/// </summary>
		/// <returns>The OAuth 1.0 authorizing HTTP handler.</returns>
		protected OAuth1HttpMessageHandlerBase CreateOAuthMessageHandler() {
			return this.CreateOAuthMessageHandler(new HttpClientHandler());
		}

		/// <summary>
		/// Creates an <see cref="HttpMessageHandler"/> that automatically applies the required
		/// HTTP headers to request messages to apply OAuth 1.0 obtained authorization.
		/// </summary>
		/// <param name="innerHandler">The inner handler to use for transmitting the HTTP request.</param>
		/// <returns>The OAuth 1.0 authorizing HTTP handler.</returns>
		protected OAuth1HttpMessageHandlerBase CreateOAuthMessageHandler(HttpMessageHandler innerHandler) {
			var authorizingHandler = this.CreateOAuthMessageHandlerCore(innerHandler);
			authorizingHandler.ConsumerKey = this.ConsumerKey;
			authorizingHandler.ConsumerSecret = this.ConsumerSecret;
			authorizingHandler.AccessToken = this.AccessToken;
			authorizingHandler.AccessTokenSecret = this.AccessTokenSecret;
			return authorizingHandler;
		}

		/// <summary>
		/// Creates an <see cref="HttpMessageHandler"/> that automatically applies the required
		/// HTTP headers to request messages to apply OAuth 1.0 obtained authorization.
		/// </summary>
		/// <param name="innerHandler">The inner handler to use for transmitting the HTTP request.</param>
		/// <returns>The OAuth 1.0 authorizing HTTP handler.</returns>
		/// <remarks>
		/// Implementations of this method need not initialize the standard properties on the
		/// instance. The caller will initialize <see cref="ConsumerKey"/>, <see cref="ConsumerSecret"/>,
		/// <see cref="AccessToken"/> and <see cref="AccessTokenSecret"/>.
		/// </remarks>
		protected virtual OAuth1HttpMessageHandlerBase CreateOAuthMessageHandlerCore(HttpMessageHandler innerHandler) {
			return new OAuth1PlainTextMessageHandler(innerHandler);
		}
	}
}
