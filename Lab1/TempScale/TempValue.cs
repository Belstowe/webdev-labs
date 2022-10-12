namespace TempScale
{
    public sealed class TempValue
    {
        public double Kelvin { get; init; }
        public double Celsius => Kelvin - 273.15;
        public double Fahrenheit => (Celsius * 9) / 5 + 32;

        public static TempValue FromKelvin(double k)
        {
            return new TempValue{Kelvin = k};
        }

        public static TempValue FromCelsius(double c)
        {
            return new TempValue{Kelvin = c + 273.15};
        }

        public static TempValue FromFahrenheit(double f)
        {
            return new TempValue{Kelvin = (5 * (f - 32)) / 9 + 273.15};
        }
    }
}
