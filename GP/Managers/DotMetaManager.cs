using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

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
	class DotMetaManager : IManager
	{
		DotMetaManager()
		{

		}

		public static DotMetaManager Default => new DotMetaManager();

		protected static string pathMetaFile => "./.meta";
		public static FileInfo FileMetadados(DirectoryInfo dir){
			return dir.GetFile(pathMetaFile);
		}
		public static bool EhAmbiente(DirectoryInfo dir)
		{
			return dir.GetFile(pathMetaFile).Exists;
		}
		bool IManager.EhAmbiente(DirectoryInfo dir) => EhAmbiente(dir);
		
		public static Meta GetMeta(DirectoryInfo dir)
		{
			var meta = YAMLToMeta(dir.GetFile(pathMetaFile).ReadAllText());
			return meta;
		}
		Meta IManager.GetMeta(DirectoryInfo dir) => GetMeta(dir);

		public static string MetaToYAML(Meta meta){

			var serializer = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
			return serializer.Serialize(meta);
		}
		public static Meta MetaFromYAML(DictonaryNode dmeta){
			var meta =  new Meta();
			meta.Nome		  = dmeta[nameof(Meta.Nome)] as string;
			meta.Descricao	  = dmeta[nameof(Meta.Descricao)] as string;
			meta.Linguagem	  = dmeta[nameof(Meta.Linguagem)] as string;
			meta.ComandoAbrir = dmeta[nameof(Meta.ComandoAbrir)] as string;
			meta.Caminho	  = dmeta[nameof(Meta.Caminho)] as string;
			meta.Tipo = dmeta.GetEnum<TipoAmbiente>(nameof(Meta.Tipo));
			meta.SubProjetos = dmeta.GetArrayOfObject<Meta>(nameof(Meta.SubProjetos), (subProj)=>{
				return MetaFromYAML(new DictonaryNode(subProj));
			});	
			meta.Scripts = dmeta.GetDictonaryNode(nameof(Meta.Scripts)).Node?.ToDictionary(
				jScript => (string)jScript.Key, 
				jScript => (string)jScript.Value 
			);
			return meta;
		}
		public static Meta YAMLToMeta(string json)
		{
			if(string.IsNullOrWhiteSpace(json))
				return new Meta();
			try{
				var deserializer = new DeserializerBuilder()
					.WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
					.Build();

				return MetaFromYAML(new DictonaryNode(deserializer.Deserialize<Dictionary<object, object>>(json)));
			}
			catch (Exception e) {
				throw new MetadadosInvalidosException(null, e);
			}
		}
	}

}