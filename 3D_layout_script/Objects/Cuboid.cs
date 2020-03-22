namespace _3D_layout_script.Objects
{
    public class Cuboid : DDDObject
    {
        public Cuboid() : base()
        {
            allowedAttributes.Add("width");
            allowedAttributes.Add("height");
            allowedAttributes.Add("depth");
        }
    }
}
