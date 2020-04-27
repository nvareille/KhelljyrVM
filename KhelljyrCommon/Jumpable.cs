using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCompiler.Containers;

namespace KhelljyrCompiler
{
    public abstract class Jumpable : Instruction
    {
        public Label LabelContainer;
        public string Label;

        public void ComputeLabelPosition(Function fct)
        {
            LabelContainer = fct.Labels.First(i => i.Name == Label);
        }

        public abstract int LocalAddressToWrite();
        
    }
}
