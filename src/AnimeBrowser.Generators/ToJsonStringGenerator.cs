using AnimeBrowser.Generators.TemplateModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AnimeBrowser.Generators
{
    [Generator]
    public class ToJsonStringGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            //#if DEBUG
            //            if (!Debugger.IsAttached)
            //            {
            //                Debugger.Launch();
            //            }
            //#endif
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }


        public void Execute(GeneratorExecutionContext context)
        {
            var baseName = Assembly.GetExecutingAssembly().GetName().Name;
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{baseName}.Templates.JsonStringTemplate.txt");
            using StreamReader streamReader = new(stream, Encoding.UTF8);
            Template template = Template.Parse(streamReader.ReadToEnd());
            if (context.SyntaxReceiver is SyntaxReceiver syntaxReceiver)
            {
                foreach (ClassDeclarationSyntax cds in syntaxReceiver.ClassDeclarations)
                {
                    var semanticModel = context.Compilation.GetSemanticModel(cds.SyntaxTree);
                    var symbol = semanticModel.GetDeclaredSymbol(cds);
                    var modelNamespace = symbol?.ContainingNamespace.ToString() ?? string.Empty;

                    var className = cds.Identifier.Text;
                    var modifierString = cds.Modifiers.ToFullString().Trim();
                    var templateModel = new JsonStringTemplateModel() { ClassName = className, Modifier = modifierString, Namespace = modelNamespace };
                    var templateResult = template.Render(templateModel);
                    var fileName = $"{className}String.cs";
                    context.AddSource(fileName, SourceText.From(templateResult, Encoding.UTF8));
                }
            }
        }
    }

    class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> ClassDeclarations = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds && HasToJsonStringAttribute(cds.AttributeLists))
            {
                ClassDeclarations.Add(cds);
            }
        }

        private bool HasToJsonStringAttribute(SyntaxList<AttributeListSyntax> attributeList)
        {
            if (attributeList.Count <= 0) return false;
            var attributesResult = attributeList.SelectMany(al => al?.Attributes.Where(a => IsAttributeNameEquals(a, "ToJsonString")));
            return attributesResult?.Any() == true;
        }

        private bool IsAttributeNameEquals(AttributeSyntax attribute, string attName)
        {
            var identifierNS = attribute.Name as IdentifierNameSyntax;
            if (identifierNS == default) return false;
            return identifierNS.Identifier.Text.Equals(attName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
