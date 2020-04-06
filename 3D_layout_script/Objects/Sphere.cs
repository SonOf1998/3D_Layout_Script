using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Sphere : DDDObject
    {
        private double radius = 0;

        public Sphere() : base()
        {
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
                    case "default":
                        // ősosztály valósítja meg
                        break;
                }
            }

            return base.SetAttributes(attrList);
        }
    }
}
