using System.Globalization;
using System.Linq.Expressions;

namespace SharpGraph.Expressions
{
    /// <summary>
    /// Represents a parsed mathematical expression.
    /// Contains the compiled function and validity state.
    /// </summary>
    public class ParsedExpression
    {
        /// <summary>
        /// Gets the compiled lambda function of the parsed expression.
        /// Function signature: (double x, double y) => double result
        /// </summary>
        public Func<double, double, double>? CompiledFunction { get; init; }

        /// <summary>
        /// Sets the color the expression will be graphed in
        /// </summary>
        public Color FunctionColor { get; set; }

        /// <summary>
        /// Gets a value indicating whether the parsed expression is valid (successfully compiled).
        /// </summary>
        public bool IsValid => CompiledFunction != null;
    }

    /// <summary>
    /// Parses a mathematical expression string into a compiled lambda function.
    /// Supports variables x, y, constants, basic operators, and some math functions.
    /// </summary>
    public class ExpressionParser
    {
        private readonly string _input;
        private int _pos;

        // Parameter expressions representing the input variables x and y
        private readonly ParameterExpression _x = Expression.Parameter(typeof(double), "x");
        private readonly ParameterExpression _y = Expression.Parameter(typeof(double), "y");

        /// <summary>
        /// Private constructor to initialize the parser with normalized input.
        /// </summary>
        /// <param name="input">Raw input string of the expression.</param>
        private ExpressionParser(string input)
        {
            // Remove whitespace and convert to lowercase for normalization
            _input = input.Replace(" ", "").ToLowerInvariant();
        }

        /// <summary>
        /// Attempts to asynchronously parse and compile the expression string.
        /// Returns a ParsedExpression indicating success or failure.
        /// </summary>
        /// <param name="input">Expression string to parse</param>
        /// <returns>A task producing a ParsedExpression</returns>
        public static async Task<ParsedExpression> TryParseAsync(string input, Color expColor)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var parser = new ExpressionParser(input);
                    var expr = parser.ParseExpression();

                    // If not all input consumed, expression is incomplete/invalid
                    if (parser._pos < parser._input.Length)
                        return new ParsedExpression();

                    // Compile the expression tree into a lambda delegate Func<double, double, double>
                    var lambda = Expression.Lambda<Func<double, double, double>>(expr, parser._x, parser._y);
                    return new ParsedExpression { CompiledFunction = lambda.Compile(), FunctionColor = expColor };
                }
                catch
                {
                    // Catch any parsing or compilation errors — return invalid expression
                    return new ParsedExpression();
                }
            });
        }

        /// <summary>
        /// Parses an expression supporting addition and subtraction.
        /// Grammar: expression := term { ('+' | '-') term }
        /// </summary>
        private Expression ParseExpression()
        {
            // Parse left term first
            var left = ParseTerm();

            // Continue parsing + or - operators and right terms
            while (_pos < _input.Length)
            {
                char op = _input[_pos];
                if (op == '+' || op == '-')
                {
                    _pos++; // Consume operator
                    var right = ParseTerm();
                    left = op == '+' ? Expression.Add(left, right) : Expression.Subtract(left, right);
                }
                else
                {
                    break;
                }
            }
            return left;
        }

        /// <summary>
        /// Parses multiplication and division terms.
        /// Grammar: term := power { ('*' | '/') power }
        /// </summary>
        private Expression ParseTerm()
        {
            var left = ParsePower();

            while (_pos < _input.Length)
            {
                char op = _input[_pos];
                if (op == '*' || op == '/')
                {
                    _pos++;
                    var right = ParsePower();
                    left = op == '*' ? Expression.Multiply(left, right) : Expression.Divide(left, right);
                }
                else
                {
                    break;
                }
            }
            return left;
        }

        /// <summary>
        /// Parses exponentiation.
        /// Grammar: power := factor { '^' factor }
        /// </summary>
        private Expression ParsePower()
        {
            var left = ParseFactor();

            while (_pos < _input.Length && _input[_pos] == '^')
            {
                _pos++; // consume '^'
                var right = ParseFactor();
                // Use Math.Pow for exponentiation
                left = Expression.Call(typeof(Math).GetMethod(nameof(Math.Pow))!, left, right);
            }

            return left;
        }

        /// <summary>
        /// Parses unary negation or primary expression.
        /// Grammar: factor := '-' factor | primary
        /// </summary>
        private Expression ParseFactor()
        {
            if (_pos < _input.Length && _input[_pos] == '-')
            {
                _pos++; // consume '-'
                return Expression.Negate(ParseFactor());
            }
            return ParsePrimary();
        }

        /// <summary>
        /// Parses primary expressions:
        /// - Parenthesized expressions
        /// - Numbers
        /// - Variables or functions identifiers
        /// </summary>
        private Expression ParsePrimary()
        {
            if (_pos >= _input.Length)
                return Expression.Constant(0.0);

            if (_input[_pos] == '(')
            {
                _pos++; // consume '('
                var expr = ParseExpression();
                if (_pos < _input.Length && _input[_pos] == ')')
                    _pos++; // consume ')'
                return expr;
            }

            if (char.IsDigit(_input[_pos]) || _input[_pos] == '.')
                return ParseNumber();

            return ParseIdentifier();
        }

        /// <summary>
        /// Parses numeric literals (integer or floating-point).
        /// </summary>
        private ConstantExpression ParseNumber()
        {
            int start = _pos;
            bool hasDecimal = false;

            while (_pos < _input.Length &&
                   (char.IsDigit(_input[_pos]) || (!hasDecimal && _input[_pos] == '.')))
            {
                if (_input[_pos] == '.') hasDecimal = true;
                _pos++;
            }

            string numStr = _input[start.._pos];

            if (double.TryParse(numStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return Expression.Constant(value);

            // On parse failure, default to zero constant
            return Expression.Constant(0.0);
        }

        /// <summary>
        /// Parses identifiers: variables (x,y), constants (pi, e), or functions.
        /// </summary>
        private Expression ParseIdentifier()
        {
            int start = _pos;
            while (_pos < _input.Length && char.IsLetter(_input[_pos]))
                _pos++;

            string ident = _input[start.._pos];

            // Match known variables and constants
            return ident switch
            {
                "x" => _x,
                "y" => _y,
                "pi" => Expression.Constant(Math.PI),
                "e" => Expression.Constant(Math.E),

                // If a recognized function, parse function call expression
                _ when IsFunction(ident) => ParseFunctionCall(ident),

                // Unknown identifiers fallback to zero constant
                _ => Expression.Constant(0.0)
            };
        }

        /// <summary>
        /// Parses a function call with one argument, e.g. sin(...).
        /// </summary>
        private Expression ParseFunctionCall(string funcName)
        {
            if (_pos >= _input.Length || _input[_pos] != '(')
                return Expression.Constant(0.0); // Function call must have '('

            _pos++; // consume '('
            var arg = ParseExpression();

            if (_pos < _input.Length && _input[_pos] == ')')
                _pos++; // consume ')'

            return ApplyFunction(funcName, arg);
        }

        /// <summary>
        /// Determines whether a name corresponds to a supported math function.
        /// </summary>
        private static bool IsFunction(string name) =>
            name is "sin" or "cos" or "tan" or "log" or "exp" or "sqrt";

        /// <summary>
        /// Returns expression representing a call to the corresponding Math function.
        /// </summary>
        private static Expression ApplyFunction(string name, Expression arg)
        {
            return name switch
            {
                "sin" => Expression.Call(typeof(Math).GetMethod(nameof(Math.Sin))!, arg),
                "cos" => Expression.Call(typeof(Math).GetMethod(nameof(Math.Cos))!, arg),
                "tan" => Expression.Call(typeof(Math).GetMethod(nameof(Math.Tan))!, arg),
                "log" => Expression.Call(typeof(Math).GetMethod(nameof(Math.Log))!, arg),
                "exp" => Expression.Call(typeof(Math).GetMethod(nameof(Math.Exp))!, arg),
                "sqrt" => Expression.Call(typeof(Math).GetMethod(nameof(Math.Sqrt))!, arg),
                _ => Expression.Constant(0.0)
            };
        }
    }
}
