
namespace _DatabaseConfiguration
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

	internal partial class DatabaseConverters
	{

		internal static void Initialize(System.Data.DataTable columnsInfo)
		{

			System.Data.DataRow row = null;
			_DatabaseCommon.FactorymyModule_A.AConverter.InitializeProperties(columnsInfo);
			_DatabaseCommon.FactorymyModule_B.BConverter.InitializeProperties(columnsInfo);
		}
	}

}
