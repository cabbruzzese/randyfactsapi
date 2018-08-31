using System;
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

        private static DateTime lastRetrieved;
        private static readonly Random rand = new Random();

        //Going to improve this object by creating a cache that stores results of various queries and only reruns queries after timeouts
        private static List<Fact> _facts;
        private static object _lock = new object();
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
            lock(_lock)
            {
                _facts = RetrieveFile();
                lastRetrieved = DateTime.Now;
            }

            return true;
        }

        public IEnumerable<Fact> GetFacts ()
        {
            return facts;
        }

        public int HowMany()
        {
            return facts.Count();
        }

        public int HowMany(string token)
        {
            return List(token).Count();
        }

        public Fact GetRandom()
        {
            int factPosition = rand.Next(0, facts.Count() - 1);

            return facts[factPosition];
        }

        public Fact GetRandom(string token)
        {
            var matches = List(token);
            int factPosition = rand.Next(0, matches.Count() - 1);

            return matches.ElementAt(factPosition);
        }

        public IEnumerable<Fact> List(string token)
        {
            return facts.Where(f => f.Content.Contains(token));
        }
    }
}
