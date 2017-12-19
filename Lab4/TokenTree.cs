using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Lab4
{
    class TokenTree
    {
        double multiplier = 1;
        public Object Result { get; private set; }
        Tree<Operation>.Node Tree = new Tree<Operation>.Node();

        public TokenTree(Queue<Token> input, double n)
        {
            try
            {
                multiplier = n;
                Tree = CreateSyntaxTree(input);
                Result = CalcTree(Tree);
            }
            catch { throw new Exception(); }
        }

        Tree<Operation>.Node CreateSyntaxTree(Queue<Token> input)
        {
            Stack<Tree<Operation>.Node> opStack = new Stack<Tree<Operation>.Node>();
            Tree<Operation>.Node operation;
            Functions Constants = new Functions("const.dat");
            while (input.Count > 0)
            {
                Token current = input.Dequeue();
                switch (current.Type)
                {
                    case TokenType.Value:
                        operation = new Tree<Operation>.Node();
                        operation.Value = new FloatValue(double.Parse(current.TokenText, CultureInfo.InvariantCulture));
                        opStack.Push(operation);
                        break;
                    case TokenType.Negative:
                        operation = new Tree<Operation>.Node();
                        operation.Value = new FloatOperation1(x => -x);
                        operation.AddChild(opStack.Pop());
                        opStack.Push(operation);
                        break;
                    case TokenType.Const:
                        operation = new Tree<Operation>.Node();
                        operation.Value = new FloatValue(double.Parse(Constants.First(x => x.Name == current.TokenText).Value, CultureInfo.InvariantCulture));
                        opStack.Push(operation);
                        break;
                    case TokenType.Function:
                        operation = new Tree<Operation>.Node();
                        switch (current.TokenText)
                        {
                            case "min":
                                operation.Value = new FloatOperation2((a, b) => Math.Min(a, b));
                                operation.AddChild(opStack.Pop());
                                operation.AddChild(opStack.Pop());
                                operation.Children.Reverse();
                                opStack.Push(operation);
                                break;
                            case "max":
                                operation.Value = new FloatOperation2((a, b) => Math.Max(a, b));
                                operation.AddChild(opStack.Pop());
                                operation.AddChild(opStack.Pop());
                                operation.Children.Reverse();
                                opStack.Push(operation);
                                break;
                            case "round":
                                operation.Value = new FloatOperation1(x => Math.Round(x));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "trunc":
                                operation.Value = new FloatOperation1(x => Math.Truncate(x));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "floor":
                                operation.Value = new FloatOperation1(x => Math.Floor(x));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "ceil":
                                operation.Value = new FloatOperation1(x => Math.Ceiling(x));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "sin":
                                operation.Value = new FloatOperation1(x => Math.Sin(x * multiplier));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "cos":
                                operation.Value = new FloatOperation1(x => Math.Cos(x * multiplier));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "tan":
                                operation.Value = new FloatOperation1(x => Math.Tan(x * multiplier));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "cotan":
                                operation.Value = new FloatOperation1(x => Math.Cos(x * multiplier) / Math.Sin(x * multiplier));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "arcsin":
                                operation.Value = new FloatOperation1(x => Math.Asin(x * multiplier));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "arccos":
                                operation.Value = new FloatOperation1(x => Math.Acos(x * multiplier));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "arctan":
                                operation.Value = new FloatOperation1(x => Math.Atan(x * multiplier));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "ln":
                                operation.Value = new FloatOperation1(x => Math.Log(x));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "abs":
                                operation.Value = new FloatOperation1(x => Math.Abs(x));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                            case "sign":
                                operation.Value = new FloatOperation1(x => Math.Sign(x));
                                operation.AddChild(opStack.Pop());
                                opStack.Push(operation);
                                break;
                        }
                        break;
                    case TokenType.Operation:
                        operation = new Tree<Operation>.Node();
                        switch (current.TokenText)
                        {
                            case "+":
                                operation.Value = new FloatOperation2((a, b) => a + b);
                                break;
                            case "-":
                                operation.Value = new FloatOperation2((a, b) => a - b);
                                break;
                            case "*":
                                operation.Value = new FloatOperation2((a, b) => a * b);
                                break;
                            case "/":
                                operation.Value = new FloatOperation2((a, b) => a / b);
                                break;
                            case "%":
                                operation.Value = new FloatOperation2((a, b) => a % b);
                                break;
                            case "**":
                                operation.Value = new FloatOperation2((a, b) => Math.Pow(a, b));
                                break;
                            case "=":
                                operation.Value = new Comparsion((a, b) => (a == b) ? 1.0 : 0.0);
                                break;
                            case ">":
                                operation.Value = new Comparsion((a, b) => (a > b) ? 1.0 : 0.0);
                                break;
                            case "<":
                                operation.Value = new Comparsion((a, b) => (a < b) ? 1.0 : 0.0);
                                break;
                            case ">=":
                                operation.Value = new Comparsion((a, b) => (a >= b) ? 1.0 : 0.0);
                                break;
                            case "<=":
                                operation.Value = new Comparsion((a, b) => (a <= b) ? 1.0 : 0.0);
                                break;
                            case "!=":
                                operation.Value = new Comparsion((a, b) => (a != b) ? 1.0 : 0.0);
                                break;
                            case "&":
                                operation.Value = new BoolOperation2((a, b) =>
                                {
                                    if (Math.Abs(a) + Math.Abs(b) > 2) throw new Exception();
                                    return (((a == 1) & (b == 1)) ? 1.0 : 0.0);
                                });
                                break;
                            case "|":
                                operation.Value = new BoolOperation2((a, b) =>
                                {
                                    if (Math.Abs(a) + Math.Abs(b) > 2) throw new Exception();
                                    return (((a == 1) | (b == 1)) ? 1.0 : 0.0);
                                });
                                break;
                            case "^":
                                operation.Value = new BoolOperation2((a, b) =>
                                {
                                    if (Math.Abs(a) + Math.Abs(b) > 2) throw new Exception();
                                    return (((a == 1) ^ (b == 1)) ? 1.0 : 0.0);
                                });
                                break;
                            case "!": break;
                            default:
                                throw new Tokens.EvaluatorException("Неизвестная операция: " + current.TokenText);
                        }
                        if (current.TokenText == "!")
                        {
                            operation.Value = new BoolOperation1(x => { if (Math.Abs(x) > 1) throw new Exception(); return ((x == 0) ? 1.0 : 0.0); });
                            operation.AddChild(opStack.Pop());
                        }
                        else
                        {
                            operation.AddChild(opStack.Pop());
                            operation.AddChild(opStack.Pop());
                            operation.Children.Reverse();
                        }
                        opStack.Push(operation);
                        break;
                }
            }
            if (opStack.Count > 1)
                throw new Exception();
            else if (opStack.Count == 1)
                return opStack.Pop();
            else
                return null;
        }

        object CalcTree(Tree<Operation>.Node root)
        {
            object[] args = new object[root.Children.Count];
            for (int i = 0; i < args.Length; i++)
                args[i] = CalcTree(root.Children[i]);
            return root.Value.Calculate(args);
        }
    }
}

