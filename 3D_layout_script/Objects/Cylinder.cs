using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Cylinder : DDDObject
    {
        private double radius = 0;
        private double height = 0;

        public Cylinder() : base()
        {
            requiredAttributes.Add("height");
            requiredAttributes.Add("radius");
        }

        public override bool SetAttributes(AttributeList attrList)
        {
            foreach (var attr in attrList)
            {
                switch (attr.Name)
                {
                    case "radius":
                        radius = attr.Value;
                        break;
                    case "height":
                        height = attr.Value;
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
