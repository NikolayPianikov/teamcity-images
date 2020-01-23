using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IoC;

// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerFileContentParser : IDockerFileContentParser
    {
        private static readonly Regex ArgRegex = new Regex(@"\s*ARG\s+(?<name>\w+)=(?<value>.*)", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly DockerVariable[] EmptyVars = new DockerVariable[0];
        private readonly ILogger _logger;

        public DockerFileContentParser([NotNull] ILogger logger) => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public IEnumerable<DockerLine> Parse(string text, IReadOnlyDictionary<string, string> values)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var rawLines = text.Split(System.Environment.NewLine);
            var lines = new List<DockerLine>();
            var localVars = new List<DockerVariable>();
            foreach (var line in rawLines)
            {
                var vars = (
                    from variable in values
                    where line.Contains($"{variable.Key}")
                    select new DockerVariable(variable.Key, variable.Value))
                    .Distinct()
                    .ToList();

                var lineWithVars = ApplyVariables(line, vars);
                var argStatement = ArgRegex.Match(lineWithVars);
                if (argStatement.Success)
                {
                    var value = argStatement.Groups["value"].Value;
                    if (string.IsNullOrEmpty(value))
                    {
                        localVars.Add(new DockerVariable(argStatement.Groups["name"].Value, value));
                    }
                }

                var lineWithLocalVars = ApplyVariables(lineWithVars, localVars);
                if (lineWithLocalVars.Contains("${") && lineWithLocalVars.Contains("}"))
                {
                    _logger.Log($"The line \"{line.Trim()}\" may still contain some unresolved variables. May be these variables will be resolved by environment variables.", Result.Warning);
                }

                if (line.TrimStart().StartsWith("#"))
                {
                    lines.Add(new DockerLine(lineWithVars, DockerLineType.Comment, vars));
                }
                else
                {
                    lines.Add(new DockerLine(line, DockerLineType.Text, vars));
                }
            }

            var usedVariables = lines
                .Where(line => line.Type != DockerLineType.Comment)
                .SelectMany(line => line.Variables)
                .Distinct()
                .OrderByDescending(line => line.Name)
                .ToList();

            if (usedVariables.Any())
            {
                lines.Insert(0, new DockerLine("", DockerLineType.Text, EmptyVars));
                foreach (var variable in usedVariables)
                {
                    lines.Insert(0, new DockerLine($"ARG {variable.Name}='{variable.Value}'", DockerLineType.Text, new[] {variable}));
                }

                lines.Insert(0, new DockerLine("# Default arguments", DockerLineType.Comment, EmptyVars));
            }

            return lines;
        }

        private static string ApplyVariables([NotNull] string text, [NotNull] IEnumerable<DockerVariable> vars)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (vars == null)
            {
                throw new ArgumentNullException(nameof(vars));
            }

            return vars.Aggregate(text, (current, value) => current.Replace("${" + value.Name + "}", value.Value));
        }
    }
}
