using System.Collections.Generic;
using UnityEngine;
using System;
using Grace.DependencyInjection.Attributes;
using Object = UnityEngine.Object;

namespace RedPanda.Project.Services.ObjectPool
{


    [Serializable]
    public class ObjectPool
    {
        [SerializeField] private GameObject _original;
        [SerializeField] private bool _scaleToZero = true;

        protected static Dictionary<GameObject, Action<bool>> AvailablePoolElements = new();

        public int AvailableNum => Queue.Count;
        public GameObject Original => _original;

        private Queue<GameObject> _queue = new();

        private Queue<GameObject> Queue
        {
            get { return _queue ??= new Queue<GameObject>(); }
        }

        private IObjectPoolService _objectPoolService;

        public static bool ReturnToPool(GameObject obj)
        {
            if (AvailablePoolElements.TryGetValue(obj, out Action<bool> pool))
            {
                AvailablePoolElements.Remove(obj);
                pool.Invoke(true);
                return true;
            }

            return false;
        }
        
        [Import]
        public void Inject(IObjectPoolService objectPoolService)
        {
            _objectPoolService = objectPoolService;
        }

        public T Get<T>() where T : MonoBehaviour
        {
            T result = Get<T>(Vector3.zero, Quaternion.identity, null);
            result.transform.localPosition = Vector3.zero;

            return result;
        }

        public T Get<T>(Vector3 position) where T : MonoBehaviour
        {
            return Get<T>(position, Quaternion.identity, null);
        }

        public T Get<T>(Transform parent) where T : MonoBehaviour
        {
            T result = Get<T>(Vector3.zero, Quaternion.identity, parent);
            result.transform.localPosition = Vector3.zero;
            return result;
        }

        public T Get<T>(Vector3 position, Transform parent) where T : MonoBehaviour
        {
            return Get<T>(position, Quaternion.identity, parent);
        }

        public T Get<T>(Vector3 position, Quaternion rotation, Transform parent) where T : MonoBehaviour
        {
            var obj = Queue.Count == 0 ? Object.Instantiate(Original) : Queue.Dequeue();
            var res = obj.GetComponent<T>();
            AvailablePoolElements.Add(obj, (recursively) => SimpleReturnToPool<T>(obj, recursively));

            res.gameObject.transform.SetParent(parent, false);
            res.gameObject.transform.position = position;
            res.gameObject.transform.rotation = rotation;

            SimpleGetFromPool(res);

            return res;
        }

        private void SimpleGetFromPool<T>(T item) where T : MonoBehaviour
        {
            if (!_scaleToZero)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.transform.localScale = Original.transform.localScale;
            }

            if (item is IOnGetFromPool getable)
            {
                getable.OnGetFromPool();
            }
        }

        private void SimpleReturnToPool<T>(GameObject obj, bool recursively) where T : MonoBehaviour
        {
            T item = obj.GetComponent<T>();

            if (!_scaleToZero)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.transform.localScale = Vector3.zero;
            }

            Queue.Enqueue(item.gameObject);

            if (item is IOnReturnToPool returnable)
            {
                returnable.OnReturnToPool();
            }

            if (item.transform is RectTransform)
            {
                item.transform.SetParent(_objectPoolService.UiObjectHolder, false);
            }
            else
            {
                item.transform.SetParent(_objectPoolService.ObjectHolder, false);
            }

            if (recursively == false) return;

            foreach (var child in item.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject == obj) continue;

                if (AvailablePoolElements.TryGetValue(child.gameObject, out Action<bool> pool))
                {
                    AvailablePoolElements.Remove(child.gameObject);
                    pool.Invoke(false);
                }
            }
        }
    }
}