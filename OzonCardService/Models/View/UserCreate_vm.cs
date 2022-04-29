namespace OzonCardService.Models.View
{
    public class UserCreate_vm
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int[] Rules { get; set; }
    }
}
