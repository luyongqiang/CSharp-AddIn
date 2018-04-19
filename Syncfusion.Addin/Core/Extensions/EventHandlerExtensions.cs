using System;

namespace     Syncfusion.Core.Extensions
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Invoke an event asynchronously. Each subscriber to the event will be invoked on a separate thread.
        /// </summary>
        /// <param name="someEvent">The event to be invoked asynchronously.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The args of the event.</param>
        /// <typeparam name="TEventArgs">The type of <see cref="EventArgs"/> to be used with the event.</typeparam>
        public static void InvokeAsync<TEventArgs>(this EventHandler<TEventArgs> someEvent, object sender, TEventArgs args)
            where TEventArgs : EventArgs
        {
            if (someEvent == null)
            {
                return;
            }

            var eventListeners = someEvent.GetInvocationList();

            AsyncCallback endAsyncCallback = delegate(IAsyncResult iar)
            {
                var ar = iar as System.Runtime.Remoting.Messaging.AsyncResult;
                if (ar == null)
                {
                    return;
                }

                var invokedMethod = ar.AsyncDelegate as EventHandler<TEventArgs>;
                if (invokedMethod != null)
                {
                    invokedMethod.EndInvoke(iar);
                }
            };

            foreach (EventHandler<TEventArgs> methodToInvoke in eventListeners)
            {
                methodToInvoke.BeginInvoke(sender, args, endAsyncCallback, null);
            }
        }
    }
}
