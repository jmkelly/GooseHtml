namespace GooseHtml;

public class OneOf<T0, T1>
{
    private readonly T0? _value1;
    private readonly T1? _value2;
    private readonly byte _typeIndex; // 1 = T1, 2 = T2

    public OneOf(T0 value)
    {
        _value1 = value ?? throw new ArgumentNullException(nameof(value));
        _value2 = default;
        _typeIndex = 1;
    }

    public OneOf(T1 value)
    {
        _value2 = value ?? throw new ArgumentNullException(nameof(value));
        _value1 = default;
        _typeIndex = 2;
    }

    public bool IsT0 => _typeIndex == 1;
    public bool IsT1 => _typeIndex == 2;

    public T0 AsT0 => IsT0 ? _value1! : throw new InvalidOperationException($"Not a {typeof(T0)}");
    public T1 AsT1 => IsT1 ? _value2! : throw new InvalidOperationException($"Not a {typeof(T1)}");

    public override string ToString() =>
        _typeIndex switch
        {
            1 => _value1!.ToString()!,
            2 => _value2!.ToString()!,
            _ => throw new InvalidOperationException("Invalid OneOf state")
        };

    public T Match<T>(Func<T0, T> caseT0, Func<T1, T> caseT1) =>
        _typeIndex switch
        {
            1 => caseT0(_value1!),
            2 => caseT1(_value2!),
            _ => throw new InvalidOperationException("Invalid OneOf state")
        };

	public void Match(Action<T0> caseT1, Action<T1> caseT2)
	{
		if (_typeIndex == 1)
			caseT1(_value1!);
		else if (_typeIndex == 2)
			caseT2(_value2!);
		else
			throw new InvalidOperationException("Invalid OneOf state");
	}

	public void MatchT0(Action<T0> caseT0)
	{
		if (_typeIndex == 1)
			caseT0(_value1!);
	}

	public void MatchT1(Action<T1> caseT1)
	{
		if (_typeIndex == 2)
			caseT1(_value2!);
	}

	public static implicit operator OneOf<T0, T1>(T0 value) => new (value);
	public static implicit operator OneOf<T0, T1>(T1 value) => new (value);

}
