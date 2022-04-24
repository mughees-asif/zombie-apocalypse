using UnityEngine;
using System.Collections;

namespace Completed
{
	public abstract class MovingObject : MonoBehaviour
	{
		public float moveTime = 0.15f;			

		public LayerMask blockingLayer;	
		
		// Collision detection
		private BoxCollider2D boxCollider; 		
		private Rigidbody2D rb2D;				

		// Streamline movement
		private float inverseMoveTime;			
		
		// Generic start function that is further implemented in the Player and Enemy class
		protected virtual void Start ()
		{
			boxCollider = GetComponent <BoxCollider2D> ();
			rb2D = GetComponent <Rigidbody2D> ();
			inverseMoveTime = 1f / moveTime;
		}
		
		// Generic move function that is further implemented in the Player and Enemy class
		protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
		{
			Vector2 start = transform.position;
			Vector2 end = start + new Vector2 (xDir, yDir);
			
			boxCollider.enabled = false;
			
			hit = Physics2D.Linecast (start, end, blockingLayer);
			boxCollider.enabled = true;
			
			if(hit.transform == null)
			{
				StartCoroutine (SmoothMovement (end));
				return true;
			}
			return false;
		}
		
		
		// Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
		protected IEnumerator SmoothMovement (Vector3 end)
		{
			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			
			while(sqrRemainingDistance > float.Epsilon)
			{
				Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
				rb2D.MovePosition (newPostion);
				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				yield return null;
			}
		}
		
		// Player attempts to move
		protected virtual void AttemptMove <T> (int xDir, int yDir)
			where T : Component
		{
			RaycastHit2D hit;
			
			bool canMove = Move (xDir, yDir, out hit);
			
			if(hit.transform == null)
				return;
			
			T hitComponent = hit.transform.GetComponent <T> ();
			
			if(!canMove && hitComponent != null)
				OnCantMove (hitComponent);
		}
		
		protected abstract void OnCantMove <T> (T component)
			where T : Component;
	}
}
