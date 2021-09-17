Trello Cards For Unity
==================

[![Twitter](https://img.shields.io/twitter/follow/vivavolt?label=%40vivavolt&style=flat&colorA=000000&colorB=000000&logo=twitter&logoColor=000000)](https://twitter.com/vivavolt)
[![Donate (ETH)](https://img.shields.io/badge/Donate-(ETH)-f5f5f5?style=flat&colorA=000000&colorB=000000)](https://blockchain.com/eth/address/0x981e493b795A7a28c43Bf8d7a8E125C419435Fa7)
[![Donate ($)](https://img.shields.io/badge/Donate-($)-f5f5f5?style=flat&colorA=000000&colorB=000000)](https://ko-fi.com/vivavolt)
![Language](https://img.shields.io/github/languages/top/bfollington/Trello-Cards-Unity?style=flat&colorA=000000&colorB=000000)
![License](https://img.shields.io/github/license/bfollington/Trello-Cards-Unity?style=flat&colorA=000000&colorB=000000)

This is a simple C# system for creating Trello cards in your own boards from your Unity app or game. Some use cases may be crash reports, significant events etc.

## Installation

Just drag these `.cs` files into your scripts folder, wherever you like. If you already have `MiniJson` in your project, you may get a namespace conflict. No matter, just delete the one from this repo.

## Usage

Go ahead and grab a Trello Application Key from: https://trello.com/1/appKey/generate. Grab the application key, I'll refer to this as the `key` from now on.

Now head to: https://trello.com/1/connect?key=[yourkeygoeshere]&name=Your%20App&response_type=token&scope=read,write&expiration=never

In your browser (make sure to fill in your key from just before). You'll be taken to https://trello.com/1/token/approve, and the token code given will be your `token`.

## Example Code

```cs
using UnityEngine;
using System.Collections;
using MiniJSON;
using System.Collections.Generic;
using Trello;

public class RunAtStart : MonoBehaviour {

    // Use this for initialization
    IEnumerator Start () {

        var trello = new Trello.Trello(YOUR-KEY, YOUR-TOKEN);

        // Async, do not block
        yield return trello.populateBoards();
        trello.setCurrentBoard("Your Game");

        // Async, do not block
        yield return trello.populateLists();
        trello.setCurrentList("Achievements");

        var card = trello.newCard();
        card.name = "Unity Test";
        card.desc = "Description";
        card.due = "11/12/2014";

        yield return trello.uploadCard(card);

        // You can use the helper method to upload exceptions with relevant data
        try
        {
            throw new UnityException("Testing");
        } catch (UnityException e)
        {
            trello.uploadExceptionCard(e);
        }

    }
}
```

## Errors

If you see a `401 Unauthorized` message returnd, it likely means your `key` or `token` are invalid or have expired. Try generate new ones and trying again, if you're having trouble get in touch with me.
