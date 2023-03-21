using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using activites.Models;

namespace activites.Controllers;

public class EventController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public EventController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
        [SessionCheck]
        [HttpGet("/event/new")]
    public IActionResult EventNew()
    {
        return View();
    }

        [HttpPost("/event/create")]
    public IActionResult EventCreate(Event newEvent)
    {
        if(ModelState.IsValid)
        {
            newEvent.UserId = (int)HttpContext.Session.GetInt32("uuid");
            _context.Add(newEvent);
            _context.SaveChanges();
            return RedirectToAction("EventView",new {id = newEvent.EventId});
        }

        return View("EventNew");
    }

        [SessionCheck]
        [HttpGet("/event/{id}")]
    public IActionResult EventView(int id)
    {
        MyViewModel MyModel = new MyViewModel()
        {
            CurrentEvent =_context.Events
            .Include(e => e.Creator)
            .Include(e => e.EventParticipants)
            .ThenInclude(p => p.User)
            .SingleOrDefault(i => i.EventId == id),

            // FirstUser = _context.Participants
            // .Include(e =>e.User)
            // .Where(e => e.EventId == id)
            // .FirstOrDefault(e => e.UserId == (int)HttpContext.Session.GetInt32("uuid"))

        };
        Event? OneEvent = _context.Events
        .Include(e => e.Creator)
        .Include(e => e.EventParticipants)
        .ThenInclude(p => p.User)
        .SingleOrDefault(i => i.EventId == id);

        if(OneEvent == null){
            return View("Dashboard");
        }

        return View(MyModel);
    }

    //     [HttpGet("/event/{id}/edit")]
    // public IActionResult EventEdit(int id)
    // {
    //     Event? OneEvent = _context.Events.SingleOrDefault(i => i.EventId == id);

    //     if(OneEvent == null){
    //         return View("Index");
    //     }

    //     return View(OneEvent);
    // }

    //     [HttpPost("/event/{id}/update")]
    // public IActionResult EventUpdate(Event newEvent, int id)
    // {
    //     Event? OldEvent = _context.Events.SingleOrDefault(i => i.EventId == id);

    //     if(ModelState.IsValid && OldEvent != null)
    //     {
    //       // OldEvent.FirstName = newEvent.FirstName;
    //       // add more attributes here if needed
    //         OldEvent.UpdatedAt = DateTime.Now;

    //         _context.SaveChanges();

    //     return RedirectToAction("EventView",new {id = id});

    //     }

    //         return View("EventEdit",OldEvent);
    // }

        [HttpPost("/event/{id}/delete")]
    public IActionResult EventDelete(int id)
    {
        Event? EventToDelete = _context.Events.SingleOrDefault(i => i.EventId ==id);
        if(EventToDelete != null)
        {
        _context.Events.Remove(EventToDelete);
        _context.SaveChanges();

        return RedirectToAction("Dashboard","User");
        }
        System.Console.WriteLine("Notdeleted");
        List<Event> AllEvents =_context.Events
            .Include(e => e.Creator)
            .Include(e => e.EventParticipants)
            .ThenInclude(u => u.User)
            .ToList();
        return View("Dashboard", AllEvents);
    }
}