namespace DTOs
{
    public record UserDTO(
     int Id ,

     string Email ,

     string FirstName ,

     string LastName ,

     bool IsAdmin,

     ICollection<OrderDTO> Orders 
    );
    

    
}
