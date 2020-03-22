using System.Collections.Generic;

namespace _3D_layout_script.Objects
{
    public abstract class DDDObject
    {
        protected HashSet<string> allowedAttributes;

        public vec3         Position { get; set; }
        public List<vec3>   RotationAxes;
        public List<double> RotationAngles;

        public DDDObject()
        {
            allowedAttributes.Add("position");
            allowedAttributes.Add("rotation-axis");
            allowedAttributes.Add("rotation-angles");
        }

        // public abstract ... GENERATE() <--- TODO
    }
}
