using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;

namespace Trello
{
	public class Trello {
		
		private string token;
		private string key;
		private List<object> boards;
		private List<object> lists;
		private const string memberBaseUrl = "https://api.trello.com/1/members/me";
		private const string boardBaseUrl = "https://api.trello.com/1/boards/";
		private const string cardBaseUrl = "https://api.trello.com/1/cards/";
		private string currentBoardId = "";
		private string currentListId = "";
		
		public Trello(string key, string token)
		{
			this.key = key;
			this.token = token;
		}
		
		public List<object> populateBoards()
		{
			boards = null;
			WWW www = new WWW(memberBaseUrl + "?" + "key=" + key + "&token=" + token + "&boards=all");
			
			// Wait for request to return
			while (!www.isDone) {}
			
			var dict = Json.Deserialize(www.text) as Dictionary<string,object>;
			
			Debug.Log (www.text);
			Debug.Log (dict["boards"]);
			
			boards = (List<object>)dict["boards"];
			return boards;
		}
		
		public void setCurrentBoard(string name)
		{
			for (int i = 0; i < boards.Count; i++)
			{
				var board = (Dictionary<string, object>)boards[i];
				if ( (string)board["name"] == name)
				{
					currentBoardId = (string)board["id"];
					Debug.Log (currentBoardId);
					return;
				}
			}
			
			currentBoardId = "";
			throw new SystemException("No such board found.");
		}
		
		public List<object> populateLists()
		{
			lists = null;
			WWW www = new WWW(boardBaseUrl + currentBoardId + "?" + "key=" + key + "&token=" + token + "&lists=all");
			
			// Wait for request to return
			while (!www.isDone) {}
			
			var dict = Json.Deserialize(www.text) as Dictionary<string,object>;
			
			Debug.Log (www.text);
			Debug.Log (dict["lists"]);
			
			lists = (List<object>)dict["lists"];
			return lists;
		}
		
		public void setCurrentList(string name)
		{
			for (int i = 0; i < lists.Count; i++)
			{
				var list = (Dictionary<string, object>)lists[i];
				if ( (string)list["name"] == name)
				{
					currentListId = (string)list["id"];
					Debug.Log (currentListId);
					return;
				}
			}
			
			currentListId = "";
			throw new SystemException("No such list found.");
		}
		
		public TrelloCard newCard()
		{
			var card = new TrelloCard();
			card.idList = currentListId;
			return card;
		}
		
		public TrelloCard uploadCard(TrelloCard card)
		{
			WWWForm post = new WWWForm();
			post.AddField("name", card.name);
			post.AddField("desc", card.desc);
			post.AddField("due", card.due);
			post.AddField("idList", card.idList);
			post.AddField("urlSource", card.urlSource);
			
			WWW www = new WWW(cardBaseUrl + "?" + "key=" + key + "&token=" + token, post);
			
			// Wait for request to return
			while (!www.isDone) {}
			Debug.Log (www.text);
			
			return card;
		}
		
		
		
	}
}