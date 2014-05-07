
namespace _DatabaseCommon.FactorymyModule_A
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

	using System.Globalization;
	using System.IO;
	using NGS.DatabasePersistence;
	using NGS.DatabasePersistence.Postgres;
	using NGS.DatabasePersistence.Postgres.Converters;
	using NGS.DomainPatterns;
	using NGS.Utility;

	internal class AConverter : IPostgresTypeConverter
	{
		public object CreateInstance(object value, IServiceLocator locator)
		{
			if (value == null)
				return null;
			using(var sr = value as System.IO.TextReader ?? new System.IO.StringReader(value as string ?? value.ToString()))
			{
				return CreateFromRecord(sr, 0, locator);
			}
		}

		public PostgresTuple ToTuple(object instance)
		{
			return CreateTupleFrom(instance as myModule.A);
		}

		public static RecordTuple CreateExtendedTupleFrom(myModule.A item)
		{
			if(item == null)
				return null;

			var items = new PostgresTuple[ExtendedColumnCount];

			items[ExtendedProperty_URI_Index] = new ValueTuple(item.URI);
			items[ExtendedProperty_ID_Index] = _DatabaseCommon.Utility.IntegerToTuple(item.ID);
			return new RecordTuple(items);
		}

		public static RecordTuple CreateTupleFrom(myModule.A item)
		{
			if(item == null)
				return null;

			var items = new PostgresTuple[ColumnCount];

			items[Property_URI_Index] = new ValueTuple(item.URI);
			items[Property_ID_Index] = _DatabaseCommon.Utility.IntegerToTuple(item.ID);
			return new RecordTuple(items);
		}

		private static int ColumnCount;
		private static int ExtendedColumnCount;

		internal static void InitializeProperties(System.Data.DataTable columnsInfo)
		{
			System.Data.DataRow row = null;

			ColumnCount = columnsInfo.Select("type_schema = 'myModule' AND type_name = 'A_entity'").Length;
			ExtendedColumnCount = columnsInfo.Select("type_schema = 'myModule' AND type_name = '-ngs_A_type-'").Length;

			ReaderConfiguration = new Action<myModule.A, TextReader, int, IServiceLocator>[ColumnCount > 0 ? ColumnCount : 1];
			ReaderExtendedConfiguration = new Action<myModule.A, TextReader, int, IServiceLocator>[ExtendedColumnCount > 0 ? ExtendedColumnCount : 1];
			for(int i = 0;i < ColumnCount; i++)
				ReaderConfiguration[i] = (agg, tr, c, sl) => StringConverter.Skip(tr, c);
			if(ColumnCount != ReaderConfiguration.Length)
				ReaderConfiguration[0] = (agg, tr, c, sl) => tr.Read();
			for(int i = 0;i < ExtendedColumnCount; i++)
				ReaderExtendedConfiguration[i] = (agg, tr, c, sl) => StringConverter.Skip(tr, c);
			if(ExtendedColumnCount != ReaderExtendedConfiguration.Length)
				ReaderExtendedConfiguration[0] = (agg, tr, c, sl) => tr.Read();

			row = columnsInfo.Rows.Find(new[] { "myModule", "A_entity", "URI" });
			if(row == null)
				throw new System.Configuration.ConfigurationException("Couldn't find column URI in type myModule.A_entity. Check if database is out of sync with code");
			Property_URI_Index = (short)row["column_index"] - 1;

			row = columnsInfo.Rows.Find(new[] { "myModule", "-ngs_A_type-", "URI" });
			if(row == null)
				throw new System.Configuration.ConfigurationException("Couldn't find column URI in type myModule.A. Check if database is out of sync with code");
			ExtendedProperty_URI_Index = (short)row["column_index"] - 1;

			ReaderConfiguration[Property_URI_Index] = (item, reader, context, locator) => item.URI = StringConverter.Parse(reader, context);

			ReaderExtendedConfiguration[ExtendedProperty_URI_Index] = (item, reader, context, locator) => item.URI = StringConverter.Parse(reader, context);

			row = columnsInfo.Rows.Find(new[] { "myModule", "A_entity", "ID" });
			if(row == null)
				throw new System.Configuration.ConfigurationException("Couldn't find column ID in type myModule.A_entity. Check if database is out of sync with code");
			Property_ID_Index = (short)row["column_index"] - 1;

			ReaderConfiguration[Property_ID_Index] = (item, reader, context, locator) => item._ID = _DatabaseCommon.Utility.ParseInt(reader, context);

			row = columnsInfo.Rows.Find(new[] { "myModule", "-ngs_A_type-", "ID" });
			if(row == null)
				throw new System.Configuration.ConfigurationException("Couldn't find column ID in type myModule.A. Check if database is out of sync with code");
			ExtendedProperty_ID_Index = (short)row["column_index"] - 1;

			ReaderExtendedConfiguration[ExtendedProperty_ID_Index] = (item, reader, context, locator) => item._ID = _DatabaseCommon.Utility.ParseInt(reader, context);

		}

		private static Action<myModule.A, TextReader, int, IServiceLocator>[] ReaderConfiguration;
		private static Action<myModule.A, TextReader, int, IServiceLocator>[] ReaderExtendedConfiguration;

		public static myModule.A CreateFromRecord(TextReader reader, int context, IServiceLocator locator)
		{
			var cur = reader.Read();
			if (cur == ',' || cur == ')')
				return null;
			var result = CreateFromRecord(reader, context, context == 0 ? 1 : context << 1, locator);
			reader.Read();
			return result;
		}

		public static myModule.A CreateFromRecord(TextReader reader, int outerContext, int context, IServiceLocator locator)
		{
			for (int i = 0; i < outerContext; i++)
				reader.Read();
			var item = new myModule.A();
			foreach (var config in ReaderConfiguration)
				config(item, reader, context, locator);
			for (int i = 0; i < outerContext; i++)
				reader.Read();

			item.__OriginalValue = item.Clone();
			return item;
		}

		public static myModule.A CreateFromExtendedRecord(TextReader reader, int context, IServiceLocator locator)
		{
			var cur = reader.Read();
			if (cur == ',' || cur == ')')
				return null;
			var result = CreateFromExtendedRecord(reader, context, context == 0 ? 1 : context << 1, locator);
			reader.Read();
			return result;
		}

		public static myModule.A CreateFromExtendedRecord(TextReader reader, int outerContext, int context, IServiceLocator locator)
		{
			for (int i = 0; i < outerContext; i++)
				reader.Read();
			var item = new myModule.A();
			foreach (var config in ReaderExtendedConfiguration)
				config(item, reader, context, locator);
			for (int i = 0; i < outerContext; i++)
				reader.Read();

			item.__OriginalValue = item.Clone();
			return item;
		}

		private static int Property_URI_Index;
		private static int ExtendedProperty_URI_Index;
		internal static List<int> OptionalEntityReferences = new List<int>();
		private static int Property_ID_Index;
		private static int ExtendedProperty_ID_Index;

		internal static string BuildURI(int ID)
		{
			return _DatabaseCommon.Utility.IntegerToString(ID);
		}

		internal static void ParseURI(IServiceLocator locator, string URI, out int ID)
		{

			ID = _DatabaseCommon.Utility.ToInt(URI);
		}
	}
}
