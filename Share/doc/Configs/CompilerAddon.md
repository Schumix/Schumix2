# CompilerAddon config file

## Compiler

* **Enabled:** `true` or `false`. Means compiler addon is enabled or not. Default: `true`
* **MasterChannel:**
    * **Enabled:** `true` or `false`. Should it be a max memory allocation limit that after it's reach the bot disables the compiler addon. 
                   Default: `true`
    * **Memory:** Max memory allocation in `mb`. Default: `50`.
* **CompilerOptions:** Compiler program's setting. Default: `/optimize`
* **WarningLevel:** Compiler program's setting. Default: `4`
* **TreatWarningsAsErrors:** `true`or `false`. Compiler program's setting. Default: `false`
* **Referenced:** References can be provided for the compilation. Default: `using System; using System.Threading; using System.Reflection; using System.Threading.Tasks; using System.Linq; using System.Collections.Generic; using System.Text; using System.Text.RegularExpressions; using System.Numerics; using Schumix.Libraries; using Schumix.Libraries.Mathematics; using Schumix.Libraries.Mathematics.Types;`
* **ReferencedAssemblies:** Rerefence files can be provided for the compilation. Default: `System.dll,System.Core.dll,System.Numerics.dll,Schumix.Libraries.dll`
* **MainClass:** Default main class. Default: `Entry`
* **MainClass:** Default main function. Default: `Schumix`
