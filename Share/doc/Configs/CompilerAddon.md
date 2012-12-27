# CompilerAddon konfig fájl

## Compiler

* **Enabled:** Értéke `true` vagy `false` lehet. Meghatározza hogy a fordító rész be legyen-e kapcsolva. Alapértelmezés: `true`
* **MasterChannel:**
    * **Enabled:** Értéke `true` vagy `false` lehet. Meghatározza hogy legyen-e maximális memóriafogyasztás amelynél kikapcsolja a program a fordító részt. 
                   Alapértelmezés: `true`
    * **Memory:** Maximális memóriafogyasztás `mb`-ban megadva. Alapértelmezés: `50`
* **CompilerOptions:** Fordító program beállítása. Alapértelmezés: `/optimize`
* **WarningLevel:** Fordító program beállítása. Alapértelmezés: `4`
* **TreatWarningsAsErrors:** Értéke `true` vagy `false` lehet. Fordító program beállítása. Alapértelmezés: `false`
* **Referenced:** Referenciák adhatok meg vele a fordításhoz. Alapértelmezés: `using System; using System.Threading; using System.Reflection; using System.Threading.Tasks; using System.Linq; using System.Collections.Generic; using System.Text; using System.Text.RegularExpressions; using System.Numerics; using Schumix.Libraries; using Schumix.Libraries.Mathematics; using Schumix.Libraries.Mathematics.Types;`
* **ReferencedAssemblies:** Referencia fájlok adhatok meg vele a fordításhoz. Alapértelmezés: `System.dll,System.Core.dll,System.Numerics.dll,Schumix.Libraries.dll`
* **MainClass:** Alapértelmezett fő osztály. Alapértelmezés: `Entry`
* **MainClass:** Alapértelmezett fő függvény. Alapértelmezés: `Schumix`
