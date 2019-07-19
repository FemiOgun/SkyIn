using Dapper;
using SkyInsuranceThird.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkyInsuranceThird.Controllers
{
    public class SkyInsuranceController : Controller
    {
        // Create connection string to DB

        static string strConnectionString = @"Data Source=iss-dev02;Initial Catalog=MVCDapperDB;Integrated Security=True";
               
        // GET: SkyInsurance 
        // List All Users (Main Page)

        public ActionResult Index()
        {
           
            List<User> UserList = new List<User>();
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                UserList = db.Query<User>("Select * From [SITest].[dbo].[User]").ToList();
            }
            if (TempData.ContainsKey("addv"))
                TempData["addv"].ToString();
                
            return View(UserList);
        }

        public ActionResult Search(string searchBy, string search)
        {
            if (searchBy == "Name")
            {
                List<User> UserList = new List<User>();
                using (IDbConnection db = new SqlConnection(strConnectionString))
                {
                    UserList = db.Query<User>("Select * From [SITest].[dbo].[User] WHERE Name LIKE '%"+search+"%'").ToList();
                }

                return View(UserList);
            }

            else if (searchBy == "Postcode")
            {
                List<User> UserList1 = new List<User>();
                using (IDbConnection db = new SqlConnection(strConnectionString))
                {
                    UserList1 = db.Query<User>("Select * From [SITest].[dbo].[User] WHERE Postcode LIKE '%"+search+"%'").ToList();
                }

                return View(UserList1);
            }

            else if (searchBy == "Registration")
            {
                List<User> UserVehicleList = new List<User>();
                using (IDbConnection db = new SqlConnection(strConnectionString))
                {
                    UserVehicleList = db.Query<User>("SELECT [SITest].[dbo].[User].[Name], " +
                        "[SITest].[dbo].[User].[Postcode], " +
                        "[SITest].[dbo].[Vehicle].[Registration] " +
                        "FROM[SITest].[dbo].[User] " +
                        "LEFT JOIN[SITest].[dbo].[Vehicle] ON[SITest].[dbo].[User].[Id] = [SITest].[dbo].[Vehicle].[UserId] " +
                        "WHERE Registration LIKE '%"+search+"%' " +
                        "ORDER BY[SITest].[dbo].[User].[Name]").ToList();
                }

                return View(UserVehicleList);
            }

            else
            {
                return View();
            }
        }

        // GET: SkyInsurance/Details/5
        public ActionResult Details(int id)
        {
           
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                
                string sql = @"Select * from [SITest].[dbo].[User] WHERE id = " + id +                    
                             @"Select * from [SITest].[dbo].[Vehicle] WHERE userid = " + id;

                var multi = db.QueryMultiple(sql, new { id });
                var user = multi.Read<User>().Single();
                var vehicles = multi.Read<Vehicle>().ToList();

                user.Vehicles = vehicles;
                return View(user);
            }

        }

        // GET: SkyInsurance/CreateUser
        // Empty Form to create User.

        public ActionResult CreateUser()
        {
            return View();
        }

        // POST: SkyInsurance/CreateUser
        // Creates a User 
        [HttpPost]
        public ActionResult CreateUser(User _user)
        {
            
            using(IDbConnection db = new SqlConnection(strConnectionString))
            {
                string sqlQuery = "Insert Into [SITest].[dbo].[User] (Name,Postcode) Values('" + _user.Name + "','" + _user.Postcode + "')";

                int rowsAffected = db.Execute(sqlQuery);
            }
            return RedirectToAction("Index");
        }

        // GET: SkyInsurance/Create
        public ActionResult CreateVehicle(int id)
        {
            Vehicle _vehicle = new Vehicle();
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                _vehicle = db.Query<Vehicle>("Select Id from [SITest].[dbo].[User]" + "WHERE Id =" + id, new { id }).SingleOrDefault();
            }

            return View(_vehicle);   
        }

        [HttpPost]
        public ActionResult CreateVehicle(int id, Vehicle _vehicle)
        {
            
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                string sqlQuery = @"INSERT INTO SITest.dbo.Vehicle(UserId, Registration, MakeModel) 
                                    Values("+ id + @", '"+_vehicle.Registration+@"', '" + _vehicle.MakeModel + @"')";
                int rowsAffected = db.Execute(sqlQuery);
            }
            TempData["addv"] = "Vehicle was added to User!";

            return RedirectToAction("Index");
        }

        // GET: SkyInsurance/Edit/5
        public ActionResult Edit(int id)
        {
            User _user = new User();
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                _user = db.Query<User>(@"Select * from [SITest].[dbo].[User]" + "WHERE id =" + id).SingleOrDefault();

            }
            return View(_user);
        }

        // POST: SkyInsurance/Edit/5
        [HttpPost]
        public ActionResult Edit(User _user)
        {
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                string sqlQuery = "Update [SITest].[dbo].[User] set Name='" + _user.Name + "',Postcode='" + _user.Postcode + "' where Id=" + _user.Id;

                int rowsAffected = db.Execute(sqlQuery);
            }
            return RedirectToAction("Index");
        }

       
        // GET: SkyInsurance/Delete/5
        public ActionResult Delete(int id)
        {
            User _user = new User();
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                _user = db.Query<User>(@"Select * from [SITest].[dbo].[User]" + "WHERE id =" + id).SingleOrDefault();
            }
            return View(_user);
        }

        // POST: SkyInsurance/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                string sqlQuery = "Delete from [SITest].[dbo].[User] WHERE id =" + id;

                int rowsAffected = db.Execute(sqlQuery);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteVehicle(int id)
        {
            Vehicle vehicle = new Vehicle();
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                vehicle = db.Query<Vehicle>("Select * from [SITest].[dbo].[Vehicle] WHERE id =" + id).SingleOrDefault();
            }

            return View(vehicle);
        }

        [HttpPost]
        public ActionResult DeleteVehicle(int id, FormCollection collection)
        {
            using (IDbConnection db = new SqlConnection(strConnectionString))
            {
                string sqlQuery = "Delete from [SITest].[dbo].[Vehicle] WHERE id =" + id;
                int rowsAffected = db.Execute(sqlQuery);
            }

            return RedirectToAction("Index");
        }
    }
}
