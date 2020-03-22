﻿//#define RELEASE

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _3D_layout_script
{
    class Visitor : DDD_layout_scriptBaseVisitor<object>
    {
        Stack<Scope> symbolTable;
        Scope currentScope;

        public Visitor()
        {
            symbolTable = new Stack<Scope>();
        }

        public void PrintSymbolTree()
        {
            int i = symbolTable.Count;
            foreach(Scope s in symbolTable)
            {
                Console.WriteLine($"----------- SCOPE {i} -----------");
                s.PrintSymbolTable();
                --i;
            }
        }

        private string GetLineNumberForError(ParserRuleContext context)
        {
            return $"Error at line {context.Start.Line}!";
        }

        private string GetLineNumberForWarning(ParserRuleContext context)
        {
            return $"Warning at line {context.Start.Line}!";
        }

        private void ErrorHandlingOrCommit(ParserRuleContext context, bool success)
        {
            if (success)
            {
                return;
            }

            Console.WriteLine($"{GetLineNumberForError(context)} {Scope.ErrorMsg}");
        }

        private string GetVariableTypeByName(string id)
        {
            Symbol symb = GetVariableByName(id);
            if (symb != null)
            {
                return symb.Type;
            }

            return null;
        }

        private dynamic GetVariableValueByName(string id)
        {
            dynamic ret = currentScope.GetValue(id);

            if (ret == null)
            {
                foreach (Scope sc in symbolTable)
                {
                    ret = sc.GetValue(id);
                
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }

            return ret;
        }

        private Symbol GetVariableByName(string id)
        {
            dynamic ret = currentScope.GetSymbol(id);

            if (ret == null)
            {
                foreach (Scope sc in symbolTable)
                {
                    ret = sc.GetSymbol(id);

                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }

            return ret;
        }


        private bool SetVariableValueByName(string id, dynamic value, ParserRuleContext context)
        {
            return SetVariableValueByName(id, value, null, context);
        }

        private bool SetVariableValueByName(string id, dynamic value, OtherBinaryOperation obo, ParserRuleContext context)
        {
            if (value == null)
            {
                return false;
            }

            dynamic currentValue = GetVariableValueByName(id);
            string currentType = GetVariableTypeByName(id);
            bool complexAssign = obo != null;

            if (complexAssign)
            {
                var newValue = obo.Calculate(currentValue, value);

                string warningMsg = obo.WarningMsg;
                if (warningMsg != null)
                {
                    Console.WriteLine($"{GetLineNumberForWarning(context)} {warningMsg}");
                }
                if (newValue == null)
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} {obo.ErrorMsg}");
                    return false;
                }

                bool success = currentScope.ReplaceKey(id, newValue);

                if (!success)
                {
                    foreach (Scope sc in symbolTable)
                    {
                        success = sc.ReplaceKey(id, newValue);
                        if (success)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                if (Assigner.CanAssign(currentType, value))
                {
                    bool success = currentScope.ReplaceKey(id, value);

                    if (!success)
                    {
                        foreach (Scope sc in symbolTable)
                        {
                            success = sc.ReplaceKey(id, value);
                            if (success)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} {Assigner.ErrorMsg}");
                    return false;
                }
            }

            


            return true;
        }
        

        /* Egyszerű kifejezések. (hivatkozás másik változóra, érték valahol, vec3 koordinátájának lekérése)
         * 
        */
        public override dynamic VisitSimple_expression([NotNull] DDD_layout_scriptParser.Simple_expressionContext context)
        {
            var signedId = context.signed_id();

            if (signedId != null)
            {
                string id = signedId.GetText().Replace("-", "").Replace("+", "");
                bool isNegative = signedId.GetText().Contains("-");

                // ha nincs meg a változó, akkor error objectet adunk vissza, mint a simple expression kiértékelése
                if (GetVariableByName(id) == null)
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} Using of undeclared variable ({id})");
                    return new ErrorObject();
                }

                return isNegative ? -GetVariableValueByName(id) : GetVariableValueByName(id);
            }
            else if (context.type_val() != null)
            {
                return VisitType_val(context.type_val());
            }
            else if (context.xyz() != null)
            {
                return VisitXyz(context.xyz());
            }

            return null;
        }

        /* Ezek már konkrét értékek!
         * 
         * Int-nél és Float-nál castolással visszaadhatóak az értékek.
         * Vec3 még bejárást igényel, mert az operation-öket is tartalmazhat.
         *
        */
        public override object VisitType_val([NotNull] DDD_layout_scriptParser.Type_valContext context)
        {
            if (context.FLOAT() != null)
            {
                return double.Parse(context.GetText().Replace("f", ""));
            }
            else if (context.INT() != null)
            {
                return int.Parse(context.GetText());
            }

            // Ha vec3.
            return base.VisitType_val(context);
        }

        /* Vec3 bejárása.
         * 
         * Az egyes koordináták mind operation-ök.
         *
        */
        public override dynamic VisitVec3([NotNull] DDD_layout_scriptParser.Vec3Context context)
        {
            var operationArr = context.operation();
            dynamic[] executedOperationValues = new dynamic[3];

            bool ok = true;
            for (int i = 0; i < 3; ++i)
            {
                executedOperationValues[i] = VisitOperation(operationArr[i]);

                // valamelyik koordináta vec3 volt, annak nincs értelme
                if (executedOperationValues[i] is vec3)
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} {operationArr[i].GetText()} evaluates to vec3");
                    ok = false;
                }

                if (!ok && i == 3 - 1)
                {
                    return null;
                }
            }


            return new vec3(executedOperationValues[0], executedOperationValues[1], executedOperationValues[2]);
        }

        /* Egy változó x, y, z adattagjára történt hivatkozás.
         * A változó
         *
        */
        public override object VisitXyz([NotNull] DDD_layout_scriptParser.XyzContext context)
        {
            string signedId = context.signed_id().GetText();
            bool negative = signedId.Contains('-');
            string id = signedId.Replace("+", "").Replace("-", "");

            dynamic value = GetVariableValueByName(id);
            Symbol symbol = GetVariableByName(id);

            // nem inicializált változóból olvasás
            if (value is UnitializedObject)
            {
                Console.WriteLine($"{GetLineNumberForError(context)} Uninitialized value {id}");
                return null;
            }
            // a hivatkozott változó vec3, a normál use-case
            else if (symbol.Type == "Vec3")
            {
                string xyzText = context.GetText();
                vec3 vecValue = (vec3)value;

                if (xyzText.Contains(".x"))
                {
                    return negative ? -vecValue.x : vecValue.x;
                }
                else if (xyzText.Contains(".y"))
                {
                    return negative ? -vecValue.y : vecValue.y;
                }
                else if (xyzText.Contains(".z"))
                {
                    return negative ? -vecValue.z : vecValue.z;
                }
            }
            // nem vec3-ra hivatkozott a változó
            Console.WriteLine($"{GetLineNumberForError(context)} {id} doesn't refer to a vec3 instance");
                
            return null;
        }
        
        /* Összeadások, szorzások stb, zárójelezések sorozata.
         * A végén egy konkrét érték van. (float, int, vec3, errorobject)
         */ 
        public override object VisitOperation([NotNull] DDD_layout_scriptParser.OperationContext context)
        {
            if (context == null)
            {
                return new UnitializedObject();
            }

            var opSymbolArr = context.binary_op().Select(binaryOpRule => binaryOpRule.GetText()).ToList();
            
            List<dynamic> opValues = new List<dynamic>();
            
            dynamic a = null;

            if (context.operation_helper() != null)
            {
                foreach (var opHelper in context.operation_helper())
                {
                    opValues.Add(VisitOperation_helper(opHelper));
                }
            }
            
            // elvégezzük az összes operációt, nem lesz már több műveleti jel
            while (opSymbolArr.Count() != 0)
            {
                for (int i = 0; i < opSymbolArr.Count(); ++i)
                {
                    // van-e még magas precedenciájú művelet-e a műveletláncban
                    if (opSymbolArr.Contains("*") || opSymbolArr.Contains("/"))
                    {
                        // ha egy olyannál vagyunk végezzük el
                        if (opSymbolArr[i] == "*" || opSymbolArr[i] == "/")
                        {
                            BinaryOperation op = new BinaryOperation(opSymbolArr[i]);
                            dynamic opResult = op.Calculate(opValues[i], opValues[i + 1]);

                            string warningMsg = op.WarningMsg;
                            if (warningMsg != null)
                            {
                                Console.WriteLine($"{GetLineNumberForWarning(context)} {warningMsg}");
                            }
                            
                            if (opResult is UnitializedObject)
                            {
                                Console.WriteLine($"{GetLineNumberForError(context)} Using of uninitialized variable");
                                return new ErrorObject();
                            }

                            if (opResult == null)
                            {
                                Console.WriteLine($"{GetLineNumberForError(context)} {op.ErrorMsg}");
                                return new ErrorObject();
                            }
                                
                            opValues[i] = opResult;
                            opValues.RemoveAt(i + 1);
                            opSymbolArr.RemoveAt(i);
                        }
                    }
                    // már csak +-ok és -ok vannak
                    else
                    {
                        BinaryOperation op = new BinaryOperation(opSymbolArr[i]);
                        dynamic opResult = op.Calculate(opValues[i], opValues[i + 1]);

                        if (opResult is UnitializedObject)
                        {
                            Console.WriteLine($"{GetLineNumberForError(context)} Using of uninitialized variable");
                            return new ErrorObject();
                        }

                        if (opResult == null)
                        {
                            Console.WriteLine($"{GetLineNumberForError(context)} {op.ErrorMsg}");
                            return new ErrorObject();
                        }

                        opValues[i] = opResult;
                        opValues.RemoveAt(i + 1);
                        opSymbolArr.RemoveAt(i);
                    }
                }
            }
            
            return opValues[0];
        }

        public override object VisitOperation_helper([NotNull] DDD_layout_scriptParser.Operation_helperContext context)
        {
            if (context.simple_expression() != null)
            {
                return VisitSimple_expression(context.simple_expression());
            }

            if (context.operation() != null)
            {
                return VisitOperation(context.operation());
            }

            return null;
        }

        public override object VisitObject_block([NotNull] DDD_layout_scriptParser.Object_blockContext context)
        {
            symbolTable.Push(currentScope);
            return base.VisitObject_block(context);
        }

        /* Létrejozza az első scope-ot
         * Majd a teljes bejárás után hozzáadja a táblához az utolsót.
         * Visszaadott értékét nem használjuk.
         */ 
        public override object VisitProgram([NotNull] DDD_layout_scriptParser.ProgramContext context)
        {
            currentScope = new Scope();
            symbolTable.Push(currentScope);
            base.VisitProgram(context);
#if RELEASE
            symbolTable = null;
#endif
            return null;
        }

        /* If blokkon megyünk végig
         * Ha a feltétel igaz, akkor az if blokk-ba megyünk bele.
         * Ha nem akkor végignézzük az else if-eken.
         * Ha azok közül sem igaz egyik sem, akkor az else blokk kerül végrehajtásra.
         * 
         * Egy új scope-ot hozunk létre, ha nem teljesül egyik if feltétel sem, akkor ez a felesleges scope egyből poppol.
         */ 
        public override object VisitIf_statement([NotNull] DDD_layout_scriptParser.If_statementContext context)
        {          
            currentScope = new Scope();
            symbolTable.Push(currentScope);

            var condition = context.if_condition();
            if ((bool)VisitIf_condition(condition) != true)
            {
                foreach (var elseif in context.else_if_statement())
                {
                    if ((bool)VisitElse_if_statement(elseif) == true)
                    {
                        return null;
                    }
                }

                if (context.else_statement() != null)
                {
                    VisitElse_statement(context.else_statement());
                }
            }
            else
            {
                foreach (var content in context.if_content())
                {
                    VisitIf_content(content);
                }
            }
            
#if RELEASE
            symbolTable.Pop();
            currentScope = symbolTable.Peek();
#endif
            return null;
        }

        /* Ha a feltétel igaz, akkor az összes if-blokkbeli kifejezést végiglátogatjuk és igazat adunk vissza, a "szülő" if blokknak,
         * jelezve, hogy nem kell megnéznie a többi ágat.
         * 
         * Különben hamis a visszatérés
         */ 
        public override object VisitElse_if_statement([NotNull] DDD_layout_scriptParser.Else_if_statementContext context)
        {
            if ((bool)VisitIf_condition(context.if_condition()) == true)
            {
                foreach (var statement in context.if_content())
                {
                    VisitIf_content(statement);
                }
                            
                return true;
            }

            return false;
        }

        /* Megnézzük a feltétel két részét, hogy egyáltalán helyesek-e.
         * 
         * Ha rendben volt akkor megnézzük, hogy összehasonlítható-e, a sorban megfogalmazott szimbólum alapján (<, <=, == stb.)
         * Ha nem akkor false-t adunk vissza. (Ez így mondhatni hibásan aktiválja az else blokkot..)
         * Ha a visszatérési érték false, akkor azért megnézzük, hogy ténylegesen ez-e az összehasonlítás eredménye vagy csak hiba.
         * Ha hiba, azt látjuk a létrehozott Comparator objektumon.
         * 
         * Hiba például < operátorral összehasonlítani két vektort.
         */
        public override object VisitIf_condition([NotNull] DDD_layout_scriptParser.If_conditionContext context)
        {
            var sides = context.operation();

            dynamic leftSide = VisitOperation(sides[0]);
            dynamic rightSide = VisitOperation(sides[1]);

            if (leftSide is ErrorObject || leftSide is UnitializedObject || rightSide is ErrorObject || rightSide is UnitializedObject)
            {
                Console.WriteLine($"{GetLineNumberForError(context)} Uninterpretable if condition");
                return false;
            }

            Comparator comparator = new Comparator(context.COMP_OP().GetText());
           
            if (comparator.Compare(leftSide, rightSide))
            {
                return true;
            }
            else
            {
                if (comparator.HasErrorMsg)
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} {comparator.ErrorMsg}");
                }                
            }


            return false;
        }

        /* Megnézzük, hogy érvényes-e a külön-külön és együtt a range és a step.
         * 
         * Ha nem, akkor nullt adunk vissza, hogy a for ciklus ne futhasson. És hibát írunk ki.
         * Különben egy háromelemű tuple-t adunk vissza, kezdet vég és step intekkel.
         * Így ha esetleg valamelyik ezek közül float-lenne, annak az alsó egészrészét vesszük és warningot kap a programozó.
         */ 
        public override object VisitRange_and_step([NotNull] DDD_layout_scriptParser.Range_and_stepContext context)
        {
            var operations = context.operation();
            dynamic rangeStart = VisitOperation(operations[0]);
            dynamic rangeEnd = VisitOperation(operations[1]);
            dynamic step = 1;

            if (context.STEP() != null)
            {
                step = VisitOperation(operations[2]);
            }

            // ha bármelyik nem int vagy float, akkor ott hiba van
            if (!(rangeStart is double || rangeStart is int) || !(rangeEnd is double || rangeEnd is int) || !(step is int || step is double))
            {
                Console.WriteLine($"{GetLineNumberForError(context)} Range and step can only hold Int or Float values");
                return null;
            }

            if (rangeStart is double)
            {
                Console.WriteLine($"{GetLineNumberForWarning(context)} The start of the range is casted from Float to Int. You may ignore this warning.");
                rangeStart = (int)rangeStart;
            }

            if (rangeEnd is double)
            {
                Console.WriteLine($"{GetLineNumberForWarning(context)} The end of the range is casted from Float to Int. You may ignore this warning.");
                rangeEnd = (int)rangeEnd;
            }

            if (step is double)
            {
                Console.WriteLine($"{GetLineNumberForWarning(context)} The step is casted from Float to Int. You may ignore this warning.");
                step = (int)step;
            }

            if (context.BRACKET_O() != null)
            {
                rangeStart += 1;
            }

            if (context.BRACKET_C() != null)
            {
                rangeEnd -= 1;
            }

            if (step == 0)
            {
                Console.WriteLine($"{GetLineNumberForError(context)} Step was 0. This would cause an infinite loop");
                return null;
            }

            if ((step > 0 && rangeEnd - rangeStart < 0) || (step < 0 && rangeEnd - rangeStart > 0))
            {
                Console.WriteLine($"{GetLineNumberForError(context)} Infinite loop. [Range: {rangeStart}..{rangeEnd}, Step: {step}]");
                return null;
            }

            return new Tuple<int, int, int>(rangeStart, rangeEnd, step);
        }

        public override object VisitFor_loop([NotNull] DDD_layout_scriptParser.For_loopContext context)
        {
            currentScope = new Scope();
            symbolTable.Push(currentScope);

            Tuple<int, int, int> rangeStepTriple = (Tuple<int, int, int>)VisitRange_and_step(context.range_and_step());
            if (rangeStepTriple != null)
            {

            }

            base.VisitFor_loop(context);
#if RELEASE
            symbolTable.Pop();
            currentScope = symbolTable.Peek();
#endif

            return null;
        }

        /* Váltózó inicializálását nézi meg.
        */
        public override object VisitVariable_decl([NotNull] DDD_layout_scriptParser.Variable_declContext context)
        {
            var op = context.operation();
            string idStr = context.ID().GetText();

            // nincsen kiírva explicit a típus
            // type inference
            if (context.TYPE() == null)
            {
                // nincsen jobb oldal sem
                if (op == null)
                {           
                    ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Unknown", idStr), VisitOperation(op)));
                }
                // jobb oldalon egy float van
                else if (Regex.IsMatch(op.GetText(), @"^([0-9]*)?\.[0-9]+f?$"))
                {
                    ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Float", idStr), VisitOperation(op)));
                }
                // jobb oldalon egy int van
                else if (Regex.IsMatch(op.GetText(), @"^[0-9]+$"))
                {
                    ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Int", idStr), VisitOperation(op)));
                }
                // jobb oldalon egy vec3 van
                else if (Regex.IsMatch(op.GetText(), @"^\[[^,]+,[^,]+,[^,]+\]$"))
                {
                    ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Vec3", idStr), VisitOperation(op)));
                }
                // jobb oldalon valami kifejezés van
                else
                {
                    // vektor jöhet ki végeredményül, int, float már tuti nem
                    if (op.GetText().Contains("["))
                    {
                        ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Vec3", idStr), VisitOperation(op)));
                    }
                    // bármi kijöhet 
                    else
                    {
                        dynamic result = VisitOperation(op);
                        ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, Extensions.Extensions.ToString(result), idStr), result));
                    }
                }
            }
            else
            {
                switch (context.TYPE().GetText())
                {
                    case "Float":
                        ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Float", idStr), VisitOperation(op)));
                        break;
                    case "Int":
                        ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Int", idStr), VisitOperation(op)));
                        break;
                    case "Vec3":
                        ErrorHandlingOrCommit(context, currentScope.Add(new Symbol(context.CONST() != null, "Vec3", idStr), VisitOperation(op)));
                        break;

                }
            }

            return null;
        }

        /* assign_statement:       (simple_modifyable_exp EQ operation SEMI) |
                        (simple_modifyable_exp other_binary_op operation SEMI);
         * 
        */
        public override object VisitAssign_statement([NotNull] DDD_layout_scriptParser.Assign_statementContext context)
        {
            var toModify = context.simple_modifyable_exp();

            if (toModify.ID() != null)
            {
                string id = toModify.ID().GetText();

                Symbol symbol = GetVariableByName(id);
                dynamic value = GetVariableValueByName(id);
                if (value == null)
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} Cannot assign to an undefined variable {id}");
                    return null;
                }
                if (symbol.Const && !(value is UnitializedObject))
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} Left hand side refers to a constant expression {id}");
                    return null;
                }


                dynamic opValue = VisitOperation(context.operation());

                // az értéket teljesen felülírjuk
                if (context.EQ() != null)
                {
                    SetVariableValueByName(id, opValue, context);
                }
                // az értéket változtatjuk ( += v -= v *= v /= )
                else
                {
                    if (value is UnitializedObject)
                    {
                        Console.WriteLine($"{GetLineNumberForError(context)} You cannot use += -= *= /= on an uninitalized variable ({id})");
                        return null;
                    }

                    SetVariableValueByName(id, opValue, new OtherBinaryOperation(context.other_binary_op().GetText()), context);
                }
            }
            // vektor valamelyik koordinátájával csinálunk valamit.
            else
            {
                var xyz = toModify.modifiable_xyz();
                string xyzText = xyz.GetText();
                string id = xyz.ID().GetText();

                Symbol symbol = GetVariableByName(id);
                dynamic value = GetVariableValueByName(id);

                if (value == null)
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} Cannot assign to an undefined variable {id}");
                    return null;
                }
                if (symbol.Const && !(value is UnitializedObject))
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} Left hand side refers to a constant expression {id}");
                    return null;
                }
                if (symbol.Type != "Vec3" && symbol.Type != "Unknown")
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} Cannot access coordinates of a non Vec3 type");
                    return null;
                }
                else if (value is UnitializedObject)
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} Accessing coordinates of an unitialized Vec3");
                    return null;
                }


                vec3 originalVec3 = (vec3)value;
                dynamic opValue = VisitOperation(context.operation());

                if (!Assigner.CanAssign("Float", opValue))
                {
                    Console.WriteLine($"{GetLineNumberForError(context)} {Assigner.ErrorMsg}");
                    return null;
                }

                if (xyzText.Contains(".x"))
                {
                    if (context.EQ() != null)
                    {
                        dynamic res = opValue;
                        originalVec3.x = res;
                        SetVariableValueByName(id, originalVec3, null);
                    }
                    else
                    {
                        OtherBinaryOperation obo = new OtherBinaryOperation(context.other_binary_op().GetText());
                        dynamic res = obo.Calculate(originalVec3.x, opValue);
                        originalVec3.x = res;
                        SetVariableValueByName(id, originalVec3, null);
                    }                    
                }
                else if (xyzText.Contains(".y"))
                {
                    if (context.EQ() != null)
                    {
                        dynamic res = opValue;
                        originalVec3.y = res;
                        SetVariableValueByName(id, originalVec3, null);
                    }
                    else
                    {
                        OtherBinaryOperation obo = new OtherBinaryOperation(context.other_binary_op().GetText());
                        dynamic res = obo.Calculate(originalVec3.y, opValue);
                        originalVec3.y = res;
                        SetVariableValueByName(id, originalVec3, null);
                    }
                }
                else if (xyzText.Contains(".z"))
                {
                    if (context.EQ() != null)
                    {
                        dynamic res = opValue;
                        originalVec3.z = res;
                        SetVariableValueByName(id, originalVec3, null);
                    }
                    else
                    {
                        OtherBinaryOperation obo = new OtherBinaryOperation(context.other_binary_op().GetText());
                        dynamic res = obo.Calculate(originalVec3.z, opValue);
                        originalVec3.z = res;
                        SetVariableValueByName(id, originalVec3, null);
                    }
                }
            }



            return null;
        }
    }
}