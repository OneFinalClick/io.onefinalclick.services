using Microsoft.CodeAnalysis;

namespace OneLastClick.Services.SourceGenerator
{
    public class OneLastClickServicesReferences
    {
        private const string ApplicationServiceAttributeName = "OneLastClick.Services.Attributes.ApplicationServiceAttribute";
        private const string MonoBehaviourServiceAttributeName = "OneLastClick.Services.Attributes.MonoBehaviourServiceAttribute";
        private const string InjectServiceAttributeName = "OneLastClick.Services.Injection.InjectServiceAttribute";
        private const string BuilderMetadataName = "OneLastClick.Services.ServicesCollectionBuilder";
        private const string ApplicationServicesMetadataName = "OneLastClick.Services.ApplicationServices";
        private const string ServiceResolverInterfaceName = "OneLastClick.Services.IServiceResolver";
        private const string ServiceInjectableInterfaceName = "OneLastClick.Services.Injection.IServiceInjectable";
        private const string ServiceRegistererInterfaceName = "OneLastClick.Services.IServiceRegisterer";
        
        public INamedTypeSymbol ApplicationServiceAttributeSymbol { get; }
        public INamedTypeSymbol ServiceCollectionBuilderSymbol { get; }
        public  INamedTypeSymbol ApplicationServicesSymbol { get; }
        public INamedTypeSymbol InjectServiceAttributeSymbol { get; }
        public INamedTypeSymbol ServiceResolverInterfaceSymbol { get; }
        public INamedTypeSymbol ServiceInjectableInterfaceSymbol { get; }
        public INamedTypeSymbol MonoBehaviourServiceAttributeSymbol { get; }
        public INamedTypeSymbol ServiceRegistererInterfaceSymbol { get; }

        private OneLastClickServicesReferences(INamedTypeSymbol applicationServiceAttributeSymbol, INamedTypeSymbol serviceCollectionServiceCollectionBuilderSymbol, INamedTypeSymbol applicationServicesSymbol, INamedTypeSymbol injectServiceAttributeSymbol, INamedTypeSymbol serviceResolverInterfaceSymbol, INamedTypeSymbol serviceInjectableInterfaceSymbol, INamedTypeSymbol monoBehaviourServiceAttributeSymbol, INamedTypeSymbol serviceRegistererInterfaceSymbol)
        {
            ApplicationServiceAttributeSymbol = applicationServiceAttributeSymbol;
            ServiceCollectionBuilderSymbol = serviceCollectionServiceCollectionBuilderSymbol;
            ApplicationServicesSymbol = applicationServicesSymbol;
            InjectServiceAttributeSymbol = injectServiceAttributeSymbol;
            ServiceResolverInterfaceSymbol = serviceResolverInterfaceSymbol;
            ServiceInjectableInterfaceSymbol = serviceInjectableInterfaceSymbol;
            MonoBehaviourServiceAttributeSymbol = monoBehaviourServiceAttributeSymbol;
            ServiceRegistererInterfaceSymbol = serviceRegistererInterfaceSymbol;
        }

        public static bool TryResolveReferences(GeneratorExecutionContext context, out OneLastClickServicesReferences references)
        {
            Compilation compilation = context.Compilation;
            string compilationName = compilation.AssemblyName ?? "<unknown>";
            
            INamedTypeSymbol registrationAttributeSymbol = compilation.GetTypeByMetadataName(ApplicationServiceAttributeName);

            if (registrationAttributeSymbol == null)
            {
                references = null;
                return false;
            }
            
            INamedTypeSymbol builderSymbol = compilation.GetTypeByMetadataName(BuilderMetadataName);

            if (builderSymbol == null)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDefinitions.BuilderNotFoundDiagnostic,
                        Location.None,
                        compilationName));

                references = null;
                return false;
            }
            
            INamedTypeSymbol applicationServicesSymbol = compilation.GetTypeByMetadataName(ApplicationServicesMetadataName);

            if (applicationServicesSymbol == null)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDefinitions.ApplicationServicesNotFoundDiagnostic,
                        Location.None,
                        compilationName));

                references = null;
                return false;
            }
            
            INamedTypeSymbol injectionAttributeSymbol = compilation.GetTypeByMetadataName(InjectServiceAttributeName);

            if (injectionAttributeSymbol == null)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDefinitions.InjectServiceAttributeNotFoundDiagnostic,
                        Location.None,
                        compilationName));

                references = null;
                return false;
            }
            
            INamedTypeSymbol serviceResolverSymbol = compilation.GetTypeByMetadataName(ServiceResolverInterfaceName);

            if (serviceResolverSymbol == null)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDefinitions.ServiceResolverInterfaceNotFoundDiagnostic,
                        Location.None,
                        compilationName));

                references = null;
                return false;
            }
            
                        
            INamedTypeSymbol serviceInjectableSymbol = compilation.GetTypeByMetadataName(ServiceInjectableInterfaceName);

            if (serviceInjectableSymbol == null)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDefinitions.ServiceInjectableInterfaceNotFoundDiagnostic,
                        Location.None,
                        compilationName));

                references = null;
                return false;
            }

            INamedTypeSymbol monoBehaviourServiceSymbol = compilation.GetTypeByMetadataName(MonoBehaviourServiceAttributeName);

            if (monoBehaviourServiceSymbol == null)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDefinitions.MonoBehaviourServiceAttributeNotFoundDiagnostic,
                        Location.None,
                        compilationName));

                references = null;
                return false;
            }
            
            INamedTypeSymbol serviceRegisterer = compilation.GetTypeByMetadataName(ServiceRegistererInterfaceName);

            if (serviceRegisterer == null)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDefinitions.ServiceRegistererInterfaceNotFoundDiagnostic,
                        Location.None,
                        compilationName));

                references = null;
                return false;
            }
            
            references = new OneLastClickServicesReferences(registrationAttributeSymbol, builderSymbol, applicationServicesSymbol, injectionAttributeSymbol, serviceResolverSymbol, serviceInjectableSymbol, monoBehaviourServiceSymbol, serviceRegisterer);
            return true;
        }
    }
}