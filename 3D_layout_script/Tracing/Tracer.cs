using System.Collections.Generic;
using System;

namespace _3D_layout_script
{
    public static class Tracer
    {
        public static void Print(Stack<Scope> scopes)
        {
            Console.WriteLine();

            int i = scopes.Count;
            foreach (Scope scope in scopes)
            {
                Console.WriteLine($"---- SCOPE {i} ----");

                foreach (KeyValuePair<Symbol, dynamic> pair in scope.table)
                {
                    Console.WriteLine($"{(string)pair.Key} : {pair.Value}");
                }

                i--;
            }

            Console.WriteLine();
        }

        public static void Print(Scope scope)
        {
            Console.WriteLine();

            foreach (KeyValuePair<Symbol, dynamic> pair in scope.table)
            {
                Console.WriteLine($"{(string)pair.Key} : {pair.Value}");
            }

            Console.WriteLine();
        }
    }
}
