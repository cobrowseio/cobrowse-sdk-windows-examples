﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

using Cobrowse.IO.Screenshare.UI;

namespace Cobrowse.IO.Screenshare.ViewModel
{
  partial class MainViewModel: INotifyPropertyChanged
  {
    private UIState state;

    public MainViewModel()
    {
      CobrowseIO.Instance.License = License;
      CobrowseIO.Instance.CustomData = CustomData;

      CommandSessionStep = new RelayCommand(CommandSessionStep_Execute);

      Window = new MainWindow()
      {
        DataContext = this
      };
    }

    public MainWindow Window { get; }

    public async Task Initialize()
    {
      try
      {
        await CobrowseIO.Instance.Start();

        CobrowseIO.Instance.SessionEnded += s => State = UIState.Closing;
        CobrowseIO.Instance.SessionAuthorizing += s => State = UIState.Authorizing;
        CobrowseIO.Instance.SessionUpdated += OnSessionUpdated;

        await CobrowseIO.Instance.CreateSession();

        State = UIState.Pending;
      }
      catch (Exception e)
      {
        MessageBox.Show(Window, $"[{e.GetType().Name}]:\n{e.Message}\n{e.StackTrace}", "Unable to Start", MessageBoxButton.OK, MessageBoxImage.Error);
        State = UIState.Closed;
      }
    }

    private void OnSessionUpdated(Session s)
    {
      if (s.State == SessionState.Active)
        State = UIState.Active;
    }

    public void Close()
    {
      if (state < UIState.Closing)
        State = UIState.Closing;
    }

    private async Task CloseCobrowse()
    {
      await CobrowseIO.Instance.Stop();
      State = UIState.Closed;
    }

    public bool CanClose
    {
      get { return State == UIState.Closed; }
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
        OnPropertyChanged(nameof(TaskbarItemProgressState));
        OnPropertyChanged(nameof(IsCodeVisible));
        OnPropertyChanged(nameof(IsButtonVisible));
        OnPropertyChanged(nameof(Message));
        OnPropertyChanged(nameof(ButtonText));
        OnPropertyChanged(nameof(IsWindowEnabled));

        if (state == UIState.Closing)
          Task.Run(CloseCobrowse);

        if (state == UIState.Closed)
          Application.Current.Dispatcher.Invoke(Window.Close);
      }
    }

    public TaskbarItemProgressState TaskbarItemProgressState
    {
      get { return state == UIState.Connecting ? TaskbarItemProgressState.Indeterminate : TaskbarItemProgressState.None; }
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
      get { return state >= UIState.Authorizing && state < UIState.Closing; }
    }

    public bool IsWindowEnabled
    {
      get { return state < UIState.Closing; }
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

    #region Commands

    public RelayCommand CommandSessionStep { get; }

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

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    public enum UIState
    {
      Connecting,
      Pending,
      Authorizing,
      Active,
      Closing,
      Closed,
    }
  }
}
