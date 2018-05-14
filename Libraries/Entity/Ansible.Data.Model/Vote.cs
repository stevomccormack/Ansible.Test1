using System;
using System.ComponentModel.DataAnnotations;

namespace Ansible.Data.Model
{
    public class Vote
    {
        #region Constructors

        public Vote()
        {
            VoteId = NewVoteId;
            VoteNumber = NewVoteNumber;
            CreatedBy = "sa";
            CreatedDateUtc = DateTime.UtcNow;
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

        #endregion

        #region IAuditBy Properties

        [Required]
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTimeOffset CreatedDateUtc { get; set; }

        [Required]
        [Display(Name = "Created By")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} must be less than {1} characters.")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTimeOffset? ModifiedDateUtc { get; set; }

        [Display(Name = "Modified By")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "{0} must be less than {1} characters.")]
        public string ModifiedBy { get; set; }

        #endregion


        #region Static Helpers

        public static int NewVoteId => new Random().Next(1,1000);

        public static string NewVoteNumber => $"VOTE{Guid.NewGuid().ToString().GetHashCode():x}";

        #endregion
    }
}
