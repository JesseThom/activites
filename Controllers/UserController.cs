using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using activites.Models;

namespace activites.Controllers;

public class UserController : Controller
{
    static int? UserId;
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public UserController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [SessionCheck]
    [HttpGet("/dashboard")]
    public IActionResult Dashboard()
    {
        User? OneUser = _context.Users.SingleOrDefault(i => i.UserId == UserId);
        
        // List<Event> AllEvents =_context.Events
        //     .Include(e => e.Creator)
        //     .Include(e => e.EventParticipants)
        //     .ThenInclude(u => u.User)
        //     .OrderBy(o => o.Date)
        //     .ToList();

        MyViewModel AllEvents = new MyViewModel()
        {
            AllEvents = _context.Events
            .Include(e => e.Creator)
            .Include(e => e.EventParticipants)
            .ThenInclude(u => u.User)
            .OrderBy(o => o.Date)
            .ToList(),

            // AllJoined = _context.Participants
            // .Include(e =>e.UserId)
            // .Include(e=>e.EventId)
            // .ThenInclude(e=>e.User)
            // .Select(e => e.UserId == (int)HttpContext.Session.GetInt32("uuid"))
            // .ToList()
        };

        return View(AllEvents);
    }

    [HttpPost("/user/login")]
    public IActionResult UserLogin(Login newLogin)
    {
        if(!ModelState.IsValid)
        {
            return View("Index");
        }
        User? OneUser = _context.Users.SingleOrDefault(i => i.Email == newLogin.EmailLogin);
        if(OneUser == null )
        {
            ModelState.AddModelError("EmailLogin","Invalid Email/Password");
            return View("Index");
        }

        UserId = OneUser.UserId;
        PasswordHasher<Login> Hasher = new PasswordHasher<Login>();
        var result = Hasher.VerifyHashedPassword(newLogin, OneUser.Password, newLogin.PasswordLogin);
        if (result == 0)
        {
            ModelState.AddModelError("EmailLogin","Invalid Email/Password");
            return View("Index");
        }
        HttpContext.Session.SetInt32("uuid",OneUser.UserId);
        HttpContext.Session.SetString("name",OneUser.FirstName);
        System.Console.WriteLine(HttpContext.Session.GetInt32("uuid"));
        return RedirectToAction("Dashboard");
    }

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("uuid");
        HttpContext.Session.Remove("name");
        return View("Index");
    }

    
    [HttpPost("/user/create")]
    public IActionResult UserCreate(User newUser)
    {
        if(ModelState.IsValid)
        {
            User? OneUser = _context.Users.SingleOrDefault(i => i.Email == newUser.Email);
            if(OneUser != null)
            {
                ModelState.AddModelError("Email","Email already exhist");
                return View("Index");
            }
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();

            UserId = newUser.UserId;
            HttpContext.Session.SetInt32("uuid",newUser.UserId);
            HttpContext.Session.SetString("name",newUser.FirstName);

            return RedirectToAction("Dashboard");
        }

        return View("Index");

    }

    [HttpGet("/user/{id}")]
    public IActionResult UserView(int id)
    {
        User? OneUser = _context.Users.SingleOrDefault(i => i.UserId == id);

        if(OneUser == null){
            return View("Index");
        }

        return View(OneUser);
    }

    [HttpGet("/user/{id}/edit")]
    public IActionResult UserEdit(int id)
    {
        User? OneUser = _context.Users.SingleOrDefault(i => i.UserId == id);

        if(OneUser == null){
            return View("Index");
        }

        return View(OneUser);
    }

    [HttpPost("/user/{id}/update")]
    public IActionResult UserUpdate(User newUser, int id)
    {
        User? OldUser = _context.Users.SingleOrDefault(i => i.UserId == id);

        if(ModelState.IsValid && OldUser != null)
        {
          // OldUser.FirstName = newUser.FirstName;
          // add more attributes here if needed
            OldUser.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

        return RedirectToAction("UserView",new {id = id});

        }

            return View("UserEdit",OldUser);
    }

    [HttpPost("/user/{id}/delete")]
    public IActionResult UserDelete(int id)
    {
        User? UserToDelete = _context.Users.SingleOrDefault(i => i.UserId ==id);
        if(UserToDelete != null)
        {
        _context.Users.Remove(UserToDelete);
        _context.SaveChanges();

        return RedirectToAction("Index");
        }

        return View("Index");
    }
}