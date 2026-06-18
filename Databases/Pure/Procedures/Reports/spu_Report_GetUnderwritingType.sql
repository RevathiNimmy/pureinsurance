SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_GetUnderwritingType'
GO


CREATE PROCEDURE spu_Report_GetUnderwritingType
    @UWType char(1) OUTPUT
AS

/**********************************************************************************************************************************
** Created by Jude Killip
** 19/11/2001
**
** NAME:        sp_Report_GetUnderwritingType
**
** PARAMETERS:  @UWType char (1)            OUTPUT
**
** USAGE:       DECLARE @UWType char (1)
**              EXECUTE spu_Report_GetUnderwritingType @UWType OUTPUT
**
** DESCRIPTION: Gets UW_Type from Hidden_Options. Used for deciding whether "Reinsurer" or "Insurer" is displayed
**              on reports. Default is 'U' ("Reinsurer")
** SJP 13/06/2002 Underwriting Type (UWType) is selected by option			 
**		   number and branch (ensuring unique record retrieved)   						
***********************************************************************************************************************************/

SELECT @UWType = ISNULL(UW_Type, 'U') FROM Hidden_Options where branch_id = 1 and option_number = 1
GO


