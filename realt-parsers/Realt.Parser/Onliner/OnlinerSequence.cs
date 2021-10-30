using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Realt.Parser.Model;

namespace Realt.Parser
{
    public class OnlinerSequence : IEnumerable<Search>
    {
        private readonly int[] YearRanges = { 1900, 1950, 1960, 1970, 1980, 1990, 2000, 2005, 2010, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022 };
        private readonly int[] RoomRanges = { 1, 2, 3, 4, 5 };
        private const int MaxRooms = 6;

        public IEnumerator<Search> GetEnumerator()
        {
            for (var y = 0; y < YearRanges.Length; y++)
            {
                for (var r = 0; r < RoomRanges.Length; r++)
                {
                    var search = new Search();
                    search.YearFrom = YearRanges[y];
                    search.YearTo = (y < YearRanges.Length - 1) ? (int?)YearRanges[y + 1] - 1 : null;

                    var rooms = RoomRanges[r];
                    if (r < RoomRanges.Length - 1)
                    {
                        search.Rooms = new int[] { rooms };
                    }
                    else
                    {
                        search.Rooms = Enumerable.Range(rooms, MaxRooms - rooms + 1).ToArray();
                    }

                    yield return search;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}