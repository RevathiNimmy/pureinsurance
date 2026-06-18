SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Add_PartyLoyaltyScheme'
GO
CREATE PROCEDURE spu_SAM_Add_PartyLoyaltyScheme      
    @party_loyalty_scheme_id     integer OUTPUT,      
    @party_cnt                   integer,      
    @loyalty_scheme_id           integer,      
    @membership_number           varchar(50),      
    @other_reference             varchar(50),      
    @start_date                  datetime,      
    @end_date                    datetime,      
    @main_membership_number      varchar(50),      
    @is_active                   tinyint      
AS      
BEGIN      
  
 INSERT INTO Party_Loyalty_Scheme (      
    party_cnt,      
    loyalty_scheme_id,      
    membership_number,      
    other_reference,      
    start_date,      
    end_date,      
    main_membership_number,      
    is_active)      
 VALUES (      
    @party_cnt,      
    @loyalty_scheme_id,      
    @membership_number,      
    @other_reference,      
    @start_date,      
    @end_date,      
    @main_membership_number,      
    @is_active)      
    
-- return the new row id as a return param      
SELECT @party_loyalty_scheme_id = @@IDENTITY 
END  

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  