Trello-Cards-Unity
==================

This is a simple C# system for creating Trello cards in your own boards from your Unity app or game. Some use cases may be crash reports, significant events etc.

## Usage

Go ahead and grab a Trello Application Key from: https://trello.com/1/appKey/generate. Grab the application key, I'll refer to this as the `key` from now on.

Now head to: https://trello.com/1/connect?key=[yourkeygoeshere]&name=Your%20App&response_type=token&scope=read,write&expiration=never

In your browser (make sure to fill in your key from just before). You'll be taken to https://trello.com/1/token/approve, and the token code given will be your `token`.

## Example Code

  using UnityEngine;
  using System.Collections;
  using MiniJSON;
  using System.Collections.Generic;
  using Trello;
  
  public class RunAtStart : MonoBehaviour {
  
  	// Use this for initialization
  	IEnumerator Start () {
  	
  		var trello = new Trello.Trello("168af1464a92b344c8300cefca96434d", "8be141a3a52534d6f3f258f2de31d1c2ad1f8155fa4819d1fadcdb3be5abda57");
  		
  		// Async, do not block
  		yield return trello.populateBoards();
  		trello.setCurrentBoard("Proof");
  		
  		// Async, do not block
  		yield return trello.populateLists();
  		trello.setCurrentList("Feedback");
  		
  		var card = trello.newCard();
  		card.name = "Unity Test";
  		card.desc = "Description";
  		card.due = "11/12/2014";
  		
  		yield return trello.uploadCard(card);
  		
  	}
  }


