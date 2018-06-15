using System;

namespace Syncfusion.Addin.Core.Collection
{
    public class ContainerLocker<ContainerType> : DisposableLocker
    {
        protected ContainerType Container
        {
            get;
            private set;
        }

        public ContainerLocker(object syncRoot, ContainerType container, int millisecondsTimeout)
            : base(syncRoot, millisecondsTimeout)
        {
            if (syncRoot == null)
            {
                throw new ArgumentNullException("mutex");
            }
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.Container = container;
        }

        public new void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}