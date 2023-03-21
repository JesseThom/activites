using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using activites.Models;

namespace activites.Controllers;

public class ParticipantController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public ParticipantController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    //     [HttpGet("/participant/new")]
    // public IActionResult ParticipantNew()
    // {
    //     return View();
    // }

        [HttpPost("/participant/create")]
    public IActionResult ParticipantCreate(Participant newParticipant)
    {
        if(ModelState.IsValid)
        {
            newParticipant.UserId = (int)HttpContext.Session.GetInt32("uuid");
            _context.Add(newParticipant);
            _context.SaveChanges();
            return RedirectToAction("Dashboard", "User");
        }
        System.Console.WriteLine("Notcreated");
        List<Event> AllEvents =_context.Events
            .Include(e => e.Creator)
            .Include(e => e.EventParticipants)
            .ThenInclude(u => u.User)
            .ToList();
        return View("Dashboard", AllEvents);
    }

    //     [HttpGet("/participant/{id}")]
    // public IActionResult ParticipantView(int id)
    // {
    //     Participant? OneParticipant = _context.Participants.SingleOrDefault(i => i.ParticipantId == id);

    //     if(OneParticipant == null){
    //         return View("Index");
    //     }

    //     return View(OneParticipant);
    // }

    //     [HttpGet("/participant/{id}/edit")]
    // public IActionResult ParticipantEdit(int id)
    // {
    //     Participant? OneParticipant = _context.Participants.SingleOrDefault(i => i.ParticipantId == id);

    //     if(OneParticipant == null){
    //         return View("Index");
    //     }

    //     return View(OneParticipant);
    // }

    //     [HttpPost("/participant/{id}/update")]
    // public IActionResult ParticipantUpdate(Participant newParticipant, int id)
    // {
    //     Participant? OldParticipant = _context.Participants.SingleOrDefault(i => i.ParticipantId == id);

    //     if(ModelState.IsValid && OldParticipant != null)
    //     {
    //       // OldParticipant.FirstName = newParticipant.FirstName;
    //       // add more attributes here if needed
    //         OldParticipant.UpdatedAt = DateTime.Now;

    //         _context.SaveChanges();

    //     return RedirectToAction("ParticipantView",new {id = id});

    //     }

    //         return View("ParticipantEdit",OldParticipant);
    // }
        [SessionCheck]
        [HttpPost("/participant/delete")]
    public IActionResult ParticipantDelete(Participant oldParticipant)
    {
        Participant? ParticipantToDelete = _context.Participants.SingleOrDefault(i => i.UserId ==(int)HttpContext.Session.GetInt32("uuid")&& i.EventId ==oldParticipant.EventId);
        if(ParticipantToDelete != null)
        {
        _context.Participants.Remove(ParticipantToDelete);
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