using log4net.Config;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

[assembly: AssemblyVersion("1.0.1.0")]
[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyCompany("polylink")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCopyright("Copyright © polylink 2012")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyFileVersion("1.0.0.88")]
[assembly: AssemblyProduct("PLAgentBar")]
[assembly: AssemblyTitle("PLAgentBar")]
[assembly: AssemblyTrademark("")]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: ComVisible(true)]
[assembly: Guid("61f6856a-04d5-47a2-84d6-9fd3a60b88ed")]
//[assembly: AllowPartiallyTrustedCallers]
