using System.Collections;
using System.Collections.Generic;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class RealtSequence : IEnumerable<Search>
    {
        private readonly int[] YearRanges = { 1900, 1950, 1960, 1970, 1980, 1990, 2000, 2005, 2010, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022 };

        public IEnumerator<Search> GetEnumerator()
        {
            for (var y = 0; y < YearRanges.Length; y++)
            {
                var search = new Search
                {
                    YearFrom = YearRanges[y],
                    YearTo = (y < YearRanges.Length - 1) ? (int?)YearRanges[y + 1] - 1 : null
                };

                yield return search;
            }

            // find all without filters (add items without year)
            yield return new Search();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
