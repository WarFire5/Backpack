class Program
{
    static void Main(string[] args)
    {
        int[] weights = { 3, 2, 1, 4, 5 };
        int[] values = { 25, 20, 15, 40, 50 };
        int W = 6;

        var (maxValue, numOptimalSubsets, dp) = BagTask(weights, values, W);
        var optimalSubsets = new List<List<int>>();
        FindOptimalSubsets(dp, weights, values, weights.Length, W, new List<int>(), optimalSubsets);

        Console.WriteLine($"Максимальная стоимость: {maxValue}");
        Console.WriteLine($"Число оптимальных вариантов: {numOptimalSubsets}");
        Console.WriteLine("Оптимальные предметы:");
        foreach (var subset in optimalSubsets)
        {
            Console.WriteLine($"[{string.Join(", ", subset)}]");
        }

        Console.WriteLine("\nПрограмма выполнена.");
        Console.ReadLine();
    }

    static (int maxValue, int numOptimalSubsets, int[,] dp) BagTask(int[] weights, int[] values, int W)
    {
        int n = weights.Length;
        int[,] dp = new int[n + 1, W + 1];
        int[,] count = new int[n + 1, W + 1];

        for (int i = 0; i <= n; i++)
        {
            count[i, 0] = 1; // Инициализация
        }

        for (int i = 1; i <= n; i++)
        {
            int wi = weights[i - 1];
            int vi = values[i - 1];
            for (int w = 0; w <= W; w++)
            {
                dp[i, w] = dp[i - 1, w];
                count[i, w] = count[i - 1, w];

                if (wi <= w)
                {
                    int takeValue = dp[i - 1, w - wi] + vi;
                    if (takeValue > dp[i, w])
                    {
                        dp[i, w] = takeValue;
                        count[i, w] = count[i - 1, w - wi];
                    }
                    else if (takeValue == dp[i, w])
                    {
                        count[i, w] += count[i - 1, w - wi];
                    }
                }
            }
        }

        int maxValue = dp[n, W];
        int numOptimalSubsets = count[n, W];
        return (maxValue, numOptimalSubsets, dp);
    }

    static void FindOptimalSubsets(int[,] dp, int[] weights, int[] values, int i, int w, List<int> current, List<List<int>> result)
    {
        if (i == 0)
        {
            result.Add(new List<int>(current));
            return;
        }

        int wi = weights[i - 1];
        int vi = values[i - 1];

        if (dp[i, w] == dp[i - 1, w])
        {
            FindOptimalSubsets(dp, weights, values, i - 1, w, current, result);
        }

        if (wi <= w && dp[i, w] == dp[i - 1, w - wi] + vi)
        {
            current.Add(i);
            FindOptimalSubsets(dp, weights, values, i - 1, w - wi, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }
}
