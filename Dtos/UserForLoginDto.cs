namespace DotnetAPI.Dtos{ // start of name space



public class UserForLoginDto
{ // start of UserForRegistrationDto class
public string Email {get; set;}

public string Password {get; set;}


 public UserForLoginDto()
{

if(Email==null){

Email= "";
}

if(Password == null){

 Password="";
}

}
}// End  UserForRegistrationDto


}// end of name space
