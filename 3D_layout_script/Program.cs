using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using _3D_layout_script.Objects;
using _3D_layout_script.ObjExport;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


namespace _3D_layout_script
{
    class Program
    {       
        static IParseTree ReadAST(string fileName)
        {
            var code = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, fileName));
            var inputStream = new AntlrInputStream(code);
            var lexer = new DDD_layout_scriptLexer(inputStream);

            //lexer.RemoveErrorListeners();
            //lexer.AddErrorListener(new ErrorListener());


            var tokenStream = new CommonTokenStream(lexer);
            var parser = new DDD_layout_scriptParser(tokenStream);
            //parser.AddParseListener(new DDD_layout_scriptBaseListener());
            var context = parser.program();
            return context;
        }
        


        static void Main(string[] args)
        {
            var ast = ReadAST("karacsony.ddd");
            var visitor = new Visitor();

            List<DDDObject> objects = (List<DDDObject>)visitor.Visit(ast);
            ExportManager em = new ObjExportManager("színtér");
            em.Export(objects);
            Console.WriteLine(".obj generated!");

            // default win10 3D viewert nyitja meg
            Process process = new Process();
            process.StartInfo.FileName = "színtér.obj";
            process.Start();

            visitor.PrintErrorsToConsole();
            visitor.PrintSymbolTree();

            Console.ReadKey();
            
        }
    }
}
