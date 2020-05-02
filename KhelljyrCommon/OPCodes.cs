using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCommon
{
    public class OPCodes
    {
        public enum Codes : int
        {
            FctPrepare = 0,
            FctStart,
            LibCall,
            Ret,
            FctPop,
            VarFctCopy,
            VarConstCopy,

            Set,

            AssignStatic,
            AssignToPointer,
            AssignReturnCarry,
            SetReturnCarry,
            AssignPtrCarry,

            AssignOperationRegister,
            AssignConstOperationRegister,
            AssignConditionRegister,
            AssignStaticConditionRegister,
            AssignTypeRegister,
            AssignTargetRegister,

            Cast,
            OperationAdd,
            /*OperationLess,
            OperationLessConst,
            OperationMultiply,
            OperationMultiplyConst,
            OperationDivide,
            OperationDivideConst,
            OperationModulus,
            OperationModulusConst,*/

            If,
            Ifn,

            Jump,

            Exit,
            Brk,
        }
    }
}
