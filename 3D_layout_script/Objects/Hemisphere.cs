using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Hemisphere : DDDObject
    {
        private double radius;

        public Hemisphere() : base()
        {
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
                    case "default":
                        // ősosztály valósítja meg
                        break;
                }
            }

            return base.SetAttributes(attrList);
        }
    }
}
