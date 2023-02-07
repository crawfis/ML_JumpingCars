using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrawfisSoftware.AssetManagement
{
    /// <summary>
    /// Abstract base class for some asynchronous implementations of IPooler
    /// </summary>
    /// <typeparam name="T">The type of instances in the pool.</typeparam>
    public abstract class PoolerBaseAsync<T> : IAssetManagerAsync<T> where T : class
    {
        private readonly Dictionary<string, Queue<T>> _pools = new Dictionary<string, Queue<T>>();
        private readonly Dictionary<T,string> _allocatedAssets = new Dictionary<T,string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public PoolerBaseAsync()
        {
            //InitPool(initialSize, maxPersistentSize, collectionChecks);
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync(string name)
        {
            T instance;
            if (!_pools.TryGetValue(name, out Queue<T> pool))
            {
                pool = new Queue<T>();
                _pools[name] = pool;
            }
            if (pool.Count == 0)
            {
                instance = await CreateNewPoolInstanceAsync(name);
            }
            else
            {
                instance = pool.Dequeue();
            }
            if (instance != null)
            {
                _allocatedAssets.Add(instance, name);
                ReinitializePoolInstance(instance);
                return instance;
            }
            return null;
        }

        /// <inheritdoc/>
        public Task ReleaseAsync(T poolObject)
        {
            if (_allocatedAssets.TryGetValue(poolObject, out string poolName))
            {
                _pools[poolName].Enqueue(poolObject);
                _allocatedAssets.Remove(poolObject);
                ReturnPoolInstance(poolObject);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ReleaseAllAsync()
        {
            foreach (var (asset,poolName) in _allocatedAssets)
            {
                _pools[poolName].Enqueue(asset);
            }
            //_pools.Clear();
            _allocatedAssets.Clear();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public abstract IEnumerable<string> AvailableAssets();

        //protected void InitPool(int initial = 10, int maxPersistentSize = 20, bool collectionChecks = false)
        //{
        //}

        /// <summary>
        /// Called by the pooling system when a new instance is added to the pool.
        /// </summary>
        /// <param name="name">The name of the asset to create.</param>
        /// <returns>A Task</returns>
        protected abstract Task<T> CreateNewPoolInstanceAsync(string name);

        /// <summary>
        /// Called by the pooling system when an existing object is re-activated from the pool.
        /// </summary>
        /// <param name="poolObject">The instance of the pooled object that is being reactived.</param>
        protected abstract void ReinitializePoolInstance(T poolObject);

        /// <summary>
        /// Called by the pooling system when an existing object is returned to the pool.
        /// </summary>
        /// <param name="poolObject">The instance of the pooled object that is being deactivated.</param>
        protected abstract void ReturnPoolInstance(T poolObject);

        /// <summary>
        /// Called by the pooling system when an existing object is being destroyed and removed from the pool.
        /// </summary>
        /// <param name="poolObject">The instance of the pooled object that is being destroyed.</param>
        protected abstract Task DestroyPoolInstanceAsync(T poolObject);

        /// <summary>
        /// Placeholder for initializing concrete implementations.
        /// </summary>
        /// <returns>A Task</returns>
        public abstract Task Initialize();
    }
}