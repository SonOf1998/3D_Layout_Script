using System.Collections.Generic;

namespace _3D_layout_script.ObjExport
{
    public class ObjFile
    {
        public List<string> Vertices { get; set; }
        public List<string> Normals { get; private set; }
        public List<string> Faces { get; private set; }

        public ObjFile(List<string> vertices, List<string> normals, List<string> faces)
        {
            Vertices = vertices;
            Normals = normals;
            Faces = faces;
        }
    }
}
