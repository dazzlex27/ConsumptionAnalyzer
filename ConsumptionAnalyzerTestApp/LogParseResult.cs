namespace ConsumptionAnalyzerTestApp
{
	internal class LogParseResult
	{
		public ConsumptionLog Log { get; }

		public int ValidEntriesCount { get; }

		public int InvalidEntriesCount { get; }

		public int EmptyEntriesCount { get; }

		public LogParseResult(ConsumptionLog log, int validEntriesCount, int invalidEntriesCount, int emptyEntriesCount)
		{
			Log = log;
			ValidEntriesCount = validEntriesCount;
			InvalidEntriesCount = invalidEntriesCount;
			EmptyEntriesCount = emptyEntriesCount;
		}
	}
}