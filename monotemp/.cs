
namespace _DatabaseCommon
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	internal static partial class Utility
	{
		public static T[] ConvertToArray<T>(List<T> source)
		{
			return source != null ? source.ToArray() : null;
		}
		public static List<T> ConvertToList<T>(List<T> source)
		{
			return source;
		}
		public static HashSet<T> ConvertToSet<T>(List<T> source)
		{
			return source != null ? new HashSet<T>(source) : null;
		}
	}
}
namespace _DatabaseCommon
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using NGS.DatabasePersistence.Postgres;
	using NGS.DatabasePersistence.Postgres.Converters;

	internal static partial class Utility
	{
		public static int? ToNullableInt(this string value)
		{
			if(!string.IsNullOrWhiteSpace(value))
				return int.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
			return null;
		}

		public static string NullableIntegerToString(int? value)
		{
			if(value.HasValue)
				return value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
			return null;
		}

		public static ValueTuple NullableIntegerToTuple(int? value)
		{
			return value != null ? new ValueTuple(value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture), false, false) : null;
		}

		public static int? ParseNullableInt(TextReader reader, int context)
		{
			return IntConverter.ParseNullable(reader);
		}

		public static List<int?> ParseNullableListInt(TextReader reader, int context)
		{
			return IntConverter.ParseNullableCollection(reader, context);
		}
	}
}
namespace _DatabaseCommon
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using NGS.DatabasePersistence.Postgres;
	using NGS.DatabasePersistence.Postgres.Converters;

	internal static partial class Utility
	{
		public static int ToInt(this string value)
		{
			if(!string.IsNullOrWhiteSpace(value))
				return int.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
			return 0;
		}

		public static string IntegerToString(int value)
		{
			return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}

		public static ValueTuple IntegerToTuple(int value)
		{
			return new ValueTuple(value.ToString(System.Globalization.CultureInfo.InvariantCulture), false, false);
		}

		public static int ParseInt(TextReader reader, int context)
		{
			return IntConverter.Parse(reader);
		}

		public static List<int> ParseListInt(TextReader reader, int context)
		{
			return IntConverter.ParseCollection(reader, context);
		}
	}
}