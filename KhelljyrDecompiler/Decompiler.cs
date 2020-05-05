using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCompiler;

namespace KhelljyrDecompiler
{
    public class Byte : ArgumentType
    {
        public Byte() : base(Defines.SIZE_BYTE) { }
    }

    public class Int : ArgumentType
    {
        public Int() : base(Defines.SIZE_INT) { }
    }

    public class Ptr : ArgumentType
    {
        public Ptr() : base(Defines.SIZE_PTR) { }
    }

    public class Array : ArgumentType
    {
        public Array(int idx) : base(0, idx) { }
    }

    public class CharArray: ArgumentType
    {
        public CharArray(int idx) : base(0, idx) { }
    }


    public class ConditionFlag : ArgumentType
    {
        public ConditionFlag() : base(Defines.SIZE_INT) { }

        public string GetFlag(int id)
        {
            return (OPInterpretor.Flags.First(i => i.Value == (KhelljyrCommon.ConditionFlag)id).Key);
        }
    }

    public class TypeFlag : ArgumentType
    {
        public TypeFlag() : base(Defines.SIZE_INT) { }

        public string GetFlag(int id)
        {
            return (Enum.GetName(typeof(KhelljyrCommon.TypeFlag), id));
        }
    }

    public class TargetFlag : ArgumentType
    {
        public TargetFlag() : base(Defines.SIZE_BYTE) { }

        public string GetFlag(int id)
        {
            return (Enum.GetName(typeof(KhelljyrCommon.TargetFlag), id));
        }
    }

    public class ArgumentType
    {
        public int Size;
        public int Idx;

        public ArgumentType(int size, int idx) : this(size)
        {
            Size = -1;
            Idx = idx;
        }

        public ArgumentType(int size)
        {
            Size = size;
        }

        public bool IsStatic()
        {
            return (Size > 0);
        }

        public int GetSize()
        {
            return (Size);
        }
    }

    public class Decompiler
    {
        public ProgramReader Reader;
        public List<int> FuncsIdx = new List<int>();
        public Outputer Output = new Outputer();

        public static Dictionary<OPCodes.Codes, ArgumentType[]> Args = new Dictionary<OPCodes.Codes, ArgumentType[]>
        {
            {OPCodes.Codes.FctPrepare, new ArgumentType[] {new Int()}},
            {OPCodes.Codes.FctStart, new ArgumentType[] {}},
            {OPCodes.Codes.LibCall, new ArgumentType[] {new Int(), new Int(), new Int(), new CharArray(1), new CharArray(2) }},
            {OPCodes.Codes.Ret, new ArgumentType[] {new Ptr(), new Int(), }},
            {OPCodes.Codes.FctPop, new ArgumentType[] {}},
            {OPCodes.Codes.VarFctCopy, new ArgumentType[] {new Int(), new Ptr(), new Ptr()}},
            {OPCodes.Codes.VarConstCopy, new ArgumentType[] {new Int(), new Array(0), new Ptr() }},

            {OPCodes.Codes.Set, new ArgumentType[] {new Int(), new Array(0), new Ptr() }},

            {OPCodes.Codes.AssignStatic, new ArgumentType[] {new Ptr(), new Int(), new Array(1) }},
            {OPCodes.Codes.AssignToPointer, new ArgumentType[] {new Ptr(), new Int(), new Ptr() }},
            {OPCodes.Codes.AssignReturnCarry, new ArgumentType[] {new Ptr(), new Int() }},
            {OPCodes.Codes.SetReturnCarry, new ArgumentType[] {new Int(), new Array(0) }},
            {OPCodes.Codes.AssignPtrCarry, new ArgumentType[] {}},

            {OPCodes.Codes.AssignOperationRegister, new ArgumentType[] {new Int(), new Int(), new Ptr() }},
            {OPCodes.Codes.AssignConstOperationRegister, new ArgumentType[] {new Int(), new Int(), new Array(1) }},
            {OPCodes.Codes.AssignConditionRegister, new ArgumentType[] {new Int(), new Ptr() }},
            {OPCodes.Codes.AssignStaticConditionRegister, new ArgumentType[] {new Int(), new Int() }},
            {OPCodes.Codes.AssignTypeRegister, new ArgumentType[] {new Int(), new TypeFlag() }},
            {OPCodes.Codes.AssignTargetRegister, new ArgumentType[] {new Int(), new TargetFlag() }},

            {OPCodes.Codes.Cast, new ArgumentType[] {new TypeFlag(), new Ptr() }},
            {OPCodes.Codes.OperationAdd, new ArgumentType[] {new Int(), new Ptr() }},
            /*{OPCodes.Codes.OperationLess, new ArgumentType[] {new Ptr(), new Ptr(), new Ptr() }},
            {OPCodes.Codes.OperationLessConst, new ArgumentType[] {new Ptr(), new Int(), new Ptr() }},
            {OPCodes.Codes.OperationMultiply, new ArgumentType[] {new Ptr(), new Ptr(), new Ptr() }},
            {OPCodes.Codes.OperationMultiplyConst, new ArgumentType[] {new Ptr(), new Int(), new Ptr() }},
            {OPCodes.Codes.OperationDivide, new ArgumentType[] {new Ptr(), new Ptr(), new Ptr() }},
            {OPCodes.Codes.OperationDivideConst, new ArgumentType[] {new Ptr(), new Int(), new Ptr() }},
            {OPCodes.Codes.OperationModulus, new ArgumentType[] {new Ptr(), new Ptr(), new Ptr() }},
            {OPCodes.Codes.OperationModulusConst, new ArgumentType[] {new Ptr(), new Int(), new Ptr() }},*/

            {OPCodes.Codes.If, new ArgumentType[] {new ConditionFlag(), new Ptr()}},
            {OPCodes.Codes.Ifn, new ArgumentType[] {new ConditionFlag(), new Ptr()}},

            {OPCodes.Codes.Jump, new ArgumentType[] {new Ptr(), }},

            {OPCodes.Codes.Brk, new ArgumentType[] {}},
        };


        public void Decompile(byte[] program, string output)
        {
            Reader = new ProgramReader(program, 0);

            ReadFunctionTable();
            ReadSymbolsTable();
            ReadProgram();
            
            Output.WriteFile(output);
        }

        public void ReadSymbolsTable()
        {
            Output.AppendLine("\nSymbols:");

            while (Reader.GetCounter() < FuncsIdx[0] + (FuncsIdx.Count + 1) * Defines.SIZE_INT)
            {
                Output.AppendLine(Reader.NextString());
            }
        }

        public List<string> ExtractArgs(OPCodes.Codes code)
        {
            List<string> args = new List<string>();
            List<int> read = new List<int>();

            if (!Args.ContainsKey(code))
            {
                args.Add("Unknown OP code");
            }

            ArgumentType[] toExtract = Args.ContainsKey(code) ? Args[code] : new ArgumentType[] {};

            foreach (ArgumentType i in toExtract)
            {
                int size = 0;
                byte[] bytes = null;

                if (i.IsStatic())
                {
                    bytes = Reader.NextArray(i.Size);
                    size = i.Size;

                    try
                    {
                        read.Add(BitConverter.ToInt32(bytes));
                    }
                    catch (Exception e)
                    {
                        read.Add(-1);
                    }
                }
                else
                {
                    bytes = Reader.NextArray(read[i.Idx]);
                    size = read[i.Idx];
                    read.Add(size);
                }

               

                if (i is Int)
                    args.Add("INT[" + BitConverter.ToInt32(bytes) + "]");

                if (i is Ptr)
                    args.Add("PTR[" + BitConverter.ToInt32(bytes) + "]");

                if (i is Array)
                {
                    List<string> strs = new List<string>();

                    foreach (byte b in bytes)
                    {
                        strs.Add(b.ToString());    
                    }

                    args.Add("ARRAY[" + String.Join(", ", strs) + "]");
                }

                if (i is CharArray)
                {
                    List<byte> strs = new List<byte>();

                    foreach (byte b in bytes)
                    {
                        strs.Add(b);
                    }

                    args.Add("STRING[" + Encoding.ASCII.GetString(strs.ToArray()) + "]");
                }

                if (i is ConditionFlag)
                    args.Add(i.As<ConditionFlag>().GetFlag(BitConverter.ToInt32(bytes)));

                if (i is TypeFlag)
                    args.Add(i.As<TypeFlag>().GetFlag(BitConverter.ToInt32(bytes)));

                if (i is TargetFlag)
                    args.Add(i.As<TargetFlag>().GetFlag(bytes[0]));
            }

            return (args);
        }

        public void ReadProgram()
        {
            int fctId = 0;

            while (!Reader.IsOver())
            {
                int idx = Reader.GetCounter();
                int checkIndex = Reader.GetCounter() - (Defines.SIZE_INT * (FuncsIdx.Count + 1));

                if (FuncsIdx.Contains(checkIndex))
                    Output.OutputFunctionStart(idx - Defines.SIZE_INT * (FuncsIdx.Count + 1), fctId++, Reader.NextInt());
                else
                {
                    OPCodes.Codes code = (OPCodes.Codes)Reader.NextInt();
                    List<string> args = ExtractArgs(code);

                    Output.OutputInstruction(idx - Defines.SIZE_INT * (FuncsIdx.Count + 1), code, args);
                }
            }
        }

        public void ReadFunctionTable()
        {
            int count = 0;
            int nbrFct = Reader.NextInt();

            while (count < nbrFct)
            {
                FuncsIdx.Add(Reader.NextInt());

                ++count;
            }

            Output.FunctionTable(FuncsIdx);
        }
    }
}
