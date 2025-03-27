namespace GooseHtml;

public class ThreadSafeLoopGuard(int max = int.MaxValue)
{
    private int counter = 0;
    private readonly int maxIterations = max;

    public bool ShouldContinue(string loopName)
    {
        int newValue = Interlocked.Increment(ref counter);
		bool inRange = newValue <= maxIterations && newValue != int.MaxValue;

		if (inRange == false)
		{
			throw new Exception($"Loop iteration limit exceeded in {loopName}");
		}
		return inRange;
    }
}

