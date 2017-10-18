using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using Xamarin.Forms;
using System.Diagnostics;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;

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

        public short DrugTypeEnum { get; set; }

        [Ignore, JsonIgnore]
        public DrugType DrugType { get => DrugTypeConverter.FromShort(DrugTypeEnum); set => DrugTypeEnum = value.Id; }
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

                yield return Pills;
                //yield return Tablet;
                yield return Inhaler;
                yield return Capsules;
                yield return Syrup;
            }
        }

        public static readonly DrugType Pills = new DrugType("Pills", 0);
        //public static readonly DrugType Tablet = new DrugType("Tablet", 1);
        public static readonly DrugType Inhaler = new DrugType("Inhaler", 1);
        public static readonly DrugType Capsules = new DrugType("Capsules", 2);
        public static readonly DrugType Syrup = new DrugType("Syrup", 3);

        private string name;
        private short id;

        public DrugType(string name, short Id)
        {
            this.name = name;
            this.id = Id;
        }


        public string Name { get => name; }
        public short Id { get => id; }

        public override string ToString()
        {
            return Name;
        }

        public static DrugType FromString(string name)
        {
            return Values.First(drug => drug.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            //foreach (var drug in Values) if (drug.Name.ToLower().Equals(name)) return drug;
            //return default(DrugType);
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

        public static DrugType FromShort(short Id)
        {
            foreach (DrugType drugType in DrugType.Values)
            {
                if (drugType.Id == Id) return drugType;
            }

            throw new NotFoundException("Cannot find DrugType from Id");
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
