using System.Collections.Generic;
using _3D_layout_script.Attributes;

namespace _3D_layout_script.Objects
{
    public abstract class DDDObject
    {
        private string warningMsg;
        public string WarningMsg
        {
            get
            {
                string copy = warningMsg;
                warningMsg = null;
                return copy;
            }
            set
            {
                warningMsg = value;
            }
        }

        protected HashSet<string> allowedAttributes;

        protected vec3         position;
        protected List<vec3>   rotationAxes;
        protected List<double> rotationAngles;

        public DDDObject()
        {
            allowedAttributes = new HashSet<string>();
            rotationAngles = new List<double>();
            rotationAxes = new List<vec3>();

            allowedAttributes.Add("position");
            allowedAttributes.Add("rotation-axis");
            allowedAttributes.Add("rotation-angle");
        }

        public virtual bool SetAttributes(AttributeList attrList)
        {
            bool ret = true;    // Minden attribútum sikeresen hozzáadódott.

            foreach (var attr in attrList)
            {
                if (!allowedAttributes.Contains(attr.Name))
                {
                    ret = false;
                }


                switch (attr.Name)
                {
                    case "position":
                        position = attr.Value;
                        break;
                    case "rotation-angle":
                        rotationAngles.Add(attr.Value);
                        break;
                    case "rotation-axis":
                        rotationAxes.Add(attr.Value);
                        break;
                    case "default":
                        // ősosztály valósítja meg
                        break;
                }
            }

            if (ret == false)
            {
                string wMsg = "";
                foreach (var attrName in allowedAttributes)
                {
                    wMsg += attrName + ", ";
                }
                wMsg = wMsg.Remove(wMsg.Length - 2);
                WarningMsg = wMsg;
            }

            return ret;
        }

        //public abstract void GENERATE() <--- TODO

        public virtual void GenerateStandaloneObj()
        {
            System.Console.WriteLine($"{rotationAxes.Count} {rotationAngles.Count}");
        }
    }
}
