SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Policy_Client_add'
GO

CREATE PROCEDURE spu_Policy_Client_add
    @insurance_folder_cnt integer ,
    @party_cnt integer ,
    @is_lead tinyint ,
    @correspondence tinyint
AS 

    INSERT INTO Policy_Client (
        insurance_folder_cnt ,
        party_cnt ,
        is_lead ,
        correspondence )
    VALUES (
        @insurance_folder_cnt ,
        @party_cnt ,
        @is_lead ,
        @correspondence )

GO

