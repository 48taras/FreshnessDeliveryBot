using Microsoft.Extensions.Configuration;
using System;

namespace FreshnessDeliveryBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var freshnessBot = new FreshnessBot("token");
        }
    }
}
