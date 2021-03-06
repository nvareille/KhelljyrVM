﻿using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon.MemoryOperators;
using Microsoft.VisualBasic.CompilerServices;

namespace KhelljyrCommon.OPCalls
{
    public class Arithmetics
    {
        private static bool Comparer(Processor proc, TypeFlag f)
        {
            return (proc.Registers.TypeRegisters[0] == f ||
                    proc.Registers.TypeRegisters[1] == f);
        }

        public static TypeFlag GetHigherFlag(Processor proc)
        {
            TypeFlag f = TypeFlag.Char;

            if (Comparer(proc, TypeFlag.Int))
                f = TypeFlag.Int;

            if (Comparer(proc, TypeFlag.Float))
                f = TypeFlag.Float;

            return (f);
        }

        public static object BytesToNative(TypeFlag f, byte[] bytes)
        {
            switch (f)
            {
                case TypeFlag.Int:
                    return BitConverter.ToInt32(bytes);
                case TypeFlag.Float:
                    return BitConverter.ToSingle(bytes);
            }
            throw new Exception("Unknown Type");
        }

        public static byte[] Convert(object o, TypeFlag f)
        {
            byte[] b = null;

            if (f == TypeFlag.Char)
            {
                o = Conversions.ToChar(o);
                b = BitConverter.GetBytes((char)o);
            }
            else if (f == TypeFlag.Int)
            {
                o = Conversions.ToInteger(o);
                b = BitConverter.GetBytes((int)o);
            }
            else if (f == TypeFlag.Float)
            {
                o = Conversions.ToSingle(o);
                b = BitConverter.GetBytes((float)o);
            }

            return (b);
        }

        public static int Cast(Processor proc, ProgramReader reader)
        {
            TypeFlag returnType = (TypeFlag)reader.NextInt();
            uint addr = reader.NextPtr();

            object v1 = BytesToNative(proc.Registers.TypeRegisters[0], proc.Registers.OperationRegisters[0]);
            byte[] b = null;

            switch (returnType)
            {
                case TypeFlag.Char:
                    v1 = Conversions.ToChar(v1);
                    break;

                case TypeFlag.Int:
                    v1 = Conversions.ToInteger(v1);
                    break;

                case TypeFlag.Float:
                    v1 = Conversions.ToSingle(v1);
                    break;
            }

            b = Convert(v1, returnType);

            MemoryWriter w = MemoryWriter.GetWriter(2, proc, addr);

            w.Write(b);

            return (reader.Elapsed());
        }

        public static int Add(Processor proc, ProgramReader reader)
        {
            TypeFlag returnType = (TypeFlag)reader.NextInt();
            uint addr = reader.NextPtr();
            TypeFlag computeType = GetHigherFlag(proc);

            object v1 = BytesToNative(proc.Registers.TypeRegisters[0], proc.Registers.OperationRegisters[0]);
            object v2 = BytesToNative(proc.Registers.TypeRegisters[1], proc.Registers.OperationRegisters[1]);
            byte[] b = null;

            switch (computeType)
            {
                case TypeFlag.Char:
                    v1 = Conversions.ToChar(v1) + Conversions.ToChar(v2);
                    break;

                case TypeFlag.Int:
                    v1 = Conversions.ToInteger(v1) + Conversions.ToInteger(v2);
                    break;

                case TypeFlag.Float:
                    v1 = Conversions.ToSingle(v1) + Conversions.ToSingle(v2);
                    break;
            }

            b = Convert(v1, returnType);

            MemoryWriter w = MemoryWriter.GetWriter(2, proc, addr);

            w.Write(b);

            return (reader.Elapsed());
        }

        public static int Less(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }

        public static int LessConst(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }

        public static int Multiply(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }

        public static int MultiplyConst(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }

        public static int Divide(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }

        public static int DivideConst(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }

        public static int Modulus(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }

        public static int ModulusConst(Processor proc, ProgramReader reader)
        {
            throw new NotImplementedException();
            return (reader.Elapsed());
        }
    }
}
