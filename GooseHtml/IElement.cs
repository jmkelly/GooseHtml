namespace GooseHtml;

/*public class OneOf<T1, T2>*/
/*{*/
/*    private readonly T1? _value1;*/
/*    private readonly T2? _value2;*/
/**/
/*    public OneOf(T1 value) => _value1 = value ?? throw new ArgumentNullException(nameof(value));*/
/*    public OneOf(T2 value) => _value2 = value ?? throw new ArgumentNullException(nameof(value));*/
/**/
/*    public bool IsT1 => _value1 is not null;*/
/*    public bool IsT2 => _value2 is not null;*/
/**/
/*    public override string ToString() */
/*	{*/
/*		if (_value2 is not null)*/
/*		{*/
/*			return _value2.ToString() ?? throw new ArgumentNullException(nameof(_value2));*/
/*		}*/
/*		if (_value1 is not null)*/
/*		{*/
/*			return _value1.ToString() ?? throw new ArgumentNullException(nameof(_value1));*/
/*		}*/
/**/
/*		throw new ArgumentException(nameof(_value1), nameof(_value2));*/
/*	}*/
/**/
/*	public T Match<T>(Func<T1, T> f1, Func<T2, T> f2) => IsT1 ? f1(_value1) : f2(_value2);*/
/*}*/


public record OneOf<T1, T2>
{
    private readonly T1? _value1;
    private readonly T2? _value2;
    private readonly byte _typeIndex; // 1 = T1, 2 = T2

    public OneOf(T1 value)
    {
        _value1 = value ?? throw new ArgumentNullException(nameof(value));
        _value2 = default;
        _typeIndex = 1;
    }

    public OneOf(T2 value)
    {
        _value2 = value ?? throw new ArgumentNullException(nameof(value));
        _value1 = default;
        _typeIndex = 2;
    }

    public bool IsT1 => _typeIndex == 1;
    public bool IsT2 => _typeIndex == 2;

    public T1 AsT1 => IsT1 ? _value1! : throw new InvalidOperationException($"Not a {typeof(T1)}");
    public T2 AsT2 => IsT2 ? _value2! : throw new InvalidOperationException($"Not a {typeof(T2)}");

    public override string ToString() =>
        _typeIndex switch
        {
            1 => _value1!.ToString()!,
            2 => _value2!.ToString()!,
            _ => throw new InvalidOperationException("Invalid OneOf state")
        };

    public T Match<T>(Func<T1, T> caseT1, Func<T2, T> caseT2) =>
        _typeIndex switch
        {
            1 => caseT1(_value1!),
            2 => caseT2(_value2!),
            _ => throw new InvalidOperationException("Invalid OneOf state")
        };

	public void Match(Action<T1> caseT1, Action<T2> caseT2)
	{
		if (_typeIndex == 1)
			caseT1(_value1!);
		else if (_typeIndex == 2)
			caseT2(_value2!);
		else
			throw new InvalidOperationException("Invalid OneOf state");
	}

	public void MatchT1(Action<T1> caseT1)
	{
		if (_typeIndex == 1)
			caseT1(_value1!);
	}

	public void MatchT2(Action<T2> caseT2)
	{
		if (_typeIndex == 2)
			caseT2(_value2!);
	}

	public static implicit operator OneOf<T1, T2>(T1 value) => new (value);
	public static implicit operator OneOf<T1, T2>(T2 value) => new (value);

}



