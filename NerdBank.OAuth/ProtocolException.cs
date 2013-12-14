namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// An exception thrown when the protocol is violated by the remote party.
	/// </summary>
	public class ProtocolException : Exception {
		/// <summary>
		/// Initializes a new instance of the <see cref="ProtocolException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public ProtocolException(string message)
			: base(message) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ProtocolException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public ProtocolException(string message, Exception innerException)
			: base(message, innerException) {
		}

		/// <summary>
		/// Throws an exception if a condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message to include in the exception if thrown.</param>
		internal static void ThrowIf(bool condition, string message) {
			if (condition) {
				throw new ProtocolException(message);
			}
		}

		/// <summary>
		/// Throws an exception if a condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message to include in the exception if thrown.</param>
		/// <param name="args">Optional formatting arguments.</param>
		internal static void ThrowIf(bool condition, string message, params object[] args) {
			if (condition) {
				throw new ProtocolException(string.Format(CultureInfo.CurrentCulture, message, args));
			}
		}

		/// <summary>
		/// Throws an exception if a condition is false.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message to include in the exception if thrown.</param>
		internal static void ThrowIfNot(bool condition, string message) {
			ThrowIf(!condition, message);
		}

		/// <summary>
		/// Throws an exception if a condition is false.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message to include in the exception if thrown.</param>
		/// <param name="args">Optional formatting arguments.</param>
		internal static void ThrowIfNot(bool condition, string message, params object[] args) {
			if (!condition) {
				throw new ProtocolException(string.Format(CultureInfo.CurrentCulture, message, args));
			}
		}
	}
}
