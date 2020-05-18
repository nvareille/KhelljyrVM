namespace KhelljyrCommon.Semantics
{
    public abstract class Instruction
    {
        public ByteContainer Bytes = new ByteContainer();
        public Function Function;
        public int InstructionPtr;

        public abstract byte[] ByteOutput();

        public byte[] ByteOutputInternal(int idx)
        {
            InstructionPtr = idx;

            return (ByteOutput());
        }
    }
}
