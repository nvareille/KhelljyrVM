using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon.OPCalls
{
    public static class Branchements
    {
        public static bool ConditionChecker(ConditionFlag flag, int a, int b)
        {
            bool result = false;

            switch (flag)
            {
                case ConditionFlag.Equals:
                    result = a == b;
                    break;

                case ConditionFlag.NotEquals:
                    result = a != b;
                    break;

                case ConditionFlag.Greater:
                    result = a > b;
                    break;

                case ConditionFlag.GreaterEquals:
                    result = a >= b;
                    break;

                case ConditionFlag.Lower:
                    result = a < b;
                    break;

                case ConditionFlag.LowerEquals:
                    result = a <= b;
                    break;
            }
            
            return (result);
        }

        public static int Ifn(Processor proc, ProgramReader reader)
        {
            ConditionFlag flag = (ConditionFlag)reader.NextInt();
            uint address = reader.NextPtr();

            int a = BitConverter.ToInt32(proc.Registers.ConditionRegisters[0]);
            int b = BitConverter.ToInt32(proc.Registers.ConditionRegisters[1]);

            bool result = !ConditionChecker(flag, a, b);

            if (result)
            {
                proc.ActiveStackContainer.ProgramCounter = (int)address;
                proc.Registers.JumpCarry = true;
                return (0);
            }

            return (reader.Elapsed());
        }

        public static int If(Processor proc, ProgramReader reader)
        {
            ConditionFlag flag = (ConditionFlag) reader.NextInt();
            uint address = reader.NextPtr();

            int a = BitConverter.ToInt32(proc.Registers.ConditionRegisters[0]);
            int b = BitConverter.ToInt32(proc.Registers.ConditionRegisters[1]);

            bool result = ConditionChecker(flag, a, b);

            if (result)
            {
                proc.ActiveStackContainer.ProgramCounter = (int)address;
                proc.Registers.JumpCarry = true;
                return (0);
            }

            return (reader.Elapsed());
        }
    }
}
