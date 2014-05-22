namespace crTools.Templates.Parsing
{
	public enum Token
	{
		Undefined,
		Identifier,
		String,
		Integer,
		Decimal,
		ParLeft,				// (
		ParRight,				// )
		IndexOpen,				// [
		IndexClose,				// ]
		IndexPoint,				// .
		OperatorAdd,			// +
		OperatorSub,			// -
		OperatorMul,			// *
		OperatorDiv,			// /
		OperatorMod,			// %
		OperatorNot,			// ! not
		CompareEqual,			// == =
		CompareNotEqual,		// != <>
		CompareLower,			// <
		CompareLowerOrEqual,	// <=
		CompareGreater,			// >
		CompareGreaterOrEqual,	// >=
		BoolAnd,				// && and
		BoolOr,					// || or
		ConditionFirst,			// ?
		ConditionSecond,		// :
		ConditionNull,			// ??
		Filter,					// |
		ArgsSeparator,			// ,
		End
	}
}