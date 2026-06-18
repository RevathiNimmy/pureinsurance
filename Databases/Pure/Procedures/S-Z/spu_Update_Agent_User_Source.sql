SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_Update_Agent_User_Source'
GO

CREATE PROCEDURE spu_Update_Agent_User_Source
@Party_Cnt INT
AS

INSERT INTO pmuser_source (user_id,source_id)  
SELECT user_id , s.source_id from PMUSER p ,Source s Where party_cnt=@Party_Cnt and s.is_deleted=0  and source_id not in
(
SELECT source_id FROM Party_Agent_Branch WHERE party_cnt=@Party_Cnt 
)
and not exists (SELECT user_id,source_id from PMUser_Source ps where ps.user_id=p.user_id and ps.source_id=s.source_id)

GO
