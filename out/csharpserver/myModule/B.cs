
namespace myModule
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading;
	using System.Runtime.Serialization;
	using NGS;
	using NGS.DomainPatterns;
	using NGS.Extensibility;

	[Serializable]
	[DataContract(Namespace="")] public partial class B : System.ICloneable, NGS.DomainPatterns.IEntity, IEquatable<B>, NGS.DomainPatterns.IAggregateRoot, NGS.DomainPatterns.ICacheable, IChangeTracking<B>, NGS.Serialization.IJsonObject
	{

		public override string ToString()
		{

			return base.ToString();
		}

		[System.Runtime.Serialization.OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{

			var locator = context.Context as IServiceLocator;
			if (locator == null) return;
			__DataCachea = new Lazy<IDataCache<myModule.A>>(() => locator.Resolve<IDataCache<myModule.A>>());
			if(aURI == null) throw new ArgumentException("In entity myModule.B, property a can't be null. aURI provided as null");
		}

		internal long _InternalGetSizeApproximation()
		{
			long size = 15;
			return size;
		}

		internal void __InternalPrepareInsert() {}
		internal void __InternalPrepareUpdate() {}
		internal void __InternalPrepareDelete() {}
		object ICloneable.Clone() { return Clone(); }

		internal void __ReapplyReferences()
		{
			if (_a != null && _a.URI != aURI) this.a = _a;
		}
		[DataMember] public string URI { get; internal set; }

		public B()

		{

			this.URI = Guid.NewGuid().ToString();
		}

		bool IEquatable<IEntity>.Equals(IEntity obj)
		{
			var other = obj as B;
			return other != null

				&& other.ID == this.ID
			;
		}

		public override int GetHashCode()
		{
			return this.URI != null ? this.URI.GetHashCode() : base.GetHashCode();
		}

		public B Clone()
		{
			var item = new B
			{
				URI = this.URI,

				ID = this.ID,
				_aURI = this._aURI, _a = this._a,
				aID = this.aID,
			};

			return item;
		}
		//TODO let's leave it out for now
		//public override bool Equals(object other) { return Equals(other as B); }
		public bool Equals(B other)
		{
			return (this as IEquatable<IEntity>).Equals(other)
				&& other.URI == this.URI

				&& other.ID == this.ID
				&& this._aURI == other._aURI
				&& other.aID == this.aID
			;
		}

		[DataMember(EmitDefaultValue=false,Name="ID")]
		internal int _ID  = NumberCounter<B>.GetNextTempInt();

		public int ID
		{

			get
			{

				return this._ID;
			}
			internal
			set
			{

				this._ID = value;

			}
		}

		Dictionary<System.Type, IEnumerable<string>> ICacheable.GetRelationships()
		{

			var result = new List<KeyValuePair<System.Type, string>>();

			result.AddRange(
				from it in new [] { this }
				let it_a = it.aURI
				where it_a != null
				select new KeyValuePair<System.Type, string>(typeof(myModule.A), it_a));
			return result.GroupBy(it => it.Key).ToDictionary(it => it.Key, it => it.Select(e => e.Value));

		}

		public static event Action<B> Validating = a => {};
		public void Validate()
		{
			Validating(this);
		}
		internal Lazy<IDataCache<myModule.A>> __DataCachea;

		internal string _aURI;
		[DataMember]
		public string aURI
		{
			get { return _aURI; }
			private set { _aURI = value; } //TODO: fakin serialization
		}

		internal myModule.A _a ;

		public myModule.A a
		{

			get
			{

				if (_a != null && _a.URI != _aURI || _a == null && _aURI != null)
					if(__DataCachea != null)
						_a = __DataCachea.Value.Find(_aURI);

				if (_aURI == null && _a != null)
					_a = null;
				return this._a;
			}

			set
			{

				if(value == null)
					throw new ArgumentNullException("Property a can't be null");
				this._a = value;

				_aURI = value != null ? value.URI : null;

				if(value == null)
					throw new ArgumentException("Property aID doesn't allow null values");
				else if(aID != value.ID)
					aID = value.ID;
			}
		}

		[DataMember(EmitDefaultValue=false,Name="aID")]
		internal int _aID ;

		public int aID
		{

			get
			{

				return this._aID;
			}
			internal
			set
			{

				this._aID = value;

			}
		}

		internal B __OriginalValue;
		B IChangeTracking<B>.GetOriginalValue()
		{
			return __OriginalValue;
		}

		void global::NGS.Serialization.IJsonObject.Serialize(System.IO.StreamWriter sw, Action<System.IO.StreamWriter, object> serializer)
		{

			sw.Write('{');
			__SerializeJsonObject(sw, new char[36], serializer, false);
			sw.Write('}');
		}

		internal void __SerializeJsonObject(System.IO.StreamWriter sw, char[] buffer, Action<System.IO.StreamWriter, object> serializer, bool hasWrittenProperty = false)
		{

			if(hasWrittenProperty) sw.Write(',');
			sw.Write("\"URI\":");
			global::NGS.Serialization.Json.Converters.StringConverter.Serialize(URI, sw);

				var __expID = this.ID;
				if (__expID != default(int))
				{
					sw.Write(",\"ID\":");
					global::NGS.Serialization.Json.Converters.NumberConverter.Serialize(__expID, sw, buffer);
				}

			if(aURI != null)
			{
				sw.Write(",\"aURI\":");
				global::NGS.Serialization.Json.Converters.StringConverter.Serialize(_aURI, sw);
			}

				var __expaID = this.aID;
				if (__expaID != default(int))
				{
					sw.Write(",\"aID\":");
					global::NGS.Serialization.Json.Converters.NumberConverter.Serialize(__expaID, sw, buffer);
				}
		}
	}

}
