SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_HoldORAlt_Insurer'
GO


CREATE PROCEDURE spu_SIRRen_HoldORAlt_Insurer
    @gis_policy_link_id int,
    @gis_scheme_id int,
    @is_holding_insurer int OUTPUT
AS

/* Check if the insurer is holder or alternative insurer*/
/* History : SSL 31/07/2001 - Created */
/* SSL 09/08/2001 - Modified to pass only one parameter either 1 or 2 */
/* SSL 22/08/2001 - Updated with new input parameters*/
BEGIN
    DECLARE @abi_81_insurer int,
    @abi_code_on_81 int,
    @ins_file_type_id int,
    @Insurance_folder_Cnt int

    SELECT @ins_file_type_id = insurance_file_type_id
    FROM insurance_file_type where code = 'POLICY'

    SELECT @abi_81_insurer=abi_81_insurer
    FROM gis_insurer
    WHERE gis_insurer_id = (SELECT gis_insurer_id FROM gis_scheme
    WHERE gis_scheme_id = @gis_scheme_id)

    Select @Insurance_Folder_Cnt = I.Insurance_Folder_Cnt
    FROM Insurance_File I, Gis_Policy_Link P
    Where I.Insurance_file_cnt = P.Insurance_File_Cnt
    AND P.Gis_Policy_Link_Id = @gis_policy_link_id

    SELECT @abi_code_on_81=abi_code_on_81 FROM party P, insurance_file I
    WHERE I.Insurance_Folder_Cnt = @Insurance_Folder_Cnt
    AND I.insurance_file_status_id IS NULL
    AND I.insurance_file_type_id = @ins_file_type_id
    AND P.Party_Cnt = I.Lead_Insurer_Cnt

    IF @abi_81_insurer = @abi_code_on_81
    SET @is_holding_insurer = 1
    ELSE
    SET @is_holding_insurer = 2

END
GO


