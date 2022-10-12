using System.Globalization;

namespace CSVStat
{
    public sealed class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey? x, TKey? y)
        {
            if (x == null || y == null)
                return 0;
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;
            else
                return result;
        }

        #endregion
    }
    public sealed class CLI
    {
        private static void NewOrJoin(IList<string> l, int index, string toInsert, string delimiter) {
            if (index >= l.Count()) {
                while (index >= l.Count())
                    l.Add("");
                l[index] = toInsert;
            } else {
                l[index] += delimiter + toInsert;
            }
        }
        private static readonly int headerRowBegin = 1;
        private static readonly int headerRowEnd = 3;
        private static readonly int nameColBegin = 0;
        private static readonly int nameColEnd = 1;
        public static int Enter(string[] args)
        {
            var columnNames = new List<string>();
            var recordRatings = new Dictionary<string, SortedList<double, string>>();
            using (var reader = new StreamReader(@"assets/vegconsumption.csv"))
            {
                int currentRecord = 0;
                for (; currentRecord < headerRowBegin; currentRecord++)
                    reader.ReadLine();

                string? row;
                for (; currentRecord < headerRowEnd; currentRecord++) {
                    row = reader.ReadLine() ?? throw new Exception($"Header rows parsing: met problem reading row #{currentRecord + 1}!");
                    var columns = row.Split(';');
                    var columnName = "";
                    for (int i = nameColEnd; i < columns.Count(); i++) {
                        if (columns[i] != "")
                            columnName = columns[i];
                        NewOrJoin(columnNames, i, columnName, ": ");
                    }
                }

                for (; (row = reader.ReadLine()) != null; currentRecord++) {
                    var columns = row.Split(';');
                    string recordName = "";
                    int currentColumn = nameColBegin;
                    for (; currentColumn < nameColEnd; currentColumn++) {
                        if (columns[currentColumn].Length == 0)
                            continue;
                        recordName = (recordName.Length == 0) ? columns[currentColumn] : recordName + ": " + columns[currentColumn];
                    }
                    for (; currentColumn < columns.Length; currentColumn++) {
                        double cellValue;
                        if (!Double.TryParse(columns[currentColumn], NumberStyles.Any, CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"), out cellValue))
                            continue;
                        if (!recordRatings.ContainsKey(columnNames[currentColumn]))
                            recordRatings.Add(columnNames[currentColumn], new SortedList<double, string>(new DuplicateKeyComparer<double>()));
                        recordRatings[columnNames[currentColumn]].Add(cellValue, recordName);
                    }
                }

                foreach (var recordRating in recordRatings) {
                    Console.WriteLine($"Statistics for '{recordRating.Key}':");
                    var minimalValuePair = recordRating.Value.First();
                    var maximumValuePair = recordRating.Value.Last();
                    var averageValue = recordRating.Value.Keys.Aggregate(0.0, (acc, x) => acc += x) / recordRating.Value.Count;
                    var varianceValue = recordRating.Value.Keys.Aggregate(0.0, (acc, x) => acc += Math.Pow(x - averageValue, 2)) / (recordRating.Value.Count - 1);
                    Console.WriteLine($"\tMinimum: {minimalValuePair.Value} ({minimalValuePair.Key:F2})");
                    Console.WriteLine($"\tMaximum: {maximumValuePair.Value} ({maximumValuePair.Key:F2})");
                    Console.WriteLine($"\tAverage: {averageValue:F2}");
                    Console.WriteLine($"\tVariance: {varianceValue:F2}");
                }
            }
            return 0;
        }
    }
}
