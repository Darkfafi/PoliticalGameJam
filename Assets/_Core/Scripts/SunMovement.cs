using UnityEngine;
using UnityEngine.UI;

public class SunMovement : MonoBehaviour {

	[SerializeField]private Image _sun;
	[SerializeField]private Image _moon;
	private Vector2 _sunPos = new Vector2(-10,0);
	private Vector2 _moonPos = new Vector2(10,0);
	private Vector2 _rotPos;

	[SerializeField][Range(-10.5f, 5)]
	private float _rotHeight = -1;


	private float _speed = 10;
	private float _flow = 0;
	private bool _day = true;

	public bool isDay { get { return _day; } }

	public Vector2 sunPos { get { return _sunPos; } }

	void Start () {
		_rotPos = new Vector2(1,_rotHeight);
		_flow = (0.1f/_speed)*45;
		_sun.transform.position = _sunPos + _rotPos;
		_moon.transform.position = _moonPos + _rotPos;
	}
	void Update () {
		if(_sunPos.x < 10 && _sunPos.x > 8.4 && _day) {
			_flow = (0.1f/_speed)*45;
			_sunPos = new Vector2(-10,0);
			_moonPos = new Vector2(-10,0);
			_day = false;
		} else {
			if(_day) {
				_flow+=0.1f/_speed*0.4f;	
				_sunPos.x += Mathf.Sin(_flow)/_speed*0.4f;
				_sunPos.y += Mathf.Cos(_flow)/_speed*0.4f;
				_sun.transform.position = _sunPos + _rotPos;
			}
		}

		if(_moonPos.x < 10 && _moonPos.x > 8.4 && !_day) {
			_flow = (0.1f/_speed)*45;
			_sunPos = new Vector2(-10,0);
			_moonPos = new Vector2(-10,0);
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
}
