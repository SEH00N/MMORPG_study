using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance { get { Init(); return instance; } }

    private InputManager input = new InputManager();
    private ResourceManager resource = new ResourceManager();
    private SceneManagerEx scene = new SceneManagerEx();
    private SoundManager sound = new SoundManager();
    private UIManager ui = new UIManager();

    public static InputManager Input => Instance.input;
    public static ResourceManager Resource => Instance.resource;
    public static SceneManagerEx Scene => Instance.scene;
    public static SoundManager Sound => Instance.sound;
    public static UIManager UI => Instance.ui;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        input.OnUpdate();
    }

    private static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("Managers");
            if(go == null)
            {
                go = new GameObject("Managers");
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();

            instance.sound.Init();
        }
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
    }
}