using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace GP
{
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