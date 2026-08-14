using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace DaLang.Lims.Web.Generator;

[Generator]
public class ComposeIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)
            .Collect();

        var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations);

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax classDecl &&
               classDecl.AttributeLists.Count > 0 &&
               classDecl.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static ComposeTarget? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDecl);
        if (classSymbol == null) return null;

        var composeAttr = classSymbol.GetAttributes()
            .FirstOrDefault(attr => attr.AttributeClass?.Name == "ComposeAttribute" ||
                                   attr.AttributeClass?.Name == "Compose");

        if (composeAttr == null) return null;

        var sourceTypes = new List<INamedTypeSymbol>();
        if (composeAttr.ConstructorArguments.Length > 0)
        {
            var arg = composeAttr.ConstructorArguments[0];
            if (arg.Kind == TypedConstantKind.Array)
            {
                foreach (var value in arg.Values)
                {
                    if (value.Value is INamedTypeSymbol typeSymbol)
                    {
                        sourceTypes.Add(typeSymbol);
                    }
                }
            }
            else if (arg.Value is INamedTypeSymbol singleType)
            {
                sourceTypes.Add(singleType);
            }
        }

        if (!sourceTypes.Any()) return null;

        var allProperties = new List<PropertyInfo>();
        var propertyNames = new HashSet<string>();

        foreach (var sourceType in sourceTypes)
        {
            var properties = sourceType.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.DeclaredAccessibility == Accessibility.Public &&
                           !p.IsStatic &&
                           p.SetMethod != null);

            foreach (var prop in properties)
            {
                var finalName = prop.Name;
                if (propertyNames.Contains(finalName))
                {
                    finalName = $"{sourceType.Name}_{prop.Name}";
                }

                allProperties.Add(new PropertyInfo
                {
                    Name = finalName,
                    Type = prop.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    OriginalName = prop.Name,
                    SourceTypeName = sourceType.Name,
                    IsConflict = propertyNames.Contains(prop.Name)
                });

                propertyNames.Add(finalName);
            }
        }

        return new ComposeTarget
        {
            ClassName = classSymbol.Name,
            Namespace = classSymbol.ContainingNamespace.ToDisplayString(),
            Properties = allProperties,
            SourceTypes = sourceTypes.Select(t => t.Name).ToList()
        };
    }

    private static void Execute(
        Compilation compilation,
        ImmutableArray<ComposeTarget?> targets,
        SourceProductionContext context)
    {
        if (targets.IsDefaultOrEmpty) return;

        foreach (var target in targets)
        {
            if (target == null)
            {
                // 报告诊断信息
                var diagnostic = Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "CG001",
                        "Composition Info",
                        "No compose targets found",
                        "Composition",
                        DiagnosticSeverity.Info,
                        true),
                    Location.None);
                context.ReportDiagnostic(diagnostic);
                return;
            }

            if (target == null) continue;

            var code = GenerateComposedClass(target);
            context.AddSource($"{target.ClassName}.Composed.g.cs", code);
        }
    }

    private static string GenerateComposedClass(ComposeTarget target)
    {
        var sb = new StringBuilder();

        // 收集需要的 using 命名空间
        var usings = new HashSet<string> { "System" };
        foreach (var prop in target.Properties)
        {
            // 提取类型所在的命名空间
            var typeName = prop.Type;
            if (typeName.StartsWith("global::"))
            {
                typeName = typeName.Substring(8);
            }

            var parts = typeName.Split('.');
            if (parts.Length > 1)
            {
                // 去掉类型名，只保留命名空间
                var ns = string.Join(".", parts.Take(parts.Length - 1));
                if (!string.IsNullOrEmpty(ns) && ns != "System" && ns != "System.Collections.Generic")
                {
                    usings.Add(ns);
                }
            }
        }

        // 添加 using 语句
        foreach (var ns in usings.OrderBy(u => u))
        {
            sb.AppendLine($"using {ns};");
        }
        sb.AppendLine();

        // 命名空间
        sb.AppendLine($"namespace {target.Namespace}");
        sb.AppendLine("{");

        // 类声明
        sb.AppendLine($"    public partial class {target.ClassName}");
        sb.AppendLine("    {");
        sb.AppendLine("        // ========== 自动组合的属性 ==========");

        // 生成属性
        foreach (var prop in target.Properties)
        {
            sb.AppendLine($"        public {prop.Type} {prop.Name} {{ get; set; }}");
        }

        sb.AppendLine();
        sb.AppendLine("        // ========== 构造函数 ==========");

        // 生成带参数的构造函数
        if (target.Properties.Any())
        {
            var paramList = string.Join(", ",
                target.Properties.Select(p => $"{p.Type} {GetValidParameterName(p.OriginalName)}"));

            sb.AppendLine($"        public {target.ClassName}({paramList})");
            sb.AppendLine("        {");
            foreach (var prop in target.Properties)
            {
                sb.AppendLine($"            this.{prop.Name} = {GetValidParameterName(prop.OriginalName)};");
            }
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        // 默认构造函数
        sb.AppendLine($"        public {target.ClassName}()");
        sb.AppendLine("        {");
        sb.AppendLine("        }");

        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    // 处理 C# 关键字作为参数名的情况
    private static string GetValidParameterName(string name)
    {
        // C# 关键字列表
        var keywords = new HashSet<string>
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
            "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
            "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is",
            "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
            "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte",
            "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch",
            "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
            "ushort", "using", "virtual", "void", "volatile", "while"
        };

        if (keywords.Contains(name))
        {
            return $"@{name}";
        }

        // 如果以小写开头，保持原样（属性名通常大写）
        return char.IsLower(name[0]) ? name : char.ToLowerInvariant(name[0]) + name.Substring(1);
    }
}

internal class ComposeTarget
{
    public string ClassName { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
    public List<PropertyInfo> Properties { get; set; } = new();
    public List<string> SourceTypes { get; set; } = new();
}

internal class PropertyInfo
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public string SourceTypeName { get; set; } = string.Empty;
    public bool IsConflict { get; set; }
}