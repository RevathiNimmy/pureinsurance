SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'Spu_wp_insurancefileTotalCommissionPercentage'
GO

CREATE PROCEDURE Spu_wp_insurancefileTotalCommissionPercentage

    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  

AS

   SELECT sum(premium_excluding_tax) as premium_excluding_tax,sum(commission_net)as commission_net,
   (sum(commission_net)*100)/sum(premium_excluding_tax)  as commission_percentage
   FROM  insurance_COB_section 
   WHERE insurance_file_cnt = @InsuranceFileCnt

GO