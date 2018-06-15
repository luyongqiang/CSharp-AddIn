using System.ComponentModel;

namespace Syncfusion.Addin.Core
{
    /// <summary>
    /// Framework event manager.
    /// </summary>
    internal class EventManager
    {
        #region --- Fields ---

        /// <summary>
        /// The events
        /// </summary>
        private static readonly EventHandlerList Events;

        /// <summary>
        /// The bundlekey
        /// </summary>
        private static readonly object Bundlekey = new object();

        /// <summary>
        /// The servicekey
        /// </summary>
        private static readonly object Servicekey = new object();

        /// <summary>
        /// The frameworkkey
        /// </summary>
        private static readonly object Frameworkkey = new object();

        #endregion --- Fields ---

        #region --- Event management ---

        /// <summary>
        /// Occurs when [bundle event].
        /// </summary>
        internal static event BundleEventHandler BundleEvent
        {
            add
            {
                Events.AddHandler(Bundlekey, value);
            }
            remove
            {
                Events.RemoveHandler(Bundlekey, value);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:BundleChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="BundleEventArgs"/> instance containing the event data.</param>
        internal static void OnBundleChanged(BundleEventArgs e)
        {
            if (Events[Bundlekey] != null)
            {
                var handler = Events[Bundlekey] as BundleEventHandler;
                if (handler != null)
                    handler(null, e);
            }
        }

        internal static event ServiceEventHandler ServiceEvent
        {
            add
            {
                Events.AddHandler(Servicekey, value);
            }
            remove
            {
                Events.RemoveHandler(Servicekey, value);
            }
        }

        internal static void OnServiceChanged(ServiceEventArgs e)
        {
            if (Events[Servicekey] != null)
            {
                ServiceEventHandler handler = Events[Servicekey] as ServiceEventHandler;
                if (handler != null) handler(null, e);
            }
        }

        internal static event FrameworkEventHandler FrameworkEvent
        {
            add
            {
                Events.AddHandler(Frameworkkey, value);
            }
            remove
            {
                Events.RemoveHandler(Frameworkkey, value);
            }
        }

        internal static void OnFrameworkChanged(FrameworkEventArgs e)
        {
            if (Events[Frameworkkey] != null)
            {
                FrameworkEventHandler handler = Events[Frameworkkey] as FrameworkEventHandler;
                if (handler != null) handler(null, e);
            }
        }

        #endregion --- Event management ---

        /// <summary>
        /// 事件管理
        /// </summary>
        static EventManager()
        {
            Events = new EventHandlerList();
        }
    }
}