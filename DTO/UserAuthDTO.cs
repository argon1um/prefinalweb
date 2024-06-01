namespace AnimalHouseRestAPI.ModelsDTO
{
    public class UserAuthDTO
    { 
        public string Phone { get; set; }
        public string Password { get; set; }

        public UserAuthDTO(string phone, string password)
        {
            Phone = phone;
            Password = password;
        }
    }
}
