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
	public abstract class OAuth1ConsumerTestsBase {
		[TestMethod]
		public void DefaultCtor() {
			var consumer = this.CreateInstance();
		}

		protected abstract OAuth1ConsumerBase CreateInstance();
	}
}
