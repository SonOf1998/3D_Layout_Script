using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Cone : DDDObject
    {
        private double radius;
        private double height;

        public Cone() : base()
        {
            allowedAttributes.Add("height");
            allowedAttributes.Add("radius");
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
