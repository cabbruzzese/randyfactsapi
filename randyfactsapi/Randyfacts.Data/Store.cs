using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Randyfacts.Data.Models;

namespace Randyfacts.Data
{
    public class Store
    {
        private const int StaleTimeoutSeconds = 300; //Five minutes in seconds
        private const int MaxCountCacheEntries = 25000; // Count cache is much larger
        private const int MaxCacheEntries = 1000; //only cache so many search results
        private const int MaxCacheContentSize = 1000; //only cache small search results

        private static DateTime lastRetrieved;
        private static readonly Random rand = new Random();

        private static ConcurrentDictionary<string, IEnumerable<Fact>> _listCache;
        private static ConcurrentDictionary<string, int> _countCache;
        private static int _count;
        private static List<Fact> _facts;
        private static object _lockObj = new object();
        private List<Fact> facts
        {
            get
            {
                if (_facts == null || lastRetrieved < DateTime.Now.AddSeconds(-StaleTimeoutSeconds))
                {
                    FetchFacts();
                }

                return _facts;
            }
        }
        private ConcurrentDictionary<string, IEnumerable<Fact>> listCache
        {
            get
            {
                if (_facts == null || lastRetrieved < DateTime.Now.AddSeconds(-StaleTimeoutSeconds))
                {
                    FetchFacts();
                }

                return _listCache;
            }
        }
        private ConcurrentDictionary<string, int> countCache
        {
            get
            {
                if (_facts == null || lastRetrieved < DateTime.Now.AddSeconds(-StaleTimeoutSeconds))
                {
                    FetchFacts();
                }

                return _countCache;
            }
        }
        private int count
        {
            get
            {
                if (_facts == null || lastRetrieved < DateTime.Now.AddSeconds(-StaleTimeoutSeconds))
                {
                    FetchFacts();
                }

                return _count;
            }
        }

        private static List<Fact> RetrieveFile()
        {
            string result = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                result = httpClient.GetStringAsync("http://randyfacts.meadowmanager.com/randyfacts.txt").Result;
            }

            var factResult = result.Split("\n").Select(r => new Fact { Content= r});

            return factResult.ToList();
        }

        private bool FetchFacts()
        {
            lock(_lockObj)
            {
                //clear cache on fetch
                _listCache = new ConcurrentDictionary<string, IEnumerable<Fact>>();
                _countCache = new ConcurrentDictionary<string, int>();

                _facts = RetrieveFile();
                _count = _facts.Count();
                lastRetrieved = DateTime.Now;
            }

            return true;
        }

        /// <summary>
        /// Gets count of all facts and caches result.
        /// </summary>
        /// <returns></returns>
        private int GetCount()
        {
            return count;
        }

        /// <summary>
        /// Gets count based on token. Calls GetList which handles Caching of count.
        /// </summary>
        /// <param name="token">Search phrase</param>
        /// <returns></returns>
        private int GetCount(string token)
        {
            if (countCache.ContainsKey(token))
            {
                return countCache[token];
            }

            return GetList(token).Count();
        }

        /// <summary>
        /// Gets list of facts based on token from cache if available, otherwise adds result to cache.
        /// Note: Doesn't add to the cache if cache is full or results are too lage.
        /// Note: Also adds count to cache of Counted tokens.
        /// </summary>
        /// <param name="token">Search phrase</param>
        /// <returns></returns>
        private IEnumerable<Fact> GetList(string token)
        {
            if (listCache != null && listCache.ContainsKey(token))
            {
                return listCache[token];
            }

            var matches = facts.Where(f => f.Content.Contains(token));
            int howMany = matches.Count();

            if (listCache.Count() < MaxCacheEntries && howMany < MaxCacheContentSize)
            {
                lock (_lockObj)
                {
                    listCache.TryAdd(token, matches);                    
                }
            }

            if (countCache != null && countCache.Count() < MaxCountCacheEntries)
            {
                lock (_lockObj)
                {
                    countCache.TryAdd(token, howMany);
                }
            }

            return matches;
        }

        public IEnumerable<Fact> GetFacts ()
        {
            return facts;
        }

        public int HowMany()
        {
            return GetCount();
        }

        public int HowMany(string token)
        {
            return GetCount(token);
        }

        public Fact GetRandom()
        {
            int factPosition = rand.Next(0, GetCount() - 1);

            return facts[factPosition];
        }

        public Fact GetRandom(string token)
        {
            var matches = GetList(token);

            if (!matches.Any())
            {
                return null;
            }

            int factPosition = rand.Next(0, GetCount(token) - 1);

            return matches.ElementAt(factPosition);
        }

        public IEnumerable<Fact> List(string token)
        {
            return GetList(token);
        }
    }
}
