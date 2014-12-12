using UnityEngine;
using System.Collections;
using System;

namespace Trello
{
	public class TrelloException : Exception {
	
		public TrelloException() : base()
		{
			
		}
		
		public TrelloException(string message) : base(message)
		{
		
		}
		
		public TrelloException(string format, params object[] args) : base(string.Format(format, args))
		{
		
		}
		
		public TrelloException(string message, Exception innerException) : base(message, innerException) 
		{
		
		}
		
		public TrelloException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) 
		{
		
		}
	}
}