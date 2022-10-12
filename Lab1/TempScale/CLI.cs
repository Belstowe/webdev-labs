using System.Text.RegularExpressions;

namespace TempScale
{
    public sealed class CLI
    {
        private static readonly Dictionary<string, Func<double, TempValue>> tempFromDouble = new Dictionary<string, Func<double, TempValue>> {
            ["K"] = kelvin => TempValue.FromKelvin(kelvin),
            ["F"] = fahrenheit => TempValue.FromFahrenheit(fahrenheit),
            ["C"] = celsius => TempValue.FromCelsius(celsius),
        };

        private static readonly Dictionary<string, Func<TempValue, double>> tempToDouble = new Dictionary<string, Func<TempValue, double>> {
            ["K"] = temp => temp.Kelvin,
            ["F"] = temp => temp.Fahrenheit,
            ["C"] = temp => temp.Celsius,
        };

        private static Regex inputTempRegex = new Regex(@"^([\+-]?(\d*\.)?\d+)\s*([KFC])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex outputTempMeasureRegex = new Regex(@"^([KFC])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static int Enter(string[] args)
        {
            Match matchForFormat;

            string inputTempLine;
            if (args.Length < 1) {
                Console.WriteLine("Please input temperature with original scale (f.ex. 25.2 C, 0 K, -10F).");
                inputTempLine = Console.ReadLine() ?? "";
                while (!inputTempRegex.IsMatch(inputTempLine)) {
                    Console.WriteLine("Invalid string, expected format '{double} {K|F|C}'.");
                    inputTempLine = Console.ReadLine() ?? "";
                }
            } else {
                inputTempLine = args[0];
                if (!inputTempRegex.IsMatch(inputTempLine)) {
                    Console.WriteLine($"'{inputTempLine}' Invalid argument, expected format '{{double}} {{K|F|C}}'.");
                    return 1;
                }
            }

            matchForFormat = inputTempRegex.Match(inputTempLine);
            double tempValue = Convert.ToDouble(matchForFormat.Groups[1].Value);
            string tempMeasure = matchForFormat.Groups[3].Value;
            TempValue temp = tempFromDouble[tempMeasure](tempValue);

            string outputTempMeasureLine;
            if (args.Length < 2) {
                Console.WriteLine("Please input destination measure (K for Kelvin, F for Fahrenheit, C for Celsius).");
                outputTempMeasureLine = Console.ReadLine() ?? "";
                while (!outputTempMeasureRegex.IsMatch(outputTempMeasureLine)) {
                    Console.WriteLine("Invalid string, expected format '{K|F|C}'.");
                    outputTempMeasureLine = Console.ReadLine() ?? "";
                }
            } else {
                outputTempMeasureLine = args[1];
                if (!outputTempMeasureRegex.IsMatch(outputTempMeasureLine)) {
                    Console.WriteLine($"'{outputTempMeasureLine}' Invalid argument, expected format '{{K|F|C}}'.");
                    return 1;
                }
            }

            matchForFormat = outputTempMeasureRegex.Match(outputTempMeasureLine);
            tempMeasure = matchForFormat.Groups[1].Value;
            Console.WriteLine($"The result is: {tempToDouble[tempMeasure](temp)} {tempMeasure}.");
            return 0;
        }
    }
}
