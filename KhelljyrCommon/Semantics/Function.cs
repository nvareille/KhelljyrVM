using System.Collections.Generic;
using System.Linq;
using KhelljyrCompiler.Containers;

namespace KhelljyrCommon.Semantics
{
    public class Function
    {
        public int Id;
        public string Name;
        public int Prio;
        public int ActualAddress = 0;
        public List<Variable> Variables = new List<Variable>();
        public List<Instruction> Instructions = new List<Instruction>();
        public List<Label> Labels = new List<Label>();

        public void AddVariable(Variable v)
        {
            v.ComputeAddress(this);
            Variables.Add(v);
        }

        public Variable GetVariable(string name)
        {
            bool isPtr = name.StartsWith("&");
            bool mustDeref = name.StartsWith("*");

            name = name.Replace("&", "");
            name = name.Replace("*", "");
            Variable v = Variables.FirstOrDefault(a => a.Name == name);

            if (v == null)
                return (null);
            if (mustDeref)
                return (new DereferencedPointer(v.Address));
            if (!isPtr)
                return (v);
            
            return (new ConstPtrVariable(v.Address));
        }
    }
}
