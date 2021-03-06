using System.IO;
using SmallScript.Grammars.BackusNaur.Parser.Details;
using SmallScript.Grammars.Shared.Interfaces;
using SmallScript.LexicalParsers.RegexParser.Parser.Internals;
using SmallScript.LexicalParsers.Shared.Details.Tokens;
using SmallScript.LexicalParsers.Shared.Enums;
using SmallScript.Shared.Base;
using SmallScript.Shared.Details.Navigation;
using Xunit;

namespace SmallScript.LexicalParsers.RegexParser.Tests
{
	public class TokenFactoryTest : SmallScriptTestBase
	{
		static TokenFactoryTest()
		{
			var staticFilesDir = Path.GetFullPath("../../../StaticFiles");

			GrammarFile = Path.Combine(staticFilesDir, "grammar");
		}

		public TokenFactoryTest()
		{
			_factory = new TokenFactory(GetGrammar(), new IdentitySource());
		}

		private static readonly string GrammarFile;

		private readonly TokenFactory _factory;

		private static IGrammar GetGrammar()
		{
			var parser = new BackusNaurGrammarParser();

			using (var file = new FileStream(GrammarFile, FileMode.Open))
			{
				return parser.Parse(file);
			}
		}

		[Fact]
		public void TestCreateEolDelimiter()
		{
			var value = "\n";

			var token = _factory.Create(value, new FilePosition(1, 1));

			Assert.IsType<DelimiterToken>(token);
			Assert.Equal(Symbol.OperationDelimiter, token.Value);
		}
	}
}