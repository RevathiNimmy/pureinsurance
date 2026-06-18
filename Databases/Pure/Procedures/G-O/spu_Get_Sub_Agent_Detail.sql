SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_Get_Sub_Agent_Detail
GO

CREATE PROCEDURE spu_Get_Sub_Agent_Detail    
    @party_code Varchar(20)=NULL,  
    @CoverStartDate as Date=NULL,  
    @CoverEndDate as Date=NULL,  
    @Insurance_file_cnt AS INT=0  
AS    
IF @Insurance_file_cnt<>0  
BEGIN  
SELECT @CoverStartDate=COVER_START_DATE FROM Insurance_File WHERE insurance_file_cnt =@Insurance_file_cnt  
END  
  
  
IF EXISTS (SELECT pcy.CertYearStartDate, pcy.CertYearEndDate  
FROM party_certificate_year pcy  
LEFT JOIN party p ON p.party_cnt=pcy.party_cnt    
WHERE p.shortname  =@party_code AND  @CoverStartDate>=pcy.CertYearStartDate AND @CoverStartDate <= pcy.CertYearEndDate AND pcy.IsDeleted <>1)  
BEGIN  
SELECT 'VALID'  
END  
ELSE  
BEGIN  
SELECT 'INVALID'  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

  
  