using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    using Application.Contracts;
    using Application.Models;
    using System.Collections.Concurrent;

    public class MemoryPkceStore : IPkceStore
    {
        private readonly ConcurrentDictionary<string, PkceData> _store = new();

        public Task SaveAsync(PkceData pkce)
        {
            _store[pkce.State] = pkce;

            return Task.CompletedTask;
        }

        public Task<PkceData?> GetAsync(string state)
        {
            _store.TryGetValue(state, out var pkce);

            return Task.FromResult(pkce);
        }

        public Task RemoveAsync(string state)
        {
            _store.TryRemove(state, out _);

            return Task.CompletedTask;
        }
    }
}
