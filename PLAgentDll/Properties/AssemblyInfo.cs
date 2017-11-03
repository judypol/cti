using log4net.Config;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("3.0.1.16")]
[assembly: XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyCompany("polylink")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCopyright("Copyright © polylink 2012")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyFileVersion("3.0.1.16")]
//[assembly: AssemblyKeyFile("AgentCom.snk")]
[assembly: AssemblyProduct("PLAgentDll")]
[assembly: AssemblyTitle("PLAgentDll")]
[assembly: AssemblyTrademark("")]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: ComVisible(true)]
[assembly: Guid("cfa722d6-62d1-49ba-bd5e-63f8ddc65b26")]
