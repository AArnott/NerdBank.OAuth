namespace NerdBank.OAuth.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Xunit;

	public class OAuth1ConsumerBaseTests {
		[Fact]
		public void DefaultCtor() {
			var consumer = new MyDerived();
		}

		private class MyDerived : OAuth1ConsumerBase {
			protected override byte[] ComputeHmacSha1(byte[] data, byte[] key) {
				throw new NotImplementedException();
			}
		}
	}
}
