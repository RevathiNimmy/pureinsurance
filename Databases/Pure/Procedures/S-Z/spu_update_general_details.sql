SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_update_general_details'
GO

CREATE PROCEDURE spu_update_general_details  
    @userdefinedperildataid int,  
    @ClaimId int,  
    @value varchar(50)  
AS  
  
  
    UPDATE User_Defined_peril_data  
       SET value=@value  
     WHERE user_defined_peril_data_id=@userdefinedperildataid  
       AND Claim_Id = @ClaimId  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
