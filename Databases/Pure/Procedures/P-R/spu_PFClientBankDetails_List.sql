EXECUTE DDLDropProcedure 'spu_PFClientBankDetails_List'
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO




CREATE PROCEDURE spu_PFClientBankDetails_List
    @ClientID int


AS BEGIN


    SELECT DISTINCT 
        BankName, BankSortCode, BankAccountNo, BankAccountName, 
        BankBranch, BankAddr1, BankAddr2, BankAddr3, BankTown, 
        BankRegion, BankPCode, bc.country_id as BankCountry, BankAreaCode, BankPhoneNo, 
        BankExtension, BankFaxAreaCode, BankFaxNo, Party_bank_id
    FROM pfPremiumFinance p
        LEFT OUTER JOIN 
            country bc
        ON 
            bc.description = p.bankcountry
    WHERE ClientID = @ClientID
	AND	BankAccountNo<>''
END
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



