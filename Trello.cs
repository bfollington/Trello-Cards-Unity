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
		
		/// <summary>
		/// Checks if a WWW objects has resulted in an error, and if so throws an exception to deal with it.
		/// </summary>
		/// <param name="errorMessage">Error message.</param>
		/// <param name="www">The request object.</param>
		private void checkWwwStatus(string errorMessage, WWW www)
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				throw new TrelloException(errorMessage + ": " + www.error);
			}
		}
		
		/// <summary>
		/// Download the list of available boards for the user, these are cached and allow populateLists() to function.
		/// </summary>
		/// <returns>A parsed JSON list of boards.</returns>
		public List<object> populateBoards()
		{
			boards = null;
			WWW www = new WWW(memberBaseUrl + "?" + "key=" + key + "&token=" + token + "&boards=all");
			
			// Wait for request to return
			while (!www.isDone)
			{
				checkWwwStatus("Connection to the Trello servers was not possible", www);
			}
			
			var dict = Json.Deserialize(www.text) as Dictionary<string,object>;
			
			Debug.Log (www.text);
			
			boards = (List<object>)dict["boards"];
			return boards;
		}
		
		/// <summary>
		/// Sets the current board to search for lists in.
		/// </summary>
		/// <param name="name">Name of the board we're after.</param>
		public void setCurrentBoard(string name)
		{
			if (boards == null)
			{
				throw new TrelloException("You have not yet populated the list of boards, so one cannot be selected.");
			}
			
			for (int i = 0; i < boards.Count; i++)
			{
				var board = (Dictionary<string, object>)boards[i];
				if ( (string)board["name"] == name)
				{
					currentBoardId = (string)board["id"];
					return;
				}
			}
			
			currentBoardId = "";
			throw new TrelloException("No such board found.");
		}
		
		/// <summary>
		/// Populate the lists for the current board, these are cached for easy card uploading later.
		/// </summary>
		/// <returns>A parsed JSON list of lists.</returns>
		public List<object> populateLists()
		{
			lists = null;
			
			if (currentBoardId == "")
			{
				throw new TrelloException("Cannot retreive the lists, you have not selected a board yet.");
			}
			
			WWW www = new WWW(boardBaseUrl + currentBoardId + "?" + "key=" + key + "&token=" + token + "&lists=all");

			
			// Wait for request to return
			while (!www.isDone)
			{
				checkWwwStatus("Connection to the Trello servers was not possible", www);
			}
			
			var dict = Json.Deserialize(www.text) as Dictionary<string,object>;
			
			Debug.Log (www.text);
			
			lists = (List<object>)dict["lists"];
			return lists;
		}
		
		/// <summary>
		/// Updates the current list to upload cards to.
		/// </summary>
		/// <param name="name">Name of the list.</param>
		public void setCurrentList(string name)
		{
			if (lists == null)
			{
				throw new TrelloException("You have not yet populated the list of lists, so one cannot be selected.");
			}
			
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
			throw new TrelloException("No such list found.");
		}
		
		/// <summary>
		/// Retrieve a new Trello card objects, with the correct list id populated already.
		/// </summary>
		/// <returns>The card object.</returns>
		public TrelloCard newCard()
		{
			if (currentListId == "")
			{
				throw new TrelloException("Cannot create a card when you have not set selected a list.");
			}
		
			var card = new TrelloCard();
			card.idList = currentListId;
			return card;
		}
		
		/// <summary>
		/// Given an exception object, a TrelloCard is created and populated with the relevant information from the exception. This is then uploaded to the Trello server.
		/// </summary>
		/// <returns>The exception card.</returns>
		/// <param name="e">E.</param>
		public TrelloCard uploadExceptionCard(Exception e)
		{
			TrelloCard card = new TrelloCard();
			card.name = e.GetType().ToString();
			card.due = DateTime.Now.ToString();
			card.desc = e.Message;
			card.idList = currentListId;
			
			return uploadCard(card);
		}
		
		/// <summary>
		/// Uploads a given TrelloCard object to the Trello servers.
		/// </summary>
		/// <returns>Your card.</returns>
		/// <param name="card">Your card.</param>
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
			while (!www.isDone)
			{
				checkWwwStatus("Could not upload Trello card", www);
			}
			
			return card;
		}
		
		
		
	}
}