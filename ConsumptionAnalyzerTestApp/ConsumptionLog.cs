using System.Collections.Generic;

namespace ConsumptionAnalyzerTestApp
{
	internal class ConsumptionLog
	{
		public List<LogRecord> Records { get; }

		public ConsumptionLog(List<LogRecord> records)
		{
			Records = records;
		}
	}
}