using System;

namespace ConsumptionAnalyzerTestApp
{
	internal class AnalyzedLogRecord
	{
		public DateTime TimeStamp { get; set; }

		public int TotalConsumedWaterMl { get; set; }

		public int TimesDrunk { get; set; }

		public int TimesBottleWasFilled { get; set; }

		public int TimesCapWasOpen { get; set; }

		public int TimesCorkWasOpen { get; set; }

		public int TimesLevelErrorOccured { get; set; }

		public AnalyzedLogRecord(DateTime timeStamp, int totalConsumedWaterMl, int timesDrunk, int timesBottleWasFilled,
			int timesCapWasOpen, int timesCorkWasOpen, int timesLevelErrorOccured)
		{
			TimeStamp = timeStamp;
			TotalConsumedWaterMl = totalConsumedWaterMl;
			TimesDrunk = timesDrunk;
			TimesBottleWasFilled = timesBottleWasFilled;
			TimesCapWasOpen = timesCapWasOpen;
			TimesCorkWasOpen = timesCorkWasOpen;
			TimesLevelErrorOccured = timesLevelErrorOccured;
		}
	}
}