
namespace SystemBoot
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

	using NGS.DatabasePersistence.Postgres.Converters;

	using NGS.DatabasePersistence.Postgres;

	using NGS.DatabasePersistence;

	using System.Data;

	public class Configuration : ISystemAspect
	{
		private static bool IsInitialized;

		public void Initialize(IObjectFactory factory)
		{
			if(IsInitialized)
				return;
			IsInitialized = true;

			{
				var dbTranConf = factory.Resolve<NGS.DatabasePersistence.IDatabaseQueryManager>();
				var dbQuery = dbTranConf.CreateQuery();
				var columnsInfo = dbQuery.Fill(@"SELECT * FROM ""-NGS-"".Load_Type_Info()");
				columnsInfo.CaseSensitive = true;
				columnsInfo.PrimaryKey = new[] { columnsInfo.Columns[0], columnsInfo.Columns[1], columnsInfo.Columns[2] };
				_DatabaseConfiguration.DatabaseConverters.Initialize(columnsInfo);
				dbTranConf.EndQuery(dbQuery, false);
			}
			var postgresConverter = factory.Resolve<NGS.DatabasePersistence.Postgres.IPostgresConverterRepository>();
			postgresConverter.RegisterConverter(typeof(myModule.A), new _DatabaseCommon.FactorymyModule_A.AConverter());
			DatabaseRepositorymyModule.RegisterA.Register(factory);
		}
	}
}
