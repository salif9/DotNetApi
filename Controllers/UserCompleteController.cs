using System.Data;
using Dapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

//[Authorize]
[ApiController]  //this is a controller something we want to mapp to our endpoints 
[Route("[controller]")] /* tell as where we want to look at our controller the place we want to look to find our controller 
                           it will look at the name of the controller in this case ["controller"]*/

public class UserCompleteController: ControllerBase // the we build the controller class that is inheriting from ControllerBase class 
{
    
  private readonly DataContextDapper _dapper; //  creat an instance of dapper is a parameter in the controller class 
  private readonly ReusableSql _reusableSql;
    public UserCompleteController(IConfiguration config)  // constructor of usercontroller take as parameter dapper connection string 
    {
        _dapper= new DataContextDapper(config); //now we pass our confing with the conection strin in the constructor of the controller 
        _reusableSql = new ReusableSql(config);
        //string con= config.GetConnectionString("DefaultConnection");
       Console.WriteLine(config.GetConnectionString("DefaultConnection")); /* we pass our connection string inside here and prin
                                                we have access to the connection string our constructor  contain the connection*/
    }

    [HttpGet("TestConnection")]  // get request this retur correcnt datetime the get method is takina a function that return datime
    public DateTime TestConnection(){
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE");
    }


/*USING STORED PROCEDURES END POINTS GETTING ALL USERS OR SINGLE USER BY PASSING PARMETERS */
    [HttpGet("GetUsers/{userId}/{isActive}")] // http get request   ("GetUsers/{testValue}") get user input GET MULTIPLE USERS END POINT
    
    public IEnumerable <UserComplete>GetUsers(int userId, bool isActive)  //(string testValue)
    {
        string sql =@"EXEC TutorialAppSchema.spUsers_Get ";
        string stringParameters = "";
        DynamicParameters sqlParameters = new DynamicParameters();
         
         if (userId !=0){
          stringParameters+= ", @UserId= @UserIdParameter";
           sqlParameters.Add("@UserIdParameter",userId, DbType.Int32);
         }

          if (isActive){
          stringParameters+= ", @Active=@ActiveParameter";
           sqlParameters.Add("@ActiveParameter",isActive, DbType.Boolean);
         }

          if(stringParameters.Length>0) 
          {
            sql+= stringParameters.Substring(1);
          
          }
          
        //Console.WriteLine(sql);    we can use this to check the output of query and test it in azure

        IEnumerable <UserComplete>Users= _dapper.LoadDataWithParameters<UserComplete>(sql,sqlParameters);
        return Users;

    }

/*USING STORED PROCEDURES END POINTS EDIT OR INSERT   */

   [HttpPut("UpsertUser")] //will update user edit use details EDIT USER END POINT
              // IActionResult will return result to let us know about the request status fail or successfull with error message
   public IActionResult UpserUser(UserComplete user) // this end point will take as input a user model
   {
    
         if (_reusableSql.UpsertUser(user)) // if dapper run succesfully we run the ok method WE CALL DAPPER HERE AND DAPER WILL RUN THIS Q
         {
          return Ok(); // status code  the ok method is build method that come with ControllerBase class we inherited 
         }
         throw new Exception("Failed to Update User");
   }



////////STORED PROCEDURE TO DELETE A USER BY PASSING AN USER ID/////////////////////////////////////

[HttpDelete("DeleteUser/{userId}")]
public IActionResult DeleteUser(int userId)
{
string sql = @"TutorialAppSchema.spUser_Delete 
      @UserId = @UserIdParameter";

 DynamicParameters sqlParameters = new DynamicParameters();
sqlParameters.Add("@UserIdParameter",userId,DbType.Int32);



//Console.WriteLine(sql);
if (_dapper.ExecuteSqlWithParameters(sql,sqlParameters)) // if dapper run succesfully we run the ok method WE CALL DAPPER HERE AND DAPER WILL RUN THIS Q
{
return Ok(); // status code  the ok method is build method that come with ControllerBase class we inherited 
}
throw new Exception("Failed to Delete user");

}   


























// [HttpPost("UserSalary")]
// public IActionResult PostUserSalary (UserSalary userSalaryForInsert)
// { // start of post 

// string sql = @"
// INSERT INTO TutorialAppSchema.UserSalary(
//    UserId,
//    Salary
// ) VALUES (" + userSalaryForInsert.UserId
//      + ", " + userSalaryForInsert.Salary
//      + ")";

// if (_dapper.ExecuteSql(sql))
// {
//     return Ok (userSalaryForInsert);
// }

// throw new Exception ("Adding userSalary failed on save");
// } // end of post 



// [HttpPut("UserSalary")]

// public IActionResult PutUserSalary (UserSalary userSalaryForUpdate)
// { // start of put 
// string sql ="UPDATE TutorialAppSchema.UserSalary SET Salary ="
// + userSalaryForUpdate.Salary
// +"WHERE UserId="+ userSalaryForUpdate.UserId.ToString();

// if(_dapper.ExecuteSql(sql))
// {
//     return Ok(userSalaryForUpdate);
// }
// throw new Exception ("Updating UserSalary failed on save");

// } // end of put


// [HttpDelete("UserSalary/{userId}")]
// public IActionResult DeleteUserSalary (int userId)
// { // start of delete 

// string sql="DELETE FROM TutorialAppSchema.UserSalary WHERE UserId="+ userId.ToString();

// if (_dapper.ExecuteSql(sql))
// {
//    return Ok(); 
// }

// throw new Exception ("Deleting User Salary failed on server");
// } // end of delete 











}  // end of controller class this class contain all of our EndPoints
