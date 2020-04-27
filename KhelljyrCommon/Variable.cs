using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;

namespace KhelljyrCompiler.Containers
{
    // TODO
    public abstract class Variable : Extractable
    {
        public string Name;
        public int Address;
        public int Size;
        public bool HaveValue;
        public TypeFlag Type;
        public TargetFlag Target;

        protected Variable(TypeFlag flag, TargetFlag target)
        {
            Type = flag;
            Target = target;
        }

        public void ComputeAddress(Function fct)
        {
            Address = fct.Variables.Sum(i => i.Size);
        }

        public virtual byte[] GetAddressOrValue()
        {
            return (BitConverter.GetBytes(Address));
        }
    }

    public abstract class Variable<T> : Variable
    {
        public T Value;

        protected Variable(TypeFlag flag, TargetFlag target) : base(flag, target) { }
    }

    public interface IConstVariable
    {
        byte[] GetValueAsBytes();
    }

    public abstract class ConstVariable<T> : Variable<T>, IConstVariable
    {
        protected ConstVariable(T value, TypeFlag flag) : base(flag, TargetFlag.Const)
        {
            HaveValue = true;
            Value = value;
        }

        public byte[] GetValueAsBytes()
        {
            if (Value is int)
                return (BitConverter.GetBytes((int)(object)Value));
            if (Value is float)
                return (BitConverter.GetBytes((float)(object)Value));

            return (null);
        }
    }

    public class IntVariable : Variable<int>
    {
        public IntVariable() : base(TypeFlag.Int, TargetFlag.Address)
        {
            Size = Defines.SIZE_INT;
        }
    }

    public class ConstIntVariable : ConstVariable<int>
    {
        public ConstIntVariable(int value) : base(value, TypeFlag.Int)
        {
            Size = Defines.SIZE_INT;
        }
    }

    public class FloatVariable : Variable<float>
    {
        public FloatVariable() : base(TypeFlag.Float, TargetFlag.Address)
        {
            Size = Defines.SIZE_FLOAT;
        }
    }

    public class ConstFloatVariable : ConstVariable<float>
    {
        public ConstFloatVariable(float value) : base(value, TypeFlag.Float)
        {
            Size = Defines.SIZE_FLOAT;
        }
    }

    public class PtrVariable : Variable<int>
    {
        public PtrVariable() : base(TypeFlag.Int, TargetFlag.Address)
        {
            Size = Defines.SIZE_PTR;
        }
    }

    public class ConstPtrVariable : ConstValue
    {
        public ConstPtrVariable(int value) : base(value)
        {
            Size = Defines.SIZE_PTR;
        }
    }

    public class DereferencedPointer : Variable<int>
    {
        public DereferencedPointer(int value) : base(TypeFlag.Int, TargetFlag.Ptr)
        {
            Address = value;
            Size = Defines.SIZE_PTR;
        }
    }

    public class ConstValue : Variable
    {
        public byte[] Value;

        public ConstValue(object value) : base(TypeFlag.Unknown, TargetFlag.Const)
        {
            if (value is float)
            {
                Value = BitConverter.GetBytes((float) value);
                Size = Defines.SIZE_FLOAT;
            }
            else if (value is int)
            {
                Value = BitConverter.GetBytes((int) value);
                Size = Defines.SIZE_INT;
            }
            else if (value is char)
            {
                Value = BitConverter.GetBytes((char) value);
                Size = Defines.SIZE_BYTE;
            }
        }

        public override byte[] GetAddressOrValue()
        {
            return (Value);
        }
    }
}
