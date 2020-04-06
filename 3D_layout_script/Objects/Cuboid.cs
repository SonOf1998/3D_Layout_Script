using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Cuboid : DDDObject
    {
        private double width = 0;
        private double height = 0;
        private double depth = 0;

        public Cuboid() : base()
        {
            requiredAttributes.Add("width");
            requiredAttributes.Add("height");
            requiredAttributes.Add("depth");
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
