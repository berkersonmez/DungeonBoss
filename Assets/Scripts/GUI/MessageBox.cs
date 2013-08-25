using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour {
	struct MessageHolder {
		public int type;
		public string[] messages;
		public string sprite;
		public string header;
		
		public MessageHolder(int type, string[] messages, string sprite, string header) {
			this.type = type;
			this.messages = messages;
			this.sprite = sprite;
			this.header = header;
		}
	}
	
	public static MessageBox instance;
	public enum MsgType{TIMED = 0, KEY_TRIGGER};
	
	public GameObject messageBoxWindow;
	private UILabel msgHeader;
	private UISprite msgAvatar;
	private UILabel msgText;
	
	private Queue<MessageHolder> msgQueue;
	private string[] msgArray;
	private int letterIndex;
	private int msgIndex;
	private int msgType;
	private bool msgShowing;
	
	void Start () {
		instance = this;
		msgQueue = new Queue<MessageHolder>();
		msgAvatar = messageBoxWindow.transform.Find("Avatar").GetComponent<UISprite>();
		msgHeader = messageBoxWindow.transform.Find("Header").GetComponent<UILabel>();
		msgText = messageBoxWindow.transform.Find("Message").GetComponent<UILabel>();
		NGUITools.SetActive(messageBoxWindow, false);
	}
	
	public void showWarning(string warningText) {
		showMessage(new string[1] {warningText}, (int)MsgType.TIMED, "ui_exclamation", "Warning!");
	}
	
	public void showMessage(string[] messages, int messageType, string sprite, string header) {
		if (msgShowing) {
			msgQueue.Enqueue(new MessageHolder(messageType, messages, sprite, header));
			return;
		}
		msgArray = messages;
		msgType = messageType;
		msgIndex = 0;
		msgHeader.text = header;
		msgAvatar.spriteName = sprite;
		msgShowing = true;
		NGUITools.SetActive(messageBoxWindow, true);
		showNextMessage();
	}
	
	public void showNextMessage() {
		if (IsInvoking("messageTypeEffect")) {
			CancelInvoke("messageTypeEffect");
		}
		if (IsInvoking("showNextMessage")) {
			CancelInvoke("showNextMessage");
		}
		if (IsInvoking("messageFinished")) {
			CancelInvoke("messageFinished");
		}
		letterIndex = 0;
		msgText.text = "";
		messageTypeEffect();
	}
	
	public void skipToNextMessage() {
		if (!msgShowing) return;
		if (IsInvoking("messageTypeEffect")) {
			msgIndex++;
		}
		if (msgIndex >= msgArray.Length) {
			messageFinished();
		} else {
			showNextMessage();
		}
	}
	
	public void messageTypeEffect() {
		msgText.text += msgArray[msgIndex][letterIndex];
		letterIndex++;
		if (letterIndex >= msgArray[msgIndex].Length) {
			typeEffectFinished();
		} else {
			Invoke("messageTypeEffect", .02f);
		}
	}
	
	void typeEffectFinished() {
		msgIndex++;
		if (msgIndex >= msgArray.Length) {
			if (msgType == (int)MsgType.TIMED) {
				Invoke("messageFinished", ((float) msgArray[msgIndex-1].Length) * .05f + 2f);
			}
		} else {
			msgText.text += " ...";
			if (msgType == (int)MsgType.TIMED) {
				Invoke("showNextMessage", ((float) msgArray[msgIndex-1].Length) * .05f + 2f);
			}
		}
	}
	
	public void messageFinished() {
		if (IsInvoking("messageTypeEffect")) {
			CancelInvoke("messageTypeEffect");
		}
		if (IsInvoking("showNextMessage")) {
			CancelInvoke("showNextMessage");
		}
		if (IsInvoking("messageFinished")) {
			CancelInvoke("messageFinished");
		}
		if (msgQueue.Count != 0) {
			msgShowing = false;
			MessageHolder msg = msgQueue.Dequeue();
			showMessage(msg.messages, msg.type, msg.sprite, msg.header);
			return;
		}
		msgShowing = false;
		NGUITools.SetActive(messageBoxWindow, false);
	}
}
