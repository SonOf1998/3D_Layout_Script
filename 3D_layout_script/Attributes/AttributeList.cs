using System.Collections.Generic;

namespace _3D_layout_script.Attributes
{
    public class AttributeList
    {
        List<Attribute> list;

        public AttributeList()
        {
            list = new List<Attribute>();
        }

        public bool Add(Attribute attr)
        {
            if (!(attr.Name == "rotation-axis" || attr.Name == "rotation-angle"))
            {
                foreach (var elem in list)
                {
                    if (elem.Name == attr.Name)
                    {
                        list.Remove(elem);
                        list.Add(attr);
                        return false;
                    }
                }
            }

            list.Add(attr);
            return true;
        }

        public List<Attribute> GetAttributeList()
        {
            return list;
        }
    }
}
