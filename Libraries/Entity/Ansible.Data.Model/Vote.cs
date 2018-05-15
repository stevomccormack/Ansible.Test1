using System;
using System.ComponentModel.DataAnnotations;
using URF.Core.EF.Trackable;

namespace Ansible.Data.Model
{
    public partial class Vote : Entity
    {
        #region Constructors

        public Vote()
        {
            VoteNumber = NewVoteNumber;
        }

        #endregion

        #region Member Properties

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Vote Id")]
        public int VoteId { get; set; }

        [Required]
        [Display(Name = "Vote Number")]
        [StringLength(25, ErrorMessage = "{0} must be less than {1} characters.")]
        public string VoteNumber { get; set; }

        [Required]
        [Display(Name = "Given Name")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} must be less than {1} characters.")]
        public string GivenName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} must be less than {1} characters.")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Gender")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "{0} must be less than {1} characters.")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Age")]
        [DataType(DataType.Text)]
        public int Age { get; set; }

        [Required]
        [Display(Name = "Is Valid")]
        public bool IsValid { get; set; }

        #endregion

        #region Static Helpers

        public static string NewVoteNumber => $"VOTE{Guid.NewGuid().ToString().GetHashCode():x}";

        #endregion
    }
}
