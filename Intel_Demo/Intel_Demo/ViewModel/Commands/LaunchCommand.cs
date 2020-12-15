using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Intel_Demo.ViewModel.Commands
{
    class LaunchCommand : ICommand
    {
        public event EventHandler CanExecuteChanged {

            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public SimulatorVM VM { get; set; }

        public LaunchCommand(SimulatorVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.ShiftRocketFromInActiveToActive();
           // throw new NotImplementedException();
        }
    }
}
