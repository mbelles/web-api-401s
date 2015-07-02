using AuthWebApp.Authentication;
using AuthWebApp.DTO;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthWebApp.Controllers
{
	[Route("api/[controller]")]
	public class WidgetController : Controller
	{
		private WidgetDTO[] widgets = new WidgetDTO[]
		{
			new WidgetDTO(1),
			new WidgetDTO(2),
			new WidgetDTO(3)
		};

		// GET: api/values
		[HttpGet]
		public IEnumerable<WidgetDTO> Get()
		{
			return widgets;
		}

		// GET api/values/5
		[BasicAuth]
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var widget = widgets.FirstOrDefault(w => w.Id == id);
			if (widget == null)
				return HttpNotFound();
			return new ObjectResult(widget);
		}
	}
}
