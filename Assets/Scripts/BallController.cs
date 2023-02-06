using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Grados de giro por segundo")]
	float rotationDegrees;

	[SerializeField]
	float kickForce;

	[SerializeField]
	float velocityThreshold;

	Transform direction;
    void Start()
    {
		// No es la forma!!
        direction = transform.GetChild(0);
		direction.gameObject.SetActive(false);
    }

    void Update()
    {
        //direction.localRotation *= Quaternion.Euler(0, 0, -rotationDegrees * Time.deltaTime);

		ManageInput();
    }

	void Kick()
	{

		Rigidbody2D rb = GetComponent<Rigidbody2D>();

		bool correctVelocity = rb.velocity.magnitude < velocityThreshold;

		List<ContactPoint2D> contacts = new List<ContactPoint2D>(1);
		int contactCount = rb.GetContacts(contacts);
	
		bool inGround = contacts.Count > 0;

		if (correctVelocity && inGround)
		{
			rb.AddForce(direction.up * kickForce, ForceMode2D.Impulse);
		}
	}

	Vector2 touchBeganPosition;
	void ManageInput()
	{
		// bool space = Input.GetKey(KeyCode.Space);

		// if (space)
		// {
		// 	Kick();
		// }

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				touchBeganPosition = touch.position;
				direction.gameObject.SetActive(true);
			}

			if (touch.phase == TouchPhase.Moved)
			{
				Vector3 dir = (touchBeganPosition - touch.position).normalized;
				float angle = Vector2.SignedAngle(Vector2.one, dir);
				direction.rotation = Quaternion.Euler(0, 0, angle);
			}

			if (touch.phase == TouchPhase.Ended)
			{
				direction.gameObject.SetActive(false);
				Kick();
			}

		}

	}
}
