using System;
using System.Linq;
using System.Collections.Generic;

namespace GP
{
	class DictonaryNode  {
		public DictonaryNode(Dictionary<object, object> dictionary)
		{
			Node = dictionary;
		}

		public Dictionary<object, object> Node {get;set;}

		public object this[string key]{
			get {
				return Node.FirstOrDefault( (pair) => (pair.Key as string)?.ToLower() == key.ToLower()).Value;		
			}
		}
		public T GetEnum<T>(string key)
			where T : struct, System.Enum
		{	
			T value = default;
			if(this[key] is string strValue)
				Enum.TryParse<T>(strValue,out value);
			return value;
		}
		public T[] GetArray<T>(string key){
			var ListObject = this[key] as List<object>;
			return ListObject?.OfType<T>().ToArray();
		}
		public T[] GetArrayOfObject<T>(string key, Func<Dictionary<object, object>,T> Parser){
			var ListObject = this[key] as IEnumerable<object>;
			return ListObject?.OfType<Dictionary<object, object>>().Select(Parser).ToArray();
		}
		public DictonaryNode GetDictonaryNode(string key){
			var dictionary = this[key] as Dictionary<object, object>;
			return new DictonaryNode(dictionary);
		} 
	}

}