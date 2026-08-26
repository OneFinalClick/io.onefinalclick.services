using System;
using OneFinalClick.Services;
using OneFinalClick.Services.Injection;
using NUnit.Framework;

namespace OneFinalClick.Tests.Services
{
    public partial class ServiceCollectionTests
    {
        #region Test Types

        public interface IFooService { }
        public interface IBarService { }
        public interface IBazService { }

        private class FooService : IService, IFooService
        {
            public int Starts;
            public int Updates;
            public int Stops;

            public void OnServiceStart() => Starts++;
            public void OnServiceUpdate() => Updates++;
            public void OnServiceStop() => Stops++;
        }

        private class MultiService : IService, IFooService, IBarService
        {
            public int Starts;

            public void OnServiceStart() => Starts++;
            public void OnServiceUpdate() { }
            public void OnServiceStop() { }
        }

        private class PlainService : IFooService { }

        private class WrongService : IBazService { }

        public partial class NeedsInjection : IService
        {
            [InjectService] public IFooService Foo { get; private set; }

            public void OnServiceStart() { }
            public void OnServiceUpdate() { }
            public void OnServiceStop() { }
        }

        #endregion

        // ---------------------------------------------------------------------
        // Registration
        // ---------------------------------------------------------------------

        [Test]
        public void Register_SingleService_RegistersInterfaceAndConcrete()
        {
            var builder = new ServicesCollectionBuilder();

            builder.Register<IFooService, FooService>();

            var services = builder.Build();

            Assert.IsTrue(services.TryGet<IFooService>(out var fooInterface));
            Assert.IsTrue(services.TryGet<FooService>(out var fooConcrete));

            Assert.AreSame(fooInterface, fooConcrete);
        }

        [Test]
        public void Register_DuplicateService_ThrowsArgumentException()
        {
            var builder = new ServicesCollectionBuilder();

            builder.Register<IFooService, FooService>();

            Assert.Throws<ArgumentException>(() =>
                builder.Register<IFooService, FooService>());
        }

        [Test]
        public void Register_MultipleInterfaces_RegistersSameInstanceForEachInterface()
        {
            var builder = new ServicesCollectionBuilder();
            var service = new MultiService();

            builder.Register(service, typeof(IFooService), typeof(IBarService));

            var services = builder.Build();

            Assert.AreSame(
                services.Get<IFooService>(),
                services.Get<IBarService>());

            Assert.AreSame(
                service,
                services.Get<IFooService>());
        }

        [Test]
        public void Register_InstanceRegistration_UsesProvidedInstance()
        {
            var builder = new ServicesCollectionBuilder();
            var instance = new FooService();

            builder.Register<IFooService, FooService>(instance);

            var services = builder.Build();

            Assert.AreSame(instance, services.Get<IFooService>());
            Assert.AreSame(instance, services.Get<FooService>());
        }

        [Test]
        public void Register_StaticRegistration_CreatesNewInstance()
        {
            var builder = new ServicesCollectionBuilder();

            builder.Register<IFooService, FooService>();

            var services = builder.Build();

            Assert.NotNull(services.Get<IFooService>());
            Assert.IsInstanceOf<FooService>(services.Get<IFooService>());
        }

        [Test]
        public void Register_InvalidRegistration_ThrowsArgumentException()
        {
            var builder = new ServicesCollectionBuilder();

            Assert.Throws<ArgumentException>(() =>
                builder.Register(new WrongService(), typeof(IFooService)));
        }

        // ---------------------------------------------------------------------
        // Resolution
        // ---------------------------------------------------------------------

        [Test]
        public void Resolve_Resolution_ReturnsRegisteredService()
        {
            var builder = new ServicesCollectionBuilder();
            var service = new FooService();

            builder.Register<IFooService, FooService>(service);

            var appServices = builder.Build();

            appServices.StartServices();

            Assert.AreSame(service, appServices.Get<IFooService>());
        }
        
        [Test]
        public void Resolve_FallbackResolve_ReturnsInnerScopeService()
        {
            var appBuilder = new ServicesCollectionBuilder();
            var appService = new FooService();
            appBuilder.Register<IFooService, FooService>(appService);
            var app = appBuilder.Build();

            var sceneBuilder = new ServicesCollectionBuilder();
            var sceneService = new FooService();
            sceneBuilder.Register<IFooService, FooService>(sceneService);
            var scene = sceneBuilder.Build();

            scene.StartServices(app);

            Assert.AreSame(sceneService, scene.Get<IFooService>());
        }

        [Test]
        public void Resolve_FallbackResolve_InjectsOuterScopeService()
        {
            var appBuilder = new ServicesCollectionBuilder();
            var foo = new FooService();
            appBuilder.Register<IFooService, FooService>(foo);
            var app = appBuilder.Build();

            var sceneBuilder = new ServicesCollectionBuilder();
            var injected = new NeedsInjection();

            sceneBuilder.Register(injected);

            var scene = sceneBuilder.Build();

            scene.StartServices(app);

            Assert.AreSame(foo, injected.Foo);
        }

        [Test]
        public void Resolve_MissingService_GetThrows()
        {
            var builder = new ServicesCollectionBuilder();
            var services = builder.Build();

            Assert.Throws<InvalidOperationException>(() =>
                services.Get<IFooService>());
        }

        [Test]
        public void Resolve_MissingService_TryGetReturnsFalse()
        {
            var builder = new ServicesCollectionBuilder();
            var services = builder.Build();

            Assert.IsFalse(services.TryGet<IFooService>(out _));
        }

        [Test]
        public void Resolve_BeforeStartup_ServiceCanStillBeResolved()
        {
            var builder = new ServicesCollectionBuilder();
            var service = new FooService();

            builder.Register<IFooService, FooService>(service);

            var services = builder.Build();

            Assert.IsFalse(services.IsStarted);
            Assert.AreSame(service, services.Get<IFooService>());
        }

        // ---------------------------------------------------------------------
        // Lifecycle
        // ---------------------------------------------------------------------

        
        
        [Test]
        public void Lifecycle_StartUpdateStop_CallsServiceCallbacksExactlyOnce()
        {
            var builder = new ServicesCollectionBuilder();
            var service = new FooService();

            builder.Register<IFooService, FooService>(service);

            var services = builder.Build();

            services.StartServices();
            services.UpdateServices();
            services.StopServices();

            Assert.AreEqual(1, service.Starts);
            Assert.AreEqual(1, service.Updates);
            Assert.AreEqual(1, service.Stops);
        }
        
        [Test]
        public void Lifecycle_StartUpdateStop_DoesNotCallServiceCallbacksOnOuterScopeServices()
        {
            var appBuilder = new ServicesCollectionBuilder();
            var appService = new FooService();
            appBuilder.Register<IFooService, FooService>(appService);
            var appServices = appBuilder.Build();
            
            var sceneBuilder = new ServicesCollectionBuilder();
            var sceneServices = sceneBuilder.Build();

            sceneServices.StartServices(appServices);
            sceneServices.UpdateServices();
            sceneServices.StopServices();

            Assert.AreEqual(0, appService.Starts);
            Assert.AreEqual(0, appService.Updates);
            Assert.AreEqual(0, appService.Stops);
        }
        
        [Test]
        public void Lifecycle_StartUpdateStop_DoesNotCallServiceCallbacksOnOuterScopeServicesWithOverride()
        {
            var appBuilder = new ServicesCollectionBuilder();
            var appService = new FooService();
            appBuilder.Register<IFooService, FooService>(appService);
            var appServices = appBuilder.Build();
            
            var sceneBuilder = new ServicesCollectionBuilder();
            var sceneService = new FooService();
            sceneBuilder.Register<IFooService, FooService>(sceneService);
            var sceneServices = sceneBuilder.Build();

            sceneServices.StartServices(appServices);
            sceneServices.UpdateServices();
            sceneServices.StopServices();

            Assert.AreEqual(0, appService.Starts);
            Assert.AreEqual(0, appService.Updates);
            Assert.AreEqual(0, appService.Stops);
            
            Assert.AreEqual(1, sceneService.Starts);
            Assert.AreEqual(1, sceneService.Updates);
            Assert.AreEqual(1, sceneService.Stops);
        }

        [Test]
        public void Lifecycle_StopBeforeStart_DoesNothing()
        {
            var builder = new ServicesCollectionBuilder();
            var service = new FooService();

            builder.Register<IFooService, FooService>(service);

            var services = builder.Build();

            services.StopServices();

            Assert.AreEqual(0, service.Stops);
        }

        [Test]
        public void Lifecycle_ServiceRegisteredMultipleInterfaces_StartsOnlyOnce()
        {
            var builder = new ServicesCollectionBuilder();
            var service = new MultiService();

            builder.Register(service, typeof(IFooService), typeof(IBarService));

            var services = builder.Build();

            services.StartServices();

            Assert.AreEqual(1, service.Starts);
        }
    }
}