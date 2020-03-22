using System;
using System.Collections.Generic;

namespace _3D_layout_script
{
    class Scope
    {
        // Symbol (type, id) + std::variant value
        Dictionary<Symbol, dynamic> table;
        public static string ErrorMsg { get; private set; }

        public Scope()
        {
            table = new Dictionary<Symbol, dynamic>();
        }

        public void PrintSymbolTable()
        {
            foreach (KeyValuePair<Symbol, dynamic> pair in table)
            {
                if (pair.Value == null)
                {
                    Console.WriteLine(pair.Key + " null");
                }
                else
                {
                    Console.WriteLine(pair.Key + " " + pair.Value);
                }              
            }
        }

        public bool Add(Symbol symbol, dynamic value)
        {
            foreach (KeyValuePair<Symbol, dynamic> pair in table)
            {
                if (pair.Key.Name == symbol.Name)
                {
                    ErrorMsg = $"Redefinition of variable '{symbol.Name}'";
                    return false;
                }
            }

            if (!Assigner.CanAssign(symbol.Type, value))
            {
                ErrorMsg = Assigner.ErrorMsg;
                return false;
            }

            table[symbol] = value;
            return true;
        }

        public bool Add(Symbol symbol)
        {
            return Add(symbol, null);
        }

        public dynamic GetValue(string id)
        {
            foreach (KeyValuePair<Symbol, dynamic> pair in table)
            {
                if (pair.Key.Name == id)
                {
                    return pair.Value;
                }
            }

            ErrorMsg = $"Using Undeclared / Uninitialized variable {id}";
            return null;
        }

        public string GetType(string id)
        {
            foreach (KeyValuePair<Symbol, dynamic> pair in table)
            {
                if (pair.Key.Name == id)
                {
                    return pair.Key.Type;
                }
            }

            ErrorMsg = $"Using Undeclared / Uninitialized variable {id}";
            return null;
        }

        public Symbol GetSymbol(string id)
        {
            foreach (KeyValuePair<Symbol, dynamic> pair in table)
            {
                if (pair.Key.Name == id)
                {
                    return pair.Key;
                }
            }

            ErrorMsg = $"Using Undeclared / Uninitialized variable {id}";
            return null;
        }

        public bool ReplaceKey(string id, dynamic value)
        {
            var keyToRemove = GetSymbol(id);

            if (keyToRemove != null)
            {
                string original_type = GetType(id);         
                string type = original_type == "Unknown" ? Extensions.Extensions.ToString(value) : original_type;
                bool isConst = keyToRemove.Const;

                table.Remove(keyToRemove);
                table.Add(new Symbol(isConst, type, id), value);
            }

            return false;
        }



    }
}
