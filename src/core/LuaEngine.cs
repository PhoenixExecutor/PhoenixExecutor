using NLua;
using System;
using System.Diagnostics;

namespace PhoenixExecutor.Core
{
    public static class LuaEngine
    {
        private static Lua lua;
        private static bool initialized = false;

        public static void Initialize()
        {
            if (initialized) return;
            
            try 
            {
                lua = new Lua();
                lua.LoadCLRPackage();
                
                // Безопасные функции
                lua["print"] = new Action<object>(SafePrint);
                lua["warn"] = new Action<object>(SafeWarn);
                lua["wait"] = new Action<double>(Wait);
                
                // API Roblox
                RegisterRobloxAPI();
                
                initialized = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lua init error: {ex.Message}");
            }
        }

        private static void RegisterRobloxAPI()
        {
            dynamic game = lua.NewTable("game");
            
            game.GetService = new Func<string, dynamic>(serviceName => 
            {
                switch(serviceName.ToLower())
                {
                    case "workspace":
                        return new WorkspaceWrapper();
                    case "players":
                        return new PlayersWrapper();
                    default:
                        return null;
                }
            });
        }

        public static void Execute(string script)
        {
            if (!initialized || string.IsNullOrEmpty(script)) return;
            
            try 
            {
                lua.DoString(script);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lua error: {ex.Message}");
            }
        }

        private static void SafePrint(object msg)
        {
            Debug.WriteLine($"[LUA] {msg}");
            // Здесь можно добавить вывод в UI консоль
        }

        private static void Wait(double seconds)
        {
            System.Threading.Thread.Sleep((int)(seconds * 1000));
        }

        public static void Dispose()
        {
            lua?.Dispose();
            initialized = false;
        }
    }
    
    // Wrapper классы для Roblox API
    public class WorkspaceWrapper
    {
        public dynamic CurrentCamera => new CameraWrapper();
    }
    
    public class PlayersWrapper
    {
        public dynamic LocalPlayer => new PlayerWrapper();
    }
}