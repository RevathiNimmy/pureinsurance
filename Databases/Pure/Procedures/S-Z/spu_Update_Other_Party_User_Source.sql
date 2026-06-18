SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Update_Other_Party_User_Source'
GO

CREATE PROCEDURE spu_Update_Other_Party_User_Source
@PartyCnt INT
AS

INSERT INTO pmuser_source (user_id,source_id)  
SELECT user_id , s.source_id from PMUSER p ,Source s Where other_party_id=@PartyCnt and s.is_deleted=0  and source_id not in
(
SELECT source_id FROM Other_Party_Branch WHERE party_cnt=@PartyCnt 
)
and not exists (SELECT user_id,source_id from PMUser_Source ps where ps.user_id=p.user_id and ps.source_id=s.source_id)
      
    
GO
