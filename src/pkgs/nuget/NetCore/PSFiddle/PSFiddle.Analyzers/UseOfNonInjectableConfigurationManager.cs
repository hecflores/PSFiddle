using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Specialized;

#if NETFRAMEWORK
using System.Web.Configuration;
#endif

namespace PSFiddle.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseOfNonInjectableConfigurationManager : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MC0001";
        internal static readonly LocalizableString Title = "Use of Non-Injectable Configuration Manager";
        internal static readonly LocalizableString MessageFormat = "Not allowed to use Non-Injectable Configuration Manager Class '{0}'";
        internal const string Category = "Dependency Injection";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(CatchConfigurManagerUses, SymbolKind.NamedType);
        }

       
        public void CatchConfigurManagerUses(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            if(namedTypeSymbol.Name == "WebConfigurationManager")
            {
                var maping = namedTypeSymbol.Locations;
                foreach(var locaton in maping)
                {
                    var diagnostic = Diagnostic.Create(Rule, locaton, $"{namedTypeSymbol.ContainingType.Name}");
                    context.ReportDiagnostic(diagnostic);
                }
                
            }
        }
    }
}
