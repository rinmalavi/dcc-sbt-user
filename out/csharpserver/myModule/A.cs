
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
	[DataContract(Namespace="")] public partial class A : System.ICloneable, NGS.DomainPatterns.IEntity, IEquatable<A>, NGS.DomainPatterns.IAggregateRoot, NGS.DomainPatterns.ICacheable, IChangeTracking<A>, NGS.Serialization.IJsonObject
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
		}

		internal long _InternalGetSizeApproximation()
		{
			long size = 5;
			return size;
		}

		internal void __InternalPrepareInsert() {}
		internal void __InternalPrepareUpdate() {}
		internal void __InternalPrepareDelete() {}
		object ICloneable.Clone() { return Clone(); }

		internal void __ReapplyReferences() {}
		[DataMember] public string URI { get; internal set; }

		public A()

		{

			this.URI = Guid.NewGuid().ToString();
		}

		bool IEquatable<IEntity>.Equals(IEntity obj)
		{
			var other = obj as A;
			return other != null

				&& other.ID == this.ID
			;
		}

		public override int GetHashCode()
		{
			return this.URI != null ? this.URI.GetHashCode() : base.GetHashCode();
		}

		public A Clone()
		{
			var item = new A
			{
				URI = this.URI,

				ID = this.ID,
			};

			return item;
		}
		//TODO let's leave it out for now
		//public override bool Equals(object other) { return Equals(other as A); }
		public bool Equals(A other)
		{
			return (this as IEquatable<IEntity>).Equals(other)
				&& other.URI == this.URI

				&& other.ID == this.ID
			;
		}

		[DataMember(EmitDefaultValue=false,Name="ID")]
		internal int _ID  = NumberCounter<A>.GetNextTempInt();

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

			return result.GroupBy(it => it.Key).ToDictionary(it => it.Key, it => it.Select(e => e.Value));

		}

		public static event Action<A> Validating = a => {};
		public void Validate()
		{
			Validating(this);
		}

		internal A __OriginalValue;
		A IChangeTracking<A>.GetOriginalValue()
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
		}
	}

}
