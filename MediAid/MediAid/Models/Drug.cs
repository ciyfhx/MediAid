using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.Diagnostics;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace MediAid.Models
{
    //public class Drug : BaseModel
    //{
    //    public int DatabaseId { get; set; }
    //    public string Name { get; set; }
    //    public DrugType DrugType { get; set; }
    //    public string ImageFile { get; set; }

    //    public DrugData ToDrugData() { return new DrugData { DatabaseId = this.DatabaseId, Name = this.Name, DrugType = this.DrugType.ToString(), ImageFile = this.ImageFile }; }

    //}

    public class Drug : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int DatabaseId { get; set; }
        public string Name { get; set; }

        public string DrugTypeString { get; set; }

        [Ignore]
        public DrugType DrugType { get => DrugTypeConverter.FromString(DrugTypeString); set => DrugTypeString = value.Name; }
        public string ImageFile { get; set; }

        //public Drug ToDrug() { return new Drug { DatabaseId = this.DatabaseId, Name = this.Name, DrugType = DrugTypeConverter.FromString(this.DrugType), ImageFile = this.ImageFile }; }
    }

    public class ReminderDrug
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Drug))]
        public int DatabaseId { get; set; }

        [ForeignKey(typeof(Reminder))]
        public int ReminderId { get; set; }
    }


    //Create a list of drugs type
    public class DrugType
    {

        //Values
        public static IEnumerable<DrugType> Values
        {

            get
            {

                yield return Pill;
                yield return Tablet;
                yield return Capsule;
                yield return Liquid;
            }
        }

        public static readonly DrugType Pill = new DrugType("Pill");
        public static readonly DrugType Tablet = new DrugType("Tablet");
        public static readonly DrugType Capsule = new DrugType("Capsule");
        public static readonly DrugType Liquid = new DrugType("Liquid");

        private string name;

        public DrugType(string name)
        {
            this.name = name;
        }


        public string Name { get { return name; } }

        public override string ToString()
        {
            return Name;
        }


    }

    public sealed class DrugTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //If is List
            if (value is IEnumerable<DrugType>)
            {
                //Convert to string
                IEnumerable<DrugType> values = (IEnumerable<DrugType>)value;

                List<string> list = new List<string>();

                values.ToList().ForEach((drugType) => list.Add(drugType.ToString()));


                return list;

            }

            //If is DrugType
            else if (value is DrugType)
            {
                DrugType drugType = value as DrugType;

                return drugType.ToString();

            }
            else
            {
                throw new InvalidClassGenericException("Value expected to be DrugType but not");
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DrugType.Values;
        }

        public static DrugType FromString(string Name)
        {
            foreach(DrugType drugType in DrugType.Values)
            {
                if (drugType.Name.Equals(Name)) return drugType;
            }

            throw new NotFoundException("Cannot find DrugType from Name");
        }

    }

    sealed class NotFoundException : Exception
    {

        public NotFoundException(string reason) : base(reason)
        {

        }


    }


    sealed class InvalidClassGenericException : Exception
    {

        public InvalidClassGenericException(string reason) : base(reason)
        {
            
        }


    }

    //public enum DrugType {
    //  Pill
    //}

}
