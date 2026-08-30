using Microsoft.CodeAnalysis;

namespace OneLastClick.Services.SourceGenerator
{
    public static class DiagnosticDefinitions
    {
        public static readonly DiagnosticDescriptor ServicesFoundDiagnostic =
            new(
                id: "FCSG001",
                title: "Application services found",
                messageFormat:
                "Found {0} application service(s) in compilation '{1}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Info,
                isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor ServicesWithInjectAttributesFoundDiagnostic =
            new(
                id: "FCSG002",
                title: "Services requiring injection found",
                messageFormat:
                "Found {0} service(s) needing injection in compilation '{1}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Info,
                isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor MonoBehaviourServicesFoundDiagnostic =
            new(
                id: "FCSG003",
                title: "MonoBehaviourServices found",
                messageFormat:
                "Found {0} monobehaviour service(s) needing injection in compilation '{1}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Info,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor BuilderNotFoundDiagnostic =
            new(
                id: "FCSG101",
                title: "ServicesCollectionBuilder not found",
                messageFormat:
                    "Could not find 'OneLastClick.Services.ServicesCollectionBuilder' " +
                    "in compilation '{0}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ApplicationServicesNotFoundDiagnostic =
            new(
                id: "FCSG102",
                title: "ApplicationServices not found",
                messageFormat:
                    "Could not find 'OneLastClick.Services.ApplicationServices' " +
                    "in compilation '{0}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor InjectServiceAttributeNotFoundDiagnostic =
            new(
                id: "FCSG104",
                title: "InjectServiceAttribute not found",
                messageFormat:
                "Could not find 'OneLastClick.Services.Injection.InjectServiceAttribute' " +
                "in compilation '{0}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor ServiceResolverInterfaceNotFoundDiagnostic =
            new(
                id: "FCSG105",
                title: "IServiceResolver not found",
                messageFormat:
                "Could not find 'OneLastClick.Services.IServiceResolver' " +
                "in compilation '{0}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor ServiceInjectableInterfaceNotFoundDiagnostic =
            new(
                id: "FCSG106",
                title: "IServiceInjectable not found",
                messageFormat:
                "Could not find 'OneLastClick.Services.Injection.IServiceInjectable' " +
                "in compilation '{0}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);
        
        public static readonly DiagnosticDescriptor MonoBehaviourServiceAttributeNotFoundDiagnostic =
            new(
                id: "FCSG107",
                title: "MonoBehaviourServiceAttribute not found",
                messageFormat:
                "Could not find 'OneLastClick.Services.Attributes.MonoBehaviourServiceAttribute' " +
                "in compilation '{0}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        
        public static readonly DiagnosticDescriptor ServiceRegistererInterfaceNotFoundDiagnostic =
            new(
                id: "FCSG108",
                title: "IServiceRegisterer not found",
                messageFormat:
                "Could not find 'OneLastClick.Services.IServiceRegisterer' " +
                "in compilation '{0}'.",
                category: "OneLastClick.Services.SourceGenerator",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);
    }
}