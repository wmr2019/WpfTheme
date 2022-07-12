namespace WTLib.Utils
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Caliburn.Micro Mef引导基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MefBootstrapperBase<T> : BootstrapperBase
        where T : class, System.ComponentModel.INotifyPropertyChanged
    {
        private CompositionContainer _container;

        protected MefBootstrapperBase()
        {
            BeforeInitialize();
            Initialize();
        }

        protected virtual void BeforeInitialize()
        {
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            this.DisplayRootViewFor<T>();
        }

        protected override void Configure()
        {
            var composablePartCatalog = AssemblySource.Instance.Select(
                x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>();
            if (composablePartCatalog == null)
                return;

            var catalog = new AggregateCatalog();
            foreach (var part in composablePartCatalog)
                catalog.Catalogs.Add(part);

            var batch = new CompositionBatch();
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            InjectWindowManager(batch);

            _container = new CompositionContainer(catalog);
            batch.AddExportedValue(_container);
            _container.Compose(batch);
        }

        protected virtual void InjectWindowManager(CompositionBatch batch)
        {
            if (batch != null)
                batch.AddExportedValue<IWindowManager>(new WindowManager());
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (_container == null)
                return null;// todo: _container == null

            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            if (contract == null)
                return null;

            var exports = _container.GetExportedValues<object>(contract);
            if (exports == null)
                return null;// todo: exports == null

            if (exports.Count() > 0)
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            if (_container == null)
                return null;// todo: _container == null

            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            base.OnUnhandledException(sender, e);
        }
    }
}
