using ServiceLocator.Events;
using ServiceLocator.Map;
using ServiceLocator.Player;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using ServiceLocator.Wave;
using UnityEngine;

public class GameService : GenericMonoSingleton<GameService>
{
    public PlayerService playerService {  get; private set; }
    public SoundService soundService { get; private set; }
    public WaveService waveService { get; private set; }

    [Header("Player Service")]
    [SerializeField] private PlayerScriptableObject playerScriptableObject;

    [Header("Sound Service")]
    [SerializeField] private SoundScriptableObject soundScriptableObject;
    [SerializeField] private AudioSource audioEffects;
    [SerializeField] private AudioSource backgroundMusic;

    [Header("UI Service")]
    [SerializeField] private UIService uiService;

    public UIService UIService => uiService;

    [Header("Wave Service")]
    [SerializeField] private WaveScriptableObject waveScriptableObject;
    [SerializeField] private EventService eventService;

    [Header("Map Service")]
    [SerializeField] private MapService mapService;
    public MapService MapService => mapService;

    private void Start()
    {
        playerService = new PlayerService(playerScriptableObject);
        soundService = new SoundService(soundScriptableObject, audioEffects, backgroundMusic);
        waveService = new WaveService(waveScriptableObject, eventService);
    }

    private void Update()
    {
        playerService.Update();
    }
}
