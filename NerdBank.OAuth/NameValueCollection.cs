namespace NerdBank.OAuth {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Validation;

	/// <summary>
	/// A ordered collection of key-value pairs, where the keys are not necessarily unique.
	/// </summary>
	/// <remarks>
	/// An imitation of the .NET Framework's collection by the same name,
	/// which isn't available in the PCL profile.
	/// </remarks>
	public class NameValueCollection : IEnumerable<string> {
		/// <summary>
		/// The ordered list of key=value pairs.
		/// </summary>
		private List<KeyValuePair<string, string>> pairs;

		/// <summary>
		/// Initializes a new instance of the <see cref="NameValueCollection"/> class.
		/// </summary>
		public NameValueCollection() {
			this.pairs = new List<KeyValuePair<string, string>>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NameValueCollection"/> class.
		/// </summary>
		/// <param name="capacity">The anticipated required capacity.</param>
		public NameValueCollection(int capacity) {
			this.pairs = new List<KeyValuePair<string, string>>(capacity);
		}

		/// <summary>
		/// Gets the number of elements in this collection.
		/// </summary>
		public int Count {
			get { return this.pairs.Count; }
		}

		/// <summary>
		/// Gets the comma-delimited list of values with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A comma-delimited list of values.</returns>
		public string this[string key] {
			get { return string.Join(",", this.GetValues(key)); }
		}

		/// <summary>
		/// Adds a key=value pair.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public void Add(string name, string value) {
			this.pairs.Add(new KeyValuePair<string, string>(name, value));
		}

		/// <summary>
		/// Adds the specified collection of key=value pairs.
		/// </summary>
		/// <param name="collection">The collection.</param>
		public void Add(NameValueCollection collection) {
			Requires.NotNull(collection, "collection");

			foreach (string key in collection) {
				foreach (string value in collection.GetValues(key)) {
					this.Add(key, value);
				}
			}
		}

		/// <summary>
		/// Gets each of the values associated with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A sequence of values.</returns>
		public IEnumerable<string> GetValues(string key) {
			foreach (var pair in this.pairs) {
				if (pair.Key == key) {
					yield return pair.Value;
				}
			}
		}

		/// <summary>
		/// Removes all values associated with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Remove(string key) {
			for (int i = this.Count - 1; i >= 0; i--) {
				if (this.pairs[i].Key == key) {
					this.pairs.RemoveAt(i);
				}
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<string> GetEnumerator() {
			return this.pairs.Select(p => p.Key).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}
	}
}
