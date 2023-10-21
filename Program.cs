using System.Text;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);  /* before we use have only this line inside our Program.cs
the below code was inside THE STARTUP.CS FILE but now we have all in file called program.cs file*/
// Add services to the container.

// THE OLD VERSION DOES NOT HAVE SWAGGER UI build in.. new versin does
/* the below line of code are part of the swagger user interface that come with the API template code 
before was not like that. this template give us an user graphical interface where we can use to start our code
is build in IPA tester frame work that will help us to visulise results   */

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//the below code allow us to create RESOURCE POLICY SHARING  using Cors Policy
builder.Services.AddCors((options) =>
{
       options.AddPolicy("DeveCors",(corsBuilder) =>  /* Cors i cross-oringin resource sharing our is the URL Where our API live at 
                                                      cors policy allow sharing of resources */
            {
                /* this define what we allow into our API angular 4200 port,react 3000 port, View 8000 port will work with this 
                frameworks i could just decide to use dipending on my interst */
            corsBuilder.WithOrigins("http://localhost:4200","http://localhost:3000","http://localhost:8000")
            .AllowAnyMethod() //allow all method get, post,put and delete request 
            .AllowAnyHeader()
            .AllowCredentials(); // cookies 
          });

          options.AddPolicy("ProdCors",(corsBuilder) =>
            {
              corsBuilder.WithOrigins("https://myProductionSite.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
          });

});

// builder.Services.AddScoped<IUserRepository,UserRepository>(); DOTNET 7


// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)// to use this AddAuthentication() we need to use install nuget package.
//  .AddJwtBearer(options=>{
//   options.TokenValidationParameters= new TokenValidationParameters()
//   {
//     ValidateIssuerSigningKey=true,
//     IssuerSigningKey= new SymmetricSecurityKey( Encoding.ASCII.GetBytes(
//       builder.Configuration.GetSection("AppSettings:TokenKey").Value
//     )),
//     ValidateIssuer = false,
//     ValidateAudience = false
//   };
//  });


string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    tokenKeyString != null ? tokenKeyString : ""
                )),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
var app = builder.Build();  // this createe the application

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())  /* DEPLOIMENT:  this check if the application is on development or not i
                                        is it will show the swagger UI if the application will be deploied successully                                     it will not be possible to see the swagger UI */
{
    app.UseCors("DeveCors"); // using our policies in development process 
    app.UseSwagger(); // in development we use swagger UI
    app.UseSwaggerUI(); 
}
else{  
app.UseCors("ProdCors"); // using our policies not development process 
    /*if we are in development it will use HTTPS Redirection */
app.UseHttpsRedirection(); // check if the app is running on HTTPS if it is running of HTTP it will redirect it to HTTPS

}


app.UseAuthentication();

app.UseAuthorization(); // IS A DEFAUL WE USE OR NOT BECAUSE WE CAN ALSO BUILD OUR OWN AUTHORIZATION 

app.MapControllers(); // THIS TAKE EVERY FROM THE BUILD IN CONTROLLER 

app.Run();


/* This build in starting code is simpler compared to the old versin because we have all code in one place 
  */