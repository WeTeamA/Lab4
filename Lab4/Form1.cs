using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public enum TokenType { None, Value, Operation, LBracket, RBracket, Positive, Negative, Name, Function, Variable, Separator };
        public class Token
        {
            public TokenType Type;
            public String TokenText;
            public int TokenStart;
            public Token(string expression, int start, int current, TokenType type)
            {
                Type = type;
                TokenStart = start;

                TokenText = expression.Substring(start, current - start);
            }
            public override string ToString()
            {
                switch (Type)
                {
                    case TokenType.None:
                        return String.Format("{0} Ничто: {1}", TokenStart, TokenText);
                    case TokenType.Value:
                        return String.Format("{0} Значение: {1}", TokenStart, TokenText);
                    case TokenType.Operation:
                        return String.Format("{0} Операция: {1}", TokenStart, TokenText);
                    case TokenType.LBracket:
                        return String.Format("{0} Левая скобка", TokenStart);
                    case TokenType.RBracket:
                        return String.Format("{0} Правая скобка", TokenStart);
                    case TokenType.Positive:
                        return String.Format("{0} Плюс", TokenStart);
                    case TokenType.Negative:
                        return String.Format("{0} Минус", TokenStart);
                    case TokenType.Name:
                        return String.Format("{0} Имя: {1}", TokenStart, TokenText);
                    case TokenType.Function:
                        return String.Format("{0} Функция: {1}", TokenStart, TokenText);
                    case TokenType.Variable:
                        return String.Format("{0} Переменная: {1}", TokenStart, TokenText);
                    case TokenType.Separator:
                        return String.Format("{0} Разделитель", TokenStart);
                    default:
                        return "Некорректный тип токена";
                }
            }
        }
        public class EvaluatorException : Exception
        {
            public int position;
            public string message;
            public EvaluatorException(int pos, string message)
            {
                position = pos;
                this.message = message;
            }
            public override string ToString()
            {
                return String.Format("Ошибка разбора выражения в позиции {0}. {1}", position + 1, message);
            }
        }
        public void NextToken(Queue<Token> que, string expression, ref int start, int i, ref TokenType current, TokenType a )
        {
            current = a;
            que.Enqueue(new Token(expression, start, i, current));
        }

        Queue<Token> Tokenize(string expression)
        {
            Queue<Token> tokens = new Queue<Token>();
            TokenType currentToken = TokenType.None;
            int start = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == ' ')
                    start = i;
                else if ((expression[i] >= '0') && (expression[i] <= '9'))
                {
                    if (currentToken != TokenType.Value && currentToken != TokenType.Name)
                        NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Value);
                }
                else if (expression[i] == '.')
                {
                    if (currentToken != TokenType.Value && currentToken != TokenType.Name)
                        throw new EvaluatorException(i, "Внезапная точка");
                }
                else if (expression[i] == 'e' || expression[i] == 'E')
                {
                    if (currentToken != TokenType.Value && currentToken != TokenType.Name)
                        NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Name);
                }
                else if (expression[i] == '+' || expression[i] == '-')
                {
                    if (!(currentToken == TokenType.Value && i > 0 && (expression[i - 1] == 'e' || expression[i - 1] == 'E')))
                        NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Operation);
                }
                else if (expression[i] == '*')
                {
                    if (!(currentToken == TokenType.Operation && i > 0 && expression[i - 1] == '*'))
                        NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Operation);
                }
                else if (expression[i] == '=')
                {
                    if (!(currentToken == TokenType.Operation && i > 0 && (expression[i - 1] == '!' || expression[i - 1] == '<' || expression[i - 1] == '>')))
                        NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Operation);
                }
                else if (expression[i] == '!' || expression[i] == '/' || expression[i] == '%' || expression[i] == '>' || expression[i] == '<')
                    NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Operation);
                else if (expression[i] == '(')
                    NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.LBracket);
                else if (expression[i] == ')')
                    NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.RBracket);
                else if (expression[i] == ',')
                    NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Separator);
                else if ((expression[i] >= 'a' && expression[i] <= 'z') || (expression[i] >= 'A' && expression[i] <= 'Z'))
                {
                    if (currentToken != TokenType.Name)
                        NextToken(tokens, expression, ref start, i, ref currentToken, TokenType.Name);
                }
                else
                    throw new EvaluatorException(i, "Непонятный символ: " + expression[i]);
            }
            NextToken(tokens, expression, ref start, expression.Length, ref currentToken, TokenType.None);

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
                        output.Enqueue(current);
                        break;
                    case TokenType.Operation:
                        if ((current.TokenText == "-" || current.TokenText == "+") &&
                            (prevToken != TokenType.Value && prevToken != TokenType.RBracket && prevToken != TokenType.Name))
                        {
                            if (current.TokenText == "+")
                                current.Type = TokenType.Positive;
                            else
                                current.Type = TokenType.Negative;
                            stack.Push(current);
                        }
                        else
                        {
                            while (stack.Count > 0 && stack.Peek().IsOperation)
                            {
                                if (stack.Peek().Priority >= current.Priority)
                                    output.Enqueue(stack.Pop());
                                else
                                    break;
                            }
                            stack.Push(current);
                        }
                        break;
                    case TokenType.LBracket:
                        stack.Push(current);
                        break;
                    case TokenType.RBracket:
                        while (stack.Count > 0 && stack.Peek().Type != TokenType.LBracket)
                            output.Enqueue(stack.Pop());
                        if (stack.Count > 0)
                            stack.Pop();
                        else
                            throw new EvaluatorException(current.TokenStart, "Отсутствует открывающая скобка");
                        if (stack.Count > 0 && stack.Peek().Type == TokenType.Function)
                            output.Enqueue(stack.Pop());
                        break;
                    case TokenType.Name:
                        if (input.Count > 0 && input.Peek().Type == TokenType.LBracket)
                        {
                            current.Type = TokenType.Function;
                            stack.Push(current);
                        }
                        else
                        {
                            current.Type = TokenType.Variable;
                            output.Enqueue(current);
                        }
                        break;
                    default:
                        throw new EvaluatorException(current.TokenStart, "Некорректный тип токена");
                }
                prevToken = currentType;
            }
            while (stack.Count > 0)
                if (stack.Peek().Type == TokenType.LBracket)
                    throw new EvaluatorException(stack.Peek().TokenStart, "Незакрытая скобка");
                else
                    output.Enqueue(stack.Pop());
            return output;
        }



        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        
    }
    }
