using System;
using System.Collections.Generic;

namespace _3D_layout_script
{
    public class Scope
    {
        // Symbol (type, id) + std::variant value
        public Dictionary<Symbol, dynamic> table;
        public static string ErrorMsg { get; private set; }
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

        // úgy adja hozzá, hogy csak az adott scope-ban néz egyezést
        // így hozzuk létre az iterátorokat
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

        // minden scope-ban nézünk egyezést. Az első-be ismétlődés találásánál errort dobunk.
        // a többiben figyelmeztetjük a programozót, hogy name shadow-ol
        public bool Add(Symbol symbol, dynamic value, Stack<Scope> scopeStack)
        {
            foreach (KeyValuePair<Symbol, dynamic> pair in table)
            {
                if (pair.Key.Name == symbol.Name)
                {
                    ErrorMsg = $"Redefinition of variable '{symbol.Name}'";
                    return false;
                }
            }
            
            if (scopeStack.Count != 0)
            {
                scopeStack.Pop();   // a mostani scope-ot kivesszök

                foreach (var scope in scopeStack)
                {
                    foreach (KeyValuePair<Symbol, dynamic> pair in scope.table)
                    {
                        if (pair.Key.Name == symbol.Name && pair.Key.IsIterator == false)
                        {
                            WarningMsg = $"Name shadowing. There is already a variable defined as '{symbol.Name}'";
                            break;
                        }
                        else if (pair.Key.Name == symbol.Name && pair.Key.IsIterator == true)
                        {
                            ErrorMsg = $"Name shadowing. You cannot declare a variable with the name of an iterator ({symbol.Name})";
                            return false;
                        }
                    }
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
                bool isIter = keyToRemove.IsIterator;

                table.Remove(keyToRemove);
                table.Add(new Symbol(isConst, isIter, type, id), value);
            }

            return false;
        }



    }
}
