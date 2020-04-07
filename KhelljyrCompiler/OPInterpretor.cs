using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCompiler.Containers;
using KhelljyrCompiler.Containers.Instructions;

namespace KhelljyrCompiler
{
    public partial class OPInterpretor
    {
        private Dictionary<string, Action<Compiler, string[]>> Fcts = new Dictionary<string, Action<Compiler, string[]>>
        {
            // Types Scalaires
            {"int", Int},
            //{"float", Float},

            // Declareurs
            {"fct", Function},
            {"lbl", Label},

            // Fonctions
            {"call", CallFct},
            {"ret", Ret},

            // Arithmétique
            {"set", Set},
            {"add", Add},

            // Branchements
            {"if", If},

            // Jumps
            {"jmp", Jump}

        };

        public void Treat(Compiler cmp, string[] args)
        {
            Fcts[args[0]](cmp, args);
        }

        public static void Int(Compiler cmp, string[] args)
        {
            int value = 0;
            Function fct = cmp.Functions.Last();
            IntVariable v = new IntVariable
            {
                Name = args[1],
            };

            if (args.Length == 3 && Int32.TryParse(args[2], out value))
            {
                v.HaveValue = true;
                v.Value = value;

                fct.Instructions.Insert(0, new SetInstruction
                {
                    Variable = v,
                    Value = v.Value
                });
            }
            else if (args.Length >= 3)
            {
                int count = 3;
                Function fctToCall = cmp.Functions.First(i => i.Name == args[2]);
                FunctionRetInstruction ret = new FunctionRetInstruction
                {
                    FunctionToCall = fctToCall
                };

                fct.Instructions.Add(ret);
                fct.Instructions.Add(new RetCarryInstruction(v));
                while (args.Length > count)
                {
                    Variable target = fct.Variables.FirstOrDefault(a => a.Name == args[count]);

                    if (target == null && Int32.TryParse(args[count], out value))
                    {
                        target = new ConstIntVariable(value);
                    }

                    ret.Variables.Add(target);
                    ++count;
                }
            }

            fct.AddVariable(v);
        }

        public static void Set(Compiler cmp, string[] args)
        {
            int value;
            Function fct = cmp.Functions.Last();
            Variable v = fct.Variables.First(a => a.Name == args[1]);
            
            if (Int32.TryParse(args[2], out value))
            {
                SetInstruction i = new SetInstruction
                {
                    Variable = v,
                    Value = value
                };

                fct.Instructions.Add(i);
            }
            else
            {
                int count = 3;
                Function target = cmp.Functions.First(n => n.Name == args[2]);
                FctCallInstruction fci = new FctCallInstruction(target);

                while (count < args.Length)
                {
                    Variable a = fct.Variables.FirstOrDefault(n => n.Name == args[count]);

                    if (a == null && Int32.TryParse(args[count], out value))
                        a = new ConstIntVariable(value);
                    
                    fci.Variables.Add(a);
                    ++count;
                }

                RetCarryInstruction rci = new RetCarryInstruction(v);

                fct.Instructions.Add(fci);
                fct.Instructions.Add(rci);
            }
        }

        public static void Add(Compiler cmp, string[] args)
        {
            Function fct = cmp.Functions.Last();

            Variable v1 = fct.Variables.First(a => a.Name == args[1]);
            Variable v2 = fct.Variables.FirstOrDefault(a => a.Name == args[2]);

            if (v2 == null)
            {
                int value = Int32.Parse(args[2]);

                v2 = new ConstIntVariable(value);
            }

            AddInstruction i = new AddInstruction
            {
                From = new Variable[]
                {
                    v1,
                    v2
                },
                To = fct.Variables.First(a => a.Name == args[3])
            };

            fct.Instructions.Add(i);
        }

        public static void Ret(Compiler cmp, string[] args)
        {
            int value = 0;
            Function fct = cmp.Functions.Last();
            Variable v = fct.Variables.FirstOrDefault(a => a.Name == args[1]);
            Instruction r = null;
            
            if (v != null)
            {
                r = new VariableRetInstruction(v);
            }
            else if(Int32.TryParse(args[1], out value))
            {
                v = new ConstIntVariable(value);
                r = new VariableRetInstruction(v);

                fct.Variables.Add(v);
            }
            else
            {
                int count = 2;

                FunctionRetInstruction ret = new FunctionRetInstruction
                {
                    FunctionToCall = cmp.Functions.First(i => i.Name == args[1])
                };

                while (args.Length > count)
                {
                    v = fct.Variables.FirstOrDefault(a => a.Name == args[count]);

                    if (v == null && Int32.TryParse(args[count], out value))
                    {
                        v = new ConstIntVariable(value);
                    }

                    ret.Variables.Add(v);
                    ++count;
                }

                r = ret;
            }

            fct.Instructions.Add(r);
        }

        public static void CallFct(Compiler cmp, string[] args)
        {
            int count = 2;
            int value = 0;
            Function fct = cmp.Functions.Last();
            Function fctToCall = cmp.Functions.First(i => i.Name == args[1]);
            FctCallInstruction c = new FctCallInstruction(fctToCall);
            
            while (args.Length > count)
            {
                Variable v = fct.Variables.FirstOrDefault(a => a.Name == args[count]);

                if (v == null && Int32.TryParse(args[count], out value))
                {
                    v = new ConstIntVariable(value);
                }

                c.Variables.Add(v);
                ++count;
            }

            fct.Instructions.Add(c);
        }

        public static void Label(Compiler cmp, string[] args)
        {
            Function fct = cmp.Functions.Last();
            Label lbl = new Label(args[1]);
            
            fct.Labels.Add(lbl);
            fct.Instructions.Add(lbl);
        }

        public static void Jump(Compiler cmp, string[] args)
        {
            Function fct = cmp.Functions.Last();

            fct.Instructions.Add(new JumpInstruction(args[1]));
        }
    }
}
