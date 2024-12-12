using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MechanicApp.Shared
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid JobId { get; set; }

        [Required]
        [ForeignKey("Client")]
        public Guid ClientId { get; set; }

        [Required]
        [WhiteSpaceAndNullVerification]
        [NumberPlateVerification]
        public string NumberPlate { get; set; }

        [Required]
        [Range(typeof(DateTime), "1900-01-01", "2024-06-01")]
        public DateTime YearOfProduction { get; set; }

        public enum JobCategory
        {
            Bodywork,
            Engine,
            Undercarriage,
            Brakes
        }

        [Required]
        [EnumDataType(typeof(JobCategory))]
        public JobCategory Category { get; set; }

        [Required]
        [WhiteSpaceAndNullVerification]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range (1, 10)]
        public int Seriousness { get; set; }

        public enum JobStatus
        {
            Assigned,
            InProgress,
            Completed
        }

        [Required]
        [EnumDataType(typeof(JobStatus))]
        public JobStatus Status { get; set; }

        public double workHourEstimater()
        {
            int Category()
            {
                switch (this.Category)
                {
                    case JobCategory.Bodywork: return 3;

                    case JobCategory.Engine: return 5;

                    case JobCategory.Undercarriage: return 2;

                    case JobCategory.Brakes: return 1;

                    default: return 0;
                }
            }

            double Age()
            {
                int yearDifference = (DateTime.Now.Date - this.YearOfProduction).Days / 365;

                if (yearDifference < 5)
                {
                    return 0.5;
                }
                else if (yearDifference < 10)
                {
                    return 1;
                }
                else if (yearDifference < 20)
                {
                    return 1.5;
                }
                else
                {
                    return 2;
                }
            }

            double Seriousness()
            {
                if (this.Seriousness < 3)
                {
                    return 0.2;
                }
                else if (this.Seriousness < 5)
                {
                    return 0.4;
                }
                else if (this.Seriousness < 8)
                {
                    return 0.6;
                }
                else if (this.Seriousness < 10)
                {
                    return 0.8;
                }
                else
                {
                    return 1;
                }
            }

            return Category() * Seriousness() * Age();
        }
    }
}
