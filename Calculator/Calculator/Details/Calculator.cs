using System;
using System.Collections.Generic;
using SmallScript.Calculator.Interfaces;
using SmallScript.Calculator.Internals.Operations;
using SmallScript.LexicalParsers.Shared.Details.Tokens;
using SmallScript.LexicalParsers.Shared.Enums;
using SmallScript.LexicalParsers.Shared.Extensions;
using SmallScript.LexicalParsers.Shared.Interfaces;

namespace SmallScript.Calculator.Details
{
	public class Calculator
	{
		private readonly IDictionary<string, IOperation> _operations;
		private readonly PolishWritebackGenerator        _generator;

		public Calculator()
		{
			_generator = new PolishWritebackGenerator();
			_operations = new Dictionary<string, IOperation>(5)
			{
					{ Symbol.Plus, new Addition() },
					{ Symbol.Minus, new Substraction() },
					{ Symbol.Multiplication, new Multiplication() },
					{ Symbol.Division, new Division() },
					{ Symbol.Power, new Power() }
			};
		}

		public double Evaluate(IEnumerable<IToken> tokens)
		{
			var postfixTokens = _generator.Generate(tokens);
			var stack = new Stack<double>();

			foreach (var token in postfixTokens)
			{
				if (token is ConstantToken)
				{
					stack.Push(Double.Parse(token.Value));
				}
				else
				{
					_operations[token.Value].Perform(stack);
				}
			}

			return stack.Pop();
		}
	}
}