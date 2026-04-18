using UnityEngine;

namespace ITU
{
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
	{
		private static readonly object _lock = new object();
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					lock (_lock)
					{
						T[] instances;
#if UNITY_6000_0_OR_NEWER
						instances = FindObjectsByType<T>(FindObjectsSortMode.None);
#else
                instances = FindObjectsOfType<T>();
#endif
						if (instances.Length > 0)
						{
							instance = instances[0];
							for (int i = 1; i < instances.Length; i++)
							{
								Destroy(instances[0]);
							}
						}
						else
						{
							instance = new GameObject(typeof(T).Name).AddComponent<T>();
						}
						instance.Initialize();
					}
				}
				return instance;
			}
		}

		[field: SerializeField] public virtual bool IsPersistent { get; private set; } = true;

		protected virtual void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(this);
				return;
			}

			instance = this as T;
			Initialize();
		}

		protected void Initialize()
		{
			if (IsPersistent)
			{
				DontDestroyOnLoad(gameObject);
			}
			OnInitialize();
		}

		protected virtual void OnInitialize() { }
	}
}
