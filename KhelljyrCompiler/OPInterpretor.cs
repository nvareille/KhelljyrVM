using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KhelljyrCommon;
using KhelljyrCommon.Libraries;
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
            {"float", Float},
            {"ptr", Ptr},

            // Declareurs
            {"fct", Function},
            {"lbl", Label},

            // Fonctions
            {"call", CallFct},
            {"ret", Ret},

            // Arithmétique
            {"set", Set},
            {"cast", Cast},
            {"add", Add},

            // Branchements
            {"if", If},

            // Jumps
            {"jmp", Jump},

            {"brk", BasicInstruction(new GenericInstruction(OPCodes.Codes.Brk))}

        };

        public void Treat(Compiler cmp, string[] args)
        {
            Fcts[args[0]](cmp, args);
        }

        private static Action<Compiler, string[]> BasicInstruction(Instruction ins)
        {
            Action<Compiler, string[]> act = (cmp, args) =>
            {
                Function fct = cmp.Functions.Last();

                fct.Instructions.Add(ins);
            };

            return (act);
        }

        private static void Ptr(Compiler cmp, string[] args)
        {
            int value = 0;
            Function fct = cmp.Functions.Last();
            PtrVariable v = new PtrVariable
            {
                Name = args[1],
            };

            if (args.Length == 3)
            {
                Variable assign = fct.GetVariable(args[2]);

                fct.Instructions.Insert(0, new NewSetInstruction
                {
                    Destination = v,
                    Source = assign
                });
            }

            fct.AddVariable(v);
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

                fct.Instructions.Insert(0, new NewSetInstruction
                {
                    Destination = v,
                    Source = new ConstValue(v.Value)
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

        public static void Float(Compiler cmp, string[] args)
        {
            float value = 0;
            Function fct = cmp.Functions.Last();
            FloatVariable v = new FloatVariable
            {
                Name = args[1],
            };

            if (args.Length == 3 && Single.TryParse(args[2], out value))
            {
                v.HaveValue = true;
                v.Value = value;

                fct.Instructions.Insert(0, new NewSetInstruction
                {
                    Destination = v,
                    Source = new ConstValue(v.Value)
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

                    if (target == null && Single.TryParse(args[count], out value))
                    {
                        target = new ConstFloatVariable(value);
                    }

                    ret.Variables.Add(target);
                    ++count;
                }
            }

            fct.AddVariable(v);
        }

        private static bool TryPtrSet(Function fct, Variable v, string stringValue)
        {
            Variable ptr = fct.GetVariable(stringValue);

            if (v is DereferencedPointer)
            {
                NewSetInstruction ins = new NewSetInstruction
                {
                    Destination = v,
                    Source = ptr
                };

                fct.Instructions.Add(ins);

                return (true);
            }

            if (ptr is ConstPtrVariable)
            {
                NewSetInstruction ins = new NewSetInstruction
                {
                    Destination = v,
                    Source = ptr
                };

                fct.Instructions.Add(ins);

                return (true);
            }

            return (false);
        }

        private static bool TryConstSet(Function fct, Variable v, string stringValue)
        {
            int i = 0;
            float f = 0;

            if (v.Type == TypeFlag.Float && Single.TryParse(stringValue, out f))
            {
                NewSetInstruction ins = new NewSetInstruction
                {
                    Destination = v,
                    Source = new ConstValue(f)
                };

                fct.Instructions.Add(ins);

                return (true);
            }
            if (v.Type == TypeFlag.Int && Int32.TryParse(stringValue, out i))
            {
                NewSetInstruction ins = new NewSetInstruction
                {
                    Destination = v,
                    Source = new ConstValue(i)
                };

                fct.Instructions.Add(ins);

                return (true);
            }

            return (false);
        }

        public static void Set(Compiler cmp, string[] args)
        {
            int value;
            Function fct = cmp.Functions.Last();
            Variable v = fct.GetVariable(args[1]);

            if (TryConstSet(fct, v, args[2]))
            {

            }
            else if (TryPtrSet(fct, v, args[2]))
            {

            }

/* TODO Casts && Copy
 else if ()
            {

            }*/
            else
            {
                int count = 3;
                int startIdx = 2;
                FctCallInstruction fci = FctCallInstruction.GetCallInstruction(cmp, args, ref startIdx);

                count = startIdx + 1;
                while (count < args.Length)
                {
                    Variable a = fct.GetVariable(args[count]);

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

        private static Variable TryConst(string stringValue)
        {
            Variable v = null;

            if (Int32.TryParse(stringValue, out int ivalue))
                v = new ConstIntVariable(ivalue);
            else if (Single.TryParse(stringValue, out float fvalue))
                v = new ConstFloatVariable(fvalue);
            
            return (v);
        }

        public static void Add(Compiler cmp, string[] args)
        {
            Function fct = cmp.Functions.Last();

            Variable v1 = fct.GetVariable(args[1]);
            Variable v2 = fct.GetVariable(args[2]) ?? TryConst(args[2]);

            AddInstruction i = new AddInstruction
            {
                From = new Variable[]
                {
                    v1,
                    v2
                },
                To = fct.GetVariable(args[3])
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
            int count = 0;
            int startIdx = 1;
            int value = 0;
            Function fct = cmp.Functions.Last();
            FctCallInstruction c = FctCallInstruction.GetCallInstruction(cmp, args, ref startIdx);

            count = startIdx + 1;
            c.ExtractVariables(fct, args, count);
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
