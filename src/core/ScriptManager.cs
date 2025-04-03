using System;
using System.Collections.Generic;
using System.IO;
using NLua;

namespace PhoenixExecutor.Core
{
    public class ScriptManager
    {
        private Lua _lua;
        private Dictionary<string, string> _scripts = new Dictionary<string, string>();

        public ScriptManager()
        {
            _lua = new Lua();
            InitializeLuaEnvironment();
        }

        /// <summary>
        /// Инициализация Lua-окружения (базовые функции, библиотеки и т.д.)
        /// </summary>
        private void InitializeLuaEnvironment()
        {
            // Загрузка стандартных библиотек Lua
            _lua.DoString(@"
                print('[ScriptManager] Lua environment initialized.')
            ");

            // Пример: регистрация кастомной функции для взаимодействия с C#
            _lua["printMessage"] = (Action<string>)((msg) => {
                Console.WriteLine($"[LUA] {msg}");
            });
        }

        /// <summary>
        /// Загружает скрипт из файла
        /// </summary>
        public bool LoadScript(string scriptName, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[Error] File not found: {filePath}");
                return false;
            }

            try
            {
                string scriptCode = File.ReadAllText(filePath);
                _scripts[scriptName] = scriptCode;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to load script: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Выполняет Lua-скрипт по имени
        /// </summary>
        public void ExecuteScript(string scriptName)
        {
            if (!_scripts.ContainsKey(scriptName))
            {
                Console.WriteLine($"[Error] Script not found: {scriptName}");
                return;
            }

            try
            {
                _lua.DoString(_scripts[scriptName]);
                Console.WriteLine($"[ScriptManager] Executed script: {scriptName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Execution failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Выполняет произвольный Lua-код
        /// </summary>
        public void ExecuteRawCode(string luaCode)
        {
            try
            {
                _lua.DoString(luaCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Raw code execution failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Возвращает список загруженных скриптов
        /// </summary>
        public List<string> GetLoadedScripts()
        {
            return new List<string>(_scripts.Keys);
        }

        /// <summary>
        /// Очищает все скрипты
        /// </summary>
        public void ClearScripts()
        {
            _scripts.Clear();
        }
    }
}