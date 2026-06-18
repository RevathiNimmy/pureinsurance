SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_BankGuarantee'
GO

CREATE PROCEDURE spu_wp_BankGuarantee
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,
    @Instance2 INT,  
    @Instance3 INT  
  
AS 

	SELECT Bank_Guarantee.BG_ref, Bank.Bank_Name, Bank_Guarantee.Bank_Branch,
		Bank_Guarantee.Issue_date, Bank_Guarantee.Expiry_Date 
	FROM Bank_Guarantee 
	INNER JOIN Bank ON Bank.Bank_id=Bank_Guarantee.Bank_name_id
	INNER JOIN Party ON Party.Party_cnt=Bank_Guarantee.Party_cnt
	WHERE Bank_Guarantee.Party_cnt=@PartyCnt

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO