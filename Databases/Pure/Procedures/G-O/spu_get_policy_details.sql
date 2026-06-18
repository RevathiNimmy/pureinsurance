SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_policy_details'
GO


CREATE PROCEDURE spu_get_policy_details
    @pol_id int,
    @clm_dt datetime
AS

/************************************************************************************************************************/
/* Change History:  Updated so UW & Broking  use Insurance_File             */
/*          rather than Event_Insurance_File.       11/04/2001  RWH */
/*                                              */
/************************************************************************************************************************/
SELECT  cover_start_date,
    expiry_date,
    currency_id,
    insurance_folder_cnt
FROM    Insurance_File
WHERE   Insurance_File.insurance_file_cnt = @pol_id
GO


