// <copyright file="GlobalSuppressions.cs" company="Ken Watson">
// Copyright (c) Ken Watson. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1413:Use trailing comma in multi-line initializers", Justification = "This is a dumb rule", Scope = "module")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Do not need documentation right now", Scope = "module")]
[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "Need to pass URIs as strings", Scope = "module")]
[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Not true for Razor Pages models", Scope = "type", Target = "~T:DexcomApp.Pages.Dexcom.DataRangeModel")]
