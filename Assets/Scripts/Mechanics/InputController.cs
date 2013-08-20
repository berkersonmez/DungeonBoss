using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	public static InputController instance;
	
    public float dragSpeed = 2;
	public float zoomSpan = 3f;
	public float zoomSpeed = 2f;
	
	public float autoMoveSmooth = .1f;
	private bool autoMove = false;
	private Vector3 autoMoveEnd;
    private Vector3 dragOrigin;
	private float zoomInitial;
	
	public bool showHealthbars = false;
	public bool showDamages = true;
	public bool inputLock = false;
	
	void Awake() {
		instance = this;
		zoomInitial = transform.position.y;
	}
	
    void Update() {
		
		if (Input.GetButtonDown("Function1")) {
			NGUITools.SetActive(GameController.instance.infoWindow, true);
		} else if (Input.GetButtonDown("Function2")) {
			HUDText [] huds = (HUDText [])FindObjectsOfType(typeof(HUDText));
			if (showHealthbars) {
				foreach(HUDText hud in huds) {
					UISprite foreground = hud.transform.Find("Healthbar").Find("Foreground").GetComponent<UISprite>();
					UISprite background = hud.transform.Find("Healthbar").Find("Background").GetComponent<UISprite>();
					foreground.enabled = false;
					background.enabled = false;
				}
				showHealthbars = false;
			} else {
				foreach(HUDText hud in huds) {
					UISprite foreground = hud.transform.Find("Healthbar").Find("Foreground").GetComponent<UISprite>();
					UISprite background = hud.transform.Find("Healthbar").Find("Background").GetComponent<UISprite>();
					foreground.enabled = true;
					background.enabled = true;
				}
				showHealthbars = true;
			}
		} else if (Input.GetButtonDown("Function3")) {
			HUDText [] huds = (HUDText [])FindObjectsOfType(typeof(HUDText));
			if (showDamages) {
				foreach(HUDText hud in huds) {
					hud.enabled = false;
				}
				showDamages = false;
			} else {
				foreach(HUDText hud in huds) {
					hud.enabled = true;
				}
				showDamages = true;
			}
		} else if (Input.GetButtonDown("Inv1")) {
			Bottombar.instance.selectMob(0);
		} else if (Input.GetButtonDown("Inv2")) {
			Bottombar.instance.selectMob(1);
		} else if (Input.GetButtonDown("Inv3")) {
			Bottombar.instance.selectMob(2);
		} else if (Input.GetButtonDown("Inv4")) {
			Bottombar.instance.selectMob(3);
		} else if (Input.GetButtonDown("Inv5")) {
			Bottombar.instance.selectMob(4);
		}
		
		// BOSSFIGHT mode
		if (GameController.instance.gameState == (int) GameController.GameState.BOSSFIGHT) {
			transform.position = new Vector3(GameController.instance.boss.transform.position.x,
				transform.position.y, GameController.instance.boss.transform.position.z);
			GameController.instance.bossC.move(new Vector3(0f, 0f, 0f));
			if (Input.GetButtonDown("Fire3")) {
				autoMove = false;
	            dragOrigin = Input.mousePosition;
	            return;
	        }
			if (!Input.GetButton("Fire3")) return;
			
			Vector3 posBoss = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        	Vector3 moveBoss = new Vector3(posBoss.x, 0, posBoss.y);
			GameController.instance.bossC.move(moveBoss);
			return;
		}
		
		if (autoMove) {
			transform.position = Vector3.Lerp(transform.position, autoMoveEnd, autoMoveSmooth * Time.deltaTime);
			if (transform.position == autoMoveEnd) {
				autoMove = false;
			}
		}
		if (inputLock) return;
		
		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			Vector3 moveVertical = new Vector3(0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 0);
			if (moveVertical.y > 0 && transform.position.y > zoomInitial+zoomSpan) return;
			if (moveVertical.y < 0 && transform.position.y < zoomInitial-zoomSpan) return;
			transform.Translate(moveVertical, Space.World);
		}
		
		if (GUIUtility.hotControl == 0) {
			if (Input.GetButtonDown("Fire1")) {
				UITooltip.ShowText("");
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 1000f)) {
					if (hit.collider.tag == "Ground") {
						Bottombar.instance.spawnMobReq(hit.point);
						GetComponent<UICamera>().ShowTooltip(false);
					}
	    		}
			}
			
			if (Input.GetButtonDown("Fire2")) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 1000f)) {
		            
	    		}
			}
		}
		
        if (Input.GetButtonDown("Fire3")) {
			autoMove = false;
            dragOrigin = Input.mousePosition;
            return;
        }
		
        if (!Input.GetButton("Fire3")) return;
		
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
        transform.Translate(move, Space.World);
    }
	
	public void moveTo(Vector3 targetTilePosition) {
		autoMoveEnd = new Vector3(targetTilePosition.x, transform.position.y, targetTilePosition.z);
		autoMove = true;
	}
}
