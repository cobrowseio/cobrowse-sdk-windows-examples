using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

using Cobrowse.IO.WpfApp.UI;

namespace Cobrowse.IO.WpfApp.ViewModel
{
  class SessionViewModel: INotifyPropertyChanged
  {
    private Session session;
    private UIState state;

    public SessionViewModel(Window ownerWindow)
    {
      CommandSessionStep = new RelayCommand(CommandSessionStep_Execute);

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

        Session = await CobrowseIO.Instance.CreateSession();
        session.Started += s => State = UIState.Active;
        session.AuthorizationRequired += s => State = UIState.Authorizing;
        session.Ended += s => State = UIState.Closed;

        State = UIState.Pending;
      }
      catch (Exception e)
      {
        Window.Close();
        return;
      }
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
      get { return state >= UIState.Authorizing && state != UIState.Closed; }
    }

    public void Start()
    {
      Window.ShowDialog();
    }

    public SessionWindow Window { get; }

    public Session Session
    {
      get { return session; }
      private set
      {
        session = value;
        OnPropertyChanged();
      }
    }

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

    private async void CommandSessionStep_Execute()
    {
      switch (Session.State)
      {
        case SessionState.Pending:
          await Session.SetAuthorizing();
          break;

        case SessionState.Authorizing:
          await Session.Activate();
          break;

        case SessionState.Active:
          await Session.End();
          break;
      }
    }

    #endregion

    public enum UIState
    {
      Connecting,
      Pending,
      Authorizing,
      Active,
      Closed,
    }
  }
}
