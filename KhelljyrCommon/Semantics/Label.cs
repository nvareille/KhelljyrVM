using KhelljyrCompiler;

namespace KhelljyrCommon.Semantics
{
    public class Label : Jumpable
    {
        public string Name;

        public Label(string name)
        {
            Name = name;
        }

        public override byte[] ByteOutput()
        {
            return (new byte[0]);
        }

        public override int LocalAddressToWrite()
        {
            return (0);
        }
    }
}
