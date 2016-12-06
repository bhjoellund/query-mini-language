namespace Pippi.Qml.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    internal class QmlAnalyser : IAnalyser
    {
        private readonly ILanguageProvider _languageProvider;

        public QmlAnalyser(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        public AbstractSyntaxTree Analyse(List<Token> tokens)
        {
            var index = 0;
            return AnalyseInternal(tokens, false, ref index);
        }

        private AbstractSyntaxTree AnalyseInternal(IReadOnlyList<Token> tokens, bool isSubExpression, ref int index)
        {
            var ast = new AbstractSyntaxTree();
            var state = ParseState.StartOfExpression;
            Field field = null;
            ComparisonOperator oper = 0;
            Value value = null;

            for (; index < tokens.Count; index++)
            {
                var token = tokens[index];
                switch (state)
                {
                    case ParseState.StartOfExpression:
                        if (token.Type != TokenType.Identifier && !IsGroupingStart(token))
                            throw new ParseException($"Unexpected token {token.Value} (type {token.Type}) found at state {state}");

                        if (!IsGroupingStart(token))
                        {
                            field = _languageProvider.TryGetField(token.Value);

                            if (field == null)
                                throw new ParseException($"Unknown field name: {token.Value}");

                            state = ParseState.AfterField;
                        }
                        else
                        {
                            index += 1;
                            var sub = AnalyseInternal(tokens, true, ref index);
                            ast.Nodes.AddLast(new SubExpression(sub.Nodes));
                            state = ParseState.AfterValue;
                        }
                        break;
                    case ParseState.AfterField:
                        if (token.Type != TokenType.Operator)
                            throw new ParseException($"Unexpected token {token.Value} (type {token.Type}) found at state {state}");

                        var opType = _languageProvider.GetOperatorType(token.Value);

                        if (opType == OperatorType.Invalid)
                            throw new ParseException($"Unknown operator encountered: {oper}");

                        if (opType == OperatorType.Boolean)
                            throw new ParseException($"Invalid placement for boolean operator {oper} encountered");

                        oper = _languageProvider.TryGetComparisonOperator(token.Value);
                        state = ParseState.AfterComparisonOperator;
                        break;
                    case ParseState.AfterComparisonOperator:
                        if (!IsValidValueTokenType(token.Type) && !IsListStartDelimiter(token))
                            throw new ParseException($"Unexpected token {token.Value} (type {token.Type}) found at state {state}");

                        if (!IsListStartDelimiter(token))
                            value = ConvertTokenToValue(token);
                        else
                            value = ConvertTokensToListValue(tokens, ref index);

                        if (field != null && field.Name == "type" && oper != ComparisonOperator.NotEquals)
                        {
                            if (value.Type == QmlType.String && (string) value.ContainedValue != "EbookWithAudio")
                            {
                                oper = ComparisonOperator.In;
                                value = Value.NewList(new[] {value, Value.NewString("EbookWithAudio", "")}, value.Token);
                            }
                            else if (value.Type == QmlType.List && ((Value[]) value.ContainedValue).Where(v => v.Type == QmlType.String).All(v => (string)v.ContainedValue != "EbookWithAudio"))
                            {
                                var arrayList = ((Value[]) value.ContainedValue).ToList();
                                arrayList.Add(Value.NewString("EbookWithAudio", ""));
                                value.ContainedValue = arrayList.ToArray();
                            }
                        }

                        state = ParseState.AfterValue;
                        break;
                    case ParseState.AfterValue:
                        ast.Nodes.AddLast(new Expression(field, value, oper));
                        field = null;
                        value = null;
                        oper = ComparisonOperator.Invalid;

                        if (token.Type != TokenType.Operator && (!isSubExpression || !IsGroupingEnd(token)))
                            throw new ParseException(
                                $"Unexpected token {token.Value} (type {token.Type}) found at state {state}");

                        if (isSubExpression && IsGroupingEnd(token))
                            return ast;

                        opType = _languageProvider.GetOperatorType(token.Value);

                        if (opType == OperatorType.Invalid)
                            throw new ParseException($"Unknown operator encountered: {oper}");

                        if (opType == OperatorType.Comparison)
                            throw new ParseException($"Invalid placement for comparison operator {oper} encountered");

                        var booleanOpNode = GetBooleanOperatorNodeFromToken(token);
                        ast.Nodes.AddLast(booleanOpNode);
                        state = ParseState.StartOfExpression;
                        break;
                }
            }

            if (field != null && value != null && oper != ComparisonOperator.Invalid)
                ast.Nodes.AddLast(new Expression(field, value, oper));

            AnalyseTree(ast);

            return ast;
        }

        private static bool IsGroupingEnd(Token token)
        {
            return IsListEndDelimiter(token);
        }

        private static bool IsGroupingStart(Token token)
        {
            return IsListStartDelimiter(token);
        }

        private Value ConvertTokensToListValue(IReadOnlyList<Token> tokens, ref int index)
        {
            index++;
            var values = new List<Value>();

            while (!IsListEndDelimiter(tokens[index]))
            {
                values.Add(ConvertTokenToValue(tokens[index]));
                ++index;
            }

            return Value.NewList(values.ToArray(), "");
        }

        private static bool IsListEndDelimiter(Token token)
        {
            if (token.Type != TokenType.Delimiter)
                return false;

            return token.Value == ")";
        }

        private static bool IsListStartDelimiter(Token token)
        {
            if (token.Type != TokenType.Delimiter)
                return false;

            return token.Value == "(";
        }

        private static void AnalyseTree(AbstractSyntaxTree ast)
        {
            var node = ast.Nodes.First;

            while (node != null)
            {
                var value = node.Value as Expression;

                if (value != null)
                {
                    var expression = value;

                    var fieldType = expression.Field.Type;
                    var valueType = expression.Value.Type;

                    if (fieldType != valueType && (fieldType != QmlType.Float && valueType != QmlType.Integer) && (fieldType != QmlType.DateTime && valueType != QmlType.TimeSpan) && (valueType != QmlType.List || ((Value[])expression.Value.ContainedValue).Any(v => v.Type != fieldType)))
                        throw new ParseException($"Type of field {expression.Field.Name} was {fieldType} which is not compatible with the value {expression.Value.Token}");
                }

                node = node.Next;
            }
        }

        private BooleanOperatorNode GetBooleanOperatorNodeFromToken(Token token)
        {
            var oper = _languageProvider.GetBooleanOperator(token.Value);

            switch (oper)
            {
                case BooleanOperator.And:
                    return new BooleanOperatorNode(oper);
                default:
                    throw new ParseException("Unknown boolean operator encountered");
            }
        }

        private Value ConvertTokenToValue(Token token)
        {
            switch (token.Type)
            {
                case TokenType.String:
                    return Value.NewString(token.Value.Replace("\\\"", "\""), '"' + token.Value + '"');
                case TokenType.Number:
                    if (token.Value.Contains("."))
                        return Value.NewFloat(Single.Parse(token.Value, CultureInfo.InvariantCulture), token.Value);

                    return Value.NewInteger(Int32.Parse(token.Value), token.Value);
                case TokenType.TimeSpan:
                    return Value.NewTimeSpan(TimeSpanTranslator.Translate(token.Value), token.Value);
                case TokenType.DateTime:
                    var dateTime = DateTime.ParseExact(token.Value, _languageProvider.KnownDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                    return Value.NewDateTime(dateTime, '/' + token.Value + '/');
                default:
                    throw new ParseException($"Unknown value {token.Value} (type {token.Type}) encountered during conversion");
            }
        }

        private static bool IsValidValueTokenType(TokenType type)
        {
            switch (type)
            {
                case TokenType.DateTime:
                case TokenType.Number:
                case TokenType.String:
                case TokenType.TimeSpan:
                    return true;
                default:
                    return false;
            }
        }
    }
}
