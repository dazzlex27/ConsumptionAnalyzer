using System;
using System.Collections.Generic;

namespace ConsumptionAnalyzerTestApp
{
	internal class ConsumptionLogAnalyzer
	{
		public IReadOnlyList<AnalyzedLogRecord> AnalyzeData(ConsumptionLog log)
		{
			var analyzedRecords = new List<AnalyzedLogRecord>();

			if (log.Records == null || log.Records.Count == 0)
				return analyzedRecords;

			var lastLevel = int.MinValue;
			var isCorkOpen = false;
			var isCapOpen = false;

			var lastTimeStamp = DateTime.MinValue;
			var currentTotalWaterConsumedMl = 0;
			var currentTimesDrunk = 0;
			var currentTimesBottleWasFilled = 0;
			var currentTimesCapWasOpen = 0;
			var currentTimesCorkWasOpen = 0;
			var currentTimesErrorOccured = 0;

			foreach (var record in log.Records)
			{
				if (record.TimeStamp == DateTime.MinValue)
					continue;

				if (lastTimeStamp == DateTime.MinValue)
					lastTimeStamp = record.TimeStamp;

				if (record.TimeStamp.Date != lastTimeStamp.Date)
				{
					var analyzedRecord = new AnalyzedLogRecord(lastTimeStamp, currentTotalWaterConsumedMl, currentTimesDrunk,
						currentTimesBottleWasFilled, currentTimesCapWasOpen, currentTimesCorkWasOpen, currentTimesErrorOccured);
					analyzedRecords.Add(analyzedRecord);

					lastTimeStamp = DateTime.MinValue;
					currentTotalWaterConsumedMl = 0;
					currentTimesDrunk = 0;
					currentTimesBottleWasFilled = 0;
					currentTimesCapWasOpen = 0;
					currentTimesCorkWasOpen = 0;
					currentTimesErrorOccured = 0;
				}

				switch (record.Command)
				{
					case CommandType.Level:
						if (record.LevelValueMl == -1)
							currentTimesErrorOccured++;
						else
						{
							var currentLevel = record.LevelValueMl;

							if (lastLevel != int.MinValue)
							{
								if (lastLevel < currentLevel)
									currentTimesBottleWasFilled++;
								if (lastLevel > currentLevel)
								{
									currentTimesDrunk++;
									currentTotalWaterConsumedMl += lastLevel - currentLevel;
								}
							}

							lastLevel = currentLevel;
						}
						break;
					case CommandType.CapOff:
						if (!isCapOpen)
							currentTimesCapWasOpen++;
						isCapOpen = true;
						break;
					case CommandType.CorkOff:
						if (!isCorkOpen)
							currentTimesCorkWasOpen++;
						isCorkOpen = true;
						break;
					case CommandType.CapOn:
						isCapOpen = false;
						break;
					case CommandType.CorkOn:
						isCorkOpen = false;
						break;
				}
			}

			if (lastTimeStamp != DateTime.MinValue)
			{
				var lastRecord = new AnalyzedLogRecord(lastTimeStamp, currentTotalWaterConsumedMl, currentTimesDrunk,
							currentTimesBottleWasFilled, currentTimesCapWasOpen, currentTimesCorkWasOpen, currentTimesErrorOccured);
				analyzedRecords.Add(lastRecord);
			}

			return analyzedRecords;
		}
	}
}