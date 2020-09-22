using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ConsumptionAnalyzerTestApp
{
	internal class MainWindowVm : BaseViewModel
	{
		private ConsumptionLogParser _logParser;
		private ConsumptionLogAnalyzer _logAnalyzer;
		private ConsumptionLog _currentLog;
		private AnalyzedDataToCsvSaver _analyzedDataSaver;

		private string _inputFileName;
		private string _outputFileName;
		private ObservableCollection<AnalyzedLogRecord> _analyzedLogRecords;

		public ICommand ParseLogCommand { get; }

		public ICommand SaveResultCommand { get; }

		public ICommand CloseAppCommand { get; }

		public string InputFileName
		{
			get => _inputFileName;
			set => SetField(ref _inputFileName, value, nameof(InputFileName));
		}

		public string OutputFileName
		{
			get => _outputFileName;
			set => SetField(ref _outputFileName, value, nameof(OutputFileName));
		}

		public ObservableCollection<AnalyzedLogRecord> AnalyzedLogRecords
		{
			get => _analyzedLogRecords;
			set => SetField(ref _analyzedLogRecords, value, nameof(AnalyzedLogRecords));
		}

		public MainWindowVm()
		{
			InputFileName = "";
			OutputFileName = "";

			_logParser = new ConsumptionLogParser();
			_logAnalyzer = new ConsumptionLogAnalyzer();
			_analyzedDataSaver = new AnalyzedDataToCsvSaver();

			ParseLogCommand = new CommandHandler(LoadLogFile);
			SaveResultCommand = new CommandHandler(SaveDataToFile);
			CloseAppCommand = new CommandHandler(Application.Current.Shutdown);
		}

		private void LoadLogFile()
		{
			try
			{
				var dialog = new OpenFileDialog
				{
					Filter = "csv files|*.csv",
					DefaultExt = "csv",
					Multiselect = false,
					InitialDirectory = Assembly.GetEntryAssembly().Location
				};
				var result = dialog.ShowDialog();
				if (result != true)
					return;

				InputFileName = dialog.FileName;

				var logParseResult = _logParser.ParseLogFromCsv(InputFileName);
				if (logParseResult == null)
				{
					MessageBox.Show("Failed to read file");
					InputFileName = "";
					return;
				}

				_currentLog = logParseResult.Log;
				var linesSuccessfull = $"Успешно прочитанных записей: {logParseResult.ValidEntriesCount}";
				var linesUnsuccesfull = $"Некорректных строк: {logParseResult.InvalidEntriesCount}";
				var linesEmpty = $"Пустых строк: {logParseResult.EmptyEntriesCount}";

				MessageBox.Show($"{linesSuccessfull}{Environment.NewLine}{linesUnsuccesfull}{Environment.NewLine}{linesEmpty}");

				var analysisData = _logAnalyzer.AnalyzeData(_currentLog);
				UpdateDataGrid(analysisData);

				OutputFileName = "";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void SaveDataToFile()
		{
			try
			{
				if (_currentLog == null)
					return;

				var dialog = new SaveFileDialog
				{
					Filter = "csv files|*.csv",
					DefaultExt = "csv",
					InitialDirectory = Assembly.GetEntryAssembly().Location
				};
				var result = dialog.ShowDialog();
				if (result != true)
					return;

				OutputFileName = dialog.FileName;
				var saved = _analyzedDataSaver.SaveData(AnalyzedLogRecords, OutputFileName);
				if (saved)
					MessageBox.Show("Файл успешно сохранён");
				else
					MessageBox.Show("При сохранении файла возникла ошибка");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void UpdateDataGrid(IReadOnlyList<AnalyzedLogRecord> records)
		{
			AnalyzedLogRecords = new ObservableCollection<AnalyzedLogRecord>(records);
		}
	}
}