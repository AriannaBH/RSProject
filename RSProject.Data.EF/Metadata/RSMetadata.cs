using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RSProject.Data.EF
{
    [MetadataType(typeof(CustomerAssetMetadata))]
    public partial class CustomerAsset
    {
        
    }

    public class CustomerAssetMetadata
    {
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Customer Asset ID")]
        public int CustomerAssetID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Team Name")]
        [StringLength(50, ErrorMessage = "50 Character Max!")]
        public string AssetName { get; set; }

        //FK field
        [Required(ErrorMessage = "This field is required!")]
        //[StringLength(128, ErrorMessage = "128 Character Max!")]
        [Display(Name = "Coach Name")]
        public string OwnerID { get; set; }

        [Display(Name = "Team Photo")]
        [StringLength(50, ErrorMessage = "50 Character Max!")]
        public string AssetPhoto { get; set; }

        [Display(Name = "Comments")]
        [StringLength(300, ErrorMessage = "300 Character Max!")]
        [UIHint("MultilineText")]
        public string SpecialNotes { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Date Added")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[-Date Not Provided-]")]
        public System.DateTime DateAdded { get; set; }
    }

    [MetadataType(typeof(LocationMetaData))]
    public partial class Location
    {

    }

    public class LocationMetaData
    {
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Location ID")]
        public int LocationID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Location Name")]
        [StringLength(50, ErrorMessage = "50 Character Max!")]
        public string LocationName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(100, ErrorMessage = "100 Character Max!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(100, ErrorMessage = "100 Character Max!")]
        public string City { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(2, ErrorMessage = "2 Character Max!")]
        public string State { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(5, ErrorMessage = "5 Character Max!")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Reservation Limit")]
        public byte ReservationLimit { get; set; }
    }

    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservation
    {

    }

    public class ReservationMetadata
    {
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Reservation ID")]
        public int ReservationID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Customer Asset ID")]
        public int CustomerAssetID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Location ID")]
        public int LocationID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Service ID")]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Reservation Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true, NullDisplayText = "[-Date Not Provided-]")]
        public System.DateTime ReservationDate { get; set; }

        [Display(Name = "Reservation Time")]
        [DataType(DataType.Time)]
        public System.DateTime ReservationTime { get; set; }
    }

    [MetadataType(typeof(ServiceMetadata))]
    public partial class Service
    {

    }

    public class ServiceMetadata
    {
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Service ID")]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(50, ErrorMessage = "50 Character Max!")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "300 Character Max!")]
        [UIHint("MultilineText")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public decimal Price { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        [Display(Name ="Coach")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }

    public class UserDetailMetadata
    {
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "User ID")]
        [StringLength(128, ErrorMessage = "128 Character Max!")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "50 Character Max!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "50 Character Max!")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "This field is required!")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string Phone { get; set; }

        [StringLength(256, ErrorMessage = "256 Character Max!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
    }
}
