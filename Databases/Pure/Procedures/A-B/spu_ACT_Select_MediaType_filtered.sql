SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_MediaType_filtered'
GO

CREATE PROCEDURE spu_ACT_Select_MediaType_filtered
    @PaymentsOnly TINYINT = 0,
    @ReceiptsOnly TINYINT = 0,
    @ForceInclusionOfNAPaymentType BIT = 0
AS 

IF ISNULL(@PaymentsOnly,0) = 1  
BEGIN  
    IF @ForceInclusionOfNAPaymentType = 0  
    BEGIN  
        SELECT  
            mediatype_id,  
            caption_id,  
            description,  
            code,  
            mediatype_validation_id,  
            is_rounding_enabled,  
            is_validation_enabled,  
            is_banking,  
            is_stoppable,  
            is_receipt,  
            is_payment,  
            is_manual_payment,  
            is_media_reference_mandatory,  
   --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
            is_receipt_printed_automatically,  
            numbering_scheme_id,  
            is_readonly, isnull(is_additional_details,0)
--(Emd)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
        FROM mediatype  
        WHERE is_payment = 1  
        AND effective_date < GETDATE()  
        AND is_deleted = 0  
        ORDER BY description  
    END  
    ELSE  
    BEGIN  
        SELECT  
            mediatype_id,  
            caption_id,  
            description,  
            code,  
            mediatype_validation_id,  
            is_rounding_enabled,  
            is_validation_enabled,  
            is_banking,  
            is_stoppable,  
            is_receipt,  
            is_payment,  
            is_manual_payment,  
            is_media_reference_mandatory,  
  --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
            is_receipt_printed_automatically,  
            numbering_scheme_id,  
            is_readonly, isnull(is_additional_details,0)
        --(Emd)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
        FROM mediatype  
        WHERE code = 'N/A'  
        OR  (  
                effective_date < GETDATE()  
                AND  
                is_deleted = 0  
                AND  
                is_payment = 1  
            )  
        ORDER BY description  
    END  
END  
ELSE IF ISNULL(@ReceiptsOnly,0) = 1  
BEGIN  
    SELECT  
        mediatype_id,  
        caption_id,  
        description,  
        code,  
        mediatype_validation_id,  
        is_rounding_enabled,  
        is_validation_enabled,  
        is_banking,  
        is_stoppable,  
        is_receipt,  
        is_payment,  
        is_manual_payment,  
        is_media_reference_mandatory,  
   --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
        is_receipt_printed_automatically,  
        numbering_scheme_id,  
        is_readonly, isnull(is_additional_details,0)  
  
   --(Emd)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
    FROM mediatype  
    WHERE is_receipt = 1  
    AND effective_date < GETDATE()  
    AND is_deleted = 0  
    ORDER BY description  
END  
ELSE  
BEGIN  
    SELECT  
        mediatype_id,  
        caption_id,  
        description,  
        code,  
        mediatype_validation_id,  
        is_rounding_enabled,  
        is_validation_enabled,  
        is_banking,  
        is_stoppable,  
        is_receipt,  
        is_payment,  
        is_manual_payment,  
        is_media_reference_mandatory,  
   --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
        is_receipt_printed_automatically,  
        numbering_scheme_id,  
        is_readonly, isnull(is_additional_details,0)  
  --(Emd)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.4.2)  
      
  
    FROM MediaType  
    WHERE effective_date < GETDATE()  
    AND is_deleted = 0  
    ORDER BY description  
END  


GO
