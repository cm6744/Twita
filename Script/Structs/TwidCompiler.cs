using System.Collections.Generic;
using System.Text;
using Twita.Codec;

namespace Twita.Script.Structs
{

	public class TwidCompiler
	{

		private string code;
		private int pos;

		public TwidCompiler(string code)
		{
			this.code = code;
			this.pos = 0;
		}

		public object Parse()
		{
			Skip();
			char c = code[pos];

			switch(c)
			{
				case '{':
					return parseobject();
				case '[':
					return parseArray();
				case '\"':
					return parseString();
				case 't':
				case 'f':
					return parseBoolean();
				case 'n':
					return parseNull();
				default:
					return parseNumber();
			}
		}

		void Skip()
		{
			while(pos < code.Length && (char.IsWhiteSpace(code[pos]) || code[pos] == '\t'))
			{
				//Tab is not included
				++pos;
			}
		}

		BinaryCompound parseobject()
		{
			Dictionary<string, object> result = new Dictionary<string, object>();

			++pos;
			Skip();

			char ch = code[pos];

			while(ch != '}')
			{
				string key = parseKey();
				Skip();
				++pos;

				object value = Parse();
				result[key] = value;
				Skip();
				ch = code[pos];

				if(ch == ';')
				{
					++pos;
					Skip();
				}

				ch = code[pos];
			}

			++pos;
			return new BinaryCompound(result);
		}

		BinaryList parseArray()
		{
			List<object> result = new List<object>();
			++pos;
			Skip();

			char ch = code[pos];

			while(ch != ']')
			{
				object value = Parse();
				result.Add(value);

				Skip();
				ch = code[pos];

				if(ch == ',')
				{
					++pos;
					Skip();
				}

				ch = code[pos];
			}

			++pos;

			return new BinaryList(result);
		}

		string parseKey()
		{
			StringBuilder result = new StringBuilder();

			char ch = code[pos];

			while(ch != '=' && ch != ' ') //Key cannot include white space
			{
				result.Append(ch);
				++pos;
				ch = code[pos];
			}

			return result.ToString();
		}

		string parseString()
		{
			StringBuilder result = new StringBuilder();
			++pos;

			char ch = code[pos];

			while(ch != '\"')
			{
				if(ch == '\\')
				{
					++pos;
					ch = code[pos];

					switch(ch)
					{
						case '\"':
							result.Append('\"');
							break;
						case '\\':
							result.Append('\\');
							break;
						case '/':
							result.Append('/');
							break;
						case 'b':
							result.Append('\b');
							break;
						case 'f':
							result.Append('\f');
							break;
						case 'n':
							result.Append('\n');
							break;
						case 'r':
							result.Append('\r');
							break;
						case 't':
							result.Append('\t');
							break;
					}
				}
				else
				{
					result.Append(ch);
				}

				++pos;
				ch = code[pos];
			}

			++pos;

			return result.ToString();
		}

		bool parseBoolean()
		{
			if(code[pos] == 't')
			{
				pos += 4;
				return true;
			}

			pos += 5;
			return false;
		}

		object parseNull()
		{
			pos += 4;
			return null;
		}

		object parseNumber()
		{
			int start = pos;

			char ch = code[pos];

			while(char.IsDigit(ch) || ch == '.' || ch == 'e' || ch == 'E' || ch == '+' || ch == '-')
			{
				++pos;
				if(pos >= code.Length)
				{
					break;
				}

				ch = code[pos];
			}

			string numberStr = code.Substring(start, pos - start);

			if(int.TryParse(numberStr, out int ov))
			{
				return ov;
			}

			if(float.TryParse(numberStr, out float ov1))
			{
				return ov1;
			}

			if(double.TryParse(numberStr, out double ov2))
			{
				return ov2;
			}

			return 0;
		}

	}

}