namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class ProtocolException : Exception {
		public ProtocolException(string message)
			: base(message) {
		}

		public ProtocolException(string message, Exception innerException)
			: base(message, innerException) {
		}

		internal static void ThrowIf(bool condition, string message) {
			if (condition) {
				throw new ProtocolException(message);
			}
		}

		internal static void ThrowIfNot(bool condition, string message) {
			ThrowIf(!condition, message);
		}
	}
}
