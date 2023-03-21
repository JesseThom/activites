#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace activites.Models;
public class MyViewModel

{
    public Event CurrentEvent { get; set;}
    public Participant Participant {get;set;}
    public List<Participant> AllJoined {get;set;}
    public List<Event> AllEvents {get;set;}
}