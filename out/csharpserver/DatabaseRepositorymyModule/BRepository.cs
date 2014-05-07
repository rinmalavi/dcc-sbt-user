
namespace DatabaseRepositorymyModule
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

	using NGS.Utility;
	using System.IO;

	using NGS.DatabasePersistence;
	using NGS.DatabasePersistence.Postgres;
	using NGS.DatabasePersistence.Postgres.Converters;
	using NGS.DatabasePersistence.Postgres.QueryGeneration;

	internal static class RegisterB
	{
		public static void Register(IObjectFactory factory)
		{
			factory.RegisterType(typeof(DatabaseRepositorymyModule.BRepository));
			factory.RegisterType(typeof(DatabaseRepositorymyModule.BRepository), typeof(IQueryableRepository<myModule.B>), InstanceScope.Context);
			factory.RegisterFunc<IQueryable<myModule.B>>(f => f.Resolve<IQueryableRepository<myModule.B>>().Query<myModule.B>(null));

			factory.RegisterType(typeof(DatabaseRepositorymyModule.BRepository), typeof(IRepository<myModule.B>), InstanceScope.Context);
			factory.RegisterFunc<Func<string, myModule.B>>(f => f.Resolve<IRepository<myModule.B>>().Find);
			factory.RegisterFunc<Func<IEnumerable<string>, myModule.B[]>>(f => f.Resolve<IRepository<myModule.B>>().Find);
			factory.RegisterType(typeof(DatabaseRepositorymyModule.BRepository), typeof(IPersistableRepository<myModule.B>), InstanceScope.Context);
			factory.RegisterType(typeof(DatabaseRepositorymyModule.BRepository), typeof(IPersistableRepository<myModule.B>), InstanceScope.Context);
			factory.RegisterType(typeof(DatabaseRepositorymyModule.BRepository), typeof(IAggregateRootRepository<myModule.B>), InstanceScope.Context);

		}
	}

	internal partial class BRepository : IQueryableRepository<myModule.B>, System.IDisposable, IRepository<myModule.B>, IPersistableRepository<myModule.B>, IAggregateRootRepository<myModule.B>
	{

		private readonly IServiceLocator Locator;
		private readonly IDatabaseQuery DatabaseQuery;

		public BRepository(IServiceLocator locator, IDatabaseQuery query, IEagerNotification Notifications)

		{

			this.Locator = locator;
			this.DatabaseQuery = query;

			this.Notifications = Notifications;
			DataCache = new WeakCache<myModule.B>(this);
		}

		public void Dispose()
		{

			if (!DatabaseQuery.InTransaction)
			{
				NotifyInfo ni;
				while (NotifyQueue.TryDequeue(out ni))
					Notifications.Notify(ni);
			}
		}

		public IQueryable<myModule.B> Query<TCondition>(ISpecification<TCondition> specification)
		{
			if(specification != null && specification.IsSatisfied == null)
				throw new ArgumentException("Search predicate is not specified");
			if(specification != null && !typeof(TCondition).IsAssignableFrom(typeof(myModule.B)))
				throw new ArgumentException("Specification is not compatible");

			IQueryable<myModule.B> data = new Queryable<myModule.B>(new QueryExecutor(DatabaseQuery, Locator));
			bool rewritten = false;

			if(!rewritten && specification != null)
			{
				var specAsNative = specification as ISpecification<myModule.B>;
				if(specAsNative != null)
					data = data.Where(specAsNative.IsSatisfied);
				else
					data = data.Cast<TCondition>().Where(specification.IsSatisfied).Cast<myModule.B>();
			}

			return data;
		}

		public myModule.B[] Search<TCondition>(ISpecification<TCondition> specification, int? limit, int? offset)
		{
			if(specification != null && specification.IsSatisfied == null)
				throw new ArgumentException("Search predicate is not specified");

			bool rewritten = false;
			var result = new List<myModule.B>();

			if(!rewritten)
			{
				var query = Query(specification);
				if (limit != null && limit.Value >= 0)
					query = query.Take(limit.Value);
				if (offset != null && offset.Value >= 0)
					query = query.Skip(offset.Value);
				result.AddRange(query);
			}

			return result.ToArray();
		}

		public myModule.B[] Find(IEnumerable<string> uris)
		{
			if(uris == null || !uris.Any(it => it != null))
				return new myModule.B[0];

			var pks = uris.Where(it => it != null).ToList();
			var formattedUris = NGS.DatabasePersistence.Postgres.PostgresRecordConverter.BuildSimpleUriList(pks);
			var result = new List<myModule.B>(pks.Count);
			DatabaseQuery.Execute(
				@"SELECT r FROM ""myModule"".""B_entity"" r WHERE r.""ID"" IN (" + formattedUris + ")",
				dr =>
				{
					var _pg = dr.GetValue(0);
					using(var _tr = _pg as System.IO.TextReader ?? new System.IO.StringReader(_pg as string))
						result.Add(_DatabaseCommon.FactorymyModule_B.BConverter.CreateFromRecord(_tr, 0, Locator));
				});

			return result.ToArray();
		}

		private readonly WeakCache<myModule.B> DataCache;
		private readonly System.Collections.Concurrent.ConcurrentQueue<NotifyInfo> NotifyQueue = new System.Collections.Concurrent.ConcurrentQueue<NotifyInfo>();
		private readonly IEagerNotification Notifications;

		public string[] Persist(IEnumerable<myModule.B> insert, IEnumerable<KeyValuePair<myModule.B, myModule.B>> update, IEnumerable<myModule.B> delete)
		{
			var insertedData = insert != null ? insert.ToArray() : new myModule.B[0];
			var updatedData = update != null ? update.ToList() : new List<KeyValuePair<myModule.B, myModule.B>>();
			var deletedData = delete != null ? delete.ToArray() : new myModule.B[0];

			if(insertedData.Length == 0 && updatedData.Count == 0 && deletedData.Length == 0)
				return new string[0];

			if (updatedData.Count > 0 && updatedData.Any(it => it.Key == null))
			{
				//TODO fetch only null values
				var oldValues = Find(updatedData.Select(it => it.Value.URI)).ToDictionary(it => it.URI);
				if (oldValues.Count != updatedData.Count)
					throw new ArgumentException("Can't find update value. Requested: {0}, found: {1}. Missing: {2}".With(
						updatedData.Count,
						oldValues.Count,
						string.Join(", ", updatedData.Select(it => it.Value.URI).Except(oldValues.Keys))));
				myModule.B _val;
				for(int i = 0; i < updatedData.Count; i++)
				{
					_val = updatedData[i].Value;
					updatedData[i] = new KeyValuePair<myModule.B, myModule.B>(oldValues[_val.URI], _val);
				}
			}

			updatedData.RemoveAll(kv => kv.Key.Equals(kv.Value));

			DatabaseQuery.Fill<myModule.B, int>(insertedData, @"nextval('""myModule"".""B_ID_seq""'::regclass)::int", (it, seq) => it.ID = seq);

			foreach(var it in insertedData)
				it.Validate();
			foreach(var it in updatedData)
				it.Value.Validate();
			foreach(var it in deletedData)
				it.Validate();
			for(int i = 0; i < insertedData.Length; i++)
				insertedData[i].__InternalPrepareInsert();
			foreach(var it in updatedData)
				it.Value.__InternalPrepareUpdate();
			for(int i = 0; i < deletedData.Length; i++)
				deletedData[i].__InternalPrepareDelete();

			for(int i = 0; i < insertedData.Length; i++)
			{
				var it = insertedData[i];
				it.__ReapplyReferences();
				it.URI = _DatabaseCommon.FactorymyModule_B.BConverter.BuildURI(it.ID);
				if(it.aID != null) it._aURI = _DatabaseCommon.FactorymyModule_A.AConverter.BuildURI(it.aID);
			}
			foreach(var kv in updatedData)
			{
				kv.Value.__ReapplyReferences();
				kv.Value.URI = _DatabaseCommon.FactorymyModule_B.BConverter.BuildURI(kv.Value.ID);
				if(kv.Value.aID != null) kv.Value._aURI = _DatabaseCommon.FactorymyModule_A.AConverter.BuildURI(kv.Value.aID);
			}

			_InternalDoPersist(insertedData, updatedData, deletedData);
			var resultURI = new string[insertedData.Length];
			for(int i = 0; i < resultURI.Length; i++)
				resultURI[i] = insertedData[i].URI;

			if (DatabaseQuery.InTransaction)
			{
				if (insertedData.Length > 0) NotifyQueue.Enqueue(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Insert, resultURI));
				if (updatedData.Count > 0)
				{
					NotifyQueue.Enqueue(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Update, updatedData.Select(it => it.Key.URI).ToArray()));
					if (updatedData.Any(kv => kv.Key.URI != kv.Value.URI)) NotifyQueue.Enqueue(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Change, updatedData.Where(kv => kv.Key.URI != kv.Value.URI).Select(it => it.Value.URI).ToArray()));
				}
				if (deletedData.Length > 0) NotifyQueue.Enqueue(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Delete, deletedData.Select(it => it.URI).ToArray()));
				DataCache.Invalidate(updatedData.Select(it => it.Key.URI).Union(deletedData.Select(it => it.URI)));
			}
			else
			{
				if (insertedData.Length > 0) Notifications.Notify(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Insert, resultURI));
				if (updatedData.Count > 0)
				{
					Notifications.Notify(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Update, updatedData.Select(it => it.Key.URI).ToArray()));
					if (updatedData.Any(kv => kv.Key.URI != kv.Value.URI)) Notifications.Notify(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Change, updatedData.Where(kv => kv.Key.URI != kv.Value.URI).Select(it => it.Value.URI).ToArray()));
				}
				if (deletedData.Length > 0) Notifications.Notify(new NotifyInfo("myModule.B", NotifyInfo.OperationEnum.Delete, deletedData.Select(it => it.URI).ToArray()));
			}

			return resultURI;
		}

		private readonly IPostgresTypeConverter TypeConverter = new _DatabaseCommon.FactorymyModule_B.BConverter();

		private void _InternalDoPersist(myModule.B[] insertedData, List<KeyValuePair<myModule.B, myModule.B>> updatedData, myModule.B[] deletedData)
		{
			using(var cms = ChunkedMemoryStream.Create())
			{
				var sw = cms.GetWriter();
				sw.Write("SELECT \"myModule\".\"persist_B\"(");
				PostgresTypedArray.ToArray(sw, insertedData, _DatabaseCommon.FactorymyModule_B.BConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""B_entity""[],");
				PostgresTypedArray.ToArray(sw, updatedData.Select(it => it.Key), _DatabaseCommon.FactorymyModule_B.BConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""B_entity""[],");
				PostgresTypedArray.ToArray(sw, updatedData.Select(it => it.Value), _DatabaseCommon.FactorymyModule_B.BConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""B_entity""[],");
				PostgresTypedArray.ToArray(sw, deletedData, _DatabaseCommon.FactorymyModule_B.BConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""B_entity""[]");

				sw.Write(")");

				sw.Flush();
				cms.Position = 0;
				var com = new Npgsql.NpgsqlCommand(cms);
				DatabaseQuery.Execute(com);

			}
		}

		myModule.B[] IAggregateRootRepository<myModule.B>.Create(int count, Action<myModule.B[]> initialize)
		{
			if(count < 0)
				throw new ArgumentException("count must be positive: Provided value " + count);
			var roots = new myModule.B[count];
			for(int i = 0; i < count; i++)
				roots[i] = new myModule.B();
			if(initialize != null)
				initialize(roots);
			Persist(roots, null, null);
			return roots;
		}

		myModule.B[] IAggregateRootRepository<myModule.B>.Update(string[] uris, Action<myModule.B[]> change)
		{
			var roots = Find(uris);
			if(roots.Length != uris.Length)
				throw new ArgumentException("Can't find myModule.B with uri: ".With(string.Join(", ", uris)));
			if(change != null)
			{
				var originals = roots.Select(it => it.Clone()).ToDictionary(it => it.URI);
				change(roots);
				Persist(null, roots.Select(it => new KeyValuePair<myModule.B, myModule.B>(originals[it.URI], it)).ToList(), null);
			}
			return roots;
		}

		void IAggregateRootRepository<myModule.B>.Delete(string[] uris)
		{
			var roots = Find(uris);
			if(roots.Length != uris.Length)
				throw new ArgumentException("Can't find myModule.B with uri: ".With(string.Join(", ", uris)));
			Persist(null, null, roots);
		}

		IQueryable<myModule.B> IQueryableRepository<myModule.B>.Query<TCondition>(ISpecification<TCondition> specification)
		{
			return Query(specification);
		}

		myModule.B[] IQueryableRepository<myModule.B>.Search<TCondition>(ISpecification<TCondition> specification, int? limit, int? offset)
		{
			return Search(specification, limit, offset);
		}

		myModule.B[] IRepository<myModule.B>.Find(IEnumerable<string> uris) { return Find(uris); }
	}

}
