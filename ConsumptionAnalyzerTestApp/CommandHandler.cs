using System;
using System.Windows.Input;

namespace ConsumptionAnalyzerTestApp
{
	public class CommandHandler : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private readonly Action _action;

		public CommandHandler(Action action)
		{
			_action = action;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_action();
		}

		public void FireCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(null, null);
		}
	}
}