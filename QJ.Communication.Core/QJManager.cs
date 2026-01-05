using QJ.Communication.Core;
using QJ.Communication.Core.Extension;
using QJ.Communication.Core.Helper;
using QJ.Communication.Core.Model.Plugin;
using QJ.Communication.Core.Model.Result;
using System.Reflection;
using System.Runtime.Versioning;
using TouchSocket.Core;

namespace QJ.Communication
{
    public class QJManager
    {
        // User Progarme -> CommCore -> Plugins
        // 使用者呼叫CommCore抽象插件介面，啟動時插件會載入，利用字串指定要連接的插件後，直接呼叫例如ReadInt16這樣的方法。

        public static void PrintAssemblyInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var targetFrameworkAttribute = assembly
                .GetCustomAttribute<TargetFrameworkAttribute>();

            Console.WriteLine($"Target Framework: {targetFrameworkAttribute?.FrameworkName}");
            Console.WriteLine($"Assembly Location: {assembly.Location}");
            Console.WriteLine($"Assembly Version: {assembly.GetName().Version}");
            
        }
        /// <summary>
        /// 初始化插件列表
        /// </summary>
        public void Init()
        {
            var loader = new PluginLoader();
            try
            {
                PrintAssemblyInfo();

                var pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
                Console.WriteLine("Dir:" + pluginsDir);
                // 確保目錄存在
                if (!Directory.Exists(pluginsDir))
                {
                    Directory.CreateDirectory(pluginsDir);
                }

                // 載入所有插件
                var pluginFiles = Directory.GetFiles(pluginsDir, "*.dll");
                //Console.WriteLine($"找到{pluginFiles.Length}個插件!");
                foreach (var pluginPath in pluginFiles)
                {
                    try
                    {
                        var plugin = loader.LoadPlugin(pluginPath);
                        if (plugin != null)
                        {
                            //Console.WriteLine($"Successfully loaded plugin: {plugin.Name}");
                            if (plugin.GetEnable())
                            {
                                PluginHelper.Instance.Plugins.Add(plugin.Name, plugin);
                            }
                            else
                            {
                                Console.WriteLine($"插件{plugin.Name}被開發者鎖定無法使用，請聯繫開發者解鎖。");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load plugin {pluginPath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing plugins directory: {ex.Message}");
            }
        }
        public QJResult<string> Version(string plugName)
        {
            var res = PluginHelper.Instance.Plugins.ContainsKey(plugName);
            if (!res)
            {
                return new QJResult<string>() { IsOk = false , Message = CommonMsg.MSG_PLUGIN_ERR_1};
            }
            else
            {
                var plug = PluginHelper.Instance.Plugins[plugName];
                return new QJResult<string>() { IsOk = true,  Data = plug.Version , Message = $"版本:{plug.Version}"};
            }
        }
        /// <summary>
        /// 獲取已經載入的插件資訊
        /// </summary>
        /// <returns></returns>
        public QJResult<Dictionary<string, QJPluginBase>> GetPluings()
        {
            return PluginHelper.Instance.Plugins.QJDataResponse(isOk:true);
        }

    }
}
