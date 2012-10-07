using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour
{
    // Used to record when and where the player has shot his/her gun.
    // This is used to send warnings to the Wolf AI.
    public struct GunShot
    {
        public Vector2 Location;
        public float TimeStamp;
    }

	public float speed = 1.0f;
    public AudioClip ShotgunSound = null;
	public Texture2D bulletTexture;
	
	Agent agent;
	FaceSteer _face = new FaceSteer();
	FrictionSteer _frictionSteer = new FrictionSteer();
	ArriveSteer _arriveSteer = new ArriveSteer();
    List<GunShot> _gunShots = new List<GunShot>();
	tk2dAnimatedSprite _mflash;
	int bulletCount = 10;

    public System.Collections.ObjectModel.ReadOnlyCollection<GunShot> GunShots
    {
        get
        {
            return _gunShots.AsReadOnly();
        }
    }
	
	void Start()
	{
		agent = gameObject.AddComponent<Agent>();
		DebugUtil.Assert(agent != null);
        _frictionSteer.VelocityFrictionPercentage = 1.0f;
		
		agent.AddBehaviour("face", _face, 0);
		//agent.AddBehaviour("friction", frictionSteer, 0);
		agent.AddBehaviour("arrive", _arriveSteer, 0);
		agent.MaxVelocity = speed;
		
		_face.SlowRadius = 0.5f;
		agent.MaxAngularVelocity = 2 * Mathf.PI;

		_frictionSteer.VelocityFrictionPercentage = 1.0f;
		_frictionSteer.AngularVelocityFrictionPercentage = 0.1f;

		_arriveSteer.Target = new KinematicInfo();
		_arriveSteer.Target.Position = new Vector2(0.0f, 0.0f);// agent.KinematicInfo.Position;
		_arriveSteer.SlowRadius = 1.5f;
		_arriveSteer.MaxAcceleration = 8.0f;
		_arriveSteer.MaxVelocity = 4.0f;
		
		_mflash = GetComponentInChildren<tk2dAnimatedSprite>();
  
		//controller = gameObject.GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update()
	{
		var mousePosition = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		var vectorised = new Vector2(mousePosition.x, mousePosition.z);
		_face.LocalTarget.Position = vectorised;
		Vector2 mousePosDif = vectorised - agent.KinematicInfo.Position;

		//transform.LookAt(mousePosition);
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.01f &&
            Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01f)
        {
            agent.RemoveBehaviour("arrive");
            agent.AddBehaviour("friction", _frictionSteer, 0);
        }
        else
        {
            agent.AddBehaviour("arrive", _arriveSteer, 0);
            agent.RemoveBehaviour("friction");
        }

		float vert = Input.GetAxis("Vertical");
		if (mousePosDif.magnitude < 1.0f) {
			vert = Mathf.Min(0.0f, vert);
		}
		var movementDirection = transform.TransformDirection
            (new Vector3(Input.GetAxis("Horizontal"), 0.0f, vert));
		var movement2D = new Vector2(movementDirection.x, movementDirection.z);

		_arriveSteer.Target.Position = agent.KinematicInfo.Position + movement2D;
        

		Vector3 posone = new Vector3(agent.KinematicInfo.Position.x, transform.position.y, agent.KinematicInfo.Position.y);
		Debug.DrawLine(posone, posone + movementDirection, Color.green);

        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) && _mflash.isPlaying() == false)
        {
			if(bulletCount > 0){
	            AddGunshot();
				--bulletCount;
	            Vector3 outward = agent.transform.position + 15.0f * agent.transform.forward;
	
	            //var hits = Physics.RaycastAll(agent.transform.position, outward);
	            audio.PlayOneShot(ShotgunSound);
				//_mflash.Play();
				_mflash.Play (_mflash.clipId);
	
	            var agents = agent.GetAgentsInArea(10.0f).Where(a => a.GetType() == typeof(Werewolf));
	            // Take the direction of the player, and find the angle.
	
	            foreach (var a in agents)
	            {
	               Vector2 forward = MotionUtils.GetOrientationAsVector(agent.KinematicInfo.Orientation);
	               Vector2 otherDir = a.KinematicInfo.Position - agent.KinematicInfo.Position;
	               otherDir.Normalize();
	               float angle = Vector2.Angle(forward, otherDir);
	
	               if (Mathf.Abs(angle) < 20)
	               {
	                   a.Health -= 45;
	                  a.StateMachine.PostMessage("TakeHit");
	               }
	            }
			}
        }
		
		var campers = GameObject.FindGameObjectsWithTag("Camper");
		if(campers == null || campers.Length == 0)
		{
			Application.LoadLevel("GameOver");
		}

        CleanupGunshots();
	}
	
	void OnGUI()
	{
		var height = Screen.height - bulletTexture.height;
		for(int i = 1; i <= bulletCount; ++i){
			GUI.DrawTexture(new Rect(Screen.width - i * bulletTexture.width, height, 
				bulletTexture.width	, bulletTexture.height), bulletTexture);
		}
	}
	
	public void TopUpAmmo()
	{
		bulletCount = 10;	
	}

    // Append a gunshot to the gunshot list.
    private void AddGunshot()
    {
        GunShot gunShot;
        gunShot.Location = agent.transform.position.To2D();
        gunShot.TimeStamp = Time.time;
        _gunShots.Add(gunShot);
    }

    private void CleanupGunshots()
    {
        // Remove add gunshots which happened more than four seconds ago.
        _gunShots.RemoveAll(gunShot => (Time.time - gunShot.TimeStamp) > 10.0f);
    }
}
