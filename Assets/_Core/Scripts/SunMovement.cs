using UnityEngine;
using UnityEngine.UI;

public class SunMovement : MonoBehaviour
{
    public delegate void VoidDelegate();
    public event VoidDelegate DayEndedEvent;
    public event VoidDelegate DayStartedEvent;

	[SerializeField]private SpriteRenderer _sun;
	[SerializeField]private SpriteRenderer _moon;
	private Vector2 _sunPos;
	private Vector2 _moonPos;
	private Vector2 _rotPos;

	[SerializeField][Range(-10.5f, 5)]
	private float _rotHeight = -1;
	[SerializeField][Range(0, 1)]
	private float _sunSpeed = 0.2f;
	[SerializeField][Range(0, 1)]
	private float _moonSpeed = 1;
	private float _speed = 10;
	private float _flow = 0;
	private bool _day = true;
	private bool _animation = true;
	private Animator _shadow;

	public bool isDay { get { return _day; } }
    public bool isRunning = true;

    public Vector2 sunPos { get { return _sunPos; } }

    private float _timerPaper = -1;

	void Awake() {
		_shadow = GameObject.FindWithTag("Shadow").GetComponent<Animator>();
	}

	void Start () {
		_rotPos = new Vector2(1,_rotHeight);
		_sunPos = new Vector2(-10,_rotHeight);
		_moonPos = new Vector2(-10,_rotHeight);
		_flow = (0.1f/_speed)*45;
		_sun.transform.position = _sunPos + _rotPos;
		_moon.transform.position = _moonPos + _rotPos;
	}
	void Update () {

        if (_timerPaper != -1)
        {
            _timerPaper += Time.deltaTime;
            if(_timerPaper >= 5f)
            {
                _timerPaper = 0;
            }
        }

        if (!isRunning) { return; }
		if(_sunPos.x > 8.4 && _day) {
			_flow = (0.1f/_speed)*45;
			_sunPos = new Vector2(-10,_rotHeight);
			_moonPos = new Vector2(-10,_rotHeight);
			_day = false;
			EndDay();
		} else {
			if(_day) {
				if(_animation) {
					StartShadow();
					_animation = false;
				}
				_flow+=0.1f/_speed*_sunSpeed;	
				_sunPos.x += Mathf.Sin(_flow)/_speed*_sunSpeed;
				_sunPos.y += Mathf.Cos(_flow)/_speed*_sunSpeed;
				_sun.transform.position = _sunPos + _rotPos;
			}
		}

		if(_moonPos.x > 8.4 && !_day) {
			_flow = (0.1f/_speed)*45;
			_sunPos = new Vector2(-10,_rotHeight);
			_moonPos = new Vector2(-10,_rotHeight);
			_day = true;
			StartShadow();
			StartDay();
		} else {
			if(!_day) {
				_flow+=0.1f/_speed*_moonSpeed;	
				_moonPos.x += Mathf.Sin(_flow)/_speed*_moonSpeed;
				_moonPos.y += Mathf.Cos(_flow)/_speed*_moonSpeed;
				_moon.transform.position = _moonPos + _rotPos;
				if (!_animation)
					_animation = true;
			}
		}
	}

	void StartShadow() {
		_shadow.SetTrigger("Start");
	}

    //wanneer dag start
    void StartDay()
    {
        NewspaperManager.instance.PublishNewspaper("test", NewspaperManager.newspaperStates.Neutral);

        isRunning = false;
        this._timerPaper = 0;

        if (DayStartedEvent != null)
            DayStartedEvent();

    }

	//wanneer dag eindigt
	void EndDay()
    {

        if (this.DayEndedEvent != null)
            DayEndedEvent();
	}
}