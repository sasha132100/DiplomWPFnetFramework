using DiplomWPFnetFramework.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace DiplomWPFnetFramework.Classes
{
    internal class CreatingObjects
    {
        public static Passport CreatingPassportObject(int id, string serialNumber, string divisionCode, DateTime giveDate, string byWhom, string fio, DateTime birthDate, string gender, string birthPlace, string residencePlace)
        {
            using (var db = new test123Entities1())
            {
                Passport passport = new Passport();
                passport.Id = id;
                passport.SerialNumber = serialNumber;
                passport.DivisionCode = divisionCode;
                passport.GiveDate = giveDate;
                passport.ByWhom = byWhom;
                passport.FIO = fio;
                passport.BirthDate = birthDate;
                passport.Gender = gender;
                passport.BirthPlace = birthPlace;
                passport.ResidencePlace = residencePlace;
                return passport;
            }
        }

        public static Items CreateNewItemObject(string itemType)
        {
            Items item = new Items();
            item.Title = "NewTitle";
            item.IType = itemType;
            item.IPriority = 0;
            item.IsHidden = 0;
            item.IsSelected = 0;
            item.DateCreation = DateTime.Now;
            item.UserId = SystemContext.User.Id;
            return item;
        }

    }
}
