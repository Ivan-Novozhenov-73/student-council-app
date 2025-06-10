using System;

namespace StudentCouncilAPI.DTOs
{
    public class PartnerDto
    {
        public required Guid PartnerId { get; set; }
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public string? Patronymic { get; set; }
        public required string Description { get; set; }
        public required long Phone { get; set; }
        public required string Contacts { get; set; }
        public required bool Archive { get; set; }
    }

    public class CreatePartnerDto
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string Description { get; set; }
        public long Phone { get; set; }
        public string Contacts { get; set; }
    }

    public class UpdatePartnerDto
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public string? Description { get; set; }
        public long? Phone { get; set; }
        public string? Contacts { get; set; }
        public bool? Archive { get; set; }
    }
}