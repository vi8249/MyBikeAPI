using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using YouBikeAPI.Data;

namespace YouBikeAPI.Controllers
{
	[Route("api/[controller]")]
	public class TestsController : Controller
	{
		[HttpGet]
		public IActionResult Test()
		{
			string calculatedDistanceData = "{\n                \"destination_addresses\" : [\n                      \"No. 130-7, Section 2, Zhonghua Road, North District, Taichung City, Taiwan 404\"\n                   ],\n                   \"origin_addresses\" : [\n                      \"No. 104, Jiaobanian E Rd, Beitun District, Taichung City, Taiwan 406\"\n                   ],\n                   \"rows\" : [\n                      {\n                                \"elements\" : [\n                                   {\n                                    \"distance\" : {\n                                        \"text\" : \"2.4 km\",\n                                  \"value\" : 2433\n                                    },\n                               \"duration\" : {\n                                        \"text\" : \"9 mins\",\n                                  \"value\" : 517\n                               },\n                               \"status\" : \"OK\"\n                                   }\n                         ]\n                      }\n                   ],\n                   \"status\" : \"OK\"\n                }";
			long distance = 0L;
			long duration = 0L;
			DistanceMatrix distanceMatrix = JsonSerializer.Deserialize<DistanceMatrix>(calculatedDistanceData);
			if (distanceMatrix.status == "OK" && distanceMatrix.rows[0].elements[0].status == "OK")
			{
				distance = distanceMatrix.rows[0].elements[0].distance.value;
				duration = distanceMatrix.rows[0].elements[0].duration.value;
			}
			TimeSpan t = TimeSpan.FromSeconds(duration);
			return Ok($"距離：{Math.Round((double)distance / 1000.0, 2)} 耗時：{t.Hours}:{t.Minutes}");
		}
	}
}
