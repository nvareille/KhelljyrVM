using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KhelljyrCommon;
using KhelljyrCompiler;

namespace KhelljyrVM
{
    class Program
    {
        public static void StartCompiler(CompilationOptions o)
        {
            Compiler compiler = new Compiler();

            compiler.LibraryHandler.LoadLibraries(Directory.GetFiles("Libs/", "*Lib.dll"), false);
            compiler.AddFiles(o.Files.ToArray());
            compiler.Compile();

            byte[] program = compiler.BinaryOutput();

            File.WriteAllBytes(o.Output, program);
        }

        public static void StartVM(VMOptions o)
        {
            Processor p = new Processor();

            p.LibraryHandler.LoadLibraries(Directory.GetFiles("Libs/", "*Lib.dll"), true);
            byte[] program = File.ReadAllBytes(o.File);
            p.LoadProgram(program);
            p.Run();
        }

        public static IEnumerable<string> GetFiles(string[] args)
        {
            return (args.Where(i => !i.StartsWith('-')));
        }

        static void Main(string[] args)
        {
            FileVerificator.EnsureFolders();

            if (args.Contains("-c"))
            {
                CompilationOptions c = new CompilationOptions();

                c.Files = GetFiles(args);

                StartCompiler(c);
            }
            else
            {
                VMOptions o = new VMOptions
                {
                    File = args[0]
                };
                
                StartVM(o);
            }
        }
    }
}
