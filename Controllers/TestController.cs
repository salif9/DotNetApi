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

public class TestController: ControllerBase // the we build the controller class that is inheriting from ControllerBase class 
{
    
  private readonly DataContextDapper _dapper; //  creat an instance of dapper is a parameter in the controller class 
 // private readonly ReusableSql _reusableSql;
    public TestController(IConfiguration config)  // constructor of usercontroller take as parameter dapper connection string 
    {
        _dapper= new DataContextDapper(config); //now we pass our confing with the conection strin in the constructor of the controller 
      //  _reusableSql = new ReusableSql(config);
        //string con= config.GetConnectionString("DefaultConnection");
       Console.WriteLine(config.GetConnectionString("DefaultConnection")); /* we pass our connection string inside here and prin
                                                we have access to the connection string our constructor  contain the connection*/
    }

    [HttpGet("Connection")]  // get request this retur correcnt datetime the get method is takina a function that return datime
    
    public DateTime Connection()
    {
    return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }


    [HttpGet]

    public string Test()
    {
        return "Salifo Bance your application is up and running correctly on AZURE Yea!! now you need to build UI";
    }









}  // end of controller class this class contain all of our EndPoints
