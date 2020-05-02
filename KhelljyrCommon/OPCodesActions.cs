using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public class OPCodesActions
    {
        public static Dictionary<OPCodes.Codes, Func<Processor, ProgramReader, int>> Actions = new Dictionary<OPCodes.Codes, Func<Processor, ProgramReader, int>>()
        {
            {OPCodes.Codes.FctPrepare, OPCalls.Functions.FunctionCall},
            {OPCodes.Codes.FctStart, OPCalls.Functions.FunctionStart},
            {OPCodes.Codes.LibCall, OPCalls.Functions.LibCall},
            {OPCodes.Codes.Ret, OPCalls.Functions.Return},
            {OPCodes.Codes.FctPop, OPCalls.Functions.FunctionPop},
            {OPCodes.Codes.VarFctCopy, OPCalls.Functions.VarFctCopy},
            {OPCodes.Codes.VarConstCopy, OPCalls.Functions.VarConstCopy},

            {OPCodes.Codes.Set, OPCalls.Memset.Set},

            {OPCodes.Codes.AssignStatic, OPCalls.Memset.AssignStatic},
            {OPCodes.Codes.AssignToPointer, OPCalls.Memset.AssignToPointer},
            {OPCodes.Codes.AssignReturnCarry, OPCalls.Memset.AssignReturnCarry},
            {OPCodes.Codes.SetReturnCarry, OPCalls.Memset.SetReturnCarry},
            {OPCodes.Codes.AssignPtrCarry, OPCalls.Memset.AssignPtrCarry},
            
            {OPCodes.Codes.AssignOperationRegister, OPCalls.Memset.AssignOperationRegister},
            {OPCodes.Codes.AssignConstOperationRegister, OPCalls.Memset.AssignConstOperationRegister},
            {OPCodes.Codes.AssignConditionRegister, OPCalls.Memset.AssignConditionRegister},
            {OPCodes.Codes.AssignStaticConditionRegister, OPCalls.Memset.AssignStaticConditionRegister},
            {OPCodes.Codes.AssignTypeRegister, OPCalls.Memset.AssignTypeRegister},
            {OPCodes.Codes.AssignTargetRegister, OPCalls.Memset.AssignTargetRegister},

            {OPCodes.Codes.Cast, OPCalls.Arithmetics.Cast},
            {OPCodes.Codes.OperationAdd, OPCalls.Arithmetics.Add},
            /*{OPCodes.Codes.OperationLess, OPCalls.Arithmetics.Less},
            {OPCodes.Codes.OperationLessConst, OPCalls.Arithmetics.LessConst},
            {OPCodes.Codes.OperationMultiply, OPCalls.Arithmetics.Multiply},
            {OPCodes.Codes.OperationMultiplyConst, OPCalls.Arithmetics.MultiplyConst},
            {OPCodes.Codes.OperationDivide, OPCalls.Arithmetics.Divide},
            {OPCodes.Codes.OperationDivideConst, OPCalls.Arithmetics.DivideConst},
            {OPCodes.Codes.OperationModulus, OPCalls.Arithmetics.Modulus},
            {OPCodes.Codes.OperationModulusConst, OPCalls.Arithmetics.ModulusConst},*/

            {OPCodes.Codes.If, OPCalls.Branchements.If},
            {OPCodes.Codes.Ifn, OPCalls.Branchements.Ifn},

            {OPCodes.Codes.Jump, OPCalls.Jumps.Jump},

            {OPCodes.Codes.Exit, OPCalls.SysCalls.Exit},
            {OPCodes.Codes.Brk, OPCalls.SysCalls.Brk}
        };
    }
}
