// TODO: csak egyenlő oldalú háromszögeket bír

namespace _3D_layout_script.Objects
{
    public class Triangle : DDDObject
    {
        public Triangle() : base()
        {
            allowedAttributes.Add("width");
        }
    }
}
