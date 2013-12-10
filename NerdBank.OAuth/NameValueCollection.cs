namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Validation;

	public class NameValueCollection : IEnumerable<string> {
		private List<KeyValuePair<string, string>> pairs;

		public NameValueCollection() {
			this.pairs = new List<KeyValuePair<string, string>>();
		}

		public NameValueCollection(int capacity) {
			this.pairs = new List<KeyValuePair<string, string>>(capacity);
		}

		public int Count {
			get { return this.pairs.Count; }
		}

		public string this[string key] {
			get { return string.Join(",", this.GetValues(key)); }
		}

		public void Add(string name, string value) {
			this.pairs.Add(new KeyValuePair<string, string>(name, value));
		}

		public void Add(NameValueCollection collection) {
			Requires.NotNull(collection, "collection");

			foreach (string key in collection) {
				foreach (string value in collection.GetValues(key)) {
					this.Add(key, value);
				}
			}
		}

		public IEnumerable<string> GetValues(string key) {
			foreach (var pair in this.pairs) {
				if (pair.Key == key) {
					yield return pair.Value;
				}
			}
		}

		public void Remove(string key) {
			for (int i = this.Count - 1; i >= 0; i--) {
				if (this.pairs[i].Key == key) {
					this.pairs.RemoveAt(i);
				}
			}
		}

		public IEnumerator<string> GetEnumerator() {
			return this.pairs.Select(p => p.Key).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}
	}
}
