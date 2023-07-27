using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers instance;
    private static Managers Instance { get { Init(); return instance; } }

    #region Contents
    private GameManager game = new GameManager();
    public static GameManager Game => Instance.game;
    #endregion

    #region Core
    private DataManager data = new DataManager();
    private InputManager input = new InputManager();
    private PoolManager pool = new PoolManager();
    private ResourceManager resource = new ResourceManager();
    private SceneManagerEx scene = new SceneManagerEx();
    private SoundManager sound = new SoundManager();
    private UIManager ui = new UIManager();

    public static DataManager Data => Instance.data;
    public static InputManager Input => Instance.input;
    public static PoolManager Pool => Instance.pool;
    public static ResourceManager Resource => Instance.resource;
    public static SceneManagerEx Scene => Instance.scene;
    public static SoundManager Sound => Instance.sound;
    public static UIManager UI => Instance.ui;
    #endregion

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

            instance.data.Init();
            instance.pool.Init();
            instance.sound.Init();
        }
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}