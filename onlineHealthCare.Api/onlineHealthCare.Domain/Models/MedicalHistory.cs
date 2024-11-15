﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Domain.Models
{
    public class MedicalHistory
    {
        [Key]
        public int MedicalHistoryID { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Patient { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfEntry { get; set; }

        [MaxLength(255)]
        [Required]
        public string MedicalCondition { get; set; }
        [MaxLength(255)]
        public string Medications { get; set; }
        [MaxLength(255)]
        public string Allergies { get; set; }
        [MaxLength(255)]
        public string Surgeries { get; set; }
        [MaxLength(255)]
        public string FamilyMedicalHistory { get; set; }

        public MedicalHistory()
        {
            DateOfEntry = DateTime.Now;
        }
    }

}
