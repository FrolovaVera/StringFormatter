namespace StringFormatter
{
    public class User
    {
        public string FirstName { get; }
        public string LastName { get; }

        private int i;

        public User(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            i = 1;
        }

        public string GetGreeting()
        {
            return StringFormatter.Shared.Format(
                "Привет, {FirstName} {LastName}!", this);
        }
    }
}