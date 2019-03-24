using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObject.Find(id);
            if (celestialObject == null)
                return NotFound();

            celestialObject.Satellites = _context.CelestialObject.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObject.Where(e => e.Name == name).ToList();

            if (!celestialObjects.Any()) return NotFound();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObject.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObject.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObject.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }
    }
}
