using System;
using System.Collections.Generic;
using System.Text;

namespace KhelljyrCompiler.Containers
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
    }
}
