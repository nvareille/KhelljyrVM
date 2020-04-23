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
            Ret,
            FctPop,
            VarFctCopy,
            VarConstCopy,

            AssignStatic,
            AssignReturnCarry,
            SetReturnCarry,
            
            AssignOperationRegister,
            AssignConstOperationRegister,
            AssignConditionRegister,
            AssignStaticConditionRegister,
            AssignTypeRegister,

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
        }
    }
}
