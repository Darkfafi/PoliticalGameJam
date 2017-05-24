using UnityEngine;
using UnityEngine.UI;

public class SunMovement : MonoBehaviour {

	[SerializeField]private Image _sun;
	[SerializeField]private Image _moon;
	private Vector2 _sunPos;
	private Vector2 _moonPos;
	private Vector2 _rotPos;

	[SerializeField][Range(-10.5f, 5)]
	private float _rotHeight = -1;
	private float _speed = 10;
	private float _flow = 0;
	private bool _day = true;
	private Animator _shadow;

	public bool isDay { get { return _day; } }

	public Vector2 sunPos { get { return _sunPos; } }

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
		StartShadow();
		if(_sunPos.x > 8.4 && _day) {
			_flow = (0.1f/_speed)*45;
			_sunPos = new Vector2(-10,_rotHeight);
			_moonPos = new Vector2(-10,_rotHeight);
			_day = false;
		} else {
			if(_day) {
				_flow+=0.1f/_speed*0.4f;	
				_sunPos.x += Mathf.Sin(_flow)/_speed*0.4f;
				_sunPos.y += Mathf.Cos(_flow)/_speed*0.4f;
				_sun.transform.position = _sunPos + _rotPos;
			}
		}

		if(_moonPos.x > 8.4 && !_day) {
			_flow = (0.1f/_speed)*45;
			_sunPos = new Vector2(-10,_rotHeight);
			_moonPos = new Vector2(-10,_rotHeight);
			_day = true;
		} else {
			if(!_day) {
				_flow+=0.1f/_speed;	
				_moonPos.x += Mathf.Sin(_flow)/_speed;
				_moonPos.y += Mathf.Cos(_flow)/_speed;
				_moon.transform.position = _moonPos + _rotPos;
			}
		}
	}

	void StartShadow() {
		_shadow.SetBool("Start", _day);
	}
}
