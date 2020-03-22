namespace _3D_layout_script.Objects
{
    public class Cylinder : DDDObject
    {
        public Cylinder() : base()
        {
            allowedAttributes.Add("height");
            allowedAttributes.Add("radius");
        }
    }
}
