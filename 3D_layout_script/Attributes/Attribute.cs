namespace _3D_layout_script.Attributes
{
    public class Attribute
    {
        public string Name { get; private set; }
        public dynamic Value { get; private set; }

        public Attribute(string attrName, dynamic attrValue)
        {
            Name = attrName;
            Value = attrValue;
        }
    }
}
