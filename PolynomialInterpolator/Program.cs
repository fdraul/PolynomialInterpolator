namespace PolynomialInterpolator;

public class Program
{
    public static void Main()
    {
        List<double> xs = new(), ys = new();
        for (; ; )
        {
            double x, y;
            if (!double.TryParse(Input($"x{xs.Count + 1}="), out x)) break;
            for (; ; ) if (double.TryParse(Input($"y{ys.Count + 1}="), out y)) break;
            xs.Add(x);
            ys.Add(y);
        }

        Console.Clear();

        double[,] matrix = new double[xs.Count, xs.Count];
        for (int i = 0; i < xs.Count; i++)
        {
            Console.WriteLine($"f({xs[i]})={ys[i]}");
            for (int _i = 0; _i < xs.Count; _i++) matrix[i, _i] = double.Pow((double)xs[i], xs.Count - _i - 1);
        }

        Console.WriteLine();

        double determinant = GetDeterminant(matrix);
        if (determinant == 0)
        {
            Console.WriteLine("The data provided is not valid.");
            return;
        }

        double[] coefficients = new double[xs.Count];
        for (int i = 0; i < xs.Count; i++)
        {
            double[,] _matrix = (double[,])matrix.Clone();
            for (int _i = 0; _i < ys.Count; _i++) _matrix[_i, i] = ys[_i];
            coefficients[i] = GetDeterminant(_matrix) / determinant;
        }

        string str = string.Join("", coefficients.Select((c, i) =>
        {
            if (c == 0) return "";
            string _c = c == 1 && i != coefficients.Length - 1 ? "" : c.ToString();
            string str = i == coefficients.Length - 1 ? _c : i == coefficients.Length - 2 ? $"{_c}x" : $"{_c}x^{coefficients.Length - i - 1}";
            return $"+{str}";
        })).Trim('+').Replace("+-", "-");
        if (str.Length == 0) str = "0";

        Console.WriteLine($"f(x)={str}");
    }

    private static string Input(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? "";
    }

    private static double GetDeterminant(double[,] matrix)
    {
        if (matrix.GetLength(0) == 1) return matrix[0, 0];

        double sum = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            double[,] _matrix = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
            for (int _i = 0; _i < matrix.GetLength(0); _i++)
            {
                if (_i == i) continue;
                for (int _j = 0; _j < matrix.GetLength(1); _j++)
                {
                    if (_j == 0) continue;
                    _matrix[_i > i ? _i - 1 : _i, _j - 1] = matrix[_i, _j];
                }
            }

            sum += matrix[i, 0] * (int)Math.Pow(-1, i + 2) * GetDeterminant(_matrix);
        }

        return sum;
    }
}
