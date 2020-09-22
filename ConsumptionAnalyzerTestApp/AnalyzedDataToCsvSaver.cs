using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsumptionAnalyzerTestApp
{
	internal class AnalyzedDataToCsvSaver
	{
		public bool SaveData(IReadOnlyList<AnalyzedLogRecord> records, string path)
		{
			try
			{
				var builder = new StringBuilder();
				foreach (var record in records)
				{
					builder.Append($"{record.TimeStamp:d}");
					builder.Append(GlobalConstants.CsvSeparator);
					builder.Append(record.TotalConsumedWaterMl);
					builder.Append(GlobalConstants.CsvSeparator);
					builder.Append(record.TimesDrunk);
					builder.Append(GlobalConstants.CsvSeparator);
					builder.Append(record.TimesBottleWasFilled);
					builder.Append(GlobalConstants.CsvSeparator);
					builder.Append(record.TimesCapWasOpen);
					builder.Append(GlobalConstants.CsvSeparator);
					builder.Append(record.TimesCorkWasOpen);
					builder.Append(GlobalConstants.CsvSeparator);
					builder.Append(record.TimesLevelErrorOccured);
					builder.Append(Environment.NewLine);
				}

				File.WriteAllText(path, builder.ToString());

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}