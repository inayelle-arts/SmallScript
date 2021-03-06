using System.Collections.Generic;
using SmallScript.LexicalParsers.Shared.Interfaces;
using SmallScript.PolishWriteback.Executor.Interfaces;
using SmallScript.PolishWriteback.Generator.Details;
using SmallScript.Shared.Details.Auxiliary;

namespace SmallScript.PolishWriteback.Executor.Internals
{
	internal class Runtime
	{
		public Stack<IToken> Stack    { get; private set; }
		public TokenIterator Iterator { get; private set; }

		public IInput        Input     { get; private set; }
		public IOutput       Output    { get; private set; }
		public VariablesData Variables { get; private set; }

		public static RuntimeBuilder Builder => new RuntimeBuilder();

		internal class RuntimeBuilder
		{
			private Runtime _instance;

			public RuntimeBuilder()
			{
				_instance = new Runtime();
			}

			public RuntimeBuilder UseIterator(TokenIterator iterator)
			{
				_instance.Iterator = iterator;
				return this;
			}

			public RuntimeBuilder UseStack(Stack<IToken> stack)
			{
				_instance.Stack = stack;
				return this;
			}

			public RuntimeBuilder UseInput(IInput input)
			{
				_instance.Input = input;
				return this;
			}

			public RuntimeBuilder UseOutput(IOutput output)
			{
				_instance.Output = output;
				return this;
			}

			public RuntimeBuilder UseVariableTable(VariablesData variables)
			{
				_instance.Variables = variables;
				return this;
			}

			public Runtime Build()
			{
				var instance = _instance;

				_instance = new Runtime();

				return instance;
			}
		}
	}
}