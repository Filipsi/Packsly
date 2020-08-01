using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NLog;
using Packsly3.Core.Common.Register;
using Packsly3.Core.Launcher.Instance;

namespace Packsly3.Core.Launcher.Adapter {

    internal static class AdapterHandler {

        #region Properties

        public static readonly IReadOnlyCollection<IAdapter> Adapters = Array.AsReadOnly(
            RegisterAttribute.GetOccurrencesFor<IAdapter>()
        );

        #endregion

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Handlers

        public static void HandleLifecycleChange(object sender, Lifecycle.ChangedEventArgs args) {
            if (args.Instance == null) {
                return;
            }

            IEnumerable<IAdapter> usedAdapters = Adapters.Where(adapter =>
                args.Instance.PackslyConfig.Adapters.Any(pair => pair.Key == adapter.Id) &&
                adapter.IsCompatible(args.Instance) &&
                adapter.IsCompatible(args.EventName)
            );

            foreach (IAdapter adapter in usedAdapters) {
                if (TryGetAdapterConfig(adapter, args.Instance, out JObject config)) {
                    logger.Debug($"Executing adapter {adapter.Id}...");
                    adapter.Execute(config, args.EventName, args.Instance);
                } else {
                    logger.Warn($"Unable to execute adapter {adapter.Id}, without valid configuration!");
                }
            }
        }

        #endregion

        #region Helpers

        public static bool TryGetAdapterConfig(IAdapter adapter, IMinecraftInstance instance, out JObject config) {
            // Set default return value for the config
            config = null;

            // Try to get adapter config from the instance
            object adapterConfig = instance.PackslyConfig.Adapters.GetConfigFor(adapter);
            
            // This adapter has no configuration
            if (adapterConfig == null) {
                return true;
            }

            // Try to parse adapter config
            try {
                config = JObject.FromObject(adapterConfig);
                return true;
            } catch (Exception ex) {
                logger.Error(ex, $"Failed to parse configuration for adapter {adapter.Id}");
            }

            return false;
        }

        #endregion

    }

}
