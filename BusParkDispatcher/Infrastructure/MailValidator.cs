namespace BusParkDispatcher.Infrastructure
{
    public static class MailValidator
    {
        public static bool IsValidEmail(string email)
        {
            try { return new System.Net.Mail.MailAddress(email).Address == email; }
            catch { return false; }
        }
    }
}
