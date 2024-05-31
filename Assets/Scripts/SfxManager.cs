using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MBExtensions;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;
	private AudioSource audioSource;

    [System.Serializable] public enum Sfx { Message, CameraSwap };

	[Serializable]
	public class SfxReference
    {
        public Sfx type;
		public AudioClip clip;
    }[Serializable]
	public class SfxReference2
    {
        public Sfx type;
		public List<AudioClip> clip;
		[Range(0.0f, 1.0f)] public float vol = 1.0f;
    }

    [Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<TKey> keys = new List<TKey>();

		[SerializeField]
		private List<TValue> values = new List<TValue>();

		// save the dictionary to lists
		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();
			foreach (KeyValuePair<TKey, TValue> pair in this)
			{
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		// load dictionary from lists
		public void OnAfterDeserialize()
		{
			this.Clear();

			if (keys.Count != values.Count)
				throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

			for (int i = 0; i < keys.Count; i++)
				this.Add(keys[i], values[i]);
		}
	}
	[Serializable] public class DictionaryOfStringAndInt : SerializableDictionary<int, AudioClip> { }

    // public AudioClip testClip;
    //[SerializeField] public DictionaryOfStringAndInt sfxDict;
	//[SerializeField] public SfxReference newMessage;
	[SerializeField] private List<SfxReference> sfxReferences;
	[SerializeField] private List<SfxReference2> sfxReferences2;
	private Dictionary<Sfx, AudioClip> sfxReferencesDict;



	private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this);

		audioSource = GetComponent<AudioSource>();

		sfxReferencesDict = new();
		foreach (var reference in sfxReferences)
			sfxReferencesDict.TryAdd(reference.type, reference.clip);
	}

    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(this.DelayedExecution(() => PlaySfx(Sfx.CameraSwap), 2f));
		StartCoroutine(this.DelayedExecution(() => PlaySfx(Sfx.Message), 3f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySfx(Sfx sfx)
    {
		sfxReferencesDict.TryGetValue(sfx, out var clip);
		if(clip is AudioClip)
        {
			audioSource.clip = clip;
			audioSource.Play();
        }
    }
}
