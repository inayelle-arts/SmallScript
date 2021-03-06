using System.Linq;
using SmallScript.LexicalParsers.RegexParser.Parser.Internals;
using SmallScript.Shared.Base;
using Xunit;

namespace SmallScript.LexicalParsers.RegexParser.Tests
{
	public class SourceCodeSplitterTest : SmallScriptTestBase
	{
		public SourceCodeSplitterTest()
		{
			_splitter = new SourceCodeSplitter();
		}

		private readonly SourceCodeSplitter _splitter;

		[Fact]
		public void TestSplitLines()
		{
			const int    expectedLineCount = 5;
			const string text              = "qwe\nasd\nzxc\n123 321\n";

			var entries = _splitter.SplitByLines(text);

			Assert.Equal(expectedLineCount, entries.Count);
			Assert.Collection(entries,
					item1 => Assert.Equal("qwe\n", item1),
					item1 => Assert.Equal("asd\n", item1),
					item1 => Assert.Equal("zxc\n", item1),
					item1 => Assert.Equal("123 321\n", item1),
					item1 => Assert.Equal("\n", item1));
		}

//		[Fact]
//		public void TestSplitTokens()
//		{
//			const int expectedTokenCount = 51;
//			const string text = "@decl{\n" +
//			                    "declare var as int of 322\n" +
//			                    "}\n" +
//			                    "@impl{\n" +
//			                    "read var.\n" +
//			                    "write var.\n" +
//			                    "for var by 1 to 10 do {\n" +
//			                    "if [ 1 > 5] then do {\n" +
//			                    "write var.\n" +
//			                    "}fi\n" +
//			                    "}\n";
//
//			var entries = _splitter.SplitByTokens(text);
//
//			Assert.NotEmpty(entries);
//			Assert.DoesNotContain(entries, string.IsNullOrEmpty);
//			Assert.Equal(expectedTokenCount, entries.Count);
//		}
	}
}