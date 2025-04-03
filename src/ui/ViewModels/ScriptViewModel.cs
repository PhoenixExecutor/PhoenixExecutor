using System.Windows.Input;

namespace PhoenixExecutor.ViewModels
{
    public class ScriptViewModel : ViewModelBase
    {
        private string _scriptCode;
        public string ScriptCode
        {
            get => _scriptCode;
            set => SetProperty(ref _scriptCode, value);
        }

        public ICommand RunScriptCommand { get; }

        public ScriptViewModel()
        {
            RunScriptCommand = new RelayCommand(RunScript);
        }

        private void RunScript()
        {
            if (string.IsNullOrEmpty(ScriptCode)) return;
            
            // Здесь можно добавить вызов ScriptManager.ExecuteRawCode(ScriptCode)
            // и вывод результата в консоль.
        }
    }
}