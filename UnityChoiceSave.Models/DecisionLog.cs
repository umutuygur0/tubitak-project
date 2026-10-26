using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnityChoiceSave.Models
{
    [Table("decision_logs")]
    public class DecisionLog
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("child_id")]
        public int ChildId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("scenario_id")]
        public string ScenarioId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("question_id")]
        public string QuestionId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("selected_option")]
        public string SelectedOption { get; set; } = string.Empty;

        [Column("response_time_seconds")]
        public float ResponseTimeSeconds { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}