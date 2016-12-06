namespace Pippi.Qml.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    class QmlLexer : ILexer
    {
        public List<Token> Tokenize(string source)
        {
            var tokens = new List<Token>();

            for (var i = 0; i < source.Length;)
            {
                var token = FetchNextToken(source, ref i);
                tokens.Add(token);

                AdvanceWhiteSpace(source, ref i);
            }

            return tokens;
        }

        private static void AdvanceWhiteSpace(string source, ref int i)
        {
            while (source.Length > i && Char.IsWhiteSpace(source, i))
                ++i;
        }

        private static Token FetchNextToken(string source, ref int i)
        {
            var tokenType = GetTokenType(source, i);

            switch (tokenType)
            {
                case TokenType.DateTime:
                    return new Token(GetDateTimeToken(source, ref i), tokenType);
                case TokenType.TimeSpan:
                    return new Token(GetTimeSpanToken(source, ref i), tokenType);
                case TokenType.Number:
                    return new Token(GetNumberToken(source, ref i), tokenType);
                case TokenType.Delimiter:
                    return new Token(GetDelimiterToken(source, ref i), tokenType);
                case TokenType.String:
                    return new Token(GetStringToken(source, ref i), tokenType);
                case TokenType.Identifier:
                    return new Token(GetIdentifierToken(source, ref i), tokenType);
                case TokenType.Operator:
                    return new Token(GetOperatorToken(source, ref i), tokenType);
                default:
                    throw new ParseException($"Encountered unsuspected token at position {i}");
            }
        }

        private static string GetOperatorToken(string source, ref int i)
        {
            var sb = new StringBuilder();

            while (source.Length > i && IsOperator(source[i]))
            {
                sb.Append(source[i]);
                ++i;
            }

            return sb.ToString();
        }

        private static string GetIdentifierToken(string source, ref int i)
        {
            var sb = new StringBuilder();

            while (source.Length > i && IsIdentifier(source[i]))
            {
                sb.Append(source[i]);
                ++i;
            }

            return sb.ToString();
        }

        private static string GetStringToken(string source, ref int i)
        {
            var sb = new StringBuilder();
            ++i;

            while (source.Length > i && (source[i] != '"' || source[i - 1] == '\\'))
            {
                sb.Append(source[i]);
                ++i;
            }
            ++i;

            return sb.ToString();
        }

        private static string GetDelimiterToken(string source, ref int i)
        {
            var delimiter = source.Substring(i, 1);
            ++i;
            return delimiter;
        }

        private static string GetNumberToken(string source, ref int i)
        {
            var sb = new StringBuilder();
            var foundDecimalPoint = false;

            while (source.Length > i && (Char.IsDigit(source, i) || (!foundDecimalPoint && source[i] == '.')))
            {
                var c = source[i];
                sb.Append(c);
                ++i;

                if (c == '.')
                    foundDecimalPoint = true;
            }

            return sb.ToString();
        }

        private static string GetTimeSpanToken(string source, ref int i)
        {
            var sb = new StringBuilder();
            ++i;

            while (source.Length > i && (source[i] != '`'))
            {
                sb.Append(source[i]);
                ++i;
            }
            ++i;

            return sb.ToString();
        }

        private static string GetDateTimeToken(string source, ref int i)
        {
            var sb = new StringBuilder();
            ++i;

            while (source.Length > i && (source[i] != '/'))
            {
                sb.Append(source[i]);
                ++i;
            }

            ++i;
            return sb.ToString();
        }

        private static TokenType GetTokenType(string source, int i)
        {
            if(IsDelimiter(source[i]))
                return TokenType.Delimiter;

            if(IsDateTime(source[i]))
                return TokenType.DateTime;

            if (IsTimeSpan(source[i]))
                return TokenType.TimeSpan;

            if(IsString(source[i]))
                return TokenType.String;

            if(IsNumber(source[i]))
                return TokenType.Number;

            if(IsOperator(source[i]))
                return TokenType.Operator;

            if(IsIdentifier(source[i]))
                return TokenType.Identifier;

            throw new ParseException($"Encountered unsuspected character at position {i}: {source[i]}");
        }

        private static bool IsIdentifier(char c)
        {
            return Char.IsLetter(c) || c == '_';
        }

        private static bool IsOperator(char c)
        {
            return c == '~' || c == '=' || c == '<' || c == '>' || c == '!' || c == '&';
        }

        private static bool IsNumber(char c)
        {
            return Char.IsDigit(c);
        }

        private static bool IsString(char c)
        {
            return c == '"';
        }

        private static bool IsTimeSpan(char c)
        {
            return c == '`';
        }

        private static bool IsDateTime(char c)
        {
            return c == '/';
        }

        private static bool IsDelimiter(char c)
        {
            return c == '(' || c == ')';
        }
    }
}