﻿using System;
using System.Collections.Generic;
using System.Text;
using KhelljyrCommon.MemoryOperators;

namespace KhelljyrCommon.OPCalls
{
    public static class Memset
    {
        public static int AssignStatic(Processor proc, ProgramReader reader)
        {
            uint ptr = reader.NextPtr();
            int size = reader.NextInt();
            byte[] toCopy = reader.NextArray(size);

            if (proc.Registers.PtrCarry)
            {
                int a = BitConverter.ToInt32(toCopy) + proc.ActiveStackContainer.Memory.Start;

                toCopy = BitConverter.GetBytes(a);
                proc.Registers.PtrCarry = false;
            }

            Array.Copy(toCopy, 0, proc.ActiveStackContainer.Memory.Memory, ptr, size);

            return (reader.Elapsed());
        }

        public static int AssignToPointer(Processor proc, ProgramReader reader)
        {
            uint source = reader.NextPtr();
            int size = reader.NextInt();
            uint dest = reader.NextPtr();
            byte[] toCopy = new byte[size];

            dest = proc.MMU.ReadPtr(dest);

            Array.Copy(proc.ActiveStackContainer.Memory.Memory, source, toCopy, 0, size);

            proc.MMU.WriteBytes(toCopy, dest);

            return (reader.Elapsed());
        }

        public static int AssignReturnCarry(Processor proc, ProgramReader reader)
        {
            uint ptr = reader.NextPtr();
            int size = reader.NextInt();

            Array.Copy(proc.Registers.ReturnCarry, 0, proc.ActiveStackContainer.Memory.Memory, ptr, size);

            return (reader.Elapsed());
        }
        
        public static int SetReturnCarry(Processor proc, ProgramReader reader)
        {
            int size = reader.NextInt();
            byte[] value = reader.NextArray(size);
            
            Array.Copy(value, 0, proc.Registers.ReturnCarry, 0, size);

            return (reader.Elapsed());
        }

        public static int AssignOperationRegister(Processor proc, ProgramReader reader)
        {
            int id = reader.NextInt();
            int size = reader.NextInt();
            byte[] ptr = reader.NextArray(Defines.SIZE_PTR);
            byte[] reg = new byte[size];

            MemoryReader r = MemoryReader.GetReader(id, proc, ptr, Defines.SIZE_PTR);

            Array.Copy(r.Data, 0, reg, 0, size);

            proc.Registers.OperationRegisters[id] = reg;

            return (reader.Elapsed());
        }

        public static int AssignConstOperationRegister(Processor proc, ProgramReader reader)
        {
            int id = reader.NextInt();
            int size = reader.NextInt();
            byte[] value = reader.NextArray(size);

            proc.Registers.OperationRegisters[id] = value;

            return (reader.Elapsed());
        }

        public static int AssignConditionRegister(Processor proc, ProgramReader reader)
        {
            int id = reader.NextInt();
            uint ptr = reader.NextPtr();

            Array.Copy(proc.ActiveStackContainer.Memory.Memory, ptr, proc.Registers.ConditionRegisters[id], 0, Defines.SIZE_INT);

            return (reader.Elapsed());
        }

        public static int AssignStaticConditionRegister(Processor proc, ProgramReader reader)
        {
            int id = reader.NextInt();
            byte[] value = reader.NextArray(Defines.SIZE_INT);

            Array.Copy(value, 0, proc.Registers.ConditionRegisters[id], 0, Defines.SIZE_INT);

            return (reader.Elapsed());
        }

        public static int AssignTypeRegister(Processor proc, ProgramReader reader)
        {
            int id = reader.NextInt();
            int flag = reader.NextInt();

            proc.Registers.TypeRegisters[id] = (TypeFlag) flag;
            return (reader.Elapsed());
        }

        public static int AssignTargetRegister(Processor proc, ProgramReader reader)
        {
            int id = reader.NextInt();
            byte flag = reader.NextByte();

            proc.Registers.TargetRegisters[id] = (TargetFlag)flag;
            return (reader.Elapsed());
        }

        public static int AssignPtrCarry(Processor proc, ProgramReader reader)
        {
            proc.Registers.PtrCarry = true;

            return (reader.Elapsed());
        }

        public static int Set(Processor proc, ProgramReader reader)
        {
            int sourceSize = reader.NextInt();
            byte[] source = reader.NextArray(sourceSize);
            uint dest = reader.NextPtr();
            
            MemoryReader r = MemoryReader.GetReader(0, proc, source, sourceSize);
            MemoryWriter w = MemoryWriter.GetWriter(1, proc, dest);
            
            w.Write(r.Data);
            
            return (reader.Elapsed());
        }
    }
}
