using System;

namespace ConsumptionAnalyzerTestApp
{
	internal class LogRecord
	{
		public DateTime TimeStamp { get; }

		public CommandType Command { get; }

		public int LevelValueMl { get; }

		public LogRecord(DateTime timeStamp, CommandType command, int levelValueMl)
		{
			TimeStamp = timeStamp;
			Command = command;
			LevelValueMl = levelValueMl;
		}
	}
}