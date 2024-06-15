using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomAPI.Models;

public partial class File
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FileId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public int? Iduser { get; set; }

    public byte[]? FileData { get; set; }

    public DateTime CreationDate { get; set; }

    [ForeignKey("Iduser")]
    public virtual User? IduserNavigation { get; set; }
}       
