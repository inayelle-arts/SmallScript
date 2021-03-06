using System.Collections.Generic;
using SmallScript.Grammars.Shared.Interfaces;
using SmallScript.LexicalParsers.Shared.Enums;
using SmallScript.LexicalParsers.Shared.Extensions;
using SmallScript.LexicalParsers.Shared.Interfaces;
using SmallScript.PolishWriteback.Generator.Details;
using SmallScript.PolishWriteback.Generator.Extensions;
using SmallScript.PolishWriteback.Generator.Interfaces;
using SmallScript.Shared.Extensions;

namespace SmallScript.PolishWriteback.Generator.Base
{
	internal abstract class OperationBase : IOperation
	{
		public IDictionary<IGrammarEntry, IOperation> Operations { get; set; }

		public abstract IGrammarEntry GrammarEntry { get; }

		public abstract IEnumerable<IToken> Consume(TokenIterator iterator, Stack<IToken> stack);

		protected IEnumerable<IToken> ProcessDijkstraUntilEntry(TokenIterator iterator, IGrammarEntry entry)
		{
			var stack  = new Stack<IToken>();
			var output = new List<IToken>();

			while (iterator.IsValid && !iterator.Current.GrammarEntry.Equals(entry))
			{
				var token = iterator.Current;

				if (token.IsDelimiter(Symbol.OperationDelimiter))
				{
					iterator.MoveNext();
					continue;
				}
				
				if (token.GetPriority() < 0)
				{
					output.Add(token);
				}
				else if (token.IsDelimiter(Symbol.OpenParenthesis))
				{
					stack.Push(token);
				}
				else if (token.IsDelimiter(Symbol.CloseParenthesis))
				{
					while (!stack.Peek().Value.InvariantEquals(Symbol.OpenParenthesis))
					{
						output.Add(stack.Pop());
					}

					stack.Pop(); //pop (
				}
				else
				{
					while (stack.Count > 0 && token.GetPriority() <= stack.Peek().GetPriority())
					{
						output.Add(stack.Pop());
					}

					stack.Push(token);
				}

				iterator.MoveNext();
			}

			while (stack.Count > 0)
			{
				output.Add(stack.Pop());
			}

			return output;
		}

		protected IEnumerable<IToken> ProcessDefaultOperationUntilEntry(TokenIterator iterator, IGrammarEntry entry)
		{
			var outputTokens = new List<IToken>();
			var stack        = new Stack<IToken>();

			while (iterator.IsValid && !iterator.Current.GrammarEntry.Equals(entry))
			{
				var currentEntry = iterator.Current.GrammarEntry;

				var operation = Operations[currentEntry];

				outputTokens.AddRange(operation.Consume(iterator, stack));
			}

			return outputTokens;
		}
	}
}