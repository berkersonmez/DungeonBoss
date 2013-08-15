using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class EntityAI : MonoBehaviour {
	public Transform target;
	public float speed = 3;
	public float nextWPDistance = 2;
	public float lastWPDistance = .1f;
	public float repathRate = .5f;
	public bool canMove = true;
	public bool targetSet = false;
	
	private int favouriteAvoidDir = 1;
	
	protected Path path;
	protected Seeker seeker;
	protected int currentWP;

	void Start () {
		seeker = GetComponent<Seeker>();
		seeker.pathCallback += OnPathComplete;
		InvokeRepeating("searchPath", repathRate, repathRate);
		rigidbody.freezeRotation = true;
		InvokeRepeating("changeFavAvoidDir", .1f, 5f);
	}
	
	public void changeFavAvoidDir() {
		favouriteAvoidDir = Random.Range(-1, 1);
		if (favouriteAvoidDir == 0) favouriteAvoidDir = 1;
	}
	
	public void searchPath() {
		if (target != null) {
			seeker.StartPath(transform.position, target.position);
		}
	}
	
	public void OnPathComplete(Path p) {
		if (!p.error) {
			path = p;
			currentWP = 0;
		}
	}
	
	public void releasePath() {
		path = null;
	}
	
	void FixedUpdate () {
		if (!canMove) return;
		if (path == null) return;
		if (currentWP >= path.vectorPath.Count) {
			canMove = false;
			return;
		}
		
		Vector3 dir = (path.vectorPath[currentWP] - transform.position).normalized;
		dir.y = 0f;
		dir *= speed * Time.fixedDeltaTime * 100;
		
		RaycastHit hit;
		if (Physics.Raycast(transform.position + new Vector3(0f, .1f, 0f), dir, out hit, 1f)) {
			if (hit.collider.tag == gameObject.tag) {
				Vector3 perpDir = Vector3.Cross(new Vector3(0f, 1f, 0f), dir);
				perpDir = perpDir.normalized * dir.magnitude;
				dir = perpDir * favouriteAvoidDir;
			}
		}
			
		rigidbody.AddForce(dir);
		
		float WPEndDistance = nextWPDistance;
		if (currentWP == path.vectorPath.Count -1) {
			WPEndDistance = lastWPDistance;
		}
		if (Vector3.Distance(transform.position, path.vectorPath[currentWP]) < WPEndDistance) {
			currentWP++;
			
		}
	}
}
