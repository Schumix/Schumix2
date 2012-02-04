using System;

using Schumix.API;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Schumix.PythonCompilerAddon
{
    public class PythonCompiler : ISchumixAddon
    {
        private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
        private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;

        private ScriptEngine pyEngine = null;
        private ScriptRuntime pyRuntime = null;
        private ScriptScope pyScope = null;
        //private SimpleLogger _logger = new SimpleLogger();

        public void Setup()
        {
            CommandManager.AdminCRegisterHandler("PythonCompiler", HandleCommand);
        }

        public void Destroy()
        {
            CommandManager.AdminCRemoveHandler("PythonCompiler", HandleCommand);
        }

        public bool Reload(string RName)
        {
            return false;
        }

        protected void HandleCommand(IRCMessage sIRCMessage)
        {
            if (sIRCMessage.Info.Length >= 5 && sIRCMessage.Info[4] == "[")
            {
                string script = "if __name__ == '__main__':\n\t";
                for (int i = 5; i < sIRCMessage.Info.Length; i++)
                    script += sIRCMessage.Info[i] + " ";

                CreatePyRuntime();
                CompileSourceAndExecute(script);
            }
            else
                sSendMessage.SendChatMessage(sIRCMessage, 
                    sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel));
        }

        private void CreatePyRuntime()
        {
            if (pyEngine == null)
            {
                pyEngine = Python.CreateEngine();
                pyScope = pyEngine.CreateScope();
                //pyScope.SetVariable("log", _logger);
                //_logger.AddInfo("Python Initialized");
            }   
        }

        private void CompileSourceAndExecute(string script)
        {
            ScriptSource source = pyEngine.CreateScriptSourceFromString(script, 
                SourceCodeKind.Statements);

            CompiledCode compiled = source.Compile();

            // Executes in the scope of Python
            compiled.Execute(pyScope);
        }

        /// <summary>
        /// Name of the addon
        /// </summary>
        public string Name
        {
            get { return "PythonCompilerAddon (Python 2.7)"; }
        }

        /// <summary>
        /// Author of the addon.
        /// </summary>
        public string Author
        {
            get { return "Jackneill"; }
        }

        /// <summary>
        /// Website where the addon is available.
        /// </summary>
        public string Website
        {
            get { return "https://github.com/Jackneill/Schumix2"; }
        }
    }
}
