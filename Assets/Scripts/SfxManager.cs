using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MBExtensions;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;
	private List<AudioSource> audioSource;

    [System.Serializable] public enum Sfx { Message, CameraSwap, CameraSwapCompleted, Red, Green, Blue };

	[Serializable]
	public class SfxReference2
    {
        public Sfx type;
		public List<AudioClip> clip;
		[Range(0.0f, 1.0f)] public float vol = 1.0f;
    }

	/*
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
	*/

	[SerializeField] private List<SfxReference2> sfxReferences;
	//[SerializeField] private SortedList<Sfx, SfxReference2> sfxReferences3;
	//[SerializeField] private HashSet<SfxReference2> sfxReferences4;

	private Dictionary<Sfx, SfxReference2> sfxReferencesDict;



	private void Awake()
    {
        TooManyFuncts.Singletonize(ref Instance, this);

		audioSource = new();
		audioSource = new(GetComponents<AudioSource>());

		sfxReferencesDict = new();
		foreach (var reference in sfxReferences)
			sfxReferencesDict.TryAdd(reference.type, reference);
	}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySfx(Sfx sfx)
    {
		/*
		sfxReferencesDict.TryGetValue(sfx, out var clip);
		if(clip is AudioClip)
        {
			audioSource.clip = clip;
			audioSource.Play();
        }
		*/
		sfxReferencesDict.TryGetValue(sfx, out var bundle);
		var bundle2 = sfxReferences.Find(x => x.type == sfx);
		if(bundle is SfxReference2)
        {
			AudioSource usedSrc = audioSource.Find(x => !x.isPlaying);
			if (usedSrc is not AudioSource)
				return;
			var randomClip = UnityEngine.Random.Range(0, bundle.clip.Count);
			if (!(bundle.clip.Count > randomClip))
				throw new IndexOutOfRangeException("---HEY! Empty List element in " + bundle.type.ToString() + "---");

			usedSrc.clip = bundle.clip[randomClip];	
			usedSrc.volume = bundle.vol;

			//usedSrc.clip = sfxReferences2.Find(x => x.type == sfx).clip[0];
			usedSrc.Play();
        }
    }
}
