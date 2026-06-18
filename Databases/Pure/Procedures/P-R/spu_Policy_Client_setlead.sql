SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Policy_Client_setlead'
GO

CREATE PROCEDURE spu_Policy_Client_setlead
    @insurance_folder_cnt integer ,
    @insurance_file_cnt integer,
    @party_cnt integer ,
    @resolved_name varchar(250)
AS 

    -- Update insurance_folder
    UPDATE insurance_folder SET
        insurance_holder_cnt = @party_cnt
    WHERE
        insurance_folder_cnt = @insurance_folder_cnt


    -- Update insurance_file
    UPDATE insurance_file SET
        insured_cnt = @party_cnt,
        insured_name = @resolved_name
    WHERE
        insurance_file_cnt = @insurance_file_cnt

GO

