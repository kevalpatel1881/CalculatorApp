using Calculator.Interface;
using Calculator.Models;

namespace Calculator.Datalayer
{
    public class ICalculatorRepo : ICalculator
    {
        private List<ArithmeticOperations> arithmeticOperations;
        public ICalculatorRepo()
        {
            arithmeticOperations = new List<ArithmeticOperations> {
                new ArithmeticOperations { ArithmeticOperationID = 1, ArithmeticOperationName = "add", ArithmeticOperation = '+'},
                new ArithmeticOperations { ArithmeticOperationID = 2, ArithmeticOperationName = "multiply", ArithmeticOperation = '*' },
            };
        }
        public int Calculate(string Expression)
        {
            string errorMessage = "";
            try
            {
                Expression = Expression.Replace("(", " ( ").Replace(")", " ) ");
                Stack<string> stack = new Stack<string>();
                string arithmeticOperator = "";
                string[] expressionArray = Expression.Split();
                expressionArray = expressionArray.Where(x => x != "").ToArray();
                if (expressionArray.Length > 1)
                {
                    for (int i = 0; i < expressionArray.Length; i++)
                    {
                        if (arithmeticOperations.Any(x => x.ArithmeticOperationName == expressionArray[i].ToLower()))
                        {
                            arithmeticOperator += arithmeticOperations.Where(x => x.ArithmeticOperationName == expressionArray[i].ToLower()).Select(x => x.ArithmeticOperation).FirstOrDefault().ToString();
                        }
                        else if ((expressionArray[i]).All(char.IsDigit))
                        {
                            if (i == (expressionArray.Length - 1))
                            {
                                stack.Push(arithmeticOperator);
                                arithmeticOperator = "";
                            }
                            stack.Push(expressionArray[i]);
                        }
                        else if (expressionArray[i].Equals("("))
                        {
                            string innerExpression = "";
                            int bracketCount = 0;
                            for (i = i + 1; i < Expression.Length; i++)
                            {
                                if (i >= expressionArray.Length)
                                    errorMessage = "Index out of range.";
                                else
                                {
                                    if (expressionArray[i].Equals("("))
                                        bracketCount++;

                                    if (expressionArray[i].Equals(")"))
                                        if (bracketCount == 0)
                                            break;
                                        else
                                            bracketCount--;
                                    innerExpression += " " + expressionArray[i];
                                }
                            }
                            var calculatedData = Calculate(innerExpression);
                            if (i == (expressionArray.Length - 1))
                            {
                                stack.Push(arithmeticOperator);
                                arithmeticOperator = "";
                            }
                            stack.Push(calculatedData.ToString());
                        }
                        else
                        {
                            errorMessage = "Expression is not in a correct format.";
                            break;
                        }
                    }
                    int calculatedResult = 0;
                    if (stack.Count >= 3)
                    {
                        List<int> numbers = new List<int>();
                        string operatorElement = "";
                        for (int i = stack.Count; i == stack.Count && i != 0; i--)
                        {
                            var checkElement = stack.Pop();
                            if (checkElement.All(char.IsDigit))
                                numbers.Add(Convert.ToInt32(checkElement));
                            else
                                operatorElement = checkElement;
                        }
                        switch (operatorElement)
                        {
                            case "+":
                                calculatedResult = numbers.Aggregate((x, y) => x + y);
                                break;
                            case "*":
                                calculatedResult = numbers.Aggregate((x, y) => x * y);
                                break;
                        }
                        stack.Push(calculatedResult.ToString());
                    }
                    if (stack.Count == 0)
                    {
                        if (String.IsNullOrEmpty(errorMessage))
                            errorMessage = "Stack is empty.";
                        throw new Exception(errorMessage);
                    }
                    else
                        return Convert.ToInt32(stack.Pop());
                }
                else
                {
                    if (expressionArray.Length != 0 && ((expressionArray[0]).All(char.IsDigit)))
                        return Convert.ToInt32(expressionArray[0]);
                    else
                    {
                        errorMessage = "Expression is not in a correct format.";
                        throw new Exception(errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(errorMessage);
            }
        }
    }
}