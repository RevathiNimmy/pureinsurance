SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Policy_Client_delall'
GO

CREATE PROCEDURE spu_Policy_Client_delall
    @insurance_folder_cnt int
AS

    DELETE FROM 
        Policy_Client
    WHERE
        insurance_folder_cnt = @insurance_folder_cnt

GO

