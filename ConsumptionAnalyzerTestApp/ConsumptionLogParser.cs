
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsumptionAnalyzerTestApp
{
	internal class ConsumptionLogParser
	{
		public LogParseResult ParseLogFromCsv(string path)
		{
			if (!File.Exists(path))
				return null;

            try
            {
                var logRecords = new List<LogRecord>();

                var validEntriesCount = 0;
                var invalidEntriesCount = 0;
                var emptyEntriesCount = 0;

                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(new string[] { GlobalConstants.CsvSeparator }, StringSplitOptions.RemoveEmptyEntries);

                        if (values.Length == 0)
                        {
                            emptyEntriesCount++;
                            continue;
                        }

                        if (values[0].ToLower().Contains("restart"))
                        {
                            logRecords.Add(new LogRecord(DateTime.MinValue, CommandType.Restart, -1));
                            validEntriesCount++;
                            continue;
                        }

                        var timeStampParsed = DateTime.TryParse(values[0], out DateTime recordTimeStamp);
                        if (!timeStampParsed)
                        {
                            invalidEntriesCount++;
                            continue;
                        }

                        if (values.Length < 2)
                        {
                            invalidEntriesCount++;
                            continue;
                        }

                        var commandType = ParseCommand(values[1]);
                        if (commandType == CommandType.Unknown)
                        {
                            invalidEntriesCount++;
                            continue;
                        }

                        if (commandType != CommandType.Level)
                        {
                            logRecords.Add(new LogRecord(recordTimeStamp, commandType, -1));
                            validEntriesCount++;
                            continue;
                        }

                        if (values.Length < 3)
                        {
                            invalidEntriesCount++;
                            continue;
                        }

                        var levelParsed = int.TryParse(values[2], out int levelValue);
                        if (!levelParsed)
                        {
                            invalidEntriesCount++;
                            continue;
                        }

                        if (levelValue < -1)
                        {
                            invalidEntriesCount++;
                            continue;
                        }

                        logRecords.Add(new LogRecord(recordTimeStamp, commandType, levelValue));
                        validEntriesCount++;
                    }
                }

                var log = new ConsumptionLog(logRecords);

                return new LogParseResult(log, validEntriesCount, invalidEntriesCount, emptyEntriesCount);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private CommandType ParseCommand(string token)
        {
            switch (token)
            {
                case "U":
                    return CommandType.Level;
                case "P1":
                    return CommandType.CorkOff;
                case "P0":
                    return CommandType.CorkOn;
                case "K1":
                    return CommandType.CapOff;
                case "K0":
                    return CommandType.CapOn;
                case "A1":
                    return CommandType.AccValid;
                case "A0":
                    return CommandType.AccInvalid;
                default:
                    return CommandType.Unknown;
            }
        }
	}
}