using System.Collections.Generic;
using System.Linq;
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

        protected HashSet<string> allowedAttributes;    // minden más attribútum
        protected HashSet<string> requiredAttributes;   // kötelezően megadandó attribútumok

        protected vec3         position = new vec3(0, 0, 0);    // default érték
        protected List<vec3>   rotationAxes;
        protected List<double> rotationAngles;

        public DDDObject()
        {
            allowedAttributes = new HashSet<string>();
            requiredAttributes = new HashSet<string>();
            rotationAngles = new List<double>();
            rotationAxes = new List<vec3>();

            requiredAttributes.Add("position");
            allowedAttributes.Add("rotation-axis");
            allowedAttributes.Add("rotation-angle");
        }

      

        public virtual bool SetAttributes(AttributeList attrList)
        {
            bool ret = true;    // Minden attribútum sikeresen hozzáadódott.

            var attrNameList = attrList.GetAttributeList().Select(attr => attr.Name);

            // ha az attribute list nem tartalmazza az összes required attribute-ot, akkor default értéket kell használnunk.
            var intersection = requiredAttributes.Intersect(attrNameList);
            if (intersection.Count() != requiredAttributes.Count())
            {
                ret = false;
            }

            foreach (var attr in attrList)
            {
                // Kötelező attribútumok hiánya mát kezelve van, ha olyan jön akkor nem érdekes az if blokk
                // Ha az adott attribútum nem része az opcionális attribútumok halmazának, az baj.
                if (!requiredAttributes.Contains(attr.Name) && !allowedAttributes.Contains(attr.Name))
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

            // kiírjuk a fejlesztőnek segítségül, hogy milyen attribútummokat használhat az objektumnál.
            // *-al jelöljük a kötelezőket.
            if (ret == false)
            {
                string wMsg = "";
                
                foreach (var attrName in requiredAttributes)
                {
                    wMsg += "*" + attrName + ", ";
                }

                allowedAttributes.ExceptWith(requiredAttributes);   // allowed attributes most már csak a nem kötelező, de használható attribútumokat mutatja.
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
