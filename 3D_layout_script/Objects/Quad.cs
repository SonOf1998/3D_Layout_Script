using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public class Quad : DDDObject
    {
        private double width;
        private double height;

        public Quad() : base()
        {
            allowedAttributes.Add("width");
            allowedAttributes.Add("height");
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
                    case "default":
                        // ősosztály valósítja meg
                        break;
                }
            }

            return base.SetAttributes(attrList);
        }
    }
}
