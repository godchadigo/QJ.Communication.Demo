using QJ.Communication.Core.Interface;
using QJ.Communication.Core.Model.Plugin;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core
{
    public class PluginLoader
    {
        private readonly Dictionary<string, QJPluginBase> _loadedPlugins = new();
        

        public QJPluginBase? LoadPlugin(string pluginPath)
        {
            try
            {
                var assembly = Assembly.LoadFrom(pluginPath);

                // 使用LINQ查找符合條件的插件類型
                var pluginType = assembly.GetTypes()
                    .FirstOrDefault(t =>   (typeof(QJPluginBase).IsAssignableFrom(t) && typeof(INetCommunication).IsAssignableFrom(t))
                                        || (typeof(QJPluginBase).IsAssignableFrom(t) && typeof(ISerialCommunication).IsAssignableFrom(t))
                                        && !t.IsInterface
                                        && !t.IsAbstract);

                if (pluginType is null)
                {
                    Console.WriteLine("No valid plugin type found in assembly: {Path}", pluginPath);
                    return null;
                }

                // 使用 pattern matching 和 null 檢查
                if (Activator.CreateInstance(pluginType) is not QJPluginBase plugin)
                {
                    Console.WriteLine("Failed to create plugin instance: {Type}", pluginType.FullName);
                    return null;
                }

                // 使用集合初始化器和null檢查
                _loadedPlugins[plugin.Name] = plugin;

                // 使用 try/catch 初始化插件
                try
                {
                    plugin.Initialize();
                    //Log.Information("Successfully loaded and initialized plugin: {Name}", plugin.Name);
                    Console.WriteLine($"成功載入插件 : {plugin.Name}，插件類型 :  {plugin.communicationType}");
                }
                catch (Exception ex)
                {
                    //Log.Error(ex, "Failed to initialize plugin: {Name}", plugin.Name);
                    _loadedPlugins.Remove(plugin.Name);
                    return null;
                }

                return plugin;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load plugin from path: {ex}");
                return null;
            }
        }


        // 獲取已加載插件的資訊
        public IReadOnlyCollection<PluginInfo> GetLoadedPluginInfo() =>
            _loadedPlugins.Values
                .Select(p => new PluginInfo(
                    p.Name,
                    p.GetType().Assembly.GetName().Version ?? new Version(1, 0),
                    p.GetType().GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty))
                .ToList()
                .AsReadOnly();

        // 使用 index 運算符獲取插件
        public IQJPlugin? this[string pluginName] =>
            _loadedPlugins.TryGetValue(pluginName, out var plugin) ? plugin : null;

        // 使用元組返回多個值
        public (bool Success, IQJPlugin? Plugin) TryGetPlugin(string name) =>
            _loadedPlugins.TryGetValue(name, out var plugin)
                ? (true, plugin)
                : (false, null);

        // 異步加載多個插件
        public async Task<IEnumerable<IQJPlugin>> LoadPluginsAsync(IEnumerable<string> pluginPaths)
        {
            var loadTasks = pluginPaths.Select(async path =>
            {
                await Task.Yield(); // 確保異步執行
                return LoadPlugin(path);
            });

            var results = await Task.WhenAll(loadTasks);
            return results.Where(p => p != null)!;
        }

        // 使用 switch expression 處理插件狀態
        public string GetPluginStatus(string pluginName) =>
            _loadedPlugins.TryGetValue(pluginName, out var plugin)
                ? plugin switch
                {
                    //{ IsEnabled: true } => "Running",
                    { IsInitialized: true } => "Initialized",
                    _ => "Loaded"
                }
                : "Not Found";

        // 獲取已經載入的插件資訊
        public Dictionary<string, QJPluginBase> GetLoadPlugins()
        {
            return _loadedPlugins;
        }
    }

   


    // 使用傳統類代替 record
    public class PluginInfo
    {
        public string Name { get; }
        public Version Version { get; }
        public string Description { get; }

        public PluginInfo(string name, Version version, string description)
        {
            Name = name;
            Version = version;
            Description = description;
        }
    }
}
