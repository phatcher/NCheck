using System.Reflection;

using NUnit.Framework;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NCheck.Test")]
[assembly: AssemblyDescription("Tests for NCheck")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: Parallelizable(ParallelScope.Fixtures)]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif