using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthWebApp.DTO
{
	public class WidgetDTO
	{
		public WidgetDTO()
		{
		}

		public WidgetDTO(int id)
		{
			this.Id = id;
			this.Name = $"Widget: {id}";
		}

		public int Id { get; set; }

		public string Name { get; set; } = "Widget";

		[JsonIgnore]
		public string TransientField { get; set; }
	}
}
