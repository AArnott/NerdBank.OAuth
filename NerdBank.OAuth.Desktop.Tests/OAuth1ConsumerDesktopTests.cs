namespace NerdBank.OAuth.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Xunit;

	public class OAuth1ConsumerDesktopTests {
		[Fact]
		public void DefaultCtor() {
			var consumer = new OAuth1Consumer();
		}
	}
}
