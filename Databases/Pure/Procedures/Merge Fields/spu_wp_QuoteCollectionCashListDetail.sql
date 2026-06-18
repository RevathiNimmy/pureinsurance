set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

EXEC DDLDropProcedure 'spu_wp_QuoteCollectionCashListDetail'
GO

CREATE Procedure spu_wp_QuoteCollectionCashListDetail 
    @PartyCnt INT,    
    @InsuranceFileCnt INT,    
    @RiskID INT,    
    @ClaimCnt INT,    
    @DocumentRef VARCHAR(25),    
    @Instance1 INT,    
    @Instance2 INT,    
    @Instance3 INT      
    
AS    

    SELECT cl.CashList_Ref 'CashList_Ref', cli.media_ref 'media_ref', m.description 'mediatype_id', d.document_ref 'documentref' 
    FROM cashlist cl    
 Join cashlistitem cli ON cl.cashlist_id=cli.cashlist_id    
 JOIN MediaType m on cli.mediatype_id=m.MediaType_id
 Join Transdetail t on cli.transdetail_id=t.transdetail_id
 Join Insurance_File_Payment_Details ifpd ON ifpd.transdetail_id=t.transdetail_id    
 JOIN document d on ifpd.document_id=d.document_id
    WHERE  ifpd.insurance_file_cnt = @InsuranceFileCnt  AND d.document_id=@Instance2  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO   