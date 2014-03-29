using System;
using System.Threading;
using System.Threading.Tasks;

namespace TorExitNodeManager
{
    internal abstract class MemoryCacheBase<T>
    {
        protected TimeSpan _ttl = TimeSpan.FromMinutes(new Random().Next(5, 10));
        private readonly ManualResetEventSlim _hasDataLock = new ManualResetEventSlim(false);
        private readonly AutoResetEvent _populatingLock = new AutoResetEvent(true);
        private readonly ReaderWriterLockSlim _updateLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private T _cache;
        private DateTime _cacheLastUpdated = DateTime.MinValue;

        public T Data
        {
            get
            {
                CheckIfPopulated();
                try
                {
                    if (_updateLock.TryEnterReadLock(2500))
                        return _cache;
                }
                finally { _updateLock.ExitReadLock(); }
                return default(T);
            }
        }

        public void Invalidate()
        {
            PopulateCache(true);
        }

        protected abstract T FetchCacheData();

        private void CheckIfPopulated()
        {
            if (DateTime.UtcNow.Subtract(_cacheLastUpdated) > _ttl)
            {
                Task.Factory.StartNew(new Action(PopulateCache));

                _hasDataLock.Wait();
            }
        }

        private void PopulateCache()
        {
            PopulateCache(false);
        }

        private void PopulateCache(bool force)
        {
            if (_populatingLock.WaitOne(1))
            {
                try
                {
                    if (force || DateTime.UtcNow.Subtract(_cacheLastUpdated) > _ttl)
                    {
                        T newCache = FetchCacheData();

                        if (_updateLock.TryEnterWriteLock(10000))
                        {
                            try
                            {
                                _cache = newCache;
                                _cacheLastUpdated = DateTime.UtcNow;
                            }
                            catch { }
                            finally
                            {
                                _updateLock.ExitWriteLock();
                                _hasDataLock.Set();
                            }
                        }
                    }
                }
                catch { }
                finally { _populatingLock.Set(); }
            }
        }
    }
}