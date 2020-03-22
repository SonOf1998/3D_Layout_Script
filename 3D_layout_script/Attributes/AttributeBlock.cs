using System.Collections.Generic;

namespace _3D_layout_script.Attributes
{
    public class AttributeBlock
    {
        public string Name { get; private set; }
        private AttributeList attributes;

        public AttributeBlock(string blockName)
        {
            Name = blockName;
            attributes = new AttributeList();
        }

        public bool Add(Attribute attr)
        {
            return attributes.Add(attr);
        }

        public AttributeList GetAttributeList()
        {
            return attributes;
        }
    }
}
