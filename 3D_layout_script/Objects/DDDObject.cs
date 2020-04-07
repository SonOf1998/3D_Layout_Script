using System.Collections.Generic;
using System.Linq;
using System.IO;
using _3D_layout_script.Attributes;
using _3D_layout_script.ObjExport;
using System;

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

        readonly string objTemplateFileName;

        protected HashSet<string> allowedAttributes;    // minden más attribútum
        protected HashSet<string> requiredAttributes;   // kötelezően megadandó attribútumok

        protected vec3         position = new vec3(0, 0, 0);    // default érték
        protected List<vec3>   rotationAxes;
        protected List<double> rotationAngles;

        private DDDObject()
        {
            allowedAttributes = new HashSet<string>();
            requiredAttributes = new HashSet<string>();
            rotationAngles = new List<double>();
            rotationAxes = new List<vec3>();

            requiredAttributes.Add("position");
            allowedAttributes.Add("rotation-axis");
            allowedAttributes.Add("rotation-angle");
        }

        public DDDObject(string objTemplateFileName) : this()
        {
            this.objTemplateFileName = "ObjTemplates/" + objTemplateFileName;
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

        // Eltolást végzi éppen az ObjFile ObjExportManagernek-ek való átadása előtt.
        // (eltolás - model transzformáció utolsó lépése)
        protected ObjFile TranslateWithPosition(ObjFile obj)
        {
            List<string> vertices = obj.Vertices;
            List<string> transformedVertices = new List<string>(vertices.Count);
           
            foreach (var vertex in vertices)
            {
                string[] splitVertex = vertex.Split(' ');

                double x = Double.Parse(splitVertex[1]);
                double y = Double.Parse(splitVertex[2]);
                double z = Double.Parse(splitVertex[3]);

                x += position.x;
                y += position.y;
                z += position.z;    // balkezes koordinátarendszer, ami beljebb annak nagyobb a Z koordinátája

                transformedVertices.Add($"{splitVertex[0]} {x} {y} {z}");
            }

            return new ObjFile(transformedVertices, obj.Normals, obj.Faces);
        }
        
        // ősosztály el tudja végezni a rotációt
        public virtual ObjFile GenerateStandaloneObj()
        {
            List<string> vertices = new List<string>();
            List<string> normals = new List<string>();
            List<string> faces = new List<string>();

            // RAII, Readonly megnyitása az .obj template-nek.
            using (StreamReader sr = new StreamReader(File.OpenRead(objTemplateFileName)))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains('v') && !line.Contains('#'))
                    {
                        if (rotationAngles.Count > 0)
                        {
                            string[] splitLine = line.Split(' ');
                    
                            double x = Double.Parse(splitLine[1]);
                            double y = Double.Parse(splitLine[2]);
                            double z = Double.Parse(splitLine[3]);

                            vec3 point = new vec3(x, y, z);

                            for (int i = 0; i < rotationAngles.Count; ++i)
                            {
                                point = Quaternion.Rotate(point, rotationAxes[i], rotationAngles[i]);
                            }

                            line = $"{splitLine[0]} {point.x} {point.y} {point.z}";
                        }
                                               
                        if (line.Contains('n'))
                        {
                            normals.Add(line);
                        }
                        else
                        {
                            vertices.Add(line);
                        }
                    }
                    else if (line.Contains('f') && !line.Contains('#'))
                    {
                        faces.Add(line);
                    }
                }
            }

            return new ObjFile(vertices, normals, faces);
        }
    }
}
