using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

using Cobrowse.IO.WpfApp.UI;

namespace Cobrowse.IO.WpfApp.ViewModel
{
  class SessionViewModel : INotifyPropertyChanged, IDisposable
  {
    private UIState state;

    public SessionViewModel(Window ownerWindow)
    {
      CommandSessionStep = new RelayCommand(CommandSessionStep_Execute);
      CommandAcceptRemoteControl = new RelayCommand(CommandAcceptRemoteControl_Execute);
      CommandRejectRemoteControl = new RelayCommand(CommandRejectRemoteControl_Execute);

      Window = new SessionWindow()
      {
        DataContext = this,
        Owner = ownerWindow
      };
    }

    public async Task Initialize()
    {
      try
      {
        await CobrowseIO.Instance.Start();

        CobrowseIO.Instance.SessionEnded += OnSessionEnded;
        CobrowseIO.Instance.SessionAuthorizing += OnSessionAuthorizing;
        CobrowseIO.Instance.SessionUpdated += OnSessionUpdated;
        CobrowseIO.Instance.SessionRemoteControlRequested += OnSessionRemoteControlRequested;

        await CobrowseIO.Instance.CreateSession();

        State = UIState.Pending;
      }
      catch (Exception e)
      {
        MessageBox.Show(Window, $"[{e.GetType().Name}]:\n{e.Message}\n{e.StackTrace}", "Unable to Start", MessageBoxButton.OK, MessageBoxImage.Error);
        Window.Close();
      }
    }

    private void OnSessionUpdated(Session s)
    {
      if (s.State == SessionState.Active)
        State = UIState.Active;
    }

    private void OnSessionAuthorizing(Session s)
    {
      State = UIState.Authorizing;
    }

    private void OnSessionEnded(Session s)
    {
      State = UIState.Closed;
    }

    private void OnSessionRemoteControlRequested(Session s)
    {
      State = UIState.RemoteControlRequested;
    }

    public void Close()
    {
      State = UIState.Closed;
    }

    public UIState State
    {
      get { return state; }
      private set
      {
        if (state == value)
          return;

        state = value;
        OnPropertyChanged();

        OnPropertyChanged(nameof(IsProgressVisible));
        OnPropertyChanged(nameof(IsCodeVisible));
        OnPropertyChanged(nameof(IsButtonVisible));
        OnPropertyChanged(nameof(Message));
        OnPropertyChanged(nameof(ButtonText));
        OnPropertyChanged(nameof(AreRemoteControlButtonsVisible));

        if (state == UIState.Closed)
        {
          Application.Current.Dispatcher.Invoke(Window.Close);
          Task.Run(CobrowseIO.Instance.Stop);
        }
      }
    }

    public bool IsProgressVisible
    {
      get { return state == UIState.Connecting; }
    }

    public bool IsCodeVisible
    {
      get { return state == UIState.Pending; }
    }

    public bool IsButtonVisible
    {
      get { return state >= UIState.Authorizing && state != UIState.Closed && state != UIState.RemoteControlRequested; }
    }

    public bool AreRemoteControlButtonsVisible
    {
      get { return state == UIState.RemoteControlRequested; }
    }

    public void Start()
    {
      Window.ShowDialog();
    }

    public SessionWindow Window { get; }

    public string ButtonText
    {
      get
      {
        switch (state)
        {
          case UIState.Authorizing:
            return "Authorize";

          case UIState.Active:
            return "End Session";

          default:
            return "n/a";
        }
      }
    }

    public string Message
    {
      get
      {
        switch (state)
        {
          case UIState.Connecting:
            return "Please wait";

          case UIState.Pending:
            return "Screenshare support code";

          case UIState.Authorizing:
            return "Press a button to authorize screenshare";

          case UIState.RemoteControlRequested:
            return "Do you want to accept\nthe device remote control?";

          case UIState.Active:
            return "Screenshare is active";

          default:
            return "Session is closed";
        }
      }
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region Commands

    public RelayCommand CommandSessionStep { get; }
    public RelayCommand CommandAcceptRemoteControl { get; }
    public RelayCommand CommandRejectRemoteControl { get; }

    private async void CommandSessionStep_Execute()
    {
      switch (CobrowseIO.Instance.CurrentSession.State)
      {
        case SessionState.Authorizing:
          await CobrowseIO.Instance.CurrentSession.Activate();
          break;

        case SessionState.Active:
          await CobrowseIO.Instance.CurrentSession.End();
          break;
      }
    }

    private async void CommandAcceptRemoteControl_Execute()
    {
      await CobrowseIO.Instance.CurrentSession.AcceptRemoteControl();
    }

    private async void CommandRejectRemoteControl_Execute()
    {
      await CobrowseIO.Instance.CurrentSession.RejectRemoteControl();
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
      CobrowseIO.Instance.SessionEnded -= OnSessionEnded;
      CobrowseIO.Instance.SessionAuthorizing -= OnSessionAuthorizing;
      CobrowseIO.Instance.SessionUpdated -= OnSessionUpdated;
      CobrowseIO.Instance.SessionRemoteControlRequested -= OnSessionRemoteControlRequested;
    }

    #endregion

    public enum UIState
    {
      Connecting,
      Pending,
      Authorizing,
      Active,
      RemoteControlRequested,
      Closed,
    }
  }
}
