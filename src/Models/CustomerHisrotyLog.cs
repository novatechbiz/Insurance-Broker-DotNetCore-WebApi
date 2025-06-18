public class CustomerHisrotyLog
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int CustomerIdentificationTypeId { get; set; }
    public int CustomerTypeId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? IdentificationNo { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ContactNo { get; set; } = string.Empty;
    public string? WhatsAppNo { get; set; }
    public string? EmailAddress { get; set; }
    public int GenderTypeId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int RecordStatusId { get; set; }
    public int? EnteredBy { get; set; }
    public DateTime? EnteredDate { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
