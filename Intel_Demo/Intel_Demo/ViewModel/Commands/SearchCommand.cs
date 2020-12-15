using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Intel_Demo.ViewModel.Commands
{
    class SearchCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SimulatorVM VM { get; set; }

       public SearchCommand(SimulatorVM vm)
        {
            VM = vm;
        }

        public bool CanExecute (object parameter)
        {
            return true;
        }
        public void Execute (object parameter)
        {
            VM.getRocketData();
        }
    }
}
