using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Cuboid : DDDObject
    {
        private double width;
        private double height;
        private double depth;

        public Cuboid() : base()
        {
            allowedAttributes.Add("width");
            allowedAttributes.Add("height");
            allowedAttributes.Add("depth");
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
                    case "height":
                        height = attr.Value;
                        break;
                    case "depth":
                        depth = attr.Value;
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
