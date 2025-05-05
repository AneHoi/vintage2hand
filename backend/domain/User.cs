namespace domain;

public class User 
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public float Rating { get; set; }
    public int Votes { get; set; }
}