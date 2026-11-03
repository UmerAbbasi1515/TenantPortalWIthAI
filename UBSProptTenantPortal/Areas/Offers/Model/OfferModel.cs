using System.Text.Json.Serialization;

public class OfferRecord
{
    public int OfferId { get; set; }
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? ValidateFrom { get; set; }
    public string? ValidateTo { get; set; }
}

public class OfferResponse
{
    public string? Statsus { get; set; } // Note: There's a typo in "statsus"
    public List<OfferRecord>? Record { get; set; }
}


// Detail
public class OfferDetailResponse
{
    public string? Statsus { get; set; } // Note: Typo preserved from JSON
    public OfferRecordDetail? Record { get; set; }
    public List<object>? OfferProperties { get; set; } // Replace 'object' with actual property type if known
}

public class OfferRecordDetail
{
    public int OfferId { get; set; }
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? ValidateFrom { get; set; } // Use DateTime if you prefer, with parsing
    public string? ValidateTo { get; set; }
    public string? Description { get; set; }
    public string? DesriptionAR { get; set; } // Typo preserved from JSON
}


// New offers
public class OffersNResponseModel
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("offers")]
    public List<OfferModel>? Offers { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
public class OfferModel
{
    [JsonPropertyName("offerNo")]
    public string? OfferNo { get; set; }

    [JsonPropertyName("offerID")]
    public int? OfferID { get; set; }

    [JsonPropertyName("offerName")]
    public string? OfferName { get; set; }

    [JsonPropertyName("issueDate")]
    public string? IssueDate { get; set; }

    [JsonPropertyName("expiryDate")]
    public string? ExpiryDate { get; set; }

    [JsonPropertyName("period")]
    public int? Period { get; set; }

    [JsonPropertyName("rentStartDate")]
    public string? RentStartDate { get; set; }

    [JsonPropertyName("rentEndDate")]
    public string? RentEndDate { get; set; }

    [JsonPropertyName("offerAmt")]
    public double? OfferAmt { get; set; }

    [JsonPropertyName("securityAmount")]
    public double? SecurityAmount { get; set; }

    [JsonPropertyName("reservationFee")]
    public double? ReservationFee { get; set; }

    [JsonPropertyName("paymentInstallments")]
    public int? PaymentInstallments { get; set; }

    [JsonPropertyName("propertyName")]
    public string? PropertyName { get; set; }

    [JsonPropertyName("propertyNameAR")]
    public string? PropertyNameAR { get; set; }

    [JsonPropertyName("buildingRefNo")]
    public string? BuildingRefNo { get; set; }

    [JsonPropertyName("offerStatus")]
    public string? OfferStatus { get; set; }
}
