set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

EXEC DDLDropProcedure 'spu_wp_QuoteCollectionCashListDetail_Get_Keys'
GO

CREATE Procedure spu_wp_QuoteCollectionCashListDetail_Get_Keys
    @PartyCnt INT,    
    @InsuranceFileCnt INT,    
    @RiskID INT,    
    @ClaimCnt INT,    
    @DocumentRef VARCHAR(25),    
    @Instance1 INT,    
    @Instance2 INT,    
    @Instance3 INT      
    
AS    

    SELECT d.document_id  
    FROM Insurance_File_Payment_Details ifpd 
 JOIN document d on ifpd.document_id=d.document_id  
    WHERE  ifpd.insurance_file_cnt = @InsuranceFileCnt  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO   