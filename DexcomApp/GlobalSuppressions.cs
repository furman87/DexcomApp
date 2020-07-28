// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1413:Use trailing comma in multi-line initializers", Justification = "This is a dumb rule", Scope = "module")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Do not need documentation right now", Scope = "module")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Need to pass URIs as strings", Scope = "module")]
