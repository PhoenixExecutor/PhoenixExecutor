using PhoenixExecutor.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PhoenixExecutor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ScriptManager _scriptManager;
        
        public ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();
        public ObservableCollection<ScriptItem> Scripts { get; } = new ObservableCollection<ScriptItem>();
        
        private ScriptItem _selectedScript;
        public ScriptItem SelectedScript
        {
            get => _selectedScript;
            set => SetProperty(ref _selectedScript, value);
        }

        public ICommand ExecuteScriptCommand { get; }
        public ICommand LoadScriptCommand { get; }

        public MainViewModel()
        {
            _scriptManager = new ScriptManager();
            
            ExecuteScriptCommand = new RelayCommand(ExecuteScript);
            LoadScriptCommand = new RelayCommand(LoadScript);
            
            // Пример логов
            Logs.Add("> System initialized");
        }

        private void ExecuteScript()
        {
            if (SelectedScript == null) return;
            
            string result = _scriptManager.ExecuteScript(SelectedScript.Name);
            Logs.Add($"> Executed: {SelectedScript.Name}");
            Logs.Add(result);
        }

        private void LoadScript()
        {
            // Логика загрузки скрипта из файла
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string scriptName = Path.GetFileNameWithoutExtension(dialog.FileName);
                _scriptManager.LoadScript(scriptName, dialog.FileName);
                Scripts.Add(new ScriptItem { Name = scriptName });
                Logs.Add($"> Loaded: {scriptName}");
            }
        }
    }

    public class ScriptItem
    {
        public string Name { get; set; }
    }
}