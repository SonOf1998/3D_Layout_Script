namespace _3D_layout_script
{
    class Symbol
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool Const { get; set; }

        public Symbol(string type, string name) : this(false, type, name)
        {
            
        }

        public Symbol(bool isConst, string type, string name)
        {
            Const = isConst;
            Type = type;
            Name = name;
        }
        
        public static implicit operator string(Symbol symbol)
        {
            string constQualifierStr = "";
            if (symbol.Const)
            {
                constQualifierStr = "const ";
            }
            
            return $"{constQualifierStr}{symbol.Type} {symbol.Name}";
        }
    }
}
