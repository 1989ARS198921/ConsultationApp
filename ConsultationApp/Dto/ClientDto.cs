﻿using System.ComponentModel.DataAnnotations;

namespace ConsultationApp.Dto
{
    public class ClientDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}