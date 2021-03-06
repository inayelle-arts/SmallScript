using System.Collections.Generic;
using System.Linq;
using SmallScript.Grammars.Shared.Interfaces;
using SmallScript.LexicalParsers.Shared.Enums;
using SmallScript.LexicalParsers.Shared.Extensions;
using SmallScript.LexicalParsers.Shared.Interfaces;
using SmallScript.PolishWriteback.Generator.Interfaces;
using SmallScript.PolishWriteback.Generator.Internals;
using SmallScript.PolishWriteback.Generator.Internals.Tokens;
using SmallScript.Shared.Details.Auxiliary;

namespace SmallScript.PolishWriteback.Generator.Details
{
	public class WritebackGenerator
	{
		private readonly IDictionary<IGrammarEntry, IOperation> _operations;

		public WritebackGenerator() : this(new DefaultOperationProvider())
		{
		}

		public WritebackGenerator(IOperationProvider operationProvider)
		{
			Require.NotNull(operationProvider, nameof(operationProvider));

			_operations = operationProvider.Operations
			                               .ToDictionary(k => k.GrammarEntry);

			foreach (var operation in _operations.Values)
			{
				operation.Operations = _operations;
			}
		}

		public IEnumerable<IToken> Generate(IEnumerable<IToken> tokens)
		{
			var outputTokens = new List<IToken>();
			var iterator     = new TokenIterator(tokens);
			var stack        = new Stack<IToken>();

			while (iterator.IsValid)
			{
				var currentEntry = iterator.Current.GrammarEntry;

				var operation = _operations[currentEntry];

				outputTokens.AddRange(operation.Consume(iterator, stack));
			}

			return RemoveUnnessessaryTokens(outputTokens);
		}

		private IEnumerable<IToken> RemoveUnnessessaryTokens(IEnumerable<IToken> tokens)
		{
			var tokenArray = tokens.Where(t => !t.IsDelimiter(Symbol.StreamReading))
			                       .Where(t => !t.IsDelimiter(Symbol.StreamWriting))
			                       .Where(t => !t.IsDelimiter(Symbol.Assign))
			                       .ToArray();

			for (var i = 0; i < tokenArray.Length; ++i)
			{
				if (tokenArray[i] is LabelToken label)
				{
					label.TargetTokenOrder = i + 1;
				}
			}

			return tokenArray;
		}
	}
}