using System;
using System.Windows.Input;

namespace Cobrowse.IO.WpfApp.ViewModel
{
  class RelayCommand: ICommand
  {
    private readonly Action executeAction;
    private readonly Action<object> executeActionEx;
    private readonly Func<bool> canExecuteFunc;
    private readonly Func<object, bool> canExecuteFuncEx;

    public RelayCommand(Action executeAction, Func<bool> canExecuteFunc = null)
    {
      this.executeAction = executeAction;
      this.canExecuteFunc = canExecuteFunc;
    }
    public RelayCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc = null)
    {
      executeActionEx = executeAction;
      canExecuteFuncEx = canExecuteFunc;
    }

    public void Update()
    {
      if (CanExecuteChanged != null)
        CanExecuteChanged(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public void Execute(object parameter)
    {
      if (executeAction != null)
        executeAction();
      else if (executeActionEx != null)
        executeActionEx(parameter);
      else
        throw new InvalidOperationException("RelayCommand can't execute without bound code.");
    }

    /// <inheritdoc />
    public bool CanExecute(object parameter)
    {
      if (canExecuteFunc != null)
        return canExecuteFunc();
      if (canExecuteFuncEx != null)
        return canExecuteFuncEx(parameter);
      return true;
    }

    public event EventHandler CanExecuteChanged;
  }
}
