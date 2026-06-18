SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyCnt_SelectAll'
GO


CREATE PROCEDURE spu_PartyCnt_SelectAll  

AS

SELECT party_cnt FROM Party(NOLOCK) WHERE is_deleted=0

GO