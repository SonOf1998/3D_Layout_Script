using _3D_layout_script.Attributes;
using _3D_layout_script.ObjExport;
using System;
using System.Collections.Generic;

namespace _3D_layout_script.Objects
{
    public class Cylinder : DDDObject
    {
        private double radius = 0;
        private double height = 0;

        public Cylinder() : base("cylinder.obj")
        {
            requiredAttributes.Add("height");
            requiredAttributes.Add("radius");
        }

        public override bool SetAttributes(AttributeList attrList)
        {
            foreach (var attr in attrList)
            {
                switch (attr.Name)
                {
                    case "radius":
                        radius = attr.Value;
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

        public override ObjFile GenerateStandaloneObj()
        {
            ObjFile objFile = base.GenerateStandaloneObj();     // generated

            List<string> vertices = objFile.Vertices;
            List<string> newVertices = new List<string>(vertices.Count);

            foreach (var vertex in vertices)
            {
                string[] splitVertex = vertex.Split(' ');

                double x = Double.Parse(splitVertex[1]);
                double y = Double.Parse(splitVertex[2]);
                double z = Double.Parse(splitVertex[3]);

                x *= radius;
                y *= height;
                z *= radius;

                newVertices.Add($"{splitVertex[0]} {x} {y} {z}");
            }

            objFile = new ObjFile(newVertices, objFile.Normals, objFile.Faces);     // scaled
            objFile = RotateByAxisAnglePair(objFile);                               // rotated
            return TranslateWithPosition(objFile);                                  // translated
        }
    }
}
