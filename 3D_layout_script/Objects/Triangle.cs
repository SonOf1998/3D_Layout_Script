// TODO: csak egyenlő oldalú háromszögeket bír

using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Triangle : DDDObject
    {
        private double width = 0;

        public Triangle() : base()
        {
            requiredAttributes.Add("width");
        }

        public override bool SetAttributes(AttributeList attrList)
        {
            foreach (var attr in attrList)
            {
                switch (attr.Name)
                {
                    case "width":
                        width = attr.Value;
                        break;
                    case "default":
                        // ősosztály valósítja meg
                        break;
                }
            }

            return base.SetAttributes(attrList);
        }
    }
}
