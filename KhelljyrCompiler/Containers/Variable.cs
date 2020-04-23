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

        protected Variable(TypeFlag flag)
        {
            Type = flag;
        }

        public void ComputeAddress(Function fct)
        {
            Address = fct.Variables.Sum(i => i.Size);
        }
    }

    public abstract class Variable<T> : Variable
    {
        public T Value;

        protected Variable(TypeFlag flag) : base(flag) { }
    }

    public interface IConstVariable
    {
        byte[] GetValueAsBytes();
    }

    public abstract class ConstVariable<T> : Variable<T>, IConstVariable
    {
        protected ConstVariable(T value, TypeFlag flag) : base(flag)
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
        public IntVariable() : base(TypeFlag.Int)
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
        public FloatVariable() : base(TypeFlag.Float)
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
}
