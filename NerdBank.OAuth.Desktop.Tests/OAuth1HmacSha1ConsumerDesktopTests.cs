﻿namespace NerdBank.OAuth.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class OAuth1HmacSha1ConsumerDesktopTests : OAuth1HmacSha1ConsumerTestsBase {
		protected override OAuth1Consumer CreateInstance() {
			return new OAuth1HmacSha1Consumer();
		}
	}
}