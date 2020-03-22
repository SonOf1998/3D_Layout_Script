namespace _3D_layout_script.Attributes
{
    public static class AttributeManager
    {
        public static string ErrorMsg { get; set; }

        public static bool CanBind(string attrName, string type)
        {
            switch (attrName)
            {
                case "position":
                    if (type == "Vec3")
                    {
                        return true;
                    }
                    ErrorMsg = "Position can only hold Vec3 values";
                    break;
                case "radius":
                    if (type == "Int" && type == "Float")
                    {
                        return true;
                    }
                    ErrorMsg = "Radius can only hold Int and Float values";
                    break;
                case "height":
                    if (type == "Int" && type == "Float")
                    {
                        return true;
                    }
                    ErrorMsg = "Height can only hold Int and Float values";
                    break;
                case "width":
                    if (type == "Int" && type == "Float")
                    {
                        return true;
                    }
                    ErrorMsg = "Width can only hold Int and Float values";
                    break;
                case "depth":
                    if (type == "Int" && type == "Float")
                    {
                        return true;
                    }
                    ErrorMsg = "Depth can only hold Int and Float values";
                    break;
                case "rotation-axis":
                    if (type == "Vec3")
                    {
                        return true;
                    }
                    ErrorMsg = "Rotation-axis can only hold Vec3 values";
                    break;
                case "rotation-angle":
                    if (type == "Int" && type == "Float")
                    {
                        return true;
                    }
                    ErrorMsg = "Rotation-angle can only hold Int and Float values";
                    break;
            }

            return false;
        }
    }
}
