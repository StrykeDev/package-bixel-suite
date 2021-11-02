﻿using System;
using System.Windows.Input;

namespace Blaze.ViewModels
{
    class DelegateCommand : ICommand
    {
        private readonly Action _action;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public DelegateCommand(Action action)
        {
            _action = action;
        }
    }
}
