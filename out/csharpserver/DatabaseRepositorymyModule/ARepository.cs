
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

	internal static class RegisterA
	{
		public static void Register(IObjectFactory factory)
		{
			factory.RegisterType(typeof(DatabaseRepositorymyModule.ARepository));
			factory.RegisterType(typeof(DatabaseRepositorymyModule.ARepository), typeof(IQueryableRepository<myModule.A>), InstanceScope.Context);
			factory.RegisterFunc<IQueryable<myModule.A>>(f => f.Resolve<IQueryableRepository<myModule.A>>().Query<myModule.A>(null));

			factory.RegisterType(typeof(DatabaseRepositorymyModule.ARepository), typeof(IRepository<myModule.A>), InstanceScope.Context);
			factory.RegisterFunc<Func<string, myModule.A>>(f => f.Resolve<IRepository<myModule.A>>().Find);
			factory.RegisterFunc<Func<IEnumerable<string>, myModule.A[]>>(f => f.Resolve<IRepository<myModule.A>>().Find);
			factory.RegisterType(typeof(DatabaseRepositorymyModule.ARepository), typeof(IPersistableRepository<myModule.A>), InstanceScope.Context);
			factory.RegisterType(typeof(DatabaseRepositorymyModule.ARepository), typeof(IPersistableRepository<myModule.A>), InstanceScope.Context);
			factory.RegisterType(typeof(DatabaseRepositorymyModule.ARepository), typeof(IAggregateRootRepository<myModule.A>), InstanceScope.Context);

		}
	}

	internal partial class ARepository : IQueryableRepository<myModule.A>, System.IDisposable, IRepository<myModule.A>, IPersistableRepository<myModule.A>, IAggregateRootRepository<myModule.A>
	{

		private readonly IServiceLocator Locator;
		private readonly IDatabaseQuery DatabaseQuery;

		public ARepository(IServiceLocator locator, IDatabaseQuery query, IEagerNotification Notifications)

		{

			this.Locator = locator;
			this.DatabaseQuery = query;

			this.Notifications = Notifications;
			DataCache = new WeakCache<myModule.A>(this);
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

		public IQueryable<myModule.A> Query<TCondition>(ISpecification<TCondition> specification)
		{
			if(specification != null && specification.IsSatisfied == null)
				throw new ArgumentException("Search predicate is not specified");
			if(specification != null && !typeof(TCondition).IsAssignableFrom(typeof(myModule.A)))
				throw new ArgumentException("Specification is not compatible");

			IQueryable<myModule.A> data = new Queryable<myModule.A>(new QueryExecutor(DatabaseQuery, Locator));
			bool rewritten = false;

			if(!rewritten && specification != null)
			{
				var specAsNative = specification as ISpecification<myModule.A>;
				if(specAsNative != null)
					data = data.Where(specAsNative.IsSatisfied);
				else
					data = data.Cast<TCondition>().Where(specification.IsSatisfied).Cast<myModule.A>();
			}

			return data;
		}

		public myModule.A[] Search<TCondition>(ISpecification<TCondition> specification, int? limit, int? offset)
		{
			if(specification != null && specification.IsSatisfied == null)
				throw new ArgumentException("Search predicate is not specified");

			bool rewritten = false;
			var result = new List<myModule.A>();

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

		public myModule.A[] Find(IEnumerable<string> uris)
		{
			if(uris == null || !uris.Any(it => it != null))
				return new myModule.A[0];

			var pks = uris.Where(it => it != null).ToList();
			var formattedUris = NGS.DatabasePersistence.Postgres.PostgresRecordConverter.BuildSimpleUriList(pks);
			var result = new List<myModule.A>(pks.Count);
			DatabaseQuery.Execute(
				@"SELECT r FROM ""myModule"".""A_entity"" r WHERE r.""ID"" IN (" + formattedUris + ")",
				dr =>
				{
					var _pg = dr.GetValue(0);
					using(var _tr = _pg as System.IO.TextReader ?? new System.IO.StringReader(_pg as string))
						result.Add(_DatabaseCommon.FactorymyModule_A.AConverter.CreateFromRecord(_tr, 0, Locator));
				});

			return result.ToArray();
		}

		private readonly WeakCache<myModule.A> DataCache;
		private readonly System.Collections.Concurrent.ConcurrentQueue<NotifyInfo> NotifyQueue = new System.Collections.Concurrent.ConcurrentQueue<NotifyInfo>();
		private readonly IEagerNotification Notifications;

		public string[] Persist(IEnumerable<myModule.A> insert, IEnumerable<KeyValuePair<myModule.A, myModule.A>> update, IEnumerable<myModule.A> delete)
		{
			var insertedData = insert != null ? insert.ToArray() : new myModule.A[0];
			var updatedData = update != null ? update.ToList() : new List<KeyValuePair<myModule.A, myModule.A>>();
			var deletedData = delete != null ? delete.ToArray() : new myModule.A[0];

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
				myModule.A _val;
				for(int i = 0; i < updatedData.Count; i++)
				{
					_val = updatedData[i].Value;
					updatedData[i] = new KeyValuePair<myModule.A, myModule.A>(oldValues[_val.URI], _val);
				}
			}

			updatedData.RemoveAll(kv => kv.Key.Equals(kv.Value));

			DatabaseQuery.Fill<myModule.A, int>(insertedData, @"nextval('""myModule"".""A_ID_seq""'::regclass)::int", (it, seq) => it.ID = seq);

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

				it.URI = _DatabaseCommon.FactorymyModule_A.AConverter.BuildURI(it.ID);
			}
			foreach(var kv in updatedData)
			{

				kv.Value.URI = _DatabaseCommon.FactorymyModule_A.AConverter.BuildURI(kv.Value.ID);
			}

			_InternalDoPersist(insertedData, updatedData, deletedData);
			var resultURI = new string[insertedData.Length];
			for(int i = 0; i < resultURI.Length; i++)
				resultURI[i] = insertedData[i].URI;

			if (DatabaseQuery.InTransaction)
			{
				if (insertedData.Length > 0) NotifyQueue.Enqueue(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Insert, resultURI));
				if (updatedData.Count > 0)
				{
					NotifyQueue.Enqueue(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Update, updatedData.Select(it => it.Key.URI).ToArray()));
					if (updatedData.Any(kv => kv.Key.URI != kv.Value.URI)) NotifyQueue.Enqueue(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Change, updatedData.Where(kv => kv.Key.URI != kv.Value.URI).Select(it => it.Value.URI).ToArray()));
				}
				if (deletedData.Length > 0) NotifyQueue.Enqueue(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Delete, deletedData.Select(it => it.URI).ToArray()));
				DataCache.Invalidate(updatedData.Select(it => it.Key.URI).Union(deletedData.Select(it => it.URI)));
			}
			else
			{
				if (insertedData.Length > 0) Notifications.Notify(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Insert, resultURI));
				if (updatedData.Count > 0)
				{
					Notifications.Notify(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Update, updatedData.Select(it => it.Key.URI).ToArray()));
					if (updatedData.Any(kv => kv.Key.URI != kv.Value.URI)) Notifications.Notify(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Change, updatedData.Where(kv => kv.Key.URI != kv.Value.URI).Select(it => it.Value.URI).ToArray()));
				}
				if (deletedData.Length > 0) Notifications.Notify(new NotifyInfo("myModule.A", NotifyInfo.OperationEnum.Delete, deletedData.Select(it => it.URI).ToArray()));
			}

			return resultURI;
		}

		private readonly IPostgresTypeConverter TypeConverter = new _DatabaseCommon.FactorymyModule_A.AConverter();

		private void _InternalDoPersist(myModule.A[] insertedData, List<KeyValuePair<myModule.A, myModule.A>> updatedData, myModule.A[] deletedData)
		{
			using(var cms = ChunkedMemoryStream.Create())
			{
				var sw = cms.GetWriter();
				sw.Write("SELECT \"myModule\".\"persist_A\"(");
				PostgresTypedArray.ToArray(sw, insertedData, _DatabaseCommon.FactorymyModule_A.AConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""A_entity""[],");
				PostgresTypedArray.ToArray(sw, updatedData.Select(it => it.Key), _DatabaseCommon.FactorymyModule_A.AConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""A_entity""[],");
				PostgresTypedArray.ToArray(sw, updatedData.Select(it => it.Value), _DatabaseCommon.FactorymyModule_A.AConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""A_entity""[],");
				PostgresTypedArray.ToArray(sw, deletedData, _DatabaseCommon.FactorymyModule_A.AConverter.CreateTupleFrom);
				sw.Write(@"::""myModule"".""A_entity""[]");

				sw.Write(")");

				sw.Flush();
				cms.Position = 0;
				var com = new Npgsql.NpgsqlCommand(cms);
				DatabaseQuery.Execute(com);

			}
		}

		myModule.A[] IAggregateRootRepository<myModule.A>.Create(int count, Action<myModule.A[]> initialize)
		{
			if(count < 0)
				throw new ArgumentException("count must be positive: Provided value " + count);
			var roots = new myModule.A[count];
			for(int i = 0; i < count; i++)
				roots[i] = new myModule.A();
			if(initialize != null)
				initialize(roots);
			Persist(roots, null, null);
			return roots;
		}

		myModule.A[] IAggregateRootRepository<myModule.A>.Update(string[] uris, Action<myModule.A[]> change)
		{
			var roots = Find(uris);
			if(roots.Length != uris.Length)
				throw new ArgumentException("Can't find myModule.A with uri: ".With(string.Join(", ", uris)));
			if(change != null)
			{
				var originals = roots.Select(it => it.Clone()).ToDictionary(it => it.URI);
				change(roots);
				Persist(null, roots.Select(it => new KeyValuePair<myModule.A, myModule.A>(originals[it.URI], it)).ToList(), null);
			}
			return roots;
		}

		void IAggregateRootRepository<myModule.A>.Delete(string[] uris)
		{
			var roots = Find(uris);
			if(roots.Length != uris.Length)
				throw new ArgumentException("Can't find myModule.A with uri: ".With(string.Join(", ", uris)));
			Persist(null, null, roots);
		}

		IQueryable<myModule.A> IQueryableRepository<myModule.A>.Query<TCondition>(ISpecification<TCondition> specification)
		{
			return Query(specification);
		}

		myModule.A[] IQueryableRepository<myModule.A>.Search<TCondition>(ISpecification<TCondition> specification, int? limit, int? offset)
		{
			return Search(specification, limit, offset);
		}

		myModule.A[] IRepository<myModule.A>.Find(IEnumerable<string> uris) { return Find(uris); }
	}

}
