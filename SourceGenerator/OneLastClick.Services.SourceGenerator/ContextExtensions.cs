using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OneLastClick.Services.SourceGenerator
{
    public static class ContextExtensions
    {
        public static List<INamedTypeSymbol> GetAllTypesWithAttribute(this GeneratorExecutionContext context, INamedTypeSymbol attributeSymbol)
        {
            var services = new List<INamedTypeSymbol>();

            foreach (SyntaxTree syntaxTree in context.Compilation.SyntaxTrees)
            {
                SemanticModel semanticModel = context.Compilation.GetSemanticModel(syntaxTree);

                IEnumerable<ClassDeclarationSyntax> classes = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (ClassDeclarationSyntax classSyntax in classes)
                {
                    if (semanticModel.GetDeclaredSymbol(classSyntax) is not INamedTypeSymbol typeSymbol)
                    {
                        continue;
                    }

                    bool hasAttribute = typeSymbol
                        .GetAttributes()
                        .Any(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeSymbol));

                    if (hasAttribute)
                    {
                        services.Add(typeSymbol);
                    }
                }
            }

            return services;
        }
        
        public static List<INamedTypeSymbol> GetAllTypesWithMemberAttribute(this GeneratorExecutionContext context, INamedTypeSymbol attributeSymbol)
        {
            var types = new List<INamedTypeSymbol>();

            foreach (SyntaxTree syntaxTree in context.Compilation.SyntaxTrees)
            {
                SemanticModel semanticModel = context.Compilation.GetSemanticModel(syntaxTree);

                IEnumerable<ClassDeclarationSyntax> classes = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (ClassDeclarationSyntax classSyntax in classes)
                {
                    if (semanticModel.GetDeclaredSymbol(classSyntax) is not INamedTypeSymbol typeSymbol)
                    {
                        continue;
                    }

                    bool hasAttribute = typeSymbol
                        .GetMembers()
                        .Any(member => member
                            .GetAttributes()
                            .Any(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeSymbol)));

                    if (hasAttribute == true)
                    {
                        types.Add(typeSymbol);
                    }
                }
            }

            return types;
        }
    }
}