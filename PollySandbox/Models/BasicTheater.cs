using System.Diagnostics.CodeAnalysis;
namespace PollySandbox.Models;

public class BasicTheater
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string City { get; set; }
    public string StateAbbreviation { get; set; }
    public string ZipCode { get; set; }
    public string TicketsPhone { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public float Distance { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsConcessionsEnabled { get; set; }
    public string TheaterAmenityExternalUrl { get; set; }
    public string Status { get; set; }
        
    public bool IsFEC { get; set; }

    public BasicTheater()
    {
    }
}