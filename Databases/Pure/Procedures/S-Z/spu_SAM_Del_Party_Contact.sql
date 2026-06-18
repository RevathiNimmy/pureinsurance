SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Del_Party_Contact'
GO

/*******************************************************************************************************/
/* spu_SAM_Del_Party_Contact     */                                                                              
/* Delete Contacts for Party */
/*******************************************************************************************************/

CREATE PROCEDURE spu_SAM_Del_Party_Contact
    @party_cnt int
AS

SET NOCOUNT ON

DELETE FROM 
      Party_contact_usage 
Where
      party_cnt = @party_cnt	

SET NOCOUNT OFF

GO

