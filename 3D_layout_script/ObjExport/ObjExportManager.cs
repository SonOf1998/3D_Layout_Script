using System;
using System.Collections.Generic;
using System.IO;
using _3D_layout_script.Objects;

namespace _3D_layout_script.ObjExport
{
    /* Obj fájl formátum:
     * 
     * v    előtaggal a vertexek felsorolása
     * vn   előtaggal a normálvektorok felsorolása
     * f    előtaggal az összekötött háromszögek    
     * 
     * f formátuma a mellékelt templatekben: (1//2 2//1 3//3) 
     * jelentése:
     * 
     * Az első, második és harmadik csúcs alkot egy háromszöget. 
     * Az első csúcshoz a 2.normálvektor tartozik, a másodikhoz az 1., a harmadikhoz a 3.
     * 
     * 
     */ 
    public class ObjExportManager : ExportManager
    {
        public ObjExportManager(string filename) : base(filename + ".obj") { }

        /* Feladata, hogy összemossa az összes kódban kreált 3D objektum .obj template fájlát egyetlen egy .obj fájllá.
         * 
         * Mindegyik 3D objektum rendelkezik egy saját .obj template-tel. Ebben az objektum az origóban áll, és minden mérete egység.
         * Először ezeket a megadott atrribútumok segítségével áttranszformáljuk.
         * 
         * Megnézzük, hogy a listában az első objektum mennyi vertex-szel és normálvektorral rendelkezik. 
         * Ez ezért fontos mert a rá következő objektum esetében az 'f' előtagos sorok indexelése innen kell, hogy kezdődjön.
         * 
         */ 
        public override void Export(List<DDDObject> objects)
        {
            if (objects.Count == 0)
            {
                return;
            }

            // Többszálúság ezen a ponton??? TODO

            ObjFile finalObj = objects[0].GenerateStandaloneObj();
            int vertIndex = finalObj.Vertices.Count;
            int normIndex = finalObj.Normals.Count;

            for (int i = 1; i < objects.Count; ++i)
            {
                DDDObject obj = objects[i];
                ObjFile objFile = obj.GenerateStandaloneObj();                
                List<string> faces = objFile.Faces;
                List<string> newFaces = new List<string>(faces.Count);

                foreach (string faceLine in faces)
                {
                    string face = faceLine + " ";   // hozzáteszünk egy space-t a végére így tudjuk, hogy mikor olvastuk végig az összes számot (foreach)
                    int[] VNTriangle = new int[6];  // V//N V//N V//N
                    int tmpNum = 0;
                    int indexToFill = 0;
                    foreach (char c in face)
                    {
                        if (Char.IsDigit(c))
                        {
                            tmpNum = tmpNum * 10 + int.Parse(c.ToString());
                        }
                        else
                        {
                            // ha nem számot olvastunk és még várunk számokat
                            if (tmpNum != 0 && indexToFill <= 5)
                            {
                                VNTriangle[indexToFill] = tmpNum;
                                tmpNum = 0;
                                indexToFill++;
                            }
                        }
                    }

                    for (int j = 0; j < 3; ++j)
                    {
                        VNTriangle[2 * j] += vertIndex;
                        VNTriangle[2 * j + 1] += normIndex;
                    }


                    string newFace = $"f {VNTriangle[0]}//{VNTriangle[1]} {VNTriangle[2]}//{VNTriangle[3]} {VNTriangle[4]}//{VNTriangle[5]}";
                    newFaces.Add(newFace);
                }

                finalObj.Vertices.AddRange(objFile.Vertices);
                finalObj.Normals.AddRange(objFile.Normals);
                finalObj.Faces.AddRange(newFaces);

                vertIndex += objFile.Vertices.Count;
                normIndex += objFile.Normals.Count;
            }


            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var vertex in finalObj.Vertices)
                {
                    sw.WriteLine(vertex);
                }

                foreach (var normal in finalObj.Normals)
                {
                    sw.WriteLine(normal);
                }

                foreach (var face in finalObj.Faces)
                {
                    sw.WriteLine(face);
                }
            }
        }
    }
}
