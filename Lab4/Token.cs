using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;


namespace Lab4
{

        internal enum TokenType { None, Value, Operation, LBracket, RBracket, Negative, Const, Function, Separator };

        internal class Token
        {
            public TokenType Type;
            public String TokenText;
            public Token(string text, TokenType type)
            {
                Type = type;
                TokenText = text;
            }
        }

        internal class Tokens
        {
            public Queue<Token> TokensQueue { get; private set; }
            List<String> Operators = new List<string>() { "!", "**", "*", "/", "%", "+", "-", ">", "<", ">=", "<=", "=", "!=", "&", "^", "|" };

            public Tokens(String input,string constants)
            {
                try { TokensQueue = SortStation(Tokenize(input,constants)); }
                catch { throw new Exception(); }
            }

            int Priopity(Token t1, Token t2)
            {
                return Operators.IndexOf(t1.TokenText).CompareTo(Operators.IndexOf(t2.TokenText));
            }

            Queue<Token> Tokenize(string expression, string constants)
            {
                Queue<Token> tokens = new Queue<Token>();
                TokenType currentToken = TokenType.None;
            /*List<List<string>> Constants = new List<List<string>>();
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workBook;
            Microsoft.Office.Interop.Excel.Worksheet workSheet;
            workBook = excelApp.Workbooks.Open(@"C:\Users\lebox\Desktop\Учеба\ООП\Лаб.4\Lab.40\Lab4\Lab4\bin\Debug\Constants.xlsx");
            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets.get_Item(1);
            int count = 1;
            while (workSheet.Cells[count, 1].Text != "")
            {
                Constants.Add(new List<string> { workSheet.Cells[count, 1].Text, workSheet.Cells[count, 2].Text.Replace(",", ".") });
                count++;
            }
            workBook.Save();
            workBook.Close();
            */
            //Functions Constants = new Functions("const.dat");
                string value = @"^(?<value>-?[0-9]*\.?[0-9]+)";
                string operation = @"^(?<operator>(\*\*|\+|\-|\*|\/|\%|\=|\>\=|\<\=|\>|\<|\!\=|\&|\||\^|\!))";
                string function = @"^(?<sign>-?)(?<func>(min|max|round|trunc|floor|ceil|sin|cos|tan|cotan|arcsin|arccos|arctan|ln|abs|sign))";
                expression = expression.Replace(' ', '\0');
                while (expression.Length > 0)
                {
                    if (Regex.IsMatch(expression, value) && (Regex.Match(expression, value).Value.IndexOf("-") == -1
                        || currentToken == TokenType.LBracket || currentToken == TokenType.None))
                    {
                        Match match = Regex.Match(expression, value);
                        currentToken = TokenType.Value;
                        tokens.Enqueue(new Token(match.Groups["value"].ToString(), currentToken));
                        expression = expression.Remove(0, match.Length);
                        continue;
                    }
                    else if (Regex.IsMatch(expression, function))
                    {
                        Match match = Regex.Match(expression, function);
                        if (match.Groups["sign"].ToString() == "-")
                            tokens.Enqueue(new Token(match.Groups["sign"].ToString(), TokenType.Negative));
                        currentToken = TokenType.Function;
                        tokens.Enqueue(new Token(match.Groups["func"].ToString(), currentToken));
                        expression = expression.Remove(0, match.Length);
                    }
                    else if (Regex.IsMatch(expression, constants))
                    {
                        Match match = Regex.Match(expression, constants);
                        if (match.Groups["sign"].ToString() == "-")
                            tokens.Enqueue(new Token(match.Groups["sign"].ToString(), TokenType.Negative));
                        currentToken = TokenType.Const;
                        tokens.Enqueue(new Token(match.Groups["const"].ToString(), currentToken));
                        expression = expression.Remove(0, match.Length);
                    }
                    else if (Regex.IsMatch(expression, operation))
                    {
                        Match match = Regex.Match(expression, operation);
                        if (match.Value == "-" && (currentToken == TokenType.LBracket || currentToken == TokenType.None))
                            currentToken = TokenType.Negative;
                        else
                            currentToken = TokenType.Operation;
                        tokens.Enqueue(new Token(match.Groups["operator"].ToString(), currentToken));
                        expression = expression.Remove(0, match.Length);
                    }
                    else if (expression[0] == '(') { currentToken = TokenType.LBracket; tokens.Enqueue(new Token("(", currentToken)); expression = expression.Remove(0, 1); }
                    else if (expression[0] == ')') { currentToken = TokenType.RBracket; tokens.Enqueue(new Token(")", currentToken)); expression = expression.Remove(0, 1); }
                    else if (expression[0] == ',') { currentToken = TokenType.Separator; tokens.Enqueue(new Token(",", currentToken)); expression = expression.Remove(0, 1); }
                    else { throw new EvaluatorException("Непонятный символ: " + expression[0]); }
                }
                return tokens;
            }


            Queue<Token> SortStation(Queue<Token> input)
            {
                Queue<Token> output = new Queue<Token>();
                Stack<Token> stack = new Stack<Token>();
                TokenType prevToken = TokenType.None;
                while (input.Count > 0)
                {
                    Token current = input.Dequeue();
                    TokenType currentType = current.Type;
                    switch (current.Type)
                    {
                        case TokenType.Value:
                        case TokenType.Const:
                            output.Enqueue(current);
                            break;

                        case TokenType.LBracket:
                        case TokenType.Function:
                        case TokenType.Negative:
                            stack.Push(current);
                            break;

                        case TokenType.Separator:
                            break;

                        case TokenType.Operation:
                            while (stack.Count > 0 && stack.Peek().Type == TokenType.Operation)
                            {
                                if (Priopity(stack.Peek(), current) <= 0)
                                    output.Enqueue(stack.Pop());
                                else
                                    break;
                            }
                            stack.Push(current);
                            break;

                        case TokenType.RBracket:
                            while (stack.Count > 0 && stack.Peek().Type != TokenType.LBracket)
                                output.Enqueue(stack.Pop());
                            if (stack.Count > 0)
                                stack.Pop();
                            else
                                throw new EvaluatorException("Нехватает скобок");
                            if (stack.Count > 0 && stack.Peek().Type == TokenType.Function)
                                output.Enqueue(stack.Pop());
                            break;

                        default:
                            throw new EvaluatorException("Некорректный тип токена");
                    }
                    prevToken = currentType;
                }
                while (stack.Count > 0)
                    if (stack.Peek().Type == TokenType.LBracket)
                        throw new EvaluatorException("Нехватает скобок");
                    else
                        output.Enqueue(stack.Pop());
                return output;
            }


            public class EvaluatorException : Exception
            {
                public string message;
                public EvaluatorException(string message)
                {
                    this.message = message;
                }
                public override string ToString()
                {
                    return message;
                }
            }
        }
    }

