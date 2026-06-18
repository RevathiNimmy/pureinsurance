SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Next_Agent_Shortname_sel'
GO


CREATE PROCEDURE spu_Next_Agent_Shortname_sel
    @code char(10),
    @Shortname int OUTPUT
AS


/********************************************************************************************************/
/* Stored Procedure spu_Next_Agent_Shortname_sel                             */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                            05/01/1998  JB  */
/********************************************************************************************************/
Select Distinct
    @Shortname = Max(Convert(Int, Party.shortname))
From
    Party,
    Party_Type
Where
    Party_Type.Code = @code
    AND Party_Type.party_type_id = Party.party_type_id
GO


