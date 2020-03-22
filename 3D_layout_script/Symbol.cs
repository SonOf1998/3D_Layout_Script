namespace _3D_layout_script
{
    public class Symbol
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool Const { get; set; }
        public bool IsIterator { get; set; }    // iterátátorokra más name shadowing szabályok vonatkoznak

        public Symbol(string type, string name) : this(false, false, type, name)
        {
            
        }

        public Symbol(bool isConst, string type, string name) : this(isConst, false, type, name)
        {
            
        }

        public Symbol(bool isConst, bool isIter, string type, string name)
        {
            Const = isConst;
            Type = type;
            Name = name;
            IsIterator = isIter;
        }
        
        
        public static implicit operator string(Symbol symbol)
        {
            string constQualifierStr = "";
            if (symbol.Const)
            {
                constQualifierStr = "const ";
            }
            
            string iterStr = symbol.IsIterator ? "~iterator~ " : "";

            return $"{constQualifierStr}{iterStr}{symbol.Type} {symbol.Name}";
        }
    }
}
