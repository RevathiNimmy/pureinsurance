SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Check_PMUser_Details'
GO

-- Looks up a user name in the user table and returns all relevant information for use by SA.NET security code.  
--  
-- Parameters:  
--  @username   User name  
--  <others>    User data if valid user, NULL if not  
--  
CREATE PROCEDURE spu_SAM_Check_PMUser_Details  
    @username varchar(255),  
    @password varchar(30) output,  
    @user_id smallint output,  
    @language_id smallint output,  
    @email_address varchar(255) output,  
    @party_cnt integer output,  
    @party_type_code char(10) output  
AS BEGIN  
    SET NOCOUNT ON  
  
    DECLARE @effective_date datetime  
    SELECT @effective_date = GETDATE()  
  
    SELECT  
        @password = NULL,  
        @user_id = NULL,  
        @language_id = NULL,  
        @email_address = NULL,  
        @party_cnt = NULL,  
        @party_type_code = NULL  
  
    SELECT  
        @password = PMUser.password,  
        @user_id = PMUser.user_id,  
        @language_id = PMUser.language_id,  
        @email_address = PMUser.email_address,  
        @party_cnt = PMUser.party_cnt,  
        @party_type_code = Party_Type.code  
        FROM PMUser with(nolock)  
        LEFT OUTER JOIN Party With(nolock) ON PMUser.party_cnt = Party.party_cnt  
        LEFT OUTER JOIN Party_Type With(nolock) on Party.party_type_id = Party_Type.party_type_id  
        WHERE PMUser.username = @username  
        AND PMUser.is_deleted = 0  
        AND PMUser.effective_date <= @effective_date  
  
    
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
